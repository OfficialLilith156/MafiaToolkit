using System;
using System.Collections.Generic;
using System.IO;
using Mafia2Tool;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using Utils.Models;

namespace Core.IO
{
    public class FileVertexBufferPool : FileBase
    {
        public FileVertexBufferPool(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "VBP";
        }

        public override bool CanConvertXboxToPC()
        {
            return true;
        }

        public override string ConvertXboxToPC()
        {
            // The vertex buffer only stores raw vertex bytes; the per-component
            // layout lives in the matching FrameResource (.fr). We need it to know
            // how to byte-swap each component, so the pool cannot be converted on
            // its own.
            if (!FileIndexBufferPool.TryDetectEndianness(file.FullName, out bool isBigEndian))
            {
                throw new InvalidOperationException(
                    string.Format("Could not determine the byte order of '{0}'.", GetName()));
            }

            if (!isBigEndian)
            {
                throw new InvalidOperationException(
                    string.Format("'{0}' is already a PC (Little Endian) vertex buffer pool.", GetName()));
            }

            // Build a hash -> LOD map from every FrameResource in the same folder,
            // so we know each vertex buffer's declaration (component layout).
            Dictionary<ulong, FrameLOD> layoutByHash = BuildLayoutMap();
            if (layoutByHash.Count == 0)
            {
                throw new InvalidOperationException(string.Format(
                    "Could not find a FrameResource (.fr) next to '{0}'. It is required to convert vertex buffers.",
                    GetName()));
            }

            VertexBufferPool pool;
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file.FullName), false))
            {
                pool = new VertexBufferPool(stream, true);
            }

            foreach (VertexBuffer buffer in pool.Buffers.Values)
            {
                if (!layoutByHash.TryGetValue(buffer.Hash, out FrameLOD lod))
                {
                    throw new InvalidOperationException(string.Format(
                        "Vertex buffer {0:X16} is not referenced by any FrameResource in this folder, so its layout is unknown.",
                        buffer.Hash));
                }

                SwapBufferToLittleEndian(buffer, lod);
            }

            string outputPath = GetConvertedPCPath();
            using (MemoryStream stream = new MemoryStream())
            {
                pool.WriteToFile(stream, false);
                File.WriteAllBytes(outputPath, stream.ToArray());
            }

            return outputPath;
        }

        private Dictionary<ulong, FrameLOD> BuildLayoutMap()
        {
            MaterialData.Load();

            Dictionary<ulong, FrameLOD> map = new Dictionary<ulong, FrameLOD>();

            foreach (FileInfo frInfo in file.Directory.GetFiles("*.fr"))
            {
                if (!FileFrameResource.TryDetectEndianness(frInfo.FullName, out bool frBig))
                {
                    continue;
                }

                SceneData sceneData = new SceneData();
                sceneData.ScenePath = frInfo.DirectoryName;

                FrameResource frame;
                try
                {
                    frame = new FrameResource(frInfo.FullName, sceneData, frBig);
                }
                catch
                {
                    continue;
                }

                foreach (FrameGeometry geometry in frame.FrameGeometries.Values)
                {
                    if (geometry.LOD == null) continue;
                    foreach (FrameLOD lod in geometry.LOD)
                    {
                        if (lod?.VertexBufferRef == null) continue;
                        ulong hash = lod.VertexBufferRef.Hash;
                        if (!map.ContainsKey(hash))
                        {
                            map.Add(hash, lod);
                        }
                    }
                }
            }

            return map;
        }

        // Byte-swaps every component of every vertex in the buffer, in place,
        // according to the LOD's vertex declaration. Byte-array components (normals,
        // colours, skin weights/bones, packed tangent bytes) keep their order;
        // multi-byte numeric components (half floats, floats, ints) are reversed.
        private static void SwapBufferToLittleEndian(VertexBuffer buffer, FrameLOD lod)
        {
            Dictionary<VertexFlags, FrameLOD.VertexOffset> offsets = lod.GetVertexOffsets(out int stride);
            if (stride <= 0)
            {
                throw new InvalidOperationException(
                    string.Format("Vertex buffer {0:X16} has an invalid stride ({1}).", buffer.Hash, stride));
            }

            byte[] data = buffer.Data;
            if (data.Length % stride != 0)
            {
                throw new InvalidOperationException(string.Format(
                    "Vertex buffer {0:X16} size ({1}) is not a multiple of its stride ({2}); the FrameResource layout does not match.",
                    buffer.Hash, data.Length, stride));
            }

            int numVerts = data.Length / stride;
            for (int v = 0; v < numVerts; v++)
            {
                int baseOffset = v * stride;
                foreach (KeyValuePair<VertexFlags, FrameLOD.VertexOffset> entry in offsets)
                {
                    int off = baseOffset + entry.Value.Offset;
                    switch (entry.Key)
                    {
                        case VertexFlags.Position:
                            // 3 half floats (X,Y,Z); bytes 6-7 are packed tangent bytes.
                            Reverse(data, off + 0, 2);
                            Reverse(data, off + 2, 2);
                            Reverse(data, off + 4, 2);
                            break;
                        case VertexFlags.TexCoords0:
                        case VertexFlags.TexCoords1:
                        case VertexFlags.TexCoords2:
                        case VertexFlags.ShadowTexture:
                            // 2 half floats (U,V).
                            Reverse(data, off + 0, 2);
                            Reverse(data, off + 2, 2);
                            break;
                        case VertexFlags.BBCoeffs:
                            // 3 floats.
                            Reverse(data, off + 0, 4);
                            Reverse(data, off + 4, 4);
                            Reverse(data, off + 8, 4);
                            break;
                        case VertexFlags.DamageGroup:
                            // int32.
                            Reverse(data, off + 0, 4);
                            break;
                        case VertexFlags.Normals:
                        case VertexFlags.Color:
                        case VertexFlags.Color1:
                        case VertexFlags.Skin:
                        case VertexFlags.Tangent:
                            // Stored as individual bytes; nothing to swap.
                            break;
                        default:
                            throw new InvalidOperationException(string.Format(
                                "Unsupported vertex component '{0}' in buffer {1:X16}; cannot safely convert.",
                                entry.Key, buffer.Hash));
                    }
                }
            }
        }

        private static void Reverse(byte[] data, int start, int count)
        {
            Array.Reverse(data, start, count);
        }
    }
}
