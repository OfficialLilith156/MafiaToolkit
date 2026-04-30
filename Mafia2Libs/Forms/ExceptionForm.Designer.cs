
namespace Toolkit.Forms
{
    partial class ExceptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionForm));
            Label_ExceptionMsg = new System.Windows.Forms.Label();
            RichTextBox_StackTrace = new System.Windows.Forms.RichTextBox();
            Button_Continue = new System.Windows.Forms.Button();
            Button_Quit = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // Label_ExceptionMsg
            // 
            Label_ExceptionMsg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Label_ExceptionMsg.Location = new System.Drawing.Point(51, 13);
            Label_ExceptionMsg.Name = "Label_ExceptionMsg";
            Label_ExceptionMsg.Size = new System.Drawing.Size(525, 53);
            Label_ExceptionMsg.TabIndex = 0;
            Label_ExceptionMsg.Text = resources.GetString("Label_ExceptionMsg.Text");
            // 
            // RichTextBox_StackTrace
            // 
            RichTextBox_StackTrace.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RichTextBox_StackTrace.BackColor = System.Drawing.SystemColors.Window;
            RichTextBox_StackTrace.Location = new System.Drawing.Point(13, 102);
            RichTextBox_StackTrace.Name = "RichTextBox_StackTrace";
            RichTextBox_StackTrace.ReadOnly = true;
            RichTextBox_StackTrace.Size = new System.Drawing.Size(563, 352);
            RichTextBox_StackTrace.TabIndex = 1;
            RichTextBox_StackTrace.Text = "";
            // 
            // Button_Continue
            // 
            Button_Continue.Location = new System.Drawing.Point(370, 71);
            Button_Continue.Name = "Button_Continue";
            Button_Continue.Size = new System.Drawing.Size(100, 25);
            Button_Continue.TabIndex = 2;
            Button_Continue.Text = "Continue";
            Button_Continue.UseVisualStyleBackColor = true;
            Button_Continue.Click += Button_Continue_Click;
            // 
            // Button_Quit
            // 
            Button_Quit.Location = new System.Drawing.Point(476, 71);
            Button_Quit.Name = "Button_Quit";
            Button_Quit.Size = new System.Drawing.Size(100, 25);
            Button_Quit.TabIndex = 3;
            Button_Quit.Text = "Quit";
            Button_Quit.UseVisualStyleBackColor = true;
            Button_Quit.Click += Button_Quit_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.ErrorImage = (System.Drawing.Image)resources.GetObject("pictureBox1.ErrorImage");
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new System.Drawing.Point(13, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(32, 32);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // ExceptionForm
            // 
            AcceptButton = Button_Continue;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = Button_Quit;
            ClientSize = new System.Drawing.Size(588, 466);
            Controls.Add(pictureBox1);
            Controls.Add(Button_Quit);
            Controls.Add(Button_Continue);
            Controls.Add(RichTextBox_StackTrace);
            Controls.Add(Label_ExceptionMsg);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExceptionForm";
            ShowIcon = false;
            Text = "Unhandled Exception";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Label_ExceptionMsg;
        private System.Windows.Forms.RichTextBox RichTextBox_StackTrace;
        private System.Windows.Forms.Button Button_Continue;
        private System.Windows.Forms.Button Button_Quit;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}