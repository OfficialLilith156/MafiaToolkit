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
            chkLoadFrameResource.Checked = ToolkitSettings.LoadFrameResource;
            chkLoadCollisions.Checked = ToolkitSettings.LoadCollisions;
            chkLoadActors.Checked = ToolkitSettings.LoadActors;
            chkLoadTranslokator.Checked = ToolkitSettings.LoadTranslokator;
            chkLoadAIWorld.Checked = ToolkitSettings.LoadAIWorld;
            chkLoadOBJData.Checked = ToolkitSettings.LoadOBJData;
            chkLoadRoads.Checked = ToolkitSettings.LoadRoads;
            chkLoadATP.Checked = ToolkitSettings.LoadATP;
            chkLoadPrefabs.Checked = ToolkitSettings.LoadPrefabs;
            chkLoadItemDescs.Checked = ToolkitSettings.LoadItemDescs;
            chkLoadHPD.Checked = ToolkitSettings.LoadHPD;
            chkLoadSoundSectors.Checked = ToolkitSettings.LoadSoundSectors;
        }
        private void chkLoadFrameResource_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadFrameResource = chkLoadFrameResource.Checked;
            ToolkitSettings.WriteKey("LoadFrameResource", "LoadOptions", ToolkitSettings.LoadFrameResource.ToString());
        }
        private void chkLoadCollisions_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadCollisions = chkLoadCollisions.Checked;
            ToolkitSettings.WriteKey("LoadCollisions", "LoadOptions", ToolkitSettings.LoadCollisions.ToString());
        }


        private void chkLoadActors_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadActors = chkLoadActors.Checked;
            ToolkitSettings.WriteKey("LoadActors", "LoadOptions", ToolkitSettings.LoadActors.ToString());
        }

        private void chkLoadTranslokator_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadTranslokator = chkLoadTranslokator.Checked;
            ToolkitSettings.WriteKey("LoadTranslokator", "LoadOptions", ToolkitSettings.LoadTranslokator.ToString());
        }
        private void chkLoadAIWorld_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadAIWorld = chkLoadAIWorld.Checked;
            ToolkitSettings.WriteKey("LoadAIWorld", "LoadOptions", ToolkitSettings.LoadAIWorld.ToString());
        }

        private void chkLoadOBJData_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadOBJData = chkLoadOBJData.Checked;
            ToolkitSettings.WriteKey("LoadOBJData", "LoadOptions", ToolkitSettings.LoadOBJData.ToString());
        }
        private void chkLoadRoads_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadRoads = chkLoadRoads.Checked;
            ToolkitSettings.WriteKey("LoadRoads", "LoadOptions", ToolkitSettings.LoadRoads.ToString());
        }
        private void chkLoadATP_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadATP = chkLoadATP.Checked;
            ToolkitSettings.WriteKey("LoadATP", "LoadOptions", ToolkitSettings.LoadATP.ToString());
        }
        private void chkLoadPrefabs_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadPrefabs = chkLoadPrefabs.Checked;
            ToolkitSettings.WriteKey("LoadPrefabs", "LoadOptions", ToolkitSettings.LoadPrefabs.ToString());
        }
        private void chkLoadItemDescs_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadItemDescs = chkLoadItemDescs.Checked;
            ToolkitSettings.WriteKey("LoadItemDescs", "LoadOptions", ToolkitSettings.LoadItemDescs.ToString());
        }
        private void chkLoadHPD_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadHPD = chkLoadHPD.Checked;
            ToolkitSettings.WriteKey("LoadHPD", "LoadOptions", ToolkitSettings.LoadHPD.ToString());
        }
        private void chkLoadSoundSectors_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoadSoundSectors = chkLoadSoundSectors.Checked;
            ToolkitSettings.WriteKey("LoadSoundSectors", "LoadOptions", ToolkitSettings.LoadSoundSectors.ToString());
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
