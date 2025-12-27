using ResourceTypes.Speech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers;
using Utils.Helpers.Reflection;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class SpeechEditor : Form
    {
        private FileInfo speechFile;
        private SpeechFile speechData;
        private bool bIsFileEdited = false;
        public static Dictionary<int, string> SubtitleDictionary { get; private set; } = new Dictionary<int, string>();
        public SpeechEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            speechFile = file;
            speechData = new SpeechFile(file);
            BuildData();
            LoadSubtitles();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Speech editor.");
            SearchBox.KeyDown += SearchBox_KeyDown;
        }

        private void Localise()
        {
            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_SaveToXML.Text = Language.GetString("$EXPORT_XML");
            Button_LoadFromXML.Text = Language.GetString("$IMPORT_XML");
            Button_Edit.Text = Language.GetString("$EDIT");
        }
        private void LoadSubtitles()
        {
            foreach (var item in speechData.SpeechItems)
            {
                item.Subtitle = string.Empty;
            }

            string speechTablesDir = Path.Combine(Path.GetDirectoryName(speechFile.FullName), "tables", "Speech");
            if (!Directory.Exists(speechTablesDir)) return;

            var allFiles = Directory.GetFiles(speechTablesDir, "Script_*");
            foreach (string file in allFiles)
            {
                if (!Path.GetFileName(file).Contains("_subtitles")) continue;

                try
                {
                    foreach (string line in File.ReadAllLines(file))
                    {
                        if (string.IsNullOrWhiteSpace(line) || !line.Contains(':')) continue;
                        var parts = line.Split(new[] { ':' }, 2);
                        if (int.TryParse(parts[0].Trim(), out int id))
                        {
                            var item = speechData.SpeechItems.FirstOrDefault(x => x.Unk0 == id);
                            if (item != null)
                            {
                                item.Subtitle = parts[1].Trim();
                            }
                        }
                    }
                }
                catch {}
            }
        }
        private void SaveSubtitles()
        {
            string speechTablesDir = Path.Combine(Path.GetDirectoryName(speechFile.FullName), "tables", "Speech");
            if (!Directory.Exists(speechTablesDir)) return;

            var groups = new Dictionary<string, List<SpeechFile.SpeechItemInfo>>();

            foreach (var item in speechData.SpeechItems)
            {
                if (string.IsNullOrEmpty(item.ItemName)) continue;
                string speechType = item.ItemName.Split('_')[0];           
                string matchedFile = FindMatchingSubtitleFile(speechTablesDir, speechType);

                if (matchedFile == null) continue;

                string key = matchedFile;
                if (!groups.ContainsKey(key))
                    groups[key] = new List<SpeechFile.SpeechItemInfo>();
                groups[key].Add(item);
            }
            foreach (var kvp in groups)
            {
                string filePath = kvp.Key;
                var items = kvp.Value;

                var lines = new List<string>();
                foreach (var item in items.OrderBy(x => x.Unk0))
                {
                    if (!string.IsNullOrEmpty(item.Subtitle))
                    {
                        lines.Add($"{item.Unk0}:{item.Subtitle}");
                    }
                }

                File.WriteAllLines(filePath, lines);
            }
        }
        private string FindMatchingSubtitleFile(string dir, string speechType)
        {
            var candidates = Directory.GetFiles(dir, "Script_*")
                .Where(f => Path.GetFileName(f).Contains($"_{speechType}_"))
                .ToArray();
            return candidates.FirstOrDefault();
        }

        private void BuildData()
        {
            TreeView_Speech.Nodes.Clear();
            Grid_Speech.SelectedObject = null;

            var itemsByType = new Dictionary<string, List<SpeechFile.SpeechItemInfo>>();
            foreach (var item in speechData.SpeechItems)
            {
                if (string.IsNullOrEmpty(item.ItemName)) continue;
                string type = item.ItemName.Split('_')[0];
                if (!itemsByType.ContainsKey(type))
                    itemsByType[type] = new List<SpeechFile.SpeechItemInfo>();
                itemsByType[type].Add(item);
            }

            foreach (var type in speechData.SpeechTypes)
            {
                TreeNode node = new TreeNode($"{type.SpeechType} ({type.Folder})");
                node.Tag = type;

                if (itemsByType.TryGetValue(type.SpeechType, out var items))
                {
                    foreach (var item in items)
                    {
                        TreeNode itemNode = new TreeNode(item.ItemName);
                        itemNode.Tag = item;
                        node.Nodes.Add(itemNode);
                    }
                }

                TreeView_Speech.Nodes.Add(node);
            }
        }

        private void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(speechFile.FullName, FileMode.Create)))
            {
                speechData.WriteToFile(writer);
            }
            SaveSubtitles();
            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            speechData = new SpeechFile(speechFile);
            BuildData();
            LoadSubtitles();
            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Speech.SelectedObject = e.Node.Tag;
        }

        private void Grid_Speech_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name") TreeView_Speech.SelectedNode.Text = e.ChangedItem.Value.ToString();
            Text = Language.GetString("$SPEECH_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void SpeechEditor_Closing(object sender, FormClosingEventArgs e)
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
        private void Button_Add_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = TreeView_Speech.SelectedNode;
            if (selectedNode?.Tag is SpeechFile.SpeechTypeInfo typeInfo)
            {
                var newItem = new SpeechFile.SpeechItemInfo
                {
                    ItemName = $"{typeInfo.SpeechType}_newItem",
                    Unk0 = GetNextAvailableUnk0(), 
                    Unk2 = 1,
                    Unk3 = 0,
                    Unk5 = 31,
                    Unk6 = 0,
                    Unk4 = new byte[0]
                };

                var list = speechData.SpeechItems.ToList();
                list.Add(newItem);
                speechData.SpeechItems = list.ToArray();
                TreeNode itemNode = new TreeNode(newItem.ItemName) { Tag = newItem };
                selectedNode.Nodes.Add(itemNode);
                itemNode.EnsureVisible();
                TreeView_Speech.SelectedNode = itemNode;

                bIsFileEdited = true;
                Text = Language.GetString("$SPEECH_EDITOR_TITLE") + "*";
            }
            else
            {
                MessageBox.Show("Please select a speech type node to add an item under.");
            }
        }
        private int GetNextAvailableUnk0()
        {
            if (speechData.SpeechItems.Length == 0) return 1000;
            return speechData.SpeechItems.Max(x => x.Unk0) + 1;
        }
        private void Button_Delete_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = TreeView_Speech.SelectedNode;
            if (selectedNode?.Tag is SpeechFile.SpeechItemInfo item)
            {
                var list = speechData.SpeechItems.ToList();
                list.Remove(item);
                speechData.SpeechItems = list.ToArray();
                selectedNode.Remove();

                bIsFileEdited = true;
                Text = Language.GetString("$SPEECH_EDITOR_TITLE") + "*";
            }
            else if (selectedNode?.Tag is SpeechFile.SpeechTypeInfo)
            {
                MessageBox.Show("Cannot delete speech type nodes.");
            }
            else
            {
                MessageBox.Show("Select a speech item to delete.");
            }
        }
        private void OnSaveToXMLClicked(object sender, System.EventArgs e)
        {
            if (FileSaveDialog_SelectXML.ShowDialog() == DialogResult.OK)
            {
                XElement RootElement = ReflectionHelpers.ConvertPropertyToXML<SpeechFile>(speechData);
                RootElement.Save(FileSaveDialog_SelectXML.FileName);
            }
        }

        private void OnLoadFromXMLClicked(object sender, System.EventArgs e)
        {
            if (FileOpenDialog_SelectXML.ShowDialog() == DialogResult.OK)
            {
                XElement RootElement = XElement.Load(FileOpenDialog_SelectXML.FileName);
                speechData = ReflectionHelpers.ConvertToPropertyFromXML<SpeechFile>(RootElement);
                BuildData();
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
            TreeNode foundNode = FindNode(TreeView_Speech.Nodes, query);
            if (foundNode != null)
            {
                TreeView_Speech.SelectedNode = foundNode;
                TreeView_Speech.Focus();
                foundNode.EnsureVisible();
            }
            else { }
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

        private void Button_Save_Click(object sender, System.EventArgs e) => Save();
        private void Button_Reload_Click(object sender, System.EventArgs e) => Reload();
        private void Button_Exit_Click(object sender, System.EventArgs e) => Close();
    }
}