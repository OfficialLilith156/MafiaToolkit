using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    partial class CityAreasEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CityAreasEditor));
            statusStrip = new StatusStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            addPolygonToolStripMenuItem = new ToolStripMenuItem();
            removePolygonToolStripMenuItem = new ToolStripMenuItem();
            cancelEditToolStripMenuItem = new ToolStripMenuItem();
            loadMapTextureToolStripMenuItem = new ToolStripMenuItem();
            unloadMapTextureToolStripMenuItem = new ToolStripMenuItem();
            resetPositionViewToolStripMenuItem = new ToolStripMenuItem();
            canvasPanel = new BufferedPanel();
            rightPanel = new Panel();
            polygonListBox = new ListBox();
            txtName = new TextBox();
            txtTextID = new TextBox();
            statusStrip.SuspendLayout();
            rightPanel.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.Dock = DockStyle.Top;
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2 });
            statusStrip.Location = new System.Drawing.Point(0, 0);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1275, 22);
            statusStrip.TabIndex = 1;
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, openToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(38, 20);
            toolStripDropDownButton1.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveCurrentFile;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += BtnOpen_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { addPolygonToolStripMenuItem, removePolygonToolStripMenuItem, cancelEditToolStripMenuItem, loadMapTextureToolStripMenuItem, unloadMapTextureToolStripMenuItem, resetPositionViewToolStripMenuItem });
            toolStripDropDownButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new System.Drawing.Size(40, 20);
            toolStripDropDownButton2.Text = "Edit";
            // 
            // addPolygonToolStripMenuItem
            // 
            addPolygonToolStripMenuItem.Name = "addPolygonToolStripMenuItem";
            addPolygonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            addPolygonToolStripMenuItem.Text = "Add Polygon";
            addPolygonToolStripMenuItem.Click += BtnNewPolygon_Click;
            // 
            // removePolygonToolStripMenuItem
            // 
            removePolygonToolStripMenuItem.Name = "removePolygonToolStripMenuItem";
            removePolygonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            removePolygonToolStripMenuItem.Text = "Remove Polygon";
            removePolygonToolStripMenuItem.Click += BtnDeletePolygon_Click;
            // 
            // cancelEditToolStripMenuItem
            // 
            cancelEditToolStripMenuItem.Name = "cancelEditToolStripMenuItem";
            cancelEditToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            cancelEditToolStripMenuItem.Text = "Cancel Edit Polygon";
            cancelEditToolStripMenuItem.Click += BtnCancelAdd_Click;
            // 
            // loadMapTextureToolStripMenuItem
            // 
            loadMapTextureToolStripMenuItem.Name = "loadMapTextureToolStripMenuItem";
            loadMapTextureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            loadMapTextureToolStripMenuItem.Text = "Load Map Texture";
            loadMapTextureToolStripMenuItem.Click += BtnLoadMap_Click;
            // 
            // unloadMapTextureToolStripMenuItem
            // 
            unloadMapTextureToolStripMenuItem.Name = "unloadMapTextureToolStripMenuItem";
            unloadMapTextureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            unloadMapTextureToolStripMenuItem.Text = "Unload Map Texture";
            unloadMapTextureToolStripMenuItem.Click += BtnClearMap_Click;
            // 
            // resetPositionViewToolStripMenuItem
            // 
            resetPositionViewToolStripMenuItem.Name = "resetPositionViewToolStripMenuItem";
            resetPositionViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            resetPositionViewToolStripMenuItem.Text = "Reset Position View";
            resetPositionViewToolStripMenuItem.Click += BtnResetView_Click;
            // 
            // canvasPanel
            // 
            canvasPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.Location = new System.Drawing.Point(0, 22);
            canvasPanel.Name = "canvasPanel";
            canvasPanel.Size = new System.Drawing.Size(1025, 571);
            canvasPanel.TabIndex = 0;
            canvasPanel.Paint += CanvasPanel_Paint;
            canvasPanel.MouseDoubleClick += CanvasPanel_MouseDoubleClick;
            canvasPanel.MouseDown += CanvasPanel_MouseDown;
            canvasPanel.MouseMove += CanvasPanel_MouseMove;
            canvasPanel.MouseUp += CanvasPanel_MouseUp;
            canvasPanel.MouseWheel += CanvasPanel_MouseWheel;
            // 
            // rightPanel
            // 
            rightPanel.BackColor = System.Drawing.Color.LightGray;
            rightPanel.Controls.Add(polygonListBox);
            rightPanel.Controls.Add(txtName);
            rightPanel.Controls.Add(txtTextID);
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Location = new System.Drawing.Point(1025, 22);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new System.Drawing.Size(250, 571);
            rightPanel.TabIndex = 1;
            // 
            // polygonListBox
            // 
            polygonListBox.Dock = DockStyle.Top;
            polygonListBox.ItemHeight = 15;
            polygonListBox.Location = new System.Drawing.Point(0, 46);
            polygonListBox.Name = "polygonListBox";
            polygonListBox.Size = new System.Drawing.Size(250, 199);
            polygonListBox.TabIndex = 0;
            polygonListBox.SelectedIndexChanged += PolygonListBox_SelectedIndexChanged;
            // 
            // txtName
            // 
            txtName.Dock = DockStyle.Top;
            txtName.Location = new System.Drawing.Point(0, 23);
            txtName.Margin = new Padding(5);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "District name";
            txtName.Size = new System.Drawing.Size(250, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // txtTextID
            // 
            txtTextID.Dock = DockStyle.Top;
            txtTextID.Location = new System.Drawing.Point(0, 0);
            txtTextID.Margin = new Padding(5);
            txtTextID.Name = "txtTextID";
            txtTextID.PlaceholderText = "TextID";
            txtTextID.Size = new System.Drawing.Size(250, 23);
            txtTextID.TabIndex = 2;
            txtTextID.TextChanged += TxtTextID_TextChanged;
            // 
            // CityAreasEditor
            // 
            ClientSize = new System.Drawing.Size(1275, 593);
            Controls.Add(canvasPanel);
            Controls.Add(rightPanel);
            Controls.Add(statusStrip);
            DoubleBuffered = true;
            Name = "CityAreasEditor";
            Text = "City Areas Editor";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            rightPanel.ResumeLayout(false);
            rightPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private StatusStrip statusStrip;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem addPolygonToolStripMenuItem;
        private ToolStripMenuItem removePolygonToolStripMenuItem;
        private ToolStripMenuItem cancelEditToolStripMenuItem;
        private ToolStripMenuItem loadMapTextureToolStripMenuItem;
        private ToolStripMenuItem unloadMapTextureToolStripMenuItem;
        private ToolStripMenuItem resetPositionViewToolStripMenuItem;
        private BufferedPanel canvasPanel;
        private Panel rightPanel;
        private ListBox polygonListBox;
        private TextBox txtName;
        private TextBox txtTextID;
    }

    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }
    }
}

#endregion