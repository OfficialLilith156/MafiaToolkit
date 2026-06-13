using Gibbed.IO;
using Mafia2Tool.Forms;

using ResourceTypes.Navigation.Traffic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using ZLibNet;

namespace RoadmapEditor
{
    public partial class Form1 : Form
    {
        private IRoadmap _roadmap;
        private IRoadmapFactory _factory;
        private FileFormat _currentFormat;
        private string _currentFilePath;
        private ListBox _listSplines;
        private Panel _canvas;
        private TextBox _txtX, _txtZ, _txtY;
        private Label _lblSelectedPoint;
        private Button _btnSave, _btnAddPoint, _btnDeletePoint;
        private RadioButton _rbCe, _rbDe, _rbXml;
        private int _viewMode = 0;
        private IRoadSpline _currentSpline;
        private object _currentSplineContainer;
        private bool _isJunctionMode;
        private int _selectedPointIndex = -1;
        private PointF _dragStart;
        private Vector2 _dragStartWorld;
        private bool _isDraggingPoint = false;
        private bool _limitDistance = true;
        private PointF _panStart;
        private PointF _panStartOffset;
        private bool _panning;
        private float _minX, _maxX, _minZ, _maxZ;
        private float _scale = 1.0f;
        private PointF _offset;
        private const float MaxSegmentLength = 7.0f;
        private Button _btnNewRoad, _btnDeleteRoad;

        private enum FileFormat { Ce, De, Xml }

        public Form1()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Roadmap Spline Editor";
            this.Size = new Size(1200, 800);

            var menu = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            var openItem = new ToolStripMenuItem("Open...");
            openItem.Click += OpenFile;
            var saveItem = new ToolStripMenuItem("Save");
            saveItem.Click += SaveFile;
            var saveAsItem = new ToolStripMenuItem("Save As...");
            saveAsItem.Click += SaveAsFile;
            fileMenu.DropDownItems.Add(openItem);
            fileMenu.DropDownItems.Add(saveItem);
            fileMenu.DropDownItems.Add(saveAsItem);
            menu.Items.Add(fileMenu);
            this.MainMenuStrip = menu;
            this.Controls.Add(menu);

