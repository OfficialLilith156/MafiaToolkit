using Mafia2Tool;
using Mafia2Tool.Forms;
using ResourceTypes.FrameNameTable;
using ResourceTypes.FrameResource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class MultiSelectParentWindow : Form
    {

        private TreeView objectTree;
        private RadioButton radioParent1;
        private RadioButton radioParent2;
        private Button btnOK;
        private Button btnCancel;
        private FrameResource frameResource;
        private FrameNameTable nameTable;
        public List<FrameObjectBase> SelectedObjects { get; private set; } = new List<FrameObjectBase>();
        public ParentInfo.ParentType SelectedParentType { get; private set; }

        public MultiSelectParentWindow(FrameResource frameResource, FrameNameTable nameTable)
        {
            this.frameResource = frameResource;
            this.nameTable = nameTable;
            SetupUI();
            BuildTree();
        }

        private void SetupUI()
        {
            this.Text = "Batch Set Parent";
            this.Size = new System.Drawing.Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            objectTree = new TreeView
            {
                Dock = DockStyle.Fill,
                CheckBoxes = true,
                ShowLines = false,
                ShowRootLines = false,
                ShowPlusMinus = true
            };

            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                Padding = new Padding(10)
            };

            radioParent1 = new RadioButton
            {
                Text = "Parent Index 1",
                Location = new System.Drawing.Point(10, 10),
                Checked = true
            };

            radioParent2 = new RadioButton
            {
                Text = "Parent Index 2",
                Location = new System.Drawing.Point(10, 40)
            };

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new System.Drawing.Size(75, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnOK.Location = new System.Drawing.Point(bottomPanel.Width - 170, bottomPanel.Height - 35);

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Size = new System.Drawing.Size(75, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnCancel.Location = new System.Drawing.Point(bottomPanel.Width - 85, bottomPanel.Height - 35);

            bottomPanel.Resize += (s, e) =>
            {
                btnOK.Location = new System.Drawing.Point(bottomPanel.Width - 170, bottomPanel.Height - 35);
                btnCancel.Location = new System.Drawing.Point(bottomPanel.Width - 85, bottomPanel.Height - 35);
            };

            bottomPanel.Controls.Add(radioParent1);
            bottomPanel.Controls.Add(radioParent2);
            bottomPanel.Controls.Add(btnOK);
            bottomPanel.Controls.Add(btnCancel);

            this.Controls.Add(bottomPanel);
            this.Controls.Add(objectTree);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void BuildTree()
        {
            TreeNode root = frameResource.BuildTree(nameTable);
            foreach (TreeNode node in root.Nodes)
            {
                TreeNode clone = CloneNode(node);
                objectTree.Nodes.Add(clone);
            }
            //objectTree.ExpandAll();
        }

        private TreeNode CloneNode(TreeNode original)
        {
            string text = (original.Tag is FrameObjectBase obj) ? obj.ToString() : original.Text;
            TreeNode clone = new TreeNode(text)
            {
                Tag = original.Tag,
                Checked = false
            };
            foreach (TreeNode child in original.Nodes)
            {
                clone.Nodes.Add(CloneNode(child));
            }
            return clone;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                CollectCheckedObjects(objectTree.Nodes, SelectedObjects);
                SelectedParentType = radioParent1.Checked
                    ? ParentInfo.ParentType.ParentIndex1
                    : ParentInfo.ParentType.ParentIndex2;
            }
            base.OnFormClosing(e);
        }

        private void CollectCheckedObjects(TreeNodeCollection nodes, List<FrameObjectBase> list)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && node.Tag is FrameObjectBase obj)
                {
                    list.Add(obj);
                }
                CollectCheckedObjects(node.Nodes, list);
            }
        }
    }
}





