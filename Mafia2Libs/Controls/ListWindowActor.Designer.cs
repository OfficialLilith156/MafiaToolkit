namespace Mafia2Tool
{
    partial class ListWindowActor
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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            button1 = new System.Windows.Forms.Button();
            labelInfo = new System.Windows.Forms.Label();
            SearchBox = new System.Windows.Forms.TextBox();
            checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(checkedListBox1);
            splitContainer1.Size = new System.Drawing.Size(390, 448);
            splitContainer1.SplitterDistance = 80;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(button1);
            splitContainer2.Panel1.Controls.Add(labelInfo);
            splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(SearchBox);
            splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(6);
            splitContainer2.Size = new System.Drawing.Size(390, 80);
            splitContainer2.SplitterDistance = 44;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(303, 12);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnProcessSelected_Click;
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInfo.Location = new System.Drawing.Point(6, 6);
            labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new System.Drawing.Size(165, 30);
            labelInfo.TabIndex = 0;
            labelInfo.Text = "Double click on item to copy..\r\nSearch Help here";
            // 
            // SearchBox
            // 
            SearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            SearchBox.Location = new System.Drawing.Point(6, 6);
            SearchBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(378, 23);
            SearchBox.TabIndex = 0;
            SearchBox.Text = "Search items..";
            SearchBox.Click += SearchOnClick;
            SearchBox.KeyPress += SearchOnKeyPress;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new System.Drawing.Point(0, -1);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(390, 364);
            checkedListBox1.TabIndex = 1;
            // 
            // ListWindowActor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(390, 448);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ListWindowActor";
            Text = "ListWindow";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button1;
    }
}