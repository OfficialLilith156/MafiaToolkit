namespace Forms.Docking
{
    partial class DockViewProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockViewProperties));
            EntryMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            PreviewButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteButton = new System.Windows.Forms.ToolStripMenuItem();
            DuplicateButton = new System.Windows.Forms.ToolStripMenuItem();
            Export3DButton = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            Label_PickIntersection = new System.Windows.Forms.Label();
            TextBox_PickWSLocation = new System.Windows.Forms.TextBox();
            TextBox_WithOffset = new System.Windows.Forms.TextBox();
            Label_IntersectionWithOffset = new System.Windows.Forms.Label();
            Numeric_PosZ = new System.Windows.Forms.NumericUpDown();
            Numeric_PosY = new System.Windows.Forms.NumericUpDown();
            Numeric_PosX = new System.Windows.Forms.NumericUpDown();
            Label_PosZ = new System.Windows.Forms.Label();
            Label_PosY = new System.Windows.Forms.Label();
            Label_PosX = new System.Windows.Forms.Label();
            EntryMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Numeric_PosZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Numeric_PosY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Numeric_PosX).BeginInit();
            SuspendLayout();
            // 
            // EntryMenuStrip
            // 
            EntryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { PreviewButton, DeleteButton, DuplicateButton, Export3DButton });
            EntryMenuStrip.Name = "EntryMenuStrip";
            EntryMenuStrip.Size = new System.Drawing.Size(126, 92);
            // 
            // PreviewButton
            // 
            PreviewButton.Name = "PreviewButton";
            PreviewButton.Size = new System.Drawing.Size(125, 22);
            PreviewButton.Text = "Preview";
            // 
            // DeleteButton
            // 
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new System.Drawing.Size(125, 22);
            DeleteButton.Text = "Delete";
            // 
            // DuplicateButton
            // 
            DuplicateButton.Name = "DuplicateButton";
            DuplicateButton.Size = new System.Drawing.Size(125, 22);
            DuplicateButton.Text = "Duplicate";
            // 
            // Export3DButton
            // 
            Export3DButton.Name = "Export3DButton";
            Export3DButton.Size = new System.Drawing.Size(125, 22);
            Export3DButton.Text = "Export 3D";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "ActorFrame.png");
            imageList1.Images.SetKeyName(1, "AreaFrame.png");
            imageList1.Images.SetKeyName(2, "CameraFrame.png");
            imageList1.Images.SetKeyName(3, "CollisionFrame.png");
            imageList1.Images.SetKeyName(4, "CollisionObject.png");
            imageList1.Images.SetKeyName(5, "LightFrame.png");
            imageList1.Images.SetKeyName(6, "MeshFrame.png");
            imageList1.Images.SetKeyName(7, "Placeholder.png");
            imageList1.Images.SetKeyName(8, "SceneObject.png");
            imageList1.Images.SetKeyName(9, "SkinnedFrame.png");
            imageList1.Images.SetKeyName(10, "DummyFrame.png");
            // 
            // Label_PickIntersection
            // 
            Label_PickIntersection.AutoSize = true;
            Label_PickIntersection.Location = new System.Drawing.Point(10, 10);
            Label_PickIntersection.Name = "Label_PickIntersection";
            Label_PickIntersection.Size = new System.Drawing.Size(118, 15);
            Label_PickIntersection.TabIndex = 1;
            Label_PickIntersection.Text = "Last Pick Intersection";
            // 
            // TextBox_PickWSLocation
            // 
            TextBox_PickWSLocation.Location = new System.Drawing.Point(10, 28);
            TextBox_PickWSLocation.Name = "TextBox_PickWSLocation";
            TextBox_PickWSLocation.Size = new System.Drawing.Size(288, 23);
            TextBox_PickWSLocation.TabIndex = 3;
            // 
            // TextBox_WithOffset
            // 
            TextBox_WithOffset.Location = new System.Drawing.Point(9, 176);
            TextBox_WithOffset.Name = "TextBox_WithOffset";
            TextBox_WithOffset.Size = new System.Drawing.Size(288, 23);
            TextBox_WithOffset.TabIndex = 7;
            // 
            // Label_IntersectionWithOffset
            // 
            Label_IntersectionWithOffset.AutoSize = true;
            Label_IntersectionWithOffset.Location = new System.Drawing.Point(9, 158);
            Label_IntersectionWithOffset.Name = "Label_IntersectionWithOffset";
            Label_IntersectionWithOffset.Size = new System.Drawing.Size(136, 15);
            Label_IntersectionWithOffset.TabIndex = 6;
            Label_IntersectionWithOffset.Text = "Intersection WITH Offset";
            // 
            // Numeric_PosZ
            // 
            Numeric_PosZ.DecimalPlaces = 5;
            Numeric_PosZ.Location = new System.Drawing.Point(81, 123);
            Numeric_PosZ.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Numeric_PosZ.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            Numeric_PosZ.Minimum = new decimal(new int[] { 999999999, 0, 0, int.MinValue });
            Numeric_PosZ.Name = "Numeric_PosZ";
            Numeric_PosZ.Size = new System.Drawing.Size(216, 23);
            Numeric_PosZ.TabIndex = 26;
            Numeric_PosZ.ValueChanged += Numeric_OnValueChanged;
            // 
            // Numeric_PosY
            // 
            Numeric_PosY.DecimalPlaces = 5;
            Numeric_PosY.Location = new System.Drawing.Point(81, 93);
            Numeric_PosY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Numeric_PosY.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            Numeric_PosY.Minimum = new decimal(new int[] { 999999999, 0, 0, int.MinValue });
            Numeric_PosY.Name = "Numeric_PosY";
            Numeric_PosY.Size = new System.Drawing.Size(216, 23);
            Numeric_PosY.TabIndex = 25;
            Numeric_PosY.ValueChanged += Numeric_OnValueChanged;
            // 
            // Numeric_PosX
            // 
            Numeric_PosX.DecimalPlaces = 5;
            Numeric_PosX.Location = new System.Drawing.Point(81, 63);
            Numeric_PosX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Numeric_PosX.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            Numeric_PosX.Minimum = new decimal(new int[] { 999999999, 0, 0, int.MinValue });
            Numeric_PosX.Name = "Numeric_PosX";
            Numeric_PosX.Size = new System.Drawing.Size(216, 23);
            Numeric_PosX.TabIndex = 24;
            Numeric_PosX.ValueChanged += Numeric_OnValueChanged;
            // 
            // Label_PosZ
            // 
            Label_PosZ.AutoSize = true;
            Label_PosZ.Location = new System.Drawing.Point(11, 125);
            Label_PosZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_PosZ.Name = "Label_PosZ";
            Label_PosZ.Size = new System.Drawing.Size(49, 15);
            Label_PosZ.TabIndex = 23;
            Label_PosZ.Text = "Offset Z";
            // 
            // Label_PosY
            // 
            Label_PosY.AutoSize = true;
            Label_PosY.Location = new System.Drawing.Point(11, 95);
            Label_PosY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_PosY.Name = "Label_PosY";
            Label_PosY.Size = new System.Drawing.Size(49, 15);
            Label_PosY.TabIndex = 22;
            Label_PosY.Text = "Offset Y";
            // 
            // Label_PosX
            // 
            Label_PosX.AutoSize = true;
            Label_PosX.Location = new System.Drawing.Point(10, 65);
            Label_PosX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_PosX.Name = "Label_PosX";
            Label_PosX.Size = new System.Drawing.Size(49, 15);
            Label_PosX.TabIndex = 21;
            Label_PosX.Text = "Offset X";
            // 
            // DockViewProperties
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(310, 519);
            Controls.Add(Numeric_PosZ);
            Controls.Add(Numeric_PosY);
            Controls.Add(Numeric_PosX);
            Controls.Add(Label_PosZ);
            Controls.Add(Label_PosY);
            Controls.Add(Label_PosX);
            Controls.Add(TextBox_WithOffset);
            Controls.Add(Label_IntersectionWithOffset);
            Controls.Add(TextBox_PickWSLocation);
            Controls.Add(Label_PickIntersection);
            HideOnClose = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "DockViewProperties";
            TabText = "View Properties";
            Text = "Utilities";
            Resize += OnResize;
            EntryMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Numeric_PosZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)Numeric_PosY).EndInit();
            ((System.ComponentModel.ISupportInitialize)Numeric_PosX).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem PreviewButton;
        public System.Windows.Forms.ToolStripMenuItem DeleteButton;
        public System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        public System.Windows.Forms.ToolStripMenuItem Export3DButton;
        private System.Windows.Forms.Label Label_PickIntersection;
        private System.Windows.Forms.TextBox TextBox_PickWSLocation;
        private System.Windows.Forms.TextBox TextBox_WithOffset;
        private System.Windows.Forms.Label Label_IntersectionWithOffset;
        public System.Windows.Forms.NumericUpDown Numeric_PosZ;
        public System.Windows.Forms.NumericUpDown Numeric_PosY;
        public System.Windows.Forms.NumericUpDown Numeric_PosX;
        private System.Windows.Forms.Label Label_PosZ;
        private System.Windows.Forms.Label Label_PosY;
        private System.Windows.Forms.Label Label_PosX;
    }
}