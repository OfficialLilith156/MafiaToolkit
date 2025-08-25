namespace Mafia2Tool
{
    partial class CityAreaEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CityAreaEditor));
            openM2T = new System.Windows.Forms.OpenFileDialog();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddAreaButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteArea = new System.Windows.Forms.ToolStripMenuItem();
            ListBox_Areas = new System.Windows.Forms.ListBox();
            PropertyGrid_Area = new System.Windows.Forms.PropertyGrid();
            TextBox_Search = new System.Windows.Forms.TextBox();
            Button_Search = new System.Windows.Forms.Button();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // openM2T
            // 
            openM2T.FileName = "Select M2T file.";
            openM2T.Filter = "Model File|*.m2t|All Files|*.*|FBX Model|*.fbx";
            openM2T.Tag = "";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolButton, toolButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ReloadButton, ExitButton });
            fileToolButton.Image = (System.Drawing.Image)resources.GetObject("fileToolButton.Image");
            fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            fileToolButton.Name = "fileToolButton";
            fileToolButton.Size = new System.Drawing.Size(47, 22);
            fileToolButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            SaveButton.Name = "SaveButton";
            SaveButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveButton.Size = new System.Drawing.Size(180, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveButton_Click;
            // 
            // ReloadButton
            // 
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(180, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadButton_Click;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(180, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_Click;
            // 
            // toolButton
            // 
            toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddAreaButton, DeleteArea });
            toolButton.Image = (System.Drawing.Image)resources.GetObject("toolButton.Image");
            toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButton.Name = "toolButton";
            toolButton.Size = new System.Drawing.Size(61, 22);
            toolButton.Text = "$TOOLS";
            // 
            // AddAreaButton
            // 
            AddAreaButton.Name = "AddAreaButton";
            AddAreaButton.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A;
            AddAreaButton.Size = new System.Drawing.Size(190, 22);
            AddAreaButton.Text = "$ADD_AREA";
            AddAreaButton.Click += AddAreaButton_Click;
            // 
            // DeleteArea
            // 
            DeleteArea.Name = "DeleteArea";
            DeleteArea.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D;
            DeleteArea.Size = new System.Drawing.Size(190, 22);
            DeleteArea.Text = "$DELETE_AREA";
            DeleteArea.Click += DeleteArea_Click;
            // 
            // ListBox_Areas
            // 
            ListBox_Areas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ListBox_Areas.FormattingEnabled = true;
            ListBox_Areas.ItemHeight = 15;
            ListBox_Areas.Location = new System.Drawing.Point(14, 62);
            ListBox_Areas.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListBox_Areas.Name = "ListBox_Areas";
            ListBox_Areas.Size = new System.Drawing.Size(447, 439);
            ListBox_Areas.TabIndex = 16;
            ListBox_Areas.SelectedIndexChanged += UpdateAreaData;
            // 
            // PropertyGrid_Area
            // 
            PropertyGrid_Area.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            PropertyGrid_Area.Location = new System.Drawing.Point(469, 32);
            PropertyGrid_Area.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid_Area.Name = "PropertyGrid_Area";
            PropertyGrid_Area.Size = new System.Drawing.Size(450, 470);
            PropertyGrid_Area.TabIndex = 17;
            PropertyGrid_Area.PropertyValueChanged += PropertyGrid_Area_ValueChanged;
            // 
            // TextBox_Search
            // 
            TextBox_Search.Location = new System.Drawing.Point(14, 36);
            TextBox_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_Search.Name = "TextBox_Search";
            TextBox_Search.Size = new System.Drawing.Size(408, 23);
            TextBox_Search.TabIndex = 18;
            // 
            // Button_Search
            // 
            Button_Search.Location = new System.Drawing.Point(429, 33);
            Button_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_Search.Name = "Button_Search";
            Button_Search.Size = new System.Drawing.Size(33, 27);
            Button_Search.TabIndex = 19;
            Button_Search.Text = ">>";
            Button_Search.UseVisualStyleBackColor = true;
            Button_Search.Click += Button_Search_OnClick;
            // 
            // CityAreaEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(Button_Search);
            Controls.Add(TextBox_Search);
            Controls.Add(PropertyGrid_Area);
            Controls.Add(ListBox_Areas);
            Controls.Add(toolStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CityAreaEditor";
            Text = "$ACTOR_EDITOR_TITLE";
            FormClosing += CityAreaEditor_Closing;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openM2T;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem AddAreaButton;
        private System.Windows.Forms.ListBox ListBox_Areas;
        private System.Windows.Forms.ToolStripMenuItem DeleteArea;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Area;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Button Button_Search;
    }
}