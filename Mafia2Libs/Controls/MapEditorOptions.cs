using Rendering.Core;
using System;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;

namespace Forms.OptionControls
{
    public partial class MapEditorOptions : UserControl
    {
        public MapEditorOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }
        private void Localise()
        {
            ScreenFarLabel.Text = Language.GetString("$RENDER_SCREENFAR");
            ScreenNearLabel.Text = Language.GetString("$RENDER_SCREENEAR");
            RenderFieldOfView.Text = Language.GetString("$RENDER_FOV");

            CameraSpeedLabel.Text = Language.GetString("$RENDER_CAMERASPEED");
            TexBrowser.Description = Language.GetString("$SELECT_TEX_FOLDER");
            ExperimentalBox.Text = Language.GetString("$ENABLE_EXPERIMENTAL");
            Checkbox_EnableNavigation.Text = Language.GetString("$ENABLE_NAVIGATION");
            ExperimentalBox.Text = Language.GetString("$ENABLE_EXPERIMENTAL");
            Checkbox_EnableTranslokatorTint.Text = Language.GetString("$TOGGLE_TRANSLOKATOR_TINT");
            CheckBox_VSync.Text = Language.GetString("$VSync");
            UseMIPsBox.Text = Language.GetString("$USE_MIPS");

        }
        private void LoadSettings()
        {
            chkLoadFrameResource.Checked = ToolkitSettings.LoadFrameResource;
            chkLoadCollisions.Checked = ToolkitSettings.LoadCollisions;
            chkLoadActors.Checked = ToolkitSettings.LoadActors;
            chkLoadTranslokator.Checked = ToolkitSettings.LoadTranslokator;
            chkLoadAIWorld.Checked = ToolkitSettings.LoadAIWorld;
            chkLoadOBJData.Checked = ToolkitSettings.LoadOBJData;
            chkLoadRoads.Checked = ToolkitSettings.LoadRoads;
            chkLoadATP.Checked = ToolkitSettings.LoadATP;
            chkLoadItemDescs.Checked = ToolkitSettings.LoadItemDescs;
            chkLoadHPD.Checked = ToolkitSettings.LoadHPD;
            chkLoadSoundSectors.Checked = ToolkitSettings.LoadSoundSectors;

            ScreenFarUpDown.Value = Math.Min((decimal)ToolkitSettings.ScreenDepth, ScreenFarUpDown.Maximum);
            ScreenNearUpDown.Value = Math.Min((decimal)ToolkitSettings.ScreenNear, ScreenNearUpDown.Maximum);
            CameraSpeedUpDown.Value = Math.Min((decimal)ToolkitSettings.CameraSpeed, CameraSpeedUpDown.Maximum);
            FieldOfViewNumDown.Value = Math.Min(Math.Max(Convert.ToInt16(ToolkitSettings.FieldOfView), FieldOfViewNumDown.Minimum), FieldOfViewNumDown.Maximum);

            ExperimentalBox.Checked = ToolkitSettings.Experimental;
            Checkbox_EnableNavigation.Checked = ToolkitSettings.bNavigation;
            Checkbox_EnableTranslokatorTint.Checked = ToolkitSettings.bTranslokatorTint;
            UseMIPsBox.Checked = ToolkitSettings.UseMIPS;
            CheckBox_VSync.Checked = ToolkitSettings.VSync;

        }

        private void ScreenDepth_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.ScreenDepth = Convert.ToSingle(ScreenFarUpDown.Value);
            ToolkitSettings.WriteKey("ScreenDepth", "ModelViewer", ToolkitSettings.ScreenDepth.ToString());
        }

        private void ScreenNear_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.ScreenNear = Convert.ToSingle(ScreenNearUpDown.Value);
            ToolkitSettings.WriteKey("ScreenNear", "ModelViewer", ToolkitSettings.ScreenNear.ToString());
        }

        private void CameraSpeedUpDown_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.CameraSpeed = Convert.ToSingle(CameraSpeedUpDown.Value);
            ToolkitSettings.WriteKey("CameraSpeed", "ModelViewer", ToolkitSettings.CameraSpeed.ToString());
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
        private void ExperimentalBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.Experimental = ExperimentalBox.Checked;
            ToolkitSettings.WriteKey("EnableExperimental", "ModelViewer", ToolkitSettings.Experimental.ToString());
        }

        private void UseMIPsBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.UseMIPS = UseMIPsBox.Checked;
            ToolkitSettings.WriteKey("UseMIPs", "ModelViewer", ToolkitSettings.UseMIPS.ToString());
        }

        private void FieldOfViewNumDown_ValueChanged(object sender, EventArgs e)
        {
            ToolkitSettings.FieldOfView = (float)FieldOfViewNumDown.Value;
            ToolkitSettings.WriteKey("FieldOfView", "ModelViewer", ToolkitSettings.FieldOfView.ToString());
        }

        private void CheckBox_VSync_OnChecked(object sender, EventArgs e)
        {
            ToolkitSettings.VSync = CheckBox_VSync.Checked;
            ToolkitSettings.WriteKey("VSync", "ModelViewer", ToolkitSettings.VSync.ToString());
        }

        private void Button_EnableNavigation_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.bNavigation = Checkbox_EnableNavigation.Checked;
            ToolkitSettings.WriteKey("EnableNavigation", "ModelViewer", ToolkitSettings.bNavigation.ToString());
        }

        private void Button_EnableTranslokatorTint_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.bTranslokatorTint = Checkbox_EnableTranslokatorTint.Checked;
            ToolkitSettings.WriteKey("EnableTranslokator", "ModelViewer", ToolkitSettings.bTranslokatorTint.ToString());
        }
    }
}
