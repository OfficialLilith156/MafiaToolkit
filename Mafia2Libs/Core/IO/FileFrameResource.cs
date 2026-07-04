using Mafia2Tool;
using System;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileFrameResource : FileBase
    {
        public SceneData SceneData = new SceneData();
        private bool bForceBigEndian;

        public FileFrameResource(FileInfo info) : base(info)
        {
            SceneData.ScenePath = info.DirectoryName;
            bForceBigEndian = false;
        }

        public override bool Open()
        {
            MaterialData.Load();

            if (!TryDetectEndianness(file.FullName, out bool isBigEndian))
            {
                MessageBox.Show("Could not determine the byte order. The file may be corrupted..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SceneData = new SceneData();
            SceneData.ScenePath = file.DirectoryName;
            SceneData.BuildData(isBigEndian);

            MapEditor d3dForm = new MapEditor(file, SceneData);
            d3dForm.Dispose();
            return true;
        }

        private static bool TryDetectEndianness(string filePath, out bool isBigEndian)
        {
            isBigEndian = false;
            byte[] buffer = new byte[4 * 7 + 4];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, buffer.Length);
            }

            int ReadInt32LE(byte[] buf, int offset) => BitConverter.ToInt32(buf, offset);
            int ReadInt32BE(byte[] buf, int offset)
            {
                int value = BitConverter.ToInt32(buf, offset);
                if (BitConverter.IsLittleEndian)
                    return System.Net.IPAddress.NetworkToHostOrder(value);
                return value;
            }

            int numFolderNames_LE = ReadInt32LE(buffer, 1); 
            int numGeometries_LE = ReadInt32LE(buffer, 5);
            int numMaterialResources_LE = ReadInt32LE(buffer, 9);
            int numBlendInfos_LE = ReadInt32LE(buffer, 13);
            int numSkeletons_LE = ReadInt32LE(buffer, 17);
            int numSkelHierachies_LE = ReadInt32LE(buffer, 21);
            int numObjects_LE = ReadInt32LE(buffer, 25);

            int numFolderNames_BE = ReadInt32BE(buffer, 1);
            int numGeometries_BE = ReadInt32BE(buffer, 5);
            int numMaterialResources_BE = ReadInt32BE(buffer, 9);
            int numBlendInfos_BE = ReadInt32BE(buffer, 13);
            int numSkeletons_BE = ReadInt32BE(buffer, 17);
            int numSkelHierachies_BE = ReadInt32BE(buffer, 21);
            int numObjects_BE = ReadInt32BE(buffer, 25);

            bool IsReasonable(int value) => value >= 0 && value < 100000;

            bool leOk = IsReasonable(numFolderNames_LE) && IsReasonable(numGeometries_LE) &&
                        IsReasonable(numMaterialResources_LE) && IsReasonable(numBlendInfos_LE) &&
                        IsReasonable(numSkeletons_LE) && IsReasonable(numSkelHierachies_LE) &&
                        IsReasonable(numObjects_LE);

            bool beOk = IsReasonable(numFolderNames_BE) && IsReasonable(numGeometries_BE) &&
                        IsReasonable(numMaterialResources_BE) && IsReasonable(numBlendInfos_BE) &&
                        IsReasonable(numSkeletons_BE) && IsReasonable(numSkelHierachies_BE) &&
                        IsReasonable(numObjects_BE);

            if (leOk && !beOk)
            {
                isBigEndian = false;
                return true;
            }
            if (beOk && !leOk)
            {
                isBigEndian = true;
                return true;
            }

            isBigEndian = false;
            return leOk || beOk;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "FR";
        }

        public void SetBigEndian(bool bResult)
        {
            bForceBigEndian = bResult;
        }
    }
}
