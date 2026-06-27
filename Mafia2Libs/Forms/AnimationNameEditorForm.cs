using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Animation2;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class AnimationNameEditorForm : Form
    {
        private TextBox txtName;
        private Label lblCurrentHash;
        private Button btnSave;
        private Button btnCancel;
        private Label lblName;
        private readonly string _filePath;
        private Animation2 _animation;

        public static class AnimationNameDatabase
        {
            public static Dictionary<ulong, string> HashToName = new Dictionary<ulong, string>();
        }

        public AnimationNameEditorForm(string filePath)
        {
            InitializeComponent();
            _filePath = filePath;
            LoadAnimation();
        }

        private void LoadAnimation()
        {
            try
            {
                _animation = new Animation2(_filePath);
                lblCurrentHash.Text = $"Current hash: 0x{_animation.Header.Hash:X16}";

                if (AnimationNameDatabase.HashToName.TryGetValue(_animation.Header.Hash, out string foundName))
                {
                    txtName.Text = foundName;
                }
                else
                {
                    txtName.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load the animation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newName = txtName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("The name cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _animation.Header.Hash = FNV64.Hash(newName);
                _animation.WriteToFile(_filePath);
                MessageBox.Show("Name successfully changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
