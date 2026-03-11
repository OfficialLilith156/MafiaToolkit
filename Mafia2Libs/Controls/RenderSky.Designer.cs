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
            chkLoadSoundSectors = new System.Windows.Forms.CheckBox();
            chkLoadHPD = new System.Windows.Forms.CheckBox();
            chkLoadItemDescs = new System.Windows.Forms.CheckBox();
            chkLoadPrefabs = new System.Windows.Forms.CheckBox();
            chkLoadATP = new System.Windows.Forms.CheckBox();
            chkLoadRoads = new System.Windows.Forms.CheckBox();
            chkLoadOBJData = new System.Windows.Forms.CheckBox();
            chkLoadAIWorld = new System.Windows.Forms.CheckBox();
            chkLoadTranslokator = new System.Windows.Forms.CheckBox();
            chkLoadActors = new System.Windows.Forms.CheckBox();
            chkLoadCollisions = new System.Windows.Forms.CheckBox();
            chkLoadFrameResource = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            BrowseButton5 = new System.Windows.Forms.Button();
            TexDirectoryBox5 = new System.Windows.Forms.TextBox();
            TexBrowser = new System.Windows.Forms.FolderBrowserDialog();
            RenderGroup.SuspendLayout();
            SuspendLayout();
            // 
            // RenderGroup
            // 
            RenderGroup.Controls.Add(chkLoadSoundSectors);
            RenderGroup.Controls.Add(chkLoadHPD);
            RenderGroup.Controls.Add(chkLoadItemDescs);
            RenderGroup.Controls.Add(chkLoadPrefabs);
            RenderGroup.Controls.Add(chkLoadATP);
            RenderGroup.Controls.Add(chkLoadRoads);
            RenderGroup.Controls.Add(chkLoadOBJData);
            RenderGroup.Controls.Add(chkLoadAIWorld);
            RenderGroup.Controls.Add(chkLoadTranslokator);
            RenderGroup.Controls.Add(chkLoadActors);
            RenderGroup.Controls.Add(chkLoadCollisions);
            RenderGroup.Controls.Add(chkLoadFrameResource);
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
            // chkLoadSoundSectors
            // 
            chkLoadSoundSectors.AutoSize = true;
            chkLoadSoundSectors.Location = new System.Drawing.Point(7, 341);
            chkLoadSoundSectors.Name = "chkLoadSoundSectors";
            chkLoadSoundSectors.Size = new System.Drawing.Size(96, 19);
            chkLoadSoundSectors.TabIndex = 37;
            chkLoadSoundSectors.Text = "Sound Sector";
            chkLoadSoundSectors.UseVisualStyleBackColor = true;
            chkLoadSoundSectors.CheckedChanged += chkLoadSoundSectors_CheckedChanged;
            // 
            // chkLoadHPD
            // 
            chkLoadHPD.AutoSize = true;
            chkLoadHPD.Location = new System.Drawing.Point(7, 316);
            chkLoadHPD.Name = "chkLoadHPD";
            chkLoadHPD.Size = new System.Drawing.Size(50, 19);
            chkLoadHPD.TabIndex = 36;
            chkLoadHPD.Text = "HPD";
            chkLoadHPD.UseVisualStyleBackColor = true;
            chkLoadHPD.CheckedChanged += chkLoadHPD_CheckedChanged;
            // 
            // chkLoadItemDescs
            // 
            chkLoadItemDescs.AutoSize = true;
            chkLoadItemDescs.Location = new System.Drawing.Point(8, 291);
            chkLoadItemDescs.Name = "chkLoadItemDescs";
            chkLoadItemDescs.Size = new System.Drawing.Size(78, 19);
            chkLoadItemDescs.TabIndex = 35;
            chkLoadItemDescs.Text = "Item Desc";
            chkLoadItemDescs.UseVisualStyleBackColor = true;
            chkLoadItemDescs.CheckedChanged += chkLoadItemDescs_CheckedChanged;
            // 
            // chkLoadPrefabs
            // 
            chkLoadPrefabs.AutoSize = true;
            chkLoadPrefabs.Location = new System.Drawing.Point(8, 266);
            chkLoadPrefabs.Name = "chkLoadPrefabs";
            chkLoadPrefabs.Size = new System.Drawing.Size(60, 19);
            chkLoadPrefabs.TabIndex = 34;
            chkLoadPrefabs.Text = "Prefab";
            chkLoadPrefabs.UseVisualStyleBackColor = true;
            chkLoadPrefabs.CheckedChanged += chkLoadPrefabs_CheckedChanged;
            // 
            // chkLoadATP
            // 
            chkLoadATP.AutoSize = true;
            chkLoadATP.Location = new System.Drawing.Point(8, 241);
            chkLoadATP.Name = "chkLoadATP";
            chkLoadATP.Size = new System.Drawing.Size(46, 19);
            chkLoadATP.TabIndex = 33;
            chkLoadATP.Text = "ATP";
            chkLoadATP.UseVisualStyleBackColor = true;
            chkLoadATP.CheckedChanged += chkLoadATP_CheckedChanged;
            // 
            // chkLoadRoads
            // 
            chkLoadRoads.AutoSize = true;
            chkLoadRoads.Location = new System.Drawing.Point(8, 216);
            chkLoadRoads.Name = "chkLoadRoads";
            chkLoadRoads.Size = new System.Drawing.Size(58, 19);
            chkLoadRoads.TabIndex = 32;
            chkLoadRoads.Text = "Roads";
            chkLoadRoads.UseVisualStyleBackColor = true;
            chkLoadRoads.CheckedChanged += chkLoadRoads_CheckedChanged;
            // 
            // chkLoadOBJData
            // 
            chkLoadOBJData.AutoSize = true;
            chkLoadOBJData.Location = new System.Drawing.Point(8, 191);
            chkLoadOBJData.Name = "chkLoadOBJData";
            chkLoadOBJData.Size = new System.Drawing.Size(70, 19);
            chkLoadOBJData.TabIndex = 31;
            chkLoadOBJData.Text = "OBJData";
            chkLoadOBJData.UseVisualStyleBackColor = true;
            chkLoadOBJData.CheckedChanged += chkLoadOBJData_CheckedChanged;
            // 
            // chkLoadAIWorld
            // 
            chkLoadAIWorld.AutoSize = true;
            chkLoadAIWorld.Location = new System.Drawing.Point(8, 166);
            chkLoadAIWorld.Name = "chkLoadAIWorld";
            chkLoadAIWorld.Size = new System.Drawing.Size(69, 19);
            chkLoadAIWorld.TabIndex = 30;
            chkLoadAIWorld.Text = "AIWorld";
            chkLoadAIWorld.UseVisualStyleBackColor = true;
            chkLoadAIWorld.CheckedChanged += chkLoadAIWorld_CheckedChanged;
            // 
            // chkLoadTranslokator
            // 
            chkLoadTranslokator.AutoSize = true;
            chkLoadTranslokator.Location = new System.Drawing.Point(8, 141);
            chkLoadTranslokator.Name = "chkLoadTranslokator";
            chkLoadTranslokator.Size = new System.Drawing.Size(90, 19);
            chkLoadTranslokator.TabIndex = 29;
            chkLoadTranslokator.Text = "Translokator";
            chkLoadTranslokator.UseVisualStyleBackColor = true;
            chkLoadTranslokator.CheckedChanged += chkLoadTranslokator_CheckedChanged;
            // 
            // chkLoadActors
            // 
            chkLoadActors.AutoSize = true;
            chkLoadActors.Location = new System.Drawing.Point(8, 116);
            chkLoadActors.Name = "chkLoadActors";
            chkLoadActors.Size = new System.Drawing.Size(55, 19);
            chkLoadActors.TabIndex = 28;
            chkLoadActors.Text = "Actor";
            chkLoadActors.UseVisualStyleBackColor = true;
            chkLoadActors.CheckedChanged += chkLoadActors_CheckedChanged;
            // 
            // chkLoadCollisions
            // 
            chkLoadCollisions.AutoSize = true;
            chkLoadCollisions.Location = new System.Drawing.Point(8, 91);
            chkLoadCollisions.Name = "chkLoadCollisions";
            chkLoadCollisions.Size = new System.Drawing.Size(72, 19);
            chkLoadCollisions.TabIndex = 27;
            chkLoadCollisions.Text = "Collision";
            chkLoadCollisions.UseVisualStyleBackColor = true;
            chkLoadCollisions.CheckedChanged += chkLoadCollisions_CheckedChanged;
            // 
            // chkLoadFrameResource
            // 
            chkLoadFrameResource.AutoSize = true;
            chkLoadFrameResource.Location = new System.Drawing.Point(8, 66);
            chkLoadFrameResource.Name = "chkLoadFrameResource";
            chkLoadFrameResource.Size = new System.Drawing.Size(57, 19);
            chkLoadFrameResource.TabIndex = 26;
            chkLoadFrameResource.Text = "Scene";
            chkLoadFrameResource.UseVisualStyleBackColor = true;
            chkLoadFrameResource.CheckedChanged += chkLoadFrameResource_CheckedChanged;
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
        private System.Windows.Forms.CheckBox chkLoadFrameResource;
        private System.Windows.Forms.CheckBox chkLoadATP;
        private System.Windows.Forms.CheckBox chkLoadRoads;
        private System.Windows.Forms.CheckBox chkLoadOBJData;
        private System.Windows.Forms.CheckBox chkLoadAIWorld;
        private System.Windows.Forms.CheckBox chkLoadTranslokator;
        private System.Windows.Forms.CheckBox chkLoadActors;
        private System.Windows.Forms.CheckBox chkLoadCollisions;
        private System.Windows.Forms.CheckBox chkLoadSoundSectors;
        private System.Windows.Forms.CheckBox chkLoadHPD;
        private System.Windows.Forms.CheckBox chkLoadItemDescs;
        private System.Windows.Forms.CheckBox chkLoadPrefabs;
    }
}
