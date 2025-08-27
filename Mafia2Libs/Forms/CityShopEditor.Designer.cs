namespace Mafia2Tool
{
    partial class CityShopEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CityShopEditor));
            CollisionContext = new System.Windows.Forms.ContextMenuStrip(components);
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            deletePlacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openM2T = new System.Windows.Forms.OpenFileDialog();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            Button_SaveNonDLC = new System.Windows.Forms.ToolStripMenuItem();
            Button_SaveDLC = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddAreaButton = new System.Windows.Forms.ToolStripMenuItem();
            AddDataButton = new System.Windows.Forms.ToolStripMenuItem();
            DuplicateDataButton = new System.Windows.Forms.ToolStripMenuItem();
            PopulateTranslokatorButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteAreaButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteDataButton = new System.Windows.Forms.ToolStripMenuItem();
            TreeView_CityShop = new Mafia2Tool.Controls.MTreeView();
            propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            TabControl = new System.Windows.Forms.TabControl();
            PropertyGridTab = new System.Windows.Forms.TabPage();
            DataGridTab = new System.Windows.Forms.TabPage();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            CItyShopsContext = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            ContextPaste = new System.Windows.Forms.ToolStripMenuItem();
            Button_MoveUp = new System.Windows.Forms.ToolStripMenuItem();
            Button_MoveDown = new System.Windows.Forms.ToolStripMenuItem();
            CollisionContext.SuspendLayout();
            toolStrip1.SuspendLayout();
            TabControl.SuspendLayout();
            PropertyGridTab.SuspendLayout();
            DataGridTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            CItyShopsContext.SuspendLayout();
            SuspendLayout();
            // 
            // CollisionContext
            // 
            CollisionContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextDelete, deletePlacementToolStripMenuItem });
            CollisionContext.Name = "SDSContext";
            CollisionContext.Size = new System.Drawing.Size(167, 48);
            // 
            // ContextDelete
            // 
            ContextDelete.Name = "ContextDelete";
            ContextDelete.Size = new System.Drawing.Size(166, 22);
            ContextDelete.Text = "Delete Collision";
            // 
            // deletePlacementToolStripMenuItem
            // 
            deletePlacementToolStripMenuItem.Name = "deletePlacementToolStripMenuItem";
            deletePlacementToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            deletePlacementToolStripMenuItem.Text = "Delete Placement";
            // 
            // openM2T
            // 
            openM2T.FileName = "Select M2T file.";
            openM2T.Filter = "Model File|*.m2t|All Files|*.*|FBX Model|*.fbx";
            openM2T.Tag = "";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolButton, toolButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_SaveNonDLC, Button_SaveDLC, ReloadButton, ExitButton });
            fileToolButton.Image = (System.Drawing.Image)resources.GetObject("fileToolButton.Image");
            fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            fileToolButton.Name = "fileToolButton";
            fileToolButton.Size = new System.Drawing.Size(47, 22);
            fileToolButton.Text = "$FILE";
            // 
            // Button_SaveNonDLC
            // 
            Button_SaveNonDLC.Name = "Button_SaveNonDLC";
            Button_SaveNonDLC.ShortcutKeyDisplayString = "";
            Button_SaveNonDLC.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            Button_SaveNonDLC.Size = new System.Drawing.Size(165, 22);
            Button_SaveNonDLC.Text = "$SAVE";
            Button_SaveNonDLC.Click += SaveButton_Click;
            // 
            // Button_SaveDLC
            // 
            Button_SaveDLC.Name = "Button_SaveDLC";
            Button_SaveDLC.Size = new System.Drawing.Size(165, 22);
            Button_SaveDLC.Text = "$SAVE_DLC";
            Button_SaveDLC.Click += SaveButtonDLC_Click;
            // 
            // ReloadButton
            // 
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(165, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadButton_Click;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(165, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_Click;
            // 
            // toolButton
            // 
            toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddAreaButton, AddDataButton, DuplicateDataButton, PopulateTranslokatorButton, DeleteAreaButton, DeleteDataButton });
            toolButton.Image = (System.Drawing.Image)resources.GetObject("toolButton.Image");
            toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButton.Name = "toolButton";
            toolButton.Size = new System.Drawing.Size(61, 22);
            toolButton.Text = "$TOOLS";
            // 
            // AddAreaButton
            // 
            AddAreaButton.Name = "AddAreaButton";
            AddAreaButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            AddAreaButton.Size = new System.Drawing.Size(233, 22);
            AddAreaButton.Text = "$ADD_AREA";
            AddAreaButton.Click += AddAreaButton_Click;
            // 
            // AddDataButton
            // 
            AddDataButton.Name = "AddDataButton";
            AddDataButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.D;
            AddDataButton.Size = new System.Drawing.Size(233, 22);
            AddDataButton.Text = "$ADD_DATA";
            AddDataButton.Click += AddDataButton_Click;
            // 
            // DuplicateDataButton
            // 
            DuplicateDataButton.Name = "DuplicateDataButton";
            DuplicateDataButton.Size = new System.Drawing.Size(233, 22);
            DuplicateDataButton.Text = "$DUPLICATE_DATA";
            DuplicateDataButton.Click += DuplicateData_OnClick;
            // 
            // PopulateTranslokatorButton
            // 
            PopulateTranslokatorButton.Name = "PopulateTranslokatorButton";
            PopulateTranslokatorButton.Size = new System.Drawing.Size(233, 22);
            PopulateTranslokatorButton.Text = "$POPULATE_TRANSLOKATORS";
            PopulateTranslokatorButton.Click += PopulateTranslokatorButton_Click;
            // 
            // DeleteAreaButton
            // 
            DeleteAreaButton.Name = "DeleteAreaButton";
            DeleteAreaButton.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A;
            DeleteAreaButton.Size = new System.Drawing.Size(233, 22);
            DeleteAreaButton.Text = "$DELETE_AREA";
            DeleteAreaButton.Click += DeleteArea_Click;
            // 
            // DeleteDataButton
            // 
            DeleteDataButton.Name = "DeleteDataButton";
            DeleteDataButton.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D;
            DeleteDataButton.Size = new System.Drawing.Size(233, 22);
            DeleteDataButton.Text = "$DELETE_DATA";
            DeleteDataButton.Click += DeleteData_Click;
            // 
            // TreeView_CityShop
            // 
            TreeView_CityShop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_CityShop.Location = new System.Drawing.Point(14, 32);
            TreeView_CityShop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_CityShop.Name = "TreeView_CityShop";
            TreeView_CityShop.Size = new System.Drawing.Size(360, 472);
            TreeView_CityShop.TabIndex = 16;
            TreeView_CityShop.AfterSelect += OnAfterSelect;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Location = new System.Drawing.Point(4, 3);
            propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(522, 439);
            propertyGrid1.TabIndex = 17;
            propertyGrid1.PropertyValueChanged += OnPropertyChanged;
            // 
            // TabControl
            // 
            TabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TabControl.Controls.Add(PropertyGridTab);
            TabControl.Controls.Add(DataGridTab);
            TabControl.Location = new System.Drawing.Point(382, 32);
            TabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(538, 473);
            TabControl.TabIndex = 18;
            TabControl.Selected += OnTabSelected;
            // 
            // PropertyGridTab
            // 
            PropertyGridTab.Controls.Add(propertyGrid1);
            PropertyGridTab.Location = new System.Drawing.Point(4, 24);
            PropertyGridTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGridTab.Name = "PropertyGridTab";
            PropertyGridTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGridTab.Size = new System.Drawing.Size(530, 445);
            PropertyGridTab.TabIndex = 0;
            PropertyGridTab.Text = "Properties";
            PropertyGridTab.UseVisualStyleBackColor = true;
            // 
            // DataGridTab
            // 
            DataGridTab.Controls.Add(dataGridView1);
            DataGridTab.Location = new System.Drawing.Point(4, 24);
            DataGridTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DataGridTab.Name = "DataGridTab";
            DataGridTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DataGridTab.Size = new System.Drawing.Size(530, 445);
            DataGridTab.TabIndex = 1;
            DataGridTab.Text = "Entity Grid";
            DataGridTab.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(4, 3);
            dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(522, 439);
            dataGridView1.TabIndex = 0;
            // 
            // CItyShopsContext
            // 
            CItyShopsContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, ContextCopy, ContextPaste, Button_MoveUp, Button_MoveDown });
            CItyShopsContext.Name = "SDSContext";
            CItyShopsContext.Size = new System.Drawing.Size(261, 114);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            toolStripMenuItem1.Size = new System.Drawing.Size(260, 22);
            toolStripMenuItem1.Text = "$DELETE";
            // 
            // ContextCopy
            // 
            ContextCopy.Name = "ContextCopy";
            ContextCopy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            ContextCopy.Size = new System.Drawing.Size(260, 22);
            ContextCopy.Text = "$COPY";
            // 
            // ContextPaste
            // 
            ContextPaste.Name = "ContextPaste";
            ContextPaste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            ContextPaste.Size = new System.Drawing.Size(260, 22);
            ContextPaste.Text = "$PASTE";
            // 
            // Button_MoveUp
            // 
            Button_MoveUp.Name = "Button_MoveUp";
            Button_MoveUp.ShortcutKeyDisplayString = "CTRL + PageUp";
            Button_MoveUp.Size = new System.Drawing.Size(260, 22);
            Button_MoveUp.Text = "$MOVE_UP";
            // 
            // Button_MoveDown
            // 
            Button_MoveDown.Name = "Button_MoveDown";
            Button_MoveDown.ShortcutKeyDisplayString = "CTRL + PageDown";
            Button_MoveDown.Size = new System.Drawing.Size(260, 22);
            Button_MoveDown.Text = "$MOVE_DOWN";
            // 
            // CityShopEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(TreeView_CityShop);
            Controls.Add(toolStrip1);
            Controls.Add(TabControl);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CityShopEditor";
            Text = "$ACTOR_EDITOR_TITLE";
            FormClosing += CityShopEditor_Closing;
            CollisionContext.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            TabControl.ResumeLayout(false);
            PropertyGridTab.ResumeLayout(false);
            DataGridTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            CItyShopsContext.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openM2T;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem Button_SaveNonDLC;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem deletePlacementToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem AddAreaButton;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem PopulateTranslokatorButton;
        private System.Windows.Forms.ToolStripMenuItem AddDataButton;
        private System.Windows.Forms.TabPage PropertyGridTab;
        private System.Windows.Forms.TabPage DataGridTab;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem DuplicateDataButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteAreaButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteDataButton;
        private System.Windows.Forms.ToolStripMenuItem Button_SaveDLC;
        private Controls.MTreeView TreeView_CityShop;
        private System.Windows.Forms.ContextMenuStrip CItyShopsContext;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ContextCopy;
        private System.Windows.Forms.ToolStripMenuItem ContextPaste;
        private System.Windows.Forms.ToolStripMenuItem Button_MoveUp;
        private System.Windows.Forms.ToolStripMenuItem Button_MoveDown;
    }
}