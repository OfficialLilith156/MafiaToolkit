using Rendering.Core;
using System;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class RenderSky : UserControl
    {
        private string settingsFile = Path.Combine(Application.StartupPath, "lastfolder.txt");
        private WorldSettings worldSettings;

        public RenderSky()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            TexDirectoryBox5.Text = ToolkitSettings.TexturePath5;
        }

        private void TexDirectoryBox5_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath5 = TexDirectoryBox5.Text;
            ToolkitSettings.WriteKey("TexturePath5", "ModelViewer", ToolkitSettings.TexturePath5);
        }

        private void BrowseButton5_Click(object sender, EventArgs e)
        {
            string selectedFolder = "";
            if (File.Exists(settingsFile))
            {
                selectedFolder = File.ReadAllText(settingsFile);
                if (!Directory.Exists(selectedFolder))
                {
                    selectedFolder = "";
                }
            }
            if (string.IsNullOrEmpty(selectedFolder))
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedFolder = folderDialog.SelectedPath;

                        if (!Directory.Exists(selectedFolder)) Directory.CreateDirectory(selectedFolder);

                        File.WriteAllText(settingsFile, selectedFolder);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "DDS Files (*.dds)|*.dds";
                fileDialog.Title = "Выберите текстуру (.dds)";
                fileDialog.Multiselect = false;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFile = fileDialog.FileName;
                    string destFile = Path.Combine(selectedFolder, "02_home_part1.dds");
                    try
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                    catch (Exception ex)
                    { }
                    TexDirectoryBox5.Text = selectedFolder;
                    TexDirectoryBox5_TextChanged(null, null);
                }
            }
        }
    }
}
