using Core.IO;
using ResourceTypes.ItemDesc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Utils.VorticeUtils;

namespace Mafia2Tool.Forms
{
    public partial class ItemDescEditor : Form
    {
        public ItemDescLoader ItemDesc { get; private set; }
        private string FullFilePath;

        private bool bIsFileEdited = false;
        private string OriginalFileName;

        public ItemDescEditor(ItemDescLoader itemDesc, string filePath)
        {
            InitializeComponent();
            ItemDesc = itemDesc;
            FullFilePath = itemDesc.FullPath;
            OriginalFileName = itemDesc.FileName;
            BuildData();
        }

        private void BuildData()
        {
            TreeView_Data.Nodes.Clear();

            TreeNode rootNode = new TreeNode("ItemDesc");
            rootNode.Tag = ItemDesc;

            TreeNode infoNode = new TreeNode("Info");

            TreeNode frameRefNode = new TreeNode($"FrameRef: {ItemDesc.FrameRef}");
            frameRefNode.Tag = ItemDesc.FrameRef;
            infoNode.Nodes.Add(frameRefNode);

            TreeNode unkByte1Node = new TreeNode($"Unknown Byte 1: {ItemDesc.UnkByte1}");
            unkByte1Node.Tag = ItemDesc.UnkByte1;
            infoNode.Nodes.Add(unkByte1Node);

            TreeNode colTypeNode = new TreeNode($"Collision Type: {ItemDesc.ColType} ({(int)ItemDesc.ColType})");
            colTypeNode.Tag = ItemDesc.ColType;
            infoNode.Nodes.Add(colTypeNode);

            TreeNode idHashNode = new TreeNode($"ID Hash: 0x{ItemDesc.IdHash:X16}");
            idHashNode.Tag = ItemDesc.IdHash;
            infoNode.Nodes.Add(idHashNode);

            TreeNode colMaterialNode = new TreeNode($"Collision Material: {ItemDesc.ColMaterial}");
            colMaterialNode.Tag = ItemDesc.ColMaterial;
            infoNode.Nodes.Add(colMaterialNode);

            TreeNode unkByte2Node = new TreeNode($"Unknown Byte 2: {ItemDesc.UnkByte2}");
            unkByte2Node.Tag = ItemDesc.UnkByte2;
            infoNode.Nodes.Add(unkByte2Node);

            rootNode.Nodes.Add(infoNode);

            TreeNode matrixNode = new TreeNode("Transformation Matrix");
            matrixNode.Tag = ItemDesc.Matrix;

            for (int i = 0; i < 4; i++)
            {
                Vector4 row = ItemDesc.Matrix.GetRow(i);
                TreeNode rowNode = new TreeNode($"Row {i}: [{row.X:F6}, {row.Y:F6}, {row.Z:F6}, {row.W:F6}]");
                rowNode.Tag = row;
                matrixNode.Nodes.Add(rowNode);
            }

            rootNode.Nodes.Add(matrixNode);

            if (ItemDesc.Collisions != null && ItemDesc.Collisions.Length > 0)
            {
                TreeNode collisionsNode = new TreeNode("Collisions");

                for (int i = 0; i < ItemDesc.Collisions.Length; i++)
                {
                    var collision = ItemDesc.Collisions[i];
                    TreeNode collisionNode = new TreeNode($"Collision [{i}] - {collision.GetType().Name}");
                    collisionNode.Tag = collision;

                    if (collision is CollisionBox box)
                    {
                        TreeNode extentsNode = new TreeNode($"Extents: X={box.Extents.X:F6}, Y={box.Extents.Y:F6}, Z={box.Extents.Z:F6}");
                        extentsNode.Tag = box.Extents;
                        collisionNode.Nodes.Add(extentsNode);

                        TreeNode sizeNode = new TreeNode($"Size: X={box.Size.X:F6}, Y={box.Size.Y:F6}, Z={box.Size.Z:F6}");
                        sizeNode.Tag = box.Size;
                        collisionNode.Nodes.Add(sizeNode);
                    }
                    else if (collision is CollisionSphere sphere)
                    {
                        TreeNode radiusNode = new TreeNode($"Radius: {sphere.Radius:F6}");
                        radiusNode.Tag = sphere.Radius;
                        collisionNode.Nodes.Add(radiusNode);

                        TreeNode diameterNode = new TreeNode($"Diameter: {sphere.Diameter:F6}");
                        diameterNode.Tag = sphere.Diameter;
                        collisionNode.Nodes.Add(diameterNode);
                    }
                    else if (collision is CollisionCapsule capsule)
                    {
                        TreeNode radiusNode = new TreeNode($"Radius: {capsule.Radius:F6}");
                        radiusNode.Tag = capsule.Radius;
                        collisionNode.Nodes.Add(radiusNode);

                        TreeNode halfHeightNode = new TreeNode($"Half Height: {capsule.HalfHeight:F6}");
                        halfHeightNode.Tag = capsule.HalfHeight;
                        collisionNode.Nodes.Add(halfHeightNode);
                    }
                    else if (collision is CollisionConvex convex)
                    {
                        TreeNode verticesNode = new TreeNode($"Vertices: {convex.Vertices?.Count ?? 0}");
                        verticesNode.Tag = convex.Vertices;
                        collisionNode.Nodes.Add(verticesNode);

                        TreeNode hullCenterNode = new TreeNode($"Hull Center: X={convex.HullCenter.X:F6}, Y={convex.HullCenter.Y:F6}, Z={convex.HullCenter.Z:F6}");
                        hullCenterNode.Tag = convex.HullCenter;
                        collisionNode.Nodes.Add(hullCenterNode);

                        if (convex.Min != null && convex.Max != null)
                        {
                            TreeNode bboxNode = new TreeNode($"Bounding Box");
                            bboxNode.Nodes.Add($"Min: X={convex.Min.X:F6}, Y={convex.Min.Y:F6}, Z={convex.Min.Z:F6}");
                            bboxNode.Nodes.Add($"Max: X={convex.Max.X:F6}, Y={convex.Max.Y:F6}, Z={convex.Max.Z:F6}");
                            collisionNode.Nodes.Add(bboxNode);
                        }
                    }
                    collisionsNode.Nodes.Add(collisionNode);
                }
                rootNode.Nodes.Add(collisionsNode);
            }
            TreeView_Data.Nodes.Add(rootNode);
            rootNode.Expand();
        }

