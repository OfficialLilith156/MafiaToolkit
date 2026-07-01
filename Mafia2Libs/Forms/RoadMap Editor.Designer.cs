using System.Windows.Forms;
using ResourceTypes.Navigation.Traffic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace RoadmapEditor
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            _btnSave = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            _btnAddPoint = new ToolStripMenuItem();
            _btnDeletePoint = new ToolStripMenuItem();
            newRoadToolStripMenuItem = new ToolStripMenuItem();
            deleteRoadToolStripMenuItem = new ToolStripMenuItem();
            _rbCe = new RadioButton();
            _rbDe = new RadioButton();
            _rbXml = new RadioButton();
            label1 = new Label();
            comboBox1 = new ComboBox();
            checkBox1 = new CheckBox();
            _listSplines = new ListBox();
            _canvas = new BufferedPanel();
            _txtX = new TextBox();
            _txtY = new TextBox();
            _txtZ = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            _lblSelectedPoint = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1383, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, _btnSave, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(114, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += OpenFile;
            // 
            // _btnSave
            // 
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(114, 22);
            _btnSave.Text = "Save";
            _btnSave.Click += SaveFile;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(114, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += SaveAsFile;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _btnAddPoint, _btnDeletePoint, newRoadToolStripMenuItem, deleteRoadToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // _btnAddPoint
            // 
            _btnAddPoint.Name = "_btnAddPoint";
            _btnAddPoint.Size = new Size(138, 22);
            _btnAddPoint.Text = "Add Point";
            _btnAddPoint.Click += OnAddPoint;
            // 
            // _btnDeletePoint
            // 
            _btnDeletePoint.Name = "_btnDeletePoint";
            _btnDeletePoint.Size = new Size(138, 22);
            _btnDeletePoint.Text = "Delete Point";
            _btnDeletePoint.Click += OnDeletePoint;
            // 
            // newRoadToolStripMenuItem
            // 
            newRoadToolStripMenuItem.Name = "newRoadToolStripMenuItem";
            newRoadToolStripMenuItem.Size = new Size(138, 22);
            newRoadToolStripMenuItem.Text = "New Road";
            newRoadToolStripMenuItem.Click += OnNewRoad;
            // 
            // deleteRoadToolStripMenuItem
            // 
            deleteRoadToolStripMenuItem.Name = "deleteRoadToolStripMenuItem";
            deleteRoadToolStripMenuItem.Size = new Size(138, 22);
            deleteRoadToolStripMenuItem.Text = "Delete Road";
            deleteRoadToolStripMenuItem.Click += OnDeleteRoad;
            // 
            // _rbCe
            // 
            _rbCe.AutoSize = true;
            _rbCe.Location = new Point(12, 27);
            _rbCe.Name = "_rbCe";
            _rbCe.Size = new Size(104, 19);
            _rbCe.TabIndex = 1;
            _rbCe.TabStop = true;
            _rbCe.Text = "CryEngine (CE)";
            _rbCe.UseVisualStyleBackColor = true;
            // 
            // _rbDe
            // 
            _rbDe.AutoSize = true;
            _rbDe.Location = new Point(122, 27);
            _rbDe.Name = "_rbDe";
            _rbDe.Size = new Size(87, 19);
            _rbDe.TabIndex = 2;
            _rbDe.TabStop = true;
            _rbDe.Text = "DE (Legacy)";
            _rbDe.UseVisualStyleBackColor = true;
            // 
            // _rbXml
            // 
            _rbXml.AutoSize = true;
            _rbXml.Location = new Point(215, 27);
            _rbXml.Name = "_rbXml";
            _rbXml.Size = new Size(49, 19);
            _rbXml.TabIndex = 3;
            _rbXml.TabStop = true;
            _rbXml.Text = "XML";
            _rbXml.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(270, 29);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 4;
            label1.Text = "View:";
            // 
            // comboBox1
            // 
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Top-Down (XZ)", "Side (XY)", "Front (ZY)" });
            comboBox1.Location = new Point(311, 26);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 5;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(438, 28);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(144, 19);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Limit Distance (Max 7)";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // _listSplines
            // 
            _listSplines.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            _listSplines.FormattingEnabled = true;
            _listSplines.ItemHeight = 15;
            _listSplines.Location = new Point(12, 52);
            _listSplines.Name = "_listSplines";
            _listSplines.Size = new Size(414, 514);
            _listSplines.TabIndex = 7;
            _listSplines.SelectedIndexChanged += OnSplineSelected;
            // 
            // _canvas
            // 
            _canvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _canvas.BackColor = Color.Black;
            _canvas.Location = new Point(432, 52);
            _canvas.Name = "_canvas";
            _canvas.Size = new Size(939, 574);
            _canvas.TabIndex = 8;
            _canvas.Paint += OnCanvasPaint;
            // 
            // _txtX
            // 
            _txtX.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _txtX.Location = new Point(35, 592);
            _txtX.Name = "_txtX";
            _txtX.Size = new Size(100, 23);
            _txtX.TabIndex = 9;
            // 
            // _txtY
            // 
            _txtY.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _txtY.Location = new Point(180, 592);
            _txtY.Name = "_txtY";
            _txtY.Size = new Size(100, 23);
            _txtY.TabIndex = 10;
            // 
            // _txtZ
            // 
            _txtZ.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _txtZ.Location = new Point(326, 592);
            _txtZ.Name = "_txtZ";
            _txtZ.Size = new Size(100, 23);
            _txtZ.TabIndex = 11;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(303, 595);
            label2.Name = "label2";
            label2.Size = new Size(17, 15);
            label2.TabIndex = 12;
            label2.Text = "Z:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(157, 595);
            label3.Name = "label3";
            label3.Size = new Size(17, 15);
            label3.TabIndex = 13;
            label3.Text = "Y:";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(12, 595);
            label4.Name = "label4";
            label4.Size = new Size(17, 15);
            label4.TabIndex = 14;
            label4.Text = "X:";
            // 
            // _lblSelectedPoint
            // 
            _lblSelectedPoint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _lblSelectedPoint.AutoSize = true;
            _lblSelectedPoint.Location = new Point(12, 569);
            _lblSelectedPoint.Name = "_lblSelectedPoint";
            _lblSelectedPoint.Size = new Size(34, 15);
            _lblSelectedPoint.TabIndex = 16;
            _lblSelectedPoint.Text = "none";
            // 
            // Form1
            // 
            ClientSize = new Size(1383, 633);
            Controls.Add(_lblSelectedPoint);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(_txtZ);
            Controls.Add(_txtY);
            Controls.Add(_txtX);
            Controls.Add(_canvas);
            Controls.Add(_listSplines);
            Controls.Add(checkBox1);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(_rbXml);
            Controls.Add(_rbDe);
            Controls.Add(_rbCe);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "RoadMap Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }


        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _btnSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.RadioButton _rbCe;
        private System.Windows.Forms.RadioButton _rbDe;
        private System.Windows.Forms.RadioButton _rbXml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ListBox _listSplines;
        private BufferedPanel _canvas;
        private System.Windows.Forms.TextBox _txtX;
        private System.Windows.Forms.TextBox _txtY;
        private System.Windows.Forms.TextBox _txtZ;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _btnAddPoint;
        private System.Windows.Forms.ToolStripMenuItem _btnDeletePoint;
        private System.Windows.Forms.ToolStripMenuItem newRoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRoadToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Label _lblSelectedPoint;
    }
}