namespace Mafia2Tool
{
    partial class NewObjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewObjectForm));
            label = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            buttonContinue = new System.Windows.Forms.Button();
            buttonCancel = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            SuspendLayout();
            // 
            // label
            // 
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(15, 15);
            label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label.Name = "label";
            label.Size = new System.Drawing.Size(46, 15);
            label.TabIndex = 0;
            label.Text = "$LABEL";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(19, 35);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(396, 23);
            textBox1.TabIndex = 1;
            // 
            // buttonContinue
            // 
            buttonContinue.Location = new System.Drawing.Point(304, 216);
            buttonContinue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonContinue.Name = "buttonContinue";
            buttonContinue.Size = new System.Drawing.Size(108, 27);
            buttonContinue.TabIndex = 2;
            buttonContinue.Text = "$CONTINUE";
            buttonContinue.UseVisualStyleBackColor = true;
            buttonContinue.Click += OnButtonClickContinue;
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(21, 216);
            buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(108, 27);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "$CANCEL";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += OnButtonClickCancel;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Location = new System.Drawing.Point(19, 66);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(397, 144);
            panel1.TabIndex = 4;
            // 
            // NewObjectForm
            // 
            AcceptButton = buttonContinue;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(425, 249);
            ControlBox = false;
            Controls.Add(panel1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonContinue);
            Controls.Add(textBox1);
            Controls.Add(label);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewObjectForm";
            Text = "NewObjectEntry";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
    }
}