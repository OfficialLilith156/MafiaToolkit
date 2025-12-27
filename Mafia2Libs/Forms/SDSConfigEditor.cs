using Microsoft.Win32;
using ResourceTypes.SDSConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using System.IO;

namespace Mafia2Tool.Forms
{
    public partial class SDSConfigEditor : Form
    {
        private SdsConfigFile currentFile;
        private string currentFilePath;

        private class SdsNodeTag
        {
            public object Target { get; }
            public SdsNodeTag(object target) => Target = target;
        }

        public SDSConfigEditor()
        {
            InitializeComponent();
            treeView1.AfterSelect += TreeView1_AfterSelect;
        }

        public void LoadFile(string filePath)
        {
            currentFilePath = filePath;
            currentFile = new SdsConfigFile(filePath);
            BuildTree();
            Text = $"SDS Editor - {Path.GetFileName(filePath)}";
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = (e.Node?.Tag as SdsNodeTag)?.Target;
        }

        private void BuildTree()
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            var root = treeView1.Nodes.Add("SDS Configuration");
            root.Tag = new SdsNodeTag(currentFile);
            foreach (var tmpl in currentFile.Template)
            {
                var tmplNode = new TreeNode($"[Template] {tmpl.Name}") { Tag = new SdsNodeTag(tmpl) };
                root.Nodes.Add(tmplNode);
                foreach (var g in tmpl.Unk02Data)
                {
                    var gNode = new TreeNode($"[Group] {g.Name}") { Tag = new SdsNodeTag(g) };
                    tmplNode.Nodes.Add(gNode);
                }
                foreach (var u3 in tmpl.Unk03Data)
                {
                    var u3Node = new TreeNode($"[Unk03] {u3.Name}") { Tag = new SdsNodeTag(u3) };
                    tmplNode.Nodes.Add(u3Node);
                    foreach (var u4 in u3.Unk04Data)
                    {
                        var u4Node = new TreeNode($"[Unk04] {u4.Name}") { Tag = new SdsNodeTag(u4) };
                        u3Node.Nodes.Add(u4Node);
                        foreach (var u5 in u4.Unk05Data)
                        {
                            var u5Node = new TreeNode($"[Unk05] {u5.Name}") { Tag = new SdsNodeTag(u5) };
                            u4Node.Nodes.Add(u5Node);
                        }
                    }
                }
            }
            root.Expand();
            treeView1.EndUpdate();
        }

        private void SaveInPlace(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath) || currentFile == null) return;
            try
            {
                string backupPath = currentFilePath + ".bak";
                if (File.Exists(currentFilePath))
                {
                    File.Copy(currentFilePath, backupPath, overwrite: true);
                }
                currentFile.WriteToFile(currentFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed Save:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();
    }
}