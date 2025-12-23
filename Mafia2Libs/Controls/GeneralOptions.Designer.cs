namespace Forms.OptionControls
{
    partial class GeneralOptions
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
            groupGeneral = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            languageComboBox = new System.Windows.Forms.ComboBox();
            debugLoggingCheckbox = new System.Windows.Forms.CheckBox();
            browseButton = new System.Windows.Forms.Button();
            M2DirectoryBox = new System.Windows.Forms.TextBox();
            M2Label = new System.Windows.Forms.Label();
            MafiaIIBrowser = new System.Windows.Forms.FolderBrowserDialog();
            groupBoxSplitter = new System.Windows.Forms.SplitContainer();
            groupDiscordRPC = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            DiscordStateTextBox = new System.Windows.Forms.TextBox();
            DiscordElapsedCheckBox = new System.Windows.Forms.CheckBox();
            DiscordStateCheckBox = new System.Windows.Forms.CheckBox();
            DiscordDetailsCheckBox = new System.Windows.Forms.CheckBox();
            DiscordEnabledCheckBox = new System.Windows.Forms.CheckBox();
            groupGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupBoxSplitter).BeginInit();
            groupBoxSplitter.Panel1.SuspendLayout();
            groupBoxSplitter.Panel2.SuspendLayout();
            groupBoxSplitter.SuspendLayout();
            groupDiscordRPC.SuspendLayout();
            SuspendLayout();
            // 
            // groupGeneral
            // 
            groupGeneral.Controls.Add(label1);
            groupGeneral.Controls.Add(languageComboBox);
            groupGeneral.Controls.Add(debugLoggingCheckbox);
            groupGeneral.Controls.Add(browseButton);
            groupGeneral.Controls.Add(M2DirectoryBox);
            groupGeneral.Controls.Add(M2Label);
            groupGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            groupGeneral.Location = new System.Drawing.Point(0, 0);
            groupGeneral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupGeneral.Name = "groupGeneral";
            groupGeneral.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupGeneral.Size = new System.Drawing.Size(590, 181);
            groupGeneral.TabIndex = 1;
            groupGeneral.TabStop = false;
            groupGeneral.Text = "$GENERAL";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 69);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(122, 15);
            label1.TabIndex = 6;
            label1.Text = "$LANGUAGE_OPTION";
            // 
            // languageComboBox
            // 
            languageComboBox.FormattingEnabled = true;
            languageComboBox.Items.AddRange(new object[] { "$LANGUAGE_ENGLISH", "$LANGUAGE_RUSSIAN", "$LANGUAGE_CZECH", "$LANGUAGE_POLISH", "French", "Slovak", "$LANGUAGE_ARABIC" });
            languageComboBox.Location = new System.Drawing.Point(10, 88);
            languageComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            languageComboBox.Name = "languageComboBox";
            languageComboBox.Size = new System.Drawing.Size(140, 23);
            languageComboBox.TabIndex = 5;
            languageComboBox.SelectedIndexChanged += IndexChange;
            // 
            // debugLoggingCheckbox
            // 
            debugLoggingCheckbox.AutoSize = true;
            debugLoggingCheckbox.Location = new System.Drawing.Point(10, 119);
            debugLoggingCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            debugLoggingCheckbox.Name = "debugLoggingCheckbox";
            debugLoggingCheckbox.Size = new System.Drawing.Size(172, 19);
            debugLoggingCheckbox.TabIndex = 4;
            debugLoggingCheckbox.Text = "$ENABLE_DEBUG_LOGGING";
            debugLoggingCheckbox.UseVisualStyleBackColor = true;
            debugLoggingCheckbox.CheckedChanged += DebugLoggingCheckBox_CheckedChanged;
            // 
            // browseButton
            // 
            browseButton.Location = new System.Drawing.Point(405, 43);
            browseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            browseButton.Name = "browseButton";
            browseButton.Size = new System.Drawing.Size(30, 23);
            browseButton.TabIndex = 2;
            browseButton.Text = "...";
            browseButton.UseVisualStyleBackColor = true;
            browseButton.Click += BrowseButton_Click;
            // 
            // M2DirectoryBox
            // 
            M2DirectoryBox.Location = new System.Drawing.Point(10, 43);
            M2DirectoryBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            M2DirectoryBox.Name = "M2DirectoryBox";
            M2DirectoryBox.Size = new System.Drawing.Size(387, 23);
            M2DirectoryBox.TabIndex = 1;
            M2DirectoryBox.TextChanged += M2Directory_TextChanged;
            // 
            // M2Label
            // 
            M2Label.AutoSize = true;
            M2Label.Location = new System.Drawing.Point(7, 24);
            M2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            M2Label.Name = "M2Label";
            M2Label.Size = new System.Drawing.Size(95, 15);
            M2Label.TabIndex = 0;
            M2Label.Text = "$MII_DIRECTORY";
            // 
            // MafiaIIBrowser
            // 
            MafiaIIBrowser.Description = "$SELECT_MII_FOLDER";
            // 
            // groupBoxSplitter
            // 
            groupBoxSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxSplitter.Location = new System.Drawing.Point(0, 0);
            groupBoxSplitter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxSplitter.Name = "groupBoxSplitter";
            groupBoxSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // groupBoxSplitter.Panel1
            // 
            groupBoxSplitter.Panel1.Controls.Add(groupGeneral);
            // 
            // groupBoxSplitter.Panel2
            // 
            groupBoxSplitter.Panel2.Controls.Add(groupDiscordRPC);
            groupBoxSplitter.Size = new System.Drawing.Size(590, 358);
            groupBoxSplitter.SplitterDistance = 181;
            groupBoxSplitter.SplitterWidth = 5;
            groupBoxSplitter.TabIndex = 2;
            // 
            // groupDiscordRPC
            // 
            groupDiscordRPC.Controls.Add(label2);
            groupDiscordRPC.Controls.Add(DiscordStateTextBox);
            groupDiscordRPC.Controls.Add(DiscordElapsedCheckBox);
            groupDiscordRPC.Controls.Add(DiscordStateCheckBox);
            groupDiscordRPC.Controls.Add(DiscordDetailsCheckBox);
            groupDiscordRPC.Controls.Add(DiscordEnabledCheckBox);
            groupDiscordRPC.Dock = System.Windows.Forms.DockStyle.Fill;
            groupDiscordRPC.Location = new System.Drawing.Point(0, 0);
            groupDiscordRPC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupDiscordRPC.Name = "groupDiscordRPC";
            groupDiscordRPC.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupDiscordRPC.Size = new System.Drawing.Size(590, 172);
            groupDiscordRPC.TabIndex = 0;
            groupDiscordRPC.TabStop = false;
            groupDiscordRPC.Text = "$DISCORD_RICH_PRESENCE";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(7, 123);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(125, 15);
            label2.TabIndex = 7;
            label2.Text = "$DISCORDSTATELABEL";
            // 
            // DiscordStateTextBox
            // 
            DiscordStateTextBox.Location = new System.Drawing.Point(7, 142);
            DiscordStateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DiscordStateTextBox.Name = "DiscordStateTextBox";
            DiscordStateTextBox.Size = new System.Drawing.Size(425, 23);
            DiscordStateTextBox.TabIndex = 7;
            DiscordStateTextBox.TextChanged += DiscordStateTextBox_TextChanged;
            // 
            // DiscordElapsedCheckBox
            // 
            DiscordElapsedCheckBox.AutoSize = true;
            DiscordElapsedCheckBox.Location = new System.Drawing.Point(7, 102);
            DiscordElapsedCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DiscordElapsedCheckBox.Name = "DiscordElapsedCheckBox";
            DiscordElapsedCheckBox.Size = new System.Drawing.Size(211, 19);
            DiscordElapsedCheckBox.TabIndex = 3;
            DiscordElapsedCheckBox.Text = "$DISCORD_TOGGLE_ELAPSED_TIME";
            DiscordElapsedCheckBox.UseVisualStyleBackColor = true;
            DiscordElapsedCheckBox.CheckedChanged += DiscordElapsedCheckBox_CheckedChanged;
            // 
            // DiscordStateCheckBox
            // 
            DiscordStateCheckBox.AutoSize = true;
            DiscordStateCheckBox.Location = new System.Drawing.Point(8, 75);
            DiscordStateCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DiscordStateCheckBox.Name = "DiscordStateCheckBox";
            DiscordStateCheckBox.Size = new System.Drawing.Size(163, 19);
            DiscordStateCheckBox.TabIndex = 2;
            DiscordStateCheckBox.Text = "$DISCORD_TOGGLE_STATE";
            DiscordStateCheckBox.UseVisualStyleBackColor = true;
            DiscordStateCheckBox.CheckedChanged += DiscordStateCheckBox_CheckedChanged;
            // 
            // DiscordDetailsCheckBox
            // 
            DiscordDetailsCheckBox.AutoSize = true;
            DiscordDetailsCheckBox.Location = new System.Drawing.Point(8, 48);
            DiscordDetailsCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DiscordDetailsCheckBox.Name = "DiscordDetailsCheckBox";
            DiscordDetailsCheckBox.Size = new System.Drawing.Size(175, 19);
            DiscordDetailsCheckBox.TabIndex = 1;
            DiscordDetailsCheckBox.Text = "$DISCORD_TOGGLE_DETAILS";
            DiscordDetailsCheckBox.UseVisualStyleBackColor = true;
            DiscordDetailsCheckBox.CheckedChanged += DiscordDetailsCheckBox_CheckedChanged;
            // 
            // DiscordEnabledCheckBox
            // 
            DiscordEnabledCheckBox.AutoSize = true;
            DiscordEnabledCheckBox.Location = new System.Drawing.Point(8, 22);
            DiscordEnabledCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DiscordEnabledCheckBox.Name = "DiscordEnabledCheckBox";
            DiscordEnabledCheckBox.Size = new System.Drawing.Size(220, 19);
            DiscordEnabledCheckBox.TabIndex = 0;
            DiscordEnabledCheckBox.Text = "$DISCORD_TOGGLE_RICH_PRESENCE";
            DiscordEnabledCheckBox.UseVisualStyleBackColor = true;
            DiscordEnabledCheckBox.CheckedChanged += DiscordEnabledCheckBox_CheckedChanged;
            // 
            // GeneralOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBoxSplitter);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "GeneralOptions";
            Size = new System.Drawing.Size(590, 358);
            groupGeneral.ResumeLayout(false);
            groupGeneral.PerformLayout();
            groupBoxSplitter.Panel1.ResumeLayout(false);
            groupBoxSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupBoxSplitter).EndInit();
            groupBoxSplitter.ResumeLayout(false);
            groupDiscordRPC.ResumeLayout(false);
            groupDiscordRPC.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.TextBox M2DirectoryBox;
        private System.Windows.Forms.Label M2Label;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.SplitContainer groupBoxSplitter;
        private System.Windows.Forms.GroupBox groupDiscordRPC;
        private System.Windows.Forms.CheckBox DiscordEnabledCheckBox;
        private System.Windows.Forms.CheckBox DiscordElapsedCheckBox;
        private System.Windows.Forms.CheckBox DiscordStateCheckBox;
        private System.Windows.Forms.CheckBox DiscordDetailsCheckBox;
        private System.Windows.Forms.CheckBox debugLoggingCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DiscordStateTextBox;
    }
}
