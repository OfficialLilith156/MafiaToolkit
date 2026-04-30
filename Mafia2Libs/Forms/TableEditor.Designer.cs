using System.Drawing;
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
            loadDATToolStripMenuItem = new ToolStripMenuItem();
            versionLabel = new ToolStripLabel();
            versionComboBox = new ToolStripComboBox();
            searchBox = new ToolStripTextBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabdsasdel1 = new ToolStripLabel();
            keyColumnComboBox = new ToolStripComboBox();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // treeViewRows
            // 
            treeViewRows.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            treeViewRows.Location = new Point(12, 28);
            treeViewRows.Name = "treeViewRows";
            treeViewRows.Size = new Size(272, 521);
            treeViewRows.TabIndex = 1;
            treeViewRows.AfterSelect += TreeViewRows_AfterSelect;
            // 
            // propertyGrid
            // 
            propertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            propertyGrid.Location = new Point(290, 28);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(609, 521);
            propertyGrid.TabIndex = 0;
            propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2, versionLabel, versionComboBox, searchBox, toolStripSeparator1, toolStripLabdsasdel1, keyColumnComboBox });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(911, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, reloadToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            toolStripDropDownButton1.ToolTipText = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(110, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveOnClick;
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.Size = new Size(110, 22);
            reloadToolStripMenuItem.Text = "Reload";
            reloadToolStripMenuItem.Click += ReloadOnClick;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(110, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitButtonOnClick;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { addRowToolStripMenuItem, deleteRowToolStripMenuItem, loadDATToolStripMenuItem });
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(40, 22);
            toolStripDropDownButton2.Text = "Edit";
            // 
            // addRowToolStripMenuItem
            // 
            addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            addRowToolStripMenuItem.Size = new Size(133, 22);
            addRowToolStripMenuItem.Text = "Add Row";
            addRowToolStripMenuItem.Click += AddRowOnClick;
            // 
            // deleteRowToolStripMenuItem
            // 
            deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            deleteRowToolStripMenuItem.Size = new Size(133, 22);
            deleteRowToolStripMenuItem.Text = "Delete Row";
            deleteRowToolStripMenuItem.Click += DeleteRowOnClick;
            // 
            // loadDATToolStripMenuItem
            // 
            loadDATToolStripMenuItem.Name = "loadDATToolStripMenuItem";
            loadDATToolStripMenuItem.Size = new Size(133, 22);
            loadDATToolStripMenuItem.Text = "Load DAT";
            loadDATToolStripMenuItem.Click += LoadTextDbButton_Click;
            // 
            // versionLabel
            // 
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(0, 22);
            // 
            // versionComboBox
            // 
            versionComboBox.Items.AddRange(new object[] { "1 (Classic)", "2 (Definitive Edition)" });
            versionComboBox.Name = "versionComboBox";
            versionComboBox.Size = new Size(150, 25);
            versionComboBox.SelectedIndexChanged += VersionComboBox_SelectedIndexChanged;
            // 
            // searchBox
            // 
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(150, 25);
            searchBox.TextChanged += SearchBox_TextChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripLabdsasdel1
            // 
            toolStripLabdsasdel1.Name = "toolStripLabdsasdel1";
            toolStripLabdsasdel1.Size = new Size(53, 22);
            toolStripLabdsasdel1.Text = "Column:";
            // 
            // keyColumnComboBox
            // 
            keyColumnComboBox.Name = "keyColumnComboBox";
            keyColumnComboBox.Size = new Size(121, 25);
            keyColumnComboBox.SelectedIndexChanged += KeyColumnComboBox_SelectedIndexChanged;
            // 
            // TableEditor
            // 
            ClientSize = new Size(911, 561);
            Controls.Add(toolStrip1);
            Controls.Add(propertyGrid);
            Controls.Add(treeViewRows);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private ToolStripMenuItem loadDATToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripComboBox keyColumnComboBox;
        private ToolStripLabel toolStripLabdsasdel1;
    }
}