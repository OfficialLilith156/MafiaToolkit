using System;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class DATEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DATEditor));
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStrip = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            deleteIDToolStripMenuItem = new ToolStripMenuItem();
            tbSearch = new ToolStripTextBox();
            btnSearch = new ToolStripButton();
            btnClearSearch = new ToolStripButton();
            dataGridView = new DataGridView();
            groupBoxAdd = new GroupBox();
            labelAddId = new Label();
            tbAddId = new TextBox();
            labelAddText = new Label();
            tbAddText = new TextBox();
            btnAdd = new Button();
            menuStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            groupBoxAdd.SuspendLayout();
            SuspendLayout();
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "DAT Files (*.dat)|*.dat|All Files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "DAT Files (*.dat)|*.dat|All Files (*.*)|*.*";
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(200, 24);
            menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2, tbSearch, btnSearch, btnClearSearch });
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new System.Drawing.Size(784, 25);
            toolStrip.TabIndex = 4;
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem1, exitToolStripMenuItem1 });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // saveToolStripMenuItem1
            // 
            saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            saveToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            saveToolStripMenuItem1.Text = "Save";
            saveToolStripMenuItem1.Click += SaveToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new System.Drawing.Size(98, 22);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += ExitToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { deleteIDToolStripMenuItem });
            toolStripDropDownButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new System.Drawing.Size(40, 22);
            toolStripDropDownButton2.Text = "Edit";
            // 
            // deleteIDToolStripMenuItem
            // 
            deleteIDToolStripMenuItem.Name = "deleteIDToolStripMenuItem";
            deleteIDToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            deleteIDToolStripMenuItem.Text = "Delete ID";
            deleteIDToolStripMenuItem.Click += BtnDelete_Click;
            // 
            // tbSearch
            // 
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new System.Drawing.Size(200, 25);
            tbSearch.ToolTipText = "Search by ID or text";
            // 
            // btnSearch
            // 
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(46, 22);
            btnSearch.Text = "Search";
            btnSearch.Click += BtnSearch_Click;
            // 
            // btnClearSearch
            // 
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new System.Drawing.Size(39, 22);
            btnClearSearch.Text = "Reset";
            btnClearSearch.Click += BtnClearSearch_Click;
            // 
            // dataGridView
            // 
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 25);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new System.Drawing.Size(784, 416);
            dataGridView.TabIndex = 0;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            // 
            // groupBoxAdd
            // 
            groupBoxAdd.Controls.Add(labelAddId);
            groupBoxAdd.Controls.Add(tbAddId);
            groupBoxAdd.Controls.Add(labelAddText);
            groupBoxAdd.Controls.Add(tbAddText);
            groupBoxAdd.Controls.Add(btnAdd);
            groupBoxAdd.Dock = DockStyle.Bottom;
            groupBoxAdd.Location = new System.Drawing.Point(0, 441);
            groupBoxAdd.Name = "groupBoxAdd";
            groupBoxAdd.Size = new System.Drawing.Size(784, 120);
            groupBoxAdd.TabIndex = 2;
            groupBoxAdd.TabStop = false;
            groupBoxAdd.Text = "Add new entry";
            // 
            // labelAddId
            // 
            labelAddId.AutoSize = true;
            labelAddId.Location = new System.Drawing.Point(10, 20);
            labelAddId.Name = "labelAddId";
            labelAddId.Size = new System.Drawing.Size(21, 15);
            labelAddId.TabIndex = 0;
            labelAddId.Text = "ID:";
            // 
            // tbAddId
            // 
            tbAddId.Location = new System.Drawing.Point(50, 18);
            tbAddId.Name = "tbAddId";
            tbAddId.Size = new System.Drawing.Size(400, 23);
            tbAddId.TabIndex = 1;
            // 
            // labelAddText
            // 
            labelAddText.AutoSize = true;
            labelAddText.Location = new System.Drawing.Point(10, 50);
            labelAddText.Name = "labelAddText";
            labelAddText.Size = new System.Drawing.Size(31, 15);
            labelAddText.TabIndex = 2;
            labelAddText.Text = "Text:";
            // 
            // tbAddText
            // 
            tbAddText.Location = new System.Drawing.Point(50, 48);
            tbAddText.Name = "tbAddText";
            tbAddText.Size = new System.Drawing.Size(400, 23);
            tbAddText.TabIndex = 3;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(460, 48);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(75, 23);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Add";
            btnAdd.Click += BtnAdd_Click;
            // 
            // DATEditor
            // 
            ClientSize = new System.Drawing.Size(784, 561);
            Controls.Add(dataGridView);
            Controls.Add(groupBoxAdd);
            Controls.Add(toolStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "DATEditor";
            Text = "Mafia II Text Editor";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            groupBoxAdd.ResumeLayout(false);
            groupBoxAdd.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripTextBox tbSearch;
        private ToolStripButton btnSearch;
        private ToolStripButton btnClearSearch;
        private DataGridView dataGridView;
        private GroupBox groupBoxAdd;
        private Button btnAdd;
        private TextBox tbAddText;
        private TextBox tbAddId;
        private Label labelAddId;
        private Label labelAddText;
        #endregion

        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem deleteIDToolStripMenuItem;
    }
}