using System.Windows.Forms;

namespace Mafia2Tool
{
    partial class StreamEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamEditor));
            linesTree = new TreeView();
            LineContextStrip = new ContextMenuStrip(components);
            AddLineButton = new ToolStripMenuItem();
            DeleteLineButton = new ToolStripMenuItem();
            DuplicateLine = new ToolStripMenuItem();
            MoveItemUpButton = new ToolStripMenuItem();
            MoveItemDownButton = new ToolStripMenuItem();
            ToolStrip = new ToolStrip();
            fileToolButton = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            reloadToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            Button_Tools = new ToolStripDropDownButton();
            Button_CreateLineGroup = new ToolStripMenuItem();
            Button_CreateStreamGroup = new ToolStripMenuItem();
            PropertyGrid_Stream = new PropertyGrid();
            tabControl = new TabControl();
            StreamLinesPage = new TabPage();
            StreamGroupPage = new TabPage();
            groupTree = new Mafia2Tool.Controls.MTreeView();
            StreamBlocksPage = new TabPage();
            blockView = new Mafia2Tool.Controls.MTreeView();
            SearchBox = new TextBox();
            LineContextStrip.SuspendLayout();
            ToolStrip.SuspendLayout();
            tabControl.SuspendLayout();
            StreamLinesPage.SuspendLayout();
            StreamGroupPage.SuspendLayout();
            StreamBlocksPage.SuspendLayout();
            SuspendLayout();
            // 
            // linesTree
            // 
            linesTree.ContextMenuStrip = LineContextStrip;
            linesTree.Dock = DockStyle.Fill;
            linesTree.FullRowSelect = true;
            linesTree.HideSelection = false;
            linesTree.Location = new System.Drawing.Point(0, 0);
            linesTree.Margin = new Padding(4, 3, 4, 3);
            linesTree.Name = "linesTree";
            linesTree.Size = new System.Drawing.Size(279, 412);
            linesTree.TabIndex = 11;
            linesTree.AfterSelect += OnNodeSelectSelect;
            linesTree.KeyUp += OnKeyUp;
            // 
            // LineContextStrip
            // 
            LineContextStrip.Items.AddRange(new ToolStripItem[] { AddLineButton, DeleteLineButton, DuplicateLine, MoveItemUpButton, MoveItemDownButton });
            LineContextStrip.Name = "AddLineButton";
            LineContextStrip.Size = new System.Drawing.Size(221, 114);
            LineContextStrip.Text = "Context Strip";
            LineContextStrip.Opening += OnContextMenuOpening;
            // 
            // AddLineButton
            // 
            AddLineButton.Name = "AddLineButton";
            AddLineButton.ShortcutKeys = Keys.Control | Keys.N;
            AddLineButton.Size = new System.Drawing.Size(220, 22);
            AddLineButton.Text = "$ADD_LINE";
            AddLineButton.Click += AddLineButtonPressed;
            // 
            // DeleteLineButton
            // 
            DeleteLineButton.Name = "DeleteLineButton";
            DeleteLineButton.ShortcutKeys = Keys.Control | Keys.Delete;
            DeleteLineButton.Size = new System.Drawing.Size(220, 22);
            DeleteLineButton.Text = "$DELETE_LINE";
            DeleteLineButton.Click += DeleteLineButtonPressed;
            // 
            // DuplicateLine
            // 
            DuplicateLine.Name = "DuplicateLine";
            DuplicateLine.ShortcutKeys = Keys.Control | Keys.D;
            DuplicateLine.Size = new System.Drawing.Size(220, 22);
            DuplicateLine.Text = "$DUPLICATE_LINE";
            DuplicateLine.Click += CopyLoadListAbove_Click;
            // 
            // MoveItemUpButton
            // 
            MoveItemUpButton.Name = "MoveItemUpButton";
            MoveItemUpButton.ShortcutKeys = Keys.Control | Keys.Up;
            MoveItemUpButton.Size = new System.Drawing.Size(220, 22);
            MoveItemUpButton.Text = "$MOVE_UP";
            MoveItemUpButton.Click += MoveItemUp_Click;
            // 
            // MoveItemDownButton
            // 
            MoveItemDownButton.Name = "MoveItemDownButton";
            MoveItemDownButton.ShortcutKeys = Keys.Control | Keys.Down;
            MoveItemDownButton.Size = new System.Drawing.Size(220, 22);
            MoveItemDownButton.Text = "$MOVE_DOWN";
            MoveItemDownButton.Click += MoveItemDown_Click;
            // 
            // ToolStrip
            // 
            ToolStrip.Items.AddRange(new ToolStripItem[] { fileToolButton, Button_Tools });
            ToolStrip.Location = new System.Drawing.Point(0, 0);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.Size = new System.Drawing.Size(1090, 25);
            ToolStrip.TabIndex = 15;
            ToolStrip.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            fileToolButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            fileToolButton.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, reloadToolStripMenuItem, exitToolStripMenuItem });
            fileToolButton.Image = (System.Drawing.Image)resources.GetObject("fileToolButton.Image");
            fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            fileToolButton.Name = "fileToolButton";
            fileToolButton.Size = new System.Drawing.Size(47, 22);
            fileToolButton.Text = "$FILE";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            saveToolStripMenuItem.Text = "$SAVE";
            saveToolStripMenuItem.Click += SaveButtonPressed;
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            reloadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            reloadToolStripMenuItem.Text = "$RELOAD";
            reloadToolStripMenuItem.Click += ReloadButtonPressed;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            exitToolStripMenuItem.Text = "$EXIT";
            exitToolStripMenuItem.Click += ExitButtonPressed;
            // 
            // Button_Tools
            // 
            Button_Tools.DisplayStyle = ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new ToolStripItem[] { Button_CreateLineGroup, Button_CreateStreamGroup });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            // 
            // Button_CreateLineGroup
            // 
            Button_CreateLineGroup.Name = "Button_CreateLineGroup";
            Button_CreateLineGroup.ShortcutKeys = Keys.Control | Keys.A;
            Button_CreateLineGroup.Size = new System.Drawing.Size(293, 22);
            Button_CreateLineGroup.Text = "$CREATE_LINE_GROUP";
            Button_CreateLineGroup.Click += Button_CreateLineGroup_Click;
            // 
            // Button_CreateStreamGroup
            // 
            Button_CreateStreamGroup.Name = "Button_CreateStreamGroup";
            Button_CreateStreamGroup.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
            Button_CreateStreamGroup.Size = new System.Drawing.Size(293, 22);
            Button_CreateStreamGroup.Text = "$CREATE_STREAM_GROUP";
            Button_CreateStreamGroup.Click += Button_CreateStreamGroup_Click;
            // 
            // PropertyGrid_Stream
            // 
            PropertyGrid_Stream.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PropertyGrid_Stream.Location = new System.Drawing.Point(303, 32);
            PropertyGrid_Stream.Margin = new Padding(4, 3, 4, 3);
            PropertyGrid_Stream.Name = "PropertyGrid_Stream";
            PropertyGrid_Stream.PropertySort = PropertySort.Categorized;
            PropertyGrid_Stream.Size = new System.Drawing.Size(772, 473);
            PropertyGrid_Stream.TabIndex = 10;
            PropertyGrid_Stream.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tabControl.Controls.Add(StreamLinesPage);
            tabControl.Controls.Add(StreamGroupPage);
            tabControl.Controls.Add(StreamBlocksPage);
            tabControl.Location = new System.Drawing.Point(14, 66);
            tabControl.Margin = new Padding(4, 3, 4, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(287, 440);
            tabControl.TabIndex = 16;
            // 
            // StreamLinesPage
            // 
            StreamLinesPage.Controls.Add(linesTree);
            StreamLinesPage.Location = new System.Drawing.Point(4, 24);
            StreamLinesPage.Margin = new Padding(4, 3, 4, 3);
            StreamLinesPage.Name = "StreamLinesPage";
            StreamLinesPage.Size = new System.Drawing.Size(279, 412);
            StreamLinesPage.TabIndex = 0;
            StreamLinesPage.Text = "Stream Lines";
            StreamLinesPage.UseVisualStyleBackColor = true;
            // 
            // StreamGroupPage
            // 
            StreamGroupPage.Controls.Add(groupTree);
            StreamGroupPage.Location = new System.Drawing.Point(4, 24);
            StreamGroupPage.Margin = new Padding(4, 3, 4, 3);
            StreamGroupPage.Name = "StreamGroupPage";
            StreamGroupPage.Size = new System.Drawing.Size(279, 412);
            StreamGroupPage.TabIndex = 1;
            StreamGroupPage.Text = "Stream Groups";
            StreamGroupPage.UseVisualStyleBackColor = true;
            // 
            // groupTree
            // 
            groupTree.Dock = DockStyle.Fill;
            groupTree.Location = new System.Drawing.Point(0, 0);
            groupTree.Margin = new Padding(4, 3, 4, 3);
            groupTree.Name = "groupTree";
            groupTree.Size = new System.Drawing.Size(279, 412);
            groupTree.TabIndex = 13;
            groupTree.AfterSelect += OnNodeSelectSelect;
            // 
            // StreamBlocksPage
            // 
            StreamBlocksPage.Controls.Add(blockView);
            StreamBlocksPage.Location = new System.Drawing.Point(4, 24);
            StreamBlocksPage.Margin = new Padding(4, 3, 4, 3);
            StreamBlocksPage.Name = "StreamBlocksPage";
            StreamBlocksPage.Size = new System.Drawing.Size(279, 412);
            StreamBlocksPage.TabIndex = 2;
            StreamBlocksPage.Text = "Stream Blocks";
            StreamBlocksPage.UseVisualStyleBackColor = true;
            // 
            // blockView
            // 
            blockView.Dock = DockStyle.Fill;
            blockView.Location = new System.Drawing.Point(0, 0);
            blockView.Margin = new Padding(4, 3, 4, 3);
            blockView.Name = "blockView";
            blockView.Size = new System.Drawing.Size(279, 412);
            blockView.TabIndex = 14;
            blockView.AfterSelect += OnNodeSelectSelect;
            // 
            // SearchBox
            // 
            SearchBox.Location = new System.Drawing.Point(14, 36);
            SearchBox.Margin = new Padding(4, 3, 4, 3);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(282, 23);
            SearchBox.TabIndex = 28;
            SearchBox.KeyPress += OnKeyPressed;
            // 
            // StreamEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1090, 519);
            Controls.Add(SearchBox);
            Controls.Add(ToolStrip);
            Controls.Add(PropertyGrid_Stream);
            Controls.Add(tabControl);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "StreamEditor";
            Text = "$STREAM_MAP_EDITOR_TITLE";
            FormClosing += StreamEditor_Closing;
            LineContextStrip.ResumeLayout(false);
            ToolStrip.ResumeLayout(false);
            ToolStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            StreamLinesPage.ResumeLayout(false);
            StreamGroupPage.ResumeLayout(false);
            StreamBlocksPage.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView linesTree;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Stream;
        private System.Windows.Forms.TabControl tabControl;
        private TabPage StreamLinesPage;
        private TabPage StreamGroupPage;
        private TabPage StreamBlocksPage;
        private Mafia2Tool.Controls.MTreeView groupTree;
        private Mafia2Tool.Controls.MTreeView blockView;
        private ContextMenuStrip LineContextStrip;
        private ToolStripMenuItem AddLineButton;
        private ToolStripMenuItem DeleteLineButton;
        private TextBox SearchBox;
        private ToolStripMenuItem MoveItemUpButton;
        private ToolStripMenuItem MoveItemDownButton;
        private ToolStripMenuItem DuplicateLine;
        private ToolStripDropDownButton Button_Tools;
        private ToolStripMenuItem Button_CreateLineGroup;
        private ToolStripMenuItem Button_CreateStreamGroup;
    }
}