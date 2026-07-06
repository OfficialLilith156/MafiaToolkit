using System;
using System.IO;

namespace Core.IO
{
    public class FileFrameNameTable : FileBase
    {
        public FileFrameNameTable(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "FNT";
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
                    string.Format("'{0}' is already a PC (Little Endian) frame name table.", GetName()));
            }

            // Read as Big Endian (Xbox); the model is byte-order independent once loaded.
            ResourceTypes.FrameNameTable.FrameNameTable table =
                new ResourceTypes.FrameNameTable.FrameNameTable(file.FullName, true);

            string outputPath = GetConvertedPCPath();

            // WriteToFile always emits Little Endian (plain BinaryWriter).
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
            {
                table.WriteToFile(writer);
            }

            return outputPath;
        }

        // The FNT layout is [int32 bufferSize][buffer][int32 dataSize][data...].
        // bufferSize must fit within the file, which lets us tell the byte order
        // apart from the very first field.
        public static bool TryDetectEndianness(string filePath, out bool isBigEndian)
        {
            isBigEndian = false;

            byte[] header = new byte[4];
            long length;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                length = fs.Length;
                if (fs.Read(header, 0, 4) < 4) return false;
            }

            int bufferSizeLE = BitConverter.ToInt32(header, 0);
            Array.Reverse(header);
            int bufferSizeBE = BitConverter.ToInt32(header, 0);

            bool IsReasonable(int value) => value >= 0 && value <= length - 4;

            bool leOk = IsReasonable(bufferSizeLE);
            bool beOk = IsReasonable(bufferSizeBE);

            if (leOk && !beOk) { isBigEndian = false; return true; }
            if (beOk && !leOk) { isBigEndian = true; return true; }

            // Ambiguous (or both invalid): default to Little Endian if plausible.
            isBigEndian = false;
            return leOk || beOk;
        }
    }
}
