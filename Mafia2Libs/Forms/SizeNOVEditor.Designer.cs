namespace Mafia2Tool.Forms
{
    partial class SizeNOVEditor
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SizeNOVEditor));
            label1 = new System.Windows.Forms.Label();
            txtFilePath = new System.Windows.Forms.TextBox();
            btnBrowse = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            txtTagHex = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txtTagDecimal = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            txtTagHexBytes = new System.Windows.Forms.TextBox();
            txtIDHexBytes = new System.Windows.Forms.TextBox();
            txtIdDecimal = new System.Windows.Forms.TextBox();
            txtIDHex = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            txtTag2Decimal = new System.Windows.Forms.TextBox();
            txtTag2Hex = new System.Windows.Forms.TextBox();
            txtTag2HexBytes = new System.Windows.Forms.TextBox();
            txtContextHexBytes = new System.Windows.Forms.RichTextBox();
            visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(components);
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            btnSave = new System.Windows.Forms.ToolStripMenuItem();
            saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            txtSizeHex = new System.Windows.Forms.TextBox();
            txtSizeHexBytes = new System.Windows.Forms.TextBox();
            txtObjName = new System.Windows.Forms.TextBox();
            txtExpectedSize = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(61, 45);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(18, 15);
            label1.TabIndex = 0;
            label1.Text = "ID";
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new System.Drawing.Point(12, 431);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new System.Drawing.Size(304, 23);
            txtFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new System.Drawing.Point(322, 431);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(75, 23);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Select NOV";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(340, 25);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(35, 15);
            label4.TabIndex = 4;
            label4.Text = "Bytes";
            // 
            // txtTagHex
            // 
            txtTagHex.Location = new System.Drawing.Point(297, 42);
            txtTagHex.Name = "txtTagHex";
            txtTagHex.Size = new System.Drawing.Size(101, 23);
            txtTagHex.TabIndex = 5;
            txtTagHex.TextChanged += UpdateValueFromHexBytes;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(233, 25);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(28, 15);
            label3.TabIndex = 6;
            label3.Text = "Hex";
            // 
            // txtTagDecimal
            // 
            txtTagDecimal.Location = new System.Drawing.Point(85, 42);
            txtTagDecimal.Name = "txtTagDecimal";
            txtTagDecimal.Size = new System.Drawing.Size(100, 23);
            txtTagDecimal.TabIndex = 7;
            txtTagDecimal.TextChanged += UpdateValueFromDecimal;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(125, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(35, 15);
            label2.TabIndex = 8;
            label2.Text = "Value";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtTagHexBytes
            // 
            txtTagHexBytes.Location = new System.Drawing.Point(191, 42);
            txtTagHexBytes.Name = "txtTagHexBytes";
            txtTagHexBytes.Size = new System.Drawing.Size(100, 23);
            txtTagHexBytes.TabIndex = 9;
            txtTagHexBytes.TextChanged += UpdateValueFromHex;
            // 
            // txtIDHexBytes
            // 
            txtIDHexBytes.Location = new System.Drawing.Point(191, 71);
            txtIDHexBytes.Name = "txtIDHexBytes";
            txtIDHexBytes.Size = new System.Drawing.Size(100, 23);
            txtIDHexBytes.TabIndex = 12;
            txtIDHexBytes.TextChanged += UpdateValueFromHex;
            // 
            // txtIdDecimal
            // 
            txtIdDecimal.Location = new System.Drawing.Point(85, 71);
            txtIdDecimal.Name = "txtIdDecimal";
            txtIdDecimal.Size = new System.Drawing.Size(100, 23);
            txtIdDecimal.TabIndex = 11;
            txtIdDecimal.TextChanged += UpdateValueFromDecimal;
            // 
            // txtIDHex
            // 
            txtIDHex.Location = new System.Drawing.Point(297, 71);
            txtIDHex.Name = "txtIDHex";
            txtIDHex.Size = new System.Drawing.Size(101, 23);
            txtIDHex.TabIndex = 10;
            txtIDHex.TextChanged += UpdateValueFromHexBytes;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(45, 74);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(34, 15);
            label5.TabIndex = 13;
            label5.Text = "Tag 1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(45, 103);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(34, 15);
            label6.TabIndex = 14;
            label6.Text = "Tag 2";
            // 
            // txtTag2Decimal
            // 
            txtTag2Decimal.Location = new System.Drawing.Point(85, 100);
            txtTag2Decimal.Name = "txtTag2Decimal";
            txtTag2Decimal.Size = new System.Drawing.Size(100, 23);
            txtTag2Decimal.TabIndex = 15;
            txtTag2Decimal.TextChanged += UpdateValueFromDecimal;
            // 
            // txtTag2Hex
            // 
            txtTag2Hex.Location = new System.Drawing.Point(297, 100);
            txtTag2Hex.Name = "txtTag2Hex";
            txtTag2Hex.Size = new System.Drawing.Size(100, 23);
            txtTag2Hex.TabIndex = 16;
            txtTag2Hex.TextChanged += UpdateValueFromHexBytes;
            // 
            // txtTag2HexBytes
            // 
            txtTag2HexBytes.Location = new System.Drawing.Point(191, 100);
            txtTag2HexBytes.Name = "txtTag2HexBytes";
            txtTag2HexBytes.Size = new System.Drawing.Size(100, 23);
            txtTag2HexBytes.TabIndex = 17;
            txtTag2HexBytes.TextChanged += UpdateValueFromHex;
            // 
            // txtContextHexBytes
            // 
            txtContextHexBytes.Location = new System.Drawing.Point(12, 187);
            txtContextHexBytes.Name = "txtContextHexBytes";
            txtContextHexBytes.Size = new System.Drawing.Size(385, 238);
            txtContextHexBytes.TabIndex = 20;
            txtContextHexBytes.Text = "";
            // 
            // visualStudioToolStripExtender1
            // 
            visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButton1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(417, 25);
            toolStrip1.TabIndex = 21;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnSave, saveAsToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // btnSave
            // 
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(114, 22);
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += btnSaveAs_Click;
            // 
            // txtSizeHex
            // 
            txtSizeHex.Location = new System.Drawing.Point(297, 129);
            txtSizeHex.Name = "txtSizeHex";
            txtSizeHex.Size = new System.Drawing.Size(100, 23);
            txtSizeHex.TabIndex = 24;
            // 
            // txtSizeHexBytes
            // 
            txtSizeHexBytes.Location = new System.Drawing.Point(191, 129);
            txtSizeHexBytes.Name = "txtSizeHexBytes";
            txtSizeHexBytes.Size = new System.Drawing.Size(100, 23);
            txtSizeHexBytes.TabIndex = 23;
            // 
            // txtObjName
            // 
            txtObjName.Location = new System.Drawing.Point(85, 158);
            txtObjName.Name = "txtObjName";
            txtObjName.Size = new System.Drawing.Size(312, 23);
            txtObjName.TabIndex = 25;
            // 
            // txtExpectedSize
            // 
            txtExpectedSize.Location = new System.Drawing.Point(85, 129);
            txtExpectedSize.Name = "txtExpectedSize";
            txtExpectedSize.Size = new System.Drawing.Size(100, 23);
            txtExpectedSize.TabIndex = 26;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(52, 132);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(27, 15);
            label7.TabIndex = 27;
            label7.Text = "Size";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(12, 161);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(67, 15);
            label8.TabIndex = 28;
            label8.Text = "Name_OBJ ";
            // 
            // SizeNOVEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(417, 462);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(txtExpectedSize);
            Controls.Add(txtObjName);
            Controls.Add(txtSizeHex);
            Controls.Add(txtSizeHexBytes);
            Controls.Add(toolStrip1);
            Controls.Add(txtContextHexBytes);
            Controls.Add(txtTag2HexBytes);
            Controls.Add(txtTag2Hex);
            Controls.Add(txtTag2Decimal);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(txtIDHexBytes);
            Controls.Add(txtIdDecimal);
            Controls.Add(txtIDHex);
            Controls.Add(txtTagHexBytes);
            Controls.Add(label2);
            Controls.Add(txtTagDecimal);
            Controls.Add(label3);
            Controls.Add(txtTagHex);
            Controls.Add(label4);
            Controls.Add(btnBrowse);
            Controls.Add(txtFilePath);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SizeNOVEditor";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Info NOV";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTagHex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTagDecimal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtTagHexBytes;
        private System.Windows.Forms.TextBox txtIDHexBytes;
        private System.Windows.Forms.TextBox txtIdDecimal;
        private System.Windows.Forms.TextBox txtIDHex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTag2Decimal;
        private System.Windows.Forms.TextBox txtTag2Hex;
        private System.Windows.Forms.TextBox txtTag2HexBytes;
        private System.Windows.Forms.RichTextBox txtContextHexBytes;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSizeHex;
        private System.Windows.Forms.TextBox txtSizeHexBytes;
        private System.Windows.Forms.TextBox txtObjName;
        private System.Windows.Forms.TextBox txtExpectedSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}