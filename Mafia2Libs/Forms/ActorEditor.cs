using Forms.EditorControls;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
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
        private static ActorEntry branchClipboard;
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

        private static readonly Dictionary<ActorTypes, int> TypePriority = new Dictionary<ActorTypes, int>()
        {
            { ActorTypes.Human, 14 },
            { ActorTypes.C_Player2, 16 },
            { ActorTypes.C_Car, 18 },
            { ActorTypes.C_Train, 19 },
            { ActorTypes.C_CrashObject, 20 },
            { ActorTypes.C_TrafficCar, 21 },
            { ActorTypes.C_TrafficHuman, 22 },
            { ActorTypes.C_TrafficTrain, 23 },
            { ActorTypes.ActionPoint, 25 },
            { ActorTypes.ActionPointScript, 30 },
            { ActorTypes.ActionPointSearch, 32 },
            { ActorTypes.C_Item, 36 },
            { ActorTypes.C_Door, 38 },
            { ActorTypes.Tree, 39 },
            { ActorTypes.Lift, 40 },
            { ActorTypes.C_Sound, 41 },
            { ActorTypes.SoundMixer, 43 },
            { ActorTypes.Boat, 47 },
            { ActorTypes.Radio, 48 },
            { ActorTypes.JukeBox, 49 },
            { ActorTypes.StaticEntity, 52 },
            { ActorTypes.C_TranslocatedCar, 53 },
            { ActorTypes.Garage, 54 },
            { ActorTypes.FrameWrapper, 55 },
            { ActorTypes.C_ActorDetector, 56 },
            { ActorTypes.Blocker, 63 },
            { ActorTypes.C_StaticWeapon, 64 },
            { ActorTypes.C_StaticParticle, 66 },
            { ActorTypes.FireTarget, 70 },
            { ActorTypes.LightEntity, 71 },
            { ActorTypes.C_Cutscene, 73 },
            { ActorTypes.Telephone, 95 },
            { ActorTypes.C_ScriptEntity, 98 },
            { ActorTypes.DangerZone, 103 },
            { ActorTypes.Airplane, 104 },
            { ActorTypes.C_Pinup, 106 },
            { ActorTypes.SpikeStrip, 107 },
            { ActorTypes.C_DummyDoor, 109 },
            { ActorTypes.FramesController, 110 },
            { ActorTypes.Wardrobe, 112 },
            { ActorTypes.PhysicsScene, 113 },
            { ActorTypes.CleanEntity, 114 },
            { ActorTypes.None, 999 },
        };
        private void BuildData()
        {
            actors = new Actor(actorFile);
            actors.Items.Sort((a, b) =>
            {
                int pa = TypePriority.ContainsKey((ActorTypes)a.ActorTypeID) ? TypePriority[(ActorTypes)a.ActorTypeID] : 999;
                int pb = TypePriority.ContainsKey((ActorTypes)b.ActorTypeID) ? TypePriority[(ActorTypes)b.ActorTypeID] : 999;
                int typeCompare = pa.CompareTo(pb);
                if (typeCompare != 0) return typeCompare;
                return string.Compare(a.EntityName, b.EntityName, StringComparison.InvariantCultureIgnoreCase);
            });
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

        private void CopyEntityBranch(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = ActorTreeView.SelectedNode;
            if (selectedNode == null || !(selectedNode.Tag is ActorEntry original)) return;

            ActorEntry cloned = actors.CreateActorEntry((ActorTypes)original.ActorTypeID, original.EntityName + "");
            cloned.DefinitionName = original.DefinitionName;
            cloned.FrameName = original.FrameName;
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

        private void PasteEntityBranch(object sender, System.EventArgs e)
        {
            if (branchClipboard == null) return;

            ActorEntry newEntry = actors.CreateActorEntry((ActorTypes)branchClipboard.ActorTypeID, branchClipboard.EntityName);
            newEntry.DefinitionName = branchClipboard.DefinitionName;
            newEntry.FrameName = branchClipboard.FrameName;
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
            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
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
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "EntityName") ActorTreeView.SelectedNode.Text = e.ChangedItem.Value.ToString();
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
            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
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
