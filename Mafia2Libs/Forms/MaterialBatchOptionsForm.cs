using ResourceTypes.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Mafia2Tool.Forms
{
    public partial class MaterialBatchOptionsForm : Form
    {

        public string SamplerID => TextID.Text;
        public string SamplerStates => TextSamplerStates.Text;
        public string UnkSet0 => TextUNKSET0.Text;
        public string UnkSet1 => TextUNKSET1.Text;
        public int UnkZero => int.Parse(TextUNKZERO.Text);
        public int TexTypeValue => int.Parse(TexType.Text);
        public int Unk0 => (int)numericUNK0.Value;
        public int Unk1 => (int)numericUNK1.Value;
        public int Unk3 => (int)numericUNK3.Value;
        public int Unk4 => (int)numericUNK4.Value;
        public int Unk5 => (int)numericUNK5.Value;
        public ulong ShaderID => (ulong)numericShaderID.Value;
        public uint ShaderHash => (uint)numericShaderHash.Value;

        public MaterialBatchOptionsForm()
        {
            InitializeComponent();
            numericUNK0.Maximum = decimal.MaxValue;
            numericUNK0.Value = 128;
            numericUNK1.Value = 0;
            numericUNK3.Value = 0;
            numericUNK4.Value = 0;
            numericUNK5.Value = 0;
            numericShaderID.Maximum = decimal.MaxValue;
            numericShaderID.Value = 4894707398632176459;
            numericShaderHash.Maximum = decimal.MaxValue;
            numericShaderHash.Value = 3388704532;
            checkedFlags.SetItemChecked(0, true);
            checkedFlags.SetItemChecked(3, true);
            numericShaderHash.Maximum = decimal.MaxValue;
            TextID.Text = "S000";
            TextSamplerStates.Text = "3,3,2,0,0,0";
            TexType.Text = "2";
            TextUNKSET0.Text = "0,0";
            TextUNKSET1.Text = "0,0";
            TextUNKZERO.Text = "0";
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public MaterialFlags GetSelectedFlags()
        {
            MaterialFlags flags = 0;
            foreach (var item in checkedFlags.CheckedItems)
            {
                string flagName = item.ToString();
                if (Enum.TryParse(flagName, out MaterialFlags parsedFlag))
                {
                    flags |= parsedFlag;
                }
            }
            return flags;
        }
    }
}
