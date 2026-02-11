using ResourceTypes.SoundTable;
using System.Drawing;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class SoundTableEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundTableEditor));
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            splitContainer = new SplitContainer();
            treeView = new TreeView();
            propertyGrid = new PropertyGrid();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 642);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1293, 22);
            statusStrip.TabIndex = 1;
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 17);
            statusLabel.Text = "Ready";
            // 
            // splitContainer
            // 
            splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer.Location = new Point(12, 28);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(treeView);
            //
            // ContextMenu
            //
            var treeContextMenu = new ContextMenuStrip();
            var addMenuItem = new ToolStripMenuItem("Add", null, OnAddItem);
            var deleteMenuItem = new ToolStripMenuItem("Delete", null, OnDeleteItem);
            treeContextMenu.Items.AddRange(new ToolStripItem[] { addMenuItem, deleteMenuItem });
            treeView.ContextMenuStrip = treeContextMenu;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(propertyGrid);
            splitContainer.Size = new Size(1269, 611);
            splitContainer.SplitterDistance = 775;
            splitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            treeView.Dock = DockStyle.Fill;
            treeView.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            treeView.Location = new Point(0, 0);
            treeView.Name = "treeView";
            treeView.ShowNodeToolTips = true;
            treeView.Size = new Size(775, 611);
            treeView.TabIndex = 0;
            treeView.AfterSelect += TreeView_AfterSelect;
            // 
            // propertyGrid
            // 
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.Location = new Point(0, 0);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.PropertySort = PropertySort.Categorized;
            propertyGrid.Size = new Size(490, 611);
            propertyGrid.TabIndex = 0;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1293, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += OnSaveStbl;
            // 
            // SoundTableEditor
            // 
            ClientSize = new Size(1293, 664);
            Controls.Add(toolStrip1);
            Controls.Add(splitContainer);
            Controls.Add(statusStrip);
            Name = "SoundTableEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sound Table Editor";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private SoundTable soundTable;
        private TreeView treeView;
        private PropertyGrid propertyGrid;
        private SplitContainer splitContainer;
        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
    }
}