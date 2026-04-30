
namespace Mafia2Tool.Forms
{
    partial class AboutBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            LabelToolkitName = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            ThanksBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            ProjectLink = new System.Windows.Forms.LinkLabel();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            CloseButton = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // LabelToolkitName
            // 
            LabelToolkitName.AutoSize = true;
            LabelToolkitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelToolkitName.Location = new System.Drawing.Point(105, 22);
            LabelToolkitName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelToolkitName.Name = "LabelToolkitName";
            LabelToolkitName.Size = new System.Drawing.Size(84, 16);
            LabelToolkitName.TabIndex = 0;
            LabelToolkitName.Text = "Mafia Toolkit";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(ThanksBox);
            groupBox1.Location = new System.Drawing.Point(14, 104);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(447, 240);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thanks to:";
            // 
            // ThanksBox
            // 
            ThanksBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ThanksBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ThanksBox.Location = new System.Drawing.Point(7, 22);
            ThanksBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ThanksBox.Multiline = true;
            ThanksBox.Name = "ThanksBox";
            ThanksBox.ReadOnly = true;
            ThanksBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ThanksBox.Size = new System.Drawing.Size(433, 211);
            ThanksBox.TabIndex = 99;
            ThanksBox.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(106, 63);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 15);
            label1.TabIndex = 3;
            label1.Text = "by Greavesy";
            // 
            // ProjectLink
            // 
            ProjectLink.AutoSize = true;
            ProjectLink.LinkColor = System.Drawing.Color.Blue;
            ProjectLink.Location = new System.Drawing.Point(106, 43);
            ProjectLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ProjectLink.Name = "ProjectLink";
            ProjectLink.Size = new System.Drawing.Size(255, 15);
            ProjectLink.TabIndex = 4;
            ProjectLink.TabStop = true;
            ProjectLink.Text = "https://github.com/Greavesy1899/MafiaToolkit";
            ProjectLink.VisitedLinkColor = System.Drawing.Color.Blue;
            ProjectLink.LinkClicked += ProjectLink_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(14, 14);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(75, 74);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CloseButton.Location = new System.Drawing.Point(340, 351);
            CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(121, 27);
            CloseButton.TabIndex = 0;
            CloseButton.Text = "Ok";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // AboutBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new System.Drawing.Size(469, 387);
            Controls.Add(CloseButton);
            Controls.Add(pictureBox1);
            Controls.Add(ProjectLink);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Controls.Add(LabelToolkitName);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutBox";
            Text = "About";
            Load += AboutBox_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelToolkitName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel ProjectLink;
        private System.Windows.Forms.TextBox ThanksBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CloseButton;
    }
}