namespace Forms.OptionControls
{
    partial class MTLOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupMTL = new System.Windows.Forms.GroupBox();
            removeSelectedButton = new System.Windows.Forms.Button();
            addLibraryButton = new System.Windows.Forms.Button();
            MTLListBox = new System.Windows.Forms.ListBox();
            MTLsToLoadText = new System.Windows.Forms.Label();
            MTLBrowser = new System.Windows.Forms.OpenFileDialog();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            groupMTL.SuspendLayout();
            SuspendLayout();
            // 
            // groupMTL
            // 
            groupMTL.AutoSize = true;
            groupMTL.Controls.Add(removeSelectedButton);
            groupMTL.Controls.Add(addLibraryButton);
            groupMTL.Controls.Add(MTLListBox);
            groupMTL.Controls.Add(MTLsToLoadText);
            groupMTL.Dock = System.Windows.Forms.DockStyle.Fill;
            groupMTL.Location = new System.Drawing.Point(0, 0);
            groupMTL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupMTL.Name = "groupMTL";
            groupMTL.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupMTL.Size = new System.Drawing.Size(517, 412);
            groupMTL.TabIndex = 0;
            groupMTL.TabStop = false;
            groupMTL.Text = "$MATERIAL_LIBS";
            // 
            // removeSelectedButton
            // 
            removeSelectedButton.Location = new System.Drawing.Point(115, 252);
            removeSelectedButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            removeSelectedButton.Name = "removeSelectedButton";
            removeSelectedButton.Size = new System.Drawing.Size(120, 27);
            removeSelectedButton.TabIndex = 3;
            removeSelectedButton.Text = "$MATERIAL_LIB_REMOVE";
            removeSelectedButton.UseVisualStyleBackColor = true;
            removeSelectedButton.Click += RemoveSelected_Click;
            // 
            // addLibraryButton
            // 
            addLibraryButton.Location = new System.Drawing.Point(12, 252);
            addLibraryButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            addLibraryButton.Name = "addLibraryButton";
            addLibraryButton.Size = new System.Drawing.Size(97, 27);
            addLibraryButton.TabIndex = 2;
            addLibraryButton.Text = "$MATERIAL_LIB_ADD";
            addLibraryButton.UseVisualStyleBackColor = true;
            addLibraryButton.Click += AddLibrary_Click;
            // 
            // MTLListBox
            // 
            MTLListBox.FormattingEnabled = true;
            MTLListBox.HorizontalScrollbar = true;
            MTLListBox.ItemHeight = 15;
            MTLListBox.Location = new System.Drawing.Point(12, 43);
            MTLListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MTLListBox.Name = "MTLListBox";
            MTLListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            MTLListBox.Size = new System.Drawing.Size(479, 199);
            MTLListBox.TabIndex = 1;
            // 
            // MTLsToLoadText
            // 
            MTLsToLoadText.AutoSize = true;
            MTLsToLoadText.Location = new System.Drawing.Point(8, 23);
            MTLsToLoadText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MTLsToLoadText.Name = "MTLsToLoadText";
            MTLsToLoadText.Size = new System.Drawing.Size(145, 15);
            MTLsToLoadText.TabIndex = 0;
            MTLsToLoadText.Text = "$MATERIAL_LIB_SELECTED";
            // 
            // MTLBrowser
            // 
            MTLBrowser.Filter = "MTL Files|*.mtl|All files|*.*";
            MTLBrowser.Multiselect = true;
            // 
            // MTLOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupMTL);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MTLOptions";
            Size = new System.Drawing.Size(517, 412);
            groupMTL.ResumeLayout(false);
            groupMTL.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMTL;
        private System.Windows.Forms.Label MTLsToLoadText;
        private System.Windows.Forms.ListBox MTLListBox;
        private System.Windows.Forms.Button addLibraryButton;
        private System.Windows.Forms.Button removeSelectedButton;
        private System.Windows.Forms.OpenFileDialog MTLBrowser;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
