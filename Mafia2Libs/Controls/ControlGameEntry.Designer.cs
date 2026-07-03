using System;

namespace Mafia2Tool.Controls
{
    partial class ControlGameEntry
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
            Picture_GameIcon = new System.Windows.Forms.PictureBox();
            Label_GameName = new System.Windows.Forms.Label();
            TextBox_FolderPath = new System.Windows.Forms.TextBox();
            Label_FolderPath = new System.Windows.Forms.Label();
            Picture_Status = new System.Windows.Forms.PictureBox();
            Button_Start = new System.Windows.Forms.Button();
            Button_SelectFolder = new System.Windows.Forms.Button();
            Label_GameDescription = new System.Windows.Forms.Label();
            Label_GameType = new System.Windows.Forms.Label();
            FolderDialog_MafiaFolder = new System.Windows.Forms.FolderBrowserDialog();
            Label_MissingImage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)Picture_GameIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Picture_Status).BeginInit();
            SuspendLayout();
            // 
            // Picture_GameIcon
            // 
            Picture_GameIcon.InitialImage = null;
            Picture_GameIcon.Location = new System.Drawing.Point(4, 46);
            Picture_GameIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Picture_GameIcon.Name = "Picture_GameIcon";
            Picture_GameIcon.Size = new System.Drawing.Size(365, 173);
            Picture_GameIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            Picture_GameIcon.TabIndex = 1;
            Picture_GameIcon.TabStop = false;
            // 
            // Label_GameName
            // 
            Label_GameName.AutoSize = true;
            Label_GameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            Label_GameName.Location = new System.Drawing.Point(4, 0);
            Label_GameName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_GameName.Name = "Label_GameName";
            Label_GameName.Size = new System.Drawing.Size(127, 15);
            Label_GameName.TabIndex = 2;
            Label_GameName.Text = "Label_GameName";
            // 
            // TextBox_FolderPath
            // 
            TextBox_FolderPath.Location = new System.Drawing.Point(4, 240);
            TextBox_FolderPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_FolderPath.Name = "TextBox_FolderPath";
            TextBox_FolderPath.Size = new System.Drawing.Size(301, 23);
            TextBox_FolderPath.TabIndex = 4;
            TextBox_FolderPath.TextChanged += TextBox_FolderPath_OnTextChanged;
            // 
            // Label_FolderPath
            // 
            Label_FolderPath.AutoSize = true;
            Label_FolderPath.Location = new System.Drawing.Point(4, 222);
            Label_FolderPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FolderPath.Name = "Label_FolderPath";
            Label_FolderPath.Size = new System.Drawing.Size(156, 15);
            Label_FolderPath.TabIndex = 5;
            Label_FolderPath.Text = "$GAMEENTRY_FOLDERPATH";
            // 
            // Picture_Status
            // 
            Picture_Status.Location = new System.Drawing.Point(313, 240);
            Picture_Status.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Picture_Status.Name = "Picture_Status";
            Picture_Status.Size = new System.Drawing.Size(56, 56);
            Picture_Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            Picture_Status.TabIndex = 6;
            Picture_Status.TabStop = false;
            // 
            // Button_Start
            // 
            Button_Start.Location = new System.Drawing.Point(160, 269);
            Button_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_Start.Name = "Button_Start";
            Button_Start.Size = new System.Drawing.Size(145, 27);
            Button_Start.TabIndex = 7;
            Button_Start.Text = "$START_TOOLKIT";
            Button_Start.UseVisualStyleBackColor = true;
            // 
            // Button_SelectFolder
            // 
            Button_SelectFolder.Location = new System.Drawing.Point(4, 269);
            Button_SelectFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_SelectFolder.Name = "Button_SelectFolder";
            Button_SelectFolder.Size = new System.Drawing.Size(145, 27);
            Button_SelectFolder.TabIndex = 10;
            Button_SelectFolder.Text = "$SELECT_FOLDER";
            Button_SelectFolder.UseVisualStyleBackColor = true;
            Button_SelectFolder.Click += Button_SelectFolder_OnClick;
            // 
            // Label_GameDescription
            // 
            Label_GameDescription.AutoSize = true;
            Label_GameDescription.Location = new System.Drawing.Point(4, 21);
            Label_GameDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_GameDescription.Name = "Label_GameDescription";
            Label_GameDescription.Size = new System.Drawing.Size(131, 15);
            Label_GameDescription.TabIndex = 11;
            Label_GameDescription.Text = "Label_GameDescription";
            // 
            // Label_GameType
            // 
            Label_GameType.AutoSize = true;
            Label_GameType.Location = new System.Drawing.Point(313, 222);
            Label_GameType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_GameType.Name = "Label_GameType";
            Label_GameType.Size = new System.Drawing.Size(62, 15);
            Label_GameType.TabIndex = 12;
            Label_GameType.Text = "GameType";
            // 
            // FolderDialog_MafiaFolder
            // 
            FolderDialog_MafiaFolder.Description = "$SELECT_MII_FOLDER";
            // 
            // Label_MissingImage
            // 
            Label_MissingImage.AutoSize = true;
            Label_MissingImage.Location = new System.Drawing.Point(96, 130);
            Label_MissingImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MissingImage.Name = "Label_MissingImage";
            Label_MissingImage.Size = new System.Drawing.Size(92, 15);
            Label_MissingImage.TabIndex = 13;
            Label_MissingImage.Text = "MISSING IMAGE";
            // 
            // ControlGameEntry
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(Label_MissingImage);
            Controls.Add(Label_GameType);
            Controls.Add(Label_GameDescription);
            Controls.Add(Button_SelectFolder);
            Controls.Add(Button_Start);
            Controls.Add(Picture_Status);
            Controls.Add(Label_FolderPath);
            Controls.Add(TextBox_FolderPath);
            Controls.Add(Label_GameName);
            Controls.Add(Picture_GameIcon);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ControlGameEntry";
            Size = new System.Drawing.Size(375, 308);
            ((System.ComponentModel.ISupportInitialize)Picture_GameIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)Picture_Status).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Picture_GameIcon;
        private System.Windows.Forms.Label Label_GameName;
        private System.Windows.Forms.TextBox TextBox_FolderPath;
        private System.Windows.Forms.Label Label_FolderPath;
        private System.Windows.Forms.PictureBox Picture_Status;
        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.Button Button_SelectFolder;
        private System.Windows.Forms.Label Label_GameDescription;
        private System.Windows.Forms.Label Label_GameType;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog_MafiaFolder;
        private System.Windows.Forms.Label Label_MissingImage;
    }
}
