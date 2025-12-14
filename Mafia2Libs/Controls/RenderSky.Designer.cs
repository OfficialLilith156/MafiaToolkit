namespace Forms.OptionControls
{
    partial class RenderSky
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
            RenderGroup = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            BrowseButton5 = new System.Windows.Forms.Button();
            TexDirectoryBox5 = new System.Windows.Forms.TextBox();
            TexBrowser = new System.Windows.Forms.FolderBrowserDialog();
            RenderGroup.SuspendLayout();
            SuspendLayout();
            // 
            // RenderGroup
            // 
            RenderGroup.Controls.Add(label1);
            RenderGroup.Controls.Add(BrowseButton5);
            RenderGroup.Controls.Add(TexDirectoryBox5);
            RenderGroup.Location = new System.Drawing.Point(0, 0);
            RenderGroup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RenderGroup.Name = "RenderGroup";
            RenderGroup.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RenderGroup.Size = new System.Drawing.Size(643, 383);
            RenderGroup.TabIndex = 2;
            RenderGroup.TabStop = false;
            RenderGroup.Text = "$RENDER_OPTIONS";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(8, 19);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 15);
            label1.TabIndex = 25;
            label1.Text = "Sky Texture";
            // 
            // BrowseButton5
            // 
            BrowseButton5.Location = new System.Drawing.Point(402, 37);
            BrowseButton5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BrowseButton5.Name = "BrowseButton5";
            BrowseButton5.Size = new System.Drawing.Size(30, 23);
            BrowseButton5.TabIndex = 24;
            BrowseButton5.Text = "...";
            BrowseButton5.UseVisualStyleBackColor = true;
            BrowseButton5.Click += BrowseButton5_Click;
            // 
            // TexDirectoryBox5
            // 
            TexDirectoryBox5.Location = new System.Drawing.Point(7, 37);
            TexDirectoryBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TexDirectoryBox5.Name = "TexDirectoryBox5";
            TexDirectoryBox5.Size = new System.Drawing.Size(387, 23);
            TexDirectoryBox5.TabIndex = 23;
            // 
            // TexBrowser
            // 
            TexBrowser.Description = "$SELECT_TEX_FOLDER";
            // 
            // RenderSky
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(RenderGroup);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "RenderSky";
            Size = new System.Drawing.Size(647, 386);
            RenderGroup.ResumeLayout(false);
            RenderGroup.PerformLayout();
            ResumeLayout(false);
        }



        #endregion

        private System.Windows.Forms.GroupBox RenderGroup;
        private System.Windows.Forms.FolderBrowserDialog TexBrowser;
        private System.Windows.Forms.Button BrowseButton5;
        private System.Windows.Forms.TextBox TexDirectoryBox5;
        private System.Windows.Forms.Label label1;
    }
}
