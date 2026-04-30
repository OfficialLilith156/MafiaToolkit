
using System;
using System.Windows.Forms;
using ResourceTypes.Materials;

namespace Forms.Docking
{
    partial class DockImportSceneTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockImportSceneTree));
            imageList1 = new ImageList(components);
            TreeView_Explorer = new Mafia2Tool.Controls.MTreeView();
            Tab_Explorer = new TabControl();
            TabPage_Explorer = new TabPage();
            cancelButton = new Button();
            importButton = new Button();
            importTextures = new CheckBox();
            TabPage_Searcher = new TabPage();
            Split_Searcher_Root = new SplitContainer();
            Split_Searcher_TextButton = new SplitContainer();
            TextBox_Search = new TextBox();
            Button_Search = new Button();
            TreeView_Searcher = new Mafia2Tool.Controls.MTreeView();
            Tab_Explorer.SuspendLayout();
            TabPage_Explorer.SuspendLayout();
            TabPage_Searcher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_Root).BeginInit();
            Split_Searcher_Root.Panel1.SuspendLayout();
            Split_Searcher_Root.Panel2.SuspendLayout();
            Split_Searcher_Root.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_TextButton).BeginInit();
            Split_Searcher_TextButton.Panel1.SuspendLayout();
            Split_Searcher_TextButton.Panel2.SuspendLayout();
            Split_Searcher_TextButton.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
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
            // TreeView_Explorer
            // 
            TreeView_Explorer.Dock = DockStyle.Top;
            TreeView_Explorer.HideSelection = false;
            TreeView_Explorer.ImageIndex = 3;
            TreeView_Explorer.ImageList = imageList1;
            TreeView_Explorer.Location = new System.Drawing.Point(3, 3);
            TreeView_Explorer.Margin = new Padding(4, 3, 4, 3);
            TreeView_Explorer.Name = "TreeView_Explorer";
            TreeView_Explorer.SelectedImageIndex = 0;
            TreeView_Explorer.Size = new System.Drawing.Size(316, 463);
            TreeView_Explorer.TabIndex = 0;
            TreeView_Explorer.DoubleClick += OnDoubleClick;
            // 
            // Tab_Explorer
            // 
            Tab_Explorer.Controls.Add(TabPage_Explorer);
            Tab_Explorer.Controls.Add(TabPage_Searcher);
            Tab_Explorer.Dock = DockStyle.Fill;
            Tab_Explorer.Location = new System.Drawing.Point(0, 0);
            Tab_Explorer.Name = "Tab_Explorer";
            Tab_Explorer.SelectedIndex = 0;
            Tab_Explorer.Size = new System.Drawing.Size(330, 519);
            Tab_Explorer.TabIndex = 1;
            // 
            // TabPage_Explorer
            // 
            TabPage_Explorer.Controls.Add(cancelButton);
            TabPage_Explorer.Controls.Add(importButton);
            TabPage_Explorer.Controls.Add(importTextures);
            TabPage_Explorer.Controls.Add(TreeView_Explorer);
            TabPage_Explorer.Location = new System.Drawing.Point(4, 24);
            TabPage_Explorer.Name = "TabPage_Explorer";
            TabPage_Explorer.Padding = new Padding(3);
            TabPage_Explorer.Size = new System.Drawing.Size(322, 491);
            TabPage_Explorer.TabIndex = 0;
            TabPage_Explorer.Text = "tabPage1";
            TabPage_Explorer.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(3, 468);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_OnClick;
            // 
            // importButton
            // 
            importButton.Location = new System.Drawing.Point(244, 468);
            importButton.Name = "importButton";
            importButton.Size = new System.Drawing.Size(75, 23);
            importButton.TabIndex = 1;
            importButton.Text = "Import Selected";
            importButton.UseVisualStyleBackColor = true;
            // 
            // importTextures
            // 
            importTextures.AutoSize = true;
            importTextures.Location = new System.Drawing.Point(100, 472);
            importTextures.Name = "importTextures";
            importTextures.Size = new System.Drawing.Size(108, 19);
            importTextures.TabIndex = 3;
            importTextures.Text = "Import Textures";
            importTextures.UseVisualStyleBackColor = true;
            // 
            // TabPage_Searcher
            // 
            TabPage_Searcher.Controls.Add(Split_Searcher_Root);
            TabPage_Searcher.Location = new System.Drawing.Point(4, 24);
            TabPage_Searcher.Name = "TabPage_Searcher";
            TabPage_Searcher.Padding = new Padding(3);
            TabPage_Searcher.Size = new System.Drawing.Size(322, 491);
            TabPage_Searcher.TabIndex = 1;
            TabPage_Searcher.Text = "tabPage2";
            TabPage_Searcher.UseVisualStyleBackColor = true;
            // 
            // Split_Searcher_Root
            // 
            Split_Searcher_Root.Cursor = Cursors.HSplit;
            Split_Searcher_Root.Dock = DockStyle.Fill;
            Split_Searcher_Root.FixedPanel = FixedPanel.Panel1;
            Split_Searcher_Root.IsSplitterFixed = true;
            Split_Searcher_Root.Location = new System.Drawing.Point(3, 3);
            Split_Searcher_Root.Margin = new Padding(4, 3, 4, 3);
            Split_Searcher_Root.Name = "Split_Searcher_Root";
            Split_Searcher_Root.Orientation = Orientation.Horizontal;
            // 
            // Split_Searcher_Root.Panel1
            // 
            Split_Searcher_Root.Panel1.Controls.Add(Split_Searcher_TextButton);
            // 
            // Split_Searcher_Root.Panel2
            // 
            Split_Searcher_Root.Panel2.Controls.Add(TreeView_Searcher);
            Split_Searcher_Root.Size = new System.Drawing.Size(316, 485);
            Split_Searcher_Root.SplitterDistance = 25;
            Split_Searcher_Root.SplitterWidth = 5;
            Split_Searcher_Root.TabIndex = 2;
            // 
            // Split_Searcher_TextButton
            // 
            Split_Searcher_TextButton.Cursor = Cursors.VSplit;
            Split_Searcher_TextButton.Dock = DockStyle.Fill;
            Split_Searcher_TextButton.FixedPanel = FixedPanel.Panel2;
            Split_Searcher_TextButton.IsSplitterFixed = true;
            Split_Searcher_TextButton.Location = new System.Drawing.Point(0, 0);
            Split_Searcher_TextButton.Name = "Split_Searcher_TextButton";
            // 
            // Split_Searcher_TextButton.Panel1
            // 
            Split_Searcher_TextButton.Panel1.Controls.Add(TextBox_Search);
            // 
            // Split_Searcher_TextButton.Panel2
            // 
            Split_Searcher_TextButton.Panel2.Controls.Add(Button_Search);
            Split_Searcher_TextButton.Size = new System.Drawing.Size(316, 25);
            Split_Searcher_TextButton.SplitterDistance = 269;
            Split_Searcher_TextButton.TabIndex = 1;
            // 
            // TextBox_Search
            // 
            TextBox_Search.Dock = DockStyle.Fill;
            TextBox_Search.Location = new System.Drawing.Point(0, 0);
            TextBox_Search.Margin = new Padding(4, 3, 4, 3);
            TextBox_Search.Name = "TextBox_Search";
            TextBox_Search.Size = new System.Drawing.Size(269, 23);
            TextBox_Search.TabIndex = 3;
            TextBox_Search.KeyUp += TextBox_Search_OnKeyUp;
            // 
            // Button_Search
            // 
            Button_Search.Dock = DockStyle.Fill;
            Button_Search.Location = new System.Drawing.Point(0, 0);
            Button_Search.Margin = new Padding(4, 3, 4, 3);
            Button_Search.Name = "Button_Search";
            Button_Search.Size = new System.Drawing.Size(43, 25);
            Button_Search.TabIndex = 0;
            Button_Search.Text = ">>";
            Button_Search.UseVisualStyleBackColor = true;
            Button_Search.Click += Button_Search_OnClick;
            // 
            // TreeView_Searcher
            // 
            TreeView_Searcher.Dock = DockStyle.Fill;
            TreeView_Searcher.HideSelection = false;
            TreeView_Searcher.ImageIndex = 3;
            TreeView_Searcher.ImageList = imageList1;
            TreeView_Searcher.Location = new System.Drawing.Point(0, 0);
            TreeView_Searcher.Margin = new Padding(4, 3, 4, 3);
            TreeView_Searcher.Name = "TreeView_Searcher";
            TreeView_Searcher.SelectedImageIndex = 0;
            TreeView_Searcher.Size = new System.Drawing.Size(316, 455);
            TreeView_Searcher.TabIndex = 0;
            TreeView_Searcher.DoubleClick += TreeView_Searcher_OnDoubleClick;
            TreeView_Searcher.KeyUp += TreeView_Searcher_OnKeyUp;
            // 
            // DockImportSceneTree
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(330, 519);
            Controls.Add(Tab_Explorer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HideOnClose = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(301, 39);
            Name = "DockImportSceneTree";
            TabText = "Scene Outliner";
            Text = "DockImportSceneTree";
            Tab_Explorer.ResumeLayout(false);
            TabPage_Explorer.ResumeLayout(false);
            TabPage_Explorer.PerformLayout();
            TabPage_Searcher.ResumeLayout(false);
            Split_Searcher_Root.Panel1.ResumeLayout(false);
            Split_Searcher_Root.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_Root).EndInit();
            Split_Searcher_Root.ResumeLayout(false);
            Split_Searcher_TextButton.Panel1.ResumeLayout(false);
            Split_Searcher_TextButton.Panel1.PerformLayout();
            Split_Searcher_TextButton.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_TextButton).EndInit();
            Split_Searcher_TextButton.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private Mafia2Tool.Controls.MTreeView TreeView_Explorer;
        private System.Windows.Forms.TabControl Tab_Explorer;
        private System.Windows.Forms.TabPage TabPage_Explorer;
        private System.Windows.Forms.TabPage TabPage_Searcher;
        private System.Windows.Forms.SplitContainer Split_Searcher_Root;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Button Button_Search;
        private Mafia2Tool.Controls.MTreeView TreeView_Searcher;
        private System.Windows.Forms.SplitContainer Split_Searcher_TextButton;
        public System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.Button importButton;
        public System.Windows.Forms.CheckBox importTextures;
    }
}