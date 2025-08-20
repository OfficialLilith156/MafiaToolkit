namespace Toolkit.Forms
{
    partial class FxAnimSetEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FxAnimSetEditor));
            Grid_AnimSet = new System.Windows.Forms.PropertyGrid();
            ToolStrip_Top = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Import = new System.Windows.Forms.ToolStripMenuItem();
            Button_Export = new System.Windows.Forms.ToolStripMenuItem();
            Button_Copy = new System.Windows.Forms.ToolStripMenuItem();
            Button_Paste = new System.Windows.Forms.ToolStripMenuItem();
            Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Context_Menu = new System.Windows.Forms.ContextMenuStrip(components);
            Context_Export = new System.Windows.Forms.ToolStripMenuItem();
            Context_Copy = new System.Windows.Forms.ToolStripMenuItem();
            Context_Paste = new System.Windows.Forms.ToolStripMenuItem();
            Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            TreeView_FxAnimSets = new Mafia2Tool.Controls.MTreeView();
            SearchBox = new System.Windows.Forms.TextBox();
            ToolStrip_Top.SuspendLayout();
            Context_Menu.SuspendLayout();
            SuspendLayout();
            // 
            // Grid_AnimSet
            // 
            Grid_AnimSet.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Grid_AnimSet.Location = new System.Drawing.Point(469, 32);
            Grid_AnimSet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Grid_AnimSet.Name = "Grid_AnimSet";
            Grid_AnimSet.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            Grid_AnimSet.Size = new System.Drawing.Size(450, 473);
            Grid_AnimSet.TabIndex = 10;
            Grid_AnimSet.PropertyValueChanged += Grid_AnimSet_PropertyChanged;
            // 
            // ToolStrip_Top
            // 
            ToolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            ToolStrip_Top.Location = new System.Drawing.Point(0, 0);
            ToolStrip_Top.Name = "ToolStrip_Top";
            ToolStrip_Top.Size = new System.Drawing.Size(933, 25);
            ToolStrip_Top.TabIndex = 15;
            ToolStrip_Top.Text = "ToolStrip";
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
            Button_Save.Click += Button_Save_Click;
            // 
            // Button_Reload
            // 
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(165, 22);
            Button_Reload.Text = "$RELOAD";
            Button_Reload.Click += Button_Reload_Click;
            // 
            // Button_Exit
            // 
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(165, 22);
            Button_Exit.Text = "$EXIT";
            Button_Exit.Click += Button_Exit_Click;
            // 
            // Button_Tools
            // 
            Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Import, Button_Export, Button_Copy, Button_Paste, Button_Delete });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            // 
            // Button_Import
            // 
            Button_Import.Name = "Button_Import";
            Button_Import.Size = new System.Drawing.Size(190, 22);
            Button_Import.Text = "$IMPORT";
            Button_Import.Click += Button_Import_Click;
            // 
            // Button_Export
            // 
            Button_Export.Name = "Button_Export";
            Button_Export.Size = new System.Drawing.Size(190, 22);
            Button_Export.Text = "$EXPORT";
            Button_Export.Click += Button_Export_Click;
            // 
            // Button_Copy
            // 
            Button_Copy.Name = "Button_Copy";
            Button_Copy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            Button_Copy.Size = new System.Drawing.Size(190, 22);
            Button_Copy.Text = "$COPY";
            Button_Copy.Click += Button_Copy_Click;
            // 
            // Button_Paste
            // 
            Button_Paste.Name = "Button_Paste";
            Button_Paste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            Button_Paste.Size = new System.Drawing.Size(190, 22);
            Button_Paste.Text = "$PASTE";
            Button_Paste.Click += Button_Paste_Click;
            // 
            // Button_Delete
            // 
            Button_Delete.Name = "Button_Delete";
            Button_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Button_Delete.Size = new System.Drawing.Size(190, 22);
            Button_Delete.Text = "$DELETE";
            Button_Delete.Click += Button_Delete_Click;
            // 
            // Context_Menu
            // 
            Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Context_Export, Context_Copy, Context_Paste, Context_Delete });
            Context_Menu.Name = "Context_Menu";
            Context_Menu.Size = new System.Drawing.Size(191, 92);
            // 
            // Context_Export
            // 
            Context_Export.Name = "Context_Export";
            Context_Export.Size = new System.Drawing.Size(190, 22);
            Context_Export.Text = "$EXPORT";
            Context_Export.Click += Context_Export_Click;
            // 
            // Context_Copy
            // 
            Context_Copy.Name = "Context_Copy";
            Context_Copy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            Context_Copy.Size = new System.Drawing.Size(190, 22);
            Context_Copy.Text = "$COPY";
            Context_Copy.Click += Context_Copy_Click;
            // 
            // Context_Paste
            // 
            Context_Paste.Name = "Context_Paste";
            Context_Paste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            Context_Paste.Size = new System.Drawing.Size(190, 22);
            Context_Paste.Text = "$PASTE";
            Context_Paste.Click += Context_Paste_Click;
            // 
            // Context_Delete
            // 
            Context_Delete.Name = "Context_Delete";
            Context_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Context_Delete.Size = new System.Drawing.Size(190, 22);
            Context_Delete.Text = "$DELETE";
            Context_Delete.Click += Context_Delete_Click;
            // 
            // TreeView_FxAnimSets
            // 
            TreeView_FxAnimSets.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_FxAnimSets.ContextMenuStrip = Context_Menu;
            TreeView_FxAnimSets.Location = new System.Drawing.Point(14, 32);
            TreeView_FxAnimSets.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_FxAnimSets.Name = "TreeView_FxAnimSets";
            TreeView_FxAnimSets.Size = new System.Drawing.Size(429, 472);
            TreeView_FxAnimSets.TabIndex = 11;
            TreeView_FxAnimSets.AfterSelect += TreeView_FxAnimSets_AfterSelect;
            // 
            // SearchBox
            // 
            SearchBox.Location = new System.Drawing.Point(291, 5);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(152, 23);
            SearchBox.TabIndex = 17;
            // 
            // FxAnimSetEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(SearchBox);
            Controls.Add(ToolStrip_Top);
            Controls.Add(Grid_AnimSet);
            Controls.Add(TreeView_FxAnimSets);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FxAnimSetEditor";
            Text = "$FXANIMSET_EDITOR";
            FormClosing += FxAnimSetEditor_Closing;
            KeyUp += FxAnimSetEditor_OnKeyUp;
            ToolStrip_Top.ResumeLayout(false);
            ToolStrip_Top.PerformLayout();
            Context_Menu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_AnimSet;
        private System.Windows.Forms.ToolStrip ToolStrip_Top;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private Mafia2Tool.Controls.MTreeView TreeView_FxAnimSets;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_Copy;
        private System.Windows.Forms.ToolStripMenuItem Button_Paste;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Copy;
        private System.Windows.Forms.ToolStripMenuItem Context_Paste;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_Import;
        private System.Windows.Forms.ToolStripMenuItem Button_Export;
        private System.Windows.Forms.ToolStripMenuItem Context_Export;
        private System.Windows.Forms.TextBox SearchBox;
    }
}