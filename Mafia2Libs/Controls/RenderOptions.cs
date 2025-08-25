using System;
using System.Windows.Forms;
using Utils.Language;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class RenderOptions : UserControl
    {
        public RenderOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            RenderGroup.Text = Language.GetString("$RENDER_OPTIONS");
            ScreenFarLabel.Text = Language.GetString("$RENDER_SCREENFAR");
            ScreenNearLabel.Text = Language.GetString("$RENDER_SCREENEAR");
            RenderFieldOfView.Text = Language.GetString("$RENDER_FOV");
            TexLabel.Text = Language.GetString("$TEXTURE_DIRECTORY");
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
            ScreenFarUpDown.Value = Math.Min((decimal)ToolkitSettings.ScreenDepth, ScreenFarUpDown.Maximum);
            ScreenNearUpDown.Value = Math.Min((decimal)ToolkitSettings.ScreenNear, ScreenNearUpDown.Maximum);
            CameraSpeedUpDown.Value = Math.Min((decimal)ToolkitSettings.CameraSpeed, CameraSpeedUpDown.Maximum);
            FieldOfViewNumDown.Value = Math.Min(Math.Max(Convert.ToInt16(ToolkitSettings.FieldOfView), FieldOfViewNumDown.Minimum), FieldOfViewNumDown.Maximum);
            TexDirectoryBox1.Text = ToolkitSettings.TexturePath1;
            TexDirectoryBox2.Text = ToolkitSettings.TexturePath2;
            TexDirectoryBox3.Text = ToolkitSettings.TexturePath3;
            TexDirectoryBox4.Text = ToolkitSettings.TexturePath4;
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
