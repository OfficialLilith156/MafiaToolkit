using Forms.EditorControls;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Utils.Helpers.Reflection;
using Utils.Language;
using Utils.Settings;
using XBOX.ActorFile;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Mafia2Tool
{
    public partial class ActorEditor : Form
    {
        private FileInfo actorFile;
        private Actor actors;
        private static ActorEntry branchClipboard;
        private TreeNode definitions;
        private TreeNode items;
        private static ActorExtraData globalClipboard;
        private bool bIsFileEdited = false;
        private string baseTitle;
        private static List<ActorEntry> allBranchClipboard;

        public ActorEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            actorFile = file;
            BuildData();
            UpdateTitle(false);
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

            Dictionary<ActorTypes, TreeNode> groups = new Dictionary<ActorTypes, TreeNode>();
            for (int i = 0; i < actors.Items.Count; i++)
            {
                ActorEntry entry = actors.Items[i];
                ActorTypes type = (ActorTypes)entry.ActorTypeID;
                if (!groups.ContainsKey(type))
                {
                    TreeNode groupNode = new TreeNode(type.ToString());
                    groupNode.Tag = type;
                    groups[type] = groupNode;
                    items.Nodes.Add(groupNode);
                }

                TreeNode node = new TreeNode(entry.EntityName);
                node.Tag = entry;
                if (entry.DataID != -1)
                {
                    TreeNode child = new TreeNode("Extra Data");
                    child.Tag = actors.ExtraData[entry.DataID];
                    node.Nodes.Add(child);
                }
                groups[type].Nodes.Add(node);
            }
            ActorTreeView.Nodes.Add(definitions);
            ActorTreeView.Nodes.Add(items);
        }
        private void UpdateTitle(bool edited)
        {
            string platform = actors?.IsBigEndian == true ? "Xbox" : "PC";
            string fileName = actorFile?.Name ?? "No Name";
            Text = $"{baseTitle} {platform} - {fileName}{(edited ? "*" : "")}";
        }

        private void Save()
        {
            File.Copy(actorFile.FullName, actorFile.FullName + "_old", true);
            using (EndianBinaryWriter writer = new EndianBinaryWriter(File.Open(actorFile.FullName, FileMode.Create), actors.IsBigEndian))
            {
                actors.WriteToFile(writer);
            }
            UpdateTitle(true);
            bIsFileEdited = false;
        }

        private void Reload()
        {
            ActorTreeView.Nodes.Clear();
            BuildData();
            ActorGrid.SelectedObject = null;
            ActorTreeView.SelectedNode = null;
            UpdateTitle(true);
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
                    UpdateTitle(true);
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
                UpdateTitle(true);
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
            UpdateTitle(true);
            bIsFileEdited = true;
            objectForm.Dispose();
        }

        private void CopyEntityBranch(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = ActorTreeView.SelectedNode;
            if (selectedNode == null || !(selectedNode.Tag is ActorEntry original)) return;

            ActorEntry cloned = actors.CreateActorEntry((ActorTypes)original.ActorTypeID, original.EntityName + "");
            cloned.DefinitionName = original.DefinitionName;
            cloned.FrameName = original.FrameName;
            cloned.Position = original.Position;
            cloned.Rotation = original.Rotation;
            cloned.bActivateOnInit = original.bActivateOnInit;
            cloned.Scale = original.Scale;
            cloned.UnkString = original.UnkString;
            cloned.Unk2String = original.Unk2String;
            if (original.DataID != -1 && original.Data != null)
            {
                cloned.Data = new ActorExtraData()
                {
                    BufferType = original.Data.BufferType
                };

                Type dataType = original.Data.Data.GetType();
                object clonedInternal = Activator.CreateInstance(dataType);
                ReflectionHelpers.Copy(original.Data.Data, ref clonedInternal);
                cloned.Data.Data = clonedInternal as IActorExtraDataInterface;
                branchClipboard = cloned;
            }
            else
            {
                branchClipboard = cloned;
            }
        }
        private void CopyAllBranches()
        {
            if (actors == null || actors.Items.Count == 0)
                return;

            allBranchClipboard = new List<ActorEntry>();
            var itemsCopy = actors.Items.ToList();

            foreach (var original in itemsCopy)
            {
                ActorEntry clone = CloneActorEntry(original);
                allBranchClipboard.Add(clone);
            }
        }
        private ActorEntry CloneActorEntry(ActorEntry original)
        {
            ActorEntry clone = new ActorEntry();
            clone.ActorTypeID = original.ActorTypeID;
            clone.EntityName = original.EntityName;
            clone.DefinitionName = original.DefinitionName;
            clone.FrameName = original.FrameName;
            clone.Position = original.Position;
            clone.Rotation = original.Rotation;
            clone.bActivateOnInit = original.bActivateOnInit;
            clone.Scale = original.Scale;
            clone.UnkString = original.UnkString;
            clone.Unk2String = original.Unk2String;

            if (original.Data != null)
            {
                clone.Data = new ActorExtraData()
                {
                    BufferType = original.Data.BufferType
                };

                Type dataType = original.Data.Data.GetType();
                object clonedInternal = Activator.CreateInstance(dataType);
                ReflectionHelpers.Copy(original.Data.Data, ref clonedInternal);
                clone.Data.Data = clonedInternal as IActorExtraDataInterface;
            }

            return clone;
        }

        private void PasteEntityBranch(object sender, System.EventArgs e)
        {
            if (branchClipboard == null) return;

            ActorEntry newEntry = actors.CreateActorEntry((ActorTypes)branchClipboard.ActorTypeID, branchClipboard.EntityName);
            newEntry.DefinitionName = branchClipboard.DefinitionName;
            newEntry.FrameName = branchClipboard.FrameName;
            newEntry.Position = branchClipboard.Position;
            newEntry.Rotation = branchClipboard.Rotation;
            newEntry.Scale = branchClipboard.Scale;
            newEntry.bActivateOnInit = branchClipboard.bActivateOnInit;
            newEntry.UnkString = branchClipboard.UnkString;
            newEntry.Unk2String = branchClipboard.Unk2String;
            if (branchClipboard.Data != null)
            {
                ActorExtraData newData = new ActorExtraData()
                {
                    BufferType = branchClipboard.Data.BufferType
                };
                object clonedInternal = Activator.CreateInstance(branchClipboard.Data.Data.GetType());
                ReflectionHelpers.Copy(branchClipboard.Data.Data, ref clonedInternal);
                newData.Data = clonedInternal as IActorExtraDataInterface;
                actors.ExtraData.Add(newData);
                newEntry.DataID = (short)(actors.ExtraData.Count - 1);
                newEntry.Data = newData;
            }
            TreeNode node = new TreeNode(newEntry.EntityName) { Tag = newEntry };

            if (newEntry.Data != null)
            {
                TreeNode child = new TreeNode("Extra Data") { Tag = newEntry.Data };
                node.Nodes.Add(child);
            }
            items.Nodes.Add(node);
            ActorTreeView.SelectedNode = node;
            UpdateTitle(true);
            bIsFileEdited = true;
        }

        private void PasteAllBranches()
        {
            if (allBranchClipboard == null || allBranchClipboard.Count == 0)
                return;

            foreach (var branch in allBranchClipboard)
            {
                ActorEntry newEntry = actors.CreateActorEntry((ActorTypes)branch.ActorTypeID, branch.EntityName);
                newEntry.DefinitionName = branch.DefinitionName;
                newEntry.FrameName = branch.FrameName;
                newEntry.Position = branch.Position;
                newEntry.Rotation = branch.Rotation;
                newEntry.Scale = branch.Scale;
                newEntry.bActivateOnInit = branch.bActivateOnInit;
                newEntry.UnkString = branch.UnkString;
                newEntry.Unk2String = branch.Unk2String;

                if (branch.Data != null)
                {
                    ActorExtraData newData = new ActorExtraData()
                    {
                        BufferType = branch.Data.BufferType
                    };
                    object clonedInternal = Activator.CreateInstance(branch.Data.Data.GetType());
                    ReflectionHelpers.Copy(branch.Data.Data, ref clonedInternal);
                    newData.Data = clonedInternal as IActorExtraDataInterface;

                    actors.ExtraData.Add(newData);
                    newEntry.DataID = (short)(actors.ExtraData.Count - 1);
                    newEntry.Data = newData;
                }

                TreeNode node = new TreeNode(newEntry.EntityName) { Tag = newEntry };
                if (newEntry.Data != null)
                {
                    TreeNode child = new TreeNode("Extra Data") { Tag = newEntry.Data };
                    node.Nodes.Add(child);
                }

                ActorTypes type = (ActorTypes)newEntry.ActorTypeID;
                TreeNode groupNode = null;
                foreach (TreeNode n in items.Nodes)
                {
                    if (n.Tag is ActorTypes t && t == type)
                    {
                        groupNode = n;
                        break;
                    }
                }
                if (groupNode == null)
                {
                    groupNode = new TreeNode(type.ToString()) { Tag = type };
                    items.Nodes.Add(groupNode);
                }
                groupNode.Nodes.Add(node);
            }

            UpdateTitle(true);
            bIsFileEdited = true;
            ActorTreeView.Refresh();
        }

        private void CopyAllBranches_Click(object sender, EventArgs e) => CopyAllBranches();
        private void PasteAllBranches_Click(object sender, EventArgs e) => PasteAllBranches();

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
                UpdateTitle(true);
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
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "EntityName") ActorTreeView.SelectedNode.Text = e.ChangedItem.Value.ToString();
            UpdateTitle(true);
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
                if (SelectedNode.Tag is ActorEntry || SelectedNode.Tag is ActorTypes)
                {
                    Button_MoveDown.Visible = true;
                    Button_MoveUp.Visible = true;
                }
            }
        }

        private void MoveItemDown()
        {
            TreeNode selectedNode = ActorTreeView.SelectedNode;
            if (selectedNode == null) return;

            if (selectedNode.Tag is ActorTypes)
            {
                int index = items.Nodes.IndexOf(selectedNode);
                if (index < 0 || index >= items.Nodes.Count - 1) return;

                items.Nodes.RemoveAt(index);
                items.Nodes.Insert(index + 1, selectedNode);
                ActorTreeView.SelectedNode = selectedNode;

                List<ActorEntry> newItems = new List<ActorEntry>();
                foreach (TreeNode groupNode in items.Nodes)
                {
                    ActorTypes type = (ActorTypes)groupNode.Tag;
                    newItems.AddRange(actors.Items.Where(entry => (ActorTypes)entry.ActorTypeID == type));
                }

                actors.Items.Clear();
                actors.Items.AddRange(newItems);

                UpdateTitle(true);
                bIsFileEdited = true;
                return;
            }
            else if (selectedNode.Tag is ActorEntry)
            {
                TreeNode parentGroup = selectedNode.Parent;
                if (parentGroup == null || !(parentGroup.Tag is ActorTypes))
                    return;

                int indexInGroup = parentGroup.Nodes.IndexOf(selectedNode);
                if (indexInGroup < 0 || indexInGroup >= parentGroup.Nodes.Count - 1)
                    return;

                parentGroup.Nodes.RemoveAt(indexInGroup);
                parentGroup.Nodes.Insert(indexInGroup + 1, selectedNode);
                ActorTreeView.SelectedNode = selectedNode;

                ReorderItemsFromTree();

                UpdateTitle(true);
                bIsFileEdited = true;
            }
        }
        private void ReorderItemsFromTree()
        {
            List<ActorEntry> newItems = new List<ActorEntry>();
            foreach (TreeNode groupNode in items.Nodes)
            {
                foreach (TreeNode entryNode in groupNode.Nodes)
                {
                    if (entryNode.Tag is ActorEntry entry)
                    {
                        newItems.Add(entry);
                    }
                }
            }
            actors.Items.Clear();
            actors.Items.AddRange(newItems);
        }

        private void MoveItemUp()
        {
            TreeNode selectedNode = ActorTreeView.SelectedNode;
            if (selectedNode == null) return;

            if (selectedNode.Tag is ActorTypes)
            {
                int index = items.Nodes.IndexOf(selectedNode);
                if (index <= 0) return;

                items.Nodes.RemoveAt(index);
                items.Nodes.Insert(index - 1, selectedNode);
                ActorTreeView.SelectedNode = selectedNode;

                List<ActorEntry> newItems = new List<ActorEntry>();
                foreach (TreeNode groupNode in items.Nodes)
                {
                    ActorTypes type = (ActorTypes)groupNode.Tag;
                    newItems.AddRange(actors.Items.Where(entry => (ActorTypes)entry.ActorTypeID == type));
                }

                actors.Items.Clear();
                actors.Items.AddRange(newItems);

                UpdateTitle(true);
                bIsFileEdited = true;
                return;
            }
            else if (selectedNode.Tag is ActorEntry)
            {
                TreeNode parentGroup = selectedNode.Parent;
                if (parentGroup == null || !(parentGroup.Tag is ActorTypes))
                    return;

                int indexInGroup = parentGroup.Nodes.IndexOf(selectedNode);
                if (indexInGroup <= 0) return;

                parentGroup.Nodes.RemoveAt(indexInGroup);
                parentGroup.Nodes.Insert(indexInGroup - 1, selectedNode);
                ActorTreeView.SelectedNode = selectedNode;

                ReorderItemsFromTree();

                UpdateTitle(true);
                bIsFileEdited = true;
            }
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
            if (string.IsNullOrWhiteSpace(query)) return;

            TreeNode foundNode = FindNode(items.Nodes, query);

            if (foundNode != null)
            {
                ActorTreeView.SelectedNode = foundNode;
                ActorTreeView.Focus();
                foundNode.EnsureVisible();
            }
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string query)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.ToLower().Contains(query)) return node;
                TreeNode child = FindNode(node.Nodes, query);
                if (child != null) return child;
            }
            return null;
        }

        private object CloneObjectSafely(object src)
        {
            if (src == null) return null;
            Type t = src.GetType();
            if (t.IsPrimitive || t == typeof(string) || t.IsEnum || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(Guid))
            {
                return src;
            }
            if (src is ICloneable clonable)
            {
                try
                {
                    return clonable.Clone();
                }
                catch { }
            }
            if (t.IsArray)
            {
                Array arr = (Array)src;
                Type elemType = t.GetElementType();
                Array cloneArr = Array.CreateInstance(elemType, arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    cloneArr.SetValue(CloneObjectSafely(arr.GetValue(i)), i);
                }
                return cloneArr;
            }
            if (t.IsValueType)
            {
                try
                {
                    object valCopy = Activator.CreateInstance(t);
                    ReflectionHelpers.Copy(src, ref valCopy);
                    return valCopy;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CloneObjectSafely: cannot Activator.CreateInstance value type {t.FullName}: {ex.Message}");
                    return src;
                }
            }
            object instance = null;
            try
            {
                if (t != typeof(string))
                {
                    var ctor = t.GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                    {
                        instance = Activator.CreateInstance(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CloneObjectSafely: Activator.CreateInstance failed for {t.FullName}: {ex.Message}");
                instance = null;
            }
            if (instance == null)
            {
                try
                {
                    if (t != typeof(string))
                    {
                        instance = FormatterServices.GetUninitializedObject(t);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CloneObjectSafely: FormatterServices.GetUninitializedObject failed for {t.FullName}: {ex.Message}");
                    instance = null;
                }
            }
            if (instance != null)
            {
                try
                {
                    ReflectionHelpers.Copy(src, ref instance);
                    return instance;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CloneObjectSafely: ReflectionHelpers.Copy failed for {t.FullName}: {ex.Message}");
                    return src;
                }
            }
            Debug.WriteLine($"CloneObjectSafely: Unable to create clone for type {t.FullName}. Returning original reference as fallback.");
            return src;
        }

        private void dUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = ActorTreeView.SelectedNode;
            if (selectedNode == null || !(selectedNode.Tag is ActorEntry original)) return;

            ActorEntry clone = actors.CreateActorEntry((ActorTypes)original.ActorTypeID, original.EntityName + "");

            clone.DefinitionName = original.DefinitionName;
            clone.FrameName = original.FrameName;
            if (original.Data != null && original.DataID != -1)
            {
                ActorExtraData newData = new ActorExtraData
                {
                    BufferType = original.Data.BufferType
                };
                try
                {
                    object internalCopy = CloneObjectSafely(original.Data.Data);
                    if (internalCopy is IActorExtraDataInterface asInterface)
                    {
                        newData.Data = asInterface;
                    }
                    else
                    {
                        if (original.Data.Data != null)
                        {
                            Type dataType = original.Data.Data.GetType();
                            if (typeof(IActorExtraDataInterface).IsAssignableFrom(dataType))
                            {
                                newData.Data = internalCopy as IActorExtraDataInterface;
                            }
                            else
                            {
                                Debug.WriteLine("DUP: unexpected Data.Data type: " + dataType.FullName);
                            }
                        }
                    }
                    clone.Data = newData;
                    clone.DataID = (short)actors.ExtraData.Count;
                    actors.ExtraData.Add(newData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error ExtraData: {ex.Message}\nType: {original.Data.Data?.GetType().FullName}");
                    Debug.WriteLine($"DUP clone error: {ex}");
                    return;
                }
            }
            TreeNode node = new TreeNode(clone.EntityName) { Tag = clone };

            if (clone.Data != null)
            {
                TreeNode child = new TreeNode("Extra Data") { Tag = clone.Data };
                node.Nodes.Add(child);
            }
            items.Nodes.Add(node);
            ActorTreeView.SelectedNode = node;
            ActorTreeView.Focus();
            UpdateTitle(true);
            bIsFileEdited = true;
        }

        private void RenumberButton_Click(object sender, EventArgs e)
        {
            RenumberDataIDsByTreeOrder();
        }

        private void RenumberDataIDsByTreeOrder()
        {
            if (actors == null || items == null) return;
            List<ActorExtraData> newList = new List<ActorExtraData>();
            Dictionary<ActorExtraData, short> remap = new Dictionary<ActorExtraData, short>();

            foreach (TreeNode node in items.Nodes)
            {
                if (node.Tag is ActorEntry entry && entry.Data != null)
                {
                    if (!remap.ContainsKey(entry.Data))
                    {
                        remap[entry.Data] = (short)newList.Count;
                        newList.Add(entry.Data);
                    }
                    entry.DataID = remap[entry.Data];
                }
            }
            actors.ExtraData = newList;
        }
    }
}