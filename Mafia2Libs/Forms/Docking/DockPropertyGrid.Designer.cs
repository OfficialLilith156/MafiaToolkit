namespace Forms.Docking
{
    partial class DockPropertyGrid
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
            QuickEditTab = new System.Windows.Forms.TabPage();
            button4 = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            textInvertZQuaternion = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            textQuaternion = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            ScaleZNumeric = new System.Windows.Forms.NumericUpDown();
            ScaleYNumeric = new System.Windows.Forms.NumericUpDown();
            ScaleXNumeric = new System.Windows.Forms.NumericUpDown();
            ScaleZLabel = new System.Windows.Forms.Label();
            ScaleYLabel = new System.Windows.Forms.Label();
            ScaleXLabel = new System.Windows.Forms.Label();
            RotationZNumeric = new System.Windows.Forms.NumericUpDown();
            RotationYNumeric = new System.Windows.Forms.NumericUpDown();
            RotationXNumeric = new System.Windows.Forms.NumericUpDown();
            PositionZNumeric = new System.Windows.Forms.NumericUpDown();
            PositionYNumeric = new System.Windows.Forms.NumericUpDown();
            PositionXNumeric = new System.Windows.Forms.NumericUpDown();
            CurrentEntry = new System.Windows.Forms.Label();
            RotationZLabel = new System.Windows.Forms.Label();
            RotationYLabel = new System.Windows.Forms.Label();
            RotationXLabel = new System.Windows.Forms.Label();
            PositionZLabel = new System.Windows.Forms.Label();
            PositionYLabel = new System.Windows.Forms.Label();
            PositionXLabel = new System.Windows.Forms.Label();
            PropertyTab = new System.Windows.Forms.TabPage();
            PropertyGrid = new System.Windows.Forms.PropertyGrid();
            MainTabControl = new System.Windows.Forms.TabControl();
            MaterialPage = new System.Windows.Forms.TabPage();
            MatViewPanel = new System.Windows.Forms.FlowLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            LODComboBox = new System.Windows.Forms.ComboBox();
            QuickEditTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ScaleZNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleYNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleXNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RotationZNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RotationYNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RotationXNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PositionZNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PositionYNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PositionXNumeric).BeginInit();
            PropertyTab.SuspendLayout();
            MainTabControl.SuspendLayout();
            MaterialPage.SuspendLayout();
            SuspendLayout();
            // 
            // QuickEditTab
            // 
            QuickEditTab.Controls.Add(button4);
            QuickEditTab.Controls.Add(label4);
            QuickEditTab.Controls.Add(label3);
            QuickEditTab.Controls.Add(textInvertZQuaternion);
            QuickEditTab.Controls.Add(button3);
            QuickEditTab.Controls.Add(textQuaternion);
            QuickEditTab.Controls.Add(button2);
            QuickEditTab.Controls.Add(button1);
            QuickEditTab.Controls.Add(ScaleZNumeric);
            QuickEditTab.Controls.Add(ScaleYNumeric);
            QuickEditTab.Controls.Add(ScaleXNumeric);
            QuickEditTab.Controls.Add(ScaleZLabel);
            QuickEditTab.Controls.Add(ScaleYLabel);
            QuickEditTab.Controls.Add(ScaleXLabel);
            QuickEditTab.Controls.Add(RotationZNumeric);
            QuickEditTab.Controls.Add(RotationYNumeric);
            QuickEditTab.Controls.Add(RotationXNumeric);
            QuickEditTab.Controls.Add(PositionZNumeric);
            QuickEditTab.Controls.Add(PositionYNumeric);
            QuickEditTab.Controls.Add(PositionXNumeric);
            QuickEditTab.Controls.Add(CurrentEntry);
            QuickEditTab.Controls.Add(RotationZLabel);
            QuickEditTab.Controls.Add(RotationYLabel);
            QuickEditTab.Controls.Add(RotationXLabel);
            QuickEditTab.Controls.Add(PositionZLabel);
            QuickEditTab.Controls.Add(PositionYLabel);
            QuickEditTab.Controls.Add(PositionXLabel);
            QuickEditTab.Location = new System.Drawing.Point(4, 24);
            QuickEditTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            QuickEditTab.Name = "QuickEditTab";
            QuickEditTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            QuickEditTab.Size = new System.Drawing.Size(306, 504);
            QuickEditTab.TabIndex = 1;
            QuickEditTab.Text = "Edit Transform";
            QuickEditTab.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(12, 436);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(44, 23);
            button4.TabIndex = 37;
            button4.Text = "Copy";
            button4.UseVisualStyleBackColor = true;
            button4.Click += ButtonQuatInvertZCopy_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 403);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(124, 15);
            label4.TabIndex = 36;
            label4.Text = "Quaternion + Invert Z:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 345);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 15);
            label3.TabIndex = 35;
            label3.Text = "Quaternion:";
            // 
            // textInvertZQuaternion
            // 
            textInvertZQuaternion.AutoSize = true;
            textInvertZQuaternion.Location = new System.Drawing.Point(12, 418);
            textInvertZQuaternion.Name = "textInvertZQuaternion";
            textInvertZQuaternion.Size = new System.Drawing.Size(38, 15);
            textInvertZQuaternion.TabIndex = 34;
            textInvertZQuaternion.Text = "label2";
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(12, 378);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(44, 23);
            button3.TabIndex = 33;
            button3.Text = "Copy";
            button3.UseVisualStyleBackColor = true;
            button3.Click += ButtonQuatCopy_Click;
            // 
            // textQuaternion
            // 
            textQuaternion.AutoSize = true;
            textQuaternion.Location = new System.Drawing.Point(12, 360);
            textQuaternion.Name = "textQuaternion";
            textQuaternion.Size = new System.Drawing.Size(38, 15);
            textQuaternion.TabIndex = 32;
            textQuaternion.Text = "label2";
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(158, 319);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(140, 23);
            button2.TabIndex = 31;
            button2.Text = "Paste Position";
            button2.UseVisualStyleBackColor = true;
            button2.Click += buttonPaste_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(12, 319);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(140, 23);
            button1.TabIndex = 30;
            button1.Text = "Copy Position";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonCopy_Click;
            // 
            // ScaleZNumeric
            // 
            ScaleZNumeric.DecimalPlaces = 5;
            ScaleZNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            ScaleZNumeric.Location = new System.Drawing.Point(78, 277);
            ScaleZNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ScaleZNumeric.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            ScaleZNumeric.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            ScaleZNumeric.Name = "ScaleZNumeric";
            ScaleZNumeric.Size = new System.Drawing.Size(216, 23);
            ScaleZNumeric.TabIndex = 29;
            ScaleZNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // ScaleYNumeric
            // 
            ScaleYNumeric.DecimalPlaces = 5;
            ScaleYNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            ScaleYNumeric.Location = new System.Drawing.Point(78, 247);
            ScaleYNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ScaleYNumeric.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            ScaleYNumeric.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            ScaleYNumeric.Name = "ScaleYNumeric";
            ScaleYNumeric.Size = new System.Drawing.Size(216, 23);
            ScaleYNumeric.TabIndex = 28;
            ScaleYNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // ScaleXNumeric
            // 
            ScaleXNumeric.DecimalPlaces = 5;
            ScaleXNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            ScaleXNumeric.Location = new System.Drawing.Point(78, 217);
            ScaleXNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ScaleXNumeric.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            ScaleXNumeric.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            ScaleXNumeric.Name = "ScaleXNumeric";
            ScaleXNumeric.Size = new System.Drawing.Size(216, 23);
            ScaleXNumeric.TabIndex = 27;
            ScaleXNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // ScaleZLabel
            // 
            ScaleZLabel.AutoSize = true;
            ScaleZLabel.Location = new System.Drawing.Point(12, 279);
            ScaleZLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ScaleZLabel.Name = "ScaleZLabel";
            ScaleZLabel.Size = new System.Drawing.Size(44, 15);
            ScaleZLabel.TabIndex = 26;
            ScaleZLabel.Text = "Scale Z";
            // 
            // ScaleYLabel
            // 
            ScaleYLabel.AutoSize = true;
            ScaleYLabel.Location = new System.Drawing.Point(12, 249);
            ScaleYLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ScaleYLabel.Name = "ScaleYLabel";
            ScaleYLabel.Size = new System.Drawing.Size(44, 15);
            ScaleYLabel.TabIndex = 25;
            ScaleYLabel.Text = "Scale Y";
            // 
            // ScaleXLabel
            // 
            ScaleXLabel.AutoSize = true;
            ScaleXLabel.Location = new System.Drawing.Point(12, 219);
            ScaleXLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ScaleXLabel.Name = "ScaleXLabel";
            ScaleXLabel.Size = new System.Drawing.Size(44, 15);
            ScaleXLabel.TabIndex = 24;
            ScaleXLabel.Text = "Scale X";
            // 
            // RotationZNumeric
            // 
            RotationZNumeric.DecimalPlaces = 5;
            RotationZNumeric.Location = new System.Drawing.Point(78, 187);
            RotationZNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RotationZNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            RotationZNumeric.Minimum = new decimal(new int[] { 360, 0, 0, int.MinValue });
            RotationZNumeric.Name = "RotationZNumeric";
            RotationZNumeric.Size = new System.Drawing.Size(216, 23);
            RotationZNumeric.TabIndex = 23;
            RotationZNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // RotationYNumeric
            // 
            RotationYNumeric.DecimalPlaces = 5;
            RotationYNumeric.Location = new System.Drawing.Point(78, 157);
            RotationYNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RotationYNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            RotationYNumeric.Minimum = new decimal(new int[] { 360, 0, 0, int.MinValue });
            RotationYNumeric.Name = "RotationYNumeric";
            RotationYNumeric.Size = new System.Drawing.Size(216, 23);
            RotationYNumeric.TabIndex = 22;
            RotationYNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // RotationXNumeric
            // 
            RotationXNumeric.DecimalPlaces = 5;
            RotationXNumeric.Location = new System.Drawing.Point(78, 127);
            RotationXNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RotationXNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            RotationXNumeric.Minimum = new decimal(new int[] { 360, 0, 0, int.MinValue });
            RotationXNumeric.Name = "RotationXNumeric";
            RotationXNumeric.Size = new System.Drawing.Size(216, 23);
            RotationXNumeric.TabIndex = 21;
            RotationXNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // PositionZNumeric
            // 
            PositionZNumeric.DecimalPlaces = 5;
            PositionZNumeric.Location = new System.Drawing.Point(78, 97);
            PositionZNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PositionZNumeric.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            PositionZNumeric.Minimum = new decimal(new int[] { 999999999, 0, 0, int.MinValue });
            PositionZNumeric.Name = "PositionZNumeric";
            PositionZNumeric.Size = new System.Drawing.Size(216, 23);
            PositionZNumeric.TabIndex = 20;
            PositionZNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // PositionYNumeric
            // 
            PositionYNumeric.DecimalPlaces = 5;
            PositionYNumeric.Location = new System.Drawing.Point(78, 67);
            PositionYNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PositionYNumeric.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            PositionYNumeric.Minimum = new decimal(new int[] { 999999999, 0, 0, int.MinValue });
            PositionYNumeric.Name = "PositionYNumeric";
            PositionYNumeric.Size = new System.Drawing.Size(216, 23);
            PositionYNumeric.TabIndex = 19;
            PositionYNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // PositionXNumeric
            // 
            PositionXNumeric.DecimalPlaces = 5;
            PositionXNumeric.Location = new System.Drawing.Point(78, 37);
            PositionXNumeric.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PositionXNumeric.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            PositionXNumeric.Minimum = new decimal(new int[] { 999999999, 0, 0, int.MinValue });
            PositionXNumeric.Name = "PositionXNumeric";
            PositionXNumeric.Size = new System.Drawing.Size(216, 23);
            PositionXNumeric.TabIndex = 18;
            PositionXNumeric.ValueChanged += ObjectHasUpdated;
            // 
            // CurrentEntry
            // 
            CurrentEntry.AutoSize = true;
            CurrentEntry.Location = new System.Drawing.Point(12, 8);
            CurrentEntry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CurrentEntry.Name = "CurrentEntry";
            CurrentEntry.Size = new System.Drawing.Size(0, 15);
            CurrentEntry.TabIndex = 12;
            // 
            // RotationZLabel
            // 
            RotationZLabel.AutoSize = true;
            RotationZLabel.Location = new System.Drawing.Point(8, 189);
            RotationZLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            RotationZLabel.Name = "RotationZLabel";
            RotationZLabel.Size = new System.Drawing.Size(62, 15);
            RotationZLabel.TabIndex = 10;
            RotationZLabel.Text = "Rotation Z";
            // 
            // RotationYLabel
            // 
            RotationYLabel.AutoSize = true;
            RotationYLabel.Location = new System.Drawing.Point(8, 159);
            RotationYLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            RotationYLabel.Name = "RotationYLabel";
            RotationYLabel.Size = new System.Drawing.Size(62, 15);
            RotationYLabel.TabIndex = 8;
            RotationYLabel.Text = "Rotation Y";
            // 
            // RotationXLabel
            // 
            RotationXLabel.AutoSize = true;
            RotationXLabel.Location = new System.Drawing.Point(8, 129);
            RotationXLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            RotationXLabel.Name = "RotationXLabel";
            RotationXLabel.Size = new System.Drawing.Size(62, 15);
            RotationXLabel.TabIndex = 6;
            RotationXLabel.Text = "Rotation X";
            // 
            // PositionZLabel
            // 
            PositionZLabel.AutoSize = true;
            PositionZLabel.Location = new System.Drawing.Point(8, 99);
            PositionZLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PositionZLabel.Name = "PositionZLabel";
            PositionZLabel.Size = new System.Drawing.Size(60, 15);
            PositionZLabel.TabIndex = 4;
            PositionZLabel.Text = "Position Z";
            // 
            // PositionYLabel
            // 
            PositionYLabel.AutoSize = true;
            PositionYLabel.Location = new System.Drawing.Point(8, 69);
            PositionYLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PositionYLabel.Name = "PositionYLabel";
            PositionYLabel.Size = new System.Drawing.Size(60, 15);
            PositionYLabel.TabIndex = 2;
            PositionYLabel.Text = "Position Y";
            // 
            // PositionXLabel
            // 
            PositionXLabel.AutoSize = true;
            PositionXLabel.Location = new System.Drawing.Point(7, 39);
            PositionXLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PositionXLabel.Name = "PositionXLabel";
            PositionXLabel.Size = new System.Drawing.Size(60, 15);
            PositionXLabel.TabIndex = 0;
            PositionXLabel.Text = "Position X";
            // 
            // PropertyTab
            // 
            PropertyTab.Controls.Add(PropertyGrid);
            PropertyTab.Location = new System.Drawing.Point(4, 24);
            PropertyTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyTab.Name = "PropertyTab";
            PropertyTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyTab.Size = new System.Drawing.Size(306, 504);
            PropertyTab.TabIndex = 0;
            PropertyTab.Text = "Property Grid";
            PropertyTab.UseVisualStyleBackColor = true;
            // 
            // PropertyGrid
            // 
            PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            PropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            PropertyGrid.Location = new System.Drawing.Point(4, 3);
            PropertyGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid.Name = "PropertyGrid";
            PropertyGrid.Size = new System.Drawing.Size(298, 498);
            PropertyGrid.TabIndex = 2;
            // 
            // MainTabControl
            // 
            MainTabControl.Controls.Add(PropertyTab);
            MainTabControl.Controls.Add(QuickEditTab);
            MainTabControl.Controls.Add(MaterialPage);
            MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            MainTabControl.Location = new System.Drawing.Point(0, 0);
            MainTabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MainTabControl.Name = "MainTabControl";
            MainTabControl.SelectedIndex = 0;
            MainTabControl.Size = new System.Drawing.Size(314, 532);
            MainTabControl.TabIndex = 7;
            MainTabControl.SelectedIndexChanged += MainTabControl_OnTabIndexChanged;
            // 
            // MaterialPage
            // 
            MaterialPage.Controls.Add(MatViewPanel);
            MaterialPage.Controls.Add(label1);
            MaterialPage.Controls.Add(LODComboBox);
            MaterialPage.Location = new System.Drawing.Point(4, 24);
            MaterialPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaterialPage.Name = "MaterialPage";
            MaterialPage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaterialPage.Size = new System.Drawing.Size(306, 504);
            MaterialPage.TabIndex = 2;
            MaterialPage.Text = "Model Materials";
            MaterialPage.UseVisualStyleBackColor = true;
            // 
            // MatViewPanel
            // 
            MatViewPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MatViewPanel.AutoScroll = true;
            MatViewPanel.Location = new System.Drawing.Point(14, 59);
            MatViewPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MatViewPanel.Name = "MatViewPanel";
            MatViewPanel.Size = new System.Drawing.Size(279, 434);
            MatViewPanel.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(10, 8);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 15);
            label1.TabIndex = 1;
            label1.Text = "Select LOD";
            // 
            // LODComboBox
            // 
            LODComboBox.FormattingEnabled = true;
            LODComboBox.Location = new System.Drawing.Point(9, 28);
            LODComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LODComboBox.Name = "LODComboBox";
            LODComboBox.Size = new System.Drawing.Size(283, 23);
            LODComboBox.TabIndex = 0;
            LODComboBox.SelectedIndexChanged += SelectedIndexChanged;
            // 
            // DockPropertyGrid
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(314, 532);
            Controls.Add(MainTabControl);
            DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom;
            HideOnClose = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "DockPropertyGrid";
            ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            TabText = "Frame Properties";
            Text = "PropertyGrid";
            QuickEditTab.ResumeLayout(false);
            QuickEditTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ScaleZNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleYNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleXNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)RotationZNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)RotationYNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)RotationXNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)PositionZNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)PositionYNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)PositionXNumeric).EndInit();
            PropertyTab.ResumeLayout(false);
            MainTabControl.ResumeLayout(false);
            MaterialPage.ResumeLayout(false);
            MaterialPage.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage QuickEditTab;
        public System.Windows.Forms.NumericUpDown RotationZNumeric;
        public System.Windows.Forms.NumericUpDown RotationYNumeric;
        public System.Windows.Forms.NumericUpDown RotationXNumeric;
        public System.Windows.Forms.NumericUpDown PositionZNumeric;
        public System.Windows.Forms.NumericUpDown PositionYNumeric;
        public System.Windows.Forms.NumericUpDown PositionXNumeric;
        private System.Windows.Forms.Label CurrentEntry;
        private System.Windows.Forms.Label RotationZLabel;
        private System.Windows.Forms.Label RotationYLabel;
        private System.Windows.Forms.Label RotationXLabel;
        private System.Windows.Forms.Label PositionZLabel;
        private System.Windows.Forms.Label PositionYLabel;
        private System.Windows.Forms.Label PositionXLabel;
        private System.Windows.Forms.TabPage PropertyTab;
        public System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.TabControl MainTabControl;
        public System.Windows.Forms.NumericUpDown ScaleZNumeric;
        public System.Windows.Forms.NumericUpDown ScaleYNumeric;
        public System.Windows.Forms.NumericUpDown ScaleXNumeric;
        private System.Windows.Forms.Label ScaleZLabel;
        private System.Windows.Forms.Label ScaleYLabel;
        private System.Windows.Forms.Label ScaleXLabel;
        private System.Windows.Forms.TabPage MaterialPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox LODComboBox;
        private System.Windows.Forms.FlowLayoutPanel MatViewPanel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label textQuaternion;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label textInvertZQuaternion;
    }
}