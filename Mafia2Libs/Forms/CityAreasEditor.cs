using Core.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{

    public partial class CityAreasEditor : Form
    {
        private CityData cityData;
        private float zoom = 1.0f;
        private PointF offset = new PointF(0, 0);
        private Point lastMousePos;
        private bool panning = false;
        private string currentFilePath;
        private int selectedPolygonIndex = -1;
        private int selectedPointIndex = -1;
        private bool isDraggingPoint = false;

        private bool addingPolygon = false;
        private List<PointF> newPolygonPoints = new List<PointF>();
        private List<int> newPolygonIndices = new List<int>();
        private Image backgroundImage;

        public CityAreasEditor(FileInfo file)
        {
            InitializeComponent();
            cityData = new CityData();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            currentFilePath = file.FullName;
            LoadFile(currentFilePath);
        }

       
        private void LoadFile(string path)
        {
            cityData = CityData.Load(path);
            currentFilePath = path;
            UpdatePolygonList();
            canvasPanel.Invalidate();
        }
        private void SaveCurrentFile(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {

                return;
            }
            try
            {
                cityData.Save(currentFilePath);
                MessageBox.Show("Файл сохранён.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (backgroundImage != null)
            {
                PointF topLeft = WorldToScreen(0, 0);
                PointF bottomRight = WorldToScreen(1, 1);
                RectangleF destRect = new RectangleF(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
                if (destRect.Width > 0 && destRect.Height > 0) e.Graphics.DrawImage(backgroundImage, destRect);
            }

            for (int i = 0; i < cityData.Polygons.Count; i++)
            {
                var poly = cityData.Polygons[i];
                if (poly.PointIndices.Count < 3) continue;

                List<PointF> screenPoints = new List<PointF>();
                foreach (int idx in poly.PointIndices)
                {
                    if (idx >= 0 && idx < cityData.Points.Count)
                    {
                        var p = cityData.Points[idx];
                        screenPoints.Add(WorldToScreen(p.X, p.Y));
                    }
                }

                if (screenPoints.Count >= 3)
                {
                    using (Pen pen = new Pen(i == selectedPolygonIndex ? Color.Red : Color.Blue, 2))
                    {
                        e.Graphics.DrawPolygon(pen, screenPoints.ToArray());
                    }
                }
            }

            Brush pointBrush = Brushes.Black;
            foreach (var point in cityData.Points)
            {
                PointF screen = WorldToScreen(point.X, point.Y);
                e.Graphics.FillEllipse(pointBrush, screen.X - 3, screen.Y - 3, 6, 6);
            }

            if (addingPolygon && newPolygonPoints.Count > 0)
            {
                using (Pen pen = new Pen(Color.Green, 2) { DashStyle = DashStyle.Dash })
                {
                    List<PointF> screenPoints = newPolygonPoints.Select(p => WorldToScreen(p.X, p.Y)).ToList();
                    for (int i = 0; i < screenPoints.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(pen, screenPoints[i], screenPoints[i + 1]);
                    }
                    if (screenPoints.Count >= 2)
                    {
                        e.Graphics.DrawLine(pen, screenPoints.Last(), screenPoints.First());
                    }
                    foreach (var p in screenPoints)
                    {
                        e.Graphics.FillEllipse(Brushes.Green, p.X - 3, p.Y - 3, 6, 6);
                    }
                }
            }
        }

        private PointF WorldToScreen(float x, float y)
        {
            float screenX = (x + offset.X) * canvasPanel.Width * zoom;
            float screenY = (y + offset.Y) * canvasPanel.Height * zoom;
            return new PointF(screenX, screenY);
        }

        private PointF ScreenToWorld(PointF screen)
        {
            float worldX = screen.X / (canvasPanel.Width * zoom) - offset.X;
            float worldY = screen.Y / (canvasPanel.Height * zoom) - offset.Y;
            return new PointF(worldX, worldY);
        }

        private void CanvasPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            PointF centerCanvas = new PointF(canvasPanel.Width / 2f, canvasPanel.Height / 2f);
            PointF worldCenterBefore = ScreenToWorld(centerCanvas);
            float delta = e.Delta > 0 ? 1.1f : 0.9f;
            float newZoom = zoom * delta;
            newZoom = Math.Clamp(newZoom, 0.1f, 10f);
            zoom = newZoom;
            PointF worldCenterAfter = ScreenToWorld(centerCanvas);
            offset.X += worldCenterBefore.X - worldCenterAfter.X;
            offset.Y += worldCenterBefore.Y - worldCenterAfter.Y;
            canvasPanel.Invalidate();

        }
        private void BtnResetView_Click(object sender, EventArgs e)
        {
            zoom = 1.0f;
            offset = new PointF(0, 0);
            canvasPanel.Invalidate();
        }

        private void BtnLoadMap_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        backgroundImage = Image.FromFile(ofd.FileName);
                        canvasPanel.Invalidate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnClearMap_Click(object sender, EventArgs e)
        {
            if (backgroundImage != null)
            {
                backgroundImage.Dispose();
                backgroundImage = null;
                canvasPanel.Invalidate();
            }
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                panning = true;
                lastMousePos = e.Location;
                return;
            }

            PointF worldPos = ScreenToWorld(e.Location);

            if (addingPolygon)
            {
                newPolygonPoints.Add(worldPos);
               
                canvasPanel.Invalidate();

                return;
            }

            float minDist = 5; 
            int closestPoint = -1;
            for (int i = 0; i < cityData.Points.Count; i++)
            {
                PointF screen = WorldToScreen(cityData.Points[i].X, cityData.Points[i].Y);
                float dist = MathF.Sqrt((screen.X - e.X) * (screen.X - e.X) + (screen.Y - e.Y) * (screen.Y - e.Y));
                if (dist < minDist)
                {
                    minDist = dist;
                    closestPoint = i;
                }
            }

            if (closestPoint != -1)
            {
                selectedPointIndex = closestPoint;
                isDraggingPoint = true;
                canvasPanel.Invalidate();

                return;
            }

            if (selectedPolygonIndex != -1)
            {
                var poly = cityData.Polygons[selectedPolygonIndex];
                if (poly.PointIndices.Count >= 3)
                {
                    float bestDist = 10;
                    int bestEdgeStart = -1;
                    for (int i = 0; i < poly.PointIndices.Count; i++)
                    {
                        int idx1 = poly.PointIndices[i];
                        int idx2 = poly.PointIndices[(i + 1) % poly.PointIndices.Count];
                        PointF p1 = WorldToScreen(cityData.Points[idx1].X, cityData.Points[idx1].Y);
                        PointF p2 = WorldToScreen(cityData.Points[idx2].X, cityData.Points[idx2].Y);
                        float dist = DistancePointToSegment(e.Location, p1, p2);
                        if (dist < bestDist)
                        {
                            bestDist = dist;
                            bestEdgeStart = i;
                        }
                    }
                    if (bestEdgeStart != -1)
                    {
                        int idx1 = poly.PointIndices[bestEdgeStart];
                        int idx2 = poly.PointIndices[(bestEdgeStart + 1) % poly.PointIndices.Count];
                        PointF newWorld = new PointF((cityData.Points[idx1].X + cityData.Points[idx2].X) / 2, (cityData.Points[idx1].Y + cityData.Points[idx2].Y) / 2);
                        cityData.Points.Add(new PointData(newWorld.X, newWorld.Y));
                        int newIdx = cityData.Points.Count - 1;
                        poly.PointIndices.Insert(bestEdgeStart + 1, newIdx);
                        canvasPanel.Invalidate();
                        return;
                    }
                }
            }
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (panning)
            {
                float dx = (e.X - lastMousePos.X) / (canvasPanel.Width * zoom);
                float dy = (e.Y - lastMousePos.Y) / (canvasPanel.Height * zoom);
                offset.X += dx;
                offset.Y += dy;
                lastMousePos = e.Location;
                canvasPanel.Invalidate();
                return;
            }

            if (isDraggingPoint && selectedPointIndex != -1)
            {
                PointF world = ScreenToWorld(e.Location);
                cityData.Points[selectedPointIndex].X = world.X;
                cityData.Points[selectedPointIndex].Y = world.Y;
                canvasPanel.Invalidate();
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            panning = false;
            if (isDraggingPoint)
            {
                isDraggingPoint = false;
                selectedPointIndex = -1;
                canvasPanel.Invalidate();
            }
        }

        private void CanvasPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (addingPolygon && newPolygonPoints.Count >= 3)
            {
                List<int> indices = new List<int>();
                foreach (var wp in newPolygonPoints)
                {
                    cityData.Points.Add(new PointData(wp.X, wp.Y));
                    indices.Add(cityData.Points.Count - 1);
                }
                string newName = "NewArea";
                string newTextId = "0";
                cityData.Polygons.Add(new PolygonData(newName, newTextId, indices));
                UpdatePolygonList();
                addingPolygon = false;
                newPolygonPoints.Clear();
                canvasPanel.Invalidate();
            }
        }

        private void PolygonListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (polygonListBox.SelectedIndex >= 0 && polygonListBox.SelectedIndex < cityData.Polygons.Count)
            {
                selectedPolygonIndex = polygonListBox.SelectedIndex;
                var poly = cityData.Polygons[selectedPolygonIndex];
                txtName.Text = poly.Name;
                txtTextID.Text = poly.TextID;
                canvasPanel.Invalidate();
            }
            else
            {
                selectedPolygonIndex = -1;
                txtName.Text = "";
                txtTextID.Text = "";
            }
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if (selectedPolygonIndex != -1)
            {
                cityData.Polygons[selectedPolygonIndex].Name = txtName.Text;
                UpdatePolygonList();
            }
        }

        private void TxtTextID_TextChanged(object sender, EventArgs e)
        {
            if (selectedPolygonIndex != -1)
            {
                cityData.Polygons[selectedPolygonIndex].TextID = txtTextID.Text;
            }
        }

      

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "XML files|*.xml", DefaultExt = "xml" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    cityData = CityData.Load(ofd.FileName);
                    UpdatePolygonList();
                    canvasPanel.Invalidate();
                }
            }
        }

        private void BtnNewPolygon_Click(object sender, EventArgs e)
        {
            addingPolygon = true;
            newPolygonPoints.Clear();
            canvasPanel.Invalidate();
        }

        private void BtnDeletePolygon_Click(object sender, EventArgs e)
        {
            if (selectedPolygonIndex == -1) return;

            var polyToDelete = cityData.Polygons[selectedPolygonIndex];
            var indicesToDelete = polyToDelete.PointIndices.ToHashSet();

            var usedIndices = new HashSet<int>();
            for (int i = 0; i < cityData.Polygons.Count; i++)
            {
                if (i == selectedPolygonIndex) continue;
                foreach (int idx in cityData.Polygons[i].PointIndices)
                    usedIndices.Add(idx);
            }

            var orphanIndices = indicesToDelete.Where(idx => !usedIndices.Contains(idx)).OrderByDescending(idx => idx).ToList();

            foreach (int idx in orphanIndices)
            {
                cityData.Points.RemoveAt(idx);
            }

            var offsetMap = new Dictionary<int, int>();
            int shift = 0;
            for (int i = 0; i < cityData.Points.Count + orphanIndices.Count; i++)
            {
                if (orphanIndices.Contains(i))
                    shift++;
                else
                    offsetMap[i] = i - shift;
            }

            foreach (var poly in cityData.Polygons)
            {
                if (poly == polyToDelete) continue;
                for (int j = 0; j < poly.PointIndices.Count; j++)
                {
                    int oldIdx = poly.PointIndices[j];
                    if (offsetMap.ContainsKey(oldIdx))
                        poly.PointIndices[j] = offsetMap[oldIdx];
                }
            }
            cityData.Polygons.RemoveAt(selectedPolygonIndex);
            selectedPolygonIndex = -1;
            UpdatePolygonList();
            canvasPanel.Invalidate();
        }

        private void BtnCancelAdd_Click(object sender, EventArgs e)
        {
            addingPolygon = false;
            newPolygonPoints.Clear();
            canvasPanel.Invalidate();
        }

        private void UpdatePolygonList()
        {
            polygonListBox.Items.Clear();
            foreach (var poly in cityData.Polygons)
            {
                polygonListBox.Items.Add($"{poly.Name} (ID:{poly.TextID})");
            }
            if (selectedPolygonIndex >= 0 && selectedPolygonIndex < polygonListBox.Items.Count) polygonListBox.SelectedIndex = selectedPolygonIndex;
        }

        private float DistancePointToSegment(PointF p, PointF a, PointF b)
        {
            float ax = p.X - a.X;
            float ay = p.Y - a.Y;
            float bx = b.X - a.X;
            float by = b.Y - a.Y;
            float dot = ax * bx + ay * by;
            float len2 = bx * bx + by * by;
            if (len2 == 0) return MathF.Sqrt(ax * ax + ay * ay);
            float t = dot / len2;
            if (t < 0) t = 0;
            if (t > 1) t = 1;
            float projX = a.X + t * bx;
            float projY = a.Y + t * by;
            float dx = p.X - projX;
            float dy = p.Y - projY;
            return MathF.Sqrt(dx * dx + dy * dy);
        }
    }
}
