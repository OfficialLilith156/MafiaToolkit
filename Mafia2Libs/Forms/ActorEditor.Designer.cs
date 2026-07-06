namespace Mafia2Tool
{
    partial class ActorEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActorEditor));
            ActorGrid = new System.Windows.Forms.PropertyGrid();
            ActorContext = new System.Windows.Forms.ContextMenuStrip(components);
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            ContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            ContextPaste = new System.Windows.Forms.ToolStripMenuItem();
            Button_MoveUp = new System.Windows.Forms.ToolStripMenuItem();
            Button_MoveDown = new System.Windows.Forms.ToolStripMenuItem();
            dUPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddItemButton = new System.Windows.Forms.ToolStripMenuItem();
            AddDefinitionButton = new System.Windows.Forms.ToolStripMenuItem();
            dataIDFixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            copyEntityBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteEntityBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SearchBox = new System.Windows.Forms.TextBox();
            ActorTreeView = new Mafia2Tool.Controls.MTreeView();
            pasteAllEntityBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyAllEntityBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ActorContext.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ActorGrid
            // 
            ActorGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ActorGrid.Location = new System.Drawing.Point(469, 32);
            ActorGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ActorGrid.Name = "ActorGrid";
            ActorGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            ActorGrid.Size = new System.Drawing.Size(450, 473);
            ActorGrid.TabIndex = 10;
            ActorGrid.PropertyValueChanged += ActorGrid_OnPropertyValueChanged;
            // 
            // ActorContext
            // 
            ActorContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextDelete, ContextCopy, ContextPaste, Button_MoveUp, Button_MoveDown, dUPToolStripMenuItem });
            ActorContext.Name = "SDSContext";
            ActorContext.Size = new System.Drawing.Size(221, 136);
            ActorContext.Opening += ContextMenu_OnOpening;
            // 
            // ContextDelete
            // 
            ContextDelete.Name = "ContextDelete";
            ContextDelete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            ContextDelete.Size = new System.Drawing.Size(220, 22);
            ContextDelete.Text = "$DELETE";
            ContextDelete.Click += ContextDelete_Click;
            // 
            // ContextCopy
            // 
            ContextCopy.Name = "ContextCopy";
            ContextCopy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            ContextCopy.Size = new System.Drawing.Size(220, 22);
            ContextCopy.Text = "$COPY";
            ContextCopy.Click += ContextCopy_Click;
            // 
            // ContextPaste
            // 
            ContextPaste.Name = "ContextPaste";
            ContextPaste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            ContextPaste.Size = new System.Drawing.Size(220, 22);
            ContextPaste.Text = "$PASTE";
            ContextPaste.Click += ContextPaste_Click;
            // 
            // Button_MoveUp
            // 
            Button_MoveUp.Name = "Button_MoveUp";
            Button_MoveUp.ShortcutKeyDisplayString = "";
            Button_MoveUp.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up;
            Button_MoveUp.Size = new System.Drawing.Size(220, 22);
            Button_MoveUp.Text = "$MOVE_UP";
            Button_MoveUp.Click += Button_MoveUp_Clicked;
            // 
            // Button_MoveDown
            // 
            Button_MoveDown.Name = "Button_MoveDown";
            Button_MoveDown.ShortcutKeyDisplayString = "";
            Button_MoveDown.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down;
            Button_MoveDown.Size = new System.Drawing.Size(220, 22);
            Button_MoveDown.Text = "$MOVE_DOWN";
            Button_MoveDown.Click += Button_MoveDown_Clicked;
            // 
            // dUPToolStripMenuItem
            // 
            dUPToolStripMenuItem.Name = "dUPToolStripMenuItem";
            dUPToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            dUPToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            dUPToolStripMenuItem.Text = "Duplicate Item";
            dUPToolStripMenuItem.Click += dUPToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton, toolStripDropDownButton1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // FileButton
            // 
            FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ReloadButton, ExitButton });
            FileButton.Image = (System.Drawing.Image)resources.GetObject("FileButton.Image");
            FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FileButton.Name = "FileButton";
            FileButton.Size = new System.Drawing.Size(47, 22);
            FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            SaveButton.Name = "SaveButton";
            SaveButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveButton.Size = new System.Drawing.Size(165, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveButton_OnClick;
            // 
            // ReloadButton
            // 
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(165, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadButton_OnClick;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(165, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_OnClick;
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddItemButton, AddDefinitionButton, dataIDFixToolStripMenuItem });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(49, 22);
            EditButton.Text = "$EDIT";
            // 
            // AddItemButton
            // 
            AddItemButton.Name = "AddItemButton";
            AddItemButton.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A;
            AddItemButton.Size = new System.Drawing.Size(247, 22);
            AddItemButton.Text = "$ADD_ITEM";
            AddItemButton.Click += AddItemButton_Click;
            // 
            // AddDefinitionButton
            // 
            AddDefinitionButton.Name = "AddDefinitionButton";
            AddDefinitionButton.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            AddDefinitionButton.Size = new System.Drawing.Size(247, 22);
            AddDefinitionButton.Text = "$ADD_DEFINITION";
            AddDefinitionButton.Click += AddDefinitionButton_Click;
            // 
            // dataIDFixToolStripMenuItem
            // 
            dataIDFixToolStripMenuItem.Name = "dataIDFixToolStripMenuItem";
            dataIDFixToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            dataIDFixToolStripMenuItem.Text = "DataID Fix";
            dataIDFixToolStripMenuItem.Click += RenumberButton_Click;
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { copyEntityBranchToolStripMenuItem, pasteEntityBranchToolStripMenuItem, copyAllEntityBranchToolStripMenuItem, pasteAllEntityBranchToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(47, 22);
            toolStripDropDownButton1.Text = "Tools";
            // 
            // copyEntityBranchToolStripMenuItem
            // 
            copyEntityBranchToolStripMenuItem.Name = "copyEntityBranchToolStripMenuItem";
            copyEntityBranchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            copyEntityBranchToolStripMenuItem.Text = "Copy Entity Branch";
            copyEntityBranchToolStripMenuItem.Click += CopyEntityBranch;
            // 
            // pasteEntityBranchToolStripMenuItem
            // 
            pasteEntityBranchToolStripMenuItem.Name = "pasteEntityBranchToolStripMenuItem";
            pasteEntityBranchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            pasteEntityBranchToolStripMenuItem.Text = "Paste Entity Branch";
            pasteEntityBranchToolStripMenuItem.Click += PasteEntityBranch;
            // 
            // SearchBox
            // 
            SearchBox.Location = new System.Drawing.Point(290, 7);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new System.Drawing.Size(152, 23);
            SearchBox.TabIndex = 16;
            // 
            // ActorTreeView
            // 
            ActorTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ActorTreeView.ContextMenuStrip = ActorContext;
            ActorTreeView.Location = new System.Drawing.Point(13, 35);
            ActorTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ActorTreeView.Name = "ActorTreeView";
            ActorTreeView.Size = new System.Drawing.Size(429, 472);
            ActorTreeView.TabIndex = 17;
            ActorTreeView.AfterSelect += OnNodeSelectSelect;
            ActorTreeView.KeyUp += SearchBox_KeyDown;
            // 
            // pasteAllEntityBranchToolStripMenuItem
            // 
            pasteAllEntityBranchToolStripMenuItem.Name = "pasteAllEntityBranchToolStripMenuItem";
            pasteAllEntityBranchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            pasteAllEntityBranchToolStripMenuItem.Text = "Paste All Entity Branch";
            pasteAllEntityBranchToolStripMenuItem.Click += PasteAllBranches_Click;
            // 
            // copyAllEntityBranchToolStripMenuItem
            // 
            copyAllEntityBranchToolStripMenuItem.Name = "copyAllEntityBranchToolStripMenuItem";
            copyAllEntityBranchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            copyAllEntityBranchToolStripMenuItem.Text = "Copy All Entity Branch";
            copyAllEntityBranchToolStripMenuItem.Click += CopyAllBranches_Click;
            // 
            // ActorEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(ActorTreeView);
            Controls.Add(SearchBox);
            Controls.Add(toolStrip1);
            Controls.Add(ActorGrid);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ActorEditor";
            Text = "$ACTOR_EDITOR_TITLE";
            FormClosing += ActorEditor_Closing;
            ActorContext.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PropertyGrid ActorGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip ActorContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem ContextCopy;
        private System.Windows.Forms.ToolStripMenuItem ContextPaste;
        private Mafia2Tool.Controls.MTreeView ActorTreeView;
        private System.Windows.Forms.ToolStripMenuItem Button_MoveUp;
        private System.Windows.Forms.ToolStripMenuItem Button_MoveDown;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.ToolStripMenuItem dUPToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddItemButton;
        private System.Windows.Forms.ToolStripMenuItem AddDefinitionButton;
        private System.Windows.Forms.ToolStripMenuItem dataIDFixToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem copyEntityBranchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteEntityBranchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllEntityBranchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteAllEntityBranchToolStripMenuItem;
    }
}
