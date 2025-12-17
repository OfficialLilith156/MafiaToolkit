using Forms.Docking;
using ResourceTypes.Navigation;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class RotateAIGroupForm : Form
    {
        private NumericUpDown angleControl;
        private NumericUpDown centerXControl;
        private NumericUpDown centerYControl;
        private NumericUpDown centerZControl;
        private CheckBox rotateChildrenCheckBox;
        private CheckBox updateRotationsCheckBox;
        private Button okButton;
        private Button cancelButton;
        private Label titleLabel;
        private Label countLabel;
        private Label angleLabel;
        private Label centerLabel;

        public RotateAIGroupForm(string groupName, int pointCount, Vector3 defaultCenter)
        {
            InitializeComponent();
                
            titleLabel.Text = $"Rotate: {groupName}";
            countLabel.Text = $"Points: {pointCount}";

            centerXControl.Value = (decimal)defaultCenter.X;
            centerYControl.Value = (decimal)defaultCenter.Y;
            centerZControl.Value = (decimal)defaultCenter.Z;

            angleControl.Value = 90; 
            rotateChildrenCheckBox.Checked = true;
            updateRotationsCheckBox.Checked = true;
        }

        public float AngleDegrees => (float)angleControl.Value;

        public Vector3 RotationCenter => new Vector3(
            (float)centerXControl.Value,
            (float)centerYControl.Value,
            (float)centerZControl.Value
        );

        public bool RotateChildren => rotateChildrenCheckBox.Checked;

        public bool UpdateRotations => updateRotationsCheckBox.Checked;
    }
}