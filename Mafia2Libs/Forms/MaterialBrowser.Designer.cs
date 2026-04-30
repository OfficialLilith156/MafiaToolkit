namespace Mafia2Tool.Forms
{
    partial class MaterialBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialBrowser));
            FlowPanel_Materials = new System.Windows.Forms.FlowLayoutPanel();
            Label_SelectMatLib = new System.Windows.Forms.Label();
            Label_SearchBar = new System.Windows.Forms.Label();
            ComboBox_Materials = new System.Windows.Forms.ComboBox();
            TextBox_SearchBar = new System.Windows.Forms.TextBox();
            Button_Search = new System.Windows.Forms.Button();
            Label_MaterialCount = new System.Windows.Forms.Label();
            ComboBox_SearchType = new System.Windows.Forms.ComboBox();
            Label_SearchType = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // FlowPanel_Materials
            // 
            FlowPanel_Materials.AutoScroll = true;
            FlowPanel_Materials.Location = new System.Drawing.Point(0, 60);
            FlowPanel_Materials.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FlowPanel_Materials.Name = "FlowPanel_Materials";
            FlowPanel_Materials.Size = new System.Drawing.Size(933, 459);
            FlowPanel_Materials.TabIndex = 3;
            // 
            // Label_SelectMatLib
            // 
            Label_SelectMatLib.AutoSize = true;
            Label_SelectMatLib.Location = new System.Drawing.Point(14, 10);
            Label_SelectMatLib.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SelectMatLib.Name = "Label_SelectMatLib";
            Label_SelectMatLib.Size = new System.Drawing.Size(129, 15);
            Label_SelectMatLib.TabIndex = 4;
            Label_SelectMatLib.Text = "$LABEL_SELECTMATLIB";
            // 
            // Label_SearchBar
            // 
            Label_SearchBar.AutoSize = true;
            Label_SearchBar.Location = new System.Drawing.Point(499, 10);
            Label_SearchBar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SearchBar.Name = "Label_SearchBar";
            Label_SearchBar.Size = new System.Drawing.Size(117, 15);
            Label_SearchBar.TabIndex = 5;
            Label_SearchBar.Text = "$LABEL_SEARCHBAR";
            // 
            // ComboBox_Materials
            // 
            ComboBox_Materials.FormattingEnabled = true;
            ComboBox_Materials.Location = new System.Drawing.Point(14, 29);
            ComboBox_Materials.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_Materials.Name = "ComboBox_Materials";
            ComboBox_Materials.Size = new System.Drawing.Size(220, 23);
            ComboBox_Materials.TabIndex = 6;
            ComboBox_Materials.SelectedIndexChanged += ComboBox_MaterialsSelectedIndexChanged;
            // 
            // TextBox_SearchBar
            // 
            TextBox_SearchBar.Location = new System.Drawing.Point(503, 29);
            TextBox_SearchBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_SearchBar.Name = "TextBox_SearchBar";
            TextBox_SearchBar.Size = new System.Drawing.Size(261, 23);
            TextBox_SearchBar.TabIndex = 7;
            // 
            // Button_Search
            // 
            Button_Search.Location = new System.Drawing.Point(771, 27);
            Button_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_Search.Name = "Button_Search";
            Button_Search.Size = new System.Drawing.Size(148, 27);
            Button_Search.TabIndex = 8;
            Button_Search.Text = "$SEARCH";
            Button_Search.UseVisualStyleBackColor = true;
            Button_Search.Click += Button_SearchOnClicked;
            // 
            // Label_MaterialCount
            // 
            Label_MaterialCount.AutoSize = true;
            Label_MaterialCount.Location = new System.Drawing.Point(705, 10);
            Label_MaterialCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MaterialCount.Name = "Label_MaterialCount";
            Label_MaterialCount.Size = new System.Drawing.Size(53, 15);
            Label_MaterialCount.TabIndex = 9;
            Label_MaterialCount.Text = "$COUNT";
            // 
            // ComboBox_SearchType
            // 
            ComboBox_SearchType.FormattingEnabled = true;
            ComboBox_SearchType.Items.AddRange(new object[] { "$LABEL_MATERIALNAME", "$LABEL_TEXTURENAME", "$LABEL_MATERIALHASH", "$LABEL_SHADERID", "$LABEL_SHADERHASH" });
            ComboBox_SearchType.Location = new System.Drawing.Point(259, 28);
            ComboBox_SearchType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_SearchType.Name = "ComboBox_SearchType";
            ComboBox_SearchType.Size = new System.Drawing.Size(220, 23);
            ComboBox_SearchType.TabIndex = 11;
            // 
            // Label_SearchType
            // 
            Label_SearchType.AutoSize = true;
            Label_SearchType.Location = new System.Drawing.Point(255, 10);
            Label_SearchType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SearchType.Name = "Label_SearchType";
            Label_SearchType.Size = new System.Drawing.Size(121, 15);
            Label_SearchType.TabIndex = 10;
            Label_SearchType.Text = "$LABEL_SEARCHTYPE";
            // 
            // MaterialBrowser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(ComboBox_SearchType);
            Controls.Add(Label_SearchType);
            Controls.Add(Label_MaterialCount);
            Controls.Add(Button_Search);
            Controls.Add(TextBox_SearchBar);
            Controls.Add(ComboBox_Materials);
            Controls.Add(Label_SearchBar);
            Controls.Add(Label_SelectMatLib);
            Controls.Add(FlowPanel_Materials);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MaterialBrowser";
            Text = "MatBrowser";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowPanel_Materials;
        private System.Windows.Forms.Label Label_SelectMatLib;
        private System.Windows.Forms.Label Label_SearchBar;
        private System.Windows.Forms.ComboBox ComboBox_Materials;
        private System.Windows.Forms.TextBox TextBox_SearchBar;
        private System.Windows.Forms.Button Button_Search;
        private System.Windows.Forms.Label Label_MaterialCount;
        private System.Windows.Forms.ComboBox ComboBox_SearchType;
        private System.Windows.Forms.Label Label_SearchType;
    }
}