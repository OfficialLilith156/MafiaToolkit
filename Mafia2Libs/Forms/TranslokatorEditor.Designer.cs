namespace Mafia2Tool.Forms
{
    partial class TranslokatorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslokatorEditor));
            PropertyGrid = new System.Windows.Forms.PropertyGrid();
            TranslokatorTree = new Mafia2Tool.Controls.MTreeView();
            TranslokatorContext = new System.Windows.Forms.ContextMenuStrip(components);
            AddInstance = new System.Windows.Forms.ToolStripMenuItem();
            AddGroup = new System.Windows.Forms.ToolStripMenuItem();
            AddObject = new System.Windows.Forms.ToolStripMenuItem();
            Delete = new System.Windows.Forms.ToolStripMenuItem();
            CopyButton = new System.Windows.Forms.ToolStripMenuItem();
            PasteButton = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveToolButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            ToolsButton = new System.Windows.Forms.ToolStripDropDownButton();
            ViewNumInstButton = new System.Windows.Forms.ToolStripMenuItem();
            Button_ExportXml = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportXml = new System.Windows.Forms.ToolStripMenuItem();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            SearchBox = new System.Windows.Forms.TextBox();
            TranslokatorContext.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PropertyGrid
            // 
            PropertyGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PropertyGrid.Location = new System.Drawing.Point(451, 32);
            PropertyGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid.Name = "PropertyGrid";
            PropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            PropertyGrid.Size = new System.Drawing.Size(468, 480);
            PropertyGrid.TabIndex = 16;
            PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            // 
            // TranslokatorTree
            // 
            TranslokatorTree.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TranslokatorTree.ContextMenuStrip = TranslokatorContext;
            TranslokatorTree.HideSelection = false;
            TranslokatorTree.Location = new System.Drawing.Point(14, 32);
            TranslokatorTree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TranslokatorTree.Name = "TranslokatorTree";
            TranslokatorTree.Size = new System.Drawing.Size(429, 479);
            TranslokatorTree.TabIndex = 17;
            TranslokatorTree.AfterSelect += TranslokatorTree_AfterSelect;
            TranslokatorTree.KeyUp += OnKeyUp;
            // 
            // TranslokatorContext
            // 
            TranslokatorContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddInstance, AddGroup, AddObject, Delete, CopyButton, PasteButton });
            TranslokatorContext.Name = "SDSContext";
            TranslokatorContext.Size = new System.Drawing.Size(212, 136);
            TranslokatorContext.Opening += TranslokatorContext_Opening;
            // 
            // AddInstance
            // 
            AddInstance.Name = "AddInstance";
            AddInstance.ShortcutKeyDisplayString = "Ctrl + A";
            AddInstance.Size = new System.Drawing.Size(211, 22);
            AddInstance.Text = "$ADD_INSTANCE";
            AddInstance.Click += AddInstance_Click;
            // 
            // AddGroup
            // 
            AddGroup.Name = "AddGroup";
            AddGroup.ShortcutKeyDisplayString = "Ctrl + A";
            AddGroup.Size = new System.Drawing.Size(211, 22);
            AddGroup.Text = "$ADD_GROUP";
            AddGroup.Click += OnAddGroupPressed;
            // 
            // AddObject
            // 
            AddObject.Name = "AddObject";
            AddObject.ShortcutKeyDisplayString = "Ctrl + A";
            AddObject.Size = new System.Drawing.Size(211, 22);
            AddObject.Text = "$ADD_OBJECT";
            AddObject.Click += AddObjectOnClick;
            // 
            // Delete
            // 
            Delete.Name = "Delete";
            Delete.ShortcutKeyDisplayString = "";
            Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Delete.Size = new System.Drawing.Size(211, 22);
            Delete.Text = "$DELETE";
            Delete.Click += Delete_Click;
            // 
            // CopyButton
            // 
            CopyButton.Name = "CopyButton";
            CopyButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            CopyButton.Size = new System.Drawing.Size(211, 22);
            CopyButton.Text = "$COPY";
            CopyButton.Click += CopyButton_Click;
            // 
            // PasteButton
            // 
            PasteButton.Name = "PasteButton";
            PasteButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            PasteButton.Size = new System.Drawing.Size(211, 22);
            PasteButton.Text = "$PASTE";
            PasteButton.Click += PasteButton_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolButton, ToolsButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 18;
            toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveToolButton, ReloadButton, ExitButton });
            fileToolButton.Image = (System.Drawing.Image)resources.GetObject("fileToolButton.Image");
            fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            fileToolButton.Name = "fileToolButton";
            fileToolButton.Size = new System.Drawing.Size(47, 22);
            fileToolButton.Text = "$FILE";
            // 
            // SaveToolButton
            // 
            SaveToolButton.Name = "SaveToolButton";
            SaveToolButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveToolButton.Size = new System.Drawing.Size(180, 22);
            SaveToolButton.Text = "$SAVE";
            SaveToolButton.Click += SaveToolButton_Click;
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
            // ToolsButton
            // 
            ToolsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            ToolsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ViewNumInstButton, Button_ExportXml, Button_ImportXml });
            ToolsButton.Image = (System.Drawing.Image)resources.GetObject("ToolsButton.Image");
            ToolsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolsButton.Name = "ToolsButton";
            ToolsButton.Size = new System.Drawing.Size(61, 22);
            ToolsButton.Text = "$TOOLS";
            // 
            // ViewNumInstButton
            // 
            ViewNumInstButton.Name = "ViewNumInstButton";
            ViewNumInstButton.Size = new System.Drawing.Size(230, 22);
            ViewNumInstButton.Text = "$VIEW_NUM_INST";
            ViewNumInstButton.Click += ViewNumInstButton_Click;
            // 
            // Button_ExportXml
            // 
            Button_ExportXml.Name = "Button_ExportXml";
            Button_ExportXml.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            Button_ExportXml.Size = new System.Drawing.Size(230, 22);
            Button_ExportXml.Text = "$EXPORT_XML";
            Button_ExportXml.Click += Button_ExportXml_OnClick;
            // 
            // Button_ImportXml
            // 
            Button_ImportXml.Name = "Button_ImportXml";
            Button_ImportXml.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            Button_ImportXml.Size = new System.Drawing.Size(230, 22);
            Button_ImportXml.Text = "$IMPORT_XML";
            Button_ImportXml.Click += Button_ImportXml_OnClick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // SearchBox
            // 
            SearchBox.Location = new System.Drawing.Point(291, 5);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(152, 23);
            SearchBox.TabIndex = 19;
            // 
            // TranslokatorEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(SearchBox);
            Controls.Add(PropertyGrid);
            Controls.Add(TranslokatorTree);
            Controls.Add(toolStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "TranslokatorEditor";
            Text = "$TRANSLOKATOR_EDITOR";
            FormClosing += TranslocatorEditor_Closing;
            TranslokatorContext.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.ContextMenuStrip TranslokatorContext;
        private System.Windows.Forms.ToolStripMenuItem AddInstance;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem SaveToolButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportXml;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportXml;
        private System.Windows.Forms.ToolStripMenuItem AddObject;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripMenuItem CopyButton;
        private System.Windows.Forms.ToolStripMenuItem PasteButton;
        private System.Windows.Forms.ToolStripDropDownButton ToolsButton;
        private System.Windows.Forms.ToolStripMenuItem ViewNumInstButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Controls.MTreeView TranslokatorTree;
        private System.Windows.Forms.ToolStripMenuItem AddGroup;
        private System.Windows.Forms.TextBox SearchBox;
    }
}