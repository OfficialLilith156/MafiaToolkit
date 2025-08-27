namespace Mafia2Tool {
    partial class MaterialEditor {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialEditor));
            MaterialSearch = new System.Windows.Forms.TextBox();
            MaterialGrid = new System.Windows.Forms.PropertyGrid();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Open = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            Button_AddMaterial = new System.Windows.Forms.ToolStripMenuItem();
            Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Button_MergeMTL = new System.Windows.Forms.ToolStripMenuItem();
            Button_ExportSelected = new System.Windows.Forms.ToolStripMenuItem();
            Button_Debug = new System.Windows.Forms.ToolStripDropDownButton();
            Button_DumpTextures = new System.Windows.Forms.ToolStripMenuItem();
            GirdView_Materials = new System.Windows.Forms.DataGridView();
            columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            columnHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            MTLBrowser = new System.Windows.Forms.OpenFileDialog();
            Panel_Main = new System.Windows.Forms.Panel();
            Button_Search = new System.Windows.Forms.Button();
            Label_SearchType = new System.Windows.Forms.Label();
            ComboBox_SearchType = new System.Windows.Forms.ComboBox();
            MergePanel = new System.Windows.Forms.Panel();
            SelectAllNewButton = new System.Windows.Forms.Button();
            SelectAllOverwriteButton = new System.Windows.Forms.Button();
            NewMaterialLabel = new System.Windows.Forms.Label();
            OverWriteLabel = new System.Windows.Forms.Label();
            NewMatListBox = new System.Windows.Forms.CheckedListBox();
            CancelButton = new System.Windows.Forms.Button();
            MergeButton = new System.Windows.Forms.Button();
            OverwriteListBox = new System.Windows.Forms.CheckedListBox();
            MTLSaveDialog = new System.Windows.Forms.SaveFileDialog();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GirdView_Materials).BeginInit();
            Panel_Main.SuspendLayout();
            MergePanel.SuspendLayout();
            SuspendLayout();
            // 
            // MaterialSearch
            // 
            MaterialSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            MaterialSearch.Location = new System.Drawing.Point(4, 32);
            MaterialSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaterialSearch.Name = "MaterialSearch";
            MaterialSearch.Size = new System.Drawing.Size(243, 23);
            MaterialSearch.TabIndex = 0;
            MaterialSearch.KeyDown += MaterialSearch_KeyDown;
            // 
            // MaterialGrid
            // 
            MaterialGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MaterialGrid.Location = new System.Drawing.Point(394, 3);
            MaterialGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaterialGrid.Name = "MaterialGrid";
            MaterialGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            MaterialGrid.Size = new System.Drawing.Size(522, 462);
            MaterialGrid.TabIndex = 2;
            MaterialGrid.PropertyValueChanged += MaterialGrid_OnPropertyValueChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, toolButton, Button_Debug });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(916, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // Button_File
            // 
            Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Open, Button_Reload, Button_Save, Button_Exit });
            Button_File.Image = (System.Drawing.Image)resources.GetObject("Button_File.Image");
            Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_File.Name = "Button_File";
            Button_File.Size = new System.Drawing.Size(38, 22);
            Button_File.Text = "File";
            // 
            // Button_Open
            // 
            Button_Open.Enabled = false;
            Button_Open.Name = "Button_Open";
            Button_Open.Size = new System.Drawing.Size(224, 22);
            Button_Open.Text = "Open";
            // 
            // Button_Reload
            // 
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(224, 22);
            Button_Reload.Text = "$RELOAD_MATERIAL";
            Button_Reload.Click += Button_Reload_Click;
            // 
            // Button_Save
            // 
            Button_Save.Name = "Button_Save";
            Button_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            Button_Save.Size = new System.Drawing.Size(224, 22);
            Button_Save.Text = "Save";
            Button_Save.Click += Button_Save_Click;
            // 
            // Button_Exit
            // 
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(224, 22);
            Button_Exit.Text = "Exit";
            Button_Exit.Click += Button_Exit_Click;
            // 
            // toolButton
            // 
            toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_AddMaterial, Button_Delete, Button_MergeMTL, Button_ExportSelected });
            toolButton.Image = (System.Drawing.Image)resources.GetObject("toolButton.Image");
            toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButton.Name = "toolButton";
            toolButton.Size = new System.Drawing.Size(47, 22);
            toolButton.Text = "Tools";
            // 
            // Button_AddMaterial
            // 
            Button_AddMaterial.Name = "Button_AddMaterial";
            Button_AddMaterial.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A;
            Button_AddMaterial.Size = new System.Drawing.Size(208, 22);
            Button_AddMaterial.Text = "Add Material";
            Button_AddMaterial.Click += Button_AddMaterial_Click;
            // 
            // Button_Delete
            // 
            Button_Delete.Name = "Button_Delete";
            Button_Delete.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D;
            Button_Delete.Size = new System.Drawing.Size(208, 22);
            Button_Delete.Text = "$DELETE_SEL_MAT";
            Button_Delete.Click += Button_Delete_Click;
            // 
            // Button_MergeMTL
            // 
            Button_MergeMTL.Name = "Button_MergeMTL";
            Button_MergeMTL.Size = new System.Drawing.Size(208, 22);
            Button_MergeMTL.Text = "$MERGE_MTL";
            Button_MergeMTL.Click += Button_MergeMTL_Click;
            // 
            // Button_ExportSelected
            // 
            Button_ExportSelected.Name = "Button_ExportSelected";
            Button_ExportSelected.Size = new System.Drawing.Size(208, 22);
            Button_ExportSelected.Text = "$EXPORT_SELECTED";
            Button_ExportSelected.Click += Button_ExportedSelected_Click;
            // 
            // Button_Debug
            // 
            Button_Debug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Debug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_DumpTextures });
            Button_Debug.Image = (System.Drawing.Image)resources.GetObject("Button_Debug.Image");
            Button_Debug.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Debug.Name = "Button_Debug";
            Button_Debug.Size = new System.Drawing.Size(103, 22);
            Button_Debug.Text = "$DEBUG_TOOLS";
            // 
            // Button_DumpTextures
            // 
            Button_DumpTextures.Name = "Button_DumpTextures";
            Button_DumpTextures.Size = new System.Drawing.Size(210, 22);
            Button_DumpTextures.Text = "$DUMP_TEXTURE_NAMES";
            Button_DumpTextures.Click += Button_DumpTextures_Click;
            // 
            // GirdView_Materials
            // 
            GirdView_Materials.AllowUserToAddRows = false;
            GirdView_Materials.AllowUserToDeleteRows = false;
            GirdView_Materials.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            GirdView_Materials.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            GirdView_Materials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GirdView_Materials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { columnName, columnHash });
            GirdView_Materials.Location = new System.Drawing.Point(4, 62);
            GirdView_Materials.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GirdView_Materials.Name = "GirdView_Materials";
            GirdView_Materials.ReadOnly = true;
            GirdView_Materials.Size = new System.Drawing.Size(384, 399);
            GirdView_Materials.TabIndex = 3;
            GirdView_Materials.RowEnter += OnMaterialSelected;
            // 
            // columnName
            // 
            columnName.HeaderText = "Name";
            columnName.Name = "columnName";
            columnName.ReadOnly = true;
            // 
            // columnHash
            // 
            columnHash.HeaderText = "Hash";
            columnHash.Name = "columnHash";
            columnHash.ReadOnly = true;
            // 
            // MTLBrowser
            // 
            MTLBrowser.Filter = "Material Library|*.mtl";
            // 
            // Panel_Main
            // 
            Panel_Main.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Panel_Main.Controls.Add(Button_Search);
            Panel_Main.Controls.Add(Label_SearchType);
            Panel_Main.Controls.Add(ComboBox_SearchType);
            Panel_Main.Controls.Add(GirdView_Materials);
            Panel_Main.Controls.Add(MaterialSearch);
            Panel_Main.Controls.Add(MaterialGrid);
            Panel_Main.Location = new System.Drawing.Point(0, 32);
            Panel_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Panel_Main.Name = "Panel_Main";
            Panel_Main.Size = new System.Drawing.Size(916, 465);
            Panel_Main.TabIndex = 4;
            // 
            // Button_Search
            // 
            Button_Search.Location = new System.Drawing.Point(254, 29);
            Button_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_Search.Name = "Button_Search";
            Button_Search.Size = new System.Drawing.Size(133, 27);
            Button_Search.TabIndex = 6;
            Button_Search.Text = "$SEARCH";
            Button_Search.UseVisualStyleBackColor = true;
            Button_Search.Click += Button_Search_Click;
            // 
            // Label_SearchType
            // 
            Label_SearchType.AutoSize = true;
            Label_SearchType.Location = new System.Drawing.Point(9, 8);
            Label_SearchType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SearchType.Name = "Label_SearchType";
            Label_SearchType.Size = new System.Drawing.Size(121, 15);
            Label_SearchType.TabIndex = 5;
            Label_SearchType.Text = "$LABEL_SEARCHTYPE";
            // 
            // ComboBox_SearchType
            // 
            ComboBox_SearchType.FormattingEnabled = true;
            ComboBox_SearchType.Items.AddRange(new object[] { "$LABEL_MATERIALNAME", "$LABEL_TEXTURENAME", "$LABEL_MATERIALHASH", "$LABEL_SHADERID", "$LABEL_SHADERHASH" });
            ComboBox_SearchType.Location = new System.Drawing.Point(173, 3);
            ComboBox_SearchType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_SearchType.Name = "ComboBox_SearchType";
            ComboBox_SearchType.Size = new System.Drawing.Size(214, 23);
            ComboBox_SearchType.TabIndex = 4;
            ComboBox_SearchType.SelectedIndexChanged += SearchType_OnIndexChanged;
            // 
            // MergePanel
            // 
            MergePanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MergePanel.Controls.Add(SelectAllNewButton);
            MergePanel.Controls.Add(SelectAllOverwriteButton);
            MergePanel.Controls.Add(NewMaterialLabel);
            MergePanel.Controls.Add(OverWriteLabel);
            MergePanel.Controls.Add(NewMatListBox);
            MergePanel.Controls.Add(CancelButton);
            MergePanel.Controls.Add(MergeButton);
            MergePanel.Controls.Add(OverwriteListBox);
            MergePanel.Location = new System.Drawing.Point(0, 32);
            MergePanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MergePanel.Name = "MergePanel";
            MergePanel.Padding = new System.Windows.Forms.Padding(6);
            MergePanel.Size = new System.Drawing.Size(916, 465);
            MergePanel.TabIndex = 4;
            // 
            // SelectAllNewButton
            // 
            SelectAllNewButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            SelectAllNewButton.Location = new System.Drawing.Point(495, 32);
            SelectAllNewButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelectAllNewButton.Name = "SelectAllNewButton";
            SelectAllNewButton.Size = new System.Drawing.Size(117, 27);
            SelectAllNewButton.TabIndex = 8;
            SelectAllNewButton.Text = "$SELECT_ALL";
            SelectAllNewButton.UseVisualStyleBackColor = true;
            SelectAllNewButton.Click += SelectAllNewButton_Click;
            // 
            // SelectAllOverwriteButton
            // 
            SelectAllOverwriteButton.Location = new System.Drawing.Point(293, 32);
            SelectAllOverwriteButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelectAllOverwriteButton.Name = "SelectAllOverwriteButton";
            SelectAllOverwriteButton.Size = new System.Drawing.Size(117, 27);
            SelectAllOverwriteButton.TabIndex = 7;
            SelectAllOverwriteButton.Text = "$SELECT_ALL";
            SelectAllOverwriteButton.UseVisualStyleBackColor = true;
            SelectAllOverwriteButton.Click += SelectAllOverwriteButton_Click;
            // 
            // NewMaterialLabel
            // 
            NewMaterialLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            NewMaterialLabel.AutoSize = true;
            NewMaterialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NewMaterialLabel.Location = new System.Drawing.Point(615, 6);
            NewMaterialLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            NewMaterialLabel.Name = "NewMaterialLabel";
            NewMaterialLabel.Size = new System.Drawing.Size(96, 17);
            NewMaterialLabel.TabIndex = 6;
            NewMaterialLabel.Text = "New Materials";
            // 
            // OverWriteLabel
            // 
            OverWriteLabel.AutoSize = true;
            OverWriteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            OverWriteLabel.Location = new System.Drawing.Point(9, 6);
            OverWriteLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            OverWriteLabel.Name = "OverWriteLabel";
            OverWriteLabel.Size = new System.Drawing.Size(134, 17);
            OverWriteLabel.TabIndex = 5;
            OverWriteLabel.Text = "Conflicting Materials";
            // 
            // NewMatListBox
            // 
            NewMatListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            NewMatListBox.FormattingEnabled = true;
            NewMatListBox.Location = new System.Drawing.Point(618, 27);
            NewMatListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NewMatListBox.Name = "NewMatListBox";
            NewMatListBox.Size = new System.Drawing.Size(283, 418);
            NewMatListBox.TabIndex = 4;
            // 
            // CancelButton
            // 
            CancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            CancelButton.Location = new System.Drawing.Point(293, 420);
            CancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new System.Drawing.Size(117, 27);
            CancelButton.TabIndex = 3;
            CancelButton.Text = "$CANCEL";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += Button_Cancel_Click;
            // 
            // MergeButton
            // 
            MergeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            MergeButton.Location = new System.Drawing.Point(495, 420);
            MergeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MergeButton.Name = "MergeButton";
            MergeButton.Size = new System.Drawing.Size(117, 27);
            MergeButton.TabIndex = 2;
            MergeButton.Text = "$MERGE";
            MergeButton.UseVisualStyleBackColor = true;
            MergeButton.Click += Button_Merge_Click;
            // 
            // OverwriteListBox
            // 
            OverwriteListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            OverwriteListBox.FormattingEnabled = true;
            OverwriteListBox.Location = new System.Drawing.Point(9, 27);
            OverwriteListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            OverwriteListBox.Name = "OverwriteListBox";
            OverwriteListBox.Size = new System.Drawing.Size(276, 418);
            OverwriteListBox.TabIndex = 0;
            // 
            // MTLSaveDialog
            // 
            MTLSaveDialog.Filter = "Material Library|*.mtl";
            // 
            // MaterialEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(916, 497);
            Controls.Add(toolStrip1);
            Controls.Add(Panel_Main);
            Controls.Add(MergePanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MaterialEditor";
            Text = "Material Library Editor";
            FormClosing += MaterialEditor_Closing;
            KeyUp += MaterialEditor_OnKeyUp;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GirdView_Materials).EndInit();
            Panel_Main.ResumeLayout(false);
            Panel_Main.PerformLayout();
            MergePanel.ResumeLayout(false);
            MergePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MaterialSearch;
        private System.Windows.Forms.PropertyGrid MaterialGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Open;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem Button_AddMaterial;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.DataGridView GirdView_Materials;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHash;
        private System.Windows.Forms.ToolStripMenuItem Button_MergeMTL;
        private System.Windows.Forms.OpenFileDialog MTLBrowser;
        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Panel MergePanel;
        private System.Windows.Forms.CheckedListBox OverwriteListBox;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.CheckedListBox NewMatListBox;
        private System.Windows.Forms.Label OverWriteLabel;
        private System.Windows.Forms.Label NewMaterialLabel;
        private System.Windows.Forms.Button SelectAllNewButton;
        private System.Windows.Forms.Button SelectAllOverwriteButton;
        private System.Windows.Forms.Label Label_SearchType;
        private System.Windows.Forms.ComboBox ComboBox_SearchType;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportSelected;
        private System.Windows.Forms.SaveFileDialog MTLSaveDialog;
        private System.Windows.Forms.Button Button_Search;
        private System.Windows.Forms.ToolStripDropDownButton Button_Debug;
        private System.Windows.Forms.ToolStripMenuItem Button_DumpTextures;
    }
}