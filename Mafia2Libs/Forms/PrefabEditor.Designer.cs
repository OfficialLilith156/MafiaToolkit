namespace Mafia2Tool
{
    partial class PrefabEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrefabEditor));
            Grid_Prefabs = new System.Windows.Forms.PropertyGrid();
            TreeView_Prefabs = new Mafia2Tool.Controls.MTreeView();
            Context_Menu = new System.Windows.Forms.ContextMenuStrip(components);
            Context_ImportXML = new System.Windows.Forms.ToolStripMenuItem();
            Context_ExportXML = new System.Windows.Forms.ToolStripMenuItem();
            Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Browser_ImportPRB = new System.Windows.Forms.OpenFileDialog();
            ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Import = new System.Windows.Forms.ToolStripMenuItem();
            Button_Export = new System.Windows.Forms.ToolStripMenuItem();
            Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Browser_ExportPRB = new System.Windows.Forms.SaveFileDialog();
            SearchBox = new System.Windows.Forms.TextBox();
            Context_Menu.SuspendLayout();
            ToolStrip_Main.SuspendLayout();
            SuspendLayout();
            // 
            // Grid_Prefabs
            // 
            Grid_Prefabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Grid_Prefabs.Location = new System.Drawing.Point(469, 32);
            Grid_Prefabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Grid_Prefabs.Name = "Grid_Prefabs";
            Grid_Prefabs.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            Grid_Prefabs.Size = new System.Drawing.Size(450, 473);
            Grid_Prefabs.TabIndex = 10;
            Grid_Prefabs.PropertyValueChanged += Grid_Prefabs_OnPropertyValueChanged;
            // 
            // TreeView_Prefabs
            // 
            TreeView_Prefabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_Prefabs.ContextMenuStrip = Context_Menu;
            TreeView_Prefabs.Location = new System.Drawing.Point(14, 32);
            TreeView_Prefabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Prefabs.Name = "TreeView_Prefabs";
            TreeView_Prefabs.Size = new System.Drawing.Size(429, 472);
            TreeView_Prefabs.TabIndex = 11;
            TreeView_Prefabs.AfterSelect += OnNodeSelectSelect;
            // 
            // Context_Menu
            // 
            Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Context_ImportXML, Context_ExportXML, Context_Delete });
            Context_Menu.Name = "Context_Menu";
            Context_Menu.Size = new System.Drawing.Size(191, 70);
            Context_Menu.Opening += Context_Menu_OnOpening;
            // 
            // Context_ImportXML
            // 
            Context_ImportXML.Name = "Context_ImportXML";
            Context_ImportXML.Size = new System.Drawing.Size(190, 22);
            Context_ImportXML.Text = "$IMPORT_XML";
            Context_ImportXML.Click += Context_ImportAsXML_Clicked;
            // 
            // Context_ExportXML
            // 
            Context_ExportXML.Name = "Context_ExportXML";
            Context_ExportXML.Size = new System.Drawing.Size(190, 22);
            Context_ExportXML.Text = "$EXPORT_XML";
            Context_ExportXML.Click += Context_ExportAsXML_Clicked;
            // 
            // Context_Delete
            // 
            Context_Delete.Name = "Context_Delete";
            Context_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Context_Delete.Size = new System.Drawing.Size(190, 22);
            Context_Delete.Text = "$DELETE";
            Context_Delete.Click += Context_Delete_Click;
            // 
            // Browser_ImportPRB
            // 
            Browser_ImportPRB.FileName = "Select Singular Prefab file";
            Browser_ImportPRB.Filter = "Prefab File|*.prb|All Files|*.*";
            Browser_ImportPRB.Tag = "";
            // 
            // ToolStrip_Main
            // 
            ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            ToolStrip_Main.Name = "ToolStrip_Main";
            ToolStrip_Main.Size = new System.Drawing.Size(933, 25);
            ToolStrip_Main.TabIndex = 15;
            ToolStrip_Main.Text = "toolStrip1";
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
            Button_Save.Size = new System.Drawing.Size(180, 22);
            Button_Save.Text = "$SAVE";
            Button_Save.Click += Button_Save_Click;
            // 
            // Button_Reload
            // 
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(180, 22);
            Button_Reload.Text = "$RELOAD";
            Button_Reload.Click += Button_Reload_Click;
            // 
            // Button_Exit
            // 
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(180, 22);
            Button_Exit.Text = "$EXIT";
            // 
            // Button_Tools
            // 
            Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Import, Button_Export, Button_Delete });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            // 
            // Button_Import
            // 
            Button_Import.Name = "Button_Import";
            Button_Import.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            Button_Import.Size = new System.Drawing.Size(246, 22);
            Button_Import.Text = "$IMPORT_PREFAB";
            Button_Import.Click += Button_Import_Click;
            // 
            // Button_Export
            // 
            Button_Export.Name = "Button_Export";
            Button_Export.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            Button_Export.Size = new System.Drawing.Size(246, 22);
            Button_Export.Text = "$EXPORT_PREFAB";
            Button_Export.Click += Button_Export_Click;
            // 
            // Button_Delete
            // 
            Button_Delete.Name = "Button_Delete";
            Button_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Button_Delete.Size = new System.Drawing.Size(246, 22);
            Button_Delete.Text = "$DELETE_PREFAB";
            Button_Delete.Click += Button_Delete_Click;
            // 
            // Browser_ExportPRB
            // 
            Browser_ExportPRB.FileName = "Save Singular Prefab file";
            Browser_ExportPRB.Filter = "Prefab File|*.prb|All Files|*.*";
            // 
            // SearchBox
            // 
            SearchBox.Location = new System.Drawing.Point(291, 4);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(152, 23);
            SearchBox.TabIndex = 17;
            // 
            // PrefabEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(SearchBox);
            Controls.Add(ToolStrip_Main);
            Controls.Add(Grid_Prefabs);
            Controls.Add(TreeView_Prefabs);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "PrefabEditor";
            Text = "$PREFAB_EDITOR_TITLE";
            FormClosing += PrefabEditor_Closing;
            KeyUp += PrefabEditor_OnKeyUp;
            Context_Menu.ResumeLayout(false);
            ToolStrip_Main.ResumeLayout(false);
            ToolStrip_Main.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_Prefabs;
        private System.Windows.Forms.OpenFileDialog Browser_ImportPRB;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_Import;
        private System.Windows.Forms.ToolStripMenuItem Button_Export;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.SaveFileDialog Browser_ExportPRB;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private Controls.MTreeView TreeView_Prefabs;
        private System.Windows.Forms.ToolStripMenuItem Context_ImportXML;
        private System.Windows.Forms.ToolStripMenuItem Context_ExportXML;
        private System.Windows.Forms.TextBox SearchBox;
    }
}