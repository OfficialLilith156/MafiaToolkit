using System;
using System.IO;
using ResourceTypes.BufferPools;

namespace Core.IO
{
    public class FileIndexBufferPool : FileBase
    {
        public FileIndexBufferPool(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "IBP";
        }

        public override bool CanConvertXboxToPC()
        {
            return true;
        }

        public override string ConvertXboxToPC()
        {
            if (!TryDetectEndianness(file.FullName, out bool isBigEndian))
            {
                throw new InvalidOperationException(
                    string.Format("Could not determine the byte order of '{0}'.", GetName()));
            }

            if (!isBigEndian)
            {
                throw new InvalidOperationException(
                    string.Format("'{0}' is already a PC (Little Endian) index buffer pool.", GetName()));
            }

            // Index buffers store their indices element-wise, so a plain endian
            // read/write fully converts the data.
            IndexBufferPool pool;
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file.FullName), false))
            {
                pool = new IndexBufferPool(stream, true);
            }

            string outputPath = GetConvertedPCPath();
            using (MemoryStream stream = new MemoryStream())
            {
                pool.WriteToFile(stream, false);
                File.WriteAllBytes(outputPath, stream.ToArray());
            }

            return outputPath;
        }

        // Layout: [byte version][int32 numBuffers][uint32 size]...
        // numBuffers is small, which lets us tell the byte order apart.
        public static bool TryDetectEndianness(string filePath, out bool isBigEndian)
        {
            isBigEndian = false;

            byte[] header = new byte[5];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Read(header, 0, 5) < 5) return false;
            }

            int numLE = BitConverter.ToInt32(header, 1);
            byte[] be = new byte[4];
            Array.Copy(header, 1, be, 0, 4);
            Array.Reverse(be);
            int numBE = BitConverter.ToInt32(be, 0);

            bool IsReasonable(int value) => value >= 0 && value <= 100000;

            bool leOk = IsReasonable(numLE);
            bool beOk = IsReasonable(numBE);

            if (leOk && !beOk) { isBigEndian = false; return true; }
            if (beOk && !leOk) { isBigEndian = true; return true; }

            isBigEndian = false;
            return leOk || beOk;
        }
    }
}