            var formatPanel = new Panel { Dock = DockStyle.Top, Height = 35 };
            _rbCe = new RadioButton { Text = "CryEngine (Ce)", Location = new Point(10, 5), Checked = true };
            _rbDe = new RadioButton { Text = "De (Legacy)", Location = new Point(130, 5) };
            _rbXml = new RadioButton { Text = "XML", Location = new Point(250, 5) };
            formatPanel.Controls.Add(_rbCe);
            formatPanel.Controls.Add(_rbDe);
            formatPanel.Controls.Add(_rbXml);
            var lblView = new Label { Text = "View:", Location = new Point(360, 5), AutoSize = true };
            var cbView = new ComboBox { Location = new Point(400, 3), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cbView.Items.AddRange(new object[] { "Top-Down (XZ)", "Side (XY)", "Front (ZY)" });
            cbView.SelectedIndex = 0;
            formatPanel.Controls.Add(lblView);
            formatPanel.Controls.Add(cbView);

            this.Controls.Add(formatPanel);

            var split = new SplitContainer { Dock = DockStyle.Fill };
            this.Controls.Add(split);

            _listSplines = new ListBox { Dock = DockStyle.Fill };
            split.Panel1.Controls.Add(_listSplines);
            _listSplines.SelectedIndexChanged += OnSplineSelected;

            var propPanel = new Panel { Dock = DockStyle.Bottom, Height = 150 };
            _lblSelectedPoint = new Label { Text = "Selected point: none", Dock = DockStyle.Top, Height = 30 };
            var lblX = new Label { Text = "X:", Location = new Point(5, 35), AutoSize = true };
            _txtX = new TextBox { Location = new Point(30, 32), Width = 80 };
            var lblZ = new Label { Text = "Z:", Location = new Point(120, 35), AutoSize = true };
            _txtZ = new TextBox { Location = new Point(145, 32), Width = 80 };
            var lblY = new Label { Text = "Y:", Location = new Point(235, 35), AutoSize = true };
            _txtY = new TextBox { Location = new Point(260, 32), Width = 80 };

            _txtX.LostFocus += (s, e) => ApplyPointCoordinates();
            _txtZ.LostFocus += (s, e) => ApplyPointCoordinates();
            _txtY.LostFocus += (s, e) => ApplyPointCoordinates();
            _txtX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ApplyPointCoordinates(); };
            _txtZ.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ApplyPointCoordinates(); };
            _txtY.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ApplyPointCoordinates(); };

            _btnAddPoint = new Button { Text = "Add Point", Location = new Point(5, 70), Width = 100 };
            _btnDeletePoint = new Button { Text = "Delete Point", Location = new Point(110, 70), Width = 100 };
            _btnAddPoint.Click += OnAddPoint;
            _btnDeletePoint.Click += OnDeletePoint;

            propPanel.Controls.Add(_lblSelectedPoint);
            propPanel.Controls.Add(lblX);
            propPanel.Controls.Add(_txtX);
            propPanel.Controls.Add(lblZ);
            propPanel.Controls.Add(_txtZ);
            propPanel.Controls.Add(lblY);
            propPanel.Controls.Add(_txtY);
            propPanel.Controls.Add(_btnAddPoint);
            propPanel.Controls.Add(_btnDeletePoint);

            _btnNewRoad = new Button { Text = "New Road", Location = new Point(220, 70), Width = 100 };
            _btnDeleteRoad = new Button { Text = "Delete Road", Location = new Point(330, 70), Width = 100 };
            _btnNewRoad.Click += OnNewRoad;
            _btnDeleteRoad.Click += OnDeleteRoad;

            propPanel.Controls.Add(_btnNewRoad);
            propPanel.Controls.Add(_btnDeleteRoad);

            split.Panel1.Controls.Add(propPanel);

            cbView.SelectedIndexChanged += (s, ev) => {
                _viewMode = cbView.SelectedIndex;
                _canvas.Invalidate();
            };

            var chkLimit = new CheckBox
            {
                Text = "Limit distance (max 7)",
                Location = new Point(530, 5),
                AutoSize = true,
                Checked = true
            };
            chkLimit.CheckedChanged += (s, ev) => { _limitDistance = chkLimit.Checked; };
            formatPanel.Controls.Add(chkLimit);

            _canvas = new BufferedPanel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30) };
            _canvas.Paint += OnCanvasPaint;

            _canvas.MouseDown += OnCanvasMouseDown;
            _canvas.MouseMove += OnCanvasMouseMove;
            _canvas.MouseUp += OnCanvasMouseUp;
            _canvas.MouseWheel += OnCanvasMouseWheel;
            _canvas.Resize += (s, e) => { if (_roadmap != null) UpdateAllBounds(); };
            split.Panel2.Controls.Add(_canvas);



            this.Load += (sender, e) =>
            {
                split.Panel1MinSize = 200;
                split.Panel2MinSize = 400;
                int desiredLeftWidth = 300;
                if (desiredLeftWidth >= split.Panel1MinSize &&
                    desiredLeftWidth <= split.Width - split.Panel2MinSize)
                    split.SplitterDistance = desiredLeftWidth;
            };
        }



        private void OpenFile(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = _rbXml.Checked ? "XML files (*.xml)|*.xml|All files (*.*)|*.*" : "Binary files (*.gsd)|*.gsd|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _currentFilePath = ofd.FileName;
                    _currentFormat = _rbCe.Checked ? FileFormat.Ce : (_rbDe.Checked ? FileFormat.De : FileFormat.Xml);

                    try
                    {
                        LoadRoadmap(_currentFilePath, _currentFormat);
                        RefreshSplineList();
                        UpdateAllBounds();
                        _canvas.Invalidate();
                        MessageBox.Show("Loaded successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadRoadmap(string path, FileFormat format)
        {
            using (var fs = File.OpenRead(path))
            {
                switch (format)
                {
                    case FileFormat.Ce:
                        _factory = new RoadmapFactoryCe();
                        _roadmap = _factory.Roadmap();
                        ((IRoadmapSerializableCe)_roadmap).Read(fs, Endian.Little);
                        break;
                    case FileFormat.De:
                        _factory = new RoadmapFactoryDe();
                        _roadmap = _factory.Roadmap();
                        ((RoadmapDe)_roadmap).Read(fs, Endian.Little);
                        break;
                    case FileFormat.Xml:
                        _factory = new RoadmapFactoryCe();
                        var xmlSerializer = new RoadmapXmlSerializer();
                        _roadmap = xmlSerializer.Deserialize(_factory, path);
                        break;
                }
            }
        }

        private void SaveFile(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveAsFile(sender, e);
                return;
            }
            SaveRoadmap(_currentFilePath, _currentFormat);
        }

        private void SaveAsFile(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = _currentFormat == FileFormat.Xml ? "XML files (*.xml)|*.xml" : "Binary files (*.gsd)|*.gsd";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _currentFilePath = sfd.FileName;
                    SaveRoadmap(_currentFilePath, _currentFormat);
                }
            }
        }

        private void SaveRoadmap(string path, FileFormat format)
        {
            RecalculateAllSplineLengths();
            UpdateCostMap();

            try
            {
                using (var fs = File.Create(path))
                {
                    switch (format)
                    {
                        case FileFormat.Ce:
                            ((IRoadmapSerializableCe)_roadmap).Write(fs, Endian.Little);
                            break;
                        case FileFormat.De:
                            ((RoadmapDe)_roadmap).Write(fs, Endian.Little);
                            break;
                        case FileFormat.Xml:
                            var xmlSerializer = new RoadmapXmlSerializer();
                            xmlSerializer.Serialize(_roadmap, path);
                            break;
                    }
                }
                MessageBox.Show("Saved successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecalculateAllSplineLengths()
        {
            foreach (var spline in _roadmap.Splines)
                spline.CalculateLength();

            foreach (var crossroad in _roadmap.Crossroads)
                foreach (var junction in crossroad.Junctions)
                    junction.Spline.CalculateLength();
        }

        private void UpdateCostMap()
        {
            if (_currentFormat == FileFormat.Xml) return;
            for (int i = 0; i < _roadmap.CostMap.Count; i++)
            {
                var entry = _roadmap.CostMap[i];
                if (entry.RoadGraphEdgeType == RoadGraphEdgeType.Road)
                {
                    int roadIdx = entry.RoadGraphEdgeLink;
                    if (roadIdx < _roadmap.Roads.Count)
                        entry.Cost = _roadmap.CalculateRoadCost(_roadmap.Roads[roadIdx]);
                }
            }
        }

        private void RefreshSplineList()
        {
            _listSplines.Items.Clear();

            for (int i = 0; i < _roadmap.Splines.Count; i++)
                _listSplines.Items.Add($"Road {i} (Points: {_roadmap.Splines[i].Points.Count})");

            int junctionCounter = 0;
            for (int c = 0; c < _roadmap.Crossroads.Count; c++)
            {
                var crossroad = _roadmap.Crossroads[c];
                for (int j = 0; j < crossroad.Junctions.Count; j++)
                {
                    var junction = crossroad.Junctions[j];
                    _listSplines.Items.Add($"Junction {junctionCounter} (Crossroad {c}, Points: {junction.Spline.Points.Count})");
                    junctionCounter++;
                }
            }
        }

        private void OnSplineSelected(object sender, EventArgs e)
        {
            int idx = _listSplines.SelectedIndex;
            if (idx < 0) return;

            int roadCount = _roadmap.Splines.Count;
            if (idx < roadCount)
            {
                _currentSpline = _roadmap.Splines[idx];
                _currentSplineContainer = _roadmap.Roads.FirstOrDefault(r => r.RoadSplineIndex == idx);
                _isJunctionMode = false;
            }
            else
            {
                int junctionIdx = idx - roadCount;
                int j = 0;
                foreach (var crossroad in _roadmap.Crossroads)
                {
                    if (junctionIdx < crossroad.Junctions.Count)
                    {
                        var junction = crossroad.Junctions[junctionIdx];
                        _currentSpline = junction.Spline;
                        _currentSplineContainer = junction;
                        _isJunctionMode = true;
                        break;
                    }
                    junctionIdx -= crossroad.Junctions.Count;
                }
            }

            _selectedPointIndex = -1;
            UpdateSelectedPointDisplay();
            _canvas.Invalidate();
        }


        private void ComputeAllBounds()
        {
            if (_roadmap == null || (_roadmap.Splines.Count == 0 && _roadmap.Crossroads.Count == 0))
            {
                _minX = _maxX = _minZ = _maxZ = 0;
                return;
            }

            _minX = float.MaxValue;
            _maxX = float.MinValue;
            _minZ = float.MaxValue;
            _maxZ = float.MinValue;

            foreach (var spline in _roadmap.Splines)
            {
                foreach (var p in spline.Points)
                {
                    if (p.X < _minX) _minX = p.X;
                    if (p.X > _maxX) _maxX = p.X;
                    if (p.Z < _minZ) _minZ = p.Z;
                    if (p.Z > _maxZ) _maxZ = p.Z;
                }
            }

            foreach (var crossroad in _roadmap.Crossroads)
            {
                foreach (var junction in crossroad.Junctions)
                {
                    foreach (var p in junction.Spline.Points)
                    {
                        if (p.X < _minX) _minX = p.X;
                        if (p.X > _maxX) _maxX = p.X;
                        if (p.Z < _minZ) _minZ = p.Z;
                        if (p.Z > _maxZ) _maxZ = p.Z;
                    }
                }
            }

            float pad = 20f;
            _minX -= pad;
            _maxX += pad;
            _minZ -= pad;
            _maxZ += pad;
        }

        private void UpdateAllBounds()
        {
            if (_roadmap == null || _canvas == null) return;
            ComputeAllBounds();

            float width = _maxX - _minX;
            float height = _maxZ - _minZ;
            if (width < 0.1f) width = 10;
            if (height < 0.1f) height = 10;

            float padding = 50;
            float scaleX = (_canvas.Width - padding) / width;
            float scaleZ = (_canvas.Height - padding) / height;
            _scale = Math.Min(scaleX, scaleZ);
            if (_scale < 0.01f) _scale = 0.01f;

            float centerX = (_minX + _maxX) / 2;
            float centerZ = (_minZ + _maxZ) / 2;
            float screenCenterX = _canvas.Width / 2f;
            float screenCenterZ = _canvas.Height / 2f;
            _offset = new PointF(screenCenterX - centerX * _scale, screenCenterZ - centerZ * _scale);
        }



        private void OnCanvasPaint(object sender, PaintEventArgs e)
        {
            if (_roadmap == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawGrid(g);

            foreach (var spline in _roadmap.Splines)
                DrawSpline(g, spline, spline == _currentSpline && !_isJunctionMode, Color.LimeGreen, Color.Gray);

            foreach (var crossroad in _roadmap.Crossroads)
            {
                foreach (var junction in crossroad.Junctions)
                {
                    bool isActive = (_currentSpline == junction.Spline && _isJunctionMode);
                    DrawSpline(g, junction.Spline, isActive, Color.Orange, Color.DarkOrange);
                }
            }
        }
        private void DrawSpline(Graphics g, IRoadSpline spline, bool isActive, Color activeColor, Color inactiveColor)
        {
            var points = spline.Points;
            if (points.Count < 2) return;

            Color lineColor = isActive ? activeColor : inactiveColor;
            using (var pen = new Pen(lineColor, 2))
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = WorldToScreen(points[i]);
                    var p2 = WorldToScreen(points[i + 1]);
                    g.DrawLine(pen, p1, p2);
                }
            }

            if (isActive)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var screen = WorldToScreen(points[i]);
                    var brush = (i == _selectedPointIndex) ? Brushes.Red : Brushes.Yellow;
                    g.FillEllipse(brush, screen.X - 4, screen.Y - 4, 8, 8);
                    g.DrawString(i.ToString(), SystemFonts.DefaultFont, Brushes.White, screen.X + 5, screen.Y - 8);
                }
            }
            else
            {
                foreach (var p in points)
                {
                    var screen = WorldToScreen(p);
                    using (var brush = new SolidBrush(Color.FromArgb(120, 120, 120)))
                        g.FillEllipse(brush, screen.X - 2, screen.Y - 2, 4, 4);
                }
            }
        }

        private void DrawGrid(Graphics g)
        {
            int step = 50;
            using (var pen = new Pen(Color.Gray, 1))
            {
                for (int x = 0; x < _canvas.Width; x += step)
                    g.DrawLine(pen, x, 0, x, _canvas.Height);
                for (int y = 0; y < _canvas.Height; y += step)
                    g.DrawLine(pen, 0, y, _canvas.Width, y);
            }
        }

        private PointF WorldToScreen(Vector3 world)
        {
            float screenX, screenY;
            switch (_viewMode)
            {
                case 0:
                    screenX = world.X * _scale + _offset.X;
                    screenY = world.Z * _scale + _offset.Y;
                    break;
                case 1:
                    screenX = world.X * _scale + _offset.X;
                    screenY = -world.Y * _scale + _offset.Y;
                    break;
                case 2:
                    screenX = world.Z * _scale + _offset.X;
                    screenY = -world.Y * _scale + _offset.Y;
                    break;
                default:
                    screenX = world.X * _scale + _offset.X;
                    screenY = world.Z * _scale + _offset.Y;
                    break;
            }
            return new PointF(screenX, screenY);
        }

        private Vector3 ScreenToWorld(PointF screen)
        {
            float worldX = 0, worldY = 0, worldZ = 0;
            switch (_viewMode)
            {
                case 0:
                    worldX = (screen.X - _offset.X) / _scale;
                    worldZ = (screen.Y - _offset.Y) / _scale;
                    if (_currentSpline != null && _selectedPointIndex >= 0)
                        worldY = _currentSpline.Points[_selectedPointIndex].Y;
                    break;
                case 1:
                    worldX = (screen.X - _offset.X) / _scale;
                    worldY = -(screen.Y - _offset.Y) / _scale;
                    if (_currentSpline != null && _selectedPointIndex >= 0)
                        worldZ = _currentSpline.Points[_selectedPointIndex].Z;
                    break;
                case 2:
                    worldZ = (screen.X - _offset.X) / _scale;
                    worldY = -(screen.Y - _offset.Y) / _scale;
                    if (_currentSpline != null && _selectedPointIndex >= 0)
                        worldX = _currentSpline.Points[_selectedPointIndex].X;
                    break;
            }
            return new Vector3(worldX, worldY, worldZ);
        }

        private void OnCanvasMouseWheel(object sender, MouseEventArgs e)
        {
            if (_roadmap == null) return;

            PointF mouseScreen = new PointF(e.X, e.Y);

            float worldX = (mouseScreen.X - _offset.X) / _scale;
            float worldY_or_Z;

            switch (_viewMode)
            {
                case 0:
                    worldY_or_Z = (mouseScreen.Y - _offset.Y) / _scale; 
                    break;
                case 1:
                    worldY_or_Z = -(mouseScreen.Y - _offset.Y) / _scale;
                    break;
                case 2:
                    worldY_or_Z = (mouseScreen.X - _offset.X) / _scale;
                    worldX = (mouseScreen.Y - _offset.Y) / _scale;
                    break;
                default:
                    worldY_or_Z = (mouseScreen.Y - _offset.Y) / _scale;
                    break;
            }

            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            float newScale = _scale * zoomFactor;
            newScale = Math.Max(0.01f, Math.Min(1000f, newScale));
            _scale = newScale;

            float newOffsetX, newOffsetY;
            switch (_viewMode)
            {
                case 0:
                    newOffsetX = mouseScreen.X - worldX * _scale;
                    newOffsetY = mouseScreen.Y - worldY_or_Z * _scale;
                    break;
                case 1:
                    newOffsetX = mouseScreen.X - worldX * _scale;
                    newOffsetY = mouseScreen.Y + worldY_or_Z * _scale;
                    break;
                case 2:
                    float worldZ = (mouseScreen.X - _offset.X) / _scale;
                    newOffsetX = mouseScreen.X - worldZ * _scale;
                    newOffsetY = mouseScreen.Y + worldY_or_Z * _scale;
                    break;
                default:
                    newOffsetX = mouseScreen.X - worldX * _scale;
                    newOffsetY = mouseScreen.Y - worldY_or_Z * _scale;
                    break;
            }

            _offset = new PointF(newOffsetX, newOffsetY);
            _canvas.Invalidate();
        }
        private void ApplyPointCoordinates()
        {
            if (_currentSpline == null || _selectedPointIndex < 0 || _selectedPointIndex >= _currentSpline.Points.Count)
                return;

            if (!float.TryParse(_txtX.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float newX) ||
                !float.TryParse(_txtZ.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float newZ) ||
                !float.TryParse(_txtY.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float newY))
            {
                MessageBox.Show("Invalid coordinate format. Use numbers (e.g., 123.45).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateSelectedPointDisplay();
                return;
            }

            Vector3 desiredPos = new Vector3(newX, newY, newZ);
            Vector3 clampedPos = ClampPointPosition(_currentSpline, _selectedPointIndex, desiredPos);

            if (clampedPos != desiredPos)
            {
                _txtX.Text = clampedPos.X.ToString("0.00");
                _txtZ.Text = clampedPos.Z.ToString("0.00");
                _txtY.Text = clampedPos.Y.ToString("0.00");
                MessageBox.Show($"Position adjusted to respect max segment length ({MaxSegmentLength}).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            _currentSpline.Points[_selectedPointIndex] = clampedPos;
            _currentSpline.CalculateLength();
            _canvas.Invalidate();
        }

        private void OnCanvasMouseDown(object sender, MouseEventArgs e)
        {
            if (_roadmap == null) return;
            _canvas.Focus();

            if (e.Button == MouseButtons.Right)
            {
                _panning = true;
                _panStart = new PointF(e.X, e.Y);
                _panStartOffset = _offset;
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (_currentSpline != null)
                {
                    float minDist = 10;
                    int hitIndex = -1;
                    for (int i = 0; i < _currentSpline.Points.Count; i++)
                    {
                        var screen = WorldToScreen(_currentSpline.Points[i]);
                        float dx = screen.X - e.X;
                        float dy = screen.Y - e.Y;
                        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            hitIndex = i;
                        }
                    }

                    if (hitIndex >= 0)
                    {
                        _selectedPointIndex = hitIndex;
                        _dragStart = new PointF(e.X, e.Y);
                        _dragStartWorld = new Vector2(_currentSpline.Points[hitIndex].X, _currentSpline.Points[hitIndex].Z);
                        _isDraggingPoint = false; 
                        UpdateSelectedPointDisplay();
                        _canvas.Invalidate();
                        return;
                    }
                }

                var hit = FindSplineAtScreenPoint(new PointF(e.X, e.Y));
                if (hit.spline != null)
                {
                    _currentSpline = hit.spline;
                    _currentSplineContainer = hit.container;
                    _isJunctionMode = hit.isJunction;
                    _selectedPointIndex = -1;
                    _isDraggingPoint = false;

                    int listIdx = GetListIndexForSpline(_currentSpline, _isJunctionMode);
                    if (listIdx >= 0) _listSplines.SelectedIndex = listIdx;

                    UpdateSelectedPointDisplay();
                    _canvas.Invalidate();
                }
                else
                {
                    _selectedPointIndex = -1;
                    _isDraggingPoint = false;
                    UpdateSelectedPointDisplay();
                    _canvas.Invalidate();
                }
            }
        }

        private (IRoadSpline spline, object container, bool isJunction) FindSplineAtScreenPoint(PointF screenPoint)
        {
            float bestDistance = 10f;
            IRoadSpline bestSpline = null;
            object bestContainer = null;
            bool bestIsJunction = false;

            for (int s = 0; s < _roadmap.Splines.Count; s++)
            {
                var spline = _roadmap.Splines[s];
                float dist = DistanceToSpline(screenPoint, spline);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    bestSpline = spline;
                    bestContainer = _roadmap.Roads.FirstOrDefault(r => r.RoadSplineIndex == s);
                    bestIsJunction = false;
                }
            }

            foreach (var crossroad in _roadmap.Crossroads)
            {
                foreach (var junction in crossroad.Junctions)
                {
                    float dist = DistanceToSpline(screenPoint, junction.Spline);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestSpline = junction.Spline;
                        bestContainer = junction;
                        bestIsJunction = true;
                    }
                }
            }

            return (bestSpline, bestContainer, bestIsJunction);
        }

        private Vector3 ClampPointPosition(IRoadSpline spline, int pointIndex, Vector3 desiredPos)
        {
            if (!_limitDistance) return desiredPos;

            var points = spline.Points;
            Vector3 result = desiredPos;

            if (points.Count == 1) return result;

            if (pointIndex == 0 && points.Count > 1)
            {
                Vector3 next = points[1];
                float dist = Vector3.Distance(desiredPos, next);
                if (dist > MaxSegmentLength)
                {
                    Vector3 dir = Vector3.Normalize(desiredPos - next);
                    result = next + dir * MaxSegmentLength;
                }
                return result;
            }

            if (pointIndex == points.Count - 1 && points.Count > 1)
            {
                Vector3 prev = points[points.Count - 2];
                float dist = Vector3.Distance(desiredPos, prev);
                if (dist > MaxSegmentLength)
                {
                    Vector3 dir = Vector3.Normalize(desiredPos - prev);
                    result = prev + dir * MaxSegmentLength;
                }
                return result;
            }

            Vector3 prevPoint = points[pointIndex - 1];
            Vector3 nextPoint = points[pointIndex + 1];

            if (Vector3.Distance(prevPoint, nextPoint) < 0.001f)
            {
                float distToPrev = Vector3.Distance(desiredPos, prevPoint);
                if (distToPrev > MaxSegmentLength)
                {
                    Vector3 dir = Vector3.Normalize(desiredPos - prevPoint);
                    result = prevPoint + dir * MaxSegmentLength;
                }
                return result;
            }

            Vector3 lineDir = nextPoint - prevPoint;
            float lineLen = lineDir.Length();
            lineDir /= lineLen;

            Vector3 toDesired = desiredPos - prevPoint;
            float t = Vector3.Dot(toDesired, lineDir);
            t = Math.Clamp(t, 0, lineLen);

            float distToPrevProj = t;
            float distToNextProj = lineLen - t;

            if (distToPrevProj > MaxSegmentLength)
                t = MaxSegmentLength;
            if (distToNextProj > MaxSegmentLength)
                t = lineLen - MaxSegmentLength;

            t = Math.Clamp(t, 0, lineLen);
            result = prevPoint + lineDir * t;

            return result;
        }

        private float DistanceToSpline(PointF screenPoint, IRoadSpline spline)
        {
            var points = spline.Points;
            if (points.Count < 2) return float.MaxValue;

            float minDist = float.MaxValue;
            for (int i = 0; i < points.Count - 1; i++)
            {
                PointF p1 = WorldToScreen(points[i]);
                PointF p2 = WorldToScreen(points[i + 1]);
                float dist = DistancePointToSegment(screenPoint, p1, p2);
                if (dist < minDist) minDist = dist;
            }
            return minDist;
        }
        private int GetListIndexForSpline(IRoadSpline spline, bool isJunction)
        {
            if (!isJunction)
            {
                for (int i = 0; i < _roadmap.Splines.Count; i++)
                    if (_roadmap.Splines[i] == spline)
                        return i;
            }
            else
            {
                int offset = _roadmap.Splines.Count;
                foreach (var crossroad in _roadmap.Crossroads)
                {
                    for (int j = 0; j < crossroad.Junctions.Count; j++)
                    {
                        if (crossroad.Junctions[j].Spline == spline)
                            return offset + j;
                    }
                    offset += crossroad.Junctions.Count;
                }
            }
            return -1;
        }

        private float DistancePointToSegment(PointF p, PointF a, PointF b)
        {
            float ax = p.X - a.X;
            float ay = p.Y - a.Y;
            float bx = b.X - a.X;
            float by = b.Y - a.Y;
            float dot = ax * bx + ay * by;
            float len2 = bx * bx + by * by;
            if (len2 == 0) return (float)Math.Sqrt(ax * ax + ay * ay);
            float t = dot / len2;
            if (t < 0) t = 0;
            if (t > 1) t = 1;
            float projX = a.X + t * bx;
            float projY = a.Y + t * by;
            float dx = p.X - projX;
            float dy = p.Y - projY;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (_roadmap == null) return;

            if (_panning && e.Button == MouseButtons.Right)
            {
                PointF delta = new PointF(e.X - _panStart.X, e.Y - _panStart.Y);
                _offset = new PointF(_panStartOffset.X + delta.X, _panStartOffset.Y + delta.Y);
                _canvas.Invalidate();
                return;
            }

            if (_selectedPointIndex >= 0 && e.Button == MouseButtons.Left && _currentSpline != null)
            {
                if (!_isDraggingPoint)
                    _isDraggingPoint = true;

                var newWorld = ScreenToWorld(new PointF(e.X, e.Y));
                var point = _currentSpline.Points[_selectedPointIndex];
                Vector3 desiredPos = point;

                switch (_viewMode)
                {
                    case 0:
                        desiredPos.X = newWorld.X;
                        desiredPos.Z = newWorld.Z;
                        break;
                    case 1:
                        desiredPos.X = newWorld.X;
                        desiredPos.Y = newWorld.Y;
                        break;
                    case 2:
                        desiredPos.Z = newWorld.Z;
                        desiredPos.Y = newWorld.Y;
                        break;
                }

                Vector3 clampedPos = ClampPointPosition(_currentSpline, _selectedPointIndex, desiredPos);
                _currentSpline.Points[_selectedPointIndex] = clampedPos;

                UpdateSelectedPointDisplay();
                _currentSpline.CalculateLength();
                _canvas.Invalidate();
            }
        }

        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        {
            _panning = false;
        }

        private void UpdateSelectedPointDisplay()
        {
            if (_selectedPointIndex >= 0 && _currentSpline != null && _selectedPointIndex < _currentSpline.Points.Count)
            {
                var p = _currentSpline.Points[_selectedPointIndex];
                _lblSelectedPoint.Text = $"Selected point: {_selectedPointIndex}";
                _txtX.Text = p.X.ToString("0.00");
                _txtZ.Text = p.Z.ToString("0.00");
                _txtY.Text = p.Y.ToString("0.00");
            }
            else
            {
                _lblSelectedPoint.Text = "Selected point: none";
                _txtX.Text = "";
                _txtZ.Text = "";
                _txtY.Text = "";
            }
        }

        private void OnAddPoint(object sender, EventArgs e)
        {
            if (_currentSpline == null) return;

            Vector3 newPoint;
            if (_currentSpline.Points.Count == 0)
            {
                newPoint = new Vector3(0, 0, 0);
            }
            else
            {
                var last = _currentSpline.Points.Last();
                newPoint = new Vector3(last.X + 10, last.Y, last.Z);

                float dist = Vector3.Distance(last, newPoint);
                if (dist > MaxSegmentLength)
                {
                    Vector3 direction = Vector3.Normalize(newPoint - last);
                    newPoint = last + direction * MaxSegmentLength;
                }
            }

            _currentSpline.Points.Add(newPoint);
            _currentSpline.CalculateLength();
            RefreshSplineList();
            _canvas.Invalidate();
        }

        private void OnDeletePoint(object sender, EventArgs e)
        {
            if (_currentSpline == null || _selectedPointIndex < 0) return;
            _currentSpline.Points.RemoveAt(_selectedPointIndex);
            _selectedPointIndex = -1;
            _currentSpline.CalculateLength();
            RefreshSplineList();
            _canvas.Invalidate();
        }

        private void OnNewRoad(object sender, EventArgs e)
        {
            if (_roadmap == null)
            {
                CreateEmptyRoadmap();
            }

            IRoadSpline newSpline = _factory.Spline();
            newSpline.Points.Add(new Vector3(0, 0, 0));
            newSpline.CalculateLength();

            IRoadDefinition newRoad = _factory.Road();
            newRoad.RoadSplineIndex = (ushort)_roadmap.Splines.Count;
            newRoad.RoadType = RoadType.Road;
            newRoad.Direction = RoadDirection.Towards;
            newRoad.ForwardLanesCount = 1;
            newRoad.OppositeLanesCount = 1;
            newRoad.MaxSpawnedCars = 5;
            ILaneDefinition lane = _factory.Lane();
            lane.Width = 3.5f;
            lane.LaneType = LaneType.MainRoad;
            lane.CenterOffset = 0;
            newRoad.Lanes.Add(lane);
            _roadmap.Splines.Add(newSpline);
            _roadmap.Roads.Add(newRoad);

            RefreshSplineList();
            UpdateAllBounds();
            _canvas.Invalidate();

            _currentSpline = newSpline;
            _currentSplineContainer = newRoad;
            _isJunctionMode = false;
            _selectedPointIndex = -1;
            UpdateSelectedPointDisplay();
            _listSplines.SelectedIndex = _roadmap.Splines.Count - 1;
        }

        private void OnDeleteRoad(object sender, EventArgs e)
        {
            if (_currentSpline == null || _isJunctionMode)
            {
                MessageBox.Show("Select a road (not a junction) to delete.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int indexToDelete = -1;
            for (int i = 0; i < _roadmap.Splines.Count; i++)
            {
                if (_roadmap.Splines[i] == _currentSpline)
                {
                    indexToDelete = i;
                    break;
                }
            }
            if (indexToDelete == -1) return;

            var roadToDelete = _roadmap.Roads.FirstOrDefault(r => r.RoadSplineIndex == indexToDelete);
            if (roadToDelete != null)
                _roadmap.Roads.Remove(roadToDelete);

            _roadmap.Splines.RemoveAt(indexToDelete);

            foreach (var road in _roadmap.Roads)
            {
                if (road.RoadSplineIndex > indexToDelete)
                    road.RoadSplineIndex--;
            }

            _currentSpline = null;
            _currentSplineContainer = null;
            _selectedPointIndex = -1;
            UpdateSelectedPointDisplay();

            RefreshSplineList();
            UpdateAllBounds();
            _canvas.Invalidate();

            MessageBox.Show("Road deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateEmptyRoadmap()
        {
            _currentFormat = _rbCe.Checked ? FileFormat.Ce : (_rbDe.Checked ? FileFormat.De : FileFormat.Xml);
            switch (_currentFormat)
            {
                case FileFormat.Ce:
                    _factory = new RoadmapFactoryCe();
                    break;
                case FileFormat.De:
                    _factory = new RoadmapFactoryDe();
                    break;
                case FileFormat.Xml:
                    _factory = new RoadmapFactoryCe();
                    break;
            }
            _roadmap = _factory.Roadmap();
            _currentFilePath = null;
        }

    }
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}