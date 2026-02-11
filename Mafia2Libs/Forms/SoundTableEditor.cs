using ApexSDK;
using ResourceTypes.SoundTable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Mafia2Tool.Forms
{
    public partial class SoundTableEditor : Form
    {
        private string filePath;
        private string originalStblPath;
        private bool isModified = false;


        public SoundTableEditor(string stblFilePath)
        {
            this.originalStblPath = stblFilePath;
            this.filePath = stblFilePath;
            InitializeComponent();
            LoadSoundTable(filePath);
        }

        private void LoadSoundTable(string path)
        {
            try
            {
                soundTable = new SoundTable();
                using (var stream = new MemoryStream(File.ReadAllBytes(path)))
                {
                    soundTable.ReadFromFile(stream, false);
                }
                BuildTreeView();
                isModified = false;
                this.Text = "Sound Table Editor - " + Path.GetFileName(path);
                statusLabel.Text = $"Loaded: {Path.GetFileName(path)} - {GetStats()}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading STBL file: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Load failed";
            }
        }

        private string GetStats()
        {
            int totalSounds = 0;
            foreach (var category in soundTable.FSBGroups)
            {
                totalSounds += category.Variants?.Length ?? 0;
            }

            return $"Categories: {soundTable.FSBGroups?.Length ?? 0}, " +
                   $"Entry0: {soundTable.Entry0s?.Length ?? 0}, " +
                   $"Entry1: {soundTable.Entry1s?.Length ?? 0}, " +
                   $"Entry2: {soundTable.Entry2s?.Length ?? 0}, " +
                   $"Total Sounds: {totalSounds}";
        }

        private void OnSaveStbl(object sender, EventArgs e)
        {
            if (soundTable == null || string.IsNullOrEmpty(originalStblPath))
            {
                MessageBox.Show("No file is loaded or original path is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string backupPath = originalStblPath + ".backup";
                File.Copy(originalStblPath, backupPath, true);
                soundTable.WriteToFile(originalStblPath, false);
                isModified = false;
                this.Text = "Sound Table Editor - " + Path.GetFileName(originalStblPath);
                statusLabel.Text = $"Saved to {Path.GetFileName(originalStblPath)} (backup created)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving STBL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Save failed";
            }
        }

        private void BuildTreeView()
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            TreeNode rootNode = new TreeNode("Sound Table");
            rootNode.Tag = soundTable;
            rootNode.ImageKey = "root";

            if (soundTable.Entry0s?.Length > 0)
            {
                TreeNode entry0Node = new TreeNode($"Entry0s [{soundTable.Entry0s.Length}]");
                entry0Node.Tag = soundTable.Entry0s;
                entry0Node.ImageKey = "entry0s";

                for (int i = 0; i < soundTable.Entry0s.Length; i++)
                {
                    var entry = soundTable.Entry0s[i];
                    TreeNode node = new TreeNode($"Entry0 #{i}: Unk0={entry.Unk0}, Unk1={entry.Unk1}");
                    node.Tag = entry;
                    node.ImageKey = "entry0";
                    node.ToolTipText = $"Unk3 Length: {entry.Unk3?.Length ?? 0}";
                    entry0Node.Nodes.Add(node);
                }
                rootNode.Nodes.Add(entry0Node);
            }
            if (soundTable.Entry1s?.Length > 0)
            {
                TreeNode entry1Node = new TreeNode($"Entry1s [{soundTable.Entry1s.Length}]");
                entry1Node.Tag = soundTable.Entry1s;
                entry1Node.ImageKey = "entry1s";

                for (int i = 0; i < soundTable.Entry1s.Length; i++)
                {
                    var entry = soundTable.Entry1s[i];
                    string info = $"Entry1 #{i}: Unk0={entry.Unk0}, Unk1={entry.Unk1}, Unk2={entry.Unk2:F2}";
                    TreeNode node = new TreeNode(info);
                    node.Tag = entry;
                    node.ImageKey = "entry1";
                    node.ToolTipText = $"Unk3={entry.Unk3:F2}, Unk4={entry.Unk4:F2}, Unk6={entry.Unk6}";
                    entry1Node.Nodes.Add(node);
                }
                rootNode.Nodes.Add(entry1Node);
            }
            if (soundTable.Entry2s?.Length > 0)
            {
                TreeNode entry2Node = new TreeNode($"Entry2s [{soundTable.Entry2s.Length}]");
                entry2Node.Tag = soundTable.Entry2s;
                entry2Node.ImageKey = "entry2s";

                for (int i = 0; i < soundTable.Entry2s.Length; i++)
                {
                    var entry = soundTable.Entry2s[i];
                    TreeNode node = new TreeNode($"Entry2 #{i}: Unk0={entry.Unk0}, Unk1={entry.Unk1}");
                    node.Tag = entry;
                    node.ImageKey = "entry2";
                    entry2Node.Nodes.Add(node);
                }
                rootNode.Nodes.Add(entry2Node);
            }
            if (soundTable.FSBGroups?.Length > 0)
            {
                TreeNode groupsNode = new TreeNode($"Sound Categories [{soundTable.FSBGroups.Length}]");
                groupsNode.Tag = soundTable.FSBGroups;
                groupsNode.ImageKey = "categories";

                foreach (var category in soundTable.FSBGroups)
                {
                    TreeNode categoryNode = new TreeNode($"[{category.ID}] {category.Name} (Unk0={category.Unk0:F2})");
                    categoryNode.Tag = category;
                    categoryNode.ImageKey = "category";

                    if (category.Variants?.Length > 0)
                    {
                        foreach (var sound in category.Variants)
                        {
                            string soundInfo = $"[{sound.ID}] {sound.Name} (Entry1={sound.Entry1ToUse})";
                            TreeNode soundNode = new TreeNode(soundInfo);
                            soundNode.Tag = sound;
                            soundNode.ImageKey = "sound";

                            if (sound.Files?.Length > 0)
                            {
                                foreach (var file in sound.Files)
                                {
                                    TreeNode fileNode = new TreeNode($"File: {Path.GetFileName(file.FSBPath)}");
                                    fileNode.Tag = file;
                                    fileNode.ImageKey = "file";
                                    fileNode.ToolTipText = $"Volume: {file.Volume:F2}, Unk0: {file.Unk0}";
                                    soundNode.Nodes.Add(fileNode);
                                }
                            }
                            categoryNode.Nodes.Add(soundNode);
                        }
                    }
                    groupsNode.Nodes.Add(categoryNode);
                }
                rootNode.Nodes.Add(groupsNode);
            }
            treeView.Nodes.Add(rootNode);
            rootNode.Expand();
            treeView.EndUpdate();
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag != null)
            {
                propertyGrid.SelectedObject = CreateWrapper(e.Node.Tag);
                propertyGrid.Refresh();
            }
            else
            {
                propertyGrid.SelectedObject = null;
            }
        }

        private object CreateWrapper(object obj)
        {
            if (obj is Entry0 entry0)
                return new Entry0Wrapper(entry0);
            if (obj is Entry1 entry1)
                return new Entry1Wrapper(entry1);
            if (obj is Entry2 entry2)
                return new Entry2Wrapper(entry2);
            if (obj is SoundCategory category)
                return new SoundCategoryWrapper(category);
            if (obj is Sound sound)
                return new SoundWrapper(sound);
            if (obj is SoundFile file)
                return new SoundFileWrapper(file);
            return obj;
        }

        private void OnAddItem(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null) return;
            var node = treeView.SelectedNode;
            var parentTag = node.Tag;
            try
            {
                if (parentTag is SoundCategory category)
                {
                    var variants = category.Variants ?? Array.Empty<Sound>();
                    Array.Resize(ref variants, variants.Length + 1);
                    variants[^1] = new Sound
                    {
                        ID = 0,
                        Name = "NewSound",
                        Entry1ToUse = 0,
                        Files = Array.Empty<SoundFile>()
                    };
                    category.Variants = variants;
                    RebuildNode(node);
                    isModified = true;
                }
                else if (parentTag is Sound sound)
                {
                    var files = sound.Files ?? Array.Empty<SoundFile>();
                    Array.Resize(ref files, files.Length + 1);
                    files[^1] = new SoundFile
                    {
                        FSBPath = "new_sound.fsb",
                        Volume = 1.0f,
                        Unk0 = 0
                    };
                    sound.Files = files;
                    RebuildNode(node);
                    isModified = true;
                }
                else if (parentTag == soundTable)
                {
                    var groups = soundTable.FSBGroups ?? Array.Empty<SoundCategory>();
                    Array.Resize(ref groups, groups.Length + 1);
                    groups[^1] = new SoundCategory
                    {
                        ID = 0,
                        Name = "NewCategory",
                        Unk0 = 1.0f,
                        Variants = Array.Empty<Sound>()
                    };
                    soundTable.FSBGroups = groups;
                    BuildTreeView();
                    isModified = true;
                }
                else
                {
                    MessageBox.Show("Adding items here is not supported.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDeleteItem(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent == null) return;
            var node = treeView.SelectedNode;
            var parent = node.Parent;
            var parentTag = parent.Tag;
            var index = node.Index;
            try
            {
                if (parentTag is SoundCategory category)
                {
                    var list = new List<Sound>(category.Variants);
                    list.RemoveAt(index);
                    category.Variants = list.ToArray();
                    RebuildNode(parent);
                    isModified = true;
                }
                else if (parentTag is Sound sound)
                {
                    var list = new List<SoundFile>(sound.Files);
                    list.RemoveAt(index);
                    sound.Files = list.ToArray();
                    RebuildNode(parent);
                    isModified = true;
                }
                else if (parentTag is Sound[])
                {
                    var categoryNode = parent.Parent;
                    if (categoryNode?.Tag is SoundCategory cat)
                    {
                        var list = new List<Sound>(cat.Variants);
                        list.RemoveAt(index);
                        cat.Variants = list.ToArray();
                        RebuildNode(categoryNode);
                        isModified = true;
                    }
                }
                else if (parentTag is SoundCategory[] && parent.Parent?.Tag == soundTable)
                {
                    var list = new List<SoundCategory>(soundTable.FSBGroups);
                    list.RemoveAt(index);
                    soundTable.FSBGroups = list.ToArray();
                    BuildTreeView();
                    isModified = true;
                }
                else
                {
                    MessageBox.Show("Deletion not supported here.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RebuildNode(TreeNode node)
        {
            var originalTag = node.Tag;
            node.Nodes.Clear();

            if (originalTag is SoundCategory category)
            {
                if (category.Variants?.Length > 0)
                {
                    foreach (var sound in category.Variants)
                    {
                        string soundInfo = $"[{sound.ID}] {sound.Name} (Entry1={sound.Entry1ToUse})";
                        TreeNode soundNode = new TreeNode(soundInfo) { Tag = sound, ImageKey = "sound" };

                        if (sound.Files?.Length > 0)
                        {
                            foreach (var file in sound.Files)
                            {
                                TreeNode fileNode = new TreeNode($"File: {Path.GetFileName(file.FSBPath)}")
                                {
                                    Tag = file,
                                    ImageKey = "file",
                                    ToolTipText = $"Volume: {file.Volume:F2}, Unk0: {file.Unk0}"
                                };
                                soundNode.Nodes.Add(fileNode);
                            }
                        }
                        node.Nodes.Add(soundNode);
                    }
                }
                node.Text = $"[{category.ID}] {category.Name} (Unk0={category.Unk0:F2}) [Variants: {category.Variants?.Length ?? 0}]";
            }
            else if (originalTag is Sound sound)
            {
                if (sound.Files?.Length > 0)
                {
                    foreach (var file in sound.Files)
                    {
                        TreeNode fileNode = new TreeNode($"File: {Path.GetFileName(file.FSBPath)}")
                        {
                            Tag = file,
                            ImageKey = "file",
                            ToolTipText = $"Volume: {file.Volume:F2}, Unk0: {file.Unk0}"
                        };
                        node.Nodes.Add(fileNode);
                    }
                }
                node.Text = $"[{sound.ID}] {sound.Name} (Entry1={sound.Entry1ToUse}) [Files: {sound.Files?.Length ?? 0}]";
            }
            node.Expand();
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Entry0Wrapper
    {
        private Entry0 entry;
        public Entry0Wrapper(Entry0 entry) { this.entry = entry; }

        [Category("Basic")]
        [Description("Unknown byte 0")]
        public byte Unk0 { get => entry.Unk0; set => entry.Unk0 = value; }

        [Category("Basic")]
        [Description("Unknown byte 1 - determines array size")]
        public byte Unk1 { get => entry.Unk1; set => entry.Unk1 = value; }

        [Category("Data")]
        [Description("Float array data")]
        [TypeConverter(typeof(ArrayConverter))]
        public float[] Unk3 { get => entry.Unk3; set => entry.Unk3 = value; }

        public override string ToString() => $"Entry0: Unk0={Unk0}, Unk1={Unk1}, ArraySize={Unk3?.Length ?? 0}";
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Entry1Wrapper
    {
        private Entry1 entry;

        public Entry1Wrapper(Entry1 entry) { this.entry = entry; }

        [Category("Basic")]
        public byte Unk0 { get => entry.Unk0; set => entry.Unk0 = value; }

        [Category("Basic")]
        public byte Unk1 { get => entry.Unk1; set => entry.Unk1 = value; }

        [Category("Float Values")]
        public float Unk2 { get => entry.Unk2; set => entry.Unk2 = value; }

        [Category("Float Values")]
        public float Unk3 { get => entry.Unk3; set => entry.Unk3 = value; }

        [Category("Float Values")]
        public float Unk4 { get => entry.Unk4; set => entry.Unk4 = value; }

        [Category("Float Values")]
        public float Unk5 { get => entry.Unk5; set => entry.Unk5 = value; }

        [Category("Basic")]
        public byte Unk6 { get => entry.Unk6; set => entry.Unk6 = value; }

        [Category("Extra")]
        public float Unk7 { get => entry.Unk7; set => entry.Unk7 = value; }

        [Category("Extra")]
        public float Unk8 { get => entry.Unk8; set => entry.Unk8 = value; }

        [Category("Extra")]
        public float Unk9 { get => entry.Unk9; set => entry.Unk9 = value; }

        [Category("Extra")]
        public byte Unk10 { get => entry.Unk10; set => entry.Unk10 = value; }

        public override string ToString() => $"Entry1: Unk0={Unk0}, Unk1={Unk1}, Unk2={Unk2:F2}";
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Entry2Wrapper
    {
        private Entry2 entry;

        public Entry2Wrapper(Entry2 entry) { this.entry = entry; }

        [Category("Values")]
        public uint Unk0 { get => entry.Unk0; set => entry.Unk0 = value; }

        [Category("Values")]
        public ushort Unk1 { get => entry.Unk1; set => entry.Unk1 = value; }

        public override string ToString() => $"Entry2: {Unk0} -> {Unk1}";
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundCategoryWrapper
    {
        private SoundCategory category;

        public SoundCategoryWrapper(SoundCategory category) { this.category = category; }

        [Category("Basic")]
        public uint ID { get => category.ID; set => category.ID = value; }

        [Category("Basic")]
        public string Name { get => category.Name; set => category.Name = value; }

        [Category("Settings")]
        public float Unk0 { get => category.Unk0; set => category.Unk0 = value; }

        [Category("Content")]
        [TypeConverter(typeof(ArrayConverter))]
        public Sound[] Variants { get => category.Variants; set => category.Variants = value; }

        public override string ToString() => $"Category [{ID}]: {Name}";
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundWrapper
    {
        private Sound sound;

        public SoundWrapper(Sound sound) { this.sound = sound; }

        [Category("Basic")]
        public uint ID { get => sound.ID; set => sound.ID = value; }

        [Category("Basic")]
        public string Name { get => sound.Name; set => sound.Name = value; }

        [Category("Reference")]
        public ushort Entry1ToUse { get => sound.Entry1ToUse; set => sound.Entry1ToUse = value; }

        [Category("Files")]
        [TypeConverter(typeof(ArrayConverter))]
        public SoundFile[] Files { get => sound.Files; set => sound.Files = value; }

        public override string ToString() => $"Sound [{ID}]: {Name}";
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundFileWrapper
    {
        private SoundFile file;

        public SoundFileWrapper(SoundFile file) { this.file = file; }

        [Category("Path")]
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string FSBPath { get => file.FSBPath; set => file.FSBPath = value; }

        [Category("Settings")]
        public byte Unk0 { get => file.Unk0; set => file.Unk0 = value; }

        [Category("Settings")]
        public float Volume { get => file.Volume; set => file.Volume = value; }

        public override string ToString() => Path.GetFileName(FSBPath);
    }
}