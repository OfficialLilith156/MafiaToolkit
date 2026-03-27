using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Mafia2Tool.Forms
{
    public partial class HudWidgets : Form
    {
        private string xmlFilePath;
        private string texturesFolder;
        private List<TextureInfo> textures;
        private Dictionary<int, List<Widget>> widgetsByTexture;
        private XDocument originalDocument;
        private string currentFilePath;

        public HudWidgets()
        {
            InitializeComponent();
        }

        public HudWidgets(string xmlFilePath) : this()
        {
            if (LoadXmlFile(xmlFilePath))
            {
                string xmlDir = Path.GetDirectoryName(xmlFilePath);
                if (textures != null && textures.Count > 0 && !string.IsNullOrEmpty(xmlDir))
                {
                    string testPath = Path.Combine(xmlDir, textures[0].Name);
                    if (File.Exists(testPath))
                    {
                        texturesFolder = xmlDir;
                        btnShow.Enabled = true;
                        ShowTabs(); 
                        return;
                    }
                }


                if (string.IsNullOrEmpty(texturesFolder))
                {

                        BtnSelectFolder_Click(null, null);
                        if (!string.IsNullOrEmpty(texturesFolder))
                        {
                            btnShow.Enabled = true;
                            ShowTabs();
                        }
                    }
                }
            }
        
        private void ShowTabs()
        {
            if (textures == null || widgetsByTexture == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(texturesFolder))
            {
                return;
            }

            tabControlTextures.TabPages.Clear();

            foreach (var texture in textures)
            {
                var widgetList = widgetsByTexture.ContainsKey(texture.ID) ? widgetsByTexture[texture.ID] : new List<Widget>();
                CreateTextureTab(texture, widgetList);
            }
        }

        private bool LoadXmlFile(string path)
        {
            try
            {
                xmlFilePath = path;
                originalDocument = XDocument.Load(xmlFilePath);
                textures = ParseTextures(originalDocument);
                var widgets = ParseWidgets(originalDocument);
                widgetsByTexture = widgets.GroupBy(w => w.TextureID).ToDictionary(g => g.Key, g => g.ToList());

                btnShow.Enabled = !string.IsNullOrEmpty(texturesFolder) && textures != null;
                btnSave.Enabled = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnShow.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
        }

      

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the folder containing the texture files (hud.png, default.png)";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    texturesFolder = dialog.SelectedPath;
                    btnShow.Enabled = !string.IsNullOrEmpty(xmlFilePath) && textures != null;

                    if (textures != null && widgetsByTexture != null)
                        ShowTabs();
                }
            }
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (textures == null || widgetsByTexture == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(texturesFolder))
            {
                return;
            }

            tabControlTextures.TabPages.Clear();

            foreach (var texture in textures)
            {
                var widgetList = widgetsByTexture.ContainsKey(texture.ID) ? widgetsByTexture[texture.ID] : new List<Widget>();
                CreateTextureTab(texture, widgetList);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (originalDocument == null || textures == null || widgetsByTexture == null)
            {
                return;
            }

            try
            {
                var texturesElement = originalDocument.Root.Element("Textures");
                if (texturesElement != null)
                {
                    foreach (var tex in textures)
                    {
                        var texElem = tex.OriginalElement;
                        if (texElem != null)
                        {
                            texElem.Element("ID").Value = tex.ID.ToString();
                            texElem.Element("Name").Value = tex.Name;
                            texElem.Element("Width").Value = tex.Width.ToString();
                            texElem.Element("Height").Value = tex.Height.ToString();
                        }
                    }
                }

                var widgetsElement = originalDocument.Root.Element("Widgets");
                if (widgetsElement != null)
                {
                    foreach (var widgetList in widgetsByTexture.Values)
                    {
                        foreach (var widget in widgetList)
                        {
                            var wElem = widget.OriginalElement;
                            if (wElem != null)
                            {
                                wElem.Element("TextureID").Value = widget.TextureID.ToString();
                                wElem.Element("Mapping").Value = $"{widget.Mapping.Left} {widget.Mapping.Top} {widget.Mapping.Right} {widget.Mapping.Bottom}";
                                wElem.Element("Rotation").Value = widget.Rotation.ToString();
                                var pivotElem = wElem.Element("Pivot");
                                if (pivotElem != null)
                                    pivotElem.Value = $"{widget.Pivot.X.ToString(System.Globalization.CultureInfo.InvariantCulture)} {widget.Pivot.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
                            }
                        }
                    }
                }

                originalDocument.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<TextureInfo> ParseTextures(XDocument doc)
        {
            var texturesList = new List<TextureInfo>();
            var texturesElement = doc.Root.Element("Textures");
            if (texturesElement == null) return texturesList;

            foreach (var texElement in texturesElement.Elements("Texture"))
            {
                int id = (int)texElement.Element("ID");
                string name = (string)texElement.Element("Name");
                int width = (int)texElement.Element("Width");
                int height = (int)texElement.Element("Height");
                texturesList.Add(new TextureInfo(id, name, width, height, texElement));
            }
            return texturesList;
        }

        private List<Widget> ParseWidgets(XDocument doc)
        {
            var widgetsList = new List<Widget>();
            var widgetsElement = doc.Root.Element("Widgets");
            if (widgetsElement == null) return widgetsList;

            foreach (var widgetElement in widgetsElement.Elements())
            {
                string typeName = widgetElement.Name.LocalName;

                int textureId = (int)widgetElement.Element("TextureID");
                string mappingStr = (string)widgetElement.Element("Mapping");
                var rect = ParseMapping(mappingStr);

                string rotationStr = (string)widgetElement.Element("Rotation");
                Rotation rotation = rotationStr switch
                {
                    "None" => Rotation.None,
                    "Rotate90" => Rotation.Rotate90,
                    "Rotate180" => Rotation.Rotate180,
                    "Rotate270" => Rotation.Rotate270,
                    _ => Rotation.None
                };

                PointF pivot = ParsePivot((string)widgetElement.Element("Pivot"));

                widgetsList.Add(new Widget(typeName, textureId, rect, rotation, pivot, widgetElement));
            }
            return widgetsList;
        }

        private Rectangle ParseMapping(string mappingStr)
        {
            var parts = mappingStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4) throw new FormatException("Mapping must have 4 numbers");
            int x1 = int.Parse(parts[0]);
            int y1 = int.Parse(parts[1]);
            int x2 = int.Parse(parts[2]);
            int y2 = int.Parse(parts[3]);

            int left = Math.Min(x1, x2);
            int top = Math.Min(y1, y2);
            int width = Math.Abs(x2 - x1);
            int height = Math.Abs(y2 - y1);
            return new Rectangle(left, top, width, height);
        }

        private PointF ParsePivot(string pivotStr)
        {
            if (string.IsNullOrEmpty(pivotStr)) return new PointF(0.5f, 0.5f);
            var parts = pivotStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return new PointF(0.5f, 0.5f);
            float x = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
            float y = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
            return new PointF(x, y);
        }

        private void CreateTextureTab(TextureInfo texture, List<Widget> widgets)
        {
            string texturePath = FindTextureFile(texture.Name);
            Bitmap originalImage;
            float fileScaleX = 1f, fileScaleY = 1f;

            if (File.Exists(texturePath))
            {
                originalImage = new Bitmap(texturePath);
                fileScaleX = (float)originalImage.Width / texture.Width;
                fileScaleY = (float)originalImage.Height / texture.Height;
            }
            else
            {
                originalImage = new Bitmap(texture.Width, texture.Height);
                using (var g = Graphics.FromImage(originalImage))
                {
                    g.Clear(Color.Gray);
                    g.DrawString($"File not found: {texture.Name}", SystemFonts.DefaultFont, Brushes.White, 10, 10);
                }
            }

            var tabPage = new TabPage
            {
                Text = $"{texture.Name} (ID={texture.ID})",
                AutoScroll = true
            };
            tabControlTextures.TabPages.Add(tabPage);

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 900
            };
            tabPage.Controls.Add(splitContainer);

            var leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            splitContainer.Panel1.Controls.Add(leftPanel);

            var pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(0, 0),
                BackColor = Color.DarkGray
            };
            leftPanel.Controls.Add(pictureBox);

            var zoomPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = SystemColors.Control
            };
            splitContainer.Panel1.Controls.Add(zoomPanel);

            var lblZoom = new Label
            {
                Text = "Scale: 100%",
                Location = new Point(10, 10),
                AutoSize = true
            };
            zoomPanel.Controls.Add(lblZoom);

            var trackBar = new TrackBar
            {
                Minimum = 10,
                Maximum = 300,
                Value = 100,
                TickFrequency = 10,
                Location = new Point(100, 5),
                Size = new Size(200, 30)
            };
            zoomPanel.Controls.Add(trackBar);

            var btnZoomOut = new Button
            {
                Text = "-",
                Location = new Point(310, 5),
                Size = new Size(35, 25)
            };
            zoomPanel.Controls.Add(btnZoomOut);

            var btnZoomIn = new Button
            {
                Text = "+",
                Location = new Point(350, 5),
                Size = new Size(35, 25)
            };
            zoomPanel.Controls.Add(btnZoomIn);

            var btnResetZoom = new Button
            {
                Text = "100%",
                Location = new Point(395, 5),
                Size = new Size(45, 25)
            };
            zoomPanel.Controls.Add(btnResetZoom);

            var rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5),
                AutoScroll = true
            };
            splitContainer.Panel2.Controls.Add(rightPanel);

            var groupBox = new GroupBox
            {
                Text = "",
                Dock = DockStyle.Top,
                Height = 350,
                Width = 250
            };
            rightPanel.Controls.Add(groupBox);

            var lblSelect = new Label
            {
                Text = "Widget:",
                Location = new Point(10, 25),
                AutoSize = true
            };
            groupBox.Controls.Add(lblSelect);

            var cmbWidgets = new ComboBox
            {
                Location = new Point(80, 22),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            groupBox.Controls.Add(cmbWidgets);

            var lblName = new Label
            {
                Text = "Type:",
                Location = new Point(10, 55),
                AutoSize = true
            };
            groupBox.Controls.Add(lblName);

            var txtName = new TextBox
            {
                Location = new Point(80, 52),
                Width = 150
            };
            groupBox.Controls.Add(txtName);

            var lblX = new Label { Text = "X:", Location = new Point(10, 85), AutoSize = true };
            groupBox.Controls.Add(lblX);
            var numX = new NumericUpDown
            {
                Location = new Point(80, 82),
                Width = 60,
                Minimum = 0,
                Maximum = 4096,
                DecimalPlaces = 0
            };
            groupBox.Controls.Add(numX);

            var lblY = new Label { Text = "Y:", Location = new Point(150, 85), AutoSize = true };
            groupBox.Controls.Add(lblY);
            var numY = new NumericUpDown
            {
                Location = new Point(170, 82),
                Width = 60,
                Minimum = 0,
                Maximum = 4096,
                DecimalPlaces = 0
            };
            groupBox.Controls.Add(numY);

            var lblW = new Label { Text = "Width:", Location = new Point(10, 115), AutoSize = true };
            groupBox.Controls.Add(lblW);
            var numW = new NumericUpDown
            {
                Location = new Point(80, 112),
                Width = 60,
                Minimum = 1,
                Maximum = 4096,
                DecimalPlaces = 0
            };
            groupBox.Controls.Add(numW);

            var lblH = new Label { Text = "Height:", Location = new Point(150, 115), AutoSize = true };
            groupBox.Controls.Add(lblH);
            var numH = new NumericUpDown
            {
                Location = new Point(170, 112),
                Width = 60,
                Minimum = 1,
                Maximum = 4096,
                DecimalPlaces = 0
            };
            groupBox.Controls.Add(numH);

            var lblRotation = new Label
            {
                Text = "Rotation:",
                Location = new Point(10, 145),
                AutoSize = true
            };
            groupBox.Controls.Add(lblRotation);

            var cmbRotation = new ComboBox
            {
                Location = new Point(80, 142),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRotation.Items.AddRange(Enum.GetNames(typeof(Rotation)));
            cmbRotation.SelectedIndex = 0;
            groupBox.Controls.Add(cmbRotation);

            var lblPivotX = new Label { Text = "Pivot X:", Location = new Point(10, 175), AutoSize = true };
            groupBox.Controls.Add(lblPivotX);
            var numPivotX = new NumericUpDown
            {
                Location = new Point(80, 172),
                Width = 60,
                Minimum = -10,
                Maximum = 10,
                DecimalPlaces = 20,
                Increment = 0.01M
            };
            groupBox.Controls.Add(numPivotX);

            var lblPivotY = new Label { Text = "Pivot Y:", Location = new Point(150, 175), AutoSize = true };
            groupBox.Controls.Add(lblPivotY);
            var numPivotY = new NumericUpDown
            {
                Location = new Point(170, 172),
                Width = 60,
                Minimum = -10,
                Maximum = 10,
                DecimalPlaces = 20,
                Increment = 0.01M
            };
            groupBox.Controls.Add(numPivotY);

            Widget selectedWidget = null;
            bool isDragging = false;
            int activeHandle = -1;
            Point dragStartMouse = new Point();
            Rectangle dragStartRect = new Rectangle();
            bool updatingUI = false;

            Bitmap scaledBackground = null;
            float zoom = 1f;
            bool needUpdateBackground = true; 

            void UpdateScaledBackground()
            {
                if (originalImage == null) return;

                int newWidth = (int)(originalImage.Width * zoom);
                int newHeight = (int)(originalImage.Height * zoom);
                if (newWidth <= 0 || newHeight <= 0) return;

                var newBackground = new Bitmap(newWidth, newHeight);
                using (var g = Graphics.FromImage(newBackground))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                var old = scaledBackground;
                scaledBackground = newBackground;
                old?.Dispose();

                pictureBox.Size = new Size(newWidth, newHeight);
                leftPanel.AutoScrollPosition = new Point(0, 0);
                pictureBox.Invalidate();
            }

            void DrawScene(Graphics g)
            {
                if (scaledBackground != null)
                {
                    g.DrawImage(scaledBackground, 0, 0);
                }
                else
                {
                    g.Clear(Color.Gray);
                    g.DrawString("Loading...", SystemFonts.DefaultFont, Brushes.White, 10, 10);
                    return;
                }

                foreach (var w in widgets)
                {
                    int left = (int)(w.Mapping.X * fileScaleX * zoom);
                    int top = (int)(w.Mapping.Y * fileScaleY * zoom);
                    int width = (int)(w.Mapping.Width * fileScaleX * zoom);
                    int height = (int)(w.Mapping.Height * fileScaleY * zoom);
                    var rect = new Rectangle(left, top, width, height);

                    using (var pen = new Pen(w == selectedWidget ? Color.DodgerBlue : Color.LimeGreen, Math.Max(1, 2 / zoom)))
                    {
                        if (w == selectedWidget)
                            pen.Width = Math.Max(2, 3 / zoom);
                        pen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen, rect);
                    }

                    if (w == selectedWidget)
                        DrawHandles(g, rect);

                    int pivotX = rect.X + (int)(rect.Width * w.Pivot.X);
                    int pivotY = rect.Y + (int)(rect.Height * w.Pivot.Y);
                    using (var pen = new Pen(Color.Red, Math.Max(1, 2 / zoom)))
                    {
                        g.DrawLine(pen, pivotX - 5, pivotY, pivotX + 5, pivotY);
                        g.DrawLine(pen, pivotX, pivotY - 5, pivotX, pivotY + 5);
                    }

                    string label = w.Name;
                    if (w.Rotation != Rotation.None)
                        label += " [Rotated]";

                    float fontSize = 10f / zoom;
                    if (fontSize < 6) fontSize = 6;
                    if (fontSize > 14) fontSize = 14;
                    using (var font = new Font("Arial", fontSize, FontStyle.Bold))
                    {
                        var textSize = g.MeasureString(label, font);

                        float textX = rect.X + rect.Width / 2 - textSize.Width / 2;
                        float textY = rect.Y - textSize.Height - 2;

                        bool useInside = (textY < 0) || (textX < 0) || (textX + textSize.Width > scaledBackground.Width);

                        if (useInside)
                        {
                            textX = rect.X + 2;
                            textY = rect.Y + 2;
                            if (textX + textSize.Width > scaledBackground.Width)
                                textX = rect.X - textSize.Width - 2;
                            if (textY + textSize.Height > scaledBackground.Height)
                                textY = rect.Y - textSize.Height - 2;
                            textX = Math.Max(0, textX);
                            textY = Math.Max(0, textY);
                        }

                        g.FillRectangle(Brushes.Black, textX, textY, textSize.Width, textSize.Height);
                        g.DrawString(label, font, Brushes.Yellow, textX, textY);
                    }
                }
            }

            void DrawHandles(Graphics g, Rectangle rect)
            {
                int handleSize = 6;
                using (var brush = new SolidBrush(Color.White))
                using (var pen = new Pen(Color.Black, 1))
                {
                    Point[] handles = GetHandlePoints(rect);
                    foreach (var pt in handles)
                    {
                        g.FillRectangle(brush, pt.X - handleSize / 2, pt.Y - handleSize / 2, handleSize, handleSize);
                        g.DrawRectangle(pen, pt.X - handleSize / 2, pt.Y - handleSize / 2, handleSize, handleSize);
                    }
                }
            }

            Point[] GetHandlePoints(Rectangle rect)
            {
                return new Point[]
                {
                    new Point(rect.Left, rect.Top),
                    new Point(rect.Right, rect.Top),
                    new Point(rect.Right, rect.Bottom),
                    new Point(rect.Left, rect.Bottom),
                    new Point(rect.Left + rect.Width / 2, rect.Top),
                    new Point(rect.Right, rect.Top + rect.Height / 2),
                    new Point(rect.Left + rect.Width / 2, rect.Bottom),
                    new Point(rect.Left, rect.Top + rect.Height / 2)
                };
            }

            int HitTestHandles(Point mouse, Rectangle rect, float zoom)
            {
                Point[] handles = GetHandlePoints(rect);
                int handleSize = 6;
                for (int i = 0; i < handles.Length; i++)
                {
                    var pt = handles[i];
                    Rectangle handleRect = new Rectangle(pt.X - handleSize / 2, pt.Y - handleSize / 2, handleSize, handleSize);
                    if (handleRect.Contains(mouse))
                        return i;
                }
                return -1;
            }

            void UpdateRectangleFromHandle(Rectangle originalRect, Point delta, int handle, out Rectangle newRect)
            {
                newRect = originalRect;
                switch (handle)
                {
                    case 0: newRect.X += delta.X; newRect.Y += delta.Y; newRect.Width -= delta.X; newRect.Height -= delta.Y; break;
                    case 1: newRect.Y += delta.Y; newRect.Width += delta.X; newRect.Height -= delta.Y; break;
                    case 2: newRect.Width += delta.X; newRect.Height += delta.Y; break;
                    case 3: newRect.X += delta.X; newRect.Width -= delta.X; newRect.Height += delta.Y; break;
                    case 4: newRect.Y += delta.Y; newRect.Height -= delta.Y; break;
                    case 5: newRect.Width += delta.X; break;
                    case 6: newRect.Height += delta.Y; break;
                    case 7: newRect.X += delta.X; newRect.Width -= delta.X; break;
                }
                if (newRect.Width < 1) newRect.Width = 1;
                if (newRect.Height < 1) newRect.Height = 1;
                if (newRect.X < 0) newRect.X = 0;
                if (newRect.Y < 0) newRect.Y = 0;
            }

            void UpdateWidgetList()
            {
                cmbWidgets.Items.Clear();
                foreach (var w in widgets)
                    cmbWidgets.Items.Add($"{w.Name} (ID={w.TextureID})");
                if (selectedWidget != null)
                {
                    int index = widgets.IndexOf(selectedWidget);
                    if (index >= 0) cmbWidgets.SelectedIndex = index;
                }
                else if (cmbWidgets.Items.Count > 0)
                    cmbWidgets.SelectedIndex = 0;
            }

            void LoadWidgetToUI(Widget w)
            {
                if (w == null) return;
                updatingUI = true;
                txtName.Text = w.Name;
                numX.Value = w.Mapping.X;
                numY.Value = w.Mapping.Y;
                numW.Value = w.Mapping.Width;
                numH.Value = w.Mapping.Height;
                cmbRotation.SelectedItem = w.Rotation.ToString();
                numPivotX.Value = (decimal)w.Pivot.X;
                numPivotY.Value = (decimal)w.Pivot.Y;
                updatingUI = false;
            }

            void UpdateWidgetFromUI()
            {
                if (selectedWidget == null) return;
                updatingUI = true;
                selectedWidget.Name = txtName.Text;
                selectedWidget.Mapping = new Rectangle((int)numX.Value, (int)numY.Value, (int)numW.Value, (int)numH.Value);
                selectedWidget.Rotation = Enum.Parse<Rotation>(cmbRotation.SelectedItem.ToString());
                selectedWidget.Pivot = new PointF((float)numPivotX.Value, (float)numPivotY.Value);

                int idx = cmbWidgets.SelectedIndex;
                if (idx >= 0)
                    cmbWidgets.Items[idx] = $"{selectedWidget.Name} (ID={selectedWidget.TextureID})";
                updatingUI = false;
            }

            pictureBox.Paint += (s, ev) =>
            {
                DrawScene(ev.Graphics);
            };

            cmbWidgets.SelectedIndexChanged += (s, ev) =>
            {
                if (updatingUI) return;
                if (cmbWidgets.SelectedIndex >= 0 && cmbWidgets.SelectedIndex < widgets.Count)
                {
                    selectedWidget = widgets[cmbWidgets.SelectedIndex];
                    LoadWidgetToUI(selectedWidget);
                    pictureBox.Invalidate();
                }
            };

            void OnWidgetPropertyChanged(object sender, EventArgs e)
            {
                if (updatingUI) return;
                UpdateWidgetFromUI();
                pictureBox.Invalidate();
            }

            txtName.TextChanged += OnWidgetPropertyChanged;
            numX.ValueChanged += OnWidgetPropertyChanged;
            numY.ValueChanged += OnWidgetPropertyChanged;
            numW.ValueChanged += OnWidgetPropertyChanged;
            numH.ValueChanged += OnWidgetPropertyChanged;
            cmbRotation.SelectedIndexChanged += OnWidgetPropertyChanged;
            numPivotX.ValueChanged += OnWidgetPropertyChanged;
            numPivotY.ValueChanged += OnWidgetPropertyChanged;

            pictureBox.MouseDown += (s, ev) =>
            {
                if (selectedWidget == null) return;
                Point mouse = ev.Location;

                int left = (int)(selectedWidget.Mapping.X * fileScaleX * zoom);
                int top = (int)(selectedWidget.Mapping.Y * fileScaleY * zoom);
                int width = (int)(selectedWidget.Mapping.Width * fileScaleX * zoom);
                int height = (int)(selectedWidget.Mapping.Height * fileScaleY * zoom);
                Rectangle screenRect = new Rectangle(left, top, width, height);

                int handle = HitTestHandles(mouse, screenRect, zoom);
                if (handle != -1)
                {
                    isDragging = true;
                    activeHandle = handle;
                    dragStartMouse = mouse;
                    dragStartRect = selectedWidget.Mapping;
                    pictureBox.Capture = true;
                    return;
                }

                if (screenRect.Contains(mouse))
                {
                    isDragging = true;
                    activeHandle = 8;
                    dragStartMouse = mouse;
                    dragStartRect = selectedWidget.Mapping;
                    pictureBox.Capture = true;
                }
            };

            pictureBox.MouseMove += (s, ev) =>
            {
                if (!isDragging || selectedWidget == null) return;
                Point mouse = ev.Location;

                int deltaX = (int)((mouse.X - dragStartMouse.X) / (fileScaleX * zoom));
                int deltaY = (int)((mouse.Y - dragStartMouse.Y) / (fileScaleY * zoom));

                if (activeHandle == 8)
                {
                    Rectangle newRect = new Rectangle(
                        dragStartRect.X + deltaX,
                        dragStartRect.Y + deltaY,
                        dragStartRect.Width,
                        dragStartRect.Height);
                    if (newRect.X < 0) newRect.X = 0;
                    if (newRect.Y < 0) newRect.Y = 0;
                    if (newRect.X + newRect.Width > texture.Width)
                        newRect.X = texture.Width - newRect.Width;
                    if (newRect.Y + newRect.Height > texture.Height)
                        newRect.Y = texture.Height - newRect.Height;
                    selectedWidget.Mapping = newRect;
                }
                else if (activeHandle >= 0 && activeHandle < 8)
                {
                    UpdateRectangleFromHandle(dragStartRect, new Point(deltaX, deltaY), activeHandle, out Rectangle newRect);
                    if (newRect.X < 0) newRect.X = 0;
                    if (newRect.Y < 0) newRect.Y = 0;
                    if (newRect.X + newRect.Width > texture.Width)
                        newRect.Width = texture.Width - newRect.X;
                    if (newRect.Y + newRect.Height > texture.Height)
                        newRect.Height = texture.Height - newRect.Y;
                    if (newRect.Width < 1) newRect.Width = 1;
                    if (newRect.Height < 1) newRect.Height = 1;
                    selectedWidget.Mapping = newRect;
                }

                LoadWidgetToUI(selectedWidget);
                pictureBox.Invalidate();
            };

            pictureBox.MouseUp += (s, ev) =>
            {
                if (isDragging)
                {
                    isDragging = false;
                    activeHandle = -1;
                    pictureBox.Capture = false;
                    UpdateWidgetList();
                    pictureBox.Invalidate();
                }
            };

            pictureBox.MouseClick += (s, ev) =>
            {
                if (isDragging) return;

                float mouseX = ev.X / zoom;
                float mouseY = ev.Y / zoom;

                Widget hit = null;
                foreach (var w in widgets)
                {
                    float left = w.Mapping.X * fileScaleX;
                    float top = w.Mapping.Y * fileScaleY;
                    float right = left + w.Mapping.Width * fileScaleX;
                    float bottom = top + w.Mapping.Height * fileScaleY;
                    if (mouseX >= left && mouseX <= right && mouseY >= top && mouseY <= bottom)
                    {
                        hit = w;
                        break;
                    }
                }

                if (hit != null && hit != selectedWidget)
                {
                    selectedWidget = hit;
                    int idx = widgets.IndexOf(selectedWidget);
                    if (idx >= 0) cmbWidgets.SelectedIndex = idx;
                }
            };

            trackBar.ValueChanged += (s, ev) =>
            {
                zoom = trackBar.Value / 100f;
                lblZoom.Text = $"Scale: {trackBar.Value}%";
                UpdateScaledBackground();
                pictureBox.Invalidate();
            };

            btnZoomIn.Click += (s, ev) => trackBar.Value = Math.Min(trackBar.Maximum, trackBar.Value + 10);
            btnZoomOut.Click += (s, ev) => trackBar.Value = Math.Max(trackBar.Minimum, trackBar.Value - 10);
            btnResetZoom.Click += (s, ev) => trackBar.Value = 100;

            zoom = 1f;
            UpdateScaledBackground();
            UpdateWidgetList();
            if (widgets.Count > 0) cmbWidgets.SelectedIndex = 0;

            tabPage.Disposed += (s, ev) =>
            {
                originalImage?.Dispose();
                scaledBackground?.Dispose();
            };
        }

        private string FindTextureFile(string fileName)
        {
            if (!string.IsNullOrEmpty(texturesFolder))
            {
                string path = Path.Combine(texturesFolder, fileName);
                if (File.Exists(path)) return path;
            }
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                string xmlDir = Path.GetDirectoryName(xmlFilePath);
                string path = Path.Combine(xmlDir, fileName);
                if (File.Exists(path)) return path;
            }
            string workingPath = Path.Combine(Environment.CurrentDirectory, fileName);
            if (File.Exists(workingPath)) return workingPath;
            return null;
        }
    }

    public class TextureInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public XElement OriginalElement { get; }

        public TextureInfo(int id, string name, int width, int height, XElement originalElement)
        {
            ID = id;
            Name = name;
            Width = width;
            Height = height;
            OriginalElement = originalElement;
        }
    }

    public enum Rotation
    {
        None,
        Rotate90,
        Rotate180,
        Rotate270
    }

    public class Widget
    {
        public string Name { get; set; }
        public int TextureID { get; set; }
        public Rectangle Mapping { get; set; }
        public Rotation Rotation { get; set; }
        public PointF Pivot { get; set; }
        public XElement OriginalElement { get; }

        public Widget(string name, int textureId, Rectangle mapping, Rotation rotation, PointF pivot, XElement originalElement)
        {
            Name = name;
            TextureID = textureId;
            Mapping = mapping;
            Rotation = rotation;
            Pivot = pivot;
            OriginalElement = originalElement;
        }
    }
}