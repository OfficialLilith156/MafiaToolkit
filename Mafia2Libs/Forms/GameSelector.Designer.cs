namespace Mafia2Tool.Forms
{
    partial class GameSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSelector));
            FlowPanel_GamesList = new System.Windows.Forms.FlowLayoutPanel();
            CheckBox_SelectAsDefault = new System.Windows.Forms.CheckBox();
            Label_ToolkitVersion = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // FlowPanel_GamesList
            // 
            FlowPanel_GamesList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FlowPanel_GamesList.AutoScroll = true;
            FlowPanel_GamesList.Location = new System.Drawing.Point(14, 36);
            FlowPanel_GamesList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FlowPanel_GamesList.Name = "FlowPanel_GamesList";
            FlowPanel_GamesList.Size = new System.Drawing.Size(785, 698);
            FlowPanel_GamesList.TabIndex = 0;
            // 
            // CheckBox_SelectAsDefault
            // 
            CheckBox_SelectAsDefault.AutoSize = true;
            CheckBox_SelectAsDefault.Location = new System.Drawing.Point(586, 11);
            CheckBox_SelectAsDefault.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_SelectAsDefault.Name = "CheckBox_SelectAsDefault";
            CheckBox_SelectAsDefault.Size = new System.Drawing.Size(140, 19);
            CheckBox_SelectAsDefault.TabIndex = 1;
            CheckBox_SelectAsDefault.Text = "$SELECT_AS_DEFAULT";
            CheckBox_SelectAsDefault.UseVisualStyleBackColor = true;
            CheckBox_SelectAsDefault.CheckedChanged += CheckBox_SelectAsDefault_OnChecked;
            // 
            // Label_ToolkitVersion
            // 
            Label_ToolkitVersion.AutoSize = true;
            Label_ToolkitVersion.Location = new System.Drawing.Point(14, 15);
            Label_ToolkitVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_ToolkitVersion.Name = "Label_ToolkitVersion";
            Label_ToolkitVersion.Size = new System.Drawing.Size(104, 15);
            Label_ToolkitVersion.TabIndex = 2;
            Label_ToolkitVersion.Text = "TOOLKIT_VERSION";
            // 
            // GameSelector
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(815, 747);
            Controls.Add(Label_ToolkitVersion);
            Controls.Add(CheckBox_SelectAsDefault);
            Controls.Add(FlowPanel_GamesList);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameSelector";
            Text = "GameSelector";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowPanel_GamesList;
        private System.Windows.Forms.CheckBox CheckBox_SelectAsDefault;
        private System.Windows.Forms.Label Label_ToolkitVersion;
    }
}