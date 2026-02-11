using Mafia2Tool.Forms;
using Microsoft.Win32;
using ResourceTypes.SoundTable;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileSoundTable : FileBase
    {
        public FileSoundTable(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "STBL";
        }

        public override bool Open()
        {
            using (var editorForm = new SoundTableEditor(file.FullName))
            {
                if (editorForm.ShowDialog() == DialogResult.OK)
                {
                }
            }

            return true;
        }

        public override void Save()
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName),
                Filter = "XML files (*.xml)|*.xml"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SoundTable Table = new SoundTable();
                    Table.ConvertFromXML(openFile.FileName);
                    if (File.Exists(file.FullName))
                    {
                        string backupPath = file.FullName + ".backup";
                        File.Copy(file.FullName, backupPath, true);
                    }

                    Table.WriteToFile(file.FullName, false);

                    MessageBox.Show($"Successfully converted {Path.GetFileName(openFile.FileName)} to {Path.GetFileName(file.FullName)}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Error converting file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Convert To (.xml)";
        }

        public override bool CanContextMenuSave()
        {
            return true;
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Convert From (.xml)";
        }
    }
}
