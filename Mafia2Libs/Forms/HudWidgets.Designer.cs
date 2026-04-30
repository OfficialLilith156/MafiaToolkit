namespace Mafia2Tool.Forms
{
    partial class HudWidgets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HudWidgets));
            tabControlTextures = new System.Windows.Forms.TabControl();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            btnSave = new System.Windows.Forms.ToolStripMenuItem();
            toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            loadTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            btnShow = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlTextures
            // 
            tabControlTextures.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControlTextures.Location = new System.Drawing.Point(10, 28);
            tabControlTextures.Name = "tabControlTextures";
            tabControlTextures.SelectedIndex = 0;
            tabControlTextures.Size = new System.Drawing.Size(840, 579);
            tabControlTextures.TabIndex = 4;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(861, 25);
            toolStrip1.TabIndex = 6;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnSave });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // btnSave
            // 
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(98, 22);
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadTextureToolStripMenuItem, btnShow });
            toolStripDropDownButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new System.Drawing.Size(40, 22);
            toolStripDropDownButton2.Text = "Edit";
            // 
            // loadTextureToolStripMenuItem
            // 
            loadTextureToolStripMenuItem.Name = "loadTextureToolStripMenuItem";
            loadTextureToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            loadTextureToolStripMenuItem.Text = "Load Texture";
            loadTextureToolStripMenuItem.Click += BtnSelectFolder_Click;
            // 
            // btnShow
            // 
            btnShow.Name = "btnShow";
            btnShow.Size = new System.Drawing.Size(149, 22);
            btnShow.Text = "Show Widgets";
            btnShow.Click += BtnShow_Click;
            // 
            // HudWidgets
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(861, 620);
            Controls.Add(toolStrip1);
            Controls.Add(tabControlTextures);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "HudWidgets";
            Text = "HUD Widget Viewer";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private System.Windows.Forms.TabControl tabControlTextures;

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnSave;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem loadTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnShow;
    }
}