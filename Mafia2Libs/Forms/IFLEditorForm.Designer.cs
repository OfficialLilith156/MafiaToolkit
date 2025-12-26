using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class IFLEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ListBox listBoxImages;
        private Button btnAdd;
        private Button btnRemove;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IFLEditorForm));
            listBoxImages = new ListBox();
            btnAdd = new Button();
            btnRemove = new Button();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            propertyGrid1 = new PropertyGrid();
            button1 = new Button();
            button2 = new Button();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxImages
            // 
            listBoxImages.ItemHeight = 15;
            listBoxImages.Location = new System.Drawing.Point(12, 29);
            listBoxImages.Name = "listBoxImages";
            listBoxImages.Size = new System.Drawing.Size(272, 379);
            listBoxImages.TabIndex = 0;
            listBoxImages.SelectedIndexChanged += listBoxImages_SelectedIndexChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(12, 414);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(114, 23);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(170, 414);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(114, 23);
            btnRemove.TabIndex = 2;
            btnRemove.Text = "Delete";
            btnRemove.Click += btnRemove_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(719, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += btnSave_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += btnClose_Click;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Location = new System.Drawing.Point(348, 29);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(359, 414);
            propertyGrid1.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(290, 29);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(52, 34);
            button1.TabIndex = 7;
            button1.Text = "UP";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnUp_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(290, 69);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(52, 34);
            button2.TabIndex = 8;
            button2.Text = "DOWN";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnDown_Click;
            // 
            // IFLEditorForm
            // 
            ClientSize = new System.Drawing.Size(719, 455);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(propertyGrid1);
            Controls.Add(toolStrip1);
            Controls.Add(listBoxImages);
            Controls.Add(btnAdd);
            Controls.Add(btnRemove);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "IFLEditorForm";
            Text = "IFL Editor";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private PropertyGrid propertyGrid1;
        private Button button1;
        private Button button2;
    }
}