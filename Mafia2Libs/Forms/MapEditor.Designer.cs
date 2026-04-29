using System;
using Utils.Extensions;

namespace Mafia2Tool
{
    partial class MapEditor
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
            System.Windows.Forms.StatusStrip StatusStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            CurrentModeButton = new System.Windows.Forms.ToolStripSplitButton();
            PositionXTool = new NumericUpDownToolStrip();
            PositionYTool = new NumericUpDownToolStrip();
            PositionZTool = new NumericUpDownToolStrip();
            CameraSpeedTool = new NumericUpDownToolStrip();
            toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            PasteXYZ = new System.Windows.Forms.ToolStripSplitButton();
            Label_FPS = new System.Windows.Forms.ToolStripStatusLabel();
            Label_MemoryUsage = new System.Windows.Forms.ToolStripStatusLabel();
            ToolbarStrip = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveCollisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveTranslocatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveActorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveOBJDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAIWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAllItemDescToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveItemDescSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAnimalTraffciPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAllnotSafeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveSoundSectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddButton = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportFrame = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportBundle = new System.Windows.Forms.ToolStripMenuItem();
            AddSceneFolderButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewButton = new System.Windows.Forms.ToolStripDropDownButton();
            ViewTopButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewFrontButton = new System.Windows.Forms.ToolStripMenuItem();
            OptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            ToggleWireframeButton = new System.Windows.Forms.ToolStripMenuItem();
            ToggleCullingButton = new System.Windows.Forms.ToolStripMenuItem();
            EditLighting = new System.Windows.Forms.ToolStripMenuItem();
            WindowButton = new System.Windows.Forms.ToolStripDropDownButton();
            SceneTreeButton = new System.Windows.Forms.ToolStripMenuItem();
            ObjectPropertiesButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewOptionProperties = new System.Windows.Forms.ToolStripMenuItem();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            moveAIGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rotateAIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addGroupType1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addType4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addType7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addType8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addType9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addType11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            moveGroupNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addEdgesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            recalculateDistanceNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripDropDownButton5 = new System.Windows.Forms.ToolStripDropDownButton();
            removeSectorSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addSectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addPortalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            RenderPanel = new System.Windows.Forms.Panel();
            MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            TxtBrowser = new System.Windows.Forms.OpenFileDialog();
            dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            FrameBrowser = new System.Windows.Forms.OpenFileDialog();
            SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            StatusStrip.SuspendLayout();
            ToolbarStrip.SuspendLayout();
            SuspendLayout();
            // 
            // StatusStrip
            // 
            StatusStrip.AutoSize = false;
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CurrentModeButton, PositionXTool, PositionYTool, PositionZTool, CameraSpeedTool, toolStripSplitButton1, PasteXYZ, Label_FPS, Label_MemoryUsage });
            StatusStrip.Location = new System.Drawing.Point(0, 692);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            StatusStrip.Size = new System.Drawing.Size(1420, 28);
            StatusStrip.TabIndex = 6;
            StatusStrip.Text = "statusStrip1";
            // 
            // CurrentModeButton
            // 
            CurrentModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            CurrentModeButton.Image = (System.Drawing.Image)resources.GetObject("CurrentModeButton.Image");
            CurrentModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            CurrentModeButton.Name = "CurrentModeButton";
            CurrentModeButton.Size = new System.Drawing.Size(128, 26);
            CurrentModeButton.Text = "$CurrentModeLabel";
            CurrentModeButton.ButtonClick += CurrentModeButton_ButtonClick;
            // 
            // PositionXTool
            // 
            PositionXTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            PositionXTool.AutoSize = false;
            PositionXTool.DecimalPlaces = 5;
            PositionXTool.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            PositionXTool.Margin = new System.Windows.Forms.Padding(3, 0, 1, 0);
            PositionXTool.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            PositionXTool.Minimum = new decimal(new int[] { 9999999, 0, 0, int.MinValue });
            PositionXTool.Name = "PositionXTool";
            PositionXTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            PositionXTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            PositionXTool.Size = new System.Drawing.Size(84, 28);
            PositionXTool.Text = "0,00000";
            PositionXTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            PositionXTool.ValueChanged += CameraToolsOnValueChanged;
            // 
            // PositionYTool
            // 
            PositionYTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            PositionYTool.AutoSize = false;
            PositionYTool.DecimalPlaces = 5;
            PositionYTool.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            PositionYTool.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            PositionYTool.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            PositionYTool.Minimum = new decimal(new int[] { 9999999, 0, 0, int.MinValue });
            PositionYTool.Name = "PositionYTool";
            PositionYTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            PositionYTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            PositionYTool.Size = new System.Drawing.Size(84, 28);
            PositionYTool.Text = "0,00000";
            PositionYTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            PositionYTool.ValueChanged += CameraToolsOnValueChanged;
            // 
            // PositionZTool
            // 
            PositionZTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            PositionZTool.AutoSize = false;
            PositionZTool.DecimalPlaces = 5;
            PositionZTool.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            PositionZTool.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            PositionZTool.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            PositionZTool.Minimum = new decimal(new int[] { 9999999, 0, 0, int.MinValue });
            PositionZTool.Name = "PositionZTool";
            PositionZTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            PositionZTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            PositionZTool.Size = new System.Drawing.Size(84, 28);
            PositionZTool.Text = "0,00000";
            PositionZTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            PositionZTool.ValueChanged += CameraToolsOnValueChanged;
            // 
            // CameraSpeedTool
            // 
            CameraSpeedTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            CameraSpeedTool.AutoSize = false;
            CameraSpeedTool.DecimalPlaces = 5;
            CameraSpeedTool.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            CameraSpeedTool.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            CameraSpeedTool.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            CameraSpeedTool.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            CameraSpeedTool.Name = "CameraSpeedTool";
            CameraSpeedTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            CameraSpeedTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            CameraSpeedTool.Size = new System.Drawing.Size(84, 28);
            CameraSpeedTool.Text = "0,00000";
            CameraSpeedTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            CameraSpeedTool.ValueChanged += CameraSpeedUpdate;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.AutoToolTip = false;
            toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripSplitButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripSplitButton1.Image");
            toolStripSplitButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new System.Drawing.Size(75, 26);
            toolStripSplitButton1.Text = "Copy XYZ";
            toolStripSplitButton1.ButtonClick += CopyXYZ_ButtonClick;
            // 
            // PasteXYZ
            // 
            PasteXYZ.AutoToolTip = false;
            PasteXYZ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            PasteXYZ.Image = (System.Drawing.Image)resources.GetObject("PasteXYZ.Image");
            PasteXYZ.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            PasteXYZ.ImageTransparentColor = System.Drawing.Color.Magenta;
            PasteXYZ.Name = "PasteXYZ";
            PasteXYZ.Size = new System.Drawing.Size(75, 26);
            PasteXYZ.Text = "Paste XYZ";
            PasteXYZ.ButtonClick += PasteXYZ_ButtonClick;
            // 
            // Label_FPS
            // 
            Label_FPS.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            Label_FPS.Margin = new System.Windows.Forms.Padding(0, 3, 4, 2);
            Label_FPS.Name = "Label_FPS";
            Label_FPS.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            Label_FPS.Padding = new System.Windows.Forms.Padding(41, 0, 0, 0);
            Label_FPS.Size = new System.Drawing.Size(100, 23);
            Label_FPS.Text = "Label_FPS";
            // 
            // Label_MemoryUsage
            // 
            Label_MemoryUsage.Name = "Label_MemoryUsage";
            Label_MemoryUsage.Size = new System.Drawing.Size(117, 23);
            Label_MemoryUsage.Text = "Label_MemoryUsage";
            // 
            // ToolbarStrip
            // 
            ToolbarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButton4, EditButton, ViewButton, OptionsButton, WindowButton, toolStripDropDownButton1, toolStripDropDownButton3, toolStripDropDownButton5 });
            ToolbarStrip.Location = new System.Drawing.Point(0, 0);
            ToolbarStrip.Name = "ToolbarStrip";
            ToolbarStrip.Size = new System.Drawing.Size(1420, 25);
            ToolbarStrip.TabIndex = 1;
            ToolbarStrip.Text = "toolStrip1";
            // 
            // toolStripDropDownButton4
            // 
            toolStripDropDownButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { saveSceneToolStripMenuItem, saveCollisionToolStripMenuItem, saveTranslocatorToolStripMenuItem, saveActorToolStripMenuItem, saveOBJDataToolStripMenuItem, saveAIWorldToolStripMenuItem, saveAllItemDescToolStripMenuItem, saveItemDescSelectedToolStripMenuItem, saveAnimalTraffciPathToolStripMenuItem, saveAllnotSafeToolStripMenuItem, saveSoundSectorToolStripMenuItem });
            toolStripDropDownButton4.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton4.Image");
            toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            toolStripDropDownButton4.Size = new System.Drawing.Size(38, 22);
            toolStripDropDownButton4.Text = "File";
            // 
            // saveSceneToolStripMenuItem
            // 
            saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            saveSceneToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveSceneToolStripMenuItem.Text = "Save Scene";
            saveSceneToolStripMenuItem.Click += SaveButtonScene_Click;
            // 
            // saveCollisionToolStripMenuItem
            // 
            saveCollisionToolStripMenuItem.Name = "saveCollisionToolStripMenuItem";
            saveCollisionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveCollisionToolStripMenuItem.Text = "Save Collision";
            saveCollisionToolStripMenuItem.Click += SaveButtonCollision_Click;
            // 
            // saveTranslocatorToolStripMenuItem
            // 
            saveTranslocatorToolStripMenuItem.Name = "saveTranslocatorToolStripMenuItem";
            saveTranslocatorToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveTranslocatorToolStripMenuItem.Text = "Save Translocator";
            saveTranslocatorToolStripMenuItem.Click += SaveButtonTranslocator_Click;
            // 
            // saveActorToolStripMenuItem
            // 
            saveActorToolStripMenuItem.Name = "saveActorToolStripMenuItem";
            saveActorToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveActorToolStripMenuItem.Text = "Save Actor";
            saveActorToolStripMenuItem.Click += SaveButtonActor_Click;
            // 
            // saveOBJDataToolStripMenuItem
            // 
            saveOBJDataToolStripMenuItem.Name = "saveOBJDataToolStripMenuItem";
            saveOBJDataToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveOBJDataToolStripMenuItem.Text = "Save OBJData";
            saveOBJDataToolStripMenuItem.Click += SaveButtonOBJDataClick;
            // 
            // saveAIWorldToolStripMenuItem
            // 
            saveAIWorldToolStripMenuItem.Name = "saveAIWorldToolStripMenuItem";
            saveAIWorldToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveAIWorldToolStripMenuItem.Text = "Save AIWorld";
            saveAIWorldToolStripMenuItem.Click += SaveButtonAIWorldClick;
            // 
            // saveAllItemDescToolStripMenuItem
            // 
            saveAllItemDescToolStripMenuItem.Name = "saveAllItemDescToolStripMenuItem";
            saveAllItemDescToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveAllItemDescToolStripMenuItem.Text = "Save All ItemDesc";
            saveAllItemDescToolStripMenuItem.Click += SaveButtonItemDesc_Click;
            // 
            // saveItemDescSelectedToolStripMenuItem
            // 
            saveItemDescSelectedToolStripMenuItem.Name = "saveItemDescSelectedToolStripMenuItem";
            saveItemDescSelectedToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveItemDescSelectedToolStripMenuItem.Text = "Save ItemDesc selected ";
            saveItemDescSelectedToolStripMenuItem.Click += SaveButtonSelItemDesc_Click;
            // 
            // saveAnimalTraffciPathToolStripMenuItem
            // 
            saveAnimalTraffciPathToolStripMenuItem.Name = "saveAnimalTraffciPathToolStripMenuItem";
            saveAnimalTraffciPathToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveAnimalTraffciPathToolStripMenuItem.Text = "Save Animal Traffic Path";
            saveAnimalTraffciPathToolStripMenuItem.Click += SaveButtonATP_Click;
            // 
            // saveAllnotSafeToolStripMenuItem
            // 
            saveAllnotSafeToolStripMenuItem.Name = "saveAllnotSafeToolStripMenuItem";
            saveAllnotSafeToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveAllnotSafeToolStripMenuItem.Text = "Save All (not safe)";
            saveAllnotSafeToolStripMenuItem.Click += SaveButton_Click;
            // 
            // saveSoundSectorToolStripMenuItem
            // 
            saveSoundSectorToolStripMenuItem.Name = "saveSoundSectorToolStripMenuItem";
            saveSoundSectorToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            saveSoundSectorToolStripMenuItem.Text = "Save SoundSector";
            saveSoundSectorToolStripMenuItem.Click += SaveButtonSoundSector_Click;
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddButton, Button_ImportFrame, Button_ImportBundle, AddSceneFolderButton });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(66, 22);
            EditButton.Text = "$CREATE";
            // 
            // AddButton
            // 
            AddButton.Name = "AddButton";
            AddButton.Size = new System.Drawing.Size(227, 22);
            AddButton.Text = "$ADD";
            AddButton.Click += AddButtonOnClick;
            // 
            // Button_ImportFrame
            // 
            Button_ImportFrame.Name = "Button_ImportFrame";
            Button_ImportFrame.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F;
            Button_ImportFrame.Size = new System.Drawing.Size(227, 22);
            Button_ImportFrame.Text = "$IMPORT_FRAME";
            Button_ImportFrame.Click += Button_ImportFrame_OnClicked;
            // 
            // Button_ImportBundle
            // 
            Button_ImportBundle.Name = "Button_ImportBundle";
            Button_ImportBundle.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B;
            Button_ImportBundle.Size = new System.Drawing.Size(227, 22);
            Button_ImportBundle.Text = "$IMPORT_BUNDLE";
            Button_ImportBundle.Click += Button_ImportBundle_OnClick;
            // 
            // AddSceneFolderButton
            // 
            AddSceneFolderButton.Name = "AddSceneFolderButton";
            AddSceneFolderButton.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S;
            AddSceneFolderButton.Size = new System.Drawing.Size(227, 22);
            AddSceneFolderButton.Text = "$ADD_SCENE_FOLDER";
            AddSceneFolderButton.Click += AddSceneFolderButton_Click;
            // 
            // ViewButton
            // 
            ViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            ViewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ViewTopButton, ViewFrontButton });
            ViewButton.Image = (System.Drawing.Image)resources.GetObject("ViewButton.Image");
            ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            ViewButton.Name = "ViewButton";
            ViewButton.Size = new System.Drawing.Size(53, 22);
            ViewButton.Text = "$VIEW";
            // 
            // ViewTopButton
            // 
            ViewTopButton.Name = "ViewTopButton";
            ViewTopButton.Size = new System.Drawing.Size(180, 22);
            ViewTopButton.Text = "$TOP";
            ViewTopButton.Click += OnViewTopButtonClicked;
            // 
            // ViewFrontButton
            // 
            ViewFrontButton.Name = "ViewFrontButton";
            ViewFrontButton.Size = new System.Drawing.Size(180, 22);
            ViewFrontButton.Text = "$FRONT";
            ViewFrontButton.Click += OnViewFrontButtonClicked;
            // 
            // OptionsButton
            // 
            OptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            OptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToggleWireframeButton, ToggleCullingButton, EditLighting });
            OptionsButton.Image = (System.Drawing.Image)resources.GetObject("OptionsButton.Image");
            OptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            OptionsButton.Name = "OptionsButton";
            OptionsButton.Size = new System.Drawing.Size(75, 22);
            OptionsButton.Text = "$OPTIONS";
            // 
            // ToggleWireframeButton
            // 
            ToggleWireframeButton.Name = "ToggleWireframeButton";
            ToggleWireframeButton.Size = new System.Drawing.Size(192, 22);
            ToggleWireframeButton.Text = "$TOGGLE_WIREFRAME";
            ToggleWireframeButton.Click += FillModeButton_Click;
            // 
            // ToggleCullingButton
            // 
            ToggleCullingButton.Name = "ToggleCullingButton";
            ToggleCullingButton.Size = new System.Drawing.Size(192, 22);
            ToggleCullingButton.Text = "$TOGGLE_CULLING";
            ToggleCullingButton.Click += CullModeButton_Click;
            // 
            // EditLighting
            // 
            EditLighting.Name = "EditLighting";
            EditLighting.Size = new System.Drawing.Size(192, 22);
            EditLighting.Text = "$EDIT_LIGHTING";
            EditLighting.Click += EditLighting_Click;
            // 
            // WindowButton
            // 
            WindowButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            WindowButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SceneTreeButton, ObjectPropertiesButton, ViewOptionProperties });
            WindowButton.Image = (System.Drawing.Image)resources.GetObject("WindowButton.Image");
            WindowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            WindowButton.Name = "WindowButton";
            WindowButton.Size = new System.Drawing.Size(83, 22);
            WindowButton.Text = "$WINDOWS";
            // 
            // SceneTreeButton
            // 
            SceneTreeButton.Name = "SceneTreeButton";
            SceneTreeButton.Size = new System.Drawing.Size(198, 22);
            SceneTreeButton.Text = "$VIEW_SCENE_TREE";
            SceneTreeButton.Click += SceneTreeOnClicked;
            // 
            // ObjectPropertiesButton
            // 
            ObjectPropertiesButton.Name = "ObjectPropertiesButton";
            ObjectPropertiesButton.Size = new System.Drawing.Size(198, 22);
            ObjectPropertiesButton.Text = "$VIEW_PROPERTY_GRID";
            ObjectPropertiesButton.Click += PropertyGridOnClicked;
            // 
            // ViewOptionProperties
            // 
            ViewOptionProperties.Name = "ViewOptionProperties";
            ViewOptionProperties.Size = new System.Drawing.Size(198, 22);
            ViewOptionProperties.Text = "$VIEW_OPTIONS";
            ViewOptionProperties.Click += ViewOptionProperties_Click;
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { moveAIGroupToolStripMenuItem, rotateAIToolStripMenuItem, addGroupType1ToolStripMenuItem, addType4ToolStripMenuItem, addType7ToolStripMenuItem, addType8ToolStripMenuItem, addType9ToolStripMenuItem, addType11ToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(124, 22);
            toolStripDropDownButton1.Text = "Navigation AIWorld";
            toolStripDropDownButton1.ToolTipText = "Navigation";
            // 
            // moveAIGroupToolStripMenuItem
            // 
            moveAIGroupToolStripMenuItem.Name = "moveAIGroupToolStripMenuItem";
            moveAIGroupToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            moveAIGroupToolStripMenuItem.Text = "Move AIWorld Group";
            moveAIGroupToolStripMenuItem.Click += MoveAIGroupButton_Click;
            // 
            // rotateAIToolStripMenuItem
            // 
            rotateAIToolStripMenuItem.Name = "rotateAIToolStripMenuItem";
            rotateAIToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            rotateAIToolStripMenuItem.Text = "Rotate AIWorld Group";
            rotateAIToolStripMenuItem.Click += RotateAIGroupZButton_Click;
            // 
            // addGroupType1ToolStripMenuItem
            // 
            addGroupType1ToolStripMenuItem.Name = "addGroupType1ToolStripMenuItem";
            addGroupType1ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addGroupType1ToolStripMenuItem.Text = "Add Group";
            addGroupType1ToolStripMenuItem.Click += Button_AddType1Group_Click;
            // 
            // addType4ToolStripMenuItem
            // 
            addType4ToolStripMenuItem.Name = "addType4ToolStripMenuItem";
            addType4ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addType4ToolStripMenuItem.Text = "Add Shelters";
            addType4ToolStripMenuItem.Click += btnAddType4_Click;
            // 
            // addType7ToolStripMenuItem
            // 
            addType7ToolStripMenuItem.Name = "addType7ToolStripMenuItem";
            addType7ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addType7ToolStripMenuItem.Text = "Add Obstacles";
            addType7ToolStripMenuItem.Click += btnAddType7_Click;
            // 
            // addType8ToolStripMenuItem
            // 
            addType8ToolStripMenuItem.Name = "addType8ToolStripMenuItem";
            addType8ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addType8ToolStripMenuItem.Text = "Add Sidewalk";
            addType8ToolStripMenuItem.Click += btnAddType8_Click;
            // 
            // addType9ToolStripMenuItem
            // 
            addType9ToolStripMenuItem.Name = "addType9ToolStripMenuItem";
            addType9ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addType9ToolStripMenuItem.Text = "Add Pedestrian crossing";
            addType9ToolStripMenuItem.Click += btnAddType9_Click;
            // 
            // addType11ToolStripMenuItem
            // 
            addType11ToolStripMenuItem.Name = "addType11ToolStripMenuItem";
            addType11ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addType11ToolStripMenuItem.Text = "Add NPCShelters";
            addType11ToolStripMenuItem.Click += btnAddType11_Click;
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { moveGroupNodeToolStripMenuItem, addToolStripMenuItem, addEdgesToolStripMenuItem, recalculateDistanceNodesToolStripMenuItem });
            toolStripDropDownButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Size = new System.Drawing.Size(125, 22);
            toolStripDropDownButton3.Text = "Navigation OBJData";
            toolStripDropDownButton3.ToolTipText = "Navigation OBJData";
            // 
            // moveGroupNodeToolStripMenuItem
            // 
            moveGroupNodeToolStripMenuItem.Name = "moveGroupNodeToolStripMenuItem";
            moveGroupNodeToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            moveGroupNodeToolStripMenuItem.Text = "Move Group Node";
            moveGroupNodeToolStripMenuItem.Click += btnMoveNAV_Click;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            addToolStripMenuItem.Text = "Add Node";
            addToolStripMenuItem.Click += AddNavVertexButton_Click;
            // 
            // addEdgesToolStripMenuItem
            // 
            addEdgesToolStripMenuItem.Name = "addEdgesToolStripMenuItem";
            addEdgesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            // 
            // recalculateDistanceNodesToolStripMenuItem
            // 
            recalculateDistanceNodesToolStripMenuItem.Name = "recalculateDistanceNodesToolStripMenuItem";
            recalculateDistanceNodesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            recalculateDistanceNodesToolStripMenuItem.Text = "Recalculate Distance Nodes";
            recalculateDistanceNodesToolStripMenuItem.Click += Button_CalcConnectionDistances_Click;
            // 
            // toolStripDropDownButton5
            // 
            toolStripDropDownButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { removeSectorSoundToolStripMenuItem, addSectorToolStripMenuItem, addPortalToolStripMenuItem, deleteItemToolStripMenuItem });
            toolStripDropDownButton5.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton5.Image");
            toolStripDropDownButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton5.Name = "toolStripDropDownButton5";
            toolStripDropDownButton5.Size = new System.Drawing.Size(90, 22);
            toolStripDropDownButton5.Text = "Sound Sector";
            // 
            // removeSectorSoundToolStripMenuItem
            // 
            removeSectorSoundToolStripMenuItem.Name = "removeSectorSoundToolStripMenuItem";
            removeSectorSoundToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            removeSectorSoundToolStripMenuItem.Text = "Load Sector Sound";
            removeSectorSoundToolStripMenuItem.Click += BtnLoadSoundSectors_Click;
            // 
            // addSectorToolStripMenuItem
            // 
            addSectorToolStripMenuItem.Name = "addSectorToolStripMenuItem";
            addSectorToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            addSectorToolStripMenuItem.Text = "Add Sector";
            addSectorToolStripMenuItem.Click += BtnAddSoundSector_Click;
            // 
            // addPortalToolStripMenuItem
            // 
            addPortalToolStripMenuItem.Name = "addPortalToolStripMenuItem";
            addPortalToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            addPortalToolStripMenuItem.Text = "Add Portal";
            addPortalToolStripMenuItem.Click += BtnAddSoundPortal_Click;
            // 
            // deleteItemToolStripMenuItem
            // 
            deleteItemToolStripMenuItem.Name = "deleteItemToolStripMenuItem";
            deleteItemToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            deleteItemToolStripMenuItem.Text = "Delete Item";
            deleteItemToolStripMenuItem.Click += BtnDeleteSoundItem_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "StaticIcon");
            imageList1.Images.SetKeyName(1, "LightIcon");
            // 
            // RenderPanel
            // 
            RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RenderPanel.Location = new System.Drawing.Point(0, 25);
            RenderPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RenderPanel.Name = "RenderPanel";
            RenderPanel.Size = new System.Drawing.Size(1420, 667);
            RenderPanel.TabIndex = 0;
            // 
            // MeshBrowser
            // 
            MeshBrowser.Filter = "GLTF File (Binary) (*.glb)|*.glb|GLTF File (ASCII) (*.gltf)|*.gltf*";
            // 
            // TxtBrowser
            // 
            TxtBrowser.Filter = "Text Document|*txt";
            // 
            // dockPanel1
            // 
            dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            dockPanel1.Location = new System.Drawing.Point(0, 25);
            dockPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dockPanel1.Name = "dockPanel1";
            dockPanel1.Size = new System.Drawing.Size(1420, 667);
            dockPanel1.TabIndex = 0;
            // 
            // FrameBrowser
            // 
            FrameBrowser.Filter = "FrameResource|*.fr|Toolkit Frame Data|*.framedata";
            // 
            // MapEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1420, 720);
            Controls.Add(dockPanel1);
            Controls.Add(RenderPanel);
            Controls.Add(StatusStrip);
            Controls.Add(ToolbarStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MapEditor";
            Text = "Map Editor";
            FormClosing += OnFormClosing;
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ToolbarStrip.ResumeLayout(false);
            ToolbarStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ToolStrip ToolbarStrip;
        private System.Windows.Forms.ToolStripDropDownButton WindowButton;
        private System.Windows.Forms.ToolStripDropDownButton OptionsButton;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddButton;
        private System.Windows.Forms.OpenFileDialog MeshBrowser;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem SceneTreeButton;
        private System.Windows.Forms.ToolStripMenuItem ObjectPropertiesButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleWireframeButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCullingButton;
        private System.Windows.Forms.ToolStripMenuItem AddSceneFolderButton;
        private System.Windows.Forms.ToolStripMenuItem ViewOptionProperties;
        private System.Windows.Forms.OpenFileDialog TxtBrowser;
        private System.Windows.Forms.ToolStripStatusLabel Label_FPS;
        private NumericUpDownToolStrip PositionYTool;
        private NumericUpDownToolStrip PositionZTool;
        private NumericUpDownToolStrip PositionXTool;
        private NumericUpDownToolStrip CameraSpeedTool;
        private System.Windows.Forms.ToolStripDropDownButton ViewButton;
        private System.Windows.Forms.ToolStripMenuItem ViewTopButton;
        private System.Windows.Forms.ToolStripMenuItem ViewFrontButton;
        private System.Windows.Forms.ToolStripSplitButton CurrentModeButton;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem EditLighting;
        private System.Windows.Forms.ToolStripStatusLabel Label_MemoryUsage;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportFrame;
        private System.Windows.Forms.OpenFileDialog FrameBrowser;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportBundle;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog AnimFileDialog;
        private System.Windows.Forms.ToolStripSplitButton PasteXYZ;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem addType4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addGroupType1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addType7ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem addType11ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addType8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addType9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem moveGroupNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton4;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCollisionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTranslocatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveActorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveOBJDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAIWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllItemDescToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveItemDescSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveAIGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateAIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAnimalTraffciPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllnotSafeToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton5;
        private System.Windows.Forms.ToolStripMenuItem removeSectorSoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSoundSectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPortalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEdgesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recalculateDistanceNodesToolStripMenuItem;
    }
}