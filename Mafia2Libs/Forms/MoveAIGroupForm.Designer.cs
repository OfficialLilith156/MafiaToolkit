using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class MoveAIGroupForm
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
            offsetXControl = new NumericUpDown();
            offsetYControl = new NumericUpDown();
            offsetZControl = new NumericUpDown();
            moveChildrenCheckBox = new CheckBox();
            okButton = new Button();
            cancelButton = new Button();
            xLabel = new Label();
            yLabel = new Label();
            zLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)offsetXControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)offsetYControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)offsetZControl).BeginInit();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            titleLabel.Location = new System.Drawing.Point(10, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(250, 20);
            titleLabel.TabIndex = 0;
            // 
            // countLabel
            // 
            countLabel.Location = new System.Drawing.Point(10, 29);
            countLabel.Name = "countLabel";
            countLabel.Size = new System.Drawing.Size(250, 20);
            countLabel.TabIndex = 1;
            // 
            // offsetXControl
            // 
            offsetXControl.DecimalPlaces = 3;
            offsetXControl.Location = new System.Drawing.Point(76, 56);
            offsetXControl.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
            offsetXControl.Minimum = new decimal(new int[] { -1, -1, -1, int.MinValue });
            offsetXControl.Name = "offsetXControl";
            offsetXControl.Size = new System.Drawing.Size(150, 23);
            offsetXControl.TabIndex = 3;
            // 
            // offsetYControl
            // 
            offsetYControl.DecimalPlaces = 3;
            offsetYControl.Location = new System.Drawing.Point(76, 85);
            offsetYControl.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
            offsetYControl.Minimum = new decimal(new int[] { -1, -1, -1, int.MinValue });
            offsetYControl.Name = "offsetYControl";
            offsetYControl.Size = new System.Drawing.Size(150, 23);
            offsetYControl.TabIndex = 5;
            // 
            // offsetZControl
            // 
            offsetZControl.DecimalPlaces = 3;
            offsetZControl.Location = new System.Drawing.Point(76, 114);
            offsetZControl.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
            offsetZControl.Minimum = new decimal(new int[] { -1, -1, -1, int.MinValue });
            offsetZControl.Name = "offsetZControl";
            offsetZControl.Size = new System.Drawing.Size(150, 23);
            offsetZControl.TabIndex = 7;
            // 
            // moveChildrenCheckBox
            // 
            moveChildrenCheckBox.Checked = true;
            moveChildrenCheckBox.CheckState = CheckState.Checked;
            moveChildrenCheckBox.Location = new System.Drawing.Point(12, 143);
            moveChildrenCheckBox.Name = "moveChildrenCheckBox";
            moveChildrenCheckBox.Size = new System.Drawing.Size(250, 20);
            moveChildrenCheckBox.TabIndex = 8;
            moveChildrenCheckBox.Text = "Move child points in groups";
            // 
            // okButton
            // 
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new System.Drawing.Point(12, 169);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(80, 25);
            okButton.TabIndex = 9;
            okButton.Text = "OK";
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(112, 169);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(80, 25);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            // 
            // xLabel
            // 
            xLabel.Location = new System.Drawing.Point(12, 59);
            xLabel.Name = "xLabel";
            xLabel.Size = new System.Drawing.Size(60, 20);
            xLabel.TabIndex = 2;
            xLabel.Text = "Offset X:";
            // 
            // yLabel
            // 
            yLabel.Location = new System.Drawing.Point(12, 88);
            yLabel.Name = "yLabel";
            yLabel.Size = new System.Drawing.Size(60, 20);
            yLabel.TabIndex = 4;
            yLabel.Text = "Offset Y:";
            // 
            // zLabel
            // 
            zLabel.Location = new System.Drawing.Point(12, 117);
            zLabel.Name = "zLabel";
            zLabel.Size = new System.Drawing.Size(60, 20);
            zLabel.TabIndex = 6;
            zLabel.Text = "Offset Z:";
            // 
            // MoveAIGroupForm
            // 
            AcceptButton = okButton;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(245, 222);
            Controls.Add(titleLabel);
            Controls.Add(countLabel);
            Controls.Add(xLabel);
            Controls.Add(offsetXControl);
            Controls.Add(yLabel);
            Controls.Add(offsetYControl);
            Controls.Add(zLabel);
            Controls.Add(offsetZControl);
            Controls.Add(moveChildrenCheckBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MoveAIGroupForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Move AI Group";
            ((System.ComponentModel.ISupportInitialize)offsetXControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)offsetYControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)offsetZControl).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label xLabel;
        private Label yLabel;
        private Label zLabel;
    }
}