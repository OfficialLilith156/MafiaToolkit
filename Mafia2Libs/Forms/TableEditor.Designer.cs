using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace Mafia2Tool
{
    partial class TableEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableEditor));
            treeViewRows = new TreeView();
            propertyGrid = new PropertyGrid();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            reloadToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            addRowToolStripMenuItem = new ToolStripMenuItem();
            deleteRowToolStripMenuItem = new ToolStripMenuItem();
            versionLabel = new ToolStripLabel();
            versionComboBox = new ToolStripComboBox();
            searchBox = new ToolStripTextBox();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // treeViewRows
            // 
            treeViewRows.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            treeViewRows.Location = new System.Drawing.Point(12, 28);
            treeViewRows.Name = "treeViewRows";
            treeViewRows.Size = new System.Drawing.Size(272, 521);
            treeViewRows.TabIndex = 1;
            treeViewRows.AfterSelect += TreeViewRows_AfterSelect;
            // 
            // propertyGrid
            // 
            propertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            propertyGrid.Location = new System.Drawing.Point(290, 28);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new System.Drawing.Size(582, 521);
            propertyGrid.TabIndex = 0;
            propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2, versionLabel, versionComboBox, searchBox });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(884, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, reloadToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            toolStripDropDownButton1.ToolTipText = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveOnClick;
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            reloadToolStripMenuItem.Text = "Reload";
            reloadToolStripMenuItem.Click += ReloadOnClick;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitButtonOnClick;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { addRowToolStripMenuItem, deleteRowToolStripMenuItem });
            toolStripDropDownButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new System.Drawing.Size(40, 22);
            toolStripDropDownButton2.Text = "Edit";
            // 
            // addRowToolStripMenuItem
            // 
            addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            addRowToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            addRowToolStripMenuItem.Text = "Add Row";
            addRowToolStripMenuItem.Click += AddRowOnClick;
            // 
            // deleteRowToolStripMenuItem
            // 
            deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            deleteRowToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            deleteRowToolStripMenuItem.Text = "Delete Row";
            deleteRowToolStripMenuItem.Click += DeleteRowOnClick;
            // 
            // versionLabel
            // 
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // versionComboBox
            // 
            versionComboBox.Items.AddRange(new object[] { "1 (Classic)", "2 (Definitive Edition)" });
            versionComboBox.Name = "versionComboBox";
            versionComboBox.Size = new System.Drawing.Size(150, 25);
            versionComboBox.SelectedIndexChanged += VersionComboBox_SelectedIndexChanged;
            // 
            // searchBox
            // 
            searchBox.Name = "searchBox";
            searchBox.Size = new System.Drawing.Size(150, 25);
            searchBox.TextChanged += SearchBox_TextChanged;
            // 
            // TableEditor
            // 
            ClientSize = new System.Drawing.Size(884, 561);
            Controls.Add(toolStrip1);
            Controls.Add(propertyGrid);
            Controls.Add(treeViewRows);
            Name = "TableEditor";
            Text = "Table Editor";
            FormClosing += TableEditor_Closing;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView treeViewRows;
        private PropertyGrid propertyGrid;
        private Label rowIndexLabel;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem addRowToolStripMenuItem;
        private ToolStripMenuItem deleteRowToolStripMenuItem;
        private ToolStripLabel versionLabel;
        private ToolStripComboBox versionComboBox;
        private ToolStripTextBox searchBox;
    }
}