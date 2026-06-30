using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ResourceTypes.FrameProps;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class FramePropsEditor : Form
    {
        private FileInfo propsFile;
        private FramePropsFile propsData;

        private TreeNode RootNode;
        private bool bIsFileEdited;

        private List<TreeNode> searchResults = new List<TreeNode>();
        private int currentSearchIndex = -1;

        public FramePropsEditor(FileInfo file)
        {
            InitializeComponent();
            propsFile = file;
            BuildData(true);
            Show();
            ToolkitSettings.UpdateRichPresence("Editing FrameProps File.");
        }


        private void BuildData(bool fromFile)
        {
            TreeView_Main.Nodes.Clear();

            if (fromFile)
            {
                propsData = new FramePropsFile(propsFile);
            }

            string fileName = Path.GetFileName(propsFile.FullName);
            RootNode = new TreeNode($"FrameProps: {fileName}");
            RootNode.Tag = propsData;
            RootNode.NodeFont = new Font(TreeView_Main.Font, FontStyle.Bold);

            for (int i = 0; i < propsData.Entries.Length; i++)
            {
                FramePropsEntry entry = propsData.Entries[i];
                TreeNode entryNode = CreateEntryNode(entry, i);
                RootNode.Nodes.Add(entryNode);
            }

            TreeView_Main.Nodes.Add(RootNode);
            RootNode.Expand();

            UpdateStatusBar();
        }

        private TreeNode CreateEntryNode(FramePropsEntry entry, int index)
        {
            string displayName = entry.FrameName;
            TreeNode entryNode = new TreeNode($"[{index}] {displayName}");
            entryNode.Tag = entry;

            for (int i = 0; i < entry.Properties.Length; i++)
            {
                FrameProperty prop = entry.Properties[i];
                string[] valueParts = prop.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                string propDisplayText = valueParts.Length > 1
                    ? $"[{i}] {prop.PropertyName} ({valueParts.Length} values)"
                    : $"[{i}] {prop.PropertyName} = {prop.Value}";

                TreeNode propNode = new TreeNode(propDisplayText);
                propNode.Tag = prop;

                if (valueParts.Length > 1)
                {
                    for (int j = 0; j < valueParts.Length; j++)
                    {
                        string trimmedValue = valueParts[j].Trim();
                        var valuePart = new PropertyValuePart(prop, j, trimmedValue);

                        string nodeText = valuePart.IsKeyValue
                            ? $"[{j}] {valuePart.Key}: {valuePart.ParsedValue}"
                            : $"[{j}] {trimmedValue}";

                        TreeNode valueNode = new TreeNode(nodeText);
                        valueNode.Tag = valuePart;
                        propNode.Nodes.Add(valueNode);
                    }
                }

                entryNode.Nodes.Add(propNode);
            }

            return entryNode;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        private class PropertyValuePart
        {
            private string rawValue;

            [Browsable(false)]
            public FrameProperty ParentProperty { get; }

            [Description("Index of this value in the property's value list")]
            [ReadOnly(true)]
            [Category("Info")]
            public int Index { get; }

            [Description("Whether this value is a key-value pair (contains ':')")]
            [ReadOnly(true)]
            [Category("Info")]
            public bool IsKeyValue { get; private set; }

            [Description("The key part (before ':') - only for key-value pairs")]
            [Category("Key-Value")]
            public string Key { get; set; }

            [Description("The value part (after ':') - only for key-value pairs")]
            [Category("Key-Value")]
            public string ParsedValue { get; set; }

            [Description("The raw value - editing this will update the parent property")]
            [Category("Raw")]
            public string Value
            {
                get => rawValue;
                set
                {
                    rawValue = value;
                    ParseKeyValue();
                    UpdateParentProperty();
                }
            }

            public PropertyValuePart(FrameProperty parent, int index, string value)
            {
                ParentProperty = parent;
                Index = index;
                rawValue = value;
                ParseKeyValue();
            }

            private void ParseKeyValue()
            {
                int colonIndex = rawValue.IndexOf(':');
                if (colonIndex > 0 && colonIndex < rawValue.Length - 1)
                {
                    IsKeyValue = true;
                    Key = rawValue.Substring(0, colonIndex).Trim();
                    ParsedValue = rawValue.Substring(colonIndex + 1).Trim();
                }
                else
                {
                    IsKeyValue = false;
                    Key = "";
                    ParsedValue = rawValue;
                }
            }

            private void UpdateParentProperty()
            {
                string[] parts = ParentProperty.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (IsKeyValue && !string.IsNullOrEmpty(Key))
                {
                    rawValue = $"{Key}: {ParsedValue}";
                }

                if (Index < parts.Length)
                {
                    parts[Index] = rawValue;
                }

                ParentProperty.Value = string.Join(";", parts);
            }

            public override string ToString() => IsKeyValue ? $"{Key}: {ParsedValue}" : rawValue;
        }

        private void RefreshTree()
        {
            string selectedPath = GetNodePath(TreeView_Main.SelectedNode);
            BuildData(false);

            if (!string.IsNullOrEmpty(selectedPath))
            {
                TreeNode node = FindNodeByPath(selectedPath);
                if (node != null)
                {
                    TreeView_Main.SelectedNode = node;
                    node.EnsureVisible();
                }
            }
        }

        private string GetNodePath(TreeNode node)
        {
            if (node == null) return null;

            List<string> parts = new List<string>();
            while (node != null)
            {
                parts.Insert(0, node.Index.ToString());
                node = node.Parent;
            }
            return string.Join("/", parts);
        }

        private TreeNode FindNodeByPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            string[] parts = path.Split('/');
            TreeNode current = null;

            foreach (string part in parts)
            {
                if (!int.TryParse(part, out int index)) return null;

                TreeNodeCollection nodes = current == null ? TreeView_Main.Nodes : current.Nodes;
                if (index >= 0 && index < nodes.Count)
                {
                    current = nodes[index];
                }
                else
                {
                    return null;
                }
            }

            return current;
        }

        private void UpdateStatusBar()
        {
            int entryCount = propsData?.Entries?.Length ?? 0;
            int propertyCount = propsData?.Entries?.Sum(e => e.Properties?.Length ?? 0) ?? 0;

            StatusLabel_EntryCount.Text = $"Entries: {entryCount}";
            StatusLabel_PropertyCount.Text = $"Properties: {propertyCount}";
            StatusLabel_Selection.Text = "";
        }

        private void Save()
        {
            File.Copy(propsFile.FullName, propsFile.FullName + "_old", true);
            propsData.WriteToFile(propsFile.FullName);
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_Main.SelectedObject = null;
            TreeView_Main.SelectedNode = null;
            BuildData(true);
            bIsFileEdited = false;
            ClearSearch();
        }

        private void ExportXml()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.xml";
            saveFile.FileName = Path.GetFileNameWithoutExtension(propsFile.Name);
            saveFile.InitialDirectory = propsFile.DirectoryName;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                propsData.ConvertToXML(saveFile.FileName);
                MessageBox.Show("Export successful!", "FrameProps Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportXml()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML|*.xml";
            openFile.CheckFileExists = true;
            openFile.InitialDirectory = propsFile.DirectoryName;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFile.FileName))
                {
                    propsData.ConvertFromXML(openFile.FileName);
                    BuildData(false);
                    MarkAsEdited();
                }
            }
        }

        private void AddEntry()
        {
            FramePropsEntry newEntry = new FramePropsEntry
            {
                FrameNameHash = 0,
                Properties = Array.Empty<FrameProperty>()
            };

            FramePropsEntry[] newEntries = new FramePropsEntry[propsData.Entries.Length + 1];
            Array.Copy(propsData.Entries, newEntries, propsData.Entries.Length);
            newEntries[^1] = newEntry;
            propsData.Entries = newEntries;

            RefreshTree();
            MarkAsEdited();
        }

        private void DeleteEntry()
        {
            if (TreeView_Main.SelectedNode?.Tag is FramePropsEntry selectedEntry)
            {
                int entryIndex = Array.IndexOf(propsData.Entries, selectedEntry);
                if (entryIndex >= 0)
                {
                    FramePropsEntry[] newEntries = new FramePropsEntry[propsData.Entries.Length - 1];
                    int j = 0;
                    for (int i = 0; i < propsData.Entries.Length; i++)
                    {
                        if (i != entryIndex)
                        {
                            newEntries[j++] = propsData.Entries[i];
                        }
                    }
                    propsData.Entries = newEntries;

                    RefreshTree();
                    PropertyGrid_Main.SelectedObject = null;
                    MarkAsEdited();
                }
            }
        }

        private void AddProperty()
        {
            FramePropsEntry targetEntry = null;

            if (TreeView_Main.SelectedNode?.Tag is FramePropsEntry entry)
            {
                targetEntry = entry;
            }
            else if (TreeView_Main.SelectedNode?.Tag is FrameProperty prop)
            {
                if (TreeView_Main.SelectedNode.Parent?.Tag is FramePropsEntry parentEntry)
                {
                    targetEntry = parentEntry;
                }
            }

            if (targetEntry != null)
            {
                FrameProperty newProp = new FrameProperty
                {
                    PropertyNameHash = 0,
                    Value = ""
                };

                FrameProperty[] newProps = new FrameProperty[targetEntry.Properties.Length + 1];
                Array.Copy(targetEntry.Properties, newProps, targetEntry.Properties.Length);
                newProps[^1] = newProp;
                targetEntry.Properties = newProps;

                RefreshTree();
                MarkAsEdited();
            }
            else
            {
                MessageBox.Show("Please select a frame entry first!", "FrameProps Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteProperty()
        {
            if (TreeView_Main.SelectedNode?.Tag is FrameProperty selectedProp)
            {
                if (TreeView_Main.SelectedNode.Parent?.Tag is FramePropsEntry parentEntry)
                {
                    int propIndex = Array.IndexOf(parentEntry.Properties, selectedProp);
                    if (propIndex >= 0)
                    {
                        FrameProperty[] newProps = new FrameProperty[parentEntry.Properties.Length - 1];
                        int j = 0;
                        for (int i = 0; i < parentEntry.Properties.Length; i++)
                        {
                            if (i != propIndex)
                            {
                                newProps[j++] = parentEntry.Properties[i];
                            }
                        }
                        parentEntry.Properties = newProps;

                        RefreshTree();
                        PropertyGrid_Main.SelectedObject = null;
                        MarkAsEdited();
                    }
                }
            }
        }

        private void AddValueToProperty()
        {
            FrameProperty targetProp = null;

            if (TreeView_Main.SelectedNode?.Tag is FrameProperty prop)
            {
                targetProp = prop;
            }
            else if (TreeView_Main.SelectedNode?.Tag is PropertyValuePart valuePart)
            {
                targetProp = valuePart.ParentProperty;
            }

            if (targetProp != null)
            {
                if (string.IsNullOrEmpty(targetProp.Value))
                {
                    targetProp.Value = "NewValue";
                }
                else
                {
                    targetProp.Value += ";NewValue";
                }

                RefreshTree();
                MarkAsEdited();
            }
        }

        private void DeleteValueFromProperty()
        {
            if (TreeView_Main.SelectedNode?.Tag is PropertyValuePart valuePart)
            {
                var parentProp = valuePart.ParentProperty;
                string[] parts = parentProp.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 1 && valuePart.Index < parts.Length)
                {
                    var newParts = parts.Where((_, i) => i != valuePart.Index).ToArray();
                    parentProp.Value = string.Join(";", newParts);

                    RefreshTree();
                    MarkAsEdited();
                }
                else if (parts.Length == 1)
                {
                    parentProp.Value = "";
                    RefreshTree();
                    MarkAsEdited();
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Main.SelectedObject = e.Node.Tag;

            Button_DeleteEntry.Enabled = e.Node.Tag is FramePropsEntry;
            Button_AddProperty.Enabled = e.Node.Tag is FramePropsEntry || e.Node.Tag is FrameProperty;
            Button_DeleteProperty.Enabled = e.Node.Tag is FrameProperty;

            UpdateSelectionStatus(e.Node);
        }

        private void UpdateSelectionStatus(TreeNode node)
        {
            if (node?.Tag is FramePropsEntry entry)
            {
                StatusLabel_Selection.Text = $"Entry: {entry.FrameName} | Hash: 0x{entry.FrameNameHash:X16}";
            }
            else if (node?.Tag is FrameProperty prop)
            {
                StatusLabel_Selection.Text = $"Property: {prop.PropertyName} | Hash: 0x{prop.PropertyNameHash:X16}";
            }
            else if (node?.Tag is PropertyValuePart valuePart)
            {
                if (valuePart.IsKeyValue)
                {
                    StatusLabel_Selection.Text = $"Key-Value: {valuePart.Key} = {valuePart.ParsedValue}";
                }
                else
                {
                    StatusLabel_Selection.Text = $"Value: {valuePart.Value}";
                }
            }
            else
            {
                StatusLabel_Selection.Text = "";
            }
        }

        private void OnNodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.IsExpanded)
                    e.Node.Collapse();
                else
                    e.Node.Expand();
            }
        }

        private void PropertyGrid_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (TreeView_Main.SelectedNode != null)
            {
                var tag = TreeView_Main.SelectedNode.Tag;

                if (tag is FramePropsEntry entry)
                {
                    int index = TreeView_Main.SelectedNode.Index;
                    TreeView_Main.SelectedNode.Text = $"[{index}] {entry.FrameName}";
                }
                else if (tag is FrameProperty prop)
                {
                    UpdatePropertyNode(TreeView_Main.SelectedNode, prop);
                }
                else if (tag is PropertyValuePart valuePart)
                {
                    int valueIndex = TreeView_Main.SelectedNode.Index;
                    string nodeText = valuePart.IsKeyValue
                        ? $"[{valueIndex}] {valuePart.Key}: {valuePart.ParsedValue}"
                        : $"[{valueIndex}] {valuePart.Value}";

                    TreeView_Main.SelectedNode.Text = nodeText;

                    var parentNode = TreeView_Main.SelectedNode.Parent;
                    if (parentNode?.Tag is FrameProperty parentProp)
                    {
                        int propIndex = parentNode.Index;
                        string[] parts = parentProp.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        parentNode.Text = parts.Length > 1
                            ? $"[{propIndex}] {parentProp.PropertyName} ({parts.Length} values)"
                            : $"[{propIndex}] {parentProp.PropertyName} = {parentProp.Value}";
                    }
                }

                UpdateSelectionStatus(TreeView_Main.SelectedNode);
            }

            MarkAsEdited();
        }

        private void UpdatePropertyNode(TreeNode propNode, FrameProperty prop)
        {
            int index = propNode.Index;
            string[] valueParts = prop.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            propNode.Text = valueParts.Length > 1
                ? $"[{index}] {prop.PropertyName} ({valueParts.Length} values)"
                : $"[{index}] {prop.PropertyName} = {prop.Value}";

            propNode.Nodes.Clear();
            if (valueParts.Length > 1)
            {
                for (int j = 0; j < valueParts.Length; j++)
                {
                    string trimmedValue = valueParts[j].Trim();
                    var valuePart = new PropertyValuePart(prop, j, trimmedValue);

                    string nodeText = valuePart.IsKeyValue
                        ? $"[{j}] {valuePart.Key}: {valuePart.ParsedValue}"
                        : $"[{j}] {trimmedValue}";

                    TreeNode valueNode = new TreeNode(nodeText);
                    valueNode.Tag = valuePart;
                    propNode.Nodes.Add(valueNode);
                }
            }
        }

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var node = TreeView_Main.SelectedNode;

            Context_DeleteEntry.Enabled = node?.Tag is FramePropsEntry;
            Context_AddProperty.Enabled = node?.Tag is FramePropsEntry || node?.Tag is FrameProperty;
            Context_DeleteProperty.Enabled = node?.Tag is FrameProperty;
            Context_AddValue.Enabled = node?.Tag is FrameProperty || node?.Tag is PropertyValuePart;
            Context_DeleteValue.Enabled = node?.Tag is PropertyValuePart;
            Context_CopyHash.Enabled = node?.Tag is FramePropsEntry || node?.Tag is FrameProperty;
        }

        private void Context_AddValue_OnClick(object sender, EventArgs e) => AddValueToProperty();
        private void Context_DeleteValue_OnClick(object sender, EventArgs e) => DeleteValueFromProperty();

        private void Context_CopyHash_OnClick(object sender, EventArgs e)
        {
            var node = TreeView_Main.SelectedNode;
            string hash = null;

            if (node?.Tag is FramePropsEntry entry)
            {
                hash = $"0x{entry.FrameNameHash:X16}";
            }
            else if (node?.Tag is FrameProperty prop)
            {
                hash = $"0x{prop.PropertyNameHash:X16}";
            }

            if (!string.IsNullOrEmpty(hash))
            {
                Clipboard.SetText(hash);
                StatusLabel_Selection.Text = $"Copied: {hash}";
            }
        }

        private void FramePropsEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(
                    Language.GetString("$SAVE_PROMPT"),
                    "Toolkit",
                    System.Windows.MessageBoxButton.YesNoCancel);

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

        private void MarkAsEdited()
        {
            if (!bIsFileEdited)
            {
                bIsFileEdited = true;
            }
        }

        private void ClearSearch()
        {
            searchResults.Clear();
            currentSearchIndex = -1;
        }

        private bool NodeMatchesSearch(TreeNode node, string searchText, bool caseSensitive)
        {
            if (node.Tag == null) return false;

            StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            ulong? searchHash = TryParseHash(searchText);

            if (node.Tag is FramePropsEntry entry)
            {
                if (searchHash.HasValue)
                {
                    if (entry.FrameNameHash == searchHash.Value)
                        return true;
                }
                else
                {
                    if (entry.FrameName != null && entry.FrameName.IndexOf(searchText, comparison) >= 0)
                        return true;
                }
            }
            else if (node.Tag is FrameProperty prop)
            {
                if (searchHash.HasValue)
                {
                    if (prop.PropertyNameHash == searchHash.Value)
                        return true;
                }
                else
                {
                    if (prop.PropertyName != null && prop.PropertyName.IndexOf(searchText, comparison) >= 0)
                        return true;
                    if (prop.Value != null && prop.Value.IndexOf(searchText, comparison) >= 0)
                        return true;
                }
            }
            else if (node.Tag is PropertyValuePart valuePart)
            {
                if (valuePart.IsKeyValue)
                {
                    if (valuePart.Key != null && valuePart.Key.IndexOf(searchText, comparison) >= 0)
                        return true;
                    if (valuePart.ParsedValue != null && valuePart.ParsedValue.IndexOf(searchText, comparison) >= 0)
                        return true;
                }
                else
                {
                    if (valuePart.Value != null && valuePart.Value.IndexOf(searchText, comparison) >= 0)
                        return true;
                }
            }

            return false;
        }
        private ulong? TryParseHash(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            string trimmed = text.Trim();
            if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                trimmed = trimmed.Substring(2);

            if (ulong.TryParse(trimmed, System.Globalization.NumberStyles.HexNumber, null, out ulong hexValue))
                return hexValue;

            if (ulong.TryParse(trimmed, out ulong decValue))
                return decValue;

            return null;
        }

        private void CollectSearchResults(TreeNodeCollection nodes, string searchText, bool caseSensitive)
        {
            foreach (TreeNode node in nodes)
            {
                if (NodeMatchesSearch(node, searchText, caseSensitive))
                {
                    searchResults.Add(node);
                }
                if (node.Nodes.Count > 0)
                {
                    CollectSearchResults(node.Nodes, searchText, caseSensitive);
                }
            }
        }

        private void SelectSearchResult(int index)
        {
            if (index < 0 || index >= searchResults.Count) return;

            TreeNode node = searchResults[index];
            TreeView_Main.SelectedNode = node;
            node.EnsureVisible();
            node.BackColor = SystemColors.Highlight;
            Timer resetColor = new Timer();
            resetColor.Interval = 500;
            resetColor.Tick += (s, e) => { node.BackColor = SystemColors.Window; resetColor.Stop(); };
            resetColor.Start();

            currentSearchIndex = index;
            StatusLabel_Selection.Text = $"Found {index + 1} of {searchResults.Count}";
        }

        private void PerformSearch(bool reset = true)
        {
            string searchText = SearchTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter text to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (reset)
            {
                ClearSearch();
                bool caseSensitive = SearchCaseSensitive.Checked;
                CollectSearchResults(TreeView_Main.Nodes, searchText, caseSensitive);
            }

            if (searchResults.Count == 0)
            {
                MessageBox.Show("No matching nodes found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StatusLabel_Selection.Text = "No results found";
                return;
            }

            if (reset)
            {
                SelectSearchResult(0);
            }
            else
            {
                int nextIndex = (currentSearchIndex + 1) % searchResults.Count;
                SelectSearchResult(nextIndex);
            }
        }

        private void FindPrevious()
        {
            if (searchResults.Count == 0)
            {
                PerformSearch(true);
                return;
            }

            int prevIndex = currentSearchIndex - 1;
            if (prevIndex < 0) prevIndex = searchResults.Count - 1;
            SelectSearchResult(prevIndex);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch(true);
        }

        private void SearchNextButton_Click(object sender, EventArgs e)
        {
            if (searchResults.Count == 0)
                PerformSearch(true);
            else
                PerformSearch(false);
        }

        private void SearchPrevButton_Click(object sender, EventArgs e)
        {
            FindPrevious();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SearchTextBox.KeyPress += (s, args) =>
            {
                if (args.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch(true);
                    args.Handled = true;
                }
            };
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();
        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();
        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();
        private void Button_ExportXml_OnClick(object sender, EventArgs e) => ExportXml();
        private void Button_ImportXml_OnClick(object sender, EventArgs e) => ImportXml();
        private void Button_ExpandAll_OnClick(object sender, EventArgs e) => TreeView_Main.ExpandAll();
        private void Button_CollapseAll_OnClick(object sender, EventArgs e) => TreeView_Main.CollapseAll();
        private void Button_AddEntry_OnClick(object sender, EventArgs e) => AddEntry();
        private void Button_DeleteEntry_OnClick(object sender, EventArgs e) => DeleteEntry();
        private void Button_AddProperty_OnClick(object sender, EventArgs e) => AddProperty();
        private void Button_DeleteProperty_OnClick(object sender, EventArgs e) => DeleteProperty();
    }
}