using Mafia2Tool.Forms;
using ResourceTypes.ItemDesc;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileIDS : FileBase
    {
        public FileIDS(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "IDS";
        }

        public override bool Open()
        {
            ItemDescLoader itemDesc = new ItemDescLoader(file.FullName);

            using (var editor = new ItemDescEditor(itemDesc, file.FullName))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = file.FullName + "_old";
                    File.Copy(file.FullName, backupPath, true);
                    using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
                    {
                        editor.ItemDesc.WriteToFile(writer);
                    }
                }
            }
            return true;
        }

        public override void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName) + ".xml",
                Filter = "XML (*.xml)|*.xml"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ItemDescLoader itemDesc = new ItemDescLoader(file.FullName);
                MessageBox.Show("Export to XML not yet implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Open in ItemDesc Editor";
        }

        public override bool CanContextMenuSave()
        {
            return true;
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Export to XML";
        }
    }
}