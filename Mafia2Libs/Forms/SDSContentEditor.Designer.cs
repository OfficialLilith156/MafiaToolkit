namespace Mafia2Tool
{
    partial class SDSContentEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDSContentEditor));
            ResourceTreeView = new Mafia2Tool.Controls.MTreeView();
            Context_Menu = new System.Windows.Forms.ContextMenuStrip(components);
            Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Tool_Strip = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_AutoAdd = new System.Windows.Forms.ToolStripMenuItem();
            Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Button_BatchImportTextures = new System.Windows.Forms.ToolStripMenuItem();
            FileDialog_Generic = new System.Windows.Forms.OpenFileDialog();
            FolderBrowser_Generic = new System.Windows.Forms.FolderBrowserDialog();
            Context_Menu.SuspendLayout();
            Tool_Strip.SuspendLayout();
            SuspendLayout();
            // 
            // ResourceTreeView
            // 
            ResourceTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ResourceTreeView.ContextMenuStrip = Context_Menu;
            ResourceTreeView.Location = new System.Drawing.Point(14, 32);
            ResourceTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ResourceTreeView.Name = "ResourceTreeView";
            ResourceTreeView.Size = new System.Drawing.Size(463, 383);
            ResourceTreeView.TabIndex = 0;
            // 
            // Context_Menu
            // 
            Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Context_Delete });
            Context_Menu.Name = "Context_Menu";
            Context_Menu.Size = new System.Drawing.Size(191, 26);
            // 
            // Context_Delete
            // 
            Context_Delete.Name = "Context_Delete";
            Context_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Context_Delete.Size = new System.Drawing.Size(190, 22);
            Context_Delete.Text = "$DELETE";
            Context_Delete.Click += Context_Delete_Click;
            // 
            // Tool_Strip
            // 
            Tool_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            Tool_Strip.Location = new System.Drawing.Point(0, 0);
            Tool_Strip.Name = "Tool_Strip";
            Tool_Strip.Size = new System.Drawing.Size(490, 25);
            Tool_Strip.TabIndex = 15;
            Tool_Strip.Text = "toolStrip1";
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
            Button_Exit.Click += Button_Exit_Click;
            // 
            // Button_Tools
            // 
            Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_AutoAdd, Button_Delete, Button_BatchImportTextures });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            // 
            // Button_AutoAdd
            // 
            Button_AutoAdd.Name = "Button_AutoAdd";
            Button_AutoAdd.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            Button_AutoAdd.Size = new System.Drawing.Size(213, 22);
            Button_AutoAdd.Text = "$AUTO-ADD_FILES";
            Button_AutoAdd.Click += Button_AutoAdd_Click;
            // 
            // Button_Delete
            // 
            Button_Delete.Name = "Button_Delete";
            Button_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Button_Delete.Size = new System.Drawing.Size(213, 22);
            Button_Delete.Text = "$DELETE";
            Button_Delete.Click += Button_Delete_Click;
            // 
            // Button_BatchImportTextures
            // 
            Button_BatchImportTextures.Name = "Button_BatchImportTextures";
            Button_BatchImportTextures.Size = new System.Drawing.Size(213, 22);
            Button_BatchImportTextures.Text = "$BATCH_IMPORT_TEX";
            Button_BatchImportTextures.Click += Button_BatchImportTextures_Click;
            // 
            // FileDialog_Generic
            // 
            FileDialog_Generic.FileName = "AllTextures";
            FileDialog_Generic.Filter = "Text Files|*.txt";
            // 
            // FolderBrowser_Generic
            // 
            FolderBrowser_Generic.Description = "Select folder which contains textures.";
            FolderBrowser_Generic.ShowNewFolderButton = false;
            // 
            // SDSContentEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(490, 430);
            Controls.Add(Tool_Strip);
            Controls.Add(ResourceTreeView);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SDSContentEditor";
            Text = "$SDSCONTENT_EDITOR_TITLE";
            FormClosing += SDSContentEditor_Closing;
            Context_Menu.ResumeLayout(false);
            Tool_Strip.ResumeLayout(false);
            Tool_Strip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip Tool_Strip;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_AutoAdd;
        private System.Windows.Forms.ToolStripMenuItem Button_BatchImportTextures;
        private System.Windows.Forms.OpenFileDialog FileDialog_Generic;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser_Generic;
        private Controls.MTreeView ResourceTreeView;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
    }
}