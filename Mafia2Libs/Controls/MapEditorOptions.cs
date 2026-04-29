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
            LoadLightingSettings();
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

        private void LoadLightingSettings()
        {
            txtAmbient.Text = $"{ToolkitSettings.AmbientR} {ToolkitSettings.AmbientG} {ToolkitSettings.AmbientB} {ToolkitSettings.AmbientA}";
            txtDiffuse.Text = $"{ToolkitSettings.DiffuseR} {ToolkitSettings.DiffuseG} {ToolkitSettings.DiffuseB} {ToolkitSettings.DiffuseA}";
            txtSpecular.Text = $"{ToolkitSettings.SpecularR} {ToolkitSettings.SpecularG} {ToolkitSettings.SpecularB} {ToolkitSettings.SpecularA}";
            txtDirection.Text = $"{ToolkitSettings.LightDirX} {ToolkitSettings.LightDirY} {ToolkitSettings.LightDirZ}";
            txtPower.Text = ToolkitSettings.SpecularPower.ToString();
        }

        private void BtnApplyLighting_Click(object sender, EventArgs e)
        {
            ApplyLightingChanges();
        }

        private void ApplyLightingChanges()
        {
            float[] ambient = ParseFloats(txtAmbient.Text, 4);
            if (ambient != null) { ToolkitSettings.AmbientR = ambient[0]; ToolkitSettings.AmbientG = ambient[1]; ToolkitSettings.AmbientB = ambient[2]; ToolkitSettings.AmbientA = ambient[3]; }

            float[] diffuse = ParseFloats(txtDiffuse.Text, 4);
            if (diffuse != null) { ToolkitSettings.DiffuseR = diffuse[0]; ToolkitSettings.DiffuseG = diffuse[1]; ToolkitSettings.DiffuseB = diffuse[2]; ToolkitSettings.DiffuseA = diffuse[3]; }

            float[] specular = ParseFloats(txtSpecular.Text, 4);
            if (specular != null) { ToolkitSettings.SpecularR = specular[0]; ToolkitSettings.SpecularG = specular[1]; ToolkitSettings.SpecularB = specular[2]; ToolkitSettings.SpecularA = specular[3]; }

            float[] direction = ParseFloats(txtDirection.Text, 3);
            if (direction != null)
            {
                ToolkitSettings.LightDirX = direction[0];
                ToolkitSettings.LightDirY = direction[1];
                ToolkitSettings.LightDirZ = direction[2];
            }

            float power;
            if (float.TryParse(txtPower.Text, out power)) ToolkitSettings.SpecularPower = power;
            SaveLightingToIni();

        }

        private float[] ParseFloats(string text, int expectedCount)
        {
            string[] parts = text.Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < expectedCount) return null;
            float[] result = new float[expectedCount];
            for (int i = 0; i < expectedCount; i++)
                if (!float.TryParse(parts[i], out result[i])) return null;
            return result;
        }

        private void SaveLightingToIni()
        {
            WriteKey("AmbientR", ToolkitSettings.AmbientR); WriteKey("AmbientG", ToolkitSettings.AmbientG);
            WriteKey("AmbientB", ToolkitSettings.AmbientB); WriteKey("AmbientA", ToolkitSettings.AmbientA);
            WriteKey("DiffuseR", ToolkitSettings.DiffuseR); WriteKey("DiffuseG", ToolkitSettings.DiffuseG);
            WriteKey("DiffuseB", ToolkitSettings.DiffuseB); WriteKey("DiffuseA", ToolkitSettings.DiffuseA);
            WriteKey("SpecularR", ToolkitSettings.SpecularR); WriteKey("SpecularG", ToolkitSettings.SpecularG);
            WriteKey("SpecularB", ToolkitSettings.SpecularB); WriteKey("SpecularA", ToolkitSettings.SpecularA);
            WriteKey("LightDirX", ToolkitSettings.LightDirX); WriteKey("LightDirY", ToolkitSettings.LightDirY);
            WriteKey("LightDirZ", ToolkitSettings.LightDirZ); WriteKey("SpecularPower", ToolkitSettings.SpecularPower);
        }

        private void WriteKey(string key, float value) => ToolkitSettings.WriteKey(key, "Lighting", value.ToString());

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
