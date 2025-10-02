namespace Forms.EditorControls
{
    partial class ActorItemAddOption
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
            MafiaIIBrowser = new System.Windows.Forms.FolderBrowserDialog();
            groupGeneral = new System.Windows.Forms.GroupBox();
            framename = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            DefinitionBox = new System.Windows.Forms.TextBox();
            ActorDefinitionLabel = new System.Windows.Forms.Label();
            ActorTypeLabel = new System.Windows.Forms.Label();
            TypeCombo = new System.Windows.Forms.ComboBox();
            groupGeneral.SuspendLayout();
            SuspendLayout();
            // 
            // MafiaIIBrowser
            // 
            MafiaIIBrowser.Description = "$SELECT_MII_FOLDER";
            // 
            // groupGeneral
            // 
            groupGeneral.AutoSize = true;
            groupGeneral.Controls.Add(framename);
            groupGeneral.Controls.Add(label1);
            groupGeneral.Controls.Add(DefinitionBox);
            groupGeneral.Controls.Add(ActorDefinitionLabel);
            groupGeneral.Controls.Add(ActorTypeLabel);
            groupGeneral.Controls.Add(TypeCombo);
            groupGeneral.Location = new System.Drawing.Point(0, 0);
            groupGeneral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupGeneral.Name = "groupGeneral";
            groupGeneral.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupGeneral.Size = new System.Drawing.Size(397, 147);
            groupGeneral.TabIndex = 2;
            groupGeneral.TabStop = false;
            groupGeneral.Text = "$GENERAL";
            // 
            // framename
            // 
            framename.Location = new System.Drawing.Point(212, 62);
            framename.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            framename.Name = "framename";
            framename.Size = new System.Drawing.Size(177, 23);
            framename.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 67);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(132, 15);
            label1.TabIndex = 5;
            label1.Text = "$ACTOR_FRAME_NAME";
            // 
            // DefinitionBox
            // 
            DefinitionBox.Location = new System.Drawing.Point(212, 23);
            DefinitionBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DefinitionBox.Name = "DefinitionBox";
            DefinitionBox.Size = new System.Drawing.Size(177, 23);
            DefinitionBox.TabIndex = 4;
            // 
            // ActorDefinitionLabel
            // 
            ActorDefinitionLabel.AutoSize = true;
            ActorDefinitionLabel.Location = new System.Drawing.Point(7, 28);
            ActorDefinitionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ActorDefinitionLabel.Name = "ActorDefinitionLabel";
            ActorDefinitionLabel.Size = new System.Drawing.Size(117, 15);
            ActorDefinitionLabel.TabIndex = 3;
            ActorDefinitionLabel.Text = "$ACTOR_DEFINITION";
            // 
            // ActorTypeLabel
            // 
            ActorTypeLabel.AutoSize = true;
            ActorTypeLabel.Location = new System.Drawing.Point(7, 106);
            ActorTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ActorTypeLabel.Name = "ActorTypeLabel";
            ActorTypeLabel.Size = new System.Drawing.Size(81, 15);
            ActorTypeLabel.TabIndex = 1;
            ActorTypeLabel.Text = "$ACTOR_TYPE";
            // 
            // TypeCombo
            // 
            TypeCombo.FormattingEnabled = true;
            TypeCombo.Location = new System.Drawing.Point(212, 102);
            TypeCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TypeCombo.Name = "TypeCombo";
            TypeCombo.Size = new System.Drawing.Size(177, 23);
            TypeCombo.TabIndex = 0;
            // 
            // ActorItemAddOption
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupGeneral);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ActorItemAddOption";
            Size = new System.Drawing.Size(397, 134);
            groupGeneral.ResumeLayout(false);
            groupGeneral.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label ActorTypeLabel;
        private System.Windows.Forms.ComboBox TypeCombo;
        private System.Windows.Forms.Label ActorDefinitionLabel;
        private System.Windows.Forms.TextBox DefinitionBox;
        private System.Windows.Forms.TextBox framename;
        private System.Windows.Forms.Label label1;
    }
}
