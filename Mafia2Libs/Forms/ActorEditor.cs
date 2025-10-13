using Forms.EditorControls;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Utils.Helpers.Reflection;
using Utils.Language;
using Utils.Settings;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Mafia2Tool
{
    public partial class ActorEditor : Form
    {
        private FileInfo actorFile;
        private Actor actors;

        private TreeNode definitions;
        private TreeNode items;

        private static ActorExtraData globalClipboard;

        private bool bIsFileEdited = false;


        public ActorEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            actorFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Actor editor.");
            SearchBox.KeyDown += SearchBox_KeyDown;
        }

        private void Localise()
        {
            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            FileButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            EditButton.Text = Language.GetString("$EDIT");
            AddDefinitionButton.Text = Language.GetString("$ADD_DEFINITION");
            AddItemButton.Text = Language.GetString("$ADD_ITEM");
            ContextCopy.Text = Language.GetString("$COPY");
            ContextPaste.Text = Language.GetString("$PASTE");
            Button_MoveDown.Text = Language.GetString("$MOVE_DOWN");
            Button_MoveUp.Text = Language.GetString("$MOVE_UP");
            ContextDelete.Text = Language.GetString("$DELETE");
        }

        private void BuildData()
        {
            actors = new Actor(actorFile);
            definitions = new TreeNode("Definitions");
            items = new TreeNode("Entities");

            for (int i = 0; i != actors.Definitions.Count; i++)
            {
                TreeNode node = new TreeNode(actors.Definitions[i].Name);
                node.Name = actors.Definitions[i].FrameNameHash.ToString();
                node.Tag = actors.Definitions[i];
                definitions.Nodes.Add(node);
            }

            for (int i = 0; i != actors.Items.Count; i++)
            {
                TreeNode node = new TreeNode(actors.Items[i].EntityName);
                node.Tag = actors.Items[i];

                if (actors.Items[i].DataID != -1)
                {
                    TreeNode child = new TreeNode("Extra Data");
                    child.Tag = actors.ExtraData[actors.Items[i].DataID];
                    node.Nodes.Add(child);

                    if (Debugger.IsAttached)
                    {
                        string folder = "actors_unks/" + (ActorTypes)actors.Items[i].ActorTypeID + "/";
                        string filename = actors.Items[i].EntityName + ".dat";

                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        File.WriteAllBytes(Path.Combine(folder, filename), actors.Items[i].Data.GetDataInBytes());
                    }
                }

                items.Nodes.Add(node);
            }
            ActorTreeView.Nodes.Add(definitions);
            ActorTreeView.Nodes.Add(items);
        }

        private void Save()
        {
            File.Copy(actorFile.FullName, actorFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(actorFile.FullName, FileMode.Create)))
            {
                actors.WriteToFile(writer);
            }

            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            ActorTreeView.Nodes.Clear();
            BuildData();

            ActorGrid.SelectedObject = null;
            ActorTreeView.SelectedNode = null;

            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Copy()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode != null && SelectedNode.Text.Equals("Extra Data"))
            {
                ActorExtraData ExtraData = (SelectedNode.Tag as ActorExtraData);
                if (ExtraData != null && ExtraData.Data != null)
                {
                    globalClipboard = ExtraData;
                }
                else
                {
                    MessageBox.Show("Cannot copy: Selected extra data is empty.");
                }
            }
        }

        private void Paste()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (globalClipboard == null || SelectedNode == null || !SelectedNode.Text.Equals("Extra Data"))
            {
                return;
            }

            ActorExtraData ExtraDataToPaste = globalClipboard;
            ActorExtraData ExtraDataTarget = (SelectedNode.Tag as ActorExtraData);

            if (ExtraDataToPaste == null || ExtraDataTarget == null)
            {
                MessageBox.Show("Cannot paste: Source or target data is null.");
                return;
            }

            if (ExtraDataToPaste.BufferType != ExtraDataTarget.BufferType)
            {
                MessageBox.Show("Cannot paste: Buffer types do not match.");
                return;
            }

            if (ExtraDataToPaste.Data == null)
            {
                MessageBox.Show("Cannot paste: Source data is empty.");
                return;
            }

            try
            {
                Type dataType = ExtraDataToPaste.Data.GetType();
                object clonedData = null;

                var copyConstructor = dataType.GetConstructor(new Type[] { dataType });
                if (copyConstructor != null)
                {
                    clonedData = copyConstructor.Invoke(new object[] { ExtraDataToPaste.Data });
                }
                else
                {
                    clonedData = Activator.CreateInstance(dataType);
                    ReflectionHelpers.Copy(ExtraDataToPaste.Data, ref clonedData);
                }

                if (clonedData != null)
                {
                    ExtraDataTarget.Data = clonedData as IActorExtraDataInterface;
                    ActorGrid.SelectedObject = SelectedNode.Tag;
                    Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;
                }
                else
                {
                    MessageBox.Show("Cannot paste: Failed to create copy of data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during paste operation: {ex.Message}");
            }
        }

        private bool IsTypeofInterface(object ObjectToCheck, Type InterfaceType)
        {
            Type TypeOfObject = ObjectToCheck.GetType();
            return InterfaceType.IsAssignableFrom(TypeOfObject);
        }

        private void Delete()
        {
            object data = ActorTreeView.SelectedNode.Tag;
            bool isDeleted = false;
            if (data is ActorEntry)
            {
                actors.Items.Remove((ActorEntry)data);
                isDeleted = true;
            }
            else if (data is ActorDefinition)
            {
                actors.Definitions.Remove((ActorDefinition)data);
                isDeleted = true;
            }

            if (isDeleted)
            {
                ActorTreeView.Nodes.Remove(ActorTreeView.SelectedNode);

                Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            ActorGrid.SelectedObject = e.Node.Tag;
        }

        private void AddItemButton_Click(object sender, System.EventArgs e)
        {
            NewObjectForm objectForm = new NewObjectForm(true);
            objectForm.SetLabel("Entity Name");
            ActorItemAddOption optionControl = new ActorItemAddOption();
            objectForm.LoadOption(optionControl);

            if (objectForm.ShowDialog() == DialogResult.OK)
            {
                ActorTypes type = optionControl.GetSelectedType();
                string def = optionControl.GetDefinitionName();
                string framedef = optionControl.GetFrameName();
                ActorEntry entry = actors.CreateActorEntry(type, objectForm.GetInputText());
                entry.DefinitionName = def;
                entry.FrameName = framedef;

                TreeNode node = new TreeNode(entry.EntityName);
                node.Text = entry.EntityName;
                node.Tag = entry;

                if (entry.DataID != -1)
                {
                    TreeNode child = new TreeNode("Extra Data");
                    child.Tag = actors.ExtraData[entry.DataID];
                    node.Nodes.Add(child);
                }

                items.Nodes.Add(node);
            }

            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            objectForm.Dispose();
        }

        private void AddDefinitionButton_Click(object sender, System.EventArgs e)
        {
            ListWindowActor window = new ListWindowActor();
            window.PopulateForm(actors.Items);

            if (window.ShowDialog() == DialogResult.OK && window.chosenObjects.Count > 0)
            {
                foreach (var obj in window.chosenObjects)
                {
                    if (obj is ActorEntry selectedActor)
                    {
                        ActorDefinition definition = actors.CreateActorDefinition(selectedActor);
                        TreeNode node = new TreeNode(definition.Name);
                        node.Name = definition.FrameNameHash.ToString();
                        node.Tag = definition;
                        definitions.Nodes.Add(node);
                    }
                }

                Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void ActorTreeView_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                ActorGrid.SelectedObject = null;
                ActorTreeView.SelectedNode = null;
            }
            else if (e.Control && e.KeyCode == Keys.PageUp)
            {
                MoveItemUp();
            }
            else if (e.Control && e.KeyCode == Keys.PageDown)
            {
                MoveItemDown();
            }
        }

        private void ActorGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "EntityName")
                ActorTreeView.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            ActorGrid.Refresh();
        }

        private void ActorEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void ContextDelete_Click(object sender, System.EventArgs e) => Delete();
        private void SaveButton_OnClick(object sender, System.EventArgs e) => Save();
        private void ReloadButton_OnClick(object sender, System.EventArgs e) => Reload();
        private void ExitButton_OnClick(object sender, System.EventArgs e) => Close();
        private void ContextCopy_Click(object sender, System.EventArgs e) => Copy();
        private void ContextPaste_Click(object sender, System.EventArgs e) => Paste();
        private void Button_MoveUp_Clicked(object sender, EventArgs e) => MoveItemUp();
        private void Button_MoveDown_Clicked(object sender, EventArgs e) => MoveItemDown();
        private void ContextMenu_OnOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextCopy.Visible = false;
            ContextPaste.Visible = false;
            Button_MoveDown.Visible = false;
            Button_MoveUp.Visible = false;

            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode != null && SelectedNode.Tag != null)
            {
                if (SelectedNode.Text.Equals("Extra Data") || SelectedNode.Tag is ActorExtraData)
                {
                    ContextCopy.Visible = true;
                    ContextPaste.Visible = true;
                }

                // For now, Move Up/Down only active for ActorEntry.
                ActorEntry Item = (SelectedNode.Tag as ActorEntry);
                if (Item != null)
                {
                    Button_MoveDown.Visible = true;
                    Button_MoveUp.Visible = true;
                }
            }
        }

        private void MoveItemDown()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                return;
            }

            ActorEntry Item = (SelectedNode.Tag as ActorEntry);
            if (Item == null)
            {
                // Only works for ActorEntry for now
                return;
            }

            int Index = actors.Items.IndexOf(Item);
            int NextIndex = (actors.Items.Count != Index ? Index + 1 : -1);
            if (NextIndex == -1)
            {
                return;
            }

            // Can move down, start by swapping entires
            ActorEntry ItemBelow = actors.Items[NextIndex];
            actors.Items[Index] = ItemBelow;
            actors.Items[NextIndex] = Item;

            // Now move down in TreeView
            TreeNode ParentNode = SelectedNode.Parent;
            int NodeIndex = ParentNode.Nodes.IndexOf(SelectedNode);
            ParentNode.Nodes.RemoveAt(NodeIndex);
            ParentNode.Nodes.Insert(NodeIndex + 1, SelectedNode);
            ActorTreeView.SelectedNode = SelectedNode;

            // Update UI
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void MoveItemUp()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                return;
            }

            ActorEntry Item = (SelectedNode.Tag as ActorEntry);
            if (Item == null)
            {
                // Only works for ActorEntry for now
                return;
            }

            int Index = actors.Items.IndexOf(Item);
            int NextIndex = (Index != 0 ? Index - 1 : -1);
            if (NextIndex == -1)
            {
                return;
            }

            // Can move up, start by swapping entires
            ActorEntry ItemAbove = actors.Items[NextIndex];
            actors.Items[Index] = ItemAbove;
            actors.Items[NextIndex] = Item;

            // Now move up in TreeView
            TreeNode ParentNode = SelectedNode.Parent;
            int NodeIndex = ParentNode.Nodes.IndexOf(SelectedNode);
            ParentNode.Nodes.RemoveAt(NodeIndex);
            ParentNode.Nodes.Insert(NodeIndex - 1, SelectedNode);
            ActorTreeView.SelectedNode = SelectedNode;

            // Update UI
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                RunSearch(SearchBox.Text);
            }
        }

        private void RunSearch(string query)
        {
            query = query.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(query))
                return;

            TreeNode foundNode = FindNode(ActorTreeView.Nodes, query);

            if (foundNode != null)
            {
                ActorTreeView.SelectedNode = foundNode;
                ActorTreeView.Focus();
                foundNode.EnsureVisible();
            }
            else
            {
                //MessageBox.Show("Ничего не найдено.");
            }
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string query)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.ToLower().Contains(query))
                    return node;

                TreeNode child = FindNode(node.Nodes, query);
                if (child != null)
                    return child;
            }
            return null;
        }

        private void dUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = ActorTreeView.SelectedNode;
            if (selectedNode == null || !(selectedNode.Tag is ActorEntry originalEntry))
                return;

            ActorEntry clonedEntry = actors.CreateActorEntry(
                (ActorTypes)originalEntry.ActorTypeID,
                originalEntry.EntityName + ""
            );
            clonedEntry.DefinitionName = originalEntry.DefinitionName;
            clonedEntry.FrameName = originalEntry.FrameName;

            if (originalEntry.DataID != -1)
            {
                ActorExtraData emptyExtraData = new ActorExtraData()
                {
                    BufferType = actors.ExtraData[originalEntry.DataID].BufferType,
                    Data = null
                };

                clonedEntry.DataID = (short)actors.ExtraData.Count;
                actors.ExtraData.Add(emptyExtraData);
            }


            TreeNode node = new TreeNode(clonedEntry.EntityName);
            node.Tag = clonedEntry;

            if (clonedEntry.DataID != -1)
            {
                TreeNode child = new TreeNode("Extra Data");
                child.Tag = actors.ExtraData[clonedEntry.DataID];
                node.Nodes.Add(child);
            }

            items.Nodes.Add(node);
            ActorTreeView.SelectedNode = node;
            ActorTreeView.Focus();

            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            CopyExtraData(originalEntry, clonedEntry);
        }

        private void CopyExtraData(ActorEntry source, ActorEntry target)
        {
            if (source.DataID == -1 || target.DataID == -1)
                return;

            ActorExtraData sourceData = actors.ExtraData[source.DataID];
            ActorExtraData targetData = actors.ExtraData[target.DataID];

            if (sourceData?.Data == null)
                return;

            try
            {
                Type dataType = sourceData.Data.GetType();
                object clonedData = null;

                var copyConstructor = dataType.GetConstructor(new Type[] { dataType });
                if (copyConstructor != null)
                {
                    clonedData = copyConstructor.Invoke(new object[] { sourceData.Data });
                }
                else
                {
                    clonedData = Activator.CreateInstance(dataType);
                    ReflectionHelpers.Copy(sourceData.Data, ref clonedData);
                }

                targetData.Data = clonedData as IActorExtraDataInterface;

                ActorTreeView.SelectedNode = FindNodeByActorEntry(target);
                ActorGrid.SelectedObject = targetData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании ExtraData: {ex.Message}");
            }
        }

        private TreeNode FindNodeByActorEntry(ActorEntry entry)
        {
            foreach (TreeNode node in items.Nodes)
            {
                if (node.Tag == entry)
                    return node;
            }
            return null;
        }

        private void RenumberButton_Click(object sender, EventArgs e)
        {
            RenumberDataIDsByTreeOrder();
        }

        private void RenumberDataIDsByTreeOrder()
        {
            if (actors == null || items == null || items.Nodes.Count == 0)
            {
                //MessageBox.Show("Нет элементов для перенумерации.");
                return;
            }

            short newId = 0;

            foreach (TreeNode node in items.Nodes)
            {
                if (node?.Tag is ActorEntry entry)
                {
                    if (entry.DataID != -1 && entry.DataID < actors.ExtraData.Count)
                    {
                        entry.DataID = newId;
                        newId++;
                    }
                }
            }

            ActorGrid.Refresh();
            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            //MessageBox.Show($"DataID перенумерованы в порядке дерева: 0 .. {newId - 1}.", "Готово");
        }
        
    }
}
