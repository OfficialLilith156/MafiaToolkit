using Mafia2Tool;
using System;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileFrameResource : FileBase
    {
        public bool IsBigEndian { get; private set; }
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
                MessageBox.Show("Could not determine the byte order. The file may be corrupted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            IsBigEndian = isBigEndian;
            SceneData = new SceneData();
            SceneData.ScenePath = file.DirectoryName;
            SceneData.BuildData(isBigEndian);

            MapEditor d3dForm = new MapEditor(file, SceneData, isBigEndian);
            d3dForm.Dispose();
            return true;
        }

        public static bool TryDetectEndianness(string filePath, out bool isBigEndian)
        {
            isBigEndian = false;
            byte[] buffer = new byte[1 + 7 * 4];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int read = fs.Read(buffer, 0, buffer.Length);
                if (read < buffer.Length) return false;
            }

            int numFolderNames_LE = BitConverter.ToInt32(buffer, 1);
            int numGeometries_LE = BitConverter.ToInt32(buffer, 5);
            int numMaterialResources_LE = BitConverter.ToInt32(buffer, 9);
            int numBlendInfos_LE = BitConverter.ToInt32(buffer, 13);
            int numSkeletons_LE = BitConverter.ToInt32(buffer, 17);
            int numSkelHierachies_LE = BitConverter.ToInt32(buffer, 21);
            int numObjects_LE = BitConverter.ToInt32(buffer, 25);

            int numFolderNames_BE = BitConverter.ToInt32(buffer, 1);
            int numGeometries_BE = BitConverter.ToInt32(buffer, 5);
            int numMaterialResources_BE = BitConverter.ToInt32(buffer, 9);
            int numBlendInfos_BE = BitConverter.ToInt32(buffer, 13);
            int numSkeletons_BE = BitConverter.ToInt32(buffer, 17);
            int numSkelHierachies_BE = BitConverter.ToInt32(buffer, 21);
            int numObjects_BE = BitConverter.ToInt32(buffer, 25);

            if (BitConverter.IsLittleEndian)
            {
                numFolderNames_BE = System.Net.IPAddress.NetworkToHostOrder(numFolderNames_BE);
                numGeometries_BE = System.Net.IPAddress.NetworkToHostOrder(numGeometries_BE);
                numMaterialResources_BE = System.Net.IPAddress.NetworkToHostOrder(numMaterialResources_BE);
                numBlendInfos_BE = System.Net.IPAddress.NetworkToHostOrder(numBlendInfos_BE);
                numSkeletons_BE = System.Net.IPAddress.NetworkToHostOrder(numSkeletons_BE);
                numSkelHierachies_BE = System.Net.IPAddress.NetworkToHostOrder(numSkelHierachies_BE);
                numObjects_BE = System.Net.IPAddress.NetworkToHostOrder(numObjects_BE);
            }

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
