using System;
using System.Numerics;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class MoveAIGroupForm : Form
    {
        private NumericUpDown offsetXControl;
        private NumericUpDown offsetYControl;
        private NumericUpDown offsetZControl;
        private CheckBox moveChildrenCheckBox;
        private Button okButton;
        private Button cancelButton;
        private Label titleLabel;
        private Label countLabel;

        public MoveAIGroupForm(string groupName, int pointCount)
        {
            InitializeComponent();
    
            titleLabel.Text = $"Move: {groupName}";
            countLabel.Text = $"Points: {pointCount}";
        }

        public Vector3 GetOffset()
        {
            return new Vector3(
                (float)offsetXControl.Value,
                (float)offsetYControl.Value,
                (float)offsetZControl.Value
            );
        }

        public bool MoveChildren
        {
            get { return moveChildrenCheckBox.Checked; }
        }
    }
}