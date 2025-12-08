namespace Mafia2Tool.Forms
{
    partial class MaterialBatchOptionsForm
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
            TextSamplerStates = new System.Windows.Forms.TextBox();
            checkedFlags = new System.Windows.Forms.CheckedListBox();
            buttonOk = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            numericUNK0 = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            numericUNK1 = new System.Windows.Forms.NumericUpDown();
            numericUNK3 = new System.Windows.Forms.NumericUpDown();
            numericUNK4 = new System.Windows.Forms.NumericUpDown();
            numericUNK5 = new System.Windows.Forms.NumericUpDown();
            TextID = new System.Windows.Forms.TextBox();
            TexType = new System.Windows.Forms.TextBox();
            TextUNKSET0 = new System.Windows.Forms.TextBox();
            TextUNKSET1 = new System.Windows.Forms.TextBox();
            TextUNKZERO = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            numericShaderID = new System.Windows.Forms.NumericUpDown();
            numericShaderHash = new System.Windows.Forms.NumericUpDown();
            label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)numericUNK0).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericShaderID).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericShaderHash).BeginInit();
            SuspendLayout();
            // 
            // TextSamplerStates
            // 
            TextSamplerStates.Location = new System.Drawing.Point(318, 41);
            TextSamplerStates.Name = "TextSamplerStates";
            TextSamplerStates.Size = new System.Drawing.Size(100, 23);
            TextSamplerStates.TabIndex = 0;
            // 
            // checkedFlags
            // 
            checkedFlags.FormattingEnabled = true;
            checkedFlags.Items.AddRange(new object[] { "flag0", "flag_1", "Alpha", "flag_4", "flag_8", "Disable_ZWriting", "flag_32", "flag_64", "flag_128", "flag_256", "flag_512", "flag_1024", "flag_2048", "CastShadows", "flag_8192", "flag_16384", "flag_32768", "flag_65536", "flag_131072", "flag_262144", "flag_524288", "flag_1048576", "flag_2097152", "flag_4194304", "flag_8388608", "flag_16777216", "flag_33554432", "flag_67108864", "flag_134217728", "flag_268435456", "flag_536870912", "flag_1073741824" });
            checkedFlags.Location = new System.Drawing.Point(424, 10);
            checkedFlags.Name = "checkedFlags";
            checkedFlags.Size = new System.Drawing.Size(227, 580);
            checkedFlags.TabIndex = 1;
            // 
            // buttonOk
            // 
            buttonOk.Location = new System.Drawing.Point(12, 596);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(75, 23);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(112, 596);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += buttonCancel_Click;
            // 
            // numericUNK0
            // 
            numericUNK0.Location = new System.Drawing.Point(52, 10);
            numericUNK0.Name = "numericUNK0";
            numericUNK0.Size = new System.Drawing.Size(120, 23);
            numericUNK0.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(34, 15);
            label1.TabIndex = 5;
            label1.Text = "Unk0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 39);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(34, 15);
            label2.TabIndex = 6;
            label2.Text = "Unk1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 68);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(34, 15);
            label4.TabIndex = 8;
            label4.Text = "Unk3";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(12, 97);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(34, 15);
            label5.TabIndex = 9;
            label5.Text = "Unk4";

            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(12, 126);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(34, 15);
            label6.TabIndex = 10;
            label6.Text = "Unk5";
            // 
            // numericUNK1
            // 
            numericUNK1.Location = new System.Drawing.Point(52, 37);
            numericUNK1.Name = "numericUNK1";
            numericUNK1.Size = new System.Drawing.Size(120, 23);
            numericUNK1.TabIndex = 11;
            // 
            // numericUNK3
            // 
            numericUNK3.Location = new System.Drawing.Point(52, 66);
            numericUNK3.Name = "numericUNK3";
            numericUNK3.Size = new System.Drawing.Size(120, 23);
            numericUNK3.TabIndex = 13;

            // 
            // numericUNK4
            // 
            numericUNK4.Location = new System.Drawing.Point(52, 95);
            numericUNK4.Name = "numericUNK4";
            numericUNK4.Size = new System.Drawing.Size(120, 23);
            numericUNK4.TabIndex = 14;

            // 
            // numericUNK5
            // 
            numericUNK5.Location = new System.Drawing.Point(52, 124);
            numericUNK5.Name = "numericUNK5";
            numericUNK5.Size = new System.Drawing.Size(120, 23);
            numericUNK5.TabIndex = 15;

            // 
            // TextID
            // 
            TextID.Location = new System.Drawing.Point(275, 12);
            TextID.Name = "TextID";
            TextID.Size = new System.Drawing.Size(100, 23);
            TextID.TabIndex = 16;
            // 
            // TexType
            // 
            TexType.Location = new System.Drawing.Point(285, 70);
            TexType.Name = "TexType";
            TexType.Size = new System.Drawing.Size(100, 23);
            TexType.TabIndex = 17;
            // 
            // TextUNKSET0
            // 
            TextUNKSET0.Location = new System.Drawing.Point(285, 99);
            TextUNKSET0.Name = "TextUNKSET0";
            TextUNKSET0.Size = new System.Drawing.Size(100, 23);
            TextUNKSET0.TabIndex = 18;
            // 
            // TextUNKSET1
            // 
            TextUNKSET1.Location = new System.Drawing.Point(285, 129);
            TextUNKSET1.Name = "TextUNKSET1";
            TextUNKSET1.Size = new System.Drawing.Size(100, 23);
            TextUNKSET1.TabIndex = 19;
            // 
            // TextUNKZERO
            // 
            TextUNKZERO.Location = new System.Drawing.Point(285, 158);
            TextUNKZERO.Name = "TextUNKZERO";
            TextUNKZERO.Size = new System.Drawing.Size(100, 23);
            TextUNKZERO.TabIndex = 20;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(231, 15);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(18, 15);
            label7.TabIndex = 21;
            label7.Text = "ID";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(231, 44);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(81, 15);
            label8.TabIndex = 22;
            label8.Text = "SamplerStates";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(231, 74);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(48, 15);
            label9.TabIndex = 23;
            label9.Text = "TexType";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(231, 103);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(50, 15);
            label10.TabIndex = 24;
            label10.Text = "UnkSet0";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(231, 132);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(50, 15);
            label11.TabIndex = 25;
            label11.Text = "UnkSet1";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(231, 161);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(52, 15);
            label12.TabIndex = 26;
            label12.Text = "UnkZero";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(12, 155);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(54, 15);
            label13.TabIndex = 27;
            label13.Text = "ShaderID";
            // 
            // numericShaderID
            // 
            numericShaderID.Location = new System.Drawing.Point(72, 155);
            numericShaderID.Name = "numericShaderID";
            numericShaderID.Size = new System.Drawing.Size(120, 23);
            numericShaderID.TabIndex = 28;

            // 
            // numericShaderHash
            // 
            numericShaderHash.Location = new System.Drawing.Point(88, 184);
            numericShaderHash.Name = "numericShaderHash";
            numericShaderHash.Size = new System.Drawing.Size(120, 23);
            numericShaderHash.TabIndex = 29;

            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(12, 186);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(70, 15);
            label14.TabIndex = 30;
            label14.Text = "ShaderHash";

            // 
            // MaterialBatchOptionsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(663, 631);
            Controls.Add(label14);
            Controls.Add(numericShaderHash);
            Controls.Add(numericShaderID);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(TextUNKZERO);
            Controls.Add(TextUNKSET1);
            Controls.Add(TextUNKSET0);
            Controls.Add(TexType);
            Controls.Add(TextID);
            Controls.Add(numericUNK5);
            Controls.Add(numericUNK4);
            Controls.Add(numericUNK3);
            Controls.Add(numericUNK1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUNK0);
            Controls.Add(button2);
            Controls.Add(buttonOk);
            Controls.Add(checkedFlags);
            Controls.Add(TextSamplerStates);
            Name = "MaterialBatchOptionsForm";
            Text = "MaterialBatchOptionsForm";

            ((System.ComponentModel.ISupportInitialize)numericUNK0).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK4).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUNK5).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericShaderID).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericShaderHash).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox TextSamplerStates;
        private System.Windows.Forms.CheckedListBox checkedFlags;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUNK0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUNK1;
        private System.Windows.Forms.NumericUpDown numericUNK3;
        private System.Windows.Forms.NumericUpDown numericUNK4;
        private System.Windows.Forms.NumericUpDown numericUNK5;
        private System.Windows.Forms.TextBox TextID;
        private System.Windows.Forms.TextBox TexType;
        private System.Windows.Forms.TextBox TextUNKSET0;
        private System.Windows.Forms.TextBox TextUNKSET1;
        private System.Windows.Forms.TextBox TextUNKZERO;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericShaderID;
        private System.Windows.Forms.NumericUpDown numericShaderHash;
        private System.Windows.Forms.Label label14;
    }
}