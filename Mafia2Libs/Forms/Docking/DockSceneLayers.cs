using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Toolkit.Core;
using Utils.Language;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public class DockSceneLayers : DockContent
    {
        private SceneManager sceneManager;
        private ListView sceneListView;
        private ToolStrip toolStrip;
        private ToolStripButton addReferenceSceneButton;
        private ToolStripButton removeSceneButton;
        private ToolStripButton setActiveSceneButton;
        private ToolStripButton mergeeScenesButton;

        public event Action<string> OnSceneAdded;
        public event Action<string> OnSceneRemoved;
        public event Action<string> OnActiveSceneChanged;
        public event Action<string> OnSceneVisibilityChanged;

        public DockSceneLayers(SceneManager manager)
        {
            sceneManager = manager;
            InitializeControls();
            BuildSceneList();
            Localize();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();

            // ToolStrip
            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;

            addReferenceSceneButton = new ToolStripButton();
            addReferenceSceneButton.Text = "Add Reference Scene";
            addReferenceSceneButton.Click += LoadReferenceScene_Click;

            removeSceneButton = new ToolStripButton();
            removeSceneButton.Text = "Remove Scene";
            removeSceneButton.Click += RemoveScene_Click;

            setActiveSceneButton = new ToolStripButton();
            setActiveSceneButton.Text = "Set Active";
            setActiveSceneButton.Click += SetActiveScene_Click;

            mergeeScenesButton = new ToolStripButton();
            mergeeScenesButton.Text = "Merge Scenes";
            mergeeScenesButton.Click += MergeScenes_Click;

            toolStrip.Items.Add(addReferenceSceneButton);
            toolStrip.Items.Add(removeSceneButton);
            toolStrip.Items.Add(setActiveSceneButton);
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(mergeeScenesButton);

            // ListView
            sceneListView = new ListView();
            sceneListView.Dock = DockStyle.Fill;
            sceneListView.View = View.Details;
            sceneListView.FullRowSelect = true;
            sceneListView.CheckBoxes = true;
            sceneListView.MultiSelect = false;

            sceneListView.Columns.Add("Scene", 150);
            sceneListView.Columns.Add("Type", 80);
            sceneListView.Columns.Add("Active", 60);
            sceneListView.Columns.Add("Editable", 70);

            sceneListView.ItemChecked += SceneListView_ItemChecked;
            sceneListView.SelectedIndexChanged += SceneListView_SelectedIndexChanged;

            // Form properties
            this.Controls.Add(sceneListView);
            this.Controls.Add(toolStrip);
            this.Text = "Scene Layers";
            this.DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop |
                             DockAreas.DockBottom | DockAreas.Float | DockAreas.Document;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Localize()
        {
            Text = "Scene Layers"; // Could use Language.GetString() if we add translation keys
            addReferenceSceneButton.Text = "Add Reference Scene";
            removeSceneButton.Text = "Remove Scene";
            setActiveSceneButton.Text = "Set Active";
            mergeeScenesButton.Text = "Merge Scenes";
        }

        private void BuildSceneList()
        {
            sceneListView.Items.Clear();

            if (sceneManager == null || sceneManager.AllScenes == null)
                return;

            foreach (var scene in sceneManager.AllScenes)
            {
                AddSceneToList(scene);
            }
        }

        private void AddSceneToList(ManagedScene scene)
        {
            ListViewItem item = new ListViewItem(scene.SceneID.Substring(0, 8));
            item.Tag = scene.SceneID;
            item.Checked = scene.IsVisible;
            item.BackColor = scene.SceneTint;

            item.SubItems.Add(scene.Layer.ToString());
            item.SubItems.Add(scene.SceneID == sceneManager.ActiveScene?.SceneID ? "Yes" : "No");
            item.SubItems.Add(scene.IsEditable ? "Yes" : "No");

            sceneListView.Items.Add(item);
        }

        private void RefreshSceneList()
        {
            BuildSceneList();
        }

        private void SceneListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Tag is string sceneID)
            {
                var scene = sceneManager.GetScene(sceneID);
                if (scene != null)
                {
                    scene.IsVisible = e.Item.Checked;
                    OnSceneVisibilityChanged?.Invoke(sceneID);
                }
            }
        }

        private void SceneListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = sceneListView.SelectedItems.Count > 0;
            removeSceneButton.Enabled = hasSelection;
            setActiveSceneButton.Enabled = hasSelection;
        }

        private void LoadReferenceScene_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select reference scene folder";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var bgScene = new Mafia2Tool.SceneData();
                        bgScene.ScenePath = dialog.SelectedPath;
                        bgScene.BuildData(false);

                        string bgSceneID = sceneManager.AddScene(bgScene, SceneLayer.Reference);

                        // Setup scene contexts for textures and resources
                        Rendering.Graphics.TextureLoader.SetSceneContext(bgSceneID, bgScene.ScenePath);
                        Rendering.Graphics.RenderStorageSingleton.Instance.SetSceneContext(bgSceneID);

                        RefreshSceneList();
                        OnSceneAdded?.Invoke(bgSceneID);

                        MessageBox.Show($"Reference scene loaded successfully!\nScene ID: {bgSceneID.Substring(0, 8)}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load reference scene: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RemoveScene_Click(object sender, EventArgs e)
        {
            if (sceneListView.SelectedItems.Count == 0)
                return;

            string sceneID = sceneListView.SelectedItems[0].Tag as string;
            if (sceneID == null)
                return;

            var scene = sceneManager.GetScene(sceneID);
            if (scene == null)
                return;

            if (scene.Layer == SceneLayer.Primary)
            {
                MessageBox.Show("Cannot remove the primary scene.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Remove scene {sceneID.Substring(0, 8)}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                sceneManager.RemoveScene(sceneID);
                RefreshSceneList();
                OnSceneRemoved?.Invoke(sceneID);
            }
        }

        private void SetActiveScene_Click(object sender, EventArgs e)
        {
            if (sceneListView.SelectedItems.Count == 0)
                return;

            string sceneID = sceneListView.SelectedItems[0].Tag as string;
            if (sceneID == null)
                return;

            sceneManager.SetActiveScene(sceneID);
            RefreshSceneList();
            OnActiveSceneChanged?.Invoke(sceneID);

            MessageBox.Show($"Active scene changed to {sceneID.Substring(0, 8)}",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MergeScenes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Scene merging functionality will be implemented in Phase 5.",
                "Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
