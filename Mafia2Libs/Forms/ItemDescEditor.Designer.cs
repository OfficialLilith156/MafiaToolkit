using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class ItemDescEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDescEditor));
            TreeView_Data = new TreeView();
            PropertyGrid_Data = new PropertyGrid();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // TreeView_Data
            // 
            TreeView_Data.Location = new System.Drawing.Point(12, 28);
            TreeView_Data.Name = "TreeView_Data";
            TreeView_Data.Size = new System.Drawing.Size(238, 391);
            TreeView_Data.TabIndex = 0;
            TreeView_Data.AfterSelect += TreeView_Data_AfterSelect;
            // 
            // PropertyGrid_Data
            // 
            PropertyGrid_Data.Location = new System.Drawing.Point(256, 28);
            PropertyGrid_Data.Name = "PropertyGrid_Data";
            PropertyGrid_Data.Size = new System.Drawing.Size(520, 391);
            PropertyGrid_Data.TabIndex = 1;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(788, 25);
            toolStrip1.TabIndex = 6;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += Button_Save_Click;
            // 
            // ItemDescEditor
            // 
            ClientSize = new System.Drawing.Size(788, 423);
            Controls.Add(TreeView_Data);
            Controls.Add(PropertyGrid_Data);
            Controls.Add(toolStrip1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ItemDescEditor";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ItemDesc Editor";
            FormClosing += ItemDescEditor_FormClosing;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        // UI Components
        private TreeView TreeView_Data;
        private PropertyGrid PropertyGrid_Data;

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
    }
}