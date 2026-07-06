using System;
using System.IO;
using ResourceTypes.Collisions;

namespace Core.IO
{
    public class FileCollision : FileBase
    {
        public FileCollision(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "COL";
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
                    string.Format("'{0}' is already a PC (Little Endian) collision.", GetName()));
            }

            // The reader auto-detects the byte order from the 'Platform' field and
            // the PhysX mesh chunk self-describes its endianness, so everything is
            // decoded into a byte-order independent model on load.
            Collision collision = new Collision(file.FullName);

            // Mark the output as PC (0). WriteToFile emits Little Endian.
            collision.Platform = 0;

            string outputPath = GetConvertedPCPath();
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
            {
                collision.WriteToFile(writer);
            }

            return outputPath;
        }

        // Layout: [int32 version][uint32 platform]... The version is always stored
        // Little Endian; the platform field is >2 (byte-swapped) on Xbox/PS3.
        public static bool TryDetectEndianness(string filePath, out bool isBigEndian)
        {
            isBigEndian = false;

            byte[] header = new byte[8];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Read(header, 0, 8) < 8) return false;
            }

            uint platformRaw = BitConverter.ToUInt32(header, 4);
            // 0 = PC, 1 = Xbox360, 2 = PS3 (values > 2 mean the field is byte-swapped).
            isBigEndian = platformRaw > 2;
            return true;
        }
    }
}
