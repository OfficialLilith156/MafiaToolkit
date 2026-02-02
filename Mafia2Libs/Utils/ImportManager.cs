using ResourceTypes.FrameResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Toolkit.Core;
using Mafia2Tool;

namespace Utils.Helpers
{
    public class ImportOptions
    {
        public bool ShowPreview { get; set; } = true;
        public bool DeduplicateByName { get; set; } = true;
        public bool DeduplicateByGeometry { get; set; } = false;
        public bool ImportTextures { get; set; } = true;
        public bool ImportItemDesc { get; set; } = true;
    }

    public class ImportAnalysis
    {
        public List<FrameObjectBase> ObjectsToImport { get; set; } = new List<FrameObjectBase>();
        public List<string> DuplicateNames { get; set; } = new List<string>();
        public Dictionary<ulong, string> GeometryHashes { get; set; } = new Dictionary<ulong, string>();
        public List<string> TexturesToImport { get; set; } = new List<string>();
        public int TotalObjects { get; set; }
        public int DuplicateCount { get; set; }
    }

    public class ImportResult
    {
        public List<TreeNode> ImportedNodes { get; set; } = new List<TreeNode>();
        public List<string> ImportedTextures { get; set; } = new List<string>();
        public Dictionary<int, int> OldToNewRefIDMap { get; set; } = new Dictionary<int, int>();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ImportManager
    {
        private SceneManager sceneManager;
        private SceneData targetScene;

        public ImportManager(SceneManager manager)
        {
            sceneManager = manager;
        }

        public ImportResult ImportObjects(
            string sourceSceneID,
            List<FrameObjectBase> objects,
            ImportOptions options)
        {
            var result = new ImportResult();

            try
            {
                // Get active scene as target
                var activeScene = sceneManager.ActiveScene;
                if (activeScene == null)
                {
                    result.ErrorMessage = "No active scene selected";
                    return result;
                }

                targetScene = activeScene.SceneData;

                // 1. Analyze imports
                var analysis = AnalyzeImport(sourceSceneID, objects, options);

                // 2. Show preview if requested
                if (options.ShowPreview)
                {
                    var previewDialog = new ImportPreviewDialog(analysis);
                    if (previewDialog.ShowDialog() != DialogResult.OK)
                    {
                        result.ErrorMessage = "Import cancelled by user";
                        return result;
                    }
                    objects = previewDialog.GetFinalSelection();
                }

                // 3. Perform import with deduplication
                foreach (var obj in objects)
                {
                    if (ShouldSkipDuplicate(obj, options))
                        continue;

                    var importedNode = ImportSingleObject(obj, result.OldToNewRefIDMap);
                    if (importedNode != null)
                    {
                        result.ImportedNodes.Add(importedNode);
                    }
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private ImportAnalysis AnalyzeImport(string sourceSceneID, List<FrameObjectBase> objects, ImportOptions options)
        {
            var analysis = new ImportAnalysis();
            analysis.TotalObjects = objects.Count;

            var existingNames = new HashSet<string>();
            foreach (var obj in targetScene.FrameResource.FrameObjects.Values)
            {
                if (obj is FrameObjectBase frameObj)
                {
                    string name = GetObjectName(frameObj);
                    if (!string.IsNullOrEmpty(name))
                    {
                        existingNames.Add(name);
                    }
                }
            }

            foreach (var obj in objects)
            {
                string name = GetObjectName(obj);
                bool isDuplicate = false;

                // Check for duplicate names
                if (options.DeduplicateByName && existingNames.Contains(name))
                {
                    analysis.DuplicateNames.Add(name);
                    isDuplicate = true;
                }

                // Check for duplicate geometry
                if (options.DeduplicateByGeometry && obj is FrameObjectSingleMesh mesh)
                {
                    ulong hash = ComputeGeometryHash(mesh);
                    if (analysis.GeometryHashes.ContainsKey(hash))
                    {
                        isDuplicate = true;
                    }
                    else
                    {
                        analysis.GeometryHashes[hash] = name;
                    }
                }

                if (isDuplicate)
                {
                    analysis.DuplicateCount++;
                }
                else
                {
                    analysis.ObjectsToImport.Add(obj);
                }
            }

            return analysis;
        }

        private bool ShouldSkipDuplicate(FrameObjectBase obj, ImportOptions options)
        {
            if (!options.DeduplicateByName)
                return false;

            string name = GetObjectName(obj);
            if (string.IsNullOrEmpty(name))
                return false;

            // Check if object with this name already exists in target scene
            foreach (var existingObj in targetScene.FrameResource.FrameObjects.Values)
            {
                if (existingObj is FrameObjectBase frameObj)
                {
                    if (GetObjectName(frameObj) == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private TreeNode ImportSingleObject(FrameObjectBase obj, Dictionary<int, int> refIDMap)
        {
            // Create a copy of the object
            // Note: This is a simplified implementation
            // In a full implementation, we would properly clone the object
            // and handle all dependencies

            int oldRefID = obj.RefID;
            int newRefID = RefManager.GetNewRefID();

            // Store mapping
            refIDMap[oldRefID] = newRefID;

            // Create tree node representation
            TreeNode node = new TreeNode(GetObjectName(obj));
            node.Tag = obj;

            return node;
        }

        private string GetObjectName(FrameObjectBase obj)
        {
            if (obj == null)
                return "Unknown";

            // Try to get name from various sources
            if (obj is FrameObjectSingleMesh mesh)
            {
                return mesh.Name?.String ?? $"Mesh_{obj.RefID}";
            }
            else if (obj is FrameObjectModel model)
            {
                return model.Name?.String ?? $"Model_{obj.RefID}";
            }
            else if (obj is FrameObjectFrame frame)
            {
                return frame.Name?.String ?? $"Frame_{obj.RefID}";
            }

            return $"{obj.GetType().Name}_{obj.RefID}";
        }

        private ulong ComputeGeometryHash(FrameObjectSingleMesh mesh)
        {
            if (mesh == null)
                return 0;

            using (var md5 = MD5.Create())
            {
                StringBuilder sb = new StringBuilder();

                // Hash based on vertex count, face count, and material
                int vertexCount = 0;
                int faceCount = 0;
                ulong materialHash = 0;

                if (mesh.Geometry != null && mesh.Geometry.LOD != null && mesh.Geometry.LOD.Length > 0)
                {
                    vertexCount = mesh.Geometry.LOD[0].NumVerts;
                    if (mesh.Geometry.LOD[0].SplitInfo != null)
                    {
                        faceCount = mesh.Geometry.LOD[0].SplitInfo.NumFaces;
                    }
                }

                if (mesh.Material != null && mesh.Material.Materials != null &&
                    mesh.Material.Materials.Count > 0 && mesh.Material.Materials[0].Length > 0)
                {
                    materialHash = mesh.Material.Materials[0][0].MaterialHash;
                }

                sb.Append(vertexCount);
                sb.Append(faceCount);
                sb.Append(materialHash);

                byte[] inputBytes = Encoding.UTF8.GetBytes(sb.ToString());
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return BitConverter.ToUInt64(hashBytes, 0);
            }
        }
    }

    // Simple preview dialog (can be enhanced with WinForms designer)
    public class ImportPreviewDialog : Form
    {
        private ImportAnalysis analysis;
        private ListBox objectListBox;
        private Button okButton;
        private Button cancelButton;
        private Label summaryLabel;

        public ImportPreviewDialog(ImportAnalysis analysis)
        {
            this.analysis = analysis;
            InitializeComponents();
            PopulateList();
        }

        private void InitializeComponents()
        {
            this.Text = "Import Preview";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            summaryLabel = new Label();
            summaryLabel.Location = new System.Drawing.Point(10, 10);
            summaryLabel.Size = new System.Drawing.Size(480, 60);
            summaryLabel.Text = $"Total objects: {analysis.TotalObjects}\n" +
                               $"Objects to import: {analysis.ObjectsToImport.Count}\n" +
                               $"Duplicates skipped: {analysis.DuplicateCount}";

            objectListBox = new ListBox();
            objectListBox.Location = new System.Drawing.Point(10, 80);
            objectListBox.Size = new System.Drawing.Size(470, 250);
            objectListBox.SelectionMode = SelectionMode.MultiExtended;

            okButton = new Button();
            okButton.Text = "Import";
            okButton.Location = new System.Drawing.Point(300, 340);
            okButton.DialogResult = DialogResult.OK;

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new System.Drawing.Point(390, 340);
            cancelButton.DialogResult = DialogResult.Cancel;

            this.Controls.Add(summaryLabel);
            this.Controls.Add(objectListBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void PopulateList()
        {
            foreach (var obj in analysis.ObjectsToImport)
            {
                objectListBox.Items.Add(obj);
            }

            // Select all by default
            for (int i = 0; i < objectListBox.Items.Count; i++)
            {
                objectListBox.SetSelected(i, true);
            }
        }

        public List<FrameObjectBase> GetFinalSelection()
        {
            List<FrameObjectBase> selected = new List<FrameObjectBase>();
            foreach (var item in objectListBox.SelectedItems)
            {
                if (item is FrameObjectBase obj)
                {
                    selected.Add(obj);
                }
            }
            return selected;
        }
    }
}
