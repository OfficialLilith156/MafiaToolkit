using Mafia2Tool;
using ResourceTypes.Materials;
using System;
using System.IO;

namespace Core.IO
{
    public class FileMaterialLibrary : FileBase
    {
        public FileMaterialLibrary(FileInfo info) : base(info)
        {
        }

        public override bool Open()
        {
            MaterialEditor editor = new MaterialEditor(file);
            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "MTL";
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
                    string.Format("Could not determine the byte order of '{0}' (unknown MTL version).", GetName()));
            }

            if (!isBigEndian)
            {
                throw new InvalidOperationException(
                    string.Format("'{0}' is already a PC (Little Endian) material library.", GetName()));
            }

            // ReadMatFile auto-detects the byte order and decodes materials into a
            // byte-order independent model; WriteMatFile always emits Little Endian.
            MaterialLibrary library = new MaterialLibrary(VersionsEnumerator.V_57);
            library.ReadMatFile(file.FullName);

            string outputPath = GetConvertedPCPath();
            library.WriteMatFile(outputPath);

            return outputPath;
        }

        // Layout: [4 bytes "MTLB"][int32 version]. Version is one of 57/58/63,
        // which tells the byte order apart.
        public static bool TryDetectEndianness(string filePath, out bool isBigEndian)
        {
            isBigEndian = false;

            byte[] header = new byte[8];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Read(header, 0, 8) < 8) return false;
            }

            int versionLE = BitConverter.ToInt32(header, 4);
            byte[] be = new byte[4];
            Array.Copy(header, 4, be, 0, 4);
            Array.Reverse(be);
            int versionBE = BitConverter.ToInt32(be, 0);

            bool IsKnownVersion(int value) => value == 57 || value == 58 || value == 63;

            if (IsKnownVersion(versionLE)) { isBigEndian = false; return true; }
            if (IsKnownVersion(versionBE)) { isBigEndian = true; return true; }

            return false;
        }
    }
}
