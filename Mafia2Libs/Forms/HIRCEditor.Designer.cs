namespace Mafia2Tool
{
    partial class HIRCEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIRCEditor));
            HircGrid = new System.Windows.Forms.PropertyGrid();
            BnkContext = new System.Windows.Forms.ContextMenuStrip(components);
            ContextExport = new System.Windows.Forms.ToolStripMenuItem();
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            TreeView_HIRC = new System.Windows.Forms.TreeView();
            BnkContext.SuspendLayout();
            SuspendLayout();
            // 
            // HircGrid
            // 
            HircGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            HircGrid.Location = new System.Drawing.Point(469, 32);
            HircGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            HircGrid.Name = "HircGrid";
            HircGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            HircGrid.Size = new System.Drawing.Size(450, 473);
            HircGrid.TabIndex = 10;
            HircGrid.PropertyValueChanged += WemGrid_OnPropertyValueChanged;
            // 
            // BnkContext
            // 
            BnkContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextExport, ContextDelete });
            BnkContext.Name = "SDSContext";
            BnkContext.Size = new System.Drawing.Size(180, 48);
            // 
            // ContextExport
            // 
            ContextExport.Name = "ContextExport";
            ContextExport.Size = new System.Drawing.Size(179, 22);
            ContextExport.Text = "$EXPORT_WEM";
            // 
            // ContextDelete
            // 
            ContextDelete.Name = "ContextDelete";
            ContextDelete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            ContextDelete.Size = new System.Drawing.Size(179, 22);
            ContextDelete.Text = "Delete";
            // 
            // TreeView_HIRC
            // 
            TreeView_HIRC.Location = new System.Drawing.Point(14, 32);
            TreeView_HIRC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_HIRC.Name = "TreeView_HIRC";
            TreeView_HIRC.Size = new System.Drawing.Size(429, 472);
            TreeView_HIRC.TabIndex = 11;
            TreeView_HIRC.AfterSelect += OnNodeSelectSelect;
            // 
            // HIRCEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(TreeView_HIRC);
            Controls.Add(HircGrid);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "HIRCEditor";
            Text = "$HIRC_EDITOR_TITLE";
            Load += HIRCEditor_Load;
            BnkContext.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid HircGrid;
        private System.Windows.Forms.ContextMenuStrip BnkContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private Controls.MTreeView TreeView_Wems;
        private System.Windows.Forms.ToolStripMenuItem ContextExport;
        private System.Windows.Forms.TreeView TreeView_HIRC;
    }
}