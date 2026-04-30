namespace Mafia2Tool
{
    partial class PCKEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PCKEditor));
            WemGrid = new System.Windows.Forms.PropertyGrid();
            TreeView_Wems = new Mafia2Tool.Controls.MTreeView();
            PckContext = new System.Windows.Forms.ContextMenuStrip(components);
            ContextReplace = new System.Windows.Forms.ToolStripMenuItem();
            ContextExport = new System.Windows.Forms.ToolStripMenuItem();
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            ContextEditHIRC = new System.Windows.Forms.ToolStripMenuItem();
            ContextLoadHIRC = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Pck = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            Button_ReplaceWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_ExportWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_DeleteWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_ExportAll = new System.Windows.Forms.ToolStripMenuItem();
            Button_LoadHIRC = new System.Windows.Forms.ToolStripMenuItem();
            PckContext.SuspendLayout();
            ToolStrip_Pck.SuspendLayout();
            SuspendLayout();
            // 
            // WemGrid
            // 
            WemGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            WemGrid.Location = new System.Drawing.Point(469, 32);
            WemGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WemGrid.Name = "WemGrid";
            WemGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            WemGrid.Size = new System.Drawing.Size(450, 473);
            WemGrid.TabIndex = 10;
            WemGrid.PropertyValueChanged += WemGrid_OnPropertyValueChanged;
            // 
            // TreeView_Wems
            // 
            TreeView_Wems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_Wems.ContextMenuStrip = PckContext;
            TreeView_Wems.Location = new System.Drawing.Point(14, 32);
            TreeView_Wems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Wems.Name = "TreeView_Wems";
            TreeView_Wems.Size = new System.Drawing.Size(429, 472);
            TreeView_Wems.TabIndex = 11;
            TreeView_Wems.AfterSelect += OnNodeSelectSelect;
            TreeView_Wems.KeyUp += PckTreeView_OnKeyUp;
            // 
            // PckContext
            // 
            PckContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextReplace, ContextExport, ContextDelete, ContextEditHIRC, ContextLoadHIRC });
            PckContext.Name = "SDSContext";
            PckContext.Size = new System.Drawing.Size(180, 114);
            PckContext.Opening += Context_Opening;
            // 
            // ContextReplace
            // 
            ContextReplace.Name = "ContextReplace";
            ContextReplace.Size = new System.Drawing.Size(179, 22);
            ContextReplace.Text = "$REPLACE_WEM";
            ContextReplace.Click += Button_ReplaceWem_Click;
            // 
            // ContextExport
            // 
            ContextExport.Name = "ContextExport";
            ContextExport.Size = new System.Drawing.Size(179, 22);
            ContextExport.Text = "$EXPORT_WEM";
            ContextExport.Click += Button_ExportWem_Click;
            // 
            // ContextDelete
            // 
            ContextDelete.Name = "ContextDelete";
            ContextDelete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            ContextDelete.Size = new System.Drawing.Size(179, 22);
            ContextDelete.Text = "Delete";
            ContextDelete.Click += ContextDelete_Click;
            // 
            // ContextEditHIRC
            // 
            ContextEditHIRC.Name = "ContextEditHIRC";
            ContextEditHIRC.Size = new System.Drawing.Size(179, 22);
            ContextEditHIRC.Text = "$EDIT_HIRC";
            ContextEditHIRC.Click += Button_EditHIRC_Click;
            // 
            // ContextLoadHIRC
            // 
            ContextLoadHIRC.Name = "ContextLoadHIRC";
            ContextLoadHIRC.Size = new System.Drawing.Size(179, 22);
            ContextLoadHIRC.Text = "$LOAD_HIRC";
            ContextLoadHIRC.Click += Button_LoadHIRC_Click;
            // 
            // ToolStrip_Pck
            // 
            ToolStrip_Pck.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton });
            ToolStrip_Pck.Location = new System.Drawing.Point(0, 0);
            ToolStrip_Pck.Name = "ToolStrip_Pck";
            ToolStrip_Pck.Size = new System.Drawing.Size(933, 25);
            ToolStrip_Pck.TabIndex = 15;
            ToolStrip_Pck.Text = "ToolStrip_Pck";
            // 
            // FileButton
            // 
            FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ReloadButton, ExitButton });
            FileButton.Image = (System.Drawing.Image)resources.GetObject("FileButton.Image");
            FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FileButton.Name = "FileButton";
            FileButton.Size = new System.Drawing.Size(47, 22);
            FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            SaveButton.Name = "SaveButton";
            SaveButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveButton.Size = new System.Drawing.Size(165, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveButton_OnClick;
            // 
            // ReloadButton
            // 
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(165, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadButton_OnClick;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(165, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_OnClick;
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_ReplaceWem, Button_ImportWem, Button_ExportWem, Button_DeleteWem, Button_ExportAll, Button_LoadHIRC });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(49, 22);
            EditButton.Text = "$EDIT";
            // 
            // Button_ReplaceWem
            // 
            Button_ReplaceWem.Name = "Button_ReplaceWem";
            Button_ReplaceWem.Size = new System.Drawing.Size(185, 22);
            Button_ReplaceWem.Text = "$REPLACE_WEM";
            Button_ReplaceWem.Click += Button_ReplaceWem_Click;
            // 
            // Button_ImportWem
            // 
            Button_ImportWem.Name = "Button_ImportWem";
            Button_ImportWem.Size = new System.Drawing.Size(185, 22);
            Button_ImportWem.Text = "$IMPORT_WEM";
            Button_ImportWem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            Button_ImportWem.Click += Button_ImportWem_Click;
            // 
            // Button_ExportWem
            // 
            Button_ExportWem.Name = "Button_ExportWem";
            Button_ExportWem.Size = new System.Drawing.Size(185, 22);
            Button_ExportWem.Text = "$EXPORT_WEM";
            Button_ExportWem.Click += Button_ExportWem_Click;
            // 
            // Button_DeleteWem
            // 
            Button_DeleteWem.Name = "Button_DeleteWem";
            Button_DeleteWem.Size = new System.Drawing.Size(185, 22);
            Button_DeleteWem.Text = "$DELETE_WEM";
            Button_DeleteWem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            Button_DeleteWem.Click += Button_DeleteWem_Click;
            // 
            // Button_ExportAll
            // 
            Button_ExportAll.Name = "Button_ExportAll";
            Button_ExportAll.Size = new System.Drawing.Size(185, 22);
            Button_ExportAll.Text = "$EXPORT_ALL_WEMS";
            Button_ExportAll.Click += Button_ExportAll_Click;
            // 
            // Button_LoadHIRC
            // 
            Button_LoadHIRC.Name = "Button_LoadHIRC";
            Button_LoadHIRC.Size = new System.Drawing.Size(185, 22);
            Button_LoadHIRC.Text = "$LOAD_HIRC";
            Button_LoadHIRC.Click += Button_LoadHIRC_Click;
            // 
            // PCKEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(ToolStrip_Pck);
            Controls.Add(WemGrid);
            Controls.Add(TreeView_Wems);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "PCKEditor";
            Text = "$PCK_EDITOR_TITLE";
            FormClosing += PckEditor_Closing;
            FormClosed += PckEditor_FormClosed;
            PckContext.ResumeLayout(false);
            ToolStrip_Pck.ResumeLayout(false);
            ToolStrip_Pck.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid WemGrid;
        private System.Windows.Forms.ToolStrip ToolStrip_Pck;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip PckContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportWem;
        private Controls.MTreeView TreeView_Wems;
        private System.Windows.Forms.ToolStripMenuItem Button_DeleteWem;
        private System.Windows.Forms.ToolStripMenuItem ContextExport;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportWem;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportAll;
        private System.Windows.Forms.ToolStripMenuItem ContextEdit;
        private System.Windows.Forms.ToolStripMenuItem ContextLoad;
        private System.Windows.Forms.ToolStripMenuItem ContextEditHIRC;
        private System.Windows.Forms.ToolStripMenuItem ContextLoadHIRC;
        private System.Windows.Forms.ToolStripMenuItem Button_LoadHIRC;
        private System.Windows.Forms.ToolStripMenuItem ContextReplace;
        private System.Windows.Forms.ToolStripMenuItem Button_ReplaceWem;
    }
}