using System.Windows.Forms;
using Utils.Extensions;

namespace Mafia2Tool
{
    partial class GameExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameExplorer));
            mainContainer = new SplitContainer();
            folderView = new Mafia2Tool.Controls.MTreeView();
            imageBank = new ImageList(components);
            toolStripContainer1 = new ToolStripContainer();
            toolStrip2 = new ToolStrip();
            FolderUpButton = new ToolStripButton();
            FolderPath = new ToolStripTextBox();
            FolderRefreshButton = new ToolStripButton();
            SearchEntryText = new ToolStripTextBox();
            fileListView = new Mafia2Tool.Controls.MListView();
            columnName = new ColumnHeader();
            columnType = new ColumnHeader();
            columnSize = new ColumnHeader();
            columnLastModified = new ColumnHeader();
            GEContext = new ContextMenuStrip(components);
            ContextSDSUnpack = new ToolStripMenuItem();
            ContextSDSPack = new ToolStripMenuItem();
            ContextFileExport = new ToolStripMenuItem();
            ContextFileImport = new ToolStripMenuItem();
            ContextOpenFolder = new ToolStripMenuItem();
            ContextSDSUnpackAll = new ToolStripMenuItem();
            ContextView = new ToolStripMenuItem();
            ContextViewIcon = new ToolStripMenuItem();
            ContextViewDetails = new ToolStripMenuItem();
            ContextViewSmallIcon = new ToolStripMenuItem();
            ContextViewList = new ToolStripMenuItem();
            ContextViewTile = new ToolStripMenuItem();
            ContextForceBigEndian = new ToolStripMenuItem();
            ContextSeperator = new ToolStripSeparator();
            ContextDeleteSelectedFiles = new ToolStripMenuItem();
            ContextUnpackSelectedSDS = new ToolStripMenuItem();
            ContextPackSelectedSDS = new ToolStripMenuItem();
            topContainer = new ToolStripContainer();
            Toolstrip_ButtonContainer = new ToolStrip();
            Button_Settings = new ToolStripButton();
            Button_PackSDS = new ToolStripButton();
            Button_UnpackSDS = new ToolStripButton();
            Button_OpenMapEditor = new ToolStripButton();
            tools = new ToolStrip();
            dropdownFile = new ToolStripDropDownButton();
            OpenGameFolderButton = new ToolStripMenuItem();
            RunGameButton = new ToolStripMenuItem();
            SelectGameButton = new ToolStripMenuItem();
            ExitEditorButton = new ToolStripMenuItem();
            dropdownView = new ToolStripDropDownButton();
            ViewStripMenuIcon = new ToolStripMenuItem();
            ViewStripMenuDetails = new ToolStripMenuItem();
            ViewStripMenuSmallIcon = new ToolStripMenuItem();
            ViewStripMenuList = new ToolStripMenuItem();
            ViewStripMenuTile = new ToolStripMenuItem();
            dropdownTools = new ToolStripDropDownButton();
            OptionsItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            PackCurrentSDSButton = new ToolStripMenuItem();
            UnpackCurrentSDSButton = new ToolStripMenuItem();
            UnpackAllSDSButton = new ToolStripMenuItem();
            dropdownAbout = new ToolStripDropDownButton();
            AboutButton = new ToolStripMenuItem();
            bottomContainer = new ToolStripContainer();
            status = new StatusStrip();
            infoText = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)mainContainer).BeginInit();
            mainContainer.Panel1.SuspendLayout();
            mainContainer.Panel2.SuspendLayout();
            mainContainer.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            toolStrip2.SuspendLayout();
            GEContext.SuspendLayout();
            topContainer.ContentPanel.SuspendLayout();
            topContainer.SuspendLayout();
            Toolstrip_ButtonContainer.SuspendLayout();
            tools.SuspendLayout();
            bottomContainer.ContentPanel.SuspendLayout();
            bottomContainer.SuspendLayout();
            status.SuspendLayout();
            SuspendLayout();
            // 
            // mainContainer
            // 
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.Location = new System.Drawing.Point(0, 63);
            mainContainer.Margin = new Padding(4, 3, 4, 3);
            mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            mainContainer.Panel1.Controls.Add(folderView);
            // 
            // mainContainer.Panel2
            // 
            mainContainer.Panel2.Controls.Add(toolStripContainer1);
            mainContainer.Panel2.Controls.Add(fileListView);
            mainContainer.Size = new System.Drawing.Size(933, 426);
            mainContainer.SplitterDistance = 310;
            mainContainer.SplitterWidth = 5;
            mainContainer.TabIndex = 0;
            // 
            // folderView
            // 
            folderView.Dock = DockStyle.Fill;
            folderView.ImageIndex = 0;
            folderView.ImageList = imageBank;
            folderView.Location = new System.Drawing.Point(0, 0);
            folderView.Margin = new Padding(4, 3, 4, 3);
            folderView.Name = "folderView";
            folderView.SelectedImageIndex = 0;
            folderView.Size = new System.Drawing.Size(310, 426);
            folderView.TabIndex = 0;
            folderView.AfterSelect += FolderViewAfterExpand;
            // 
            // imageBank
            // 
            imageBank.ColorDepth = ColorDepth.Depth32Bit;
            imageBank.ImageStream = (ImageListStreamer)resources.GetObject("imageBank.ImageStream");
            imageBank.TransparentColor = System.Drawing.Color.Transparent;
            imageBank.Images.SetKeyName(0, "folderIcon");
            imageBank.Images.SetKeyName(1, ".sds");
            // 
            // toolStripContainer1
            // 
            toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(toolStrip2);
            toolStripContainer1.ContentPanel.Margin = new Padding(4, 3, 4, 3);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(618, 33);
            toolStripContainer1.Dock = DockStyle.Top;
            toolStripContainer1.LeftToolStripPanelVisible = false;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Margin = new Padding(4, 3, 4, 3);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.RightToolStripPanelVisible = false;
            toolStripContainer1.Size = new System.Drawing.Size(618, 33);
            toolStripContainer1.TabIndex = 1;
            toolStripContainer1.Text = "toolStripContainer1";
            toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // toolStrip2
            // 
            toolStrip2.CanOverflow = false;
            toolStrip2.Items.AddRange(new ToolStripItem[] { FolderUpButton, FolderPath, FolderRefreshButton, SearchEntryText });
            toolStrip2.LayoutStyle = ToolStripLayoutStyle.Flow;
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new System.Drawing.Size(618, 23);
            toolStrip2.TabIndex = 2;
            toolStrip2.Text = "toolStrip1";
            toolStrip2.Resize += toolStrip1_Resize;
            // 
            // FolderUpButton
            // 
            FolderUpButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            FolderUpButton.Image = (System.Drawing.Image)resources.GetObject("FolderUpButton.Image");
            FolderUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FolderUpButton.Name = "FolderUpButton";
            FolderUpButton.Size = new System.Drawing.Size(23, 20);
            FolderUpButton.Text = "Up a Folder";
            FolderUpButton.ToolTipText = "$UP_TOOLTIP";
            FolderUpButton.Click += OnUpButtonClicked;
            // 
            // FolderPath
            // 
            FolderPath.AutoSize = false;
            FolderPath.Name = "FolderPath";
            FolderPath.Size = new System.Drawing.Size(233, 23);
            FolderPath.ToolTipText = "$FOLDER_PATH_TOOLTIP";
            FolderPath.KeyPress += onPathChange;
            // 
            // FolderRefreshButton
            // 
            FolderRefreshButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            FolderRefreshButton.Image = (System.Drawing.Image)resources.GetObject("FolderRefreshButton.Image");
            FolderRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FolderRefreshButton.Name = "FolderRefreshButton";
            FolderRefreshButton.Size = new System.Drawing.Size(23, 20);
            FolderRefreshButton.Text = "$REFRESH";
            FolderRefreshButton.Click += OnRefreshButtonClicked;
            // 
            // SearchEntryText
            // 
            SearchEntryText.AutoSize = false;
            SearchEntryText.Name = "SearchEntryText";
            SearchEntryText.Size = new System.Drawing.Size(233, 23);
            SearchEntryText.ToolTipText = "$SEARCH_TOOLTIP";
            SearchEntryText.TextChanged += SearchBarOnTextChanged;
            // 
            // fileListView
            // 
            fileListView.AllowDrop = true;
            fileListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fileListView.Columns.AddRange(new ColumnHeader[] { columnName, columnType, columnSize, columnLastModified });
            fileListView.ContextMenuStrip = GEContext;
            fileListView.LargeImageList = imageBank;
            fileListView.Location = new System.Drawing.Point(4, 35);
            fileListView.Margin = new Padding(4, 3, 4, 3);
            fileListView.Name = "fileListView";
            fileListView.Size = new System.Drawing.Size(608, 391);
            fileListView.SmallImageList = imageBank;
            fileListView.TabIndex = 0;
            fileListView.UseCompatibleStateImageBehavior = false;
            fileListView.View = View.Details;
            fileListView.ItemActivate += listView1_ItemActivate;
            fileListView.ItemSelectionChanged += ListView_ItemSelectionChanged;
            fileListView.DragDrop += ListView_OnDragDrop;
            fileListView.DragEnter += ListView_OnDragEnter;
            fileListView.DragLeave += ListView_OnDragLeave;
            fileListView.KeyUp += ListView_OnKeyUp;
            // 
            // columnName
            // 
            columnName.Text = "$NAME";
            columnName.Width = 157;
            // 
            // columnType
            // 
            columnType.Text = "$TYPE";
            columnType.Width = 143;
            // 
            // columnSize
            // 
            columnSize.Text = "$SIZE";
            // 
            // columnLastModified
            // 
            columnLastModified.Text = "$LAST_MODIFIED";
            columnLastModified.Width = 281;
            // 
            // GEContext
            // 
            GEContext.Items.AddRange(new ToolStripItem[] { ContextSDSUnpack, ContextSDSPack, ContextFileExport, ContextFileImport, ContextOpenFolder, ContextSDSUnpackAll, ContextView, ContextForceBigEndian, ContextSeperator, ContextDeleteSelectedFiles, ContextUnpackSelectedSDS, ContextPackSelectedSDS });
            GEContext.Name = "SDSContext";
            GEContext.Size = new System.Drawing.Size(294, 252);
            GEContext.Text = "$VIEW";
            GEContext.Opening += OnOpening;
            // 
            // ContextSDSUnpack
            // 
            ContextSDSUnpack.Name = "ContextSDSUnpack";
            ContextSDSUnpack.ShortcutKeyDisplayString = "CTRL + U";
            ContextSDSUnpack.Size = new System.Drawing.Size(293, 22);
            ContextSDSUnpack.Text = "$UNPACK";
            ContextSDSUnpack.Visible = false;
            ContextSDSUnpack.Click += ContextSDSUnpack_Click;
            // 
            // ContextSDSPack
            // 
            ContextSDSPack.Name = "ContextSDSPack";
            ContextSDSPack.ShortcutKeyDisplayString = "CTRL + P";
            ContextSDSPack.Size = new System.Drawing.Size(293, 22);
            ContextSDSPack.Text = "$PACK";
            ContextSDSPack.Visible = false;
            ContextSDSPack.Click += ContextSDSPack_Click;
            // 
            // ContextFileExport
            // 
            ContextFileExport.Name = "ContextFileExport";
            ContextFileExport.Size = new System.Drawing.Size(293, 22);
            ContextFileExport.Text = "$FILE_EXPORT";
            ContextFileExport.Click += ContextFileExport_OnClick;
            // 
            // ContextFileImport
            // 
            ContextFileImport.Name = "ContextFileImport";
            ContextFileImport.Size = new System.Drawing.Size(293, 22);
            ContextFileImport.Text = "$FILE_IMPORT";
            ContextFileImport.Click += ContextFileImport_OnClick;
            // 
            // ContextOpenFolder
            // 
            ContextOpenFolder.Name = "ContextOpenFolder";
            ContextOpenFolder.Size = new System.Drawing.Size(293, 22);
            ContextOpenFolder.Text = "$OPEN_FOLDER_EXPLORER";
            ContextOpenFolder.Click += ContextOpenFolder_Click;
            // 
            // ContextSDSUnpackAll
            // 
            ContextSDSUnpackAll.Name = "ContextSDSUnpackAll";
            ContextSDSUnpackAll.Size = new System.Drawing.Size(293, 22);
            ContextSDSUnpackAll.Text = "$UNPACK_ALL_SDS";
            ContextSDSUnpackAll.Click += ContextSDSUnpackAll_Click;
            // 
            // ContextView
            // 
            ContextView.DropDownItems.AddRange(new ToolStripItem[] { ContextViewIcon, ContextViewDetails, ContextViewSmallIcon, ContextViewList, ContextViewTile });
            ContextView.Name = "ContextView";
            ContextView.Size = new System.Drawing.Size(293, 22);
            ContextView.Text = "$VIEW";
            // 
            // ContextViewIcon
            // 
            ContextViewIcon.CheckOnClick = true;
            ContextViewIcon.Name = "ContextViewIcon";
            ContextViewIcon.Size = new System.Drawing.Size(151, 22);
            ContextViewIcon.Text = "$ICON";
            ContextViewIcon.Click += OnViewIconClicked;
            // 
            // ContextViewDetails
            // 
            ContextViewDetails.CheckOnClick = true;
            ContextViewDetails.Name = "ContextViewDetails";
            ContextViewDetails.Size = new System.Drawing.Size(151, 22);
            ContextViewDetails.Text = "$DETAILS";
            ContextViewDetails.Click += OnViewDetailsClicked;
            // 
            // ContextViewSmallIcon
            // 
            ContextViewSmallIcon.CheckOnClick = true;
            ContextViewSmallIcon.Name = "ContextViewSmallIcon";
            ContextViewSmallIcon.Size = new System.Drawing.Size(151, 22);
            ContextViewSmallIcon.Text = "$SMALL_ICON";
            ContextViewSmallIcon.Click += OnViewSmallIconClicked;
            // 
            // ContextViewList
            // 
            ContextViewList.CheckOnClick = true;
            ContextViewList.Name = "ContextViewList";
            ContextViewList.Size = new System.Drawing.Size(151, 22);
            ContextViewList.Text = "$LIST";
            ContextViewList.Click += OnViewListClicked;
            // 
            // ContextViewTile
            // 
            ContextViewTile.CheckOnClick = true;
            ContextViewTile.Name = "ContextViewTile";
            ContextViewTile.Size = new System.Drawing.Size(151, 22);
            ContextViewTile.Text = "$TILE";
            ContextViewTile.Click += OnViewTileClicked;
            // 
            // ContextForceBigEndian
            // 
            ContextForceBigEndian.Name = "ContextForceBigEndian";
            ContextForceBigEndian.Size = new System.Drawing.Size(293, 22);
            ContextForceBigEndian.Text = "$FORCE_BIG_ENDIAN";
            ContextForceBigEndian.Click += ContextForceBigEndian_Click;
            // 
            // ContextSeperator
            // 
            ContextSeperator.Name = "ContextSeperator";
            ContextSeperator.Size = new System.Drawing.Size(290, 6);
            // 
            // ContextDeleteSelectedFiles
            // 
            ContextDeleteSelectedFiles.Name = "ContextDeleteSelectedFiles";
            ContextDeleteSelectedFiles.ShortcutKeyDisplayString = "CTRL + DELETE";
            ContextDeleteSelectedFiles.Size = new System.Drawing.Size(293, 22);
            ContextDeleteSelectedFiles.Text = "$DELETE_SELECTED_FILES";
            ContextDeleteSelectedFiles.Click += ContextDeleteSelectedFiles_OnClick;
            // 
            // ContextUnpackSelectedSDS
            // 
            ContextUnpackSelectedSDS.Name = "ContextUnpackSelectedSDS";
            ContextUnpackSelectedSDS.Size = new System.Drawing.Size(293, 22);
            ContextUnpackSelectedSDS.Text = "$UNPACK_SELECTED_SDS";
            ContextUnpackSelectedSDS.Click += ContextUnpackSelectedSDS_OnClick;
            // 
            // ContextPackSelectedSDS
            // 
            ContextPackSelectedSDS.Name = "ContextPackSelectedSDS";
            ContextPackSelectedSDS.Size = new System.Drawing.Size(293, 22);
            ContextPackSelectedSDS.Text = "$PACK_SELECTED_SDS";
            ContextPackSelectedSDS.Click += ContextPackSelectedSDS_OnClick;
            // 
            // topContainer
            // 
            topContainer.BottomToolStripPanelVisible = false;
            // 
            // topContainer.ContentPanel
            // 
            topContainer.ContentPanel.Controls.Add(Toolstrip_ButtonContainer);
            topContainer.ContentPanel.Controls.Add(tools);
            topContainer.ContentPanel.Margin = new Padding(4, 3, 4, 3);
            topContainer.ContentPanel.Size = new System.Drawing.Size(933, 63);
            topContainer.Dock = DockStyle.Top;
            topContainer.LeftToolStripPanelVisible = false;
            topContainer.Location = new System.Drawing.Point(0, 0);
            topContainer.Margin = new Padding(4, 3, 4, 3);
            topContainer.Name = "topContainer";
            topContainer.RightToolStripPanelVisible = false;
            topContainer.Size = new System.Drawing.Size(933, 63);
            topContainer.TabIndex = 1;
            topContainer.Text = "topContainer";
            topContainer.TopToolStripPanelVisible = false;
            // 
            // Toolstrip_ButtonContainer
            // 
            Toolstrip_ButtonContainer.AutoSize = false;
            Toolstrip_ButtonContainer.BackgroundImageLayout = ImageLayout.None;
            Toolstrip_ButtonContainer.ImageScalingSize = new System.Drawing.Size(18, 18);
            Toolstrip_ButtonContainer.Items.AddRange(new ToolStripItem[] { Button_Settings, Button_PackSDS, Button_UnpackSDS, Button_OpenMapEditor });
            Toolstrip_ButtonContainer.LayoutStyle = ToolStripLayoutStyle.Flow;
            Toolstrip_ButtonContainer.Location = new System.Drawing.Point(0, 22);
            Toolstrip_ButtonContainer.Name = "Toolstrip_ButtonContainer";
            Toolstrip_ButtonContainer.Padding = new Padding(0);
            Toolstrip_ButtonContainer.RenderMode = ToolStripRenderMode.System;
            Toolstrip_ButtonContainer.Size = new System.Drawing.Size(933, 45);
            Toolstrip_ButtonContainer.TabIndex = 2;
            Toolstrip_ButtonContainer.Text = "toolStrip1";
            // 
            // Button_Settings
            // 
            Button_Settings.AutoSize = false;
            Button_Settings.DisplayStyle = ToolStripItemDisplayStyle.Image;
            Button_Settings.Image = (System.Drawing.Image)resources.GetObject("Button_Settings.Image");
            Button_Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Settings.Name = "Button_Settings";
            Button_Settings.Size = new System.Drawing.Size(32, 32);
            Button_Settings.Text = "toolStripButton1";
            Button_Settings.Click += OnOptionsItem_Clicked;
            // 
            // Button_PackSDS
            // 
            Button_PackSDS.AutoSize = false;
            Button_PackSDS.BackColor = System.Drawing.SystemColors.Control;
            Button_PackSDS.DisplayStyle = ToolStripItemDisplayStyle.Image;
            Button_PackSDS.Enabled = false;
            Button_PackSDS.Image = (System.Drawing.Image)resources.GetObject("Button_PackSDS.Image");
            Button_PackSDS.ImageTransparentColor = System.Drawing.Color.White;
            Button_PackSDS.Name = "Button_PackSDS";
            Button_PackSDS.Size = new System.Drawing.Size(32, 32);
            Button_PackSDS.Text = "toolStripButton1";
            Button_PackSDS.Click += ContextSDSPack_Click;
            // 
            // Button_UnpackSDS
            // 
            Button_UnpackSDS.AutoSize = false;
            Button_UnpackSDS.DisplayStyle = ToolStripItemDisplayStyle.Image;
            Button_UnpackSDS.Enabled = false;
            Button_UnpackSDS.Image = (System.Drawing.Image)resources.GetObject("Button_UnpackSDS.Image");
            Button_UnpackSDS.ImageTransparentColor = System.Drawing.Color.White;
            Button_UnpackSDS.Name = "Button_UnpackSDS";
            Button_UnpackSDS.Size = new System.Drawing.Size(32, 32);
            Button_UnpackSDS.Text = "Button_PackSDS";
            Button_UnpackSDS.Click += ContextSDSUnpack_Click;
            // 
            // Button_OpenMapEditor
            // 
            Button_OpenMapEditor.AutoSize = false;
            Button_OpenMapEditor.DisplayStyle = ToolStripItemDisplayStyle.Image;
            Button_OpenMapEditor.Enabled = false;
            Button_OpenMapEditor.Image = (System.Drawing.Image)resources.GetObject("Button_OpenMapEditor.Image");
            Button_OpenMapEditor.ImageScaling = ToolStripItemImageScaling.None;
            Button_OpenMapEditor.ImageTransparentColor = System.Drawing.Color.White;
            Button_OpenMapEditor.Name = "Button_OpenMapEditor";
            Button_OpenMapEditor.Size = new System.Drawing.Size(32, 32);
            Button_OpenMapEditor.Text = "toolStripButton1";
            Button_OpenMapEditor.ToolTipText = "Button_OpenMapEditor";
            Button_OpenMapEditor.Click += ContextOpenMapEditor_Click;
            // 
            // tools
            // 
            tools.Items.AddRange(new ToolStripItem[] { dropdownFile, dropdownView, dropdownTools, dropdownAbout });
            tools.LayoutStyle = ToolStripLayoutStyle.Flow;
            tools.Location = new System.Drawing.Point(0, 0);
            tools.Name = "tools";
            tools.Size = new System.Drawing.Size(933, 22);
            tools.TabIndex = 1;
            tools.Text = "toolStrip1";
            // 
            // dropdownFile
            // 
            dropdownFile.DisplayStyle = ToolStripItemDisplayStyle.Text;
            dropdownFile.DropDownItems.AddRange(new ToolStripItem[] { OpenGameFolderButton, RunGameButton, SelectGameButton, ExitEditorButton });
            dropdownFile.Image = (System.Drawing.Image)resources.GetObject("dropdownFile.Image");
            dropdownFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            dropdownFile.Name = "dropdownFile";
            dropdownFile.Size = new System.Drawing.Size(47, 19);
            dropdownFile.Text = "$FILE";
            // 
            // OpenGameFolderButton
            // 
            OpenGameFolderButton.Image = (System.Drawing.Image)resources.GetObject("OpenGameFolderButton.Image");
            OpenGameFolderButton.Name = "OpenGameFolderButton";
            OpenGameFolderButton.ShortcutKeys = Keys.Control | Keys.O;
            OpenGameFolderButton.Size = new System.Drawing.Size(202, 22);
            OpenGameFolderButton.Text = "$BTN_OPEN_MII";
            OpenGameFolderButton.Click += OpenMafiaIIClicked;
            // 
            // RunGameButton
            // 
            RunGameButton.Image = (System.Drawing.Image)resources.GetObject("RunGameButton.Image");
            RunGameButton.Name = "RunGameButton";
            RunGameButton.ShortcutKeys = Keys.Control | Keys.R;
            RunGameButton.Size = new System.Drawing.Size(202, 22);
            RunGameButton.Text = "$BTN_RUN_MII";
            RunGameButton.Click += RunMafiaIIClicked;
            // 
            // SelectGameButton
            // 
            SelectGameButton.Image = (System.Drawing.Image)resources.GetObject("SelectGameButton.Image");
            SelectGameButton.Name = "SelectGameButton";
            SelectGameButton.ShortcutKeys = Keys.Control | Keys.F;
            SelectGameButton.Size = new System.Drawing.Size(202, 22);
            SelectGameButton.Text = "$SELECT_GAME";
            SelectGameButton.Click += Button_SelectGame_OnClick;
            // 
            // ExitEditorButton
            // 
            ExitEditorButton.Image = (System.Drawing.Image)resources.GetObject("ExitEditorButton.Image");
            ExitEditorButton.Name = "ExitEditorButton";
            ExitEditorButton.Size = new System.Drawing.Size(202, 22);
            ExitEditorButton.Text = "$EXIT";
            ExitEditorButton.Click += ExitToolkitClicked;
            // 
            // dropdownView
            // 
            dropdownView.DisplayStyle = ToolStripItemDisplayStyle.Text;
            dropdownView.DropDownItems.AddRange(new ToolStripItem[] { ViewStripMenuIcon, ViewStripMenuDetails, ViewStripMenuSmallIcon, ViewStripMenuList, ViewStripMenuTile });
            dropdownView.Image = (System.Drawing.Image)resources.GetObject("dropdownView.Image");
            dropdownView.ImageTransparentColor = System.Drawing.Color.Magenta;
            dropdownView.Name = "dropdownView";
            dropdownView.Size = new System.Drawing.Size(53, 19);
            dropdownView.Text = "$VIEW";
            // 
            // ViewStripMenuIcon
            // 
            ViewStripMenuIcon.CheckOnClick = true;
            ViewStripMenuIcon.Image = (System.Drawing.Image)resources.GetObject("ViewStripMenuIcon.Image");
            ViewStripMenuIcon.Name = "ViewStripMenuIcon";
            ViewStripMenuIcon.Size = new System.Drawing.Size(180, 22);
            ViewStripMenuIcon.Text = "$ICON";
            ViewStripMenuIcon.Click += OnViewIconClicked;
            // 
            // ViewStripMenuDetails
            // 
            ViewStripMenuDetails.CheckOnClick = true;
            ViewStripMenuDetails.Name = "ViewStripMenuDetails";
            ViewStripMenuDetails.Size = new System.Drawing.Size(180, 22);
            ViewStripMenuDetails.Text = "$DETAILS";
            ViewStripMenuDetails.Click += OnViewDetailsClicked;
            // 
            // ViewStripMenuSmallIcon
            // 
            ViewStripMenuSmallIcon.CheckOnClick = true;
            ViewStripMenuSmallIcon.Image = (System.Drawing.Image)resources.GetObject("ViewStripMenuSmallIcon.Image");
            ViewStripMenuSmallIcon.Name = "ViewStripMenuSmallIcon";
            ViewStripMenuSmallIcon.Size = new System.Drawing.Size(180, 22);
            ViewStripMenuSmallIcon.Text = "$SMALL_ICON";
            ViewStripMenuSmallIcon.Click += OnViewSmallIconClicked;
            // 
            // ViewStripMenuList
            // 
            ViewStripMenuList.CheckOnClick = true;
            ViewStripMenuList.Image = (System.Drawing.Image)resources.GetObject("ViewStripMenuList.Image");
            ViewStripMenuList.Name = "ViewStripMenuList";
            ViewStripMenuList.Size = new System.Drawing.Size(180, 22);
            ViewStripMenuList.Text = "$LIST";
            ViewStripMenuList.Click += OnViewListClicked;
            // 
            // ViewStripMenuTile
            // 
            ViewStripMenuTile.CheckOnClick = true;
            ViewStripMenuTile.Name = "ViewStripMenuTile";
            ViewStripMenuTile.Size = new System.Drawing.Size(180, 22);
            ViewStripMenuTile.Text = "$TILE";
            ViewStripMenuTile.Click += OnViewTileClicked;
            // 
            // dropdownTools
            // 
            dropdownTools.DisplayStyle = ToolStripItemDisplayStyle.Text;
            dropdownTools.DropDownItems.AddRange(new ToolStripItem[] { OptionsItem, toolStripSeparator1, PackCurrentSDSButton, UnpackCurrentSDSButton, UnpackAllSDSButton });
            dropdownTools.Image = (System.Drawing.Image)resources.GetObject("dropdownTools.Image");
            dropdownTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            dropdownTools.Name = "dropdownTools";
            dropdownTools.Size = new System.Drawing.Size(61, 19);
            dropdownTools.Text = "$TOOLS";
            // 
            // OptionsItem
            // 
            OptionsItem.Image = (System.Drawing.Image)resources.GetObject("OptionsItem.Image");
            OptionsItem.Name = "OptionsItem";
            OptionsItem.Size = new System.Drawing.Size(180, 22);
            OptionsItem.Text = "$OPTIONS";
            OptionsItem.Click += OnOptionsItem_Clicked;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // PackCurrentSDSButton
            // 
            PackCurrentSDSButton.Image = (System.Drawing.Image)resources.GetObject("PackCurrentSDSButton.Image");
            PackCurrentSDSButton.Name = "PackCurrentSDSButton";
            PackCurrentSDSButton.Size = new System.Drawing.Size(180, 22);
            PackCurrentSDSButton.Text = "$PACK_CUR_SDS";
            PackCurrentSDSButton.Click += ContextPackSelectedSDS_OnClick;
            // 
            // UnpackCurrentSDSButton
            // 
            UnpackCurrentSDSButton.Image = (System.Drawing.Image)resources.GetObject("UnpackCurrentSDSButton.Image");
            UnpackCurrentSDSButton.Name = "UnpackCurrentSDSButton";
            UnpackCurrentSDSButton.Size = new System.Drawing.Size(180, 22);
            UnpackCurrentSDSButton.Text = "$UNPACK_CUR_SDS";
            UnpackCurrentSDSButton.Click += ContextUnpackSelectedSDS_OnClick;
            // 
            // UnpackAllSDSButton
            // 
            UnpackAllSDSButton.Name = "UnpackAllSDSButton";
            UnpackAllSDSButton.Size = new System.Drawing.Size(180, 22);
            UnpackAllSDSButton.Text = "$UNPACK_ALL_SDS";
            UnpackAllSDSButton.Click += UnpackAllSDSButton_Click;
            // 
            // dropdownAbout
            // 
            dropdownAbout.DisplayStyle = ToolStripItemDisplayStyle.Text;
            dropdownAbout.DropDownItems.AddRange(new ToolStripItem[] { AboutButton });
            dropdownAbout.Image = (System.Drawing.Image)resources.GetObject("dropdownAbout.Image");
            dropdownAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            dropdownAbout.Name = "dropdownAbout";
            dropdownAbout.Size = new System.Drawing.Size(64, 19);
            dropdownAbout.Text = "$ABOUT";
            // 
            // AboutButton
            // 
            AboutButton.Image = (System.Drawing.Image)resources.GetObject("AboutButton.Image");
            AboutButton.Name = "AboutButton";
            AboutButton.Size = new System.Drawing.Size(118, 22);
            AboutButton.Text = "$ABOUT";
            AboutButton.Click += OnCredits_Pressed;
            // 
            // bottomContainer
            // 
            bottomContainer.BottomToolStripPanelVisible = false;
            // 
            // bottomContainer.ContentPanel
            // 
            bottomContainer.ContentPanel.Controls.Add(status);
            bottomContainer.ContentPanel.Margin = new Padding(4, 3, 4, 3);
            bottomContainer.ContentPanel.Size = new System.Drawing.Size(933, 30);
            bottomContainer.Dock = DockStyle.Bottom;
            bottomContainer.LeftToolStripPanelVisible = false;
            bottomContainer.Location = new System.Drawing.Point(0, 489);
            bottomContainer.Margin = new Padding(4, 3, 4, 3);
            bottomContainer.Name = "bottomContainer";
            bottomContainer.RightToolStripPanelVisible = false;
            bottomContainer.Size = new System.Drawing.Size(933, 30);
            bottomContainer.TabIndex = 1;
            bottomContainer.Text = "bottomContainer";
            bottomContainer.TopToolStripPanelVisible = false;
            // 
            // status
            // 
            status.Dock = DockStyle.Fill;
            status.Items.AddRange(new ToolStripItem[] { infoText });
            status.Location = new System.Drawing.Point(0, 0);
            status.Name = "status";
            status.Padding = new Padding(1, 0, 16, 0);
            status.Size = new System.Drawing.Size(933, 30);
            status.TabIndex = 0;
            status.Text = "statusStrip1";
            // 
            // infoText
            // 
            infoText.Name = "infoText";
            infoText.Size = new System.Drawing.Size(0, 25);
            // 
            // GameExplorer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(mainContainer);
            Controls.Add(bottomContainer);
            Controls.Add(topContainer);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(744, 548);
            Name = "GameExplorer";
            Text = "$MII_TK_GAME_EXPLORER";
            Load += toolStrip1_Resize;
            mainContainer.Panel1.ResumeLayout(false);
            mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainContainer).EndInit();
            mainContainer.ResumeLayout(false);
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.ContentPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            GEContext.ResumeLayout(false);
            topContainer.ContentPanel.ResumeLayout(false);
            topContainer.ContentPanel.PerformLayout();
            topContainer.ResumeLayout(false);
            topContainer.PerformLayout();
            Toolstrip_ButtonContainer.ResumeLayout(false);
            Toolstrip_ButtonContainer.PerformLayout();
            tools.ResumeLayout(false);
            tools.PerformLayout();
            bottomContainer.ContentPanel.ResumeLayout(false);
            bottomContainer.ContentPanel.PerformLayout();
            bottomContainer.ResumeLayout(false);
            bottomContainer.PerformLayout();
            status.ResumeLayout(false);
            status.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnLastModified;
        private System.Windows.Forms.ColumnHeader columnSize;
        private ToolStripContainer topContainer;
        private ToolStrip tools;
        private ToolStripContainer bottomContainer;
        private StatusStrip status;
        private ToolStripStatusLabel infoText;
        private ContextMenuStrip GEContext;
        private ToolStripMenuItem ContextSDSUnpack;
        private ToolStripMenuItem ContextSDSPack;
        private ToolStripDropDownButton dropdownFile;
        private ToolStripDropDownButton dropdownTools;
        private ToolStripMenuItem OpenGameFolderButton;
        private ToolStripMenuItem ExitEditorButton;
        private ToolStripMenuItem RunGameButton;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip2;
        private ToolStripButton FolderUpButton;
        private ToolStripTextBox FolderPath;
        private ToolStripButton FolderRefreshButton;
        private ToolStripTextBox SearchEntryText;
        private ToolStripMenuItem ContextOpenFolder;
        private ToolStripMenuItem OptionsItem;
        private ToolStripMenuItem ContextSDSUnpackAll;
        private ImageList imageBank;
        private ToolStripMenuItem ContextView;
        private ToolStripMenuItem ContextViewIcon;
        private ToolStripMenuItem ContextViewDetails;
        private ToolStripMenuItem ContextViewSmallIcon;
        private ToolStripMenuItem ContextViewList;
        private ToolStripMenuItem ContextViewTile;
        private ToolStripDropDownButton dropdownView;
        private ToolStripMenuItem ViewStripMenuIcon;
        private ToolStripMenuItem ViewStripMenuDetails;
        private ToolStripMenuItem ViewStripMenuSmallIcon;
        private ToolStripMenuItem ViewStripMenuList;
        private ToolStripMenuItem ViewStripMenuTile;
        private ToolStripMenuItem UnpackAllSDSButton;
        private ToolStripMenuItem ContextForceBigEndian;
        private ToolStripMenuItem SelectGameButton;
        private ToolStripMenuItem ContextDeleteSelectedFiles;
        private ToolStripSeparator ContextSeperator;
        private ToolStripMenuItem ContextUnpackSelectedSDS;
        private ToolStripMenuItem ContextPackSelectedSDS;
        private ToolStrip Toolstrip_ButtonContainer;
        private ToolStripButton Button_PackSDS;
        private ToolStripButton Button_UnpackSDS;
        private ToolStripButton Button_Settings;
        private ToolStripDropDownButton dropdownAbout;
        private ToolStripMenuItem AboutButton;
        private Controls.MTreeView folderView;
        private Controls.MListView fileListView;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem PackCurrentSDSButton;
        private ToolStripMenuItem UnpackCurrentSDSButton;
        private ToolStripButton Button_OpenMapEditor;
        private ToolStripMenuItem ContextFileExport;
        private ToolStripMenuItem ContextFileImport;
    }
}