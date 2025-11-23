using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ResourceTypes.Misc;
using Utils.Language;
using Utils.Logging;
using Utils.Settings;
using static ResourceTypes.Misc.StreamMapLoader;

namespace Mafia2Tool
{ 
    public partial class StreamEditor : Form
    {
        private FileInfo file;
        private StreamMapLoader stream;
        private static object clipboard;
        private bool bIsFileEdited = false;

        public StreamEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            this.file = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Stream editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$STREAM_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
            AddLineButton.Text = Language.GetString("$ADD_LINE");
            DeleteLineButton.Text = Language.GetString("$DELETE_LINE");
            DuplicateLine.Text = Language.GetString("$DUPLICATE_LINE");
            MoveItemDownButton.Text = Language.GetString("$MOVE_DOWN");
            MoveItemUpButton.Text = Language.GetString("$MOVE_UP");
        }

        private void Sort(List<StreamLoader> loaders)
        {
            for (int i = 0; i < loaders.Count - 1; i++)
            {
                for (int j = i + 1; j < loaders.Count; j++)
                {
                    if (loaders[i].start > loaders[j].start)
                    {

                        StreamLoader temp = loaders[i];
                        loaders[i] = loaders[j];
                        loaders[j] = temp;
                    }
                }
            }
        }

        private void UpdateStream()
        {
            List<StreamLine> lines = new List<StreamLine>();
            List<StreamLoader> loaders = new List<StreamLoader>();
            Dictionary<int, StreamLoader> currentLoaders = new Dictionary<int, StreamLoader>();
            Dictionary<int, bool> temp = new Dictionary<int, bool>();

            foreach (TreeNode node in linesTree.Nodes)
            {
                StreamHeaderGroup HeaderGroup = (StreamHeaderGroup)node.Tag;
                ToolkitAssert.Ensure(HeaderGroup != null, "We expect to be looking at a valid HeaderGroup.");

                foreach (TreeNode child in node.Nodes)
                {
                    StreamLine line = (child.Tag as StreamLine);
                    line.lineID = lines.Count;
                    line.Group = HeaderGroup.HeaderName;
                    
                    lines.Add(line);
                    temp = new Dictionary<int, bool>();

                    for (int i = 0; i != currentLoaders.Count; i++)
                    {
                        temp.Add(currentLoaders.ElementAt(i).Key.GetHashCode(), false);
                    }

                    foreach (var loader in currentLoaders)
                    {
                        foreach (var load in line.loadList)
                        {
                            if (loader.Key == load.GetHashCode())
                            {
                                temp[loader.Key] = true;
                            }
                        }
                    }

                    for (int i = 0; i != temp.Count;)
                    {
                        if (temp.ElementAt(i).Value == false)
                        {
                            loaders.Add(currentLoaders[temp.ElementAt(i).Key]);
                            currentLoaders.Remove(temp.ElementAt(i).Key);
                            temp.Remove(temp.ElementAt(i).Key);
                        }
                        else i++;
                    }

                    foreach (StreamLoader loader in line.loadList)
                    {
                        if (!currentLoaders.ContainsKey(loader.GetHashCode()))
                        {
                            loader.start = line.lineID;
                            loader.end = line.lineID;
                            currentLoaders.Add(loader.GetHashCode(), loader);
                            temp.Add(loader.GetHashCode(), true);
                        }
                        else
                        {
                            currentLoaders[loader.GetHashCode()].end = line.lineID;
                        }
                    }
                }
            }
            foreach (var loader in currentLoaders)
            {
                loaders.Add(loader.Value);
            }

            currentLoaders = null;
            temp = null;

            Sort(loaders);
            Dictionary<int, List<StreamLoader>> organised = new Dictionary<int, List<StreamLoader>>();
            List<StreamGroup> groups = new List<StreamGroup>();

            for(int i = 0; i < groupTree.Nodes.Count; i++)
            {
                var group = (groupTree.Nodes[i].Tag as StreamGroup);
                if (!organised.ContainsKey(i))
                {
                    organised.Add(i, new List<StreamLoader>());
                    groups.Add(group);
                }
            }

            foreach (StreamLoader pair in loaders)
            {
                // The main idea of this is to find if the user has changed the group.
                // We have to iterate through the groups first and find out if this change has indeed happened.
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];

                    // If there the user has assigned a preferred group then we can look for that too.
                    // To make sure we are saving everything necessary, lets just replace everything relating to groups.
                    if (pair.PreferredGroup == group.Name)
                    {
                        pair.AssignedGroup = group.Name;
                        pair.GroupID = i;
                        pair.Type = group.Type;
                        break;
                    }

                    // So we check if they have modified - if yes, then we reset the group assignment so the toolkit
                    // treats this as a newly created StreamLoader.                
                    if (pair.AssignedGroup == group.Name)
                    {
                        if(pair.Type != group.Type)
                        {
                            pair.AssignedGroup = string.Empty;
                            pair.GroupID = -1;
                        }
                        break;
                    }
                }

                // This will handle any non-declared group assignments. 
                if (string.IsNullOrEmpty(pair.AssignedGroup) && pair.GroupID == -1)
                {
                    if(pair.Type != GroupTypes.Null)
                    {
                        for(int i = 0; i < groups.Count; i++)
                        {
                            var group = groups[i];
                            if(group.Type == pair.Type)
                            {
                                pair.GroupID = i;
                                pair.AssignedGroup = group.Name;
                                break;
                            }
                        }
                    }
                }

                if (!organised.ContainsKey(pair.GroupID))
                {
                    organised.Add(pair.GroupID, new List<StreamLoader>());
                    organised[pair.GroupID].Add(pair);
                }
                else
                {
                    organised[pair.GroupID].Add(pair);
                }
            }

            List<StreamLoader> streamLoaders = new List<StreamLoader>();
            int idx = 0;
            foreach (KeyValuePair<int, List<StreamLoader>> pair in organised)
            {

                var group = groups[idx];
                group.startOffset = streamLoaders.Count;
                streamLoaders.AddRange(pair.Value);
                group.endOffset = streamLoaders.Count - group.startOffset;
                idx++;
            }

            stream.Lines = lines.ToArray();
            stream.Groups = groups.ToArray();
            stream.Loaders = streamLoaders.ToArray();
        }

        private void BuildData()
        {
            linesTree.Nodes.Clear();
            blockView.Nodes.Clear();
            groupTree.Nodes.Clear();
            PropertyGrid_Stream.SelectedObject = null;
            stream = new StreamMapLoader(file);

            for (int i = 0; i < stream.GroupHeaders.Length; i++)
            {
                TreeNode node = new TreeNode("group" + i);
                node.Text = stream.GroupHeaders[i];
                StreamHeaderGroup HeaderGroup = new StreamHeaderGroup();
                HeaderGroup.HeaderName = node.Text;
                node.Tag = HeaderGroup;
                linesTree.Nodes.Add(node);
            }
            for (int i = 0; i < stream.Groups.Length; i++)
            {
                var line = stream.Groups[i];
                TreeNode node = new TreeNode();
                node.Name = "GroupLoader" + i;
                node.Text = line.Name;
                node.Tag = line;

                for (int x = line.startOffset; x < line.startOffset + line.endOffset; x++)
                {
                    var loader = stream.Loaders[x];
                    loader.AssignedGroup = line.Name;
                    loader.GroupID = i;
                }

                groupTree.Nodes.Add(node);
            }
            for (int i = 0; i != stream.Lines.Length; i++)
            {
                var line = stream.Lines[i];
                TreeNode node = new TreeNode();
                node.Name = line.Name;
                node.Text = line.Name;
                node.Tag = line;

                List<StreamLoader> list = new List<StreamLoader>();
                for (int x = 0; x < stream.Loaders.Length; x++)
                {
                    var loader = stream.Loaders[x];
                    if (line.lineID >= loader.start && line.lineID <= loader.end)
                    {
                        var newLoader = new StreamLoader(loader);
                        list.Add(newLoader);
                    }
                }
                line.loadList = list.ToArray();
                linesTree.Nodes[line.groupID].Nodes.Add(node);
            }
            for (int i = 0; i < stream.Blocks.Length; i++)
            {
                TreeNode node = new TreeNode();
                node.Name = "Block" + i;
                node.Text = "Block: " + i;
                node.Tag = stream.Blocks[i];
                blockView.Nodes.Add(node);
            }

            Text = Language.GetString("$STREAM_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Save()
        {
            UpdateStream();
            stream.WriteToFile();

            Text = Language.GetString("$STREAM_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e) => PropertyGrid_Stream.SelectedObject = e.Node.Tag;
        private void ExitButtonPressed(object sender, System.EventArgs e) => Close();
        private void ReloadButtonPressed(object sender, System.EventArgs e) => BuildData();
        private void SaveButtonPressed(object sender, System.EventArgs e) => Save();


        private void OnContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (int i = 0; i != LineContextStrip.Items.Count; i++)
            {
                LineContextStrip.Items[i].Visible = false;
            }

            if (linesTree.SelectedNode != null && linesTree.SelectedNode.Tag != null)
            {
                if (linesTree.SelectedNode.Tag.GetType() == typeof(StreamHeaderGroup))
                {
                    AddLineButton.Visible = true;
                }
                else if (linesTree.SelectedNode.Tag.GetType() == typeof(StreamLine))
                {

                    DeleteLineButton.Visible = true;
                    DuplicateLine.Visible = true;
                    MoveItemDownButton.Visible = true;
                    MoveItemUpButton.Visible = true;
                }
            }
        }
        public class ExportedBranch
        {
            public StreamHeaderGroup Header { get; set; }
            public List<ExportedLine> Lines { get; set; }
        }

        public class ExportedLine
        {
            public string Name { get; set; }
            public int LoadType { get; set; }
            public string Flags { get; set; }
            public ulong Unk10 { get; set; }
            public ulong Unk11 { get; set; }
            public int Unk5 { get; set; }
            public int Unk12 { get; set; }
            public int Unk13 { get; set; }
            public int Unk14 { get; set; }
            public int Unk15 { get; set; }
            public List<ExportedLoader> LoadList { get; set; }
        }

        public class ExportedLoader
        {
            public int LoadType { get; set; }
            public string Path { get; set; }
            public string Entity { get; set; }
            public int start { get; set; }
            public int end { get; set; }
            public string Type { get; set; }
            public int LoaderSubID { get; set; }
            public int LoaderID { get; set; }
            public string AssignedGroup { get; set; }
            public string PreferredGroup { get; set; }
        }

        private void ExportBranch(object sender, System.EventArgs e)
        {
            if (linesTree.SelectedNode?.Tag is not StreamHeaderGroup headerGroup)
            {
                MessageBox.Show("Select a branch (StreamHeaderGroup) to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var branch = new ExportedBranch
            {
                Header = headerGroup,
                Lines = linesTree.SelectedNode.Nodes
                    .Cast<TreeNode>()
                    .Select(n =>
                    {
                        var line = n.Tag as StreamLine;
                        return new ExportedLine
                        {
                            Name = line.Name,
                            LoadType = line.LoadType,
                            Flags = line.Flags,
                            Unk10 = line.Unk10,
                            Unk11 = line.Unk11,
                            Unk5 = line.Unk5,
                            Unk12 = line.Unk12,
                            Unk13 = line.Unk13,
                            Unk14 = line.Unk14,
                            Unk15 = line.Unk15,
                            LoadList = line.LoadList?.Select(l => new ExportedLoader
                            {
                                LoadType = l.LoadType,
                                Path = l.Path,
                                Entity = l.Entity,
                                start = l.start,
                                end = l.end,
                                Type = l.Type.ToString(),
                                LoaderSubID = l.LoaderSubID,
                                LoaderID = l.LoaderID,
                                AssignedGroup = l.AssignedGroup,
                                PreferredGroup = l.PreferredGroup
                            }).ToList() ?? new List<ExportedLoader>()
                        };
                    }).ToList()
            };

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON Files (*.json)|*.json";
                sfd.FileName = headerGroup.HeaderName + ".json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string json = JsonConvert.SerializeObject(branch, Formatting.Indented);
                    File.WriteAllText(sfd.FileName, json);
                    MessageBox.Show("The branch was successfully exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ImportBranch(object sender, System.EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files (*.json)|*.json";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                string json = File.ReadAllText(ofd.FileName);
                try
                {
                    var imported = JsonConvert.DeserializeObject<ExportedBranch>(json);
                    if (imported == null || imported.Header == null)
                    {
                        MessageBox.Show("Incorrect branch file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    TreeNode newHeaderNode = new TreeNode(imported.Header.HeaderName);
                    newHeaderNode.Tag = imported.Header;

                    foreach (var importedLine in imported.Lines)
                    {
                        var line = new StreamLine
                        {
                            Name = importedLine.Name,
                            LoadType = importedLine.LoadType,
                            Flags = importedLine.Flags,
                            Unk10 = importedLine.Unk10,
                            Unk11 = importedLine.Unk11,
                            Unk5 = importedLine.Unk5,
                            Unk12 = importedLine.Unk12,
                            Unk13 = importedLine.Unk13,
                            Unk14 = importedLine.Unk14,
                            Unk15 = importedLine.Unk15,
                            loadList = importedLine.LoadList.Select(l => new StreamMapLoader.StreamLoader
                            {
                                LoadType = l.LoadType,
                                Path = l.Path,
                                Entity = l.Entity,
                                start = (int)l.start,
                                end = (int)l.end,
                                Type = Enum.TryParse(l.Type, out GroupTypes type) ? type : GroupTypes.Null,
                                LoaderSubID = l.LoaderSubID,
                                LoaderID = l.LoaderID,
                                AssignedGroup = l.AssignedGroup,
                                PreferredGroup = l.PreferredGroup
                            }).ToArray()
                        };

                        TreeNode lineNode = new TreeNode(line.Name) { Tag = line };
                        newHeaderNode.Nodes.Add(lineNode);
                    }

                    linesTree.Nodes.Add(newHeaderNode);
                    linesTree.ExpandAll();

                    Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;

                    MessageBox.Show("\r\nThe branch was successfully imported.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Import: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void DeleteLineButtonPressed(object sender, System.EventArgs e)
        {
            linesTree?.Nodes.Remove(linesTree.SelectedNode);

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void AddLineButtonPressed(object sender, System.EventArgs e)
        {
            TreeNode node = linesTree.SelectedNode;
            StreamLine line = new StreamLine();
            line.Group = node.Text;
            line.Flags = "";

            TreeNode child = new TreeNode();
            child.Name = "GroupLoader" + node.Index;
            child.Text = line.Name;
            child.Tag = line;
            node.Nodes.Add(child);

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                foreach (TreeNode node in linesTree.Nodes)
                {
                    if (node.Text.Contains(SearchBox.Text))
                    {
                        linesTree.SelectedNode = node;
                    }

                    foreach (TreeNode child in node.Nodes)
                    {
                        if (child.Text.Contains(SearchBox.Text))
                        {
                            linesTree.SelectedNode = child;
                        }
                    }
                }
            }
        }

        private void MoveItemUp()
        {
            if (linesTree.SelectedNode == null || linesTree.SelectedNode.Tag == null)
                return;

            TreeNode node = linesTree.SelectedNode;


            if (node.Tag is StreamLine)
            {
                TreeNode parent = node.Parent;
                int index = parent.Nodes.IndexOf(node);
                if (index > 0)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index - 1, node);
                    linesTree.SelectedNode = node;
                }
            }
   
            else if (node.Tag is StreamHeaderGroup)
            {
                int index = linesTree.Nodes.IndexOf(node);
                if (index > 0)
                {
                    linesTree.Nodes.RemoveAt(index);
                    linesTree.Nodes.Insert(index - 1, node);
                    linesTree.SelectedNode = node;
                }
            }

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void MoveItemUp_Click(object sender, System.EventArgs e)
        {
            MoveItemUp();
        }

        private void MoveItemDown()
        {
            if (linesTree.SelectedNode == null || linesTree.SelectedNode.Tag == null)
                return;

            TreeNode node = linesTree.SelectedNode;

            if (node.Tag is StreamLine)
            {
                TreeNode parent = node.Parent;
                int index = parent.Nodes.IndexOf(node);
                if (index < parent.Nodes.Count - 1)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index + 1, node);
                    linesTree.SelectedNode = node;
                }
            }
            else if (node.Tag is StreamHeaderGroup)
            {
                int index = linesTree.Nodes.IndexOf(node);
                if (index < linesTree.Nodes.Count - 1)
                {
                    linesTree.Nodes.RemoveAt(index);
                    linesTree.Nodes.Insert(index + 1, node);
                    linesTree.SelectedNode = node;
                }
            }

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void MoveItemDown_Click(object sender, System.EventArgs e)
        {
            MoveItemDown();
        }

        private void CopyLoadListAbove_Click(object sender, EventArgs e)
        {
            if (linesTree.SelectedNode != null && linesTree.SelectedNode.Tag != null)
            {
                if (linesTree.SelectedNode.Tag.GetType() == typeof(StreamLine))
                {
                    TreeNode node = linesTree.SelectedNode;
                    StreamLine newLine = new StreamLine((node.Tag as StreamLine));
                    TreeNode newNode = new TreeNode();
                    newNode.Name = "GroupLoader" + node.Index;
                    newNode.Text = newLine.Name;
                    newNode.Tag = newLine;
                    node.Parent.Nodes.Insert(node.Index + 1, newNode);

                    Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;
                }
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ChangedItem.Label == "Name")
            {
                if(tabControl.SelectedTab == StreamLinesPage)
                {
                    TreeNode selected = linesTree.SelectedNode;
                    linesTree.SelectedNode.Text = e.ChangedItem.Value.ToString();
                }
                else if (tabControl.SelectedTab == StreamGroupPage)
                {
                    TreeNode selected = groupTree.SelectedNode;
                    groupTree.SelectedNode.Text = e.ChangedItem.Value.ToString();
                }
            }
            else if(e.ChangedItem.Label == "HeaderName")
            {
                if (tabControl.SelectedTab == StreamLinesPage)
                {
                    TreeNode selected = linesTree.SelectedNode;
                    linesTree.SelectedNode.Text = e.ChangedItem.Value.ToString();
                }
            }
            else if(e.ChangedItem.Label == "PreferredGroup")
            {
                UpdateStream();
            }

            PropertyGrid_Stream.Refresh();
            Cursor.Current = Cursors.Default;

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if(linesTree.Focused)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.Control && e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
        }

        private void Paste()
        {
            if (clipboard == null)
            {
                //MessageBox.Show("Буфер пуст. Сначала скопируйте узел (Ctrl+C).", "Вставка",
                //MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (linesTree.SelectedNode?.Tag is not StreamLine targetLine)
            {
                //MessageBox.Show("Выберите строку (StreamLine) для замены.", "Ошибка",
                //MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clipboard is not StreamLine sourceLine)
            {
                //MessageBox.Show("Буфер содержит несовместимый тип данных.", "Ошибка",
                //MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            targetLine.Name = sourceLine.Name;
            targetLine.Flags = sourceLine.Flags;
            targetLine.loadList = sourceLine.loadList?.ToArray();

            linesTree.SelectedNode.Text = targetLine.Name;

            PropertyGrid_Stream.SelectedObject = targetLine;

            if (!bIsFileEdited)
            {
                Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }

        }

        private void Copy()
        {
            if (linesTree.SelectedNode?.Tag is StreamLine or StreamHeaderGroup)
            {
                clipboard = linesTree.SelectedNode.Tag;

            }
        }

        private void Button_CreateLineGroup_Click(object sender, EventArgs e)
        {
            StreamHeaderGroup HeaderGroup = new StreamHeaderGroup();
            HeaderGroup.HeaderName = "New_Line_Group";

            TreeNode NewHeaderNode = new TreeNode();
            NewHeaderNode.Text = "New_Line_Group";
            NewHeaderNode.Tag = HeaderGroup;
            linesTree.Nodes.Add(NewHeaderNode);

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_AddBlock_Click(object sender, EventArgs e)
        {
            if (stream == null)
            {
                MessageBox.Show("Stream not loaded yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            var newBlock = new StreamMapLoader.StreamBlock();
            newBlock.startOffset = 0;
            newBlock.endOffset = 0;
            newBlock.Hashes = new ulong[0];

         
            int newIndex = blockView.Nodes.Count; 
            TreeNode node = new TreeNode();
            node.Name = "Block" + newIndex;
            node.Text = "Block: " + newIndex;
            node.Tag = newBlock;
            blockView.Nodes.Add(node);

          
            var blocksList = stream.Blocks?.ToList() ?? new List<StreamMapLoader.StreamBlock>();
            blocksList.Add(newBlock);
            stream.Blocks = blocksList.ToArray();

     
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_DeleteBlock_Click(object sender, EventArgs e)
        {
            if (stream == null || blockView.SelectedNode == null)
            {
                return;
            }

            TreeNode sel = blockView.SelectedNode;
            int nodeIndex = blockView.Nodes.IndexOf(sel);
            if (nodeIndex < 0)
                return;

            blockView.Nodes.RemoveAt(nodeIndex);

            var blocksList = stream.Blocks?.ToList() ?? new List<StreamMapLoader.StreamBlock>();

            if (nodeIndex < blocksList.Count)
            {
                blocksList.RemoveAt(nodeIndex);
            }
            else
            {
                var blockToRemove = sel.Tag as StreamMapLoader.StreamBlock;
                if (blockToRemove != null)
                {
                    if (!blocksList.Remove(blockToRemove))
                    {
                        var candidate = blocksList.FirstOrDefault(b =>
                            b.startOffset == blockToRemove.startOffset &&
                            b.endOffset == blockToRemove.endOffset &&
                            ((b.Hashes == null && blockToRemove.Hashes == null) ||
                             (b.Hashes != null && blockToRemove.Hashes != null && Enumerable.SequenceEqual(b.Hashes, blockToRemove.Hashes)))
                        );
                        if (candidate != null) blocksList.Remove(candidate);
                    }
                }
            }

            stream.Blocks = blocksList.ToArray();

            for (int i = 0; i < blockView.Nodes.Count; i++)
            {
                blockView.Nodes[i].Name = "Block" + i;
                blockView.Nodes[i].Text = "Block: " + i;
            }

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_CreateStreamGroup_Click(object Sender, EventArgs Args)
        {
            StreamGroup NewGroup = new StreamGroup();
            NewGroup.Name = "New_Group";
            NewGroup.Type = GroupTypes.Null;

            TreeNode NewGroupNode = new TreeNode();
            NewGroupNode.Text = "New_Group_Node";
            NewGroupNode.Tag = NewGroup;
            groupTree.Nodes.Add(NewGroupNode);

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void StreamEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void ExportAllStreamLines(object sender, EventArgs e)
        {
            var allBranches = linesTree.Nodes
                .Cast<TreeNode>()
                .Select(node =>
                {
                    var headerGroup = node.Tag as StreamHeaderGroup;
                    return new ExportedBranch
                    {
                        Header = headerGroup,
                        Lines = node.Nodes
                            .Cast<TreeNode>()
                            .Select(childNode =>
                            {
                                var line = childNode.Tag as StreamLine;
                                return new ExportedLine
                                {
                                    Name = line.Name,
                                    LoadType = line.LoadType,
                                    Flags = line.Flags,
                                    Unk10 = line.Unk10,
                                    Unk11 = line.Unk11,
                                    Unk5 = line.Unk5,
                                    Unk12 = line.Unk12,
                                    Unk13 = line.Unk13,
                                    Unk14 = line.Unk14,
                                    Unk15 = line.Unk15,
                                    LoadList = line.loadList?.Select(l => new ExportedLoader
                                    {
                                        LoadType = l.LoadType,
                                        Path = l.Path,
                                        Entity = l.Entity,
                                        start = l.start,
                                        end = l.end,
                                        Type = l.Type.ToString(),
                                        LoaderSubID = l.LoaderSubID,
                                        LoaderID = l.LoaderID,
                                        AssignedGroup = l.AssignedGroup,
                                        PreferredGroup = l.PreferredGroup
                                    }).ToList() ?? new List<ExportedLoader>()
                                };
                            }).ToList()
                    };
                }).ToList();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON Files (*.json)|*.json";
                sfd.FileName = "AllStreamLines.json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string json = JsonConvert.SerializeObject(allBranches, Formatting.Indented);
                    File.WriteAllText(sfd.FileName, json);
                    MessageBox.Show("All Stream Lines successfully exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

     
        private void ImportAllStreamLines(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files (*.json)|*.json";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                string json = File.ReadAllText(ofd.FileName);
                try
                {
                    var importedBranches = JsonConvert.DeserializeObject<List<ExportedBranch>>(json);
                    if (importedBranches == null)
                    {
                        MessageBox.Show("Incorrect file format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (var branch in importedBranches)
                    {
                        TreeNode newHeaderNode = new TreeNode(branch.Header.HeaderName)
                        {
                            Tag = branch.Header
                        };

                        foreach (var importedLine in branch.Lines)
                        {
                            var line = new StreamLine
                            {
                                Name = importedLine.Name,
                                LoadType = importedLine.LoadType,
                                Flags = importedLine.Flags,
                                Unk10 = importedLine.Unk10,
                                Unk11 = importedLine.Unk11,
                                Unk5 = importedLine.Unk5,
                                Unk12 = importedLine.Unk12,
                                Unk13 = importedLine.Unk13,
                                Unk14 = importedLine.Unk14,
                                Unk15 = importedLine.Unk15,
                                loadList = importedLine.LoadList.Select(l => new StreamMapLoader.StreamLoader
                                {
                                    LoadType = l.LoadType,
                                    Path = l.Path,
                                    Entity = l.Entity,
                                    start = l.start,
                                    end = l.end,
                                    Type = Enum.TryParse(l.Type, out GroupTypes type) ? type : GroupTypes.Null,
                                    LoaderSubID = l.LoaderSubID,
                                    LoaderID = l.LoaderID,
                                    AssignedGroup = l.AssignedGroup,
                                    PreferredGroup = l.PreferredGroup
                                }).ToArray()
                            };

                            TreeNode lineNode = new TreeNode(line.Name) { Tag = line };
                            newHeaderNode.Nodes.Add(lineNode);
                        }

                        linesTree.Nodes.Add(newHeaderNode);
                    }

                    linesTree.ExpandAll();
                    Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;

                    MessageBox.Show("All Stream Lines successfully imported.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Import: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DeleteStreamGroup_Click(object sender, EventArgs e)
        {
            if (groupTree.SelectedNode == null || groupTree.SelectedNode.Tag == null)
                return;

            groupTree.Nodes.Remove(groupTree.SelectedNode);

            if (stream != null)
            {
                var groupsList = stream.Groups?.ToList() ?? new List<StreamGroup>();
                var groupToRemove = groupTree.SelectedNode.Tag as StreamGroup;
                if (groupToRemove != null)
                {
                    groupsList.Remove(groupToRemove);
                    stream.Groups = groupsList.ToArray();
                }
            }

            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void DeleteAllStreamLines(object sender, EventArgs e)
        {
            if (linesTree.Nodes.Count == 0)
                return;

           
            var result = MessageBox.Show(
                "Are you sure you want to delete ALL Stream Lines?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;


            foreach (TreeNode headerNode in linesTree.Nodes)
            {
                headerNode.Nodes.Clear();
            }

          
            if (stream != null)
            {
                stream.Lines = new StreamMapLoader.StreamLine[0];
            }


            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

    }



    public class StreamHeaderGroup
    {
        public string HeaderName { get; set; }
    }

}