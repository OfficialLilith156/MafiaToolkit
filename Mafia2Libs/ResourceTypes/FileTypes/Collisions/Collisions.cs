using Gibbed.IO;
using ResourceTypes.Collisions;
using ResourceTypes.Collisions.PhysX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Helpers;
using Utils.Logging;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Collisions
{
    public class Collision
    {
        private const int Version = 0x11; // 17

        public string Name { get; set; }
        /// <summary>
        /// Platform (== 0 on PC/Mac, == 1 on XBox360, == 2 on PS3)
        /// </summary>
        /// <remarks>Could be <c>NxPlatform</c> type, enum values are match</remarks>
        public uint Platform { get; set; } = 0;
        public List<Placement> Placements { get; private set; }= new List<Placement>();
        public SortedDictionary<ulong, CollisionModel> Models { get; private set; } = new SortedDictionary<ulong, CollisionModel>();


        public Collision()
        {
        }

        public Collision(string fileName)
        {
            this.Name = fileName;
            using (BinaryReader reader = new BinaryReader(File.Open(this.Name, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int version = reader.ReadInt32();
            if (version != Version) throw new Exception("Unknown collision version");

            uint platformRaw = reader.ReadUInt32();
            uint platform = platformRaw;
            bool isBigEndian = false;
            if (platformRaw > 2)
            {
                byte[] platformBytes = BitConverter.GetBytes(platformRaw);
                Array.Reverse(platformBytes);
                platform = BitConverter.ToUInt32(platformBytes, 0);
                isBigEndian = true;
            }
            Platform = platform;
            if (Platform > 2) throw new Exception($"Unknown platform {Platform}");

            int numPlacements = (int)ReadUInt32(reader, isBigEndian);
            Placements = new List<Placement>(numPlacements);
            for (int i = 0; i < numPlacements; i++)
                Placements.Add(new Placement(reader, isBigEndian));

            int numModels = (int)ReadUInt32(reader, isBigEndian);
            Models = new SortedDictionary<ulong, CollisionModel>();
            for (int i = 0; i < numModels; i++)
            {
                CollisionModel model = new CollisionModel(reader, isBigEndian);
                Models.Add(model.Hash, model);
            }
        }
        private static uint ReadUInt32(BinaryReader reader, bool bigEndian)
        {
            uint val = reader.ReadUInt32();
            if (bigEndian) val = val.Swap();
            return val;
        }

        public void WriteToFile()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new Exception("Name is null or empty");
            }

            //force cook collisions
            if (Utils.Settings.ToolkitSettings.CookCollisions)
            {
                using(BinaryWriter writer = new BinaryWriter(File.Open("MeshBundle.bin", FileMode.Create)))
                {
                    writer.Write(Models.Count);

                    TriangleCooking cooker = new TriangleCooking();

                    foreach (var collisionModel in Models)
                    {
                        cooker.WriteRawFormatToFile(writer, collisionModel.Value.Mesh);
                    }
                }

                PhysXHelper.MultiCookTriangleCollision("MeshBundle.bin", "CookedBundle.bin");

                using(BinaryReader reader = new BinaryReader(File.Open("CookedBundle.bin", FileMode.Open)))
                {
                    uint NumModels = reader.ReadUInt32();
                    for(int i = 0; i < NumModels; i++)
                    {
                        TriangleMesh CookedMesh = new TriangleMesh();
                        CookedMesh.Load(reader);
                        CookedMesh.Force32BitIndices();
                        Models.ElementAt(i).Value.Mesh = CookedMesh;
                    }
                }

                if (File.Exists("MeshBundle.bin"))
                {
                    File.Delete("MeshBundle.bin");
                }

                if (File.Exists("CookedBundle.bin"))
                {
                    File.Delete("CookedBundle.bin");
                }
            }


            using (BinaryWriter writer = new BinaryWriter(File.Open(Name, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Platform);

            writer.Write(Placements.Count);
            foreach (var placement in Placements)
            {
                placement.WriteToFile(writer);
            }

            writer.Write(Models.Count);

            // NOTE: Models should be sorted by hash (ascending)
            // that's why SortedDictionary is used
            foreach (var collisionModel in Models)
            {
                collisionModel.Value.WriteToFile(writer);
            }
        }

        public void WriteToFile(string name)
        {
            this.Name = name;
            WriteToFile();
        }

        /** Util to remove model from Collisions file. */
        public void RemoveModel(ulong Hash)
        {
            if(Models.ContainsKey(Hash))
            {
                Models.Remove(Hash);
            }

            RemoveAllPlacementsForModels(Hash);
        }

        /** Util to remove model from Collisions file */
        public void RemoveModel(CollisionModel Model)
        {
            RemoveModel(Model.Hash);
        }

        /** Remove Placements for given Collision Object. */
        private void RemoveAllPlacementsForModels(ulong CollisionHash)
        {
            // Check if the placement has the collision hash, 
            // If so, delete from list.
            for (int i = Placements.Count - 1; i >= 0; i--)
            {
                Placement Instance = Placements[i];
                if(Instance.Hash == CollisionHash)
                {
                    Placements.RemoveAt(i);
                }
            }
        }

        public class Placement
        {
            public Vector3 Position { get; set; }

            public Vector3 Rotation { get; set; }

            public ulong Hash { get; set; }
            public int IndexModel { get; set; }
            public byte Unk5 { get; set; }

            /// <summary>
            /// Helper property to get/set rotation in degrees (with Z axes adopted to the Toolkit render coordinate system)
            /// instead of original rotation which is stored in radians
            /// </summary>
            public Vector3 RotationDegrees
            {
                get
                {
                    Vector3 vec = new Vector3();
                    vec.X = MathHelper.ToDegrees(Rotation.X);
                    vec.Y = MathHelper.ToDegrees(Rotation.Y);
                    //vec.X = ((vec.X + vec.Y) * -1);
                    //vec.X = ((vec.X + vec.Y) * -1);
                    vec.Y = ((vec.X + vec.Y) * -1);
                    vec.X = 0.0f;
                    vec.Z = /*Unk5 != 128 ? MathUtil.RadiansToDegrees(Rotation.Z) : */-MathHelper.ToDegrees(Rotation.Z);
                    return vec;
                }
                set
                {
                    Vector3 vec = new Vector3();
                    vec.X = MathHelper.ToRadians(value.X);
                    vec.Y = MathHelper.ToRadians(value.Y);
                    //vec.X = ((vec.X + vec.Y) * -1);
                    //vec.Y = ((vec.X + vec.Y) * -1);
                    vec.Z = /*Unk5 != 128 ? MathUtil.DegreesToRadians(value.Z) : */-MathHelper.ToRadians(value.Z);
                    Rotation = vec;
                }
            }

            /// <summary>
            /// Helper property to easily build the transform for the Toolkit render system.
            /// </summary>
            public Matrix4x4 Transform
            {
                get
                {

                    Matrix4x4 transform = MatrixUtils.SetMatrix(RotationDegrees, Vector3.One, Position);
                    ToolkitAssert.Ensure(!transform.IsNaN(), "Transform is NaN");
                    transform.Translation = Position;
                    return transform;
                }
            }


            public Placement(BinaryReader reader, bool bigEndian)
            {
                ReadFromFile(reader, bigEndian);
            }

            public Placement()
            {
                Unk5 = 128;
                IndexModel = -1;
                Position = new Vector3(0, 0, 0);
                Rotation = new Vector3(0);
            }

            public Placement(Placement other)
            {
                Position = other.Position;
                Rotation = other.Rotation;
                Hash = other.Hash;
                IndexModel = other.IndexModel;
                Unk5 = other.Unk5;
            }

            public void ReadFromFile(BinaryReader reader, bool bigEndian)
            {
                Position = new Vector3(
                    ReadFloat(reader, bigEndian),
                    ReadFloat(reader, bigEndian),
                    ReadFloat(reader, bigEndian)
                );
                Rotation = new Vector3(
                    ReadFloat(reader, bigEndian),
                    ReadFloat(reader, bigEndian),
                    ReadFloat(reader, bigEndian)
                );
                Hash = ReadUInt64(reader, bigEndian);
                IndexModel = (int)ReadUInt32(reader, bigEndian);
                Unk5 = reader.ReadByte();
            }
            private static float ReadFloat(BinaryReader reader, bool bigEndian)
            {
                return SerializationUtils.ReadFloat(reader, bigEndian);
            }
            private static ulong ReadUInt64(BinaryReader reader, bool bigEndian)
            {
                ulong val = reader.ReadUInt64();
                if (bigEndian) val = val.Swap();
                return val;
            }


            public void WriteToFile(BinaryWriter writer)
            {
                Position.WriteToFile(writer);
                Rotation.WriteToFile(writer);
                writer.Write(Hash);
                writer.Write(IndexModel);
                writer.Write(Unk5);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", Hash, IndexModel, Unk5);
            }
        }

        public class CollisionModel
        {
            [Category("General")]
            [Description("Hash of the collision model.")]
            public ulong Hash { get; set; }

            [Browsable(false)]
            public TriangleMesh Mesh { get; set; }

            [Category("Collision Data")]
            [Description("Material sections of this collision model. Click '...' to edit.")]
            public List<Section> Sections { get; set; }

            public CollisionModel(BinaryReader reader, bool bigEndian)
            {
                ReadFromFile(reader, bigEndian);
            }

            public CollisionModel()
            {
                Hash = 0;
                Mesh = new TriangleMesh();
                Sections = new List<Section>();
            }

            public void ReadFromFile(BinaryReader reader, bool bigEndian)
            {
                Hash = ReadUInt64(reader, bigEndian);
                int dataSize = (int)ReadUInt32(reader, bigEndian);
                Mesh = new TriangleMesh();
                Mesh.Load(reader);
                int numSections = (int)ReadUInt32(reader, bigEndian);
                Sections = new List<Section>();
                for (int i = 0; i < numSections; i++)
                {
                    Section sec = new Section(reader, bigEndian);
                    sec.ParentModel = this;
                    Sections.Add(sec);
                }
            }
            private static ulong ReadUInt64(BinaryReader reader, bool bigEndian)
            {
                ulong val = reader.ReadUInt64();
                if (bigEndian) val = val.Swap();
                return val;
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(Hash);

                writer.Write(Mesh.GetUsedBytes());
                Mesh.Save(writer);

                writer.Write(Sections.Count);
                foreach (var section in Sections)
                {
                    section.WriteToFile(writer);
                }
            }
        }

        public class Section
        {
            public int Start { get; set; }
            public int NumEdges { get; set; }

            [Browsable(false)]
            public int Material { get; set; }

            public int Unk2 { get; set; }

            [Browsable(false)]
            public CollisionModel ParentModel { get; set; }

            [Category("Material")]
            [Description("The physics material type for this collision section.")]
            [DisplayName("Material Type")]
            public CollisionMaterials MaterialType
            {
                get
                {
                    int enumValue = Material + 2;
                    if (Enum.IsDefined(typeof(CollisionMaterials), enumValue))
                    {
                        return (CollisionMaterials)enumValue;
                    }
                    return CollisionMaterials.Undefined;
                }
                set
                {
                    Material = (int)value - 2;
                    ApplyMaterialToThisSection();
                }
            }

            private void ApplyMaterialToThisSection()
            {
                if (ParentModel?.Mesh?.MaterialIndices == null) return;

                ushort materialIndex = (ushort)(Material + 2);
                int startTriangle = Start / 3;
                int numTriangles = NumEdges / 3;

                int maxTriangles = ParentModel.Mesh.MaterialIndices.Count;
                if (startTriangle >= maxTriangles) return;
                int trianglesToProcess = Math.Min(numTriangles, maxTriangles - startTriangle);

                for (int i = 0; i < trianglesToProcess; i++)
                {
                    ParentModel.Mesh.MaterialIndices[startTriangle + i] = materialIndex;
                }
            }

            public Section()
            {
            }

            public Section(BinaryReader reader, bool bigEndian)
            {
                Start = (int)ReadUInt32(reader, bigEndian);
                NumEdges = (int)ReadUInt32(reader, bigEndian);
                Material = (int)ReadUInt32(reader, bigEndian);
                Unk2 = (int)ReadUInt32(reader, bigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(Start);
                writer.Write(NumEdges);
                writer.Write(Material);
                writer.Write(Unk2);
            }
        }
    }
}