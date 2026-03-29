using Gibbed.Illusion.FileFormats.Hashing;
using Gibbed.Mafia2.ResourceFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class TableEditor : Form
    {
        private FileInfo file;
        private TableData data;
        private Dictionary<uint, string> columnNames = new Dictionary<uint, string>();
        private ushort Version;
        private bool bIsFileEdited = false;
        private Dictionary<uint, string> columnDescriptions = new Dictionary<uint, string>();
        public string FileName => file?.Name ?? "";
        private Dictionary<string, string> textDatabase = null;
        private string selectedKeyColumnName = null;
        private string textDbFilePath = null;



        public TableEditor(FileInfo file)
        {
            InitializeComponent();
            this.file = file;
            Initialise();
            Show();
        }

        private void LoadTextDbButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                ofd.Title = "Load Text Database";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textDbFilePath = ofd.FileName;
                    ParseTextDatabase(textDbFilePath);
                    UpdateTextDatabaseUI();
                }
            }
        }

        private void ParseTextDatabase(string path)
        {
            textDatabase = new Dictionary<string, string>();
            var lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                int colonIndex = line.IndexOf(':');
                if (colonIndex <= 0) continue;
                string key = line.Substring(0, colonIndex).Trim();
                string value = line.Substring(colonIndex + 1);
                textDatabase[key] = value;
            }
            if (data != null && data.Columns != null)
            {
                keyColumnComboBox.Items.Clear();
                foreach (var col in data.Columns)
                {
                    string colName = GetColumnName(col.NameHash);
                    keyColumnComboBox.Items.Add(colName);
                }
                if (keyColumnComboBox.Items.Count > 0)
                {
                    keyColumnComboBox.SelectedIndex = 0;
                    selectedKeyColumnName = keyColumnComboBox.SelectedItem.ToString();
                }
            }
        }

        private void UpdateTextDatabaseUI()
        {

            keyColumnComboBox.Enabled = textDatabase != null && data != null && data.Columns.Count > 0;
            RefreshCurrentRow();
        }

        private void KeyColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keyColumnComboBox.SelectedItem != null)
            {
                selectedKeyColumnName = keyColumnComboBox.SelectedItem.ToString();
                RefreshCurrentRow();
            }
        }

        private void RefreshCurrentRow()
        {
            if (treeViewRows.SelectedNode != null && treeViewRows.SelectedNode.Tag is TableData.Row row)
            {
                var wrapper = new DynamicRowWrapper(row, data.Columns, this);
                propertyGrid.SelectedObject = wrapper;
            }
        }

        public void Initialise()
        {
            ReadExternalHashes();
            LoadColumnDescriptions();
            LoadTableData();
        }

        private void LoadColumnDescriptions()
        {
            string descFilePath = Path.Combine("Resources", "column_descriptions.txt");
            if (!File.Exists(descFilePath))
                return;

            try
            {
                string[] lines = File.ReadAllLines(descFilePath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(new char[] { ' ' }, 2);
                    if (parts.Length < 2) continue;

                    string hashStr = parts[0];
                    string description = parts[1].Trim();

                    uint hash;
                    if (hashStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        hash = Convert.ToUInt32(hashStr.Substring(2), 16);
                    else if (hashStr.IndexOfAny("ABCDEFabcdef".ToCharArray()) >= 0)
                        hash = Convert.ToUInt32(hashStr, 16);
                    else
                        hash = uint.Parse(hashStr);

                    columnDescriptions[hash] = description;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load column descriptions: {ex.Message}");
            }
        }

        private string GetColumnDescription(uint hash)
        {
            if (columnDescriptions.TryGetValue(hash, out string desc))
                return desc;
            return null;
        }

        private void ReadExternalHashes()
        {
            columnNames.Clear();
            columnDescriptions.Clear();

            try
            {
                string[] hashes = File.ReadAllLines(Path.Combine("Resources", "hashes.txt"));
                foreach (var hash in hashes)
                {
                    uint key = FNV32.Hash(hash);
                    columnNames.TryAdd(key, hash);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Missing hashes.txt, No column names will be present.", "Toolkit", MessageBoxButtons.OK);
                columnNames = new Dictionary<uint, string>();
            }
            FileInfo CustomHashesFile = new FileInfo(Path.Combine("Resources", "custom_hashes.txt"));
            if (CustomHashesFile.Exists)
            {
                string[] CustomHashes = File.ReadAllLines(CustomHashesFile.FullName);
                foreach (string Line in CustomHashes)
                {
                    if (string.IsNullOrWhiteSpace(Line)) continue;

                    string[] parts = Line.Split(new char[] { ' ' }, 2);
                    if (parts.Length < 2) continue;

                    string hashStr = parts[0];
                    string rest = parts[1].Trim();

                    string name = rest;
                    string description = null;

                    int pipeIndex = rest.IndexOf('|');
                    if (pipeIndex >= 0)
                    {
                        name = rest.Substring(0, pipeIndex).Trim();
                        description = rest.Substring(pipeIndex + 1).Trim();
                    }

                    uint hash;
                    if (hashStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        hash = Convert.ToUInt32(hashStr.Substring(2), 16);
                    else if (hashStr.IndexOfAny("ABCDEFabcdef".ToCharArray()) >= 0)
                        hash = Convert.ToUInt32(hashStr, 16);
                    else
                        hash = uint.Parse(hashStr);

                    columnNames.TryAdd(hash, name);
                    if (!string.IsNullOrEmpty(description))
                        columnDescriptions.TryAdd(hash, description);
                }
            }
        }
        
        private string GetColumnName(uint hash)
        {
            if (columnNames.ContainsKey(hash))
                return columnNames[hash];
            return hash.ToString("X8");
        }

        private void LoadTableData()
        {
            data = new TableData();
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                Version = (ushort)reader.ReadInt32();
                data.Deserialize(Version, reader.BaseStream, Gibbed.IO.Endian.Little);
            }
            treeViewRows.Nodes.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                TreeNode node = new TreeNode(GetRowDisplayName(data.Rows[i], i));
                node.Tag = data.Rows[i];
                treeViewRows.Nodes.Add(node);
            }
            versionComboBox.SelectedIndex = (Version == 1) ? 0 : (Version == 2) ? 1 : 0;
            versionLabel.Text = $"Version: {Version}";
            bIsFileEdited = false;
            this.Text = $"{FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}";
        }

        private void SaveTableData()
        {
            ushort selectedVersion = (ushort)(versionComboBox.SelectedIndex == 0 ? 1 : 2);

            if (selectedVersion == 1 && (data.PatchedNameHash != 0 || !string.IsNullOrEmpty(data.PatchedName) || data.PatchedUnk1 != 0 || data.PatchedUnk2 != 0))
            {
                var result = MessageBox.Show("Switching to version 1 will discard Patched fields (PatchedName, PatchedUnk1, PatchedUnk2). Continue?", "Warning", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                    return;
            }

            if (selectedVersion == 2 && data.PatchedName == null)
                data.PatchedName = "";

            TableData newData = new TableData();
            newData.NameHash = data.NameHash;
            newData.Name = data.Name;
            if (selectedVersion == 1)
            {
                newData.PatchedNameHash = 0;
                newData.PatchedName = null;
                newData.PatchedUnk1 = 0;
                newData.PatchedUnk2 = 0;
            }
            else
            {
                newData.PatchedNameHash = data.PatchedNameHash;
                newData.PatchedName = data.PatchedName;
                newData.PatchedUnk1 = data.PatchedUnk1;
                newData.PatchedUnk2 = data.PatchedUnk2;
            }
            newData.Unk1 = data.Unk1;
            newData.Unk2 = data.Unk2;
            newData.Columns = data.Columns;

            foreach (var row in data.Rows)
            {
                newData.Rows.Add(row);
            }
            if (!newData.Validate())
            {
                MessageBox.Show("Failed to validate. Not saving data.", "Toolkit", MessageBoxButtons.OK);
                return;
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                writer.Write((int)selectedVersion);
                newData.Serialize(selectedVersion, writer.BaseStream, Gibbed.IO.Endian.Little);
            }
            data = newData;
            Version = selectedVersion;
            versionLabel.Text = $"Version: {Version}";
            bIsFileEdited = false;
            this.Text = $"{FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}";
        }

        private void TreeViewRows_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is TableData.Row row)
            {
                var wrapper = new DynamicRowWrapper(row, data.Columns, this);
                propertyGrid.SelectedObject = wrapper;
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (!bIsFileEdited)
            {
                bIsFileEdited = true;
                this.Text = $"{FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
            }
            if (treeViewRows.SelectedNode != null && treeViewRows.SelectedNode.Tag is TableData.Row row)
            {
                int rowIndex = treeViewRows.SelectedNode.Index;
                treeViewRows.SelectedNode.Text = GetRowDisplayName(row, rowIndex);
            }
        }

        private void AddRowOnClick(object sender, EventArgs e)
        {
            List<object> newRowValues = new List<object>();
            foreach (var column in data.Columns)
            {
                Type dataType = TableData.GetValueTypeForColumnType(column.Type);
                if (dataType == typeof(bool))
                    newRowValues.Add(false);
                else if (dataType == typeof(uint) || dataType == typeof(int) || dataType == typeof(float))
                    newRowValues.Add(Activator.CreateInstance(dataType));
                else if (dataType == typeof(string))
                {
                    if (column.Type == TableData.ColumnType.Color)
                        newRowValues.Add("255 255 255");
                    else
                        newRowValues.Add("");
                }
                else
                    newRowValues.Add("");
            }
            TableData.Row newRow = new TableData.Row { Values = newRowValues };
            data.Rows.Add(newRow);

            TreeNode newNode = new TreeNode($"Row {data.Rows.Count - 1}");
            newNode.Tag = newRow;
            treeViewRows.Nodes.Add(newNode);
            bIsFileEdited = true;
            this.Text = $"{FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
        }

        private void DeleteRowOnClick(object sender, EventArgs e)
        {
            if (treeViewRows.SelectedNode != null)
            {
                data.Rows.Remove((TableData.Row)treeViewRows.SelectedNode.Tag);
                treeViewRows.Nodes.Remove(treeViewRows.SelectedNode);
                bIsFileEdited = true;
                this.Text = $"{FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
                propertyGrid.SelectedObject = null;
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string query = searchBox.Text?.Trim();
            if (string.IsNullOrEmpty(query))
            {
                treeViewRows.SelectedNode = null;
                return;
            }

            foreach (TreeNode node in treeViewRows.Nodes)
            {
                TableData.Row row = node.Tag as TableData.Row;
                if (row != null)
                {
                    bool found = false;
                    foreach (object val in row.Values)
                    {
                        if (val != null && val.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found && node.Text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                        found = true;

                    if (found)
                    {
                        treeViewRows.SelectedNode = node;
                        node.EnsureVisible();
                        return;
                    }
                }
            }
        }

        private string GetRowDisplayName(TableData.Row row, int rowIndex)
        {
            string[] priorityNames = { "Model", "Name", "Notes", "Description", "Descr", "Civil", "E8939FBA", "Path", "File_Name", "ADBBFF55", "74203AAC", "Car_Name" };

            int targetColumnIndex = -1;

            foreach (string pName in priorityNames)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    string colName = GetColumnName(data.Columns[i].NameHash);
                    if (colName.Equals(pName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetColumnIndex = i;
                        break;
                    }
                }
                if (targetColumnIndex >= 0) break;
            }

            if (targetColumnIndex < 0 && data.Columns.Count > 0)
                targetColumnIndex = 0;

            if (targetColumnIndex >= 0 && targetColumnIndex < row.Values.Count)
            {
                object val = row.Values[targetColumnIndex];
                return val?.ToString() ?? "(null)";
            }

            return $"Row {rowIndex}";
        }
        private void VersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (data == null) return;
            ushort newVersion = (ushort)(versionComboBox.SelectedIndex == 0 ? 1 : 2);
            if (newVersion == Version) return;
            if (newVersion == 2 && data.PatchedName == null)
                data.PatchedName = "";
            bIsFileEdited = true;
            this.Text = $"{FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
        }

        private void ReloadOnClick(object sender, EventArgs e) => LoadTableData();
        private void SaveOnClick(object sender, EventArgs e) => SaveTableData();
        private void ExitButtonOnClick(object sender, EventArgs e) => Close();

        private void TableEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                DialogResult result = MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                    SaveTableData();
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
        private class DynamicRowWrapper : ICustomTypeDescriptor
        {
            private TableData.Row row;
            private List<TableData.Column> columns;
            private TableEditor editor;

            [DisplayName("Localized Text")]
            [Description("Text from loaded database, key taken from selected column.")]
            public string LocalizedText
            {
                get
                {
                    if (editor.textDatabase == null || string.IsNullOrEmpty(editor.selectedKeyColumnName))
                        return null;

                    int keyColumnIndex = FindKeyColumnIndex();
                    if (keyColumnIndex == -1 || keyColumnIndex >= row.Values.Count)
                        return null;

                    object keyObj = row.Values[keyColumnIndex];
                    string rawKey = keyObj?.ToString();
                    if (string.IsNullOrEmpty(rawKey))
                        return null;

                    string formattedKey = FormatKey(rawKey, keyObj);
                    if (editor.textDatabase.TryGetValue(formattedKey, out string text))
                        return text;
                    return null;
                }
                set
                {
                    if (editor.textDatabase == null || string.IsNullOrEmpty(editor.selectedKeyColumnName))
                        return;

                    int keyColumnIndex = FindKeyColumnIndex();
                    if (keyColumnIndex == -1 || keyColumnIndex >= row.Values.Count)
                        return;

                    object keyObj = row.Values[keyColumnIndex];
                    string rawKey = keyObj?.ToString();
                    if (string.IsNullOrEmpty(rawKey))
                        return;

                    string formattedKey = FormatKey(rawKey, keyObj);
                    editor.textDatabase[formattedKey] = value ?? "";
                    editor.bIsFileEdited = true;
                    editor.Text = $"{editor.FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
                }
            }

            private int FindKeyColumnIndex()
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    string colName = editor.GetColumnName(columns[i].NameHash);
                    if (colName == editor.selectedKeyColumnName)
                        return i;
                }
                return -1;
            }

            private string FormatKey(string rawKey, object originalValue)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(rawKey, @"^\d{2}_\d{2}_\d{2}_\d{4}$"))
                    return rawKey;

                bool isNumeric = originalValue is int || originalValue is uint || originalValue is long || originalValue is ulong || (originalValue is string s && long.TryParse(s, out _));

                if (isNumeric)
                {
                    string numStr = rawKey.PadLeft(10, '0');
                    if (numStr.Length == 10)
                    {
                        return $"{numStr.Substring(0, 2)}_{numStr.Substring(2, 2)}_{numStr.Substring(4, 2)}_{numStr.Substring(6, 4)}";
                    }
                }
                return rawKey;
            }

            public DynamicRowWrapper(TableData.Row row, List<TableData.Column> columns, TableEditor editor)
            {
                this.row = row;
                this.columns = columns;
                this.editor = editor;
            }

            public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
            public string GetClassName() => "Table Row";
            public string GetComponentName() => null;
            public TypeConverter GetConverter() => null;
            public EventDescriptor GetDefaultEvent() => null;
            public PropertyDescriptor GetDefaultProperty() => null;
            public object GetEditor(Type editorBaseType) => null;
            public EventDescriptorCollection GetEvents() => EventDescriptorCollection.Empty;
            public EventDescriptorCollection GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;
            public PropertyDescriptorCollection GetProperties()
            {
                return GetProperties(new Attribute[0]);
            }

            private bool TryGetRgbColumnIndices(List<TableData.Column> columns, out int rIndex, out int gIndex, out int bIndex)
            {
                rIndex = gIndex = bIndex = -1;
                for (int i = 0; i < columns.Count - 2; i++)
                {
                    if (columns[i].NameHash == 0x050C5D4D &&
                        columns[i + 1].NameHash == 0x050C5D58 &&
                        columns[i + 2].NameHash == 0x050C5D5D)
                    {
                        rIndex = i;
                        gIndex = i + 1;
                        bIndex = i + 2;
                        return true;
                    }
                }
                return false;
            }

            private class LocalizedTextPropertyDescriptor : PropertyDescriptor
            {
                public LocalizedTextPropertyDescriptor() : base("Localized Text", new Attribute[] { new DescriptionAttribute("Text from loaded database") }) { }
                public override Type ComponentType => typeof(DynamicRowWrapper);
                public override bool IsReadOnly => false;
                public override Type PropertyType => typeof(string);

                public override object GetValue(object component)
                {
                    var wrapper = component as DynamicRowWrapper;
                    return wrapper?.LocalizedText;
                }

                public override void SetValue(object component, object value)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null)
                    {
                        wrapper.LocalizedText = value as string;
                        OnValueChanged(component, EventArgs.Empty);
                    }
                }

                public override bool CanResetValue(object component) => false;
                public override void ResetValue(object component) { }
                public override bool ShouldSerializeValue(object component) => false;
            }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                List<PropertyDescriptor> props = new List<PropertyDescriptor>();
                if (TryGetRgbColumnIndices(columns, out int rIdx, out int gIdx, out int bIdx))
                {
                    string baseName = "Color"; 
                    string description = $"RGB color (from columns {editor.GetColumnName(columns[rIdx].NameHash)}, {editor.GetColumnName(columns[gIdx].NameHash)}, {editor.GetColumnName(columns[bIdx].NameHash)})";
                    props.Add(new RgbColorPropertyDescriptor(baseName, rIdx, gIdx, bIdx, description, editor));
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (i == rIdx || i == gIdx || i == bIdx)
                            continue;
                        var col = columns[i];
                        string name = editor.GetColumnName(col.NameHash);
                        string desc = editor.GetColumnDescription(col.NameHash) ?? "";
                        Type valueType = TableData.GetValueTypeForColumnType(col.Type);
                        props.Add(new RowPropertyDescriptor(name, valueType, i, desc));
                    }
                }
                else
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var col = columns[i];
                        string name = editor.GetColumnName(col.NameHash);
                        string desc = editor.GetColumnDescription(col.NameHash) ?? "";
                        if (col.Type == TableData.ColumnType.Color &&
                            (col.NameHash == 0xA0979CFC || col.NameHash == 0xA0979CFE || col.NameHash == 0xA0979CFF))
                        {
                            props.Add(new ColorPropertyDescriptor(name, i, desc));
                        }
                        else
                        {
                            Type valueType = TableData.GetValueTypeForColumnType(col.Type);
                            props.Add(new RowPropertyDescriptor(name, valueType, i, desc));
                        }
                    }
                }
                if (editor.textDatabase != null && !string.IsNullOrEmpty(editor.selectedKeyColumnName))
                {
                    bool keyColumnExists = false;
                    foreach (var col in columns)
                    {
                        if (editor.GetColumnName(col.NameHash) == editor.selectedKeyColumnName)
                        {
                            keyColumnExists = true;
                            break;
                        }
                    }
                    if (keyColumnExists)
                    {
                        props.Add(new LocalizedTextPropertyDescriptor());
                    }
                }
                return new PropertyDescriptorCollection(props.ToArray());
            }
            private class ColorPropertyDescriptor : PropertyDescriptor
            {
                private int columnIndex;

                public ColorPropertyDescriptor(string name, int index, string description) : base(name, new Attribute[] {new DescriptionAttribute(description), new EditorAttribute(typeof(ColorEditor), typeof(UITypeEditor))})
                {
                    columnIndex = index;
                }

                public override Type ComponentType => typeof(DynamicRowWrapper);
                public override bool IsReadOnly => false;
                public override Type PropertyType => typeof(Color);

                public override object GetValue(object component)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null && wrapper.row.Values.Count > columnIndex)
                    {
                        string colorStr = wrapper.row.Values[columnIndex] as string;
                        if (!string.IsNullOrEmpty(colorStr))
                        {
                            string[] parts = colorStr.Split(' ');
                            if (parts.Length == 3 &&
                                float.TryParse(parts[0], out float r) &&
                                float.TryParse(parts[1], out float g) &&
                                float.TryParse(parts[2], out float b))
                            {
                                return Color.FromArgb(
                                    (int)(r * 255),
                                    (int)(g * 255),
                                    (int)(b * 255));
                            }
                        }
                    }
                    return Color.Black;
                }

                public override void SetValue(object component, object value)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null && wrapper.row.Values.Count > columnIndex && value is Color color)
                    {
                        string newValue = $"{color.R / 255.0f} {color.G / 255.0f} {color.B / 255.0f}";
                        wrapper.row.Values[columnIndex] = newValue;
                        wrapper.editor.bIsFileEdited = true;
                        wrapper.editor.Text = $"{wrapper.editor.FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
                        OnValueChanged(component, EventArgs.Empty);
                    }
                }

                public override bool CanResetValue(object component) => false;
                public override void ResetValue(object component) { }
                public override bool ShouldSerializeValue(object component) => false;
            }

            public object GetPropertyOwner(PropertyDescriptor pd) => this;

            private class RowPropertyDescriptor : PropertyDescriptor
            {
                private int columnIndex;
                private Type propertyType;

                public RowPropertyDescriptor(string name, Type type, int index, string description) : base(name, new Attribute[] { new DescriptionAttribute(description) })
                {
                    columnIndex = index;
                    propertyType = type;
                }
                public override Type ComponentType => typeof(DynamicRowWrapper);
                public override bool IsReadOnly => false;
                public override Type PropertyType => propertyType;
                public override object GetValue(object component)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null && wrapper.row.Values.Count > columnIndex)
                        return wrapper.row.Values[columnIndex];
                    return null;
                }

                public override void SetValue(object component, object value)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null && wrapper.row.Values.Count > columnIndex)
                    {
                        wrapper.row.Values[columnIndex] = value;
                        wrapper.editor.bIsFileEdited = true;
                        wrapper.editor.Text = $"{wrapper.editor.FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
                        OnValueChanged(component, EventArgs.Empty);
                    }
                }

                public override bool CanResetValue(object component) => false;
                public override void ResetValue(object component) { }
                public override bool ShouldSerializeValue(object component) => false;
            }
            private class RgbColorPropertyDescriptor : PropertyDescriptor
            {
                private int rIndex, gIndex, bIndex;
                private TableEditor editor;

                public RgbColorPropertyDescriptor(string name, int rIdx, int gIdx, int bIdx, string description, TableEditor editor) : base(name, new Attribute[] { new DescriptionAttribute(description), new EditorAttribute(typeof(ColorEditor), typeof(UITypeEditor)) })
                {
                    this.rIndex = rIdx;
                    this.gIndex = gIdx;
                    this.bIndex = bIdx;
                    this.editor = editor;
                }

                public override Type ComponentType => typeof(DynamicRowWrapper);
                public override bool IsReadOnly => false;
                public override Type PropertyType => typeof(Color);
                public override object GetValue(object component)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null && wrapper.row.Values.Count > Math.Max(rIndex, Math.Max(gIndex, bIndex)))
                    {
                        object rObj = wrapper.row.Values[rIndex];
                        object gObj = wrapper.row.Values[gIndex];
                        object bObj = wrapper.row.Values[bIndex];

                        float r = 0, g = 0, b = 0;

                        if (rObj is float rf) r = rf;
                        else if (rObj is int ri) r = ri / 255.0f;
                        else if (rObj is uint rui) r = rui / 255.0f;
                        else if (rObj is string rs) float.TryParse(rs, out r);

                        if (gObj is float gf) g = gf;
                        else if (gObj is int gi) g = gi / 255.0f;
                        else if (gObj is uint gui) g = gui / 255.0f;
                        else if (gObj is string gs) float.TryParse(gs, out g);

                        if (bObj is float bf) b = bf;
                        else if (bObj is int bi) b = bi / 255.0f;
                        else if (bObj is uint bui) b = bui / 255.0f;
                        else if (bObj is string bs) float.TryParse(bs, out b);

                        return Color.FromArgb(
                            (int)(Math.Clamp(r, 0, 1) * 255),
                            (int)(Math.Clamp(g, 0, 1) * 255),
                            (int)(Math.Clamp(b, 0, 1) * 255));
                    }
                    return Color.Black;
                }

                public override void SetValue(object component, object value)
                {
                    var wrapper = component as DynamicRowWrapper;
                    if (wrapper != null && value is Color color)
                    {
                        object rValue, gValue, bValue;
                        Type originalType = wrapper.row.Values[rIndex]?.GetType() ?? typeof(float);

                        if (originalType == typeof(float))
                        {
                            rValue = color.R / 255.0f;
                            gValue = color.G / 255.0f;
                            bValue = color.B / 255.0f;
                        }
                        else if (originalType == typeof(int) || originalType == typeof(uint))
                        {
                            rValue = (int)color.R;
                            gValue = (int)color.G;
                            bValue = (int)color.B;
                        }
                        else
                        {
                            rValue = $"{color.R / 255.0f}";
                            gValue = $"{color.G / 255.0f}";
                            bValue = $"{color.B / 255.0f}";
                        }

                        wrapper.row.Values[rIndex] = rValue;
                        wrapper.row.Values[gIndex] = gValue;
                        wrapper.row.Values[bIndex] = bValue;

                        editor.bIsFileEdited = true;
                        editor.Text = $"{editor.FileName} - {Language.GetString("$TABLE_EDITOR_TITLE")}*";
                        OnValueChanged(component, EventArgs.Empty);
                    }
                }

                public override bool CanResetValue(object component) => false;
                public override void ResetValue(object component) { }
                public override bool ShouldSerializeValue(object component) => false;
            }
        }
    }
}