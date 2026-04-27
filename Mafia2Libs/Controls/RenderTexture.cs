using Rendering.Core;
using System;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class RenderTexture : UserControl
    {
        private string settingsFile = Path.Combine(Application.StartupPath, "lastfolder.txt");

        public RenderTexture()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            TexDirectoryBox5.Text = ToolkitSettings.TexturePath5;
            TexDirectoryBox1.Text = ToolkitSettings.TexturePath1;
            TexDirectoryBox2.Text = ToolkitSettings.TexturePath2;
            TexDirectoryBox3.Text = ToolkitSettings.TexturePath3;
            TexDirectoryBox4.Text = ToolkitSettings.TexturePath4;
        }
       
        private void TexDirectoryBox5_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath5 = TexDirectoryBox5.Text;
            ToolkitSettings.WriteKey("TexturePath5", "ModelViewer", ToolkitSettings.TexturePath5);
        }

        private void TexDirectoryBox1_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath1 = TexDirectoryBox1.Text;
            ToolkitSettings.WriteKey("TexturePath1", "ModelViewer", ToolkitSettings.TexturePath1);
        }

        private void TexDirectoryBox2_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath2 = TexDirectoryBox2.Text;
            ToolkitSettings.WriteKey("TexturePath2", "ModelViewer", ToolkitSettings.TexturePath2);
        }

        private void TexDirectoryBox3_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath3 = TexDirectoryBox3.Text;
            ToolkitSettings.WriteKey("TexturePath3", "ModelViewer", ToolkitSettings.TexturePath3);
        }

        private void TexDirectoryBox4_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath4 = TexDirectoryBox4.Text;
            ToolkitSettings.WriteKey("TexturePath4", "ModelViewer", ToolkitSettings.TexturePath4);
        }

        private void BrowseButton1_Click(object sender, EventArgs e)
        {
            TexBrowser.SelectedPath = "";
            if (TexBrowser.ShowDialog() == DialogResult.OK)
            {
                TexDirectoryBox1.Text = TexBrowser.SelectedPath;
                TexDirectoryBox1_TextChanged(null, null);
            }
            else return;
        }

        private void BrowseButton2_Click(object sender, EventArgs e)
        {
            TexBrowser.SelectedPath = "";
            if (TexBrowser.ShowDialog() == DialogResult.OK)
            {
                TexDirectoryBox2.Text = TexBrowser.SelectedPath;
                TexDirectoryBox2_TextChanged(null, null);
            }
            else return;
        }

        private void BrowseButton3_Click(object sender, EventArgs e)
        {
            TexBrowser.SelectedPath = "";
            if (TexBrowser.ShowDialog() == DialogResult.OK)
            {
                TexDirectoryBox3.Text = TexBrowser.SelectedPath;
                TexDirectoryBox3_TextChanged(null, null);
            }
            else return;
        }

        private void BrowseButton4_Click(object sender, EventArgs e)
        {
            TexBrowser.SelectedPath = "";
            if (TexBrowser.ShowDialog() == DialogResult.OK)
            {
                TexDirectoryBox4.Text = TexBrowser.SelectedPath;
                TexDirectoryBox4_TextChanged(null, null);
            }
            else return;
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
                    catch (Exception)
                    { }
                    TexDirectoryBox5.Text = selectedFolder;
                    TexDirectoryBox5_TextChanged(null, null);
                }
            }
        }
    }
}
