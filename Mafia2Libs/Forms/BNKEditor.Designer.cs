namespace Mafia2Tool
{
    partial class BNKEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BNKEditor));
            WemGrid = new System.Windows.Forms.PropertyGrid();
            TreeView_Wems = new Mafia2Tool.Controls.MTreeView();
            BnkContext = new System.Windows.Forms.ContextMenuStrip(components);
            ContextReplace = new System.Windows.Forms.ToolStripMenuItem();
            ContextEdit = new System.Windows.Forms.ToolStripMenuItem();
            ContextExport = new System.Windows.Forms.ToolStripMenuItem();
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            Toolstrip_Bnk = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            Button_ExportWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_ReplaceWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_DeleteWem = new System.Windows.Forms.ToolStripMenuItem();
            Button_ExportAll = new System.Windows.Forms.ToolStripMenuItem();
            Checkbox_Trim = new System.Windows.Forms.CheckBox();
            BnkContext.SuspendLayout();
            Toolstrip_Bnk.SuspendLayout();
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
            TreeView_Wems.ContextMenuStrip = BnkContext;
            TreeView_Wems.Location = new System.Drawing.Point(14, 32);
            TreeView_Wems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Wems.Name = "TreeView_Wems";
            TreeView_Wems.Size = new System.Drawing.Size(429, 472);
            TreeView_Wems.TabIndex = 11;
            TreeView_Wems.AfterSelect += OnNodeSelectSelect;
            TreeView_Wems.KeyUp += BnkTreeView_OnKeyUp;
            // 
            // BnkContext
            // 
            BnkContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextReplace, ContextEdit, ContextExport, ContextDelete });
            BnkContext.Name = "SDSContext";
            BnkContext.Size = new System.Drawing.Size(180, 92);
            // 
            // ContextReplace
            // 
            ContextReplace.Name = "ContextReplace";
            ContextReplace.Size = new System.Drawing.Size(179, 22);
            ContextReplace.Text = "$REPLACE_WEM";
            ContextReplace.Click += Button_ReplaceWem_Click;
            // 
            // ContextEdit
            // 
            ContextEdit.Name = "ContextEdit";
            ContextEdit.Size = new System.Drawing.Size(179, 22);
            ContextEdit.Text = "$EDIT_HIRC";
            ContextEdit.Click += ContextEdit_Click;
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
            // Toolstrip_Bnk
            // 
            Toolstrip_Bnk.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton });
            Toolstrip_Bnk.Location = new System.Drawing.Point(0, 0);
            Toolstrip_Bnk.Name = "Toolstrip_Bnk";
            Toolstrip_Bnk.Size = new System.Drawing.Size(933, 25);
            Toolstrip_Bnk.TabIndex = 15;
            Toolstrip_Bnk.Text = "Toolstrip_Bnk";
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
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_ExportWem, Button_ReplaceWem, Button_ImportWem, Button_DeleteWem, Button_ExportAll });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(49, 22);
            EditButton.Text = "$EDIT";
            // 
            // Button_ExportWem
            // 
            Button_ExportWem.Name = "Button_ExportWem";
            Button_ExportWem.Size = new System.Drawing.Size(223, 22);
            Button_ExportWem.Text = "$EXPORT_WEM";
            Button_ExportWem.Click += Button_ExportWem_Click;
            // 
            // Button_ReplaceWem
            // 
            Button_ReplaceWem.Name = "Button_ReplaceWem";
            Button_ReplaceWem.Size = new System.Drawing.Size(223, 22);
            Button_ReplaceWem.Text = "$REPLACE_WEM";
            Button_ReplaceWem.Click += Button_ReplaceWem_Click;
            // 
            // Button_ImportWem
            // 
            Button_ImportWem.Name = "Button_ImportWem";
            Button_ImportWem.Size = new System.Drawing.Size(223, 22);
            Button_ImportWem.Text = "$IMPORT_WEM";
            Button_ImportWem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            Button_ImportWem.Click += Button_ImportWem_Click;
            // 
            // Button_DeleteWem
            // 
            Button_DeleteWem.Name = "Button_DeleteWem";
            Button_DeleteWem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Button_DeleteWem.Size = new System.Drawing.Size(223, 22);
            Button_DeleteWem.Text = "$DELETE_WEM";
            Button_DeleteWem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            Button_DeleteWem.Click += Button_DeleteWem_Click;
            // 
            // Button_ExportAll
            // 
            Button_ExportAll.Name = "Button_ExportAll";
            Button_ExportAll.Size = new System.Drawing.Size(223, 22);
            Button_ExportAll.Text = "$EXPORT_ALL_WEMS";
            Button_ExportAll.Click += Button_ExportAll_Click;
            // 
            // Checkbox_Trim
            // 
            Checkbox_Trim.AutoSize = true;
            Checkbox_Trim.Location = new System.Drawing.Point(112, 3);
            Checkbox_Trim.Name = "Checkbox_Trim";
            Checkbox_Trim.Size = new System.Drawing.Size(98, 19);
            Checkbox_Trim.TabIndex = 16;
            Checkbox_Trim.Text = "$TRIM_WEMS";
            Checkbox_Trim.UseVisualStyleBackColor = true;
            // 
            // BNKEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(Checkbox_Trim);
            Controls.Add(Toolstrip_Bnk);
            Controls.Add(WemGrid);
            Controls.Add(TreeView_Wems);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BNKEditor";
            Text = "$PCK_EDITOR_TITLE";
            FormClosing += BnkEditor_Closing;
            BnkContext.ResumeLayout(false);
            Toolstrip_Bnk.ResumeLayout(false);
            Toolstrip_Bnk.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid WemGrid;
        private System.Windows.Forms.ToolStrip Toolstrip_Bnk;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip BnkContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportWem;
        private Controls.MTreeView TreeView_Wems;
        private System.Windows.Forms.ToolStripMenuItem Button_DeleteWem;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportWem;
        private System.Windows.Forms.ToolStripMenuItem ContextExport;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportAll;
        private System.Windows.Forms.ToolStripMenuItem ContextEdit;
        private System.Windows.Forms.CheckBox Checkbox_Trim;
        private System.Windows.Forms.ToolStripMenuItem ContextReplace;
        private System.Windows.Forms.ToolStripMenuItem Button_ReplaceWem;
    }
}