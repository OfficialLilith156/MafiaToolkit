namespace Mafia2Tool
{
    partial class XBinEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XBinEditor));
            Grid_XBin = new System.Windows.Forms.PropertyGrid();
            TreeView_XBin = new Mafia2Tool.Controls.MTreeView();
            ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Import = new System.Windows.Forms.ToolStripMenuItem();
            Button_Export = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Main.SuspendLayout();
            SuspendLayout();
            // 
            // Grid_XBin
            // 
            Grid_XBin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Grid_XBin.Location = new System.Drawing.Point(469, 32);
            Grid_XBin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Grid_XBin.Name = "Grid_XBin";
            Grid_XBin.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            Grid_XBin.Size = new System.Drawing.Size(450, 473);
            Grid_XBin.TabIndex = 10;
            Grid_XBin.PropertyValueChanged += OnPropertyValidChanged;
            // 
            // TreeView_XBin
            // 
            TreeView_XBin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_XBin.Location = new System.Drawing.Point(14, 32);
            TreeView_XBin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_XBin.Name = "TreeView_XBin";
            TreeView_XBin.Size = new System.Drawing.Size(429, 472);
            TreeView_XBin.TabIndex = 11;
            TreeView_XBin.AfterSelect += OnNodeSelectSelect;
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
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Import, Button_Export });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            // 
            // Button_Import
            // 
            Button_Import.Name = "Button_Import";
            Button_Import.Size = new System.Drawing.Size(153, 22);
            Button_Import.Text = "$IMPORT_XBIN";
            Button_Import.Click += Button_Import_Click;
            // 
            // Button_Export
            // 
            Button_Export.Name = "Button_Export";
            Button_Export.Size = new System.Drawing.Size(153, 22);
            Button_Export.Text = "$EXPORT_XBIN";
            Button_Export.Click += Button_Export_Click;
            // 
            // XBinEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(ToolStrip_Main);
            Controls.Add(Grid_XBin);
            Controls.Add(TreeView_XBin);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "XBinEditor";
            Text = "$XBIN_EDITOR";
            FormClosing += XbinEditor_Closing;
            ToolStrip_Main.ResumeLayout(false);
            ToolStrip_Main.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_XBin;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_Import;
        private System.Windows.Forms.ToolStripMenuItem Button_Export;
        private Controls.MTreeView TreeView_XBin;
    }
}