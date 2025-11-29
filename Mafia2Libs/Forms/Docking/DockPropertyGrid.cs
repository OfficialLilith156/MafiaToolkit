using Mafia2Tool;
using Mafia2Tool.Forms;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Numerics;
using ResourceTypes.Translokator;
using Utils.Language;
using Utils.VorticeUtils;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockPropertyGrid : DockContent
    {
        private bool isMaterialTabFocused;
        private bool hasLoadedMaterials;
        private object currentObject;
        private TextureEntry currentEntry;
        private Dictionary<TextureEntry, MaterialStruct> currentMaterials;
        private ContextMenuStrip materialContextMenu;
        private decimal savedValueX;
        private decimal savedValueY;
        private decimal savedValueZ;
        private MaterialStruct? copiedMaterial = null;
        public bool IsEntryReady;

        public event EventHandler<EventArgs> OnObjectUpdated;

        public DockPropertyGrid()
        {
            InitializeComponent();
            Localise();
            currentObject = null;
            currentEntry = null;
            IsEntryReady = false;
            isMaterialTabFocused = false;
            hasLoadedMaterials = false;
            currentMaterials = new Dictionary<TextureEntry, MaterialStruct>();

            RotationXNumeric.ValueChanged += Rotation_ValueChanged;
            RotationYNumeric.ValueChanged += Rotation_ValueChanged;
            RotationZNumeric.ValueChanged += Rotation_ValueChanged;
            RotationXNumeric.ValueChanged += InvertZRotation_ValueChanged;
            RotationYNumeric.ValueChanged += InvertZRotation_ValueChanged;
            RotationZNumeric.ValueChanged += InvertZRotation_ValueChanged;

            materialContextMenu = new ContextMenuStrip();
            var copyItem = new ToolStripMenuItem("Copy");
            var pasteItem = new ToolStripMenuItem("Paste");
            copyItem.Click += buttonCopyMaterial_Click;
            pasteItem.Click += buttonPasteMaterial_Click;
            materialContextMenu.Items.AddRange(new ToolStripItem[] { copyItem, pasteItem });
            MatViewPanel.ContextMenuStrip = materialContextMenu;
        }

        private void Localise()
        {
            MainTabControl.TabPages[0].Text = Language.GetString("$PROPERTY_GRID");
            MainTabControl.TabPages[1].Text = Language.GetString("$EDIT_TRANSFORM");
            PositionXLabel.Text = Language.GetString("$POSITION_X");
            PositionYLabel.Text = Language.GetString("$POSITION_Y");
            PositionZLabel.Text = Language.GetString("$POSITION_Z");
            RotationXLabel.Text = Language.GetString("$ROTATION_X");
            RotationYLabel.Text = Language.GetString("$ROTATION_Y");
            RotationZLabel.Text = Language.GetString("$ROTATION_Z");
            ScaleXLabel.Text = Language.GetString("$SCALE_X");
            ScaleYLabel.Text = Language.GetString("$SCALE_Y");
            ScaleZLabel.Text = Language.GetString("$SCALE_Z");
        }

        public void SetObject(object obj)
        {
            currentObject = obj;
            SetTransformEdit();
            SetMaterialTab();
            SetPropertyGrid();
        }

        private void SetMaterialTab()
        {
            hasLoadedMaterials = false;
            currentEntry = null;
            LODComboBox.Items.Clear();
            if (FrameResource.IsFrameType(currentObject))
            {
                if (currentObject is FrameObjectSingleMesh)
                {
                    var entry = (currentObject as FrameObjectSingleMesh);
                    for (int i = 0; i != entry.Geometry.NumLods; i++)
                    {
                        LODComboBox.Items.Add("LOD #" + i);
                    }
                    LODComboBox.SelectedIndex = 0;
                    LoadMaterials();
                }
            }
        }

        private void LoadMaterials()
        {
            if (isMaterialTabFocused && !hasLoadedMaterials)
            {
                MatViewPanel.Controls.Clear();
                currentMaterials.Clear();
                if (FrameResource.IsFrameType(currentObject))
                {
                    if (currentObject is FrameObjectSingleMesh)
                    {
                        var entry = (currentObject as FrameObjectSingleMesh);
                        MaterialStruct[] materialAssignments = entry.Material.Materials[LODComboBox.SelectedIndex];
                        for (int x = 0; x != materialAssignments.Length; x++)
                        {
                            TextureEntry textEntry = new TextureEntry();

                            var mat = materialAssignments[x];
                            IMaterial material = MaterialsManager.LookupMaterialByHash(mat.MaterialHash);

                            textEntry.OnEntrySingularClick += MatViewPanel_TextureEntryOnSingularClick;
                            textEntry.OnEntryDoubleClick += MatViewPanel_TextureEntryOnDoubleClick;
                            textEntry.SetMaterial(material);

                            currentMaterials.Add(textEntry, mat);
                            MatViewPanel.Controls.Add(textEntry);
                        }
                    }
                }

                hasLoadedMaterials = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                CopySelectedMaterial();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.V))
            {
                PasteMaterialToSelected();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CopySelectedMaterial()
        {
            if (currentEntry != null && currentMaterials.ContainsKey(currentEntry))
            {
                copiedMaterial = new MaterialStruct(currentMaterials[currentEntry]);
                Clipboard.SetText(copiedMaterial.MaterialName);
            }
        }
        private void PasteMaterialToSelected()
        {
            if (currentEntry != null && copiedMaterial != null)
            {
                IMaterial mat = MaterialsManager.LookupMaterialByHash(copiedMaterial.MaterialHash);
                if (mat != null)
                {
                    currentMaterials[currentEntry].MaterialName = mat.GetMaterialName();
                    currentMaterials[currentEntry].MaterialHash = mat.GetMaterialHash();
                    currentEntry.SetMaterial(mat);
                    OnObjectUpdated(currentEntry, EventArgs.Empty);
                }
            }
        }

        private void buttonCopyMaterial_Click(object sender, EventArgs e)
        {
            if (currentEntry != null && currentMaterials.ContainsKey(currentEntry))
            {
                copiedMaterial = new MaterialStruct(currentMaterials[currentEntry]);
                Clipboard.SetText(copiedMaterial.MaterialName);
            }
        }
        private void buttonPasteMaterial_Click(object sender, EventArgs e)
        {
            if (currentEntry != null && copiedMaterial != null)
            {
                IMaterial mat = MaterialsManager.LookupMaterialByHash(copiedMaterial.MaterialHash);
                if (mat != null)
                {
                    currentMaterials[currentEntry].MaterialName = mat.GetMaterialName();
                    currentMaterials[currentEntry].MaterialHash = mat.GetMaterialHash();
                    currentEntry.SetMaterial(mat);
                    OnObjectUpdated(currentEntry, EventArgs.Empty);
                }
            }
        }
        private void SetTransformEdit()
        {
            IsEntryReady = false;
            if (FrameResource.IsFrameType(currentObject))
            {
                FrameObjectBase fObject = (currentObject as FrameObjectBase);
                Vector3 position = Vector3.Zero;
                Quaternion rotation2 = Quaternion.Identity;
                Vector3 scale = Vector3.Zero;
                Matrix4x4.Decompose(fObject.LocalTransform, out scale, out rotation2, out position);

                CurrentEntry.Text = fObject.Name.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(position.X);
                PositionYNumeric.Value = Convert.ToDecimal(position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(position.Z);

                Vector3 rotation = rotation2.ToEuler();
                RotationXNumeric.Value = Convert.ToDecimal(rotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(rotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(rotation.Z);
                ScaleXNumeric.Enabled = ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = true;
                ScaleXNumeric.Value = Convert.ToDecimal(scale.X);
                ScaleYNumeric.Value = Convert.ToDecimal(scale.Y);
                ScaleZNumeric.Value = Convert.ToDecimal(scale.Z);
            }
            else if (currentObject is ResourceTypes.Collisions.Collision.Placement)
            {
                ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                CurrentEntry.Text = placement.Hash.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(placement.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(placement.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(placement.Position.Z);
                Vector3 placementRotation = placement.RotationDegrees;
                RotationXNumeric.Value = Convert.ToDecimal(placementRotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(placementRotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(placementRotation.Z);
                ScaleXNumeric.Value = ScaleYNumeric.Value = ScaleZNumeric.Value = 0.0M;
                ScaleXNumeric.Enabled = ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = false;
            }
            else if (currentObject is Instance instance)
            {
                CurrentEntry.Text = instance.ID.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(instance.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(instance.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(instance.Position.Z);
                RotationXNumeric.Value = Convert.ToDecimal(instance.Rotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(instance.Rotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(instance.Rotation.Z);
                ScaleXNumeric.Value = ScaleYNumeric.Value = ScaleZNumeric.Value = Convert.ToDecimal(instance.Scale);
                ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = false;
            }
            IsEntryReady = true;
        }

        private void SetPropertyGrid()
        {
            PropertyGrid.SelectedObject = currentObject;
        }

        public void UpdateObject()
        {
            if (IsEntryReady && currentObject != null)
            {
                Vector3 position = new Vector3(Convert.ToSingle(PositionXNumeric.Value), Convert.ToSingle(PositionYNumeric.Value), Convert.ToSingle(PositionZNumeric.Value));
                Vector3 rotation = new Vector3(Convert.ToSingle(RotationXNumeric.Value), Convert.ToSingle(RotationYNumeric.Value), Convert.ToSingle(RotationZNumeric.Value));
                Vector3 scale = new Vector3(Convert.ToSingle(ScaleXNumeric.Value), Convert.ToSingle(ScaleYNumeric.Value), Convert.ToSingle(ScaleZNumeric.Value));

                if (FrameResource.IsFrameType(currentObject))
                {
                    FrameObjectBase fObject = (currentObject as FrameObjectBase);
                    fObject.LocalTransform = MatrixUtils.SetMatrix(rotation, scale, position);
                }
                else if (currentObject is ResourceTypes.Collisions.Collision.Placement)
                {
                    ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                    placement.Position = position;
                    placement.RotationDegrees = rotation;
                }
                else if (currentObject is Instance instance)
                {
                    instance.Position = position;
                    instance.Rotation = rotation;
                    instance.Scale = scale.X;
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            hasLoadedMaterials = false;
            LoadMaterials();
        }

        private void MatViewPanel_TextureEntryOnDoubleClick(object sender, EventArgs e)
        {
            // Get our entry
            TextureEntry Entry = (sender as TextureEntry);

            // Create our browser; once the user has finished with this menu they should? have a material.
            string MaterialName = "";
            IMaterial OurMaterial = Entry.GetMaterial();
            if (OurMaterial != null)
            {
                MaterialName = OurMaterial.GetMaterialName();
            }

            MaterialBrowser Browser = new MaterialBrowser(MaterialName);
            IMaterial SelectedMaterial = Browser.GetSelectedMaterial();

            // Set the new material data, notify the map editor that a change has been made.
            if (SelectedMaterial != null)
            {
                currentMaterials[Entry].MaterialName = SelectedMaterial.GetMaterialName();
                currentMaterials[Entry].MaterialHash = SelectedMaterial.GetMaterialHash();
                Entry.SetMaterial(SelectedMaterial);
                OnObjectUpdated(sender, e);
            }

            // Yeet the browser into the shadow realm.
            Browser.Dispose();
            Browser = null;
            Entry.IsSelected = false;
        }

        void MatViewPanel_TextureEntryOnSingularClick(object sender, EventArgs e)
        {
            // Set IsSelected for all UCs in the FlowLayoutPanel to false. 
            // Add the new selected one
            TextureEntry Entry = (sender as TextureEntry);

            // Remove the previous entry
            if (currentEntry != null)
            {
                currentEntry.IsSelected = false;
            }

            currentEntry = Entry;
        }

        private void MainTabControl_OnTabIndexChanged(object sender, EventArgs e)
        {
            isMaterialTabFocused = (MainTabControl.SelectedIndex == 2);

            if (currentObject != null)
            {
                LoadMaterials();
            }
        }

        private void ObjectHasUpdated(object sender, EventArgs e)
        {
            OnObjectUpdated(this, EventArgs.Empty);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string copiedText = string.Format(
                "X:{0:0.00000} Y:{1:0.00000} Z:{2:0.00000}",
            PositionXNumeric.Value,
            PositionYNumeric.Value,
            PositionZNumeric.Value
            );

            Clipboard.SetText(copiedText);
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string clipboardText = Clipboard.GetText().Trim();

                decimal x, y, z;

                if (clipboardText.Contains("X:") && clipboardText.Contains("Y:") && clipboardText.Contains("Z:"))
                {
                    var regex = new System.Text.RegularExpressions.Regex(
                        @"X:(?<x>[-+]?[0-9]*\.?[0-9]+)\s+Y:(?<y>[-+]?[0-9]*\.?[0-9]+)\s+Z:(?<z>[-+]?[0-9]*\.?[0-9]+)"
                    );

                    var match = regex.Match(clipboardText);

                    if (!match.Success)
                    {
                        //MessageBox.Show("Error");
                        return;
                    }

                    x = decimal.Parse(match.Groups["x"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    y = decimal.Parse(match.Groups["y"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    z = decimal.Parse(match.Groups["z"].Value, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    string[] parts = clipboardText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 3)
                    {
                        //MessageBox.Show("Error");
                        return;
                    }

                    x = decimal.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                    y = decimal.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                    z = decimal.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);
                }

                PositionXNumeric.Value = x;
                PositionYNumeric.Value = y;
                PositionZNumeric.Value = z;

                savedValueX = x;
                savedValueY = y;
                savedValueZ = z;
            }
            catch { }
        }
        private void Rotation_ValueChanged(object sender, EventArgs e)
        {
            UpdateQuaternion();
        }
        private void InvertZRotation_ValueChanged(object sender, EventArgs e)
        {
            InvertZUpdateQuaternion();
        }

        private void UpdateQuaternion()
        {
            double x = (double)RotationXNumeric.Value;
            double y = (double)RotationYNumeric.Value;
            double z = (double)RotationZNumeric.Value;

            double angle = Math.Sqrt(x * x + y * y + z * z);
            if (angle == 0)
            {
                textQuaternion.Text = "X:0 Y:0 Z:0 W:1";
                return;
            }

            double axisX = x / angle;
            double axisY = y / angle;
            double axisZ = z / angle;

            double angleRad = angle * Math.PI / 180.0;

            double half = angleRad / 2.0;
            double sinHalf = Math.Sin(half);

            Quaternion q = new Quaternion(
                (float)(axisX * sinHalf),
                (float)(axisY * sinHalf),
                (float)(axisZ * sinHalf),
                (float)Math.Cos(half)
            );

            textQuaternion.Text = $"X:{q.X:F6} Y:{q.Y:F6} Z:{q.Z:F6} W:{q.W:F6}";
        }

        private void InvertZUpdateQuaternion()
        {
            double x = (double)RotationXNumeric.Value;
            double y = (double)RotationYNumeric.Value;
            double z = (double)RotationZNumeric.Value;

            double angle = Math.Sqrt(x * x + y * y + z * z);
            if (angle == 0)
            {
                textInvertZQuaternion.Text = "X:0 Y:0 Z:0 W:1";
                return;
            }

            double axisX = x / angle;
            double axisY = y / angle;
            double axisZ = z / angle;

            double angleRad = angle * Math.PI / 180.0;

            double half = angleRad / 2.0;
            double sinHalf = Math.Sin(half);

            Quaternion q = new Quaternion(
                (float)(axisX * sinHalf),
                (float)(axisY * sinHalf),
                (float)(axisZ * sinHalf),
                (float)Math.Cos(half)
            );

            q.Z = -q.Z;

            textInvertZQuaternion.Text = $"X:{q.X:F6} Y:{q.Y:F6} Z:{q.Z:F6} W:{q.W:F6}";
        }
        private void ButtonQuatCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textQuaternion.Text);
        }
        private void ButtonQuatInvertZCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textInvertZQuaternion.Text);
        }
    }
}