        private void TreeView_Data_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Data.SelectedObject = null;

            if (e.Node != null && e.Node.Tag != null)
            {
                PropertyGrid_Data.SelectedObject = e.Node.Tag;

                if (e.Node.Tag is Vector3 || e.Node.Tag is Vector4 || e.Node.Tag is float ||
                    e.Node.Tag is ulong || e.Node.Tag is byte || e.Node.Tag is short)
                {
                    PropertyGrid_Data.PropertySort = PropertySort.Alphabetical;
                }
                else
                {
                    PropertyGrid_Data.PropertySort = PropertySort.CategorizedAlphabetical;
                }
            }
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            try
            {
                if (!File.Exists(FullFilePath))
                {
                    MessageBox.Show($"File not found: {FullFilePath}\nPlease save to a different location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string backupPath = FullFilePath + "_backup";
                File.Copy(FullFilePath, backupPath, true);
                using (BinaryWriter writer = new BinaryWriter(File.Open(FullFilePath, FileMode.Create)))
                {
                    ItemDesc.WriteToFile(writer);
                }
                bIsFileEdited = false;
                Text = Path.GetFileName(FullFilePath) + " - ItemDesc Editor";
                MessageBox.Show($"File saved successfully to:\n{FullFilePath}\n\nBackup created: {Path.GetFileName(backupPath)}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ItemDescEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                DialogResult result = MessageBox.Show("You have unsaved changes. Do you want to save before closing?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}