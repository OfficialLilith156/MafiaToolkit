namespace Toolkit.Forms
{
    partial class ATPEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ATPEditor));
            PropertyGrid_Item = new System.Windows.Forms.PropertyGrid();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_CopyData = new System.Windows.Forms.ToolStripMenuItem();
            Button_PasteData = new System.Windows.Forms.ToolStripMenuItem();
            Button_ExportXML = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportXML = new System.Windows.Forms.ToolStripMenuItem();
            Context_Menu = new System.Windows.Forms.ContextMenuStrip(components);
            ToolStrip_Copy = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Paste = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Add = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Delete = new System.Windows.Forms.ToolStripMenuItem();
            TreeView_Tables = new Mafia2Tool.Controls.MTreeView();
            FileDialog_Open = new System.Windows.Forms.OpenFileDialog();
            FileDialog_Save = new System.Windows.Forms.SaveFileDialog();
            toolStrip1.SuspendLayout();
            Context_Menu.SuspendLayout();
            SuspendLayout();
            // 
            // PropertyGrid_Item
            // 
            PropertyGrid_Item.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PropertyGrid_Item.Location = new System.Drawing.Point(469, 32);
            PropertyGrid_Item.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid_Item.Name = "PropertyGrid_Item";
            PropertyGrid_Item.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            PropertyGrid_Item.Size = new System.Drawing.Size(450, 473);
            PropertyGrid_Item.TabIndex = 10;
            PropertyGrid_Item.PropertyValueChanged += PropertyGrid_OnValueChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // Button_File
            // 
            Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Save, Button_Reload, Button_Exit });
            Button_File.Image = (System.Drawing.Image)resources.GetObject("Button_File.Image");
            Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_File.Name = "Button_File";
            Button_File.Size = new System.Drawing.Size(47, 22);
            Button_File.Text = "$FILE";
            // 
            // Button_Save
            // 
            Button_Save.Name = "Button_Save";
            Button_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            Button_Save.Size = new System.Drawing.Size(165, 22);
            Button_Save.Text = "$SAVE";
            Button_Save.Click += Button_Save_OnClick;
            // 
            // Button_Reload
            // 
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(165, 22);
            Button_Reload.Text = "$RELOAD";
            Button_Reload.Click += Button_Reload_OnClick;
            // 
            // Button_Exit
            // 
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(165, 22);
            Button_Exit.Text = "$EXIT";
            Button_Exit.Click += Button_Exit_OnClick;
            // 
            // Button_Tools
            // 
            Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_CopyData, Button_PasteData, Button_ExportXML, Button_ImportXML });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            // 
            // Button_CopyData
            // 
            Button_CopyData.Name = "Button_CopyData";
            Button_CopyData.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            Button_CopyData.Size = new System.Drawing.Size(226, 22);
            Button_CopyData.Text = "$COPY";
            Button_CopyData.Click += Button_CopyData_Click;
            // 
            // Button_PasteData
            // 
            Button_PasteData.Name = "Button_PasteData";
            Button_PasteData.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            Button_PasteData.Size = new System.Drawing.Size(226, 22);
            Button_PasteData.Text = "$PASTE";
            Button_PasteData.Click += Button_Paste_Click;
            // 
            // Button_ExportXML
            // 
            Button_ExportXML.Name = "Button_ExportXML";
            Button_ExportXML.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            Button_ExportXML.Size = new System.Drawing.Size(226, 22);
            Button_ExportXML.Text = "$EXPORT_XML";
            Button_ExportXML.Click += Button_ExportXML_Click;
            // 
            // Button_ImportXML
            // 
            Button_ImportXML.Name = "Button_ImportXML";
            Button_ImportXML.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A;
            Button_ImportXML.Size = new System.Drawing.Size(226, 22);
            Button_ImportXML.Text = "$IMPORT_XML";
            Button_ImportXML.Click += Button_ImportXML_Click;
            // 
            // Context_Menu
            // 
            Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStrip_Copy, ToolStrip_Paste, ToolStrip_Add, ToolStrip_Delete });
            Context_Menu.Name = "Context_Menu";
            Context_Menu.Size = new System.Drawing.Size(154, 92);
            Context_Menu.Opening += ContextMenu_Opening;
            // 
            // ToolStrip_Copy
            // 
            ToolStrip_Copy.Name = "ToolStrip_Copy";
            ToolStrip_Copy.ShortcutKeyDisplayString = "";
            ToolStrip_Copy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            ToolStrip_Copy.Size = new System.Drawing.Size(153, 22);
            ToolStrip_Copy.Text = "$COPY";
            ToolStrip_Copy.Click += ToolStrip_Copy_Click;
            // 
            // ToolStrip_Paste
            // 
            ToolStrip_Paste.Name = "ToolStrip_Paste";
            ToolStrip_Paste.ShortcutKeyDisplayString = "";
            ToolStrip_Paste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            ToolStrip_Paste.Size = new System.Drawing.Size(153, 22);
            ToolStrip_Paste.Text = "$PASTE";
            ToolStrip_Paste.Click += ToolStrip_Paste_Click;
            // 
            // ToolStrip_Add
            // 
            ToolStrip_Add.Name = "ToolStrip_Add";
            ToolStrip_Add.Size = new System.Drawing.Size(153, 22);
            ToolStrip_Add.Text = "$ADD";
            ToolStrip_Add.Click += ATPTreeView_OnAdd;
            // 
            // ToolStrip_Delete
            // 
            ToolStrip_Delete.Name = "ToolStrip_Delete";
            ToolStrip_Delete.Size = new System.Drawing.Size(153, 22);
            ToolStrip_Delete.Text = "$DELETE";
            ToolStrip_Delete.Click += ATPTreeView_OnDelete;
            // 
            // TreeView_Tables
            // 
            TreeView_Tables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_Tables.ContextMenuStrip = Context_Menu;
            TreeView_Tables.Location = new System.Drawing.Point(14, 32);
            TreeView_Tables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Tables.Name = "TreeView_Tables";
            TreeView_Tables.Size = new System.Drawing.Size(429, 472);
            TreeView_Tables.TabIndex = 11;
            TreeView_Tables.AfterSelect += OnNodeSelectSelect;
            // 
            // FileDialog_Open
            // 
            FileDialog_Open.DefaultExt = "xml";
            FileDialog_Open.Filter = "XML|*xml";
            FileDialog_Open.Title = "$OPEN_FILE";
            // 
            // FileDialog_Save
            // 
            FileDialog_Save.DefaultExt = "xml";
            FileDialog_Save.Filter = "XML|.xml";
            FileDialog_Save.Title = "$SAVE_FILE";
            // 
            // ATPEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(toolStrip1);
            Controls.Add(PropertyGrid_Item);
            Controls.Add(TreeView_Tables);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ATPEditor";
            Text = "$ATP_EDITOR_TITLE";
            FormClosing += ATPEditor_Closing;
            KeyUp += ATPEditor_OnKeyUp;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            Context_Menu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid_Item;
        private Mafia2Tool.Controls.MTreeView TreeView_Tables;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_CopyData;
        private System.Windows.Forms.ToolStripMenuItem Button_PasteData;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Copy;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Paste;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportXML;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportXML;
        private System.Windows.Forms.OpenFileDialog FileDialog_Open;
        private System.Windows.Forms.SaveFileDialog FileDialog_Save;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Add;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Delete;
    }
}