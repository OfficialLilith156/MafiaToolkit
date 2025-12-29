using Core.IO;
using ResourceTypes.Cutscene;
using ResourceTypes.Cutscene.AnimEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utils.Extensions;
using Utils.Language;
using Utils.Logging;
using static ResourceTypes.Cutscene.CutsceneLoader;
using static ResourceTypes.Cutscene.CutsceneLoader.Cutscene;

namespace Mafia2Tool.Forms
{
    public partial class CutsceneEditor : Form
    {
        private Dictionary<string, CutsceneEntityNames> entityNamesByCutscene = new Dictionary<string, CutsceneEntityNames>();
        // File access. We should not directly edit cutscene from here.
        private FileCutscene OriginalFile;
        private CutsceneLoader.Cutscene[] Cutscenes;
        private CutsceneLoader.GCRData[] VehicleData;
        private bool bIsFileEdited = false;
        private bool isRightMouseButtonDown = false;
        private Dictionary<TreeNode, (string cutsceneName, int entityIndex, bool isSound)> nodeInfoMap =
        new Dictionary<TreeNode, (string, int, bool)>();
       
        public CutsceneEditor(FileCutscene CutsceneFile)
        {
            InitializeComponent();
            OriginalFile = CutsceneFile;
            Localise();
            BuildData();
            TreeView_Cutscene.LabelEdit = true;
            TreeView_Cutscene.DoubleClick += TreeView_Cutscene_DoubleClick;
            TreeView_Cutscene.AfterLabelEdit += TreeView_Cutscene_AfterLabelEdit;
            TreeView_Cutscene.MouseDown += TreeView_Cutscene_MouseDown;
            TreeView_Cutscene.BeforeLabelEdit += TreeView_Cutscene_BeforeLabelEdit;
            TreeView_Cutscene.ShowNodeToolTips = true;
        }

        private void TreeView_Cutscene_DoubleClick(object sender, EventArgs e)
        {
            isRightMouseButtonDown = false;
            if (TreeView_Cutscene.SelectedNode != null && TreeView_Cutscene.SelectedNode.Tag is AnimEntityWrapper)
            {
                TreeView_Cutscene.SelectedNode.BeginEdit();
            }
        }

        private bool EnsureNotEditing()
        {
            if (TreeView_Cutscene.SelectedNode != null && TreeView_Cutscene.SelectedNode.IsEditing)
            {
                TreeView_Cutscene.SelectedNode.EndEdit(false);
                return false;
            }
            return true;
        }

        private void TreeView_Cutscene_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isRightMouseButtonDown = true;

                TreeNode node = TreeView_Cutscene.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    TreeView_Cutscene.SelectedNode = node;
                }

                return;
            }
        }
        
        private void TreeView_Cutscene_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (isRightMouseButtonDown)
            {
                e.CancelEdit = true;
                return;
            }
            if (!(e.Node.Tag is AnimEntityWrapper))
            {
                e.CancelEdit = true;
                return;
            }
            if (e.Node.Tag is GCSData || e.Node.Tag is SPDData ||
                e.Node.Tag is Cutscene || e.Node.Tag is GCRData)
            {
                e.CancelEdit = true;
            }
        }

        private CutsceneEntityNames GetEntityNamesForCutscene(string cutsceneName)
        {
            if (!entityNamesByCutscene.ContainsKey(cutsceneName))
            {
                entityNamesByCutscene[cutsceneName] = CutsceneEntityNames.Load(cutsceneName);
            }

            return entityNamesByCutscene[cutsceneName];
        }

        private string GetDefaultEntityName(AnimEntityWrapper entity, int index, AnimEntityTypes entityType)
        {
            string typeName = entityType.ToString().Replace("Ae", "");

            string entityName = "";
            try
            {
                var nameProperty = entity.GetType().GetProperty("Name");
                if (nameProperty != null)
                {
                    var nameValue = nameProperty.GetValue(entity);
                    if (nameValue != null && !string.IsNullOrEmpty(nameValue.ToString()))
                    {
                        entityName = nameValue.ToString();
                    }
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(entityName))
            {
                return $"{typeName}: {entityName}";
            }

            return $"{typeName}_{index}";
        }

        public void Localise()
        {
            Text = Language.GetString("$CUTSCENE_EDITOR"); 
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            ContextMenu_Import.Text = Language.GetString("$CUTSCENE_IMPORT_ENTITY");
            ContextMenu_Export.Text = Language.GetString("$CUTSCENE_EXPORT_ENTITY");
            ContextMenu_Duplicate.Text = Language.GetString("$CUTSCENE_DUPLICATE_ENTITY");
            ContextMenu_Delete.Text = Language.GetString("$CUTSCENE_DELETE_ENTITY");
            Button_Edit.Text = Language.GetString("$EDIT");
        }

        private void AddCutsceneToTreeView(CutsceneLoader.Cutscene Cutscene)
        {
            TreeNode CutsceneParent = new TreeNode(Cutscene.CutsceneName);
            CutsceneParent.Tag = Cutscene;

            if (Cutscene.AssetContent != null)
            {
                var Assets = Cutscene.AssetContent;
                TreeNode AssetsParent = new TreeNode("Game Cutscene Content: (GCS Data)");
                AssetsParent.Tag = Assets;

                for (int i = 0; i < Assets.entities.Length; i++)
                {
                    var Asset = Assets.entities[i];
                    AnimEntityTypes entityType = Asset.GetEntityType();

                    var entityNames = GetEntityNamesForCutscene(Cutscene.CutsceneName);

                    string defaultName = GetDefaultEntityName(Asset, i, entityType);
                    string displayName = entityNames.GetDisplayName(i, entityType, defaultName);

                    TreeNode AssetNode = new TreeNode(displayName)
                    {
                        ToolTipText = $"Index: {i}, Type: {entityType}, Original: {defaultName}"
                    };
                    AssetNode.Tag = Asset;

                    nodeInfoMap[AssetNode] = (Cutscene.CutsceneName, i, false);

                    AssetsParent.Nodes.Add(AssetNode);
                }
                CutsceneParent.Nodes.Add(AssetsParent);
            }

            if (Cutscene.SoundContent != null)
            {
                var Assets = Cutscene.SoundContent;
                TreeNode AssetsParent = new TreeNode("Sound Content: (SPD Data)");
                AssetsParent.Tag = Assets;

                for (int i = 0; i < Assets.EntityDefinitions.Length; i++)
                {
                    var Asset = Assets.EntityDefinitions[i];
                    AnimEntityTypes entityType = Asset.GetEntityType();

                    var entityNames = GetEntityNamesForCutscene(Cutscene.CutsceneName);

                    string defaultName = GetDefaultEntityName(Asset, i, entityType);
                    string displayName = entityNames.GetDisplayName(i, entityType, defaultName);

                    TreeNode AssetNode = new TreeNode(displayName)
                    {
                        ToolTipText = $"Index: {i}, Type: {entityType}, Original: {defaultName}"
                    };
                    AssetNode.Tag = Asset;

                    nodeInfoMap[AssetNode] = (Cutscene.CutsceneName, i, true);

                    AssetsParent.Nodes.Add(AssetNode);
                }
                CutsceneParent.Nodes.Add(AssetsParent);
            }

            TreeView_Cutscene.Nodes.Add(CutsceneParent);
        }

        private void TreeView_Cutscene_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null && nodeInfoMap.ContainsKey(e.Node))
            {
                var info = nodeInfoMap[e.Node];

                var entityNames = GetEntityNamesForCutscene(info.cutsceneName);
                entityNames.SetDisplayName(info.entityIndex, e.Label, info.cutsceneName);

                e.Node.ToolTipText = $"Index: {info.entityIndex}, Custom Name: {e.Label}";

                Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
                bIsFileEdited = true;
            }
            else if (e.Label == null)
            {
                e.CancelEdit = true;
            }
        }
        private void ResetEntityNameToDefault(TreeNode node)
        {
            if (nodeInfoMap.ContainsKey(node))
            {
                var info = nodeInfoMap[node];
                var entityNames = GetEntityNamesForCutscene(info.cutsceneName);
                entityNames.RemoveDisplayName(info.entityIndex, info.cutsceneName);
                Reload();
            }
        }

        public void BuildData()
        {
            nodeInfoMap.Clear();

            Cutscenes = OriginalFile.GetCutsceneLoader().Cutscenes;
            for (int i = 0; i < Cutscenes.Length; i++)
            {
                AddCutsceneToTreeView(Cutscenes[i]);
            }
            VehicleData = OriginalFile.GetCutsceneLoader().VehicleContent;
            TreeNode GCRParent = new TreeNode("Vehicle Content: (GCR Data)");
            for (int i = 0; i < VehicleData.Length; i++)
            {
                TreeNode GCR = new TreeNode(VehicleData[i].Name);
                GCR.Tag = VehicleData[i];
                GCRParent.Nodes.Add(GCR);
            }
            TreeView_Cutscene.Nodes.Add(GCRParent);
        }

        private void Save()
        {
            CutsceneLoader Loader = OriginalFile.GetCutsceneLoader();
            Loader.WriteToFile(OriginalFile.GetUnderlyingFileInfo().FullName);
            Text = Language.GetString("$CUTSCENE_EDITOR");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_Cutscene.SelectedObject = null;
            TreeView_Cutscene.SelectedNode = null;
            TreeView_Cutscene.Nodes.Clear();
            BuildData();
            Text = Language.GetString("$CUTSCENE_EDITOR");
            bIsFileEdited = false;
        }

        private void TreeView_Cutscene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Cutscene.SelectedObject = e.Node.Tag;
        }

        private void PropertyGrid_Cutscene_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "CutsceneName") TreeView_Cutscene.SelectedNode.Text = e.ChangedItem.Value.ToString();
            Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void CutsceneEditor_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                PropertyGrid_Cutscene.SelectedObject = null;
                TreeView_Cutscene.SelectedNode = null;
            }
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();
        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();
        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();

        private void CutsceneEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);
                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void TreeViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenu_Import.Enabled = false;
            ContextMenu_Export.Enabled = false;
            ContextMenu_Duplicate.Enabled = false;
            if (TreeView_Cutscene.SelectedNode.Tag is AnimEntityWrapper)
            {
                ContextMenu_Import.Enabled = true;
                ContextMenu_Export.Enabled = true;
                ContextMenu_Duplicate.Enabled = true;
            }
            else if (TreeView_Cutscene.SelectedNode.Tag is GCSData || TreeView_Cutscene.SelectedNode.Tag is SPDData)
            {
                ContextMenu_Import.Enabled = true;
                ContextMenu_Export.Enabled = false;
                ContextMenu_Duplicate.Enabled = false;
            }
            bool canRename = TreeView_Cutscene.SelectedNode != null &&
                        nodeInfoMap.ContainsKey(TreeView_Cutscene.SelectedNode);
            if (canRename)
            {
                TreeView_Cutscene.SelectedNode.BeginEdit();
            }
            bool canReset = TreeView_Cutscene.SelectedNode != null &&
                      nodeInfoMap.ContainsKey(TreeView_Cutscene.SelectedNode);
        }

        private void ContextMenu_Duplicate_Click(object sender, EventArgs e)
        {
            if (!(TreeView_Cutscene.SelectedNode.Tag is AnimEntityWrapper entity))
                return;

            if (!nodeInfoMap.ContainsKey(TreeView_Cutscene.SelectedNode))
                return;

            var originalInfo = nodeInfoMap[TreeView_Cutscene.SelectedNode];
            int newIndex = originalInfo.entityIndex + 1;

            var entityNames = GetEntityNamesForCutscene(originalInfo.cutsceneName);
            entityNames.InsertDisplayName(originalInfo.entityIndex, newIndex, originalInfo.cutsceneName);

            AnimEntityWrapper newEntity;
            byte[] entityData = new byte[0];
            byte[] animEntityData = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                // Write Entity to the Stream
                CutsceneEntityFactory.WriteAnimEntityToFile(stream, entity);
                entityData = stream.ToArray();
            }
            using (MemoryStream EntityStream = new MemoryStream())
            {
                bool isBigEndian = false;
                EntityStream.Write(entity.AnimEntityData.DataType, isBigEndian);
                EntityStream.Write(0, isBigEndian);
                entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);

                animEntityData = EntityStream.ToArray();
            }
            using (MemoryStream Reader = new MemoryStream(entityData))
            {
                newEntity = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(entity.GetEntityType(), Reader);
            }
            using (MemoryStream stream = new MemoryStream(animEntityData))
            {
                newEntity.AnimEntityData.ReadFromFile(stream, false);
            }
            var cutscenes = OriginalFile.GetCutsceneLoader().Cutscenes;
            for (int i = 0; i < cutscenes.Length; i++)
            {
                var cutscene = cutscenes[i];
                if (cutscene.AssetContent.entities.Contains(entity))
                {
                    var list = cutscene.AssetContent.entities.ToList();
                    var index = list.IndexOf(entity);
                    list.Insert(index + 1, newEntity);
                    cutscene.AssetContent.entities = list.ToArray();
                    Reload();
                    TreeView_Cutscene.Nodes[i].Nodes[0].Expand();
                    TreeView_Cutscene.SelectedNode = TreeView_Cutscene.Nodes[i].Nodes[0].Nodes[index];
                    Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
                    bIsFileEdited = true;
                    return;
                }
                else if (cutscene.SoundContent.EntityDefinitions.Contains(entity))
                {
                    var list = cutscene.SoundContent.EntityDefinitions.ToList();
                    var index = list.IndexOf(entity);
                    list.Insert(index + 1, newEntity);
                    cutscene.SoundContent.EntityDefinitions = list.ToArray();
                    Reload();
                    TreeView_Cutscene.Nodes[i].Nodes[1].Expand();
                    TreeView_Cutscene.SelectedNode = TreeView_Cutscene.Nodes[i].Nodes[1].Nodes[index];
                    Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
                    bIsFileEdited = true;
                    return;
                }
                Reload();

                if (TreeView_Cutscene.SelectedNode != null)
                {
                    TreeView_Cutscene.SelectedNode.BeginEdit();
                }
            }
        }
        private void ContextMenu_ResetName_Click(object sender, EventArgs e)
        {
            if (TreeView_Cutscene.SelectedNode != null)
            {
                ResetEntityNameToDefault(TreeView_Cutscene.SelectedNode);
            }
        }

        private void ContextMenu_Import_Click(object sender, EventArgs e)
        {
            GCSData gcsData = null;
            SPDData spdData = null;
            TreeNode gcsNode = null;
            TreeNode spdNode = null;
            if (TreeView_Cutscene.SelectedNode.Tag is GCSData)
            {
                gcsData = (GCSData)TreeView_Cutscene.SelectedNode.Tag;
                gcsNode = TreeView_Cutscene.SelectedNode;
            }
            else if (TreeView_Cutscene.SelectedNode.Tag is SPDData)
            {
                spdData = (SPDData)TreeView_Cutscene.SelectedNode.Tag;
                spdNode = TreeView_Cutscene.SelectedNode;
            }
            else if (TreeView_Cutscene.SelectedNode.Tag is AnimEntityWrapper)
            {
                if (TreeView_Cutscene.SelectedNode.Parent.Tag is GCSData)
                {
                    gcsData = (GCSData)TreeView_Cutscene.SelectedNode.Parent.Tag;
                    gcsNode = TreeView_Cutscene.SelectedNode.Parent;
                }
                else if (TreeView_Cutscene.SelectedNode.Parent.Tag is SPDData)
                {
                    spdData = (SPDData)TreeView_Cutscene.SelectedNode.Parent.Tag;
                    spdNode = TreeView_Cutscene.SelectedNode.Parent;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            OpenFileDialog openFile = new();
            openFile.InitialDirectory = OriginalFile.GetUnderlyingFileInfo().DirectoryName;
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Title = "Import Cutscene entity data";
            openFile.Filter = "Cutscene entity data|*.CutEntityData";
            openFile.FileName = "Open file";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                AnimEntityWrapper EntityWrapper = null;
                using (MemoryStream ms = new(File.ReadAllBytes(openFile.FileName)))
                {
                    int Size = ms.ReadInt32(false);
                    AnimEntityTypes AnimEntityType = (AnimEntityTypes)ms.ReadInt32(false);
                    using (MemoryStream Reader = new(ms.ReadBytes(Size - 4)))
                    {
                        EntityWrapper = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(AnimEntityType, Reader);
                    }
                    if (EntityWrapper == null)
                    {
                        return;
                    }
                    Size = ms.ReadInt32(false);
                    using (MemoryStream stream = new(ms.ReadBytes(Size)))
                    {
                        EntityWrapper.AnimEntityData.ReadFromFile(stream, false);
                    }
                }
                if (gcsData != null)
                {
                    var entities = gcsData.entities.ToList();
                    entities.Add(EntityWrapper);
                    gcsData.entities = entities.ToArray();
                    var Asset = gcsData.entities[^1];
                    TreeNode AssetNode = new TreeNode(string.Format("{0}: {1}", Asset.GetType().Name, gcsData.entities.Length - 1));
                    AssetNode.Tag = Asset;
                    gcsNode.Nodes.Add(AssetNode);
                    TreeView_Cutscene.SelectedNode = AssetNode;
                }
                else if (spdData != null)
                {
                    var entities = spdData.EntityDefinitions.ToList();
                    entities.Add(EntityWrapper);
                    spdData.EntityDefinitions = entities.ToArray();
                    var Asset = spdData.EntityDefinitions[^1];
                    TreeNode AssetNode = new TreeNode(string.Format("{0}: {1}", Asset.GetType().Name, spdData.EntityDefinitions.Length - 1));
                    AssetNode.Tag = Asset;
                    spdNode.Nodes.Add(AssetNode);
                    TreeView_Cutscene.SelectedNode = AssetNode;
                }
            }
            Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void ContextMenu_Export_Click(object sender, EventArgs e)
        {
            AnimEntityWrapper entity = (AnimEntityWrapper)TreeView_Cutscene.SelectedNode.Tag;
            string name = TreeView_Cutscene.SelectedNode.Text.Replace(": ", "_");
            SaveFileDialog saveFile = new();
            saveFile.InitialDirectory = OriginalFile.GetUnderlyingFileInfo().DirectoryName;
            saveFile.CheckFileExists = false;
            saveFile.CheckPathExists = true;
            saveFile.Title = "Export Cutscene entity data";
            saveFile.Filter = "Cutscene entity data|*.CutEntityData";
            saveFile.FileName = TreeView_Cutscene.SelectedNode.Parent.Parent.Text + "_" + name + ".CutEntityData";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                byte[] entityData = new byte[0];
                byte[] animEntityData = new byte[0];
                byte[] data = new byte[0];
                using (MemoryStream stream = new())
                {
                    stream.Write((int)entity.GetEntityType(), false);
                    CutsceneEntityFactory.WriteAnimEntityToFile(stream, entity);
                    entityData = stream.ToArray();
                }
                using (MemoryStream EntityStream = new())
                {
                    bool isBigEndian = false;
                    entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);
                    animEntityData = EntityStream.ToArray();
                }
                using (MemoryStream dataStream = new())
                {
                    dataStream.Write(entityData.Length, false);
                    dataStream.Write(entityData);
                    dataStream.Write(animEntityData.Length + 8, false);
                    dataStream.Write(entity.AnimEntityData.DataType, false);
                    dataStream.Write(animEntityData.Length + 8, false);
                    dataStream.Write(animEntityData);
                    data = dataStream.ToArray();
                }
                File.WriteAllBytes(saveFile.FileName, data);
            }
        }

        private void ContextMenu_Delete_Click(object sender, EventArgs e)
        {
            if (!EnsureNotEditing())
            {
                MessageBox.Show("Please finish editing before deleting.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!(TreeView_Cutscene.SelectedNode?.Tag is AnimEntityWrapper))
            {
                MessageBox.Show("Please select an entity to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GCSData gcsData = null;
            SPDData spdData = null;
            TreeNode gcsNode = null;
            TreeNode spdNode = null;
            if (TreeView_Cutscene.SelectedNode.Tag is AnimEntityWrapper)
            {
                if (TreeView_Cutscene.SelectedNode.Parent.Tag is GCSData)
                {
                    gcsData = (GCSData)TreeView_Cutscene.SelectedNode.Parent.Tag;
                    gcsNode = TreeView_Cutscene.SelectedNode.Parent;
                }
                else if (TreeView_Cutscene.SelectedNode.Parent.Tag is SPDData)
                {
                    spdData = (SPDData)TreeView_Cutscene.SelectedNode.Parent.Tag;
                    spdNode = TreeView_Cutscene.SelectedNode.Parent;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            var entityNode = TreeView_Cutscene.SelectedNode;
            AnimEntityWrapper entity = (AnimEntityWrapper)entityNode.Tag;

            if (gcsData != null)
            {
                var entities = gcsData.entities.ToList();
                entities.Remove(entity);
                gcsData.entities = entities.ToArray();
                gcsNode.Nodes.Remove(entityNode);

                if (nodeInfoMap.ContainsKey(entityNode))
                {
                    var info = nodeInfoMap[entityNode];
                    var entityNames = GetEntityNamesForCutscene(info.cutsceneName);
                    entityNames.ReindexAfterDeletion(info.entityIndex, info.cutsceneName);
                    nodeInfoMap.Remove(entityNode);
                }
            }
            else if (spdData != null)
            {
                var entities = spdData.EntityDefinitions.ToList();
                entities.Remove(entity);
                spdData.EntityDefinitions = entities.ToArray();
                spdNode.Nodes.Remove(entityNode);

                if (nodeInfoMap.ContainsKey(entityNode))
                {
                    var info = nodeInfoMap[entityNode];
                    var entityNames = GetEntityNamesForCutscene(info.cutsceneName);
                    entityNames.ReindexAfterDeletion(info.entityIndex, info.cutsceneName);
                    nodeInfoMap.Remove(entityNode);
                }
            }
        }
    }
}
