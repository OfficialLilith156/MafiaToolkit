using System.Drawing;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class RotateAIGroupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleLabel = new Label();
            countLabel = new Label();
            angleControl = new NumericUpDown();
            centerXControl = new NumericUpDown();
            centerYControl = new NumericUpDown();
            centerZControl = new NumericUpDown();
            rotateChildrenCheckBox = new CheckBox();
            updateRotationsCheckBox = new CheckBox();
            okButton = new Button();
            cancelButton = new Button();
            angleLabel = new Label();
            centerLabel = new Label();
            xLabel = new Label();
            yLabel = new Label();
            zLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)angleControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)centerXControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)centerYControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)centerZControl).BeginInit();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point);
            titleLabel.Location = new Point(15, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(300, 20);
            titleLabel.TabIndex = 0;
            // 
            // countLabel
            // 
            countLabel.Location = new Point(15, 29);
            countLabel.Name = "countLabel";
            countLabel.Size = new Size(300, 20);
            countLabel.TabIndex = 1;
            // 
            // angleControl
            // 
            angleControl.DecimalPlaces = 2;
            angleControl.Location = new Point(15, 93);
            angleControl.Maximum = new decimal(new int[] { 720, 0, 0, 0 });
            angleControl.Minimum = new decimal(new int[] { 720, 0, 0, int.MinValue });
            angleControl.Name = "angleControl";
            angleControl.Size = new Size(120, 23);
            angleControl.TabIndex = 3;
            angleControl.Value = new decimal(new int[] { 90, 0, 0, 0 });
            // 
            // centerXControl
            // 
            centerXControl.DecimalPlaces = 3;
            centerXControl.Location = new Point(38, 151);
            centerXControl.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
            centerXControl.Minimum = new decimal(new int[] { -1, -1, -1, int.MinValue });
            centerXControl.Name = "centerXControl";
            centerXControl.Size = new Size(120, 23);
            centerXControl.TabIndex = 6;
            // 
            // centerYControl
            // 
            centerYControl.DecimalPlaces = 3;
            centerYControl.Location = new Point(38, 177);
            centerYControl.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
            centerYControl.Minimum = new decimal(new int[] { -1, -1, -1, int.MinValue });
            centerYControl.Name = "centerYControl";
            centerYControl.Size = new Size(120, 23);
            centerYControl.TabIndex = 8;
            // 
            // centerZControl
            // 
            centerZControl.DecimalPlaces = 3;
            centerZControl.Location = new Point(38, 204);
            centerZControl.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
            centerZControl.Minimum = new decimal(new int[] { -1, -1, -1, int.MinValue });
            centerZControl.Name = "centerZControl";
            centerZControl.Size = new Size(120, 23);
            centerZControl.TabIndex = 10;
            // 
            // rotateChildrenCheckBox
            // 
            rotateChildrenCheckBox.Checked = true;
            rotateChildrenCheckBox.CheckState = CheckState.Checked;
            rotateChildrenCheckBox.Location = new Point(12, 259);
            rotateChildrenCheckBox.Name = "rotateChildrenCheckBox";
            rotateChildrenCheckBox.Size = new Size(250, 20);
            rotateChildrenCheckBox.TabIndex = 12;
            rotateChildrenCheckBox.Text = "Rotate child points in groups";
            // 
            // updateRotationsCheckBox
            // 
            updateRotationsCheckBox.Checked = true;
            updateRotationsCheckBox.CheckState = CheckState.Checked;
            updateRotationsCheckBox.Location = new Point(12, 233);
            updateRotationsCheckBox.Name = "updateRotationsCheckBox";
            updateRotationsCheckBox.Size = new Size(250, 20);
            updateRotationsCheckBox.TabIndex = 13;
            updateRotationsCheckBox.Text = "Update rotation values (Type4/Type7)";
            // 
            // okButton
            // 
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(12, 285);
            okButton.Name = "okButton";
            okButton.Size = new Size(80, 25);
            okButton.TabIndex = 14;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(216, 285);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(80, 25);
            cancelButton.TabIndex = 15;
            cancelButton.Text = "Cancel";
            // 
            // angleLabel
            // 
            angleLabel.Location = new Point(15, 70);
            angleLabel.Name = "angleLabel";
            angleLabel.Size = new Size(150, 20);
            angleLabel.TabIndex = 2;
            angleLabel.Text = "Rotation Angle (degrees):";
            // 
            // centerLabel
            // 
            centerLabel.Location = new Point(12, 128);
            centerLabel.Name = "centerLabel";
            centerLabel.Size = new Size(150, 20);
            centerLabel.TabIndex = 4;
            centerLabel.Text = "Rotation Center:";
            // 
            // xLabel
            // 
            xLabel.Location = new Point(12, 151);
            xLabel.Name = "xLabel";
            xLabel.Size = new Size(20, 20);
            xLabel.TabIndex = 5;
            xLabel.Text = "X:";
            // 
            // yLabel
            // 
            yLabel.Location = new Point(12, 180);
            yLabel.Name = "yLabel";
            yLabel.Size = new Size(20, 20);
            yLabel.TabIndex = 7;
            yLabel.Text = "Y:";
            // 
            // zLabel
            // 
            zLabel.Location = new Point(12, 204);
            zLabel.Name = "zLabel";
            zLabel.Size = new Size(20, 20);
            zLabel.TabIndex = 9;
            zLabel.Text = "Z:";
            // 
            // RotateAIGroupForm
            // 
            AcceptButton = okButton;
            CancelButton = cancelButton;
            ClientSize = new Size(327, 336);
            Controls.Add(titleLabel);
            Controls.Add(countLabel);
            Controls.Add(angleLabel);
            Controls.Add(angleControl);
            Controls.Add(centerLabel);
            Controls.Add(xLabel);
            Controls.Add(centerXControl);
            Controls.Add(yLabel);
            Controls.Add(centerYControl);
            Controls.Add(zLabel);
            Controls.Add(centerZControl);
            Controls.Add(rotateChildrenCheckBox);
            Controls.Add(updateRotationsCheckBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RotateAIGroupForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Rotate AI Group (Z Axis)";
            ((System.ComponentModel.ISupportInitialize)angleControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)centerXControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)centerYControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)centerZControl).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label xLabel;
        private Label yLabel;
        private Label zLabel;
    }
}