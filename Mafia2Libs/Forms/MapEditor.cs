using Forms.Docking;
using Forms.EditorControls;
using Mafia2Tool.Forms;
using Microsoft.VisualBasic;
using Rendering.Core;
using Rendering.Factories;
using Rendering.Graphics;
using Rendering.Input;
using ResourceTypes.Actors;
using ResourceTypes.BufferPools;
using ResourceTypes.Collisions;
using ResourceTypes.FrameNameTable;
using ResourceTypes.FrameResource;
using ResourceTypes.ItemDesc;
using ResourceTypes.Materials;
using ResourceTypes.ModelHelpers.ModelExporter;
using ResourceTypes.Navigation;
using ResourceTypes.Navigation.Traffic;
using ResourceTypes.Translokator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Toolkit.Core;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Language;
using Utils.Logging;
using Utils.Models;
using Utils.Settings;
using Utils.VorticeUtils;
using Vortice.Mathematics;
using WeifenLuo.WinFormsUI.Docking;
using static ResourceTypes.Collisions.Collision;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Collision = ResourceTypes.Collisions.Collision;
using Object = ResourceTypes.Translokator.Object;

namespace Mafia2Tool
{
    public partial class MapEditor : Form
    {
        private List<SoundSectorData> loadedSoundSectors = new List<SoundSectorData>();
        private List<PortalData> loadedPortals = new List<PortalData>();
        private string _soundSectorsFilePath;
        private string _soundSectorsRootName;
        private List<ulong> _soundSectorsHashes = new List<ulong>();

        private SceneData SceneData = new SceneData();
        private SceneData ImportedScene;
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }
        private AIWorld.NavigationData navData;
        private Point mousePos;
        private Point lastMousePos;
        private FileInfo fileLocation;
        private DockPropertyGrid dPropertyGrid;
        private DockSceneTree dSceneTree;
        private DockImportSceneTree dImportSceneTree;
        private DockViewProperties dViewProperties;
        private TreeNode frameResourceRoot;
        private TreeNode importFRRoot;
        private TreeNode collisionRoot;
        private TreeNode roadRoot;
        private TreeNode junctionRoot;
        private TreeNode animalTrafficRoot;
        private TreeNode actorRoot;
        private TreeNode AIWorldRoot;
        private TreeNode OBJDataRoot;
        private TreeNode translokatorRoot;
        private bool bSelectMode = false;
        private float selectTimer = 0.0f;
        private bool bHideChildren = false;
        private Dictionary<int, ActorEntry> RefIDToActorEntry = new Dictionary<int, ActorEntry>();
        private Dictionary<string, int> NamesAndDuplicationStore;

        // Undo/Redo key state tracking
        private bool undoKeyWasPressed = false;
        private bool redoKeyWasPressed = false;
        private bool bboxKeyWasPressed = false;

        public MapEditor(FileInfo info, SceneData sceneData)
        {
            SceneData = sceneData;
            TextureLoader.ScenePath = SceneData.ScenePath;
            InitializeComponent();
            Localise();

            navData = new AIWorld.NavigationData
            {
                Worlds = new List<AIWorld>()
            };

            sceneData.FrameResource.OnFrameRemoved += OnFrameRemoved;

            if (MaterialsManager.MaterialLibraries.Count == 0)
            {
                MessageBox.Show("No material libraries have loaded, make sure they are set up correctly in the options window!", "Warning!", MessageBoxButtons.OK);
            }

            ToolkitSettings.UpdateRichPresence(string.Format("Editing '{0}'", info.Directory.Name));
            fileLocation = info;
            InitDockingControls();
            if (ToolkitSettings.LoadFrameResource)
            {
                PopulateList();
            }
            NamesAndDuplicationStore = new Dictionary<string, int>();
            CameraSpeedTool.Value = (decimal)ToolkitSettings.CameraSpeed;
            KeyPreview = true;
            Text += " -" + info.Directory.Name;
            SwitchMode(true);
            StartD3DPanel();
        }

        private void Localise()
        {
            EditButton.Text = Language.GetString("$CREATE");
            ViewButton.Text = Language.GetString("$VIEW");
            ViewTopButton.Text = Language.GetString("$TOP");
            ViewFrontButton.Text = Language.GetString("$FRONT");
            OptionsButton.Text = Language.GetString("$OPTIONS");
            ToggleWireframeButton.Text = Language.GetString("$TOGGLE_WIREFRAME");
            ToggleCullingButton.Text = Language.GetString("$TOGGLE_CULLING");
            EditLighting.Text = Language.GetString("$EDIT_LIGHTING");
            SceneTreeButton.Text = Language.GetString("$VIEW_SCENE_TREE");
            ObjectPropertiesButton.Text = Language.GetString("$VIEW_PROPERTY_GRID");
            WindowButton.Text = Language.GetString("$VIEW_OPTIONS");
            ViewOptionProperties.Text = Language.GetString("$VIEW_UTILITIES");
            AddButton.Text = Language.GetString("$ADD");
            Button_ImportFrame.Text = Language.GetString("$IMPORT_FRAME");
            Button_ImportBundle.Text = Language.GetString("$IMPORT_BUNDLE");
            AddSceneFolderButton.Text = Language.GetString("$ADD_SCENE_FOLDER");
            Button_TestConvert32.Text = Language.GetString("$TEST_CONVERT_32BIT");
            Button_TestConvert16.Text = Language.GetString("$TEST_CONVERT_16BIT");
            Button_DumpTexture.Text = Language.GetString("$DUMP_TEXTURES");
        }

        private void InitDockingControls()
        {
            VS2015LightTheme BlueTheme = new VS2015LightTheme();
            dockPanel1.Theme = BlueTheme;
            dockPanel1.Controls.Add(RenderPanel);
            RenderPanel.Resize += RenderPanel_Resize;
            RenderPanel.MouseWheel += RenderPanel_MouseWheel;
            dPropertyGrid = new DockPropertyGrid();
            dSceneTree = new DockSceneTree();
            dViewProperties = new DockViewProperties();
            dImportSceneTree = new DockImportSceneTree();
            dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            dSceneTree.Show(dockPanel1, DockState.DockLeft);
            dSceneTree.Select();
            dSceneTree.SetEventHandler("AfterSelect", new TreeViewEventHandler(OnAfterSelect));
            dSceneTree.ExportFrameButton.Click += new EventHandler(ExportFrame_Click);
            dSceneTree.Export3DButton.Click += new EventHandler(Export3DButton_Click);
            dSceneTree.JumpToButton.Click += new EventHandler(JumpButton_Click);
            dSceneTree.DeleteButton.Click += new EventHandler(DeleteButton_Click);
            dSceneTree.DuplicateButton.Click += new EventHandler(DuplicateButton_Click);
            dSceneTree.SetEventHandler("AfterCheck", new TreeViewEventHandler(OutlinerAfterCheck));
            dSceneTree.SetKeyHandler("KeyUp", new KeyEventHandler(OnKeyUpDockedPanel));
            dSceneTree.SetKeyHandler("KeyDown", new KeyEventHandler(OnKeyDownDockedPanel));
            dSceneTree.LinkToActorButton.Click += new EventHandler(LinkToActor_Click);
            dPropertyGrid.KeyUp += new KeyEventHandler(OnKeyUpDockedPanel);
            dSceneTree.UpdateParent1Button.Click += new EventHandler(UpdateParent_Click);
            dSceneTree.UpdateParent2Button.Click += new EventHandler(UpdateParent_Click);
            dSceneTree.TreeViewNodeDropped += OnTreeViewNodeDropped;
            dPropertyGrid.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyValueChanged);
            dPropertyGrid.OnObjectUpdated += ApplyEntryChanges;
            dSceneTree.TranslokatorNewInstanceButton.Click += new EventHandler(TranslokatorNewInstanceButton_Click);
            dSceneTree.ActorEntryNewTRObjectButton.Click += new EventHandler(ActorEntryNewTRObjectButton_Click);
            dSceneTree.TRRebuildObjectButton.Click += new EventHandler(TRRebuildObjectButton_Click);
        }
        private void BtnAddEdgeBox_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = dSceneTree.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show("Select a Set node (UnkSet0) or an existing EdgeBox first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UnkSet0 targetSet = null;
            OBJData targetOBJData = null;
            TreeNode setNode = null;

            if (selectedNode.Tag is BoundingBox)
            {
                setNode = selectedNode.Parent;
                targetSet = setNode.Tag as UnkSet0;
                TreeNode navNode = setNode.Parent?.Parent;
                if (navNode?.Tag is RenderNav renderNav)
                    targetOBJData = renderNav.GetData();
            }
            else if (selectedNode.Tag is UnkSet0 set)
            {
                targetSet = set;
                setNode = selectedNode;
                TreeNode navNode = selectedNode.Parent?.Parent;
                if (navNode?.Tag is RenderNav renderNav)
                    targetOBJData = renderNav.GetData();
            }

            if (targetSet == null || targetOBJData == null)
            {
                MessageBox.Show("Cannot find parent navigation data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BoundingBox newBox = new BoundingBox(new Vector3(-1f), new Vector3(1f));
            var boxesList = targetSet.EdgeBoxes?.ToList() ?? new List<BoundingBox>();
            boxesList.Add(newBox);
            targetSet.EdgeBoxes = boxesList.ToArray();
            targetSet.NumEdges = targetSet.EdgeBoxes.Length;

            int newIndex = targetSet.EdgeBoxes.Length - 1;
            TreeNode newEdgeNode = new TreeNode($"EdgeBox {newIndex}: {newBox.Min} - {newBox.Max}");
            newEdgeNode.Name = $"EDGEBOX_{Array.IndexOf(targetOBJData.runtimeMesh.Cells.SelectMany(c => c.Sets).ToArray(), targetSet)}_{newIndex}";
            newEdgeNode.Tag = newBox;
            setNode.Nodes.Add(newEdgeNode);
            setNode.Expand();
            dSceneTree.SelectedNode = newEdgeNode;

            UpdateNavMeshVisualization(targetOBJData, targetSet);
        }
        private void UpdateNavMeshVisualization(OBJData objData, UnkSet0 set)
        {
            foreach (TreeNode rootNode in OBJDataRoot.Nodes)
            {
                if (rootNode.Tag is RenderNav nav && nav.GetData() == objData)
                {
                    foreach (TreeNode cellNode in rootNode.Nodes)
                    {
                        if (cellNode.Tag is KynogonRuntimeMesh.Cell cell && cell.Sets.Contains(set))
                        {
                            RenderNavCell renderCell = cellNode.Tag as RenderNavCell;
                            if (renderCell != null)
                            {
                                renderCell.Shutdown();
                            }
                            RenderNavCell newRenderCell = new RenderNavCell(Graphics);
                            newRenderCell.Init(cell);
                            cellNode.Tag = newRenderCell;
                            newRenderCell.SetVisibility(cellNode.Checked);
                            break;
                        }
                    }
                    break;
                }
            }
            Graphics.Frame();
        }

        public class SoundSectorData
        {
            public string Name { get; set; }
            public List<Plane4> Planes { get; set; } = new List<Plane4>();
            public int RefID { get; set; }

            [Browsable(false)]
            public RenderBoundingBox RenderBox { get; set; }
            public string Type { get; set; }
            public List<ushort> Unk0 { get; set; } = new List<ushort>();
            public uint Unk1 { get; set; }
            public uint Unk2 { get; set; }
            public short Unk3 { get; set; }
            public ushort Unk4 { get; set; }
            public ushort Unk5 { get; set; }
            public bool bBasicSceneOnly { get; set; }

            public void RebuildRenderBox()
            {
                if (RenderBox == null) return;

                List<Vector3> vertices = MapEditor.CalculateVerticesFromPlanes(Planes, 50.0f);
                if (vertices.Count >= 2)
                {
                    Vector3 min = vertices[0], max = vertices[0];
                    foreach (var v in vertices)
                    {
                        min = Vector3.Min(min, v);
                        max = Vector3.Max(max, v);
                    }
                    BoundingBox bbox = new BoundingBox(min, max);
                    RenderBox.Update(bbox);
                }
            }

            private Vector3 _min;
            private Vector3 _max;

            [Category("Bounds")]
            [Description("Minimum corner of the sector box")]
            public Vector3 Min
            {
                get => _min;
                set
                {
                    if (_min != value)
                    {
                        _min = value;
                        UpdateFromBounds();
                    }
                }
            }

            [Category("Bounds")]
            [Description("Maximum corner of the sector box")]
            public Vector3 Max
            {
                get => _max;
                set
                {
                    if (_max != value)
                    {
                        _max = value;
                        UpdateFromBounds();
                    }
                }
            }

            public void UpdateFromBounds()
            {
                Planes.Clear();
                Planes.Add(new Plane4 { X = 1, Y = 0, Z = 0, W = -_max.X });
                Planes.Add(new Plane4 { X = -1, Y = 0, Z = 0, W = _min.X });
                Planes.Add(new Plane4 { X = 0, Y = 1, Z = 0, W = -_max.Y });
                Planes.Add(new Plane4 { X = 0, Y = -1, Z = 0, W = _min.Y });
                Planes.Add(new Plane4 { X = 0, Y = 0, Z = 1, W = -_max.Z });
                Planes.Add(new Plane4 { X = 0, Y = 0, Z = -1, W = _min.Z });

                RebuildRenderBox();
            }

            public void UpdateBoundsFromPlanes()
            {
                if (Planes == null || Planes.Count == 0)
                    return;

                float minX = float.MaxValue, maxX = float.MinValue;
                float minY = float.MaxValue, maxY = float.MinValue;
                float minZ = float.MaxValue, maxZ = float.MinValue;
                bool hasAxisPlanes = false;

                foreach (var p in Planes)
                {
                    Vector3 normal = new Vector3(p.X, p.Y, p.Z);
                    float d = p.W;

                    if (Math.Abs(normal.X) > 0.99f)
                    {
                        if (normal.X > 0) maxX = -d;
                        else minX = d;
                        hasAxisPlanes = true;
                    }
                    else if (Math.Abs(normal.Y) > 0.99f)
                    {
                        if (normal.Y > 0) maxY = -d;
                        else minY = d;
                        hasAxisPlanes = true;
                    }
                    else if (Math.Abs(normal.Z) > 0.99f)
                    {
                        if (normal.Z > 0) maxZ = -d;
                        else minZ = d;
                        hasAxisPlanes = true;
                    }
                }

                if (hasAxisPlanes &&
                    minX != float.MaxValue && maxX != float.MinValue &&
                    minY != float.MaxValue && maxY != float.MinValue &&
                    minZ != float.MaxValue && maxZ != float.MinValue)
                {
                    _min = new Vector3(minX, minY, minZ);
                    _max = new Vector3(maxX, maxY, maxZ);
                }
                else
                {
                    List<Vector3> vertices = MapEditor.CalculateVerticesFromPlanes(Planes, 50.0f);
                    if (vertices.Count >= 2)
                    {
                        Vector3 min = vertices[0], max = vertices[0];
                        foreach (var v in vertices)
                        {
                            min = Vector3.Min(min, v);
                            max = Vector3.Max(max, v);
                        }
                        _min = min;
                        _max = max;
                    }
                    else
                    {
                        _min = Vector3.Zero;
                        _max = Vector3.Zero;
                    }
                }
            }
        }

        public class PortalData
        {
            public string Name { get; set; }
            public Vector3 Position { get; set; }
            public float Unk0 { get; set; }
            public float OpenRatio { get; set; }
            public string LinkA { get; set; }
            public byte Unk2 { get; set; }
            public string LinkB { get; set; }
            public byte Unk3 { get; set; }
            public float CostFactor { get; set; }
            public string EntityName { get; set; }
            public byte Unk6 { get; set; }
            public byte bVolumeFactorEnabled { get; set; }
            public float VolumeFactor { get; set; }
            public int RefID { get; set; }
            public RenderBoundingBox RenderBox { get; set; }
        }

        public class Plane4
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }
        }

        private void BtnLoadSoundSectors_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Sound Sectors XML file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    LoadAndVisualizeSoundSectors(openFileDialog.FileName);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Sound sectors loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error loading sound sectors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAndVisualizeSoundSectors(string xmlFilePath)
        {
            foreach (var sector in loadedSoundSectors)
                if (sector.RenderBox != null)
                    Graphics.DeleteAsset(sector.RefID);
            loadedSoundSectors.Clear();

            foreach (var portal in loadedPortals)
                if (portal.RenderBox != null)
                    Graphics.DeleteAsset(portal.RefID);
            loadedPortals.Clear();

            TreeNode[] oldSectorNodes = dSceneTree.TreeView.Nodes.Find("SoundSectors", false);
            foreach (var node in oldSectorNodes) dSceneTree.RemoveNode(node);
            TreeNode[] oldPortalNodes = dSceneTree.TreeView.Nodes.Find("SoundPortals", false);
            foreach (var node in oldPortalNodes) dSceneTree.RemoveNode(node);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNode root = xmlDoc.SelectSingleNode("SoundSectorResource");
            if (root == null) throw new Exception("Invalid SoundSectorResource XML format");

            XmlNode rootNameNode = root.SelectSingleNode("Name");
            _soundSectorsRootName = rootNameNode?.InnerText;

            XmlNode hashesNode = root.SelectSingleNode("Hashes");
            _soundSectorsHashes.Clear();
            if (hashesNode != null)
            {
                foreach (XmlNode hashNode in hashesNode.SelectNodes("UInt64"))
                    _soundSectorsHashes.Add(ulong.Parse(hashNode.InnerText));
            }

            TreeNode soundSectorsRoot = new TreeNode("Sound Sectors") { Tag = "Folder", Name = "SoundSectors" };
            TreeNode portalsRoot = new TreeNode("Portals") { Tag = "Folder", Name = "SoundPortals" };

            XmlNode sectorsNode = root.SelectSingleNode("Sectors");
            if (sectorsNode != null)
            {
                foreach (XmlNode elementNode in sectorsNode.SelectNodes("Element"))
                {
                    string type = elementNode.Attributes["Type"]?.Value;
                    if (type != "SoundSectorNormal" && type != "SoundSectorPrimary")
                        continue;

                    SoundSectorData sector = new SoundSectorData();
                    sector.RefID = RefManager.GetNewRefID();
                    sector.Type = type;

                    XmlNode nameNode = elementNode.SelectSingleNode("Name");
                    sector.Name = nameNode?.InnerText ?? $"Sector_{loadedSoundSectors.Count}";

                    XmlNode unk0Node = elementNode.SelectSingleNode("Unk0");
                    if (unk0Node != null)
                    {
                        foreach (XmlNode u16 in unk0Node.SelectNodes("UInt16"))
                            sector.Unk0.Add(ushort.Parse(u16.InnerText));
                    }

                    XmlNode unk1Node = elementNode.SelectSingleNode("Unk1");
                    if (unk1Node != null)
                        sector.Unk1 = uint.Parse(unk1Node.InnerText);

                    XmlNode unk2Node = elementNode.SelectSingleNode("Unk2");
                    if (unk2Node != null)
                        sector.Unk2 = uint.Parse(unk2Node.InnerText);

                    XmlNode unk3Node = elementNode.SelectSingleNode("Unk3");
                    if (unk3Node != null)
                        sector.Unk3 = short.Parse(unk3Node.InnerText);

                    XmlNode unk4Node = elementNode.SelectSingleNode("Unk4");
                    if (unk4Node != null)
                        sector.Unk4 = ushort.Parse(unk4Node.InnerText);

                    XmlNode unk5Node = elementNode.SelectSingleNode("Unk5");
                    if (unk5Node != null)
                        sector.Unk5 = ushort.Parse(unk5Node.InnerText);

                    XmlNode basicNode = elementNode.SelectSingleNode("bBasicSceneOnly");
                    if (basicNode != null)
                        sector.bBasicSceneOnly = bool.Parse(basicNode.InnerText);

                    XmlNode planesNode = elementNode.SelectSingleNode("Planes");
                    if (planesNode != null)
                    {
                        foreach (XmlNode planeNode in planesNode.SelectNodes("Element"))
                        {
                            Plane4 plane = new Plane4
                            {
                                X = float.Parse(planeNode.SelectSingleNode("X")?.InnerText ?? "0", CultureInfo.InvariantCulture),
                                Y = float.Parse(planeNode.SelectSingleNode("Y")?.InnerText ?? "0", CultureInfo.InvariantCulture),
                                Z = float.Parse(planeNode.SelectSingleNode("Z")?.InnerText ?? "0", CultureInfo.InvariantCulture),
                                W = float.Parse(planeNode.SelectSingleNode("W")?.InnerText ?? "0", CultureInfo.InvariantCulture)
                            };
                            sector.Planes.Add(plane);
                        }
                    }

                    if (sector.Planes.Count >= 4)
                    {
                        List<Vector3> vertices = CalculateVerticesFromPlanes(sector.Planes, 50.0f);
                        if (vertices.Count >= 2)
                        {
                            Vector3 min = vertices[0], max = vertices[0];
                            foreach (var v in vertices)
                            {
                                min = Vector3.Min(min, v);
                                max = Vector3.Max(max, v);
                            }
                            sector.RenderBox = new RenderBoundingBox();
                            sector.RenderBox.SetColour(System.Drawing.Color.Lime);
                            sector.RenderBox.Init(new BoundingBox(min, max));
                            sector.RenderBox.SetTransform(Matrix4x4.Identity);
                            Graphics.InitObjectStack[sector.RefID] = sector.RenderBox;
                        }
                    }

                    TreeNode sectorNode = new TreeNode(sector.Name) { Name = sector.RefID.ToString(), Tag = sector };
                    soundSectorsRoot.Nodes.Add(sectorNode);
                    loadedSoundSectors.Add(sector);
                    sector.UpdateBoundsFromPlanes();
                }
            }

            XmlNode portalsNode = root.SelectSingleNode("Portals");
            if (portalsNode != null)
            {
                foreach (XmlNode portalNode in portalsNode.SelectNodes("PortalSphere"))
                {
                    PortalData portal = new PortalData();
                    portal.RefID = RefManager.GetNewRefID();

                    portal.Name = portalNode.SelectSingleNode("Name")?.InnerText ?? $"Portal_{loadedPortals.Count}";

                    XmlNode posNode = portalNode.SelectSingleNode("Position");
                    if (posNode != null)
                    {
                        portal.Position = new Vector3(
                            float.Parse(posNode.SelectSingleNode("X")?.InnerText ?? "0", CultureInfo.InvariantCulture),
                            float.Parse(posNode.SelectSingleNode("Y")?.InnerText ?? "0", CultureInfo.InvariantCulture),
                            float.Parse(posNode.SelectSingleNode("Z")?.InnerText ?? "0", CultureInfo.InvariantCulture));
                    }

                    portal.Unk0 = float.Parse(portalNode.SelectSingleNode("Unk0")?.InnerText ?? "5", CultureInfo.InvariantCulture);
                    portal.OpenRatio = float.Parse(portalNode.SelectSingleNode("OpenRatio")?.InnerText ?? "0.05", CultureInfo.InvariantCulture);
                    portal.LinkA = portalNode.SelectSingleNode("LinkA")?.InnerText ?? "";
                    portal.Unk2 = byte.Parse(portalNode.SelectSingleNode("Unk2")?.InnerText ?? "0");
                    portal.LinkB = portalNode.SelectSingleNode("LinkB")?.InnerText ?? "";
                    portal.Unk3 = byte.Parse(portalNode.SelectSingleNode("Unk3")?.InnerText ?? "0");
                    portal.CostFactor = float.Parse(portalNode.SelectSingleNode("CostFactor")?.InnerText ?? "8", CultureInfo.InvariantCulture);
                    portal.EntityName = portalNode.SelectSingleNode("EntityName")?.InnerText ?? "";
                    portal.Unk6 = byte.Parse(portalNode.SelectSingleNode("Unk6")?.InnerText ?? "1");
                    portal.bVolumeFactorEnabled = byte.Parse(portalNode.SelectSingleNode("bVolumeFactorEnabled")?.InnerText ?? "1");
                    portal.VolumeFactor = float.Parse(portalNode.SelectSingleNode("VolumeFactor")?.InnerText ?? "0.4", CultureInfo.InvariantCulture);

                    float size = portal.Unk0 * 0.1f;
                    BoundingBox bbox = new BoundingBox(portal.Position - new Vector3(size), portal.Position + new Vector3(size));
                    portal.RenderBox = new RenderBoundingBox();
                    portal.RenderBox.SetColour(System.Drawing.Color.Orange);
                    portal.RenderBox.Init(bbox);
                    portal.RenderBox.SetTransform(Matrix4x4.Identity);
                    Graphics.InitObjectStack[portal.RefID] = portal.RenderBox;

                    TreeNode portalNodeTree = new TreeNode(portal.Name) { Name = portal.RefID.ToString(), Tag = portal };
                    portalNodeTree.Nodes.Add($"Links: {portal.LinkA} <-> {portal.LinkB}");
                    portalsRoot.Nodes.Add(portalNodeTree);
                    loadedPortals.Add(portal);
                }
            }

            if (soundSectorsRoot.Nodes.Count > 0)
                dSceneTree.AddToTree(soundSectorsRoot);
            if (portalsRoot.Nodes.Count > 0)
                dSceneTree.AddToTree(portalsRoot);

            _soundSectorsFilePath = xmlFilePath;
        }


        private void BtnAddSoundSector_Click(object sender, EventArgs e)
        {
            SoundSectorData newSector = new SoundSectorData();
            newSector.RefID = RefManager.GetNewRefID();
            newSector.Type = "SoundSectorNormal";
            newSector.Name = "NewSector";

            if (loadedSoundSectors.Count > 0)
            {
                var first = loadedSoundSectors[0];
                newSector.Unk0 = new List<ushort>(first.Unk0);
                newSector.Unk1 = first.Unk1;
                newSector.Unk2 = first.Unk2;
                newSector.Unk3 = first.Unk3;
                newSector.Unk4 = first.Unk4;
                newSector.Unk5 = first.Unk5;
                newSector.bBasicSceneOnly = first.bBasicSceneOnly;
            }
            else
            {
                newSector.Unk0 = new List<ushort> { 1, 2, 3, 4, 5, 6, 7, 8 };
                newSector.Unk1 = 0;
                newSector.Unk2 = 0;
                newSector.Unk3 = 0;
                newSector.Unk4 = 0;
                newSector.Unk5 = 0;
                newSector.bBasicSceneOnly = false;
            }

            newSector.Min = new Vector3(-5, -5, -5);
            newSector.Max = new Vector3(5, 5, 5);

            newSector.RenderBox = new RenderBoundingBox();
            newSector.RenderBox.SetColour(System.Drawing.Color.Lime);
            newSector.RebuildRenderBox();
            Graphics.InitObjectStack[newSector.RefID] = newSector.RenderBox;
            loadedSoundSectors.Add(newSector);

            TreeNode soundSectorsRoot = null;
            foreach (TreeNode node in dSceneTree.TreeView.Nodes)
            {
                if (node.Name == "SoundSectors")
                {
                    soundSectorsRoot = node;
                    break;
                }
            }
            if (soundSectorsRoot == null)
            {
                soundSectorsRoot = new TreeNode("Sound Sectors") { Tag = "Folder", Name = "SoundSectors" };
                dSceneTree.AddToTree(soundSectorsRoot);
            }

            TreeNode sectorNode = new TreeNode(newSector.Name) { Name = newSector.RefID.ToString(), Tag = newSector };
            soundSectorsRoot.Nodes.Add(sectorNode);
            soundSectorsRoot.Expand();

            dSceneTree.SelectedNode = sectorNode;
            TreeViewUpdateSelected();
            dPropertyGrid.SetObject(newSector);
        }

        private void BtnAddSoundPortal_Click(object sender, EventArgs e)
        {
            PortalData newPortal = new PortalData();
            newPortal.RefID = RefManager.GetNewRefID();
            newPortal.Name = "NewPortal";
            newPortal.Position = Vector3.Zero;
            newPortal.Unk0 = 1f;
            newPortal.OpenRatio = 0f;
            newPortal.LinkA = "Name";
            newPortal.Unk2 = 0;
            newPortal.LinkB = "Name";
            newPortal.Unk3 = 0;
            newPortal.CostFactor = 0f;
            newPortal.EntityName = "Name";
            newPortal.Unk6 = 0;
            newPortal.bVolumeFactorEnabled = 0;
            newPortal.VolumeFactor = 0f;

            float size = newPortal.Unk0 * 0.1f;
            BoundingBox bbox = new BoundingBox(newPortal.Position - new Vector3(size), newPortal.Position + new Vector3(size));
            newPortal.RenderBox = new RenderBoundingBox();
            newPortal.RenderBox.SetColour(System.Drawing.Color.Orange);
            newPortal.RenderBox.Init(bbox);
            newPortal.RenderBox.SetTransform(Matrix4x4.Identity);
            Graphics.InitObjectStack[newPortal.RefID] = newPortal.RenderBox;

            loadedPortals.Add(newPortal);

            TreeNode portalsRoot = null;
            foreach (TreeNode node in dSceneTree.TreeView.Nodes)
            {
                if (node.Name == "SoundPortals")
                {
                    portalsRoot = node;
                    break;
                }
            }
            if (portalsRoot == null)
            {
                portalsRoot = new TreeNode("Portals") { Tag = "Folder", Name = "SoundPortals" };
                dSceneTree.AddToTree(portalsRoot);
            }

            TreeNode portalNode = new TreeNode(newPortal.Name) { Name = newPortal.RefID.ToString(), Tag = newPortal };
            portalNode.Nodes.Add($"Links: {newPortal.LinkA} <-> {newPortal.LinkB}");
            portalsRoot.Nodes.Add(portalNode);
            portalsRoot.Expand();

            dSceneTree.SelectedNode = portalNode;
            TreeViewUpdateSelected();
            dPropertyGrid.SetObject(newPortal);
        }

        private void BtnDeleteSoundItem_Click(object sender, EventArgs e)
        {
            TreeNode selected = dSceneTree.SelectedNode;
            if (selected == null)
            {
                MessageBox.Show("No item selected.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected.Tag is SoundSectorData sector)
            {
                if (sector.RenderBox != null)
                    Graphics.DeleteAsset(sector.RefID);
                loadedSoundSectors.Remove(sector);
                dSceneTree.RemoveNode(selected);
            }
            else if (selected.Tag is PortalData portal)
            {
                if (portal.RenderBox != null)
                    Graphics.DeleteAsset(portal.RefID);
                loadedPortals.Remove(portal);
                dSceneTree.RemoveNode(selected);
            }
            else
            {
                MessageBox.Show("Selected item is not a sound sector or portal.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static List<Vector3> CalculateVerticesFromPlanes(List<Plane4> planes, float margin = 50.0f)
        {
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < planes.Count; i++)
            {
                var p = planes[i];
                Console.WriteLine($"  Plane {i}: normal=({p.X:F6}, {p.Y:F6}, {p.Z:F6}), d={p.W:F6}");
            }

            int totalCombinations = 0;
            for (int i = 0; i < planes.Count - 2; i++)
            {
                for (int j = i + 1; j < planes.Count - 1; j++)
                {
                    for (int k = j + 1; k < planes.Count; k++)
                    {
                        totalCombinations++;
                    }
                }
            }

            int validCount = 0;
            for (int i = 0; i < planes.Count - 2; i++)
            {
                for (int j = i + 1; j < planes.Count - 1; j++)
                {
                    for (int k = j + 1; k < planes.Count; k++)
                    {
                        Vector3? point = PlaneIntersection(planes[i], planes[j], planes[k]);

                        if (point.HasValue)
                        {
                            bool isInside = PointInSector(point.Value, planes, margin);

                            if (isInside)
                            {
                                vertices.Add(point.Value);
                                validCount++;
                            }
                        }
                    }
                }
            }
            List<Vector3> uniqueVerts = RemoveDuplicateVertices(vertices, tolerance: 1.0f);

            return uniqueVerts;
        }

        private static Vector3? PlaneIntersection(Plane4 p1, Plane4 p2, Plane4 p3)
        {
            Vector3 n1 = new Vector3(p1.X, p1.Y, p1.Z);
            Vector3 n2 = new Vector3(p2.X, p2.Y, p2.Z);
            Vector3 n3 = new Vector3(p3.X, p3.Y, p3.Z);

            float d1 = p1.W;
            float d2 = p2.W;
            float d3 = p3.W;

            Vector3 cross23 = Vector3.Cross(n2, n3);
            float denom = Vector3.Dot(n1, cross23);

            if (Math.Abs(denom) < 1e-6f)
            {
                return null;
            }

            Vector3 cross31 = Vector3.Cross(n3, n1);
            Vector3 cross12 = Vector3.Cross(n1, n2);

            Vector3 point = (-d1 * cross23 - d2 * cross31 - d3 * cross12) / denom;

            return point;
        }

        private static bool PointInSector(Vector3 point, List<Plane4> planes, float margin = 50.0f)
        {
            foreach (var p in planes)
            {
                Vector3 normal = new Vector3(p.X, p.Y, p.Z);
                float d = p.W;
                float dist = Vector3.Dot(normal, point) + d;

                if (dist > margin)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<Vector3> RemoveDuplicateVertices(List<Vector3> vertices, float tolerance = 1.0f)
        {
            List<Vector3> unique = new List<Vector3>();

            foreach (var v in vertices)
            {
                bool isDuplicate = false;
                foreach (var uv in unique)
                {
                    if (Vector3.Distance(v, uv) < tolerance)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate)
                {
                    unique.Add(v);
                }
            }

            return unique;
        }

        private void RenderPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            decimal value = (e.Delta > 0 ? CameraSpeedTool.Increment : -CameraSpeedTool.Increment);
            CameraSpeedTool.Value += value;
        }

        private void RenderPanel_Resize(object sender, EventArgs e)
        {
            // TODO: Do we need to restructure the initialisation order for this form?
            // On some PCs this resized before graphics has initialised.
            if (Graphics != null)
            {
                Graphics.OnResize(RenderPanel.Width, RenderPanel.Height);
            }
        }

        private void OnTreeViewNodeDropped(object sender, TreeViewDragEventArgs e)
        {
            if (e.DraggedNode.Tag is FrameHeaderScene || e.DraggedNode.Tag is FrameHeader)
            {
                return;
            }
            if (e.DraggedNode.Tag is not FrameObjectBase)
            {
                return;
            }
            if (e.DragButton == MouseButtons.Left)
            {
                FrameEntry NewParent = (e.TargetNode.Tag != null ? e.TargetNode.Tag as FrameEntry : null);
                int ParentRefID = (NewParent != null ? NewParent.RefID : -1);
                UpdateObjectParents(ParentInfo.ParentType.ParentIndex1, ParentRefID, NewParent);
            }
            else if (e.DragButton == MouseButtons.Right)
            {
                FrameEntry NewParent = (e.TargetNode.Tag != null ? e.TargetNode.Tag as FrameEntry : null);
                int ParentRefID = (NewParent != null ? NewParent.RefID : -1);
                UpdateObjectParents(ParentInfo.ParentType.ParentIndex2, ParentRefID, NewParent);
            }
            else if (e.DragButton == MouseButtons.Middle)
            {
                TreeNode node1 = (e.TargetNode != null ? e.TargetNode : null);
                TreeNode node2 = (e.DraggedNode != null ? e.DraggedNode : null);
                SwitchFrames(node1, node2);
            }
        }

        // TODO: The fetching of the actor file should be inside SceneData,
        // or whatever I can the Multi-SDS class later on.
        private void LinkToActor_Click(object sender, EventArgs e)
        {
            var node = dSceneTree.SelectedNode;
            if (node == null)
            {
                // Not selecting a node
                return;
            }
            if (node.Tag == null)
            {
                // Doesn't have any valid data
                return;
            }
            if (SceneData.Actors == null || SceneData.Actors.Length == 0)
            {
                SceneData.Actors = new Actor[0];
                SceneData.CreateNewActor();
                LoadActorFiles();
            }
            // Should have atleast one file, try to link actors.
            if (SceneData.Actors.Length > 0 && SceneData.Actors[0] != null)
            {
                FrameObjectFrame frame = (node.Tag as FrameObjectFrame);
                NewObjectForm objectForm = new NewObjectForm(true);
                objectForm.SetLabel("$SELECT_TYPE_AND_NAME");
                ActorItemAddOption optionControl = new ActorItemAddOption();
                objectForm.LoadOption(optionControl);
                if (objectForm.ShowDialog() == DialogResult.OK)
                {
                    //create the new entry
                    ActorTypes type = optionControl.GetSelectedType();
                    string def = optionControl.GetDefinitionName();
                    string framedef = optionControl.GetFrameName();
                    ActorEntry entry = SceneData.Actors[0].CreateActorEntry(type, objectForm.GetInputText());
                    entry.DefinitionName = def;
                    entry.FrameName = framedef;
                    entry.FrameName = frame.Name.String;
                    entry.FrameNameHash = frame.Name.Hash;
                    frame.Item = entry;

                    //create the definition
                    ActorDefinition definition = SceneData.Actors[0].CreateActorDefinition(entry);
                    definition.FrameIndex = (uint)SceneData.FrameResource.FrameObjects.IndexOfValue(frame.RefID);
                    frame.ActorHash.Set(definition.Name);

                    //create the node
                    TreeNode entityNode = new TreeNode("actor_" + entry.EntityName);
                    entityNode.Text = entry.EntityName;
                    entityNode.Tag = entry;

                    //now add the node to the scene tree
                    var typeString = string.Format("actorType_" + entry.ActorTypeName);
                    var foundnodes = actorRoot.Nodes[0].Nodes.Find(typeString, false);
                    if (foundnodes.Length > 0)
                    {
                        dSceneTree.AddToTree(entityNode, foundnodes[0]);
                    }
                    else
                    {
                        TreeNode typeNode = new TreeNode(typeString);
                        typeNode.Name = typeString;
                        typeNode.Text = entry.ActorTypeName;
                        typeNode.Nodes.Add(entityNode);
                        dSceneTree.AddToTree(typeNode, actorRoot.Nodes[0]);
                    }
                }
                objectForm.Dispose();
            }
        }

        private void ExportFrame_Click(object sender, EventArgs e)
        {
            var node = dSceneTree.SelectedNode;
            if (node.Tag.GetType() == typeof(FrameHeaderScene) || node.Tag.GetType() == typeof(FrameHeader))//this should catch scenes and frameresource content
            {
                //todo manage exporting scenes, skip frameheader
                return;
            }
            FrameObjectBase frame = (node.Tag as FrameObjectBase);
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (SaveFileDialog != null)
                    {
                        SaveFileDialog.Reset();
                    }
                    string ExportName = null;
                    SaveFileDialog.FileName = frame.Name.String;
                    SaveFileDialog.RestoreDirectory = true;
                    SaveFileDialog.Filter = "FrameData File (*.framedata)|*.framedata*";
                    SaveFileDialog.FilterIndex = 1;
                    SaveFileDialog.DefaultExt = "framedata";
                    if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportName = SaveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                    SceneData.FrameResource.SaveFramesToFile(ExportName, frame);
                }
            }
        }

        private void UpdateAssetVisualisation(TreeNode node, TreeNode parent)
        {
            if (node.Tag != null)
            {
                bool bIsFrame = FrameResource.IsFrameType(node.Tag);
                int result = -1;
                int.TryParse(node.Name, out result);
                bool isVisible = node.Checked && node.CheckIfParentsAreValid();

                if (bHideChildren && (node != parent))
                {
                    node.Checked = parent.Checked;
                    isVisible = parent.Checked && parent.CheckIfParentsAreValid();
                }
                if (node.Tag is RenderNavCell renderNavGrid)
                {
                    renderNavGrid.SetVisibility(isVisible);
                }
                else if (node == OBJDataRoot)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        if (child.Tag is RenderNavCell nav)
                        {
                            nav.SetVisibility(isVisible);
                        }
                    }
                }
                if (node.Tag is RenderNav renderNav)
                {
                    renderNav.SetVisible(isVisible);
                }
                else if (node == OBJDataRoot)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        if (child.Tag is RenderNav nav)
                        {
                            nav.SetVisible(isVisible);
                        }
                    }
                }
                else if (node.Tag is BoundingBox)
                {
                    node.Checked = parent.Checked;
                    return;
                }
                else if (node.Tag is Instance && node.Parent?.Tag is Object trObject)
                {
                    UpdateInstanceVisualisation(node, trObject, isVisible);
                }
                else if (node.Tag is Grid trGrid)
                {
                    bool enabled = isVisible;
                    if (enabled)
                    {
                        RebuildTranslokatorGrids();
                    }
                    else
                    {
                        int trGridIndex = Array.IndexOf(SceneData.Translokator.Grids, trGrid);
                        Graphics.SetTranslokatorGridEnabled(trGridIndex, enabled);
                    }
                }
                else
                {
                    int refID = bIsFrame ? (node.Tag as FrameEntry).RefID : result;
                    Graphics.SetAssetVisibility(refID, isVisible);
                }
            }
            foreach (TreeNode child in node.Nodes)
            {
                UpdateAssetVisualisation(child, node);
            }
        }

        private void OutlinerAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                dSceneTree.RemoveEventHandler("AfterCheck", new TreeViewEventHandler(OutlinerAfterCheck));
                UpdateAssetVisualisation(e.Node, e.Node);
                dSceneTree.SetEventHandler("AfterCheck", new TreeViewEventHandler(OutlinerAfterCheck));
            }
        }

        public void PopulateList()
        {
            TreeNode tree = SceneData.FrameResource.BuildTree(SceneData.FrameNameTable);
            tree.Tag = SceneData.FrameResource.Header;
            frameResourceRoot = tree;
            dSceneTree.AddToTree(tree);
        }

        public void PopulateImportedData(string ImportedFilename)
        {
            ImportedScene = new SceneData();
            ImportedScene.ScenePath = Path.GetDirectoryName(ImportedFilename);
            ImportedScene.BuildData(false);
            InitImportTree();
        }

        public void InitImportTree()
        {
            TreeNode Importedtree = ImportedScene.FrameResource.BuildTree(ImportedScene.FrameNameTable);
            Importedtree.Tag = ImportedScene.FrameResource.Header;
            importFRRoot = Importedtree;
            dImportSceneTree = new DockImportSceneTree(ImportedScene.ScenePath);
            dImportSceneTree.importButton.Click += new EventHandler(ImportButton_Click);
            dImportSceneTree.FormClosed += new FormClosedEventHandler(CancelButton_Click);
            dImportSceneTree.AddToTree(importFRRoot);
            dImportSceneTree.Owner = this;
            dImportSceneTree.Show();
            Button_ImportFrame.Enabled = false;//limiting users to one instance at a time
        }

        public void StartD3DPanel()
        {
            Init(RenderPanel.Handle);
            Run();
        }

        public bool Init(IntPtr handle)
        {
            bool result = false;
            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                Graphics.PreInit(handle);
                BuildRenderObjects();
                result = Graphics.InitScene(RenderPanel.Width, RenderPanel.Height);
            }
            if (Input == null)
            {
                Input = Graphics.Input;
            }
            return result;
        }

        public void Run()
        {
            RenderPanel.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            RenderPanel.KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
            RenderPanel.MouseDown += (s, e) => Input.ButtonDown(e.Button);
            RenderPanel.MouseUp += (s, e) => Input.ButtonUp(e.Button);
            RenderPanel.MouseMove += RenderForm_MouseMove;
            RenderPanel.MouseEnter += RenderPanel_MouseEnter;
            RenderLoop.Run(this, () => { if (!Frame()) Shutdown(); });
        }

        private void RenderPanel_MouseEnter(object sender, EventArgs e) => RenderPanel.Focus();
        private void RenderForm_MouseMove(object sender, MouseEventArgs e) => mousePos = new Point(e.Location.X, e.Location.Y);
        private void CullModeButton_Click(object sender, EventArgs e) => Graphics.ToggleD3DCullMode();
        private void FillModeButton_Click(object sender, EventArgs e) => Graphics.ToggleD3DFillMode();
        private void OnAfterSelect(object sender, TreeViewEventArgs e) => TreeViewUpdateSelected();
        private void SaveButton_Click(object sender, EventArgs e) => Save();
        private void SaveButtonScene_Click(object sender, EventArgs e) => SaveScene();
        private void SaveButtonCollision_Click(object sender, EventArgs e) => SaveCollision();
        private void SaveButtonSoundSector_Click(object sender, EventArgs e) => SaveSoundSectors();
        private void SaveButtonATP_Click(object sender, EventArgs e) => SaveATP();
        private void SaveButtonItemDesc_Click(object sender, EventArgs e) => SaveCollisionItemDesc();
        private void SaveButtonSelItemDesc_Click(object sender, EventArgs e) => SaveSELCollisionItemDesc();
        private void SaveButtonTranslocator_Click(object sender, EventArgs e) => SaveTranslocator();
        private void SaveButtonActor_Click(object sender, EventArgs e) => SaveActor();
        private void SaveButtonOBJDataClick(object sender, EventArgs e) => SaveOBJData();
        private void SaveButtonAIWorldClick(object sender, EventArgs e) => SaveAIWorld();
        private void PropertyGridOnClicked(object sender, EventArgs e) => dPropertyGrid.Show(dockPanel1, DockState.DockRight);
        private void SceneTreeOnClicked(object sender, EventArgs e) => dSceneTree.Show(dockPanel1, DockState.DockLeft);
        private void CurrentModeButton_ButtonClick(object sender, EventArgs e) => SwitchMode(!bSelectMode);
        private void ViewOptionProperties_Click(object sender, EventArgs e) => dViewProperties.Show(dockPanel1, DockState.DockRight);

        private void UpdateParent_Click(object sender, EventArgs e)
        {
            string name = (sender as ToolStripMenuItem).Name;
            ParentInfo.ParentType ParentType = (name == "UpdateParent1Button" ? ParentInfo.ParentType.ParentIndex1 : ParentInfo.ParentType.ParentIndex2);
            ListWindow window = new ListWindow();
            window.PopulateForm(ParentType, SceneData.FrameResource);

            if (window.ShowDialog() == DialogResult.OK)
            {
                FrameEntry NewParent = (window.chosenObject != null ? window.chosenObject as FrameEntry : null);
                int ParentRefID = (NewParent != null ? NewParent.RefID : -1);

                // Request parent update
                UpdateObjectParents(ParentType, ParentRefID, NewParent);
            }
        }

        private void SwitchFrames(TreeNode node1, TreeNode node2)
        {
            if (node1 != null && node2 != null &&
                node1.Tag is FrameObjectBase frame1 &&
                node2.Tag is FrameObjectBase frame2)
            {
                if (frame1.Parent != null && frame2.Parent != null && frame1.Parent.RefID == frame2.Parent.RefID)
                {
                    return;//switching objects under same parent is redundant
                }

                var tempRefs = frame1.Refs;
                var tempParent1 = frame1.ParentIndex1;
                var tempParent2 = frame1.ParentIndex2;
                var tempParent = frame1.Parent;

                frame1.ParentIndex1 = frame2.ParentIndex1;
                frame1.ParentIndex2 = frame2.ParentIndex2;
                frame1.Parent = frame2.Parent;
                frame1.Refs = frame2.Refs;

                frame2.ParentIndex1 = tempParent1;
                frame2.ParentIndex2 = tempParent2;
                frame2.Parent = tempParent;
                frame2.Refs = tempRefs;

                int tempIcon = node1.ImageIndex;
                int tepmIconSelect = node1.SelectedImageIndex;
                node1.Tag = frame2;
                node1.Name = frame2.RefID.ToString();
                node1.Text = frame2.ToString();
                node1.ImageIndex = node2.ImageIndex;
                node1.SelectedImageIndex = node2.SelectedImageIndex;

                node2.Tag = frame1;
                node2.Name = frame1.RefID.ToString();
                node2.Text = frame1.ToString();
                node2.ImageIndex = tempIcon;
                node2.SelectedImageIndex = tepmIconSelect;

                TreeNode parent1 = node1.Parent;
                TreeNode parent2 = node2.Parent;

                if (parent1 != null && parent2 != null)
                {
                    int index1 = node1.Index;
                    int index2 = node2.Index;
                    parent1.Nodes.RemoveAt(index1);
                    parent2.Nodes.RemoveAt(index2);
                    parent1.Nodes.Insert(index2, node1);
                    parent2.Nodes.Insert(index1, node2);
                }
                ApplyChangesToRenderable(frame1);
                ApplyChangesToRenderable(frame2);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SceneData.CleanData();
            RenderStorageSingleton.Instance.TextureCache.Clear();
            dSceneTree.Dispose();
            dImportSceneTree.Dispose();
            dPropertyGrid.Dispose();
            dViewProperties.Dispose();
            Shutdown();
        }

        private Vector3 MoveObjectWithMouse(float z, int sx, int sy)
        {
            Ray ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(RenderPanel.Size.Width, RenderPanel.Size.Height));
            Vector3 worldPosition = ray.Position + (ray.Direction * 1);
            for (int i = 0; i < 99999; i++)
            {
                worldPosition = ray.Position + (ray.Direction * i);
                if (worldPosition.Z - z < 10)
                {
                    break;
                }
            }
            return worldPosition;
        }

        public bool Frame()
        {
            bool bCameraUpdated = false;

            if (Input.IsKeyDown(Keys.Delete))
            {
                dSceneTree.DeleteButton.PerformClick();
            }

            // Gizmo mode switching: 1 = Translate, 2 = Rotate, 3 = Scale
            if (!Input.IsButtonDown(MouseButtons.Right)) // Only when not moving camera
            {
                if (Input.IsKeyDown(Keys.D1))
                {
                    Graphics.SetGizmoMode(GizmoMode.Translate);
                }
                else if (Input.IsKeyDown(Keys.D2))
                {
                    Graphics.SetGizmoMode(GizmoMode.Rotate);
                }
                else if (Input.IsKeyDown(Keys.D3))
                {
                    Graphics.SetGizmoMode(GizmoMode.Scale);
                }
            }

            // Undo/Redo: Ctrl+Z, Ctrl+Y
            if (Input.IsKeyDown(Keys.ControlKey))
            {
                if (Input.IsKeyDown(Keys.Z) && !undoKeyWasPressed)
                {
                    if (Graphics.Undo())
                    {
                        SyncGizmoToSelectedObject();
                        dPropertyGrid.RefreshPropertyGrid();
                    }
                    undoKeyWasPressed = true;
                }
                else if (!Input.IsKeyDown(Keys.Z))
                {
                    undoKeyWasPressed = false;
                }

                if (Input.IsKeyDown(Keys.Y) && !redoKeyWasPressed)
                {
                    if (Graphics.Redo())
                    {
                        SyncGizmoToSelectedObject();
                        dPropertyGrid.RefreshPropertyGrid();
                    }
                    redoKeyWasPressed = true;
                }
                else if (!Input.IsKeyDown(Keys.Y))
                {
                    redoKeyWasPressed = false;
                }
            }
            else
            {
                undoKeyWasPressed = false;
                redoKeyWasPressed = false;
            }

            // Toggle BBox selection mode with B key
            if (Input.IsKeyDown(Keys.B) && !bboxKeyWasPressed)
            {
                Graphics.BBoxSelectionMode = !Graphics.BBoxSelectionMode;
                Console.WriteLine($"BBox Selection Mode: {(Graphics.BBoxSelectionMode ? "ON" : "OFF")}");
                bboxKeyWasPressed = true;
            }
            else if (!Input.IsKeyDown(Keys.B))
            {
                bboxKeyWasPressed = false;
            }

            // Переключение режимов: R - поворот объектов, T - перемещение объектов
            bool rWasPressed = false;
            bool tWasPressed = false;

            bool rIsDown = Input.IsKeyDown(Keys.R);
            bool tIsDown = Input.IsKeyDown(Keys.T);

            if (rIsDown && !rWasPressed)
            {
                bSelectMode = true; // Режим поворота объектов
                Console.WriteLine("Режим: Поворот объектов (R)");
            }
            if (tIsDown && !tWasPressed)
            {
                bSelectMode = false; // Режим перемещения объектов
                Console.WriteLine("Режим: Перемещение объектов (T)");
            }

            rWasPressed = rIsDown;
            tWasPressed = tIsDown;

            // Режим поворота: выбор оси вращения
            RotationAxis currentRotationAxis = RotationAxis.All;
            bool xWasPressed = false;
            bool yWasPressed = false;
            bool zWasPressed = false;

            bool xIsDown = Input.IsKeyDown(Keys.X);
            bool yIsDown = Input.IsKeyDown(Keys.Y);
            bool zIsDown = Input.IsKeyDown(Keys.Z);

            if (bSelectMode)
            {
                if (xIsDown && !xWasPressed)
                {
                    currentRotationAxis = RotationAxis.X;
                    Console.WriteLine("Ось вращения: X");
                }
                if (yIsDown && !yWasPressed)
                {
                    currentRotationAxis = RotationAxis.Y;
                    Console.WriteLine("Ось вращения: Y");
                }
                if (zIsDown && !zWasPressed)
                {
                    currentRotationAxis = RotationAxis.Z;
                    Console.WriteLine("Ось вращения: Z");
                }
                if (Input.IsKeyDown(Keys.A) && !Input.IsKeyDown(Keys.LShiftKey))
                {
                    currentRotationAxis = RotationAxis.All;
                    Console.WriteLine("Ось вращения: Все (свободное вращение)");
                }
            }

            xWasPressed = xIsDown;
            yWasPressed = yIsDown;
            zWasPressed = zIsDown;

            if (RenderPanel.Focused)
            {
                if (Input.IsButtonDown(MouseButtons.Right))
                {
                    var dx = -0.25f * (mousePos.X - lastMousePos.X);
                    var dy = -0.25f * (mousePos.Y - lastMousePos.Y);
                    Graphics.RotateCamera(dx, dy);
                    bCameraUpdated = true;
                }
                else if (Input.IsButtonDown(MouseButtons.Left))
                {
                    // Handle gizmo manipulation
                    if (Graphics.IsGizmoActive())
                    {
                        // Continue gizmo manipulation during drag
                        Graphics.UpdateGizmoManipulation(mousePos.X, mousePos.Y, RenderPanel.Width, RenderPanel.Height);
                        SyncGizmoToSelectedObject();
                    }
                    else if (selectTimer <= 0.0f)
                    {
                        // Check if clicking on gizmo first
                        GizmoAxis clickedAxis = Graphics.PickGizmoAxis(mousePos.X, mousePos.Y, RenderPanel.Width, RenderPanel.Height);

                        if (clickedAxis != GizmoAxis.None)
                        {
                            // Start gizmo manipulation
                            Graphics.StartGizmoManipulation(clickedAxis, mousePos.X, mousePos.Y, RenderPanel.Width, RenderPanel.Height);
                        }
                        else if (bSelectMode)
                        {
                            // Normal object picking
                            Pick(mousePos.X, mousePos.Y);
                            selectTimer = 0.1f;
                        }
                        else
                        {
                            // Legacy: Move object with mouse (MoveObjectWithMouse mode)
                            if (dSceneTree.SelectedNode != null)
                            {
                                var node = dSceneTree.SelectedNode;
                                var tag = dSceneTree.SelectedNode.Tag;

                                if (FrameResource.IsFrameType(tag))
                                {
                                    FrameObjectBase fObject = (tag as FrameObjectBase);
                                    var translation = MoveObjectWithMouse(fObject.LocalTransform.Translation.Z, mousePos.X, mousePos.Y);
                                    var local = fObject.LocalTransform;
                                    translation.Z = local.Translation.Z;
                                    fObject.LocalTransform = Matrix4x4Extensions.SetTranslation(local, translation);
                                    TreeViewUpdateSelected();
                                    ApplyChangesToRenderable(fObject);
                                }
                                else if (tag is Collision.Placement)
                                {
                                    Collision.Placement placement = (tag as Collision.Placement);
                                    var translation = MoveObjectWithMouse(placement.Position.Z, mousePos.X, mousePos.Y);
                                    var local = placement.Position;
                                    translation.Z = local.Z;
                                    placement.Position = translation;
                                    TreeViewUpdateSelected();
                                    IRenderer asset;
                                    Graphics.Assets.TryGetValue(int.Parse(node.Name), out asset);
                                    RenderInstance instance = (asset as RenderInstance);
                                    instance.SetTransform(placement.Transform);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Mouse button released - end gizmo manipulation if active
                    if (Graphics.IsGizmoActive())
                    {
                        Graphics.EndGizmoManipulation();
                    }
                }

                // Перемещение выбранного объекта стрелками
                if (dSceneTree.SelectedNode != null && dSceneTree.SelectedNode.Tag != null)
                {
                    // Alt = медленное перемещение, Shift = быстрое перемещение
                    float moveSpeed = 0.1f;
                    if (Input.IsKeyDown(Keys.LMenu) || Input.IsKeyDown(Keys.RMenu))
                    {
                        moveSpeed = 0.01f; // Медленное перемещение с Alt
                    }
                    else if (Input.IsKeyDown(Keys.LShiftKey) || Input.IsKeyDown(Keys.RShiftKey))
                    {
                        moveSpeed = 1.0f; // Быстрое перемещение с Shift
                    }
                    Vector3 moveDelta = Vector3.Zero;

                    if (Input.IsKeyDown(Keys.Up))
                    {
                        moveDelta.Y += moveSpeed;
                    }
                    if (Input.IsKeyDown(Keys.Down))
                    {
                        moveDelta.Y -= moveSpeed;
                    }
                    if (Input.IsKeyDown(Keys.Left))
                    {
                        moveDelta.X -= moveSpeed;
                    }
                    if (Input.IsKeyDown(Keys.Right))
                    {
                        moveDelta.X += moveSpeed;
                    }

                    // Дополнительно: перемещение по Z с PageUp/PageDown
                    if (Input.IsKeyDown(Keys.PageUp))
                    {
                        moveDelta.Z += moveSpeed;
                    }
                    if (Input.IsKeyDown(Keys.PageDown))
                    {
                        moveDelta.Z -= moveSpeed;
                    }

                    // Применяем перемещение к выбранному объекту
                    if (moveDelta != Vector3.Zero)
                    {
                        var tag = dSceneTree.SelectedNode.Tag;

                        if (FrameResource.IsFrameType(tag))
                        {
                            FrameObjectBase fObject = (tag as FrameObjectBase);
                            var local = fObject.LocalTransform;
                            var currentPos = local.GetTranslation();
                            var newPos = currentPos + moveDelta;

                            fObject.LocalTransform = local.SetTranslation(newPos);
                            TreeViewUpdateSelected();
                            ApplyChangesToRenderable(fObject);
                        }
                        else if (tag is Collision.Placement)
                        {
                            Collision.Placement placement = (tag as Collision.Placement);
                            placement.Position += moveDelta;
                            TreeViewUpdateSelected();

                            IRenderer asset;
                            Graphics.Assets.TryGetValue(int.Parse(dSceneTree.SelectedNode.Name), out asset);
                            RenderInstance instance = (asset as RenderInstance);
                            instance.SetTransform(placement.Transform);
                        }
                    }

                    // Поворот объекта в режиме поворота (R)
                    if (bSelectMode)
                    {
                        float rotateSpeed = 0.05f; // Скорость вращения

                        // Применяем вращение к выбранному объекту
                        if (Input.IsKeyDown(Keys.Up) || Input.IsKeyDown(Keys.Down) ||
                            Input.IsKeyDown(Keys.Left) || Input.IsKeyDown(Keys.Right) ||
                            Input.IsKeyDown(Keys.PageUp) || Input.IsKeyDown(Keys.PageDown))
                        {
                            var tag = dSceneTree.SelectedNode.Tag;

                            if (FrameResource.IsFrameType(tag))
                            {
                                FrameObjectBase fObject = (tag as FrameObjectBase);
                                var transform = fObject.LocalTransform;

                                // Вращение в зависимости от выбранной оси
                                switch (currentRotationAxis)
                                {
                                    case RotationAxis.X:
                                        // Вращение только по оси X (стрелки Вверх/Вниш)
                                        if (Input.IsKeyDown(Keys.Up))
                                        {
                                            transform = Matrix4x4.CreateRotationX(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Down))
                                        {
                                            transform = Matrix4x4.CreateRotationX(-rotateSpeed) * transform;
                                        }
                                        // Left/Right тоже вращают по X (альтернативное управление)
                                        if (Input.IsKeyDown(Keys.Left))
                                        {
                                            transform = Matrix4x4.CreateRotationX(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Right))
                                        {
                                            transform = Matrix4x4.CreateRotationX(rotateSpeed) * transform;
                                        }
                                        break;

                                    case RotationAxis.Y:
                                        // Вращение только по оси Y (стрелки Влево/Вправо)
                                        if (Input.IsKeyDown(Keys.Left))
                                        {
                                            transform = Matrix4x4.CreateRotationY(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Right))
                                        {
                                            transform = Matrix4x4.CreateRotationY(-rotateSpeed) * transform;
                                        }
                                        // Up/Down тоже вращают по Y (альтернативное управление)
                                        if (Input.IsKeyDown(Keys.Up))
                                        {
                                            transform = Matrix4x4.CreateRotationY(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Down))
                                        {
                                            transform = Matrix4x4.CreateRotationY(-rotateSpeed) * transform;
                                        }
                                        break;

                                    case RotationAxis.Z:
                                        // Вращение только по оси Z (PageUp/PageDown)
                                        if (Input.IsKeyDown(Keys.PageUp))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.PageDown))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(-rotateSpeed) * transform;
                                        }
                                        // Стрелки тоже вращают по Z (альтернативное управление)
                                        if (Input.IsKeyDown(Keys.Up))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Down))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Left))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Right))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(rotateSpeed) * transform;
                                        }
                                        break;

                                    case RotationAxis.All:
                                    default:
                                        // Свободное вращение по всем осям (оригинальное поведение)
                                        if (Input.IsKeyDown(Keys.Up))
                                        {
                                            transform = Matrix4x4.CreateRotationX(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Down))
                                        {
                                            transform = Matrix4x4.CreateRotationX(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Left))
                                        {
                                            transform = Matrix4x4.CreateRotationY(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Right))
                                        {
                                            transform = Matrix4x4.CreateRotationY(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.PageUp))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.PageDown))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(-rotateSpeed) * transform;
                                        }
                                        break;
                                }

                                fObject.LocalTransform = transform;
                                TreeViewUpdateSelected();
                                ApplyChangesToRenderable(fObject);
                            }
                            else if (tag is Collision.Placement placement)
                            {
                                // Для Placement преобразуем положение и ориентацию
                                var transform = placement.Transform;

                                // Аналогичная логика для Placement
                                switch (currentRotationAxis)
                                {
                                    case RotationAxis.X:
                                        if (Input.IsKeyDown(Keys.Up))
                                        {
                                            transform = Matrix4x4.CreateRotationX(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Down))
                                        {
                                            transform = Matrix4x4.CreateRotationX(-rotateSpeed) * transform;
                                        }
                                        break;

                                    case RotationAxis.Y:
                                        if (Input.IsKeyDown(Keys.Left))
                                        {
                                            transform = Matrix4x4.CreateRotationY(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Right))
                                        {
                                            transform = Matrix4x4.CreateRotationY(-rotateSpeed) * transform;
                                        }
                                        break;

                                    case RotationAxis.Z:
                                        if (Input.IsKeyDown(Keys.PageUp))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.PageDown))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(-rotateSpeed) * transform;
                                        }
                                        break;

                                    case RotationAxis.All:
                                    default:
                                        if (Input.IsKeyDown(Keys.Up))
                                        {
                                            transform = Matrix4x4.CreateRotationX(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Down))
                                        {
                                            transform = Matrix4x4.CreateRotationX(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Left))
                                        {
                                            transform = Matrix4x4.CreateRotationY(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.Right))
                                        {
                                            transform = Matrix4x4.CreateRotationY(-rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.PageUp))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(rotateSpeed) * transform;
                                        }
                                        if (Input.IsKeyDown(Keys.PageDown))
                                        {
                                            transform = Matrix4x4.CreateRotationZ(-rotateSpeed) * transform;
                                        }
                                        break;
                                }

                                //placement.Transform = transform;
                                TreeViewUpdateSelected();

                                IRenderer asset;
                                Graphics.Assets.TryGetValue(int.Parse(dSceneTree.SelectedNode.Name), out asset);
                                if (asset is RenderInstance instance)
                                {
                                    instance.SetTransform(placement.Transform);
                                }
                            }
                        }
                    }
                }

                bCameraUpdated = Graphics.UpdateInput();

                if (selectTimer > 0.0f)
                {
                    selectTimer -= 0.1f;
                }
            }

            lastMousePos = mousePos;
            Graphics.Frame();

            if (bCameraUpdated)
            {
                // Hack: We have to remove the delegate before we can change the values, 
                // or we'll fire some unnecessary code.
                PositionXTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionYTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionZTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionXTool.Value = (decimal)Graphics.Camera.Position.X;
                PositionYTool.Value = (decimal)Graphics.Camera.Position.Y;
                PositionZTool.Value = (decimal)Graphics.Camera.Position.Z;
                PositionXTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                PositionYTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                PositionZTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
            }
            Process process = Process.GetCurrentProcess();
            Label_MemoryUsage.Text = string.Format("Usage: {0}", process.WorkingSet64.ConvertToMemorySize());
            Label_FPS.Text = Graphics.Profile.ToString();
            return true;
        }

        // Добавьте этот enum в начало класса (вне метода Frame)
        public enum RotationAxis
        {
            All,  // Свободное вращение
            X,    // Только по оси X
            Y,    // Только по оси Y
            Z     // Только по оси Z
        }

        private void SanitizeBuffers()
        {
            #region vertex sanitize;
            //vertex pool check
            var bufferPools = new Dictionary<ulong, bool>();
            foreach (var pool in SceneData.VertexBufferPool.Buffers)
            {
                bufferPools.Add(pool.Key, false);
            }
            foreach (KeyValuePair<int, FrameGeometry> pair in SceneData.FrameResource.FrameGeometries)
            {
                foreach (var lod in pair.Value.LOD)
                {
                    if (bufferPools.ContainsKey(lod.VertexBufferRef.Hash))
                    {
                        bufferPools[lod.VertexBufferRef.Hash] = true;
                    }
                }
            }
            for (int i = 0; i < bufferPools.Count; i++)
            {
                KeyValuePair<ulong, bool> pair = bufferPools.ElementAt(i);
                if (!pair.Value)
                {
                    SceneData.VertexBufferPool.RemoveBuffer(pair.Key);
                    Console.WriteLine("Removed Vertex Buffer {0}", pair.Key);
                }
            }
            #endregion vertex sanitize;
            #region index sanitize;
            //index pool check
            bufferPools = new Dictionary<ulong, bool>();
            foreach (var pool in SceneData.IndexBufferPool.Buffers)
            {
                bufferPools.Add(pool.Key, false);
            }
            foreach (KeyValuePair<int, FrameGeometry> pair in SceneData.FrameResource.FrameGeometries)
            {
                foreach (var lod in pair.Value.LOD)
                {
                    if (bufferPools.ContainsKey(lod.IndexBufferRef.Hash))
                    {
                        bufferPools[lod.IndexBufferRef.Hash] = true;
                    }
                }
            }
            for (int i = 0; i < bufferPools.Count; i++)
            {
                KeyValuePair<ulong, bool> pair = bufferPools.ElementAt(i);
                if (!pair.Value)
                {
                    SceneData.IndexBufferPool.RemoveBuffer(pair.Key);
                    Console.WriteLine("Removed Index Buffer {0}", pair.Key);
                }
            }
            #endregion index sanitize;
        }

        private void Save()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileLocation.FullName, FileMode.Create)))
                {
                    SceneData.FrameResource.WriteToFile(writer);
                }
                using (BinaryWriter writer = new BinaryWriter(File.Open(SceneData.FrameNameTable.FileName, FileMode.Create)))
                {
                    FrameNameTable nameTable = new FrameNameTable();
                    nameTable.FileName = SceneData.FrameNameTable.FileName;
                    nameTable.BuildDataFromResource(SceneData.FrameResource);
                    nameTable.WriteToFile(writer);
                    SceneData.FrameNameTable = nameTable;
                }
                SanitizeBuffers();
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();
                if (SceneData.Actors != null && ToolkitSettings.Experimental)
                {
                    for (int i = 0; i < SceneData.Actors.Length; i++)
                    {
                        FixActorDefintions(SceneData.Actors[i]);
                        SceneData.Actors[i].WriteToFile();
                    }
                }
                if (SceneData.OBJData != null && ToolkitSettings.Experimental)
                {
                    for (int i = 0; i < SceneData.OBJData.Length; i++)
                    {
                        var obj = SceneData.OBJData[i];
                        obj.WriteToFile();
                    }
                }
                if (SceneData.AIWorlds != null && ToolkitSettings.Experimental)
                {
                    foreach (NAVData navData in SceneData.AIWorlds)
                    {
                        if (navData.Data is AIWorld aiWorld)
                        {
                            foreach (TreeNode rootNode in dSceneTree.TreeView.Nodes)
                            {
                                if (rootNode.Tag == aiWorld)
                                {
                                    aiWorld.Types1.Clear();
                                    foreach (TreeNode groupNode in rootNode.Nodes)
                                    {
                                        if (groupNode.Tag is AIWorld_Type1 group)
                                        {
                                            group.AIPoints.Clear();
                                            foreach (TreeNode pointNode in groupNode.Nodes)
                                            {
                                                if (pointNode.Tag is IType point)
                                                {
                                                    group.AIPoints.Add(point);
                                                }
                                            }
                                            aiWorld.Types1.Add(group);
                                        }
                                    }
                                }
                            }
                            navData.WriteToFile();
                        }
                    }
                }
                if (SceneData.Collisions != null)
                {
                    Collision collision = new Collision();
                    collision.Name = SceneData.Collisions.Name;
                    for (int i = 0; i != collisionRoot.Nodes.Count; i++)
                    {
                        TreeNode node = collisionRoot.Nodes[i];
                        Collision.CollisionModel collisionModel = (node.Tag as Collision.CollisionModel);
                        collision.Models.Add(collisionModel.Hash, collisionModel);
                        for (int x = 0; x != node.Nodes.Count; x++)
                        {
                            TreeNode child = node.Nodes[x];
                            Collision.Placement placement = (child.Tag as Collision.Placement);
                            collision.Placements.Add(placement);
                        }
                    }
                    SceneData.Collisions = collision;
                    SceneData.Collisions.WriteToFile();
                }
                if (SceneData.Translokator != null && ToolkitSettings.Experimental)
                {
                    TranslokatorLoader translokator = SceneData.Translokator;
                    translokator.Grids = new Grid[translokatorRoot.Nodes[1].GetNodeCount(false)];
                    for (int i = 0; i < translokator.Grids.Length; i++)
                    {
                        Grid grid = (translokatorRoot.Nodes[1].Nodes[i].Tag as Grid);
                        translokator.Grids[i] = grid;
                    }
                    translokator.ObjectGroups = new ObjectGroup[translokatorRoot.Nodes[0].GetNodeCount(false)];
                    for (int i = 0; i < translokator.ObjectGroups.Length; i++)
                    {
                        ObjectGroup objectGroup = (translokatorRoot.Nodes[0].Nodes[i].Tag as ObjectGroup);
                        objectGroup.Objects = new ResourceTypes.Translokator.Object[translokatorRoot.Nodes[0].Nodes[i].GetNodeCount(false)];
                        for (int y = 0; y < objectGroup.Objects.Length; y++)
                        {
                            ResourceTypes.Translokator.Object obj = (translokatorRoot.Nodes[0].Nodes[i].Nodes[y].Tag as ResourceTypes.Translokator.Object);
                            obj.Instances = new Instance[translokatorRoot.Nodes[0].Nodes[i].Nodes[y].GetNodeCount(false)];
                            for (int z = 0; z < obj.Instances.Length; z++)
                            {
                                Instance instance = (translokatorRoot.Nodes[0].Nodes[i].Nodes[y].Nodes[z].Tag as Instance);
                                obj.Instances[z] = instance;
                            }
                            objectGroup.Objects[y] = obj;
                        }
                        translokator.ObjectGroups[i] = objectGroup;
                    }
                    translokator.WriteToFile(new FileInfo(SceneData.sdsContent.GetResourceFiles("Translokator", true)[0]));
                }
                SceneData.UpdateResourceType();
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveTranslocator()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (SceneData.Translokator != null && ToolkitSettings.Experimental)
                {
                    TranslokatorLoader translokator = SceneData.Translokator;
                    translokator.Grids = new Grid[translokatorRoot.Nodes[1].GetNodeCount(false)];
                    for (int i = 0; i < translokator.Grids.Length; i++)
                    {
                        Grid grid = (translokatorRoot.Nodes[1].Nodes[i].Tag as Grid);
                        translokator.Grids[i] = grid;
                    }
                    translokator.ObjectGroups = new ObjectGroup[translokatorRoot.Nodes[0].GetNodeCount(false)];
                    for (int i = 0; i < translokator.ObjectGroups.Length; i++)
                    {
                        ObjectGroup objectGroup = (translokatorRoot.Nodes[0].Nodes[i].Tag as ObjectGroup);
                        objectGroup.Objects = new ResourceTypes.Translokator.Object[translokatorRoot.Nodes[0].Nodes[i].GetNodeCount(false)];
                        for (int y = 0; y < objectGroup.Objects.Length; y++)
                        {
                            ResourceTypes.Translokator.Object obj = (translokatorRoot.Nodes[0].Nodes[i].Nodes[y].Tag as ResourceTypes.Translokator.Object);
                            obj.Instances = new Instance[translokatorRoot.Nodes[0].Nodes[i].Nodes[y].GetNodeCount(false)];
                            for (int z = 0; z < obj.Instances.Length; z++)
                            {
                                Instance instance = (translokatorRoot.Nodes[0].Nodes[i].Nodes[y].Nodes[z].Tag as Instance);
                                obj.Instances[z] = instance;
                            }
                            objectGroup.Objects[y] = obj;
                        }
                        translokator.ObjectGroups[i] = objectGroup;
                    }
                    translokator.WriteToFile(new FileInfo(SceneData.sdsContent.GetResourceFiles("Translokator", true)[0]));
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveSoundSectors()
        {
            if (string.IsNullOrEmpty(_soundSectorsFilePath))
            {
                MessageBox.Show("No sound sectors file loaded.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Save sound sectors to file?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration decl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(decl);

                XmlElement root = xmlDoc.CreateElement("SoundSectorResource");
                xmlDoc.AppendChild(root);

                if (!string.IsNullOrEmpty(_soundSectorsRootName))
                {
                    XmlElement nameElem = xmlDoc.CreateElement("Name");
                    nameElem.SetAttribute("Type", "String");
                    nameElem.InnerText = _soundSectorsRootName;
                    root.AppendChild(nameElem);
                }

                if (_soundSectorsHashes.Count > 0)
                {
                    XmlElement hashesElem = xmlDoc.CreateElement("Hashes");
                    foreach (ulong hash in _soundSectorsHashes)
                    {
                        XmlElement hashElem = xmlDoc.CreateElement("UInt64");
                        hashElem.InnerText = hash.ToString();
                        hashesElem.AppendChild(hashElem);
                    }
                    root.AppendChild(hashesElem);
                }

                XmlElement sectorsElem = xmlDoc.CreateElement("Sectors");
                foreach (var sector in loadedSoundSectors)
                {
                    XmlElement elem = xmlDoc.CreateElement("Element");
                    elem.SetAttribute("Type", sector.Type);

                    XmlElement planesElem = xmlDoc.CreateElement("Planes");
                    foreach (var plane in sector.Planes)
                    {
                        XmlElement planeElem = xmlDoc.CreateElement("Element");
                        planeElem.SetAttribute("Type", "Plane");
                        AddElementWithType(xmlDoc, planeElem, "X", "Single", plane.X);
                        AddElementWithType(xmlDoc, planeElem, "Y", "Single", plane.Y);
                        AddElementWithType(xmlDoc, planeElem, "Z", "Single", plane.Z);
                        AddElementWithType(xmlDoc, planeElem, "W", "Single", plane.W);
                        planesElem.AppendChild(planeElem);
                    }
                    elem.AppendChild(planesElem);

                    if (sector.Unk0.Count > 0)
                    {
                        XmlElement unk0Elem = xmlDoc.CreateElement("Unk0");
                        foreach (ushort val in sector.Unk0)
                        {
                            XmlElement u16Elem = xmlDoc.CreateElement("UInt16");
                            u16Elem.InnerText = val.ToString();
                            unk0Elem.AppendChild(u16Elem);
                        }
                        elem.AppendChild(unk0Elem);
                    }

                    AddElementWithType(xmlDoc, elem, "Unk1", "UInt32", sector.Unk1);
                    AddElementWithType(xmlDoc, elem, "Unk2", "UInt32", sector.Unk2);
                    AddElementWithType(xmlDoc, elem, "Name", "String", sector.Name);
                    AddElementWithType(xmlDoc, elem, "Unk3", "Int16", sector.Unk3);
                    AddElementWithType(xmlDoc, elem, "Unk4", "UInt16", sector.Unk4);
                    AddElementWithType(xmlDoc, elem, "Unk5", "UInt16", sector.Unk5);
                    AddElementWithType(xmlDoc, elem, "bBasicSceneOnly", "Boolean", sector.bBasicSceneOnly);

                    sectorsElem.AppendChild(elem);
                }
                root.AppendChild(sectorsElem);

                XmlElement portalsElem = xmlDoc.CreateElement("Portals");
                foreach (var portal in loadedPortals)
                {
                    XmlElement portalElem = xmlDoc.CreateElement("PortalSphere");

                    AddElementWithType(xmlDoc, portalElem, "Name", "String", portal.Name);

                    XmlElement posElem = xmlDoc.CreateElement("Position");
                    posElem.SetAttribute("Type", "Vec3");
                    AddElementWithType(xmlDoc, posElem, "X", "Single", portal.Position.X);
                    AddElementWithType(xmlDoc, posElem, "Y", "Single", portal.Position.Y);
                    AddElementWithType(xmlDoc, posElem, "Z", "Single", portal.Position.Z);
                    portalElem.AppendChild(posElem);

                    AddElementWithType(xmlDoc, portalElem, "Unk0", "Single", portal.Unk0);
                    AddElementWithType(xmlDoc, portalElem, "OpenRatio", "Single", portal.OpenRatio);
                    AddElementWithType(xmlDoc, portalElem, "LinkA", "String", portal.LinkA);
                    AddElementWithType(xmlDoc, portalElem, "Unk2", "Byte", portal.Unk2);
                    AddElementWithType(xmlDoc, portalElem, "LinkB", "String", portal.LinkB);
                    AddElementWithType(xmlDoc, portalElem, "Unk3", "Byte", portal.Unk3);
                    AddElementWithType(xmlDoc, portalElem, "CostFactor", "Single", portal.CostFactor);
                    AddElementWithType(xmlDoc, portalElem, "EntityName", "String", portal.EntityName);
                    AddElementWithType(xmlDoc, portalElem, "Unk6", "Byte", portal.Unk6);
                    AddElementWithType(xmlDoc, portalElem, "bVolumeFactorEnabled", "Byte", portal.bVolumeFactorEnabled);
                    AddElementWithType(xmlDoc, portalElem, "VolumeFactor", "Single", portal.VolumeFactor);

                    portalsElem.AppendChild(portalElem);
                }
                root.AppendChild(portalsElem);

                xmlDoc.Save(_soundSectorsFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving sound sectors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void AddElementWithType(XmlDocument doc, XmlElement parent, string name, string type, object value)
        {
            XmlElement elem = doc.CreateElement(name);
            elem.SetAttribute("Type", type);
            elem.InnerText = Convert.ToString(value, CultureInfo.InvariantCulture);
            parent.AppendChild(elem);
        }

        private void SaveCollision()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (SceneData.Collisions != null)
                {
                    Collision collision = new Collision();
                    collision.Name = SceneData.Collisions.Name;
                    for (int i = 0; i != collisionRoot.Nodes.Count; i++)
                    {
                        TreeNode node = collisionRoot.Nodes[i];
                        Collision.CollisionModel collisionModel = (node.Tag as Collision.CollisionModel);
                        collision.Models.Add(collisionModel.Hash, collisionModel);

                        for (int x = 0; x != node.Nodes.Count; x++)
                        {
                            TreeNode child = node.Nodes[x];
                            Collision.Placement placement = (child.Tag as Collision.Placement);
                            collision.Placements.Add(placement);
                        }
                    }
                    SceneData.Collisions = collision;
                    SceneData.Collisions.WriteToFile();
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveATP()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (SceneData.ATLoader != null)
                {
                    SceneData.ATLoader.WriteToFile();
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveAIWorld()
        {
            if (MessageBox.Show("Save AIWorld?", "Toolkit", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (SceneData?.AIWorlds == null)
                {
                    return;
                }
                int savedCount = 0;
                foreach (NAVData navData in SceneData.AIWorlds)
                {
                    try
                    {
                        if (navData?.Data is AIWorld aiWorld)
                        {
                            SyncAIWorldCollections(aiWorld);
                            navData.WriteToFile();
                            savedCount++;
                        }
                    }
                    catch { }
                }
            }
            catch { }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void SyncAIWorldCollections(AIWorld aiWorld)
        {
            try
            {
                if (dSceneTree == null || dSceneTree.TreeView == null || dSceneTree.TreeView.Nodes == null)
                {
                    return;
                }
                TreeNode worldNode = null;
                foreach (TreeNode rootNode in dSceneTree.TreeView.Nodes)
                {
                    if (rootNode != null && object.ReferenceEquals(rootNode.Tag, aiWorld))
                    {
                        worldNode = rootNode;
                        break;
                    }
                }
                if (worldNode == null)
                {
                    return;
                }
                aiWorld.AIPoints.Clear();
                aiWorld.Types1.Clear();
                foreach (TreeNode childNode in worldNode.Nodes)
                {
                    if (childNode?.Tag is IType aiPoint)
                    {
                        aiWorld.AIPoints.Add(aiPoint);
                        if (aiPoint is AIWorld_Type1 type1Group)
                        {
                            aiWorld.Types1.Add(type1Group);
                            if (type1Group.AIPoints != null)
                            {
                                foreach (IType childPoint in type1Group.AIPoints)
                                {
                                    if (!aiWorld.AIPoints.Contains(childPoint))
                                    {
                                        aiWorld.AIPoints.Add(childPoint);
                                    }
                                }
                            }
                        }
                        else { }
                    }
                }
            }
            catch { }
        }

        private void SaveOBJData()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (SceneData.OBJData != null && ToolkitSettings.Experimental)
                {
                    for (int i = 0; i < SceneData.OBJData.Length; i++)
                    {
                        var obj = SceneData.OBJData[i];
                        obj.WriteToFile();
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveActor()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (SceneData.Actors != null && ToolkitSettings.Experimental)
                {
                    for (int i = 0; i < SceneData.Actors.Length; i++)
                    {
                        FixActorDefintions(SceneData.Actors[i]);
                        SceneData.Actors[i].WriteToFile();
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveCollisionItemDesc()
        {
            if (SceneData.ItemDescs == null || SceneData.ItemDescs.Length == 0)
            {
                MessageBox.Show("No ItemDescs to save.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Do you want to save all defined collisions for all ItemDescs?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;
            Cursor.Current = Cursors.WaitCursor;
            foreach (var item in SceneData.ItemDescs)
            {
                string path = Path.Combine(SceneData.ScenePath, item.FileName);
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    writer.Write(item.FrameRef);
                    writer.Write(item.UnkByte1);
                    writer.Write((byte)item.ColType);
                    writer.Write(item.IdHash);
                    writer.Write(item.ColMaterial);
                    item.Matrix.WriteToFile(writer);
                    writer.Write(item.UnkByte2);
                    if (item.Collisions != null && item.Collisions.Length > 0)
                    {
                        foreach (var col in item.Collisions)
                        {
                            switch (col)
                            {
                                case CollisionBox box:
                                    box.WriteToFile(writer);
                                    break;
                                case CollisionSphere sphere:
                                    sphere.WriteToFile(writer);
                                    break;
                                case CollisionCapsule capsule:
                                    capsule.WriteToFile(writer);
                                    break;
                                case CollisionConvex convex:
                                    convex.WriteToFile(writer);
                                    break;
                                default:
                                    Console.WriteLine($"Skipping unknown collision type in {item.FileName}");
                                    break;
                            }
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void SaveSELCollisionItemDesc()
        {
            if (dSceneTree.SelectedNode == null)
            {
                MessageBox.Show("Please select an ItemDesc to save.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dSceneTree.SelectedNode.Tag is ItemDescLoader selectedItem)
            {
                DialogResult result = MessageBox.Show($"Do you want to save changes to {selectedItem.FileName}?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string path = Path.Combine(SceneData.ScenePath, selectedItem.FileName);
                    using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
                    {
                        selectedItem.WriteToFile(writer);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("Selected node is not an ItemDesc.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveScene()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileLocation.FullName, FileMode.Create)))
                {
                    SceneData.FrameResource.WriteToFile(writer);
                }
                using (BinaryWriter writer = new BinaryWriter(File.Open(SceneData.FrameNameTable.FileName, FileMode.Create)))
                {
                    FrameNameTable nameTable = new FrameNameTable();
                    nameTable.FileName = SceneData.FrameNameTable.FileName;
                    nameTable.BuildDataFromResource(SceneData.FrameResource);
                    nameTable.WriteToFile(writer);
                    SceneData.FrameNameTable = nameTable;
                }
                SanitizeBuffers();
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();
                SceneData.UpdateResourceType();
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ParentIsEmpty(ParentInfo p)
        {
            if (p == null) return true;
            bool indexEmpty = p.Index == -1;
            bool refIdEmpty = p.RefID == -1;
            bool refIdZero = p.RefID == 0;
            return indexEmpty || refIdEmpty || refIdZero;
        }

        private IRenderer BuildRenderObjectFromFrame(FrameObjectBase fObject, Dictionary<int, IRenderer> assets)
        {
            fObject.ConstructRenderable();

            IRenderer Renderable = fObject.GetRenderItem();
            if (Renderable != null)
            {
                return Renderable;
            }
            return null;
        }

        private string Vec3(Vector3 v)
        {
            return $"({v.X:0.#####}, {v.Y:0.#####}, {v.Z:0.#####})";
        }

        private void BuildRenderObjects()
        {
            Dictionary<int, IRenderer> assets = new Dictionary<int, IRenderer>();
            if (ToolkitSettings.LoadFrameResource && SceneData.FrameResource != null && SceneData.FrameNameTable != null)
            {
                foreach (FrameObjectBase FrameObject in SceneData.FrameResource.FrameObjects.Values)
                {
                    IRenderer NewAsset = BuildRenderObjectFromFrame(FrameObject, assets);
                    if (NewAsset != null)
                    {
                        assets.Add(FrameObject.RefID, NewAsset);
                    }
                }
            }
            if (ToolkitSettings.LoadRoads && SceneData.roadMap != null && ToolkitSettings.Experimental)
            {
                TreeNode node = new TreeNode("Road Data");
                TreeNode node2 = new TreeNode("Junction Data");
                node.Tag = node2.Tag = "Folder";
                roadRoot = node;
                junctionRoot = node2;
                for (int i = 0; i < SceneData.roadMap.Roads.Count; i++)
                {
                    IRoadDefinition RoadDef = SceneData.roadMap.Roads[i];
                    if (RoadDef.Direction == RoadDirection.Backwards)
                    {
                        continue;
                    }
                    IRoadSpline RoadSpline = SceneData.roadMap.Splines[RoadDef.RoadSplineIndex];
                    RenderRoad road = new RenderRoad();
                    int generatedID = RefManager.GetNewRefID();
                    road.Init(RoadDef, RoadSpline);
                    assets.Add(generatedID, road);

                    TreeNode child = new TreeNode(i.ToString());
                    child.Text = "Road ID: " + i;
                    child.Name = generatedID.ToString();
                    child.Tag = road;
                    node.Nodes.Add(child);
                }
                for (int i = 0; i < SceneData.roadMap.Crossroads.Count; i++)
                {
                    int generatedID = RefManager.GetNewRefID();
                    RenderJunction junction = new RenderJunction();
                    junction.Init(SceneData.roadMap.Crossroads[i], Graphics);
                    assets.Add(generatedID, junction);
                    TreeNode child = new TreeNode(i.ToString());
                    child.Text = "Junction ID: " + i;
                    child.Name = generatedID.ToString();
                    child.Tag = junction;
                    junctionRoot.Nodes.Add(child);
                }
                dSceneTree.AddToTree(node);
                dSceneTree.AddToTree(node2);
            }
            if (ToolkitSettings.LoadHPD && SceneData.HPDData != null)
            {
                int generatedID = RefManager.GetNewRefID();
                TreeNode navNode = new TreeNode();
                navNode.Text = string.Format("HPD");
                navNode.Name = generatedID.ToString();

                for (int i = 0; i < SceneData.HPDData.HPDEntries.Length; i++)
                {
                    generatedID = RefManager.GetNewRefID();
                    TreeNode hpdNode = new TreeNode();
                    hpdNode.Text = string.Format("NODE: {0}", i);
                    hpdNode.Name = generatedID.ToString();
                    var item = SceneData.HPDData.HPDEntries[i];
                    RenderBoundingBox bbox = new RenderBoundingBox();
                    BoundingBox box = new BoundingBox(item.BBoxMin, item.BBoxMax);
                    bbox.Init(box);
                    assets.Add(generatedID, bbox);
                    hpdNode.Tag = box;
                    navNode.Nodes.Add(hpdNode);
                }
                dSceneTree.AddToTree(navNode);
            }
            if (ToolkitSettings.LoadOBJData && SceneData.OBJData != null && SceneData.OBJData.Length > 0)
            {
                OBJDataRoot = new TreeNode();
                OBJDataRoot.Tag = "Folder";
                OBJDataRoot.Name = OBJDataRoot.Text = "Navigation: OBJDATA";
                var data = new OBJData[SceneData.OBJData.Length];
                string[] fileNames = new string[SceneData.OBJData.Length];
                for (int i = 0; i < SceneData.OBJData.Length; i++)
                {
                    data[i] = (OBJData)SceneData.OBJData[i].Data;
                    fileNames[i] = SceneData.OBJData[i].FileName;
                }
                TreeNode Grids = Graphics.SetNavigationGrid(data);

                for (int i = 0; i < Grids.Nodes.Count && i < fileNames.Length; i++)
                {
                    Grids.Nodes[i].Text = fileNames[i];
                }

                OBJDataRoot.Nodes.Add(Grids);
                for (int i = 0; i < SceneData.OBJData.Length; i++)
                {
                    var obj = (SceneData.OBJData[i].Data as OBJData);
                    if (obj == null) continue;
                    RenderNav navigationPoints = new RenderNav(Graphics);
                    navigationPoints.Init(obj);
                    TreeNode navNode = new TreeNode();
                    navNode.Text = SceneData.OBJData[i].FileName;
                    navNode.Name = "NAV_OBJ_DATA";
                    navNode.Tag = navigationPoints;

                    TreeNode verticesFolder = new TreeNode("NavNodes");
                    verticesFolder.Tag = "Folder";
                    verticesFolder.Name = "NAV_VERTICES_FOLDER";
                    navNode.Nodes.Add(verticesFolder);

                    if (obj.vertices != null)
                    {
                        for (int x = 0; x < obj.vertices.Length; x++)
                        {
                            TreeNode childNode = new TreeNode();
                            childNode.Text = string.Format("NAVNode: {0}", obj.vertices[x].Unk7);
                            childNode.Name = "NAV_INDEXED_NODE";
                            childNode.Tag = obj.vertices[x];
                            verticesFolder.Nodes.Add(childNode);
                        }
                    }

                    if (obj.connections != null && obj.connections.Length > 0)
                    {
                        TreeNode connFolder = new TreeNode("Connections") { Tag = "Folder", Name = "NAV_CONNECTIONS_FOLDER" };

                        for (int connIdx = 0; connIdx < obj.connections.Length; connIdx++)
                        {
                            var conn = obj.connections[connIdx];

                            uint fromUnk7 = (conn.NodeID < obj.vertices?.Length) ? obj.vertices[(int)conn.NodeID].Unk7 : conn.NodeID;
                            uint toUnk7 = (conn.ConnectedNodeID < obj.vertices?.Length) ? obj.vertices[(int)conn.ConnectedNodeID].Unk7 : conn.ConnectedNodeID;

                            TreeNode connNode = new TreeNode(
                                $"Conn[{connIdx}]: Node {fromUnk7} -> {toUnk7} | Flags: 0x{conn.Flags:X4}"
                            );
                            connNode.Name = $"NAV_CONN_{connIdx}";
                            connNode.Tag = conn;
                            connFolder.Nodes.Add(connNode);
                        }
                        navNode.Nodes.Add(connFolder);
                    }
                    if (obj.runtimeMesh != null && obj.runtimeMesh.Cells != null)
                    {
                        TreeNode cellsFolder = new TreeNode("Cells");
                        cellsFolder.Tag = "Folder";
                        cellsFolder.Name = "NAV_CELLS_FOLDER";

                        for (int cellIndex = 0; cellIndex < obj.runtimeMesh.Cells.Length; cellIndex++)
                        {
                            KynogonRuntimeMesh.Cell cell = obj.runtimeMesh.Cells[cellIndex];
                            TreeNode cellNode = new TreeNode();
                            cellNode.Text = string.Format("Cell: {0}, Sets: {1}", cellIndex, cell.Sets?.Length ?? 0);
                            cellNode.Name = "NAV_CELL_NODE";
                            cellNode.Tag = cell;


                            cellsFolder.Nodes.Add(cellNode);
                        }
                        navNode.Nodes.Add(cellsFolder);
                    }
                    OBJDataRoot.Nodes.Add(navNode);
                }
                dSceneTree.AddToTree(OBJDataRoot);
            }
            if (ToolkitSettings.LoadAIWorld && SceneData.AIWorlds != null && SceneData.AIWorlds.Length > 0)
            {
                AIWorldRoot = new TreeNode();
                AIWorldRoot.Tag = "Folder";
                AIWorldRoot.Name = AIWorldRoot.Text = "Navigation: AIWORLD";
                var data = new AIWorld[SceneData.AIWorlds.Length];

                for (int i = 0; i < SceneData.AIWorlds.Length; i++)
                {
                    data[i] = (AIWorld)SceneData.AIWorlds[i].Data;
                    data[i].ConstructRenderable(Graphics);
                    TreeNode AIWorldNode = data[i].PopulateTreeNode();
                    AIWorldRoot.Nodes.Add(AIWorldNode);
                }
                dSceneTree.AddToTree(AIWorldRoot);
            }
            if (ToolkitSettings.LoadCollisions && SceneData.Collisions != null)
            {
                TreeNode node = new TreeNode("Collision Data");
                node.Tag = "Folder";
                collisionRoot = node;

                for (int i = 0; i != SceneData.Collisions.Models.Count; i++)
                {
                    Collision.CollisionModel data = SceneData.Collisions.Models.ElementAt(i).Value;
                    RenderStaticCollision collision = new RenderStaticCollision();
                    collision.ConvertCollisionToRender(data.Hash, data.Mesh);
                    RenderStorageSingleton.Instance.StaticCollisions.Add(SceneData.Collisions.Models.ElementAt(i).Key, collision);
                    TreeNode treeNode = new TreeNode(data.Hash.ToString());
                    treeNode.Text = data.Hash.ToString();
                    treeNode.Name = data.Hash.ToString();
                    treeNode.Tag = data;
                    dSceneTree.AddToTree(treeNode, collisionRoot);
                }
                for (int i = 0; i != SceneData.Collisions.Placements.Count; i++)
                {
                    Collision.Placement placement = SceneData.Collisions.Placements[i];
                    TreeNode[] nodes = collisionRoot.Nodes.Find(placement.Hash.ToString(), false);

                    if (nodes.Length > 0)
                    {
                        int refID = RefManager.GetNewRefID();
                        RenderInstance instance = new RenderInstance();
                        instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                        instance.SetTransform(placement.Transform);
                        TreeNode child = new TreeNode();
                        child.Text = nodes[0].Nodes.Count.ToString();
                        child.Name = refID.ToString();
                        child.Tag = placement;
                        assets.Add(refID, instance);
                        nodes[0].Nodes.Add(child);
                    }
                }
                dSceneTree.AddToTree(node);
                collisionRoot.Collapse(false);
            }
            if (ToolkitSettings.LoadItemDescs && SceneData.ItemDescs != null && SceneData.ItemDescs.Length > 0)
            {
                TreeNode itemDescRoot = new TreeNode("Item Descriptions");
                itemDescRoot.Tag = "Folder";

                for (int i = 0; i < SceneData.ItemDescs.Length; i++)
                {
                    var currentItem = SceneData.ItemDescs[i];
                    TreeNode itemNode = new TreeNode($"ItemDesc [{i}] | FrameRef: {currentItem.FrameRef}");
                    itemNode.Tag = currentItem;

                    TreeNode infoNode = new TreeNode("Info") { Tag = "Folder" };
                    infoNode.Nodes.Add($"Collision Type: {GetCollisionTypeName(currentItem.ColType)}");
                    infoNode.Nodes.Add($"Collision Type: {currentItem.ColType}");
                    infoNode.Nodes.Add($"Material: {currentItem.ColMaterial}");
                    infoNode.Nodes.Add($"ID Hash: {currentItem.IdHash}");
                    itemNode.Nodes.Add(infoNode);

                    if (currentItem.Collisions != null && currentItem.Collisions.Length > 0)
                    {
                        TreeNode collisionsNode = new TreeNode("Collisions") { Tag = "Folder" };

                        for (int colIndex = 0; colIndex < currentItem.Collisions.Length; colIndex++)
                        {
                            var col = currentItem.Collisions[colIndex];
                            TreeNode collisionNode = new TreeNode($"Collision [{colIndex}]") { Tag = col };
                            IRenderer render = null;
                            int refID = RefManager.GetNewRefID();

                            if (col is CollisionBox box) render = RenderableFactory.BuildBoundingBoxFromBox(box, currentItem.Matrix);
                            else if (col is CollisionSphere sphere) render = RenderableFactory.BuildBoundingSphere(sphere, currentItem.Matrix);
                            else if (col is CollisionCapsule capsule) render = RenderableFactory.BuildBoundingCapsule(capsule, currentItem.Matrix);
                            else if (col is CollisionConvex convex)
                            {
                                var rsc = new RenderStaticCollision();
                                rsc.SetTransform(currentItem.Matrix);
                                rsc.ConvertCollisionToRender(convex);
                                render = rsc;
                            }
                            if (render != null)
                            {
                                assets.Add(refID, render);
                                collisionNode.Name = refID.ToString();
                            }

                            collisionNode.Nodes.Add($"Renderable: {render != null}");
                            TreeNode detailsNode = new TreeNode("Collision Details");

                            if (col is CollisionBox b)
                            {
                                detailsNode.Nodes.Add($"Extents (Half Size): {Vec3(b.Extents)}");
                                detailsNode.Nodes.Add($"Size: {Vec3(b.Size)}");
                            }
                            else if (col is CollisionSphere s)
                            {
                                detailsNode.Nodes.Add($"Radius: {s.Radius:0.###}");
                                detailsNode.Nodes.Add($"Diameter: {s.Diameter:0.###}");
                            }
                            else if (col is CollisionCapsule c)
                            {
                                detailsNode.Nodes.Add($"Radius: {c.Radius:0.###}");
                                detailsNode.Nodes.Add($"Half Height: {c.HalfHeight:0.###}");
                                detailsNode.Nodes.Add($"Height: {c.Height:0.###}");
                                detailsNode.Nodes.Add($"Full Height: {c.FullHeight:0.###}");
                            }
                            else if (col is CollisionConvex cv)
                            {
                                detailsNode.Nodes.Add($"Vertices: {cv.Vertices.Count}");
                                detailsNode.Nodes.Add($"Hull Center: {Vec3(cv.HullCenter)}");
                                detailsNode.Nodes.Add($"BBox Min: {Vec3(cv.Min)}");
                                detailsNode.Nodes.Add($"BBox Max: {Vec3(cv.Max)}");
                                detailsNode.Nodes.Add($"BBox Size: {Vec3(cv.Size)}");
                            }
                            collisionNode.Nodes.Add(detailsNode);
                            collisionsNode.Nodes.Add(collisionNode);
                        }
                        itemNode.Nodes.Add(collisionsNode);
                    }
                    itemDescRoot.Nodes.Add(itemNode);
                }
                dSceneTree.AddToTree(itemDescRoot);
            }
            if (ToolkitSettings.LoadATP && SceneData.ATLoader != null)
            {
                animalTrafficRoot = new TreeNode("Animal Traffic Paths");
                animalTrafficRoot.Tag = "Folder";
                for (int i = 0; i < SceneData.ATLoader.Paths.Length; i++)
                {
                    var atpPath = SceneData.ATLoader.Paths[i];
                    string animalName = "Unknown";
                    if (atpPath.AnimalTypeIdx >= 0 && atpPath.AnimalTypeIdx < SceneData.ATLoader.AnimalTypes.Length)
                    {
                        animalName = SceneData.ATLoader.AnimalTypes[atpPath.AnimalTypeIdx].Name.ToString();
                    }

                    int refID = RefManager.GetNewRefID();
                    RenderATP atp = new RenderATP();
                    atp.Init(atpPath);

                    TreeNode child = new TreeNode();
                    child.Text = $"Path [{i}] || Animal: [{animalName}]";
                    child.Name = refID.ToString();
                    child.Tag = atp;
                    assets.Add(refID, atp);
                    animalTrafficRoot.Nodes.Add(child);
                }
                dSceneTree.AddToTree(animalTrafficRoot);
            }
            if (ToolkitSettings.LoadActors && SceneData.Actors.Length > 0 && ToolkitSettings.Experimental)
            {
                LoadActorFiles();
                if (ToolkitSettings.LoadActors)
                    foreach (var actor in SceneData.Actors)
                    {
                        foreach (var entry in actor.Items)
                        {
                            if (entry.ActorTypeName == "LightEntity" || entry.ActorTypeID == (int)ActorTypes.LightEntity)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.LightGoldenrodYellow);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                if (foundNodes.Length > 0)
                                {
                                    TreeNode boxNode = new TreeNode("Light Box");
                                    boxNode.Name = refIDSmallBox.ToString();
                                    boxNode.Tag = renderSmallBox;
                                    foundNodes[0].Nodes.Add(boxNode);
                                }
                                if (entry.Data != null && entry.Data.Data is ActorLight light)
                                {
                                    Vector3 min = light.BoundaryBoxMinimum;
                                    Vector3 max = light.BoundaryBoxMaximum;
                                    BoundingBox worldBBox = new BoundingBox(min, max);
                                    RenderBoundingBox renderBBox = new RenderBoundingBox();
                                    renderBBox.Init(worldBBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderBBox);
                                    RefIDToActorEntry[refID] = entry;

                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode bboxNode = new TreeNode("Light BoundingBox");
                                        bboxNode.Name = refID.ToString();
                                        bboxNode.Tag = renderBBox;
                                        foundNodes[0].Nodes.Add(bboxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "Tree" || entry.ActorTypeID == (int)ActorTypes.Tree)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.ForestGreen);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                if (foundNodes.Length > 0)
                                {
                                    TreeNode boxNode = new TreeNode("Tree Box");
                                    boxNode.Name = refIDSmallBox.ToString();
                                    boxNode.Tag = renderSmallBox;
                                    foundNodes[0].Nodes.Add(boxNode);
                                }
                            }
                            if (entry.ActorTypeName == "StaticEntity" || entry.ActorTypeID == (int)ActorTypes.StaticEntity)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.Gray);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                if (foundNodes.Length > 0)
                                {
                                    TreeNode boxNode = new TreeNode("Tree Box");
                                    boxNode.Name = refIDSmallBox.ToString();
                                    boxNode.Tag = renderSmallBox;
                                    foundNodes[0].Nodes.Add(boxNode);
                                }
                            }
                            if (entry.ActorTypeName == "C_TranslocatedCar" || entry.ActorTypeID == (int)ActorTypes.StaticEntity)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.Orange);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                if (foundNodes.Length > 0)
                                {
                                    TreeNode boxNode = new TreeNode("Tree Box");
                                    boxNode.Name = refIDSmallBox.ToString();
                                    boxNode.Tag = renderSmallBox;
                                    foundNodes[0].Nodes.Add(boxNode);
                                }
                            }
                            if (entry.ActorTypeName == "C_Sound" || entry.ActorTypeID == (int)ActorTypes.C_Sound)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.Lime);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                if (foundNodes.Length > 0)
                                {
                                    TreeNode boxNode = new TreeNode("C_Sound Box");
                                    boxNode.Name = refIDSmallBox.ToString();
                                    boxNode.Tag = renderSmallBox;
                                    foundNodes[0].Nodes.Add(boxNode);
                                }
                            }
                            if (entry.ActorTypeName == "C_StaticParticle" || entry.ActorTypeID == (int)ActorTypes.C_StaticWeapon)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.DodgerBlue);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                if (foundNodes.Length > 0)
                                {
                                    TreeNode boxNode = new TreeNode("C_Sound Box");
                                    boxNode.Name = refIDSmallBox.ToString();
                                    boxNode.Tag = renderSmallBox;
                                    foundNodes[0].Nodes.Add(boxNode);
                                }
                            }
                            if (entry.ActorTypeName == "C_TrafficCar" || entry.ActorTypeID == (int)ActorTypes.C_TrafficCar)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorTrafficCar traffic)
                                {
                                    Vector3 min = traffic.BoundingBoxMinimum;
                                    Vector3 max = traffic.BoundingBoxMaximum;
                                    BoundingBox TrafficBBox = new BoundingBox(min, max);
                                    RenderBoundingBox Traffic2BBox = new RenderBoundingBox();
                                    Traffic2BBox.Init(TrafficBBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, Traffic2BBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode bboxNode = new TreeNode("C_TrafficCar BoundingBox");
                                        bboxNode.Name = refID.ToString();
                                        bboxNode.Tag = Traffic2BBox;
                                        foundNodes[0].Nodes.Add(bboxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "C_TrafficTrain" || entry.ActorTypeID == (int)ActorTypes.C_TrafficTrain)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorTrafficTrain traffictrain)
                                {
                                    Vector3 min = traffictrain.BoundingBoxMinimum;
                                    Vector3 max = traffictrain.BoundingBoxMaximum;
                                    BoundingBox TrainTrafficBBox = new BoundingBox(min, max);
                                    RenderBoundingBox TrainTraffic2BBox = new RenderBoundingBox();
                                    TrainTraffic2BBox.Init(TrainTrafficBBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, TrainTraffic2BBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode bboxNode = new TreeNode("C_TrafficTrain BoundingBox");
                                        bboxNode.Name = refID.ToString();
                                        bboxNode.Tag = TrainTraffic2BBox;
                                        foundNodes[0].Nodes.Add(bboxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "C_TrafficHuman" || entry.ActorTypeID == (int)ActorTypes.C_TrafficHuman)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorTrafficHuman traffichuman)
                                {
                                    Vector3 min = traffichuman.BoundingBoxMinimum;
                                    Vector3 max = traffichuman.BoundingBoxMaximum;
                                    BoundingBox HumanTrafficBBox = new BoundingBox(min, max);
                                    RenderBoundingBox HumanTraffic2BBox = new RenderBoundingBox();
                                    HumanTraffic2BBox.Init(HumanTrafficBBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, HumanTraffic2BBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode bboxNode = new TreeNode("C_TrafficHuman BoundingBox");
                                        bboxNode.Name = refID.ToString();
                                        bboxNode.Tag = HumanTraffic2BBox;
                                        foundNodes[0].Nodes.Add(bboxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "C_Blocker" || entry.ActorTypeID == (int)ActorTypes.Blocker)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorBlocker blocker)
                                {
                                    Vector3 position = entry.Position;
                                    Vector3 bboxSize = blocker.BBox;
                                    Vector3 halfSize = bboxSize * 0.5f;
                                    BoundingBox blockerBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderBlockerBox = new RenderBoundingBox();
                                    renderBlockerBox.Init(blockerBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderBlockerBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Blocker BBox");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderBlockerBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "FireTarget" || entry.ActorTypeID == (int)ActorTypes.FireTarget)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorFireTarget fitetarget)
                                {
                                    Vector3 position = entry.Position;
                                    Vector3 bboxSize = fitetarget.BoxExtents;
                                    Vector3 halfSize = bboxSize * 0.5f;
                                    BoundingBox fireBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderFireBox = new RenderBoundingBox();
                                    renderFireBox.Init(fireBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderFireBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Fire Target Box Extents");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderFireBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "CleanEntity" || entry.ActorTypeID == (int)ActorTypes.CleanEntity)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorCleanEntity Cleantarget)
                                {
                                    Vector3 position = entry.Position;
                                    Vector3 bboxSize = Cleantarget.BBoxSize;
                                    Vector3 halfSize = bboxSize * 0.5f;
                                    BoundingBox cleaningBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderCleanBox = new RenderBoundingBox();
                                    renderCleanBox.Init(cleaningBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderCleanBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Clean Entity Box");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderCleanBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "DangerZone" || entry.ActorTypeID == (int)ActorTypes.DangerZone)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorDamageZone Damagetarget)
                                {
                                    Vector3 position = entry.Position;
                                    Vector3 bboxSize = Damagetarget.BBoxExtents;
                                    Vector3 halfSize = bboxSize * 0.5f;
                                    BoundingBox cleaningBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderDamageBox = new RenderBoundingBox();
                                    renderDamageBox.Init(cleaningBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderDamageBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Danger Zone Box");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderDamageBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "ActorPoint" || entry.ActorTypeID == (int)ActorTypes.ActionPoint)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorActionPoint ActionPoint)
                                {
                                    Vector3 position = entry.Position;
                                    Vector3 bboxSize = ActionPoint.BBox;
                                    Vector3 halfSize = bboxSize * 0.5f;
                                    BoundingBox ActionPBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderActionBox = new RenderBoundingBox();
                                    renderActionBox.Init(ActionPBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderActionBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Action Point BBox");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderActionBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "ActorDetector" || entry.ActorTypeID == (int)ActorTypes.C_ActorDetector)
                            {
                                Vector3 position = entry.Position;
                                Vector3 size = new Vector3(0.05f);
                                BoundingBox smallBox = new BoundingBox(position - size, position + size);
                                RenderBoundingBox renderSmallBox = new RenderBoundingBox();
                                renderSmallBox.SetColour(System.Drawing.Color.Orchid);
                                renderSmallBox.Init(smallBox);
                                int refIDSmallBox = RefManager.GetNewRefID();
                                assets.Add(refIDSmallBox, renderSmallBox);
                                RefIDToActorEntry[refIDSmallBox] = entry;

                                if (entry.Data != null && entry.Data.Data is ActorActorDetector detector)
                                {
                                    Vector3 halfSize = new Vector3(detector.SizeX * 0.5f, detector.SizeY * 0.5f, detector.SizeZ * 0.5f);
                                    BoundingBox detectorBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderDetectorBox = new RenderBoundingBox();
                                    renderDetectorBox.Init(detectorBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderDetectorBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Detector Box");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderDetectorBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("ActorDetector Box");
                                        boxNode.Name = refIDSmallBox.ToString();
                                        boxNode.Tag = renderSmallBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                            if (entry.ActorTypeName == "PhysicsScene" || entry.ActorTypeID == (int)ActorTypes.PhysicsScene)
                            {
                                if (entry.Data != null && entry.Data.Data is ActorPhysicsScene PhysScene)
                                {
                                    Vector3 position = entry.Position;
                                    Vector3 bboxSize = PhysScene.BBox;
                                    Vector3 halfSize = bboxSize * 0.5f;
                                    BoundingBox PhysSceneBox = new BoundingBox(position - halfSize, position + halfSize);
                                    RenderBoundingBox renderPhysSceneBox = new RenderBoundingBox();
                                    renderPhysSceneBox.Init(PhysSceneBox);
                                    int refID = RefManager.GetNewRefID();
                                    assets.Add(refID, renderPhysSceneBox);
                                    RefIDToActorEntry[refID] = entry;

                                    TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + entry.EntityName, true);
                                    if (foundNodes.Length > 0)
                                    {
                                        TreeNode boxNode = new TreeNode("Physics Scene Box");
                                        boxNode.Name = refID.ToString();
                                        boxNode.Tag = renderPhysSceneBox;
                                        foundNodes[0].Nodes.Add(boxNode);
                                    }
                                }
                            }
                        }
                    }
            }
            for (int i = 0; i < SceneData.FrameNameTable.FrameData.Length; i++)
            {
                FrameNameTable.Data data = SceneData.FrameNameTable.FrameData[i];
                if (data.FrameIndex != -1)
                {
                    FrameObjectBase frame = (SceneData.FrameResource.FrameObjects.ElementAt(data.FrameIndex).Value as FrameObjectBase);
                    if (frame != null)
                    {
                        frame.FrameNameTableFlags = data.Flags;
                        frame.IsOnFrameTable = true;
                    }
                }
            }
            foreach (var pair in SceneData.FrameResource.FrameObjects)
            {
                FrameObjectBase frame = (pair.Value as FrameObjectBase);
                if (assets.ContainsKey(frame.RefID))
                {
                    assets[frame.RefID].SetTransform(frame.WorldTransform);
                }
            }
            Graphics.InitObjectStack = assets;

            if (ToolkitSettings.LoadTranslokator && SceneData.Translokator != null && ToolkitSettings.Experimental)
            {
                dSceneTree.hasTranslokatorData = true;
                translokatorRoot = new TreeNode("Translokator Items");
                translokatorRoot.Tag = "Folder";
                TreeNode ogNode = new TreeNode("Objects Groups");
                ogNode.Tag = "Folder";
                for (int z = 0; z < SceneData.Translokator.ObjectGroups.Length; z++)
                {
                    ObjectGroup objectGroup = SceneData.Translokator.ObjectGroups[z];
                    TreeNode objectGroupNode = new TreeNode(String.Format("Object Group: [{0}]", objectGroup.ActorType));
                    objectGroupNode.Tag = objectGroup;
                    for (int y = 0; y < objectGroup.Objects.Length; y++)
                    {
                        Object obj = objectGroup.Objects[y];
                        TreeNode objNode = new TreeNode(obj.Name.ToString());
                        objNode.Tag = obj;
                        objectGroupNode.Nodes.Add(objNode);
                        FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(obj.Name.Hash);
                        bool hasMesh = false;
                        if (groupRef != null)
                        {
                            hasMesh = groupRef.HasMeshObject();
                        }
                        for (int x = 0; x < obj.Instances.Length; x++)
                        {
                            Instance instance = obj.Instances[x];
                            instance.RefID = RefManager.GetNewRefID();
                            if (groupRef != null && hasMesh)
                            {
                                for (int i = 0; i < groupRef.Children.Count; i++)//i dont think this for cycle is needed really if done right
                                {
                                    InstanceTranslokatorPart(assets, groupRef.Children[i], Matrix4x4.Identity, instance);
                                }
                            }
                            else
                            {
                                Graphics.InstanceGizmo.InstanceTranslokator(instance);
                            }

                            TreeNode instanceNode = new TreeNode(obj.Name + " " + x);
                            instanceNode.Tag = instance;
                            instanceNode.Name = instance.RefID.ToString();
                            objNode.Nodes.Add(instanceNode);
                        }
                    }
                    ogNode.Nodes.Add(objectGroupNode);
                }
                translokatorRoot.Nodes.Add(ogNode);

                TreeNode gridNode = new TreeNode("Grids");
                gridNode.Tag = "Folder";
                for (int i = 0; i < SceneData.Translokator.Grids.Length; i++)
                {
                    Grid grid = SceneData.Translokator.Grids[i];
                    TreeNode child = new TreeNode("Grid " + i);
                    child.Tag = grid;
                    child.Checked = false;
                    gridNode.Nodes.Add(child);
                }
                translokatorRoot.Nodes.Add(gridNode);
                dSceneTree.AddToTree(translokatorRoot);
                Graphics.BuildTranslokatorGrid(SceneData.Translokator);
            }
        }
        
        private string GetCollisionTypeName(ResourceTypes.ItemDesc.CollisionTypes type)
        {
            switch (type)
            {
                case ResourceTypes.ItemDesc.CollisionTypes.Box:
                    return "Box (Cube)";
                case ResourceTypes.ItemDesc.CollisionTypes.Sphere:
                    return "Sphere";
                case ResourceTypes.ItemDesc.CollisionTypes.Capsule:
                    return "Capsule";
                case ResourceTypes.ItemDesc.CollisionTypes.Convex:
                    return "Convex Mesh";
                default:
                    return "Unknown";
            }
        }

        private void btnMoveNAV_Click(object sender, EventArgs e)
        {
            if (dSceneTree.SelectedNode != null && dSceneTree.SelectedNode.Tag is RenderNav nav)
            {
                try
                {
                    string inputX = Interaction.InputBox("Enter the X offset", "Move NAV", "0");
                    string inputY = Interaction.InputBox("Enter the Y offset:", "Move NAV", "0");
                    string inputZ = Interaction.InputBox("Enter the Z offset:", "Move NAV", "0");
                    float dx = float.Parse(inputX);
                    float dy = float.Parse(inputY);
                    float dz = float.Parse(inputZ);
                    nav.MoveAllVertices(dx, dy, dz);
                }
                catch
                {
                    MessageBox.Show("Incorrect input!");
                }
            }
            else
            {
                MessageBox.Show("Select NAV to move");
            }
        }

        public void InstanceTranslokatorPart(Dictionary<int, IRenderer> assets, FrameObjectBase refframe, Matrix4x4 ParentTransform, Instance instance, bool updateInstanceBuffers = false)
        {
            var refTransform = ComputeWorldTransform(refframe.LocalTransform, ParentTransform);
            refTransform.M44 = 1.0f;
            if (refframe is FrameObjectSingleMesh mesh)
            {
                if (!assets.ContainsKey(refframe.RefID))
                {
                    goto SkipToChildren;
                }
                if (assets[refframe.RefID] is RenderModel model)
                {
                    Matrix4x4 newtransform = new Matrix4x4();
                    newtransform = MatrixUtils.SetMatrix(instance.Quaternion, new Vector3(instance.Scale, instance.Scale, instance.Scale), instance.Position);
                    newtransform = refTransform * newtransform;
                    if (!model.InstanceTransforms.ContainsKey(instance.RefID))
                    {
                        model.InstanceTransforms.Add(instance.RefID, Matrix4x4.Transpose(newtransform));
                        if (updateInstanceBuffers)
                        {
                            model.ReloadInstanceBuffer(Graphics.GetId3D11Device());
                        }
                    }
                }
            }
        SkipToChildren:;
            if (refframe.Children.Count > 0)
            {
                for (int i = 0; i < refframe.Children.Count; i++)
                {
                    InstanceTranslokatorPart(assets, refframe.Children[i], refTransform, instance, updateInstanceBuffers);
                }
            }
        }

        public List<RenderModel> UpdateTranslocatorPart(FrameObjectBase refframe, Matrix4x4 ParentTransform, Instance instance)
        {
            List<RenderModel> modelsToUpdate = new();
            if (Graphics == null)
            {
                return modelsToUpdate;
            }
            var refTransform = ComputeWorldTransform(refframe.LocalTransform, ParentTransform); ;
            refTransform.M44 = 1.0f;
            if (refframe is FrameObjectSingleMesh mesh)
            {
                if (!Graphics.Assets.ContainsKey(refframe.RefID))
                {
                    goto SkipToChildren;
                }
                if (Graphics.Assets[refframe.RefID] is RenderModel model)
                {
                    Matrix4x4 newtransform = new Matrix4x4();
                    newtransform = MatrixUtils.SetMatrix(instance.Quaternion, new Vector3(instance.Scale, instance.Scale, instance.Scale), instance.Position);
                    newtransform = refTransform * newtransform;
                    if (!model.InstanceTransforms.ContainsKey(instance.RefID))
                    {
                        model.InstanceTransforms.Add(instance.RefID, Matrix4x4.Transpose(newtransform));
                    }
                    else
                    {
                        model.InstanceTransforms[instance.RefID] = Matrix4x4.Transpose(newtransform);
                    }
                    modelsToUpdate.Add(model);
                }
            }
        SkipToChildren:;
            if (refframe.Children.Count > 0)
            {
                for (int i = 0; i < refframe.Children.Count; i++)
                {
                    modelsToUpdate.AddRange(UpdateTranslocatorPart(refframe.Children[i], refTransform, instance));
                }
            }
            return modelsToUpdate;
        }

        public Matrix4x4 ComputeWorldTransform(Matrix4x4 LocalTransform, Matrix4x4 ParentTransform)
        {
            //The world transform is calculated and then decomposed because some reason,
            //the renderer does not update on the first startup of the editor.
            Vector3 position, scale, newPos;
            Quaternion rotation, newRot;
            Matrix4x4.Decompose(LocalTransform, out scale, out rotation, out position);
            Vector3 parentPosition = Vector3.Zero;
            Vector3 parentScale = Vector3.One;
            Quaternion parentRotation = Quaternion.Identity;
            Matrix4x4.Decompose(ParentTransform, out parentScale, out parentRotation, out parentPosition);
            newRot = parentRotation * rotation;
            newPos = Vector3Utils.TransformCoordinate(position, ParentTransform);
            return MatrixUtils.SetMatrix(newRot, scale, newPos);
        }

        private void LoadActorFiles()
        {
            if (actorRoot == null)
            {
                actorRoot = new TreeNode("Actor Items");
                actorRoot.Tag = "Folder";
            }
            else
            {
                actorRoot.Nodes.Clear();
            }
            for (int z = 0; z < SceneData.Actors.Length; z++)
            {
                Actor actor = SceneData.Actors[z];
                TreeNode actorFile = new TreeNode("Actor File " + z);
                actorFile.Tag = "Folder";
                actorRoot.Nodes.Add(actorFile);
                for (int c = 0; c < actor.Items.Count; c++)
                {
                    var item = actor.Items[c];
                    TreeNode itemNode = new TreeNode(item.EntityName);
                    itemNode.Name = "actor_" + item.EntityName;
                    itemNode.Tag = item;
                    var typeString = "actorType_" + item.ActorTypeName;
                    var foundNodes = actorFile.Nodes.Find(typeString, false);
                    if (foundNodes.Length > 0)
                    {
                        foundNodes[0].Nodes.Add(itemNode);
                    }
                    else
                    {
                        TreeNode typeNode = new TreeNode(typeString);
                        typeNode.Name = typeString;
                        typeNode.Text = item.ActorTypeName;
                        typeNode.Nodes.Add(itemNode);
                        actorFile.Nodes.Add(typeNode);
                    }
                    FixActorDefintions(actor);
                }
            }
            dSceneTree.AddToTree(actorRoot);
        }

        private void TreeViewUpdateSelected()
        {
            var node = dSceneTree.SelectedNode;
            if (node.Tag == null)
            {
                return;
            }
            if (FrameResource.IsFrameType(node.Tag))
            {
                Graphics.SelectEntry((node.Tag as FrameEntry).RefID);
            }
            if (node.Tag is CollisionModel colModel)
            {
                dPropertyGrid.SetObject(colModel);
            }
            else if (node.Tag is SpatialCell)
            {
                SpatialGrid grid = (node.Parent.Tag as SpatialGrid);
                grid.SetSelectedCell(node.Index);
            }
            else if (node.Parent != null && node.Parent.Tag is RenderNav)
            {
                if (node.Tag is OBJData.VertexStruct vertex)
                {
                    RenderNav objNav = (node.Parent.Tag as RenderNav);
                    int vertexIndex = Array.IndexOf(objNav.GetData().vertices, vertex);
                    if (vertexIndex >= 0)
                    {
                        objNav.SelectNode(vertexIndex);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Vertex not found in data.vertices");
                    }
                }
            }
            else if (node.Tag is SoundSectorData sector)
            {
                Graphics.SelectEntry(sector.RefID);
                dPropertyGrid.SetObject(sector);
            }
            else if (node.Tag is PortalData portal)
            {
                dPropertyGrid.SetObject(portal);
            }
            else if (node.Tag is Instance instance)
            {
                Graphics.SelectInstance(instance.RefID);
            }
            else if (node.Tag is ActorEntry actorEntry)
            {
                foreach (TreeNode child in node.Nodes)
                {
                    if (int.TryParse(child.Name, out int refID))
                    {
                        Graphics.SelectEntry(refID);
                        break;
                    }
                }
            }
            else
            {
                int result = 0;
                if (int.TryParse(node.Name, out result))
                {
                    Graphics.SelectEntry(result);
                }
            }
            dPropertyGrid.SetObject(node.Tag);
        }

        // Sync gizmo transform changes to the selected object
        private void SyncGizmoToSelectedObject()
        {
            if (dSceneTree.SelectedNode == null || dSceneTree.SelectedNode.Tag == null)
                return;

            var node = dSceneTree.SelectedNode;
            var tag = node.Tag;

            // Get the updated transform from Graphics
            if (!int.TryParse(node.Name, out int refID))
                return;

            IRenderer asset = Graphics.GetAsset(refID);
            if (asset == null)
                return;

            Matrix4x4 newTransform = asset.Transform;

            if (FrameResource.IsFrameType(tag))
            {
                FrameObjectBase fObject = (tag as FrameObjectBase);
                fObject.LocalTransform = newTransform;
                dPropertyGrid.SetObject(fObject);
            }
            else if (tag is Collision.Placement placement)
            {
                // Extract position and rotation from transform matrix
                placement.Position = newTransform.Translation;

                // Extract rotation as euler angles (in radians)
                if (Matrix4x4.Decompose(newTransform, out Vector3 scale, out Quaternion rotation, out Vector3 translation))
                {
                    // Convert quaternion to euler angles (radians)
                    placement.Rotation = QuaternionToEuler(rotation);
                }

                dPropertyGrid.SetObject(placement);
            }
        }

        // Convert quaternion to euler angles (radians)
        private Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 euler = new Vector3();

            // Roll (X-axis rotation)
            float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            euler.X = MathF.Atan2(sinr_cosp, cosr_cosp);

            // Pitch (Y-axis rotation)
            float sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (MathF.Abs(sinp) >= 1)
                euler.Y = MathF.CopySign(MathF.PI / 2, sinp); // Use 90 degrees if out of range
            else
                euler.Y = MathF.Asin(sinp);

            // Yaw (Z-axis rotation)
            float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            euler.Z = MathF.Atan2(siny_cosp, cosy_cosp);

            return euler;
        }

        private void btnAddType4_Click(object sender, EventArgs e)
        {
            if (dSceneTree == null) return;

            TreeNode selectedNode = dSceneTree.SelectedNode;

            if (selectedNode?.Tag is AIWorld_Type1 group)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(group.World, 4);
                group.AddPoint(newPoint);
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            if (selectedNode?.Tag is AIWorld world)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(world, 4);
                world.AIPoints.Add(newPoint);
                world.RequestPrimitiveBatchUpdate();
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            MessageBox.Show("Select AIWorld or a Type1 group.");
        }

        private void btnAddType11_Click(object sender, EventArgs e)
        {
            if (dSceneTree == null) return;

            TreeNode selectedNode = dSceneTree.SelectedNode;

            if (selectedNode?.Tag is AIWorld_Type1 group)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(group.World, 11);
                group.AddPoint(newPoint);
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            if (selectedNode?.Tag is AIWorld world)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(world, 11);
                world.AIPoints.Add(newPoint);
                world.RequestPrimitiveBatchUpdate();
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            MessageBox.Show("Select AIWorld or a Type1 group.");
        }

        private void btnAddType8_Click(object sender, EventArgs e)
        {
            if (dSceneTree == null) return;

            TreeNode selectedNode = dSceneTree.SelectedNode;

            if (selectedNode?.Tag is AIWorld_Type1 group)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(group.World, 8);
                group.AddPoint(newPoint);
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            if (selectedNode?.Tag is AIWorld world)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(world, 8);
                world.AIPoints.Add(newPoint);
                world.RequestPrimitiveBatchUpdate();
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            MessageBox.Show("Select AIWorld or a Type1 group.");
        }

        private void btnAddType9_Click(object sender, EventArgs e)
        {
            if (dSceneTree == null) return;

            TreeNode selectedNode = dSceneTree.SelectedNode;

            if (selectedNode?.Tag is AIWorld_Type1 group)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(group.World, 9);
                group.AddPoint(newPoint);
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            if (selectedNode?.Tag is AIWorld world)
            {
                IType newPoint = AIWorld_Factory.ConstructByTypeID(world, 9);
                world.AIPoints.Add(newPoint);
                world.RequestPrimitiveBatchUpdate();
                TreeNode newNode = newPoint.PopulateTreeNode();
                selectedNode.Nodes.Add(newNode);
                selectedNode.Expand();
                dSceneTree.SelectedNode = newNode;
                return;
            }
            MessageBox.Show("Select AIWorld or a Type1 group.");
        }

        private void btnAddType7_Click(object sender, EventArgs e)
        {
            if (dSceneTree == null) return;

            TreeNode selectedNode = dSceneTree.SelectedNode;
            AIWorld targetWorld = null;
            IType newPoint = null;

            if (selectedNode?.Tag is AIWorld_Type1 group)
            {
                targetWorld = group.World;
                newPoint = AIWorld_Factory.ConstructByTypeID(group.World, 7);
                group.AddPoint(newPoint);
            }
            else if (selectedNode?.Tag is AIWorld world)
            {
                targetWorld = world;
                newPoint = AIWorld_Factory.ConstructByTypeID(world, 7);
                world.AIPoints.Add(newPoint);
            }
            else
            {
                MessageBox.Show("Select AIWorld or a Type1 group.");
                return;
            }

            if (newPoint is AIWorld_Type7 type7 && targetWorld != null)
            {
                uint maxId = 0;
                foreach (var point in targetWorld.AIPoints)
                {
                    if (point is AIWorld_Type7 t7 && t7.Unk3 > maxId)
                        maxId = t7.Unk3;
                    else if (point is AIWorld_Type1 t1)
                    {
                        foreach (var child in t1.AIPoints)
                            if (child is AIWorld_Type7 ct7 && ct7.Unk3 > maxId)
                                maxId = ct7.Unk3;
                    }
                }
                type7.Unk3 = maxId + 1;
            }

            TreeNode newNode = newPoint.PopulateTreeNode();
            selectedNode.Nodes.Add(newNode);
            selectedNode.Expand();
            dSceneTree.SelectedNode = newNode;

            targetWorld?.RequestPrimitiveBatchUpdate();
        }

        private void Button_AddType1Group_Click(object sender, EventArgs e)
        {
            if (dSceneTree != null)
            {
                dSceneTree.AddType1GroupToAIWorld();
            }
        }

        private void FixActorDefintions(Actor actor)
        {
            List<int> frameIndexes = new List<int>();
            List<FrameObjectFrame> frames = new List<FrameObjectFrame>();
            for (int x = 0; x < SceneData.FrameResource.FrameObjects.Count; x++)
            {
                FrameObjectFrame frame = (SceneData.FrameResource.FrameObjects.ElementAt(x).Value as FrameObjectFrame);
                if (frame != null)
                {
                    frames.Add(frame);
                    frameIndexes.Add(x);
                }
            }
            for (int i = 0; i != actor.Definitions.Count; i++)
            {
                FrameObjectFrame frame = null;
                ActorDefinition definition = actor.Definitions[i];
                for (int c = 0; c != actor.Items.Count; c++)
                {
                    ActorEntry item = actor.Items[c];
                    if (definition.FrameNameHash == item.FrameNameHash)
                    {
                        for (int x = 0; x < frames.Count; x++)
                        {
                            FrameObjectFrame nFrame = frames[x];
                            if (nFrame.Name.Hash == item.FrameNameHash)
                            {
                                if (!nFrame.ActorHash.String.Equals(item.DefinitionName))
                                {
                                    Console.WriteLine("ActorHash and Definition Do NotMatch");
                                }
                                definition.FrameIndex = (uint)frameIndexes[x];
                                frame = nFrame;
                                frame.Item = actor.Items[c];
                                frame.LocalTransform = MatrixUtils.SetMatrix(actor.Items[c].Rotation, actor.Items[c].Scale, actor.Items[c].Position);
                            }
                        }
                    }
                }
            }
            frames.Clear();
            frameIndexes.Clear();
        }

        private void ApplyEntryChanges(object sender, EventArgs e)
        {
            if (dPropertyGrid.IsEntryReady)
            {
                TreeNode selected = dSceneTree.SelectedNode;
                if (selected.Tag is FrameObjectBase)
                {
                    FrameObjectBase fObject = (selected.Tag as FrameObjectBase);
                    selected.Text = fObject.ToString();
                    dPropertyGrid.UpdateObject();
                    ApplyChangesToRenderable(fObject);
                }
                else if (selected.Tag is OBJData.VertexStruct vertex)
                {
                    dPropertyGrid.UpdateObject();

                    TreeNode parentNode = selected.Parent;
                    while (parentNode != null && !(parentNode.Tag is RenderNav))
                        parentNode = parentNode.Parent;

                    if (parentNode?.Tag is RenderNav nav)
                    {
                        int vertexIndex = selected.Index;
                        nav.UpdateVertexPosition(vertexIndex, vertex.Position);
                    }
                }
                else if (selected.Tag is FrameHeaderScene)
                {
                    FrameHeaderScene scene = (selected.Tag as FrameHeaderScene);
                    selected.Text = scene.ToString();
                }
                else if (selected.Tag is Collision.Placement)
                {
                    dPropertyGrid.UpdateObject();
                    Collision.Placement placement = (selected.Tag as Collision.Placement);

                    // Update rendered counterpart
                    IRenderer Asset = Graphics.GetAsset(int.Parse(selected.Name));
                    if (Asset != null)
                    {
                        RenderInstance Instance = (Asset as RenderInstance);
                        Instance.SetTransform(placement.Transform);
                    }

                    // Send an event to update our selected item. (if this is indeed our selected)
                    UpdateSelectedEventArgs Arguments = new UpdateSelectedEventArgs();
                    Arguments.RefID = int.Parse(selected.Name);
                    Graphics.OnSelectedObjectUpdated(this, Arguments);
                }
                else if (selected.Tag is ActorEntry actorEntry)
                {
                    selected.Text = actorEntry.ToString();
                    dPropertyGrid.UpdateObject();
                    SyncActorEntryWithFrame(actorEntry);
                    UpdateActorVisualization(actorEntry, selected);
                }
                else if (selected.Tag is AIWorld_Type7 type7)
                {
                    dPropertyGrid.UpdateObject();
                    AIWorld world = FindParentAIWorld(selected);
                    world?.RequestPrimitiveBatchUpdate();
                    Graphics.SelectEntry(type7.RefID);
                }
                else if (selected.Tag is Instance)
                {
                    Instance instance = (selected.Tag as Instance);
                    dPropertyGrid.UpdateObject();
                    //get refframe and set instance index transform
                    if (dSceneTree.SelectedNode.Parent.Tag is Object objGroup)
                    {
                        FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(objGroup.Name.Hash);
                        if (groupRef != null)
                        {
                            for (int i = 0; i < groupRef.Children.Count; i++)
                            {
                                var modelsToUpdate = UpdateTranslocatorPart(groupRef.Children[i], Matrix4x4.Identity, instance);
                                Graphics.UpdateInstanceBuffers(modelsToUpdate);
                            }
                        }
                        else
                        {
                            Graphics.InstanceGizmo.UpdateInstanceBuffer(instance, Graphics.GetId3D11Device());
                        }
                    }
                }
                else if (selected.Tag is ItemDescLoader itemDesc)
                {
                    if (int.TryParse(selected.Name, out int refID) && Graphics.Assets.TryGetValue(refID, out IRenderer asset))
                    {
                        asset.SetTransform(itemDesc.Matrix);
                        Graphics.SelectEntry(refID);
                    }
                }
            }
        }
        private void AddNavVertexButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = dSceneTree.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show("No node selected.");
                return;
            }

            TreeNode navNode = selectedNode;
            while (navNode != null && !(navNode.Tag is RenderNav))
                navNode = navNode.Parent;

            if (navNode?.Tag is not RenderNav nav)
            {
                MessageBox.Show("Cannot find parent navigation data (RenderNav). Select a node inside a NAV object.");
                return;
            }

            OBJData data = nav.GetData();
            if (data == null || data.vertices == null) return;

            OBJData.VertexStruct newVertex = new OBJData.VertexStruct();

            uint maxUnk7 = 0;
            foreach (var v in data.vertices)
                if (v.Unk7 > maxUnk7) maxUnk7 = v.Unk7;
            newVertex.Unk7 = maxUnk7 + 1;

            if (data.vertices.Length > 0)
            {
                var first = data.vertices[0];
                newVertex.Unk0 = first.Unk0;
                newVertex.Unk1 = first.Unk1;
                newVertex.Unk6 = first.Unk6;
            }
            else
            {
                newVertex.Unk0 = 0;
                newVertex.Unk1 = 0;
                newVertex.Unk6 = 0;
            }

            if (selectedNode.Tag is OBJData.VertexStruct selectedVertex)
            {
                newVertex.Position = selectedVertex.Position + new Vector3(1, 1, 0);
            }
            else
            {
                Vector3 camPos = Graphics.Camera.Position;
                newVertex.Position = camPos + new Vector3(0, 0, 2);
            }

            newVertex.Unk2 = -1;
            newVertex.Unk3 = -1;
            newVertex.Unk4 = -1;
            newVertex.Unk5 = -1;

            Array.Resize(ref data.vertices, data.vertices.Length + 1);
            data.vertices[data.vertices.Length - 1] = newVertex;
            data.vertSize = data.vertices.Length;

            RenderBoundingBox newBox = new RenderBoundingBox();
            newBox.Init(new BoundingBox(new Vector3(-0.1f), new Vector3(0.1f)));
            newBox.SetColour(System.Drawing.Color.Green);
            newBox.SetTransform(Matrix4x4.CreateTranslation(newVertex.Position));
            nav.AddVertex(newBox, newVertex);

            TreeNode vertexNode = new TreeNode($"NAVNode: {newVertex.Unk7}");
            vertexNode.Tag = newVertex;
            vertexNode.Name = RefManager.GetNewRefID().ToString();
            selectedNode.Nodes.Add(vertexNode);
            selectedNode.Expand();

            data.GenerateConnections();
            nav.RebuildAllConnections();
            dSceneTree.SelectedNode = vertexNode;
            TreeViewUpdateSelected();
            dPropertyGrid.SetObject(newVertex);
        }

        private void SyncActorEntryWithFrame(ActorEntry actorEntry)
        {
            FrameObjectFrame linkedFrame = FindFrameByActorEntry(actorEntry);

            if (linkedFrame != null)
            {
                linkedFrame.LocalTransform = MatrixUtils.SetMatrix(
                    actorEntry.Rotation,
                    actorEntry.Scale,
                    actorEntry.Position
                );
                ApplyChangesToRenderable(linkedFrame);
            }
        }

        private FrameObjectFrame FindFrameByActorEntry(ActorEntry actorEntry)
        {
            if (actorEntry == null || actorEntry.FrameNameHash == 0)
                return null;
            foreach (var kvp in SceneData.FrameResource.FrameObjects)
            {
                if (kvp.Value is FrameObjectFrame frame &&
                    frame.Name.Hash == actorEntry.FrameNameHash)
                {
                    return frame;
                }
            }
            return null;
        }

        private void UpdateActorVisualization(ActorEntry actorEntry, TreeNode actorNode)
        {
            foreach (TreeNode child in actorNode.Nodes)
            {
                if (int.TryParse(child.Name, out int refID) &&
                    Graphics.Assets.TryGetValue(refID, out IRenderer asset))
                {
                    if (asset is RenderBoundingBox renderBox)
                    {
                        Matrix4x4 transform = Matrix4x4.CreateTranslation(actorEntry.Position);
                        renderBox.SetTransform(transform);
                    }
                }
            }
        }

        private void ApplyChangesToRenderable(FrameObjectBase obj)
        {
            if (obj is FrameObjectFrame frame && frame.Item is ActorEntry linkedActor)
            {
                Vector3 position, scale;
                Quaternion rotation;
                Matrix4x4.Decompose(frame.LocalTransform, out scale, out rotation, out position);
                linkedActor.Position = position;
                linkedActor.Rotation = rotation;
                linkedActor.Scale = scale;

                if (dSceneTree.SelectedNode?.Tag == linkedActor)
                {
                    dPropertyGrid.Refresh();
                }
            }
            if (obj is FrameObjectArea)
            {
                FrameObjectArea area = (obj as FrameObjectArea);
                area.FillPlanesArray();
                RenderBoundingBox bbox = (Graphics.GetAsset(obj.RefID) as RenderBoundingBox);
                bbox.SetTransform(area.WorldTransform);
                bbox.Update(area.Bounds);
            }
            else if (obj is FrameObjectDummy)
            {
                FrameObjectDummy dummy = (obj as FrameObjectDummy);
                RenderBoundingBox bbox = (Graphics.GetAsset(obj.RefID) as RenderBoundingBox);
                bbox.SetTransform(dummy.WorldTransform);
                bbox.Update(dummy.Bounds);
            }
            else if (obj is FrameObjectSector)
            {
                FrameObjectSector sector = (obj as FrameObjectSector);
                sector.FillPlanesArray();
                RenderBoundingBox bbox = (Graphics.GetAsset(obj.RefID) as RenderBoundingBox);
                bbox.SetTransform(sector.WorldTransform);
                bbox.Update(sector.Bounds);
            }
            else if (obj is FrameObjectSingleMesh)
            {
                FrameObjectSingleMesh mesh = (obj as FrameObjectSingleMesh);
                RenderModel model = (Graphics.GetAsset(obj.RefID) as RenderModel);
                model.SetTransform(mesh.WorldTransform);
                model.UpdateMaterials(mesh.Material);
            }
            foreach (var child in obj.Children)
            {
                ApplyChangesToRenderable(child);
            }
            // Send an event to update our selected item. (if this is indeed our selected)
            UpdateSelectedEventArgs Arguments = new UpdateSelectedEventArgs();
            Arguments.RefID = obj.RefID;
            Graphics.OnSelectedObjectUpdated(this, Arguments);
        }

        private void CreateMeshBuffers(ModelWrapper model)
        {
            // TODO: I want to move this into FrameObjectSingleMesh.
            FrameGeometry MeshGeometry = model.FrameMesh.Geometry;
            for (int i = 0; i < MeshGeometry.NumLods; i++)
            {
                bool bAdded = SceneData.VertexBufferPool.TryAddBuffer(model.VertexBuffers[i]);
                bAdded = SceneData.IndexBufferPool.TryAddBuffer(model.IndexBuffers[i]);
            }
        }

        private void CreateNewEntry(FrameResourceObjectType SelectedType, string name, bool bAddToNameTable)
        {
            FrameObjectBase frame = FrameFactory.ConstructFrameByObjectID(SceneData.FrameResource, SelectedType);

            // Frame was not valid, there is no need to carry on.
            if (frame == null)
            {
                return;
            }
            ToolkitAssert.Ensure(frame != null, "Frame was null!");
            frame.Name.Set(name);
            frame.IsOnFrameTable = bAddToNameTable;
            TreeNode node = new TreeNode(frame.Name.String);
            node.Tag = frame;
            node.Name = frame.RefID.ToString();

            if (frame is FrameObjectSingleMesh)
            {
                // TODO: We need to find an alternative method to creating single meshes
                // The Bundle system consists of multiple objects, in which one may not even be a single mesh.
                // Therefore it doesn't make sense to use this method - all users should use bundles.
                // However there may be some benefit in keeping this, maybe as a way to re-use loaded Single Meshes?
            }
            // If everything was succesful, then we would have reached this point.
            dSceneTree.AddToTree(node, frameResourceRoot);
            IRenderer renderer = BuildRenderObjectFromFrame(frame, null);
            if (renderer != null)
            {
                Graphics.InitObjectStack.Add(frame.RefID, renderer);
            }
        }

        private void Pick(int sx, int sy)
        {
            PickOutParams outParams = Graphics.Pick(sx, sy, RenderPanel.Size.Width, RenderPanel.Size.Height);
            if (outParams.LowestInstanceID != -1)
            {
                var nodes = dSceneTree.Find(outParams.LowestInstanceID.ToString(), true);
                if (nodes.Length > 0)
                {
                    dSceneTree.SelectedNode = nodes[0];
                    TreeViewUpdateSelected();
                }
                return;
            }
            if (outParams.LowestRefID != -1)
            {
                int refID = outParams.LowestRefID;
                if (RefIDToActorEntry.TryGetValue(refID, out ActorEntry actorEntry))
                {
                    TreeNode[] actorNodes = dSceneTree.TreeView.Nodes.Find("actor_" + actorEntry.EntityName, true);
                    if (actorNodes.Length > 0)
                    {
                        dSceneTree.SelectedNode = actorNodes[0];
                        TreeViewUpdateSelected();
                        return;
                    }
                }
                var standardNodes = dSceneTree.Find(refID.ToString(), true);
                if (standardNodes.Length == 0) return;

                var node = standardNodes[0];
                if (node.Tag is FrameObjectDummy) return;

                dSceneTree.SelectedNode = node;
                TreeViewUpdateSelected();
            }
        }

        public void Shutdown()
        {
            Graphics?.Shutdown();
            Graphics = null;
            Input = null;
            RenderStorageSingleton.Instance.Shutdown();
        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            Graphics.Camera.Position = dSceneTree.JumpToHelper();
            UpdatePositionElement(Graphics.Camera.Position);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            var frnode = dImportSceneTree.SelectedNode;
            if (frnode == null)
            {
                MessageBox.Show("Please select a node to import.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isFolder = frnode.Tag is FrameHeaderScene || (frnode.Tag is string s && s == "Folder");

            if (isFolder || frnode.Tag is FrameObjectBase)
            {
                TreeNode importedRoot = null;

                if (isFolder)
                {
                    importedRoot = ImportFolderRecursive(frnode, ImportedScene.FrameResource);
                }
                else
                {
                    FrameObjectBase frame = frnode.Tag as FrameObjectBase;
                    importedRoot = SceneData.FrameResource.ReadFramesFromImport(
                        frame.Name.String,
                        ImportedScene.FrameResource.SaveFramesStream(frame)
                    );

                    if (importedRoot != null)
                    {
                        SceneData.ImportItemDescForNode(frnode, ImportedScene);
                    }
                }

                if (importedRoot != null)
                {
                    if (dImportSceneTree.importTextures.Checked)
                    {
                        HashSet<string> allTextures = new HashSet<string>();
                        CollectAllTexturesFromNode(frnode, ImportedScene.FrameResource, allTextures);

                        if (allTextures.Count > 0)
                        {
                            SceneData.ImportTextures(new List<string>(allTextures), ImportedScene.ScenePath);
                        }
                    }

                    dSceneTree.AddToTree(importedRoot, frameResourceRoot);
                    ConvertNodeToFrame(importedRoot);
                }
            }
            else
            {
                MessageBox.Show("Selected item cannot be imported.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CollectAllTexturesFromNode(TreeNode node, FrameResource sourceFrameResource, HashSet<string> textureNames)
        {
            if (node.Tag is FrameObjectBase frameObj && sourceFrameResource.CheckForMeshObjects(node))
            {
                Dictionary<uint, string> tempDict = new Dictionary<uint, string>();
                sourceFrameResource.CollectAllTextureNames(node, tempDict);
                foreach (var name in tempDict.Values)
                {
                    if (!string.IsNullOrEmpty(name))
                        textureNames.Add(name);
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                CollectAllTexturesFromNode(child, sourceFrameResource, textureNames);
            }
        }

        private TreeNode ImportFolderRecursive(TreeNode sourceFolderNode, FrameResource sourceFrameResource)
        {
            if (sourceFolderNode.Tag is not FrameHeaderScene sceneHeader)
            {
                var newScene = SceneData.FrameResource.AddSceneFolder(sourceFolderNode.Text);
                TreeNode newFolderNode = new TreeNode(newScene.ToString())
                {
                    Tag = newScene,
                    Name = newScene.RefID.ToString()
                };

                foreach (TreeNode child in sourceFolderNode.Nodes)
                {
                    if (child.Tag is FrameHeaderScene || (child.Tag is string s && s == "Folder"))
                    {
                        TreeNode importedChild = ImportFolderRecursive(child, sourceFrameResource);
                        if (importedChild != null)
                            newFolderNode.Nodes.Add(importedChild);
                    }
                    else if (child.Tag is FrameObjectBase frameObj)
                    {
                        TreeNode importedObj = SceneData.FrameResource.ReadFramesFromImport(
                            frameObj.Name.String,
                            sourceFrameResource.SaveFramesStream(frameObj)
                        );
                        if (importedObj != null)
                        {
                            SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2,
                                importedObj.Tag as FrameObjectBase, newScene);
                            newFolderNode.Nodes.Add(importedObj);
                        }
                    }
                }

                return newFolderNode;
            }
            else
            {
                return ImportFolderRecursiveAsFolder(sourceFolderNode, sourceFrameResource);
            }
        }

        private TreeNode ImportFolderRecursiveAsFolder(TreeNode sourceNode, FrameResource sourceFrameResource)
        {
            var newScene = SceneData.FrameResource.AddSceneFolder(sourceNode.Text);
            TreeNode newNode = new TreeNode(newScene.ToString())
            {
                Tag = newScene,
                Name = newScene.RefID.ToString()
            };

            foreach (TreeNode child in sourceNode.Nodes)
            {
                if (child.Tag is FrameHeaderScene || (child.Tag is string s && s == "Folder"))
                {
                    TreeNode imported = ImportFolderRecursive(child, sourceFrameResource);
                    if (imported != null) newNode.Nodes.Add(imported);
                }
                else if (child.Tag is FrameObjectBase frame)
                {
                    TreeNode importedObj = SceneData.FrameResource.ReadFramesFromImport(
                        frame.Name.String,
                        sourceFrameResource.SaveFramesStream(frame)
                    );
                    if (importedObj != null)
                    {
                        SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2,
                            importedObj.Tag as FrameObjectBase, newScene);
                        newNode.Nodes.Add(importedObj);
                    }
                }
            }

            return newNode;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Button_ImportFrame.Enabled = true;
        }

        private void UpdateObjectParentsRecurse(TreeNode parent, FrameObjectBase entry)
        {
            foreach (var child in entry.Children)
            {
                TreeNode childNode = new TreeNode();
                childNode.Tag = child;
                childNode.Name = child.RefID.ToString();
                childNode.Text = child.ToString();
                parent.Nodes.Add(childNode);
                UpdateObjectParentsRecurse(childNode, child);
            }
        }

        private void UpdateObjectParents(ParentInfo.ParentType ParentType, int refID, FrameEntry entry = null)
        {
            FrameObjectBase obj = (dSceneTree.SelectedNode.Tag as FrameObjectBase);
            TreeNode[] newChildChildren = dSceneTree.Find(obj.RefID.ToString(), true);
            //checking if we are not trying to make children our new parent
            foreach (var child in newChildChildren)
            {
                if (child.Tag is FrameObjectBase childFrame)
                {
                    if (childFrame.IsFrameOwnChildren(refID))
                    {
                        return;
                    }
                }
            }
            //make sure refID is not root.
            if (refID != 0)
            {
                //make sure entry is not null.
                if (entry == null)
                {
                    TreeNode[] objs = dSceneTree.Find(refID.ToString(), true);
                    if (objs.Length > 0)
                    {
                        entry = (objs[0].Tag as FrameEntry);
                    }
                }
                SceneData.FrameResource.SetParentOfObject(ParentType, obj, entry);
            }
            else
            {
                SceneData.FrameResource.SetParentOfObject(ParentType, obj, null);
            }
            dSceneTree.RemoveNode(dSceneTree.SelectedNode);
            TreeNode newNode = new TreeNode();
            newNode.Tag = obj;
            newNode.Name = obj.RefID.ToString();
            newNode.Text = obj.ToString();
            UpdateObjectParentsRecurse(newNode, obj);

            TreeNode[] nodes = null;
            if (obj.ParentIndex1.Index != -1)
            {
                nodes = dSceneTree.Find(obj.ParentIndex1.RefID.ToString(), true);
                if (nodes.Length > 0)
                {
                    dSceneTree.AddToTree(newNode, nodes[0]);
                }
            }
            else if (obj.ParentIndex2.Index != -1)
            {
                nodes = dSceneTree.Find(obj.ParentIndex2.RefID.ToString(), true);
                if (nodes.Length > 0)
                {
                    dSceneTree.AddToTree(newNode, nodes[0]);
                }
            }
            else
            {
                dSceneTree.AddToTree(newNode, frameResourceRoot);
            }
            dSceneTree.SelectedNode = newNode;
            ApplyChangesToRenderable(obj);
        }

        private void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGrid pGrid = (s as PropertyGrid);
            if (pGrid.SelectedObject is FrameObjectBase)
            {
                FrameObjectBase obj = (dSceneTree.SelectedNode.Tag as FrameObjectBase);
                if (dSceneTree.SelectedNode.Tag == pGrid.SelectedObject)
                {
                    TreeNode selected = dSceneTree.SelectedNode;
                    selected.Text = (pGrid.SelectedObject as FrameObjectBase).Name.ToString();
                }
                if (e.ChangedItem.Label == "Index")
                {
                    //used just incase the user wants to set the parent to "root"
                    if ((int)e.ChangedItem.Value == -1)
                    {
                        if (e.ChangedItem.Parent.Label == "ParentIndex1")
                        {
                            obj.ParentIndex1.Index = -1;
                            obj.ParentIndex1.Name = "";
                            obj.ParentIndex1.RefID = 0;
                            obj.SubRef(FrameEntryRefTypes.Parent1);
                        }
                        else if (e.ChangedItem.Parent.Label == "ParentIndex2")
                        {
                            obj.ParentIndex2.Index = -1;
                            obj.ParentIndex2.Name = "";
                            obj.ParentIndex2.RefID = 0;
                            obj.SubRef(FrameEntryRefTypes.Parent2);
                        }
                    }
                }
                else if (e.ChangedItem.Label == "RefID")
                {
                    // Used just in case the user wants to set the parent to "root"
                    ParentInfo.ParentType ParentType = (e.ChangedItem.Parent.Label == "ParentIndex1" ? ParentInfo.ParentType.ParentIndex1 : ParentInfo.ParentType.ParentIndex2);
                    UpdateObjectParents(ParentType, (int)e.ChangedItem.Value);
                }
                ApplyChangesToRenderable((FrameObjectBase)pGrid.SelectedObject);
            }
            if (pGrid.SelectedObject is RenderRoad)
            {
                RenderRoad road = (pGrid.SelectedObject as RenderRoad);
                road.Spline.UpdateVertices();
            }
            if (pGrid.SelectedObject is RenderJunction)
            {
                RenderJunction junction = (pGrid.SelectedObject as RenderJunction);
                junction.UpdateVertices();
            }
            if (pGrid.SelectedObject is SoundSectorData sector)
            {
                sector.RebuildRenderBox();
                Graphics.SelectEntry(sector.RefID);
            }
            if (pGrid.SelectedObject is AIWorld_Type7 type7)
            {
                TreeNode node = dSceneTree.SelectedNode;
                if (node != null)
                {
                    AIWorld world = FindParentAIWorld(node);
                    world?.RequestPrimitiveBatchUpdate();
                }
                Graphics.SelectEntry(type7.RefID);
            }
            if (pGrid.SelectedObject is OBJData.ConnectionStruct connCopy)
            {
                TreeNode connNode = dSceneTree.SelectedNode;
                TreeNode parentNav = connNode.Parent?.Parent;
                if (parentNav?.Tag is RenderNav renderNav)
                {
                    OBJData objData = renderNav.GetData();
                    int index = connNode.Index;
                    if (index >= 0 && index < objData.connections.Length)
                    {
                        objData.connections[index].Flags = connCopy.Flags;
                        objData.connections[index].Unk80 = connCopy.Unk80;
                        objData.connections[index].NodeID = connCopy.NodeID;
                        objData.connections[index].ConnectedNodeID = connCopy.ConnectedNodeID;
                        connNode.Tag = objData.connections[index];
                    }
                }
            }
            if (pGrid.SelectedObject is BoundingBox bbox)
            {
                TreeNode selectedNode = dSceneTree.SelectedNode;
                if (selectedNode?.Parent?.Tag is UnkSet0 parentSet)
                {
                    OBJData targetOBJData = FindParentOBJData(selectedNode);
                    if (targetOBJData != null)
                    {
                        UpdateNavMeshVisualization(targetOBJData, parentSet);
                    }
                }
            }
            if (pGrid.SelectedObject is ActorEntry)
            {
                if (dSceneTree.SelectedNode.Tag == pGrid.SelectedObject)
                {
                    TreeNode selected = dSceneTree.SelectedNode;
                    selected.Text = (pGrid.SelectedObject as ActorEntry).EntityName.ToString();
                }
            }
            if (pGrid.SelectedObject is FrameHeaderScene)
            {
                if (dSceneTree.SelectedNode.Tag == pGrid.SelectedObject)
                {
                    TreeNode selected = dSceneTree.SelectedNode;
                    selected.Text = (pGrid.SelectedObject as FrameHeaderScene).Name.ToString();
                }
            }
            if (pGrid.SelectedObject is Instance instance && dSceneTree.SelectedNode.Parent.Tag is Object objGroup)
            {
                FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(objGroup.Name.Hash);

                if (groupRef != null)
                {
                    for (int i = 0; i < groupRef.Children.Count; i++)
                    {
                        var modelsToUpdate = UpdateTranslocatorPart(groupRef.Children[i], Matrix4x4.Identity, instance);
                        Graphics.UpdateInstanceBuffers(modelsToUpdate);
                    }
                }
                else
                {
                    Graphics.InstanceGizmo.UpdateInstanceBuffer(instance, Graphics.GetId3D11Device());
                }
                dPropertyGrid.SetObject(instance);//this is done so edit transforms tab updates as it didnt happen before
            }
            if (pGrid.SelectedObject is Grid trGrid)
            {
                RebuildTranslokatorGrids();
            }
            pGrid.Refresh();
        }
        private OBJData FindParentOBJData(TreeNode node)
        {
            while (node != null)
            {
                if (node.Tag is RenderNav renderNav)
                    return renderNav.GetData();
                node = node.Parent;
            }
            return null;
        }
        private AIWorld FindParentAIWorld(TreeNode node)
        {
            while (node != null)
            {
                if (node.Tag is AIWorld world)
                    return world;
                node = node.Parent;
            }
            return null;
        }

        private void CameraSpeedUpdate(object sender, EventArgs e)
        {
            UpdateCameraSpeed();
        }

        private void UpdateCameraSpeed()
        {
            if (CameraSpeedTool.Value == CameraSpeedTool.Increment)
            {
                CameraSpeedTool.Increment = CameraSpeedTool.Increment * Convert.ToDecimal(0.1);
            }
            else if (CameraSpeedTool.Value == (CameraSpeedTool.Increment * 10) + CameraSpeedTool.Increment)
            {
                CameraSpeedTool.Value = CameraSpeedTool.Increment * 20;
                CameraSpeedTool.Increment = CameraSpeedTool.Increment * 10;
            }
            ToolkitSettings.CameraSpeed = Convert.ToSingle(CameraSpeedTool.Value);
            ToolkitSettings.WriteKey("CameraSpeed", "ModelViewer", ToolkitSettings.CameraSpeed.ToString());
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.SelectedNode;
            if (FrameResource.IsFrameType(node.Tag))
            {
                FrameEntry entry = node.Tag as FrameEntry;
                if (entry is FrameObjectFrame frame && frame.Item != null)
                {
                    DeleteActorEntry(frame.Item);
                }
                bool bDidRemove = SceneData.FrameResource.DeleteFrame(entry);
                ToolkitAssert.Ensure(bDidRemove == true, "Failed to remove!");
                // we can just delete root node here, all children are vanquished
                dSceneTree.RemoveNode(node);
            }
            else if (node.Tag is OBJData.VertexStruct)
            {
                DeleteNavVertex(node);
            }
            else if (node.Tag.GetType() == typeof(FrameHeaderScene))
            {
                FrameHeaderScene scene = (node.Tag as FrameHeaderScene);
                bool bDidRemove = SceneData.FrameResource.DeleteScene(scene);
                ToolkitAssert.Ensure(bDidRemove == true, "Failed to remove!");
                // we can just delete root node here, all children are vanquished
                dSceneTree.RemoveNode(node);
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                dSceneTree.RemoveNode(node);
                int iName = Convert.ToInt32(node.Name);
                Graphics.DeleteAsset(iName);
            }
            else if (node.Tag.GetType() == typeof(RenderRoad))
            {
                dSceneTree.RemoveNode(node);
                Graphics.DeleteAsset(int.Parse(node.Name));
            }
            else if (node.Tag is ActorEntry actorEntry)
            {
                DeleteActorEntry(actorEntry);
            }
            else if (node.Tag.GetType() == typeof(RenderJunction))
            {
                dSceneTree.RemoveNode(node);
                Graphics.DeleteAsset(int.Parse(node.Name));
            }
            else if (node.Tag is IType aiPoint)
            {
                DeleteAIPoint(node);
            }
            else if (node.Tag.GetType() == typeof(Collision.CollisionModel))
            {
                dSceneTree.RemoveNode(node);
                Collision.CollisionModel data = (node.Tag as Collision.CollisionModel);
                SceneData.Collisions.RemoveModel(data);
                RenderStorageSingleton.Instance.StaticCollisions.TryRemove(data.Hash);
                for (int i = 0; i != node.Nodes.Count; i++)
                {
                    int iName = Convert.ToInt32(node.Nodes[i].Name);
                    Graphics.DeleteAsset(iName);
                }
            }
            else if (node.Tag is Instance instance)
            {
                DeleteTRInstance(node);
            }
            else if (node.Tag is Object obj)
            {
                DeleteTRObject(node);
            }
            else if (node.Tag is BoundingBox bbox && node.Parent?.Tag is UnkSet0 parentSet)
            {
                OBJData targetOBJData = null;
                TreeNode navNode = node.Parent?.Parent?.Parent;
                if (navNode?.Tag is RenderNav renderNav)
                    targetOBJData = renderNav.GetData();

                if (targetOBJData == null)
                {
                    MessageBox.Show("Cannot find parent OBJData.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var list = parentSet.EdgeBoxes.ToList();
                list.Remove(bbox);
                parentSet.EdgeBoxes = list.ToArray();
                parentSet.NumEdges = list.Count;
                dSceneTree.RemoveNode(node);

                UpdateNavMeshVisualization(targetOBJData, parentSet);
            }
            else if (node.Tag is ObjectGroup og)
            {
                while (node.Nodes.Count > 0)
                {
                    DeleteTRObject(node.FirstNode);
                }
                dSceneTree.RemoveNode(node);
            }
        }
        private void DeleteNavVertex(TreeNode node)
        {
            TreeNode parentNavNode = node.Parent;
            while (parentNavNode != null && !(parentNavNode.Tag is RenderNav))
                parentNavNode = parentNavNode.Parent;

            if (parentNavNode == null || !(parentNavNode.Tag is RenderNav nav))
            {
                MessageBox.Show("Cannot find parent RenderNav.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OBJData data = nav.GetData();
            OBJData.VertexStruct vertexToDelete = node.Tag as OBJData.VertexStruct;

            int oldIndex = -1;
            for (int i = 0; i < data.vertices.Length; i++)
            {
                if (data.vertices[i] == vertexToDelete)
                {
                    oldIndex = i;
                    break;
                }
            }

            if (oldIndex == -1)
            {
                MessageBox.Show("Vertex not found in data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (int.TryParse(node.Name, out int refID))
            {
                Graphics.DeleteAsset(refID);
            }

            OBJData.VertexStruct[] newVertices = new OBJData.VertexStruct[data.vertices.Length - 1];
            int[] oldToNewIndex = new int[data.vertices.Length];
            for (int i = 0, j = 0; i < data.vertices.Length; i++)
            {
                if (i == oldIndex)
                {
                    oldToNewIndex[i] = -1;
                    continue;
                }
                newVertices[j] = data.vertices[i];
                oldToNewIndex[i] = j;
                j++;
            }

            List<OBJData.ConnectionStruct> newConnections = new List<OBJData.ConnectionStruct>();
            foreach (var conn in data.connections)
            {
                int nodeID = (int)conn.NodeID;
                int connectedID = (int)conn.ConnectedNodeID;
                if (nodeID == oldIndex || connectedID == oldIndex)
                    continue;

                if (oldToNewIndex[nodeID] == -1 || oldToNewIndex[connectedID] == -1)
                    continue;

                OBJData.ConnectionStruct newConn = conn;
                newConn.NodeID = (uint)oldToNewIndex[nodeID];
                newConn.ConnectedNodeID = (uint)oldToNewIndex[connectedID];
                newConnections.Add(newConn);
            }

            newConnections = newConnections.OrderBy(c => c.NodeID).ToList();

            data.vertices = newVertices;
            data.connections = newConnections.ToArray();
            data.vertSize = newVertices.Length;

            data.GenerateConnections();

            nav.RebuildAllConnections();

            node.Remove();
        }

        private void DeleteAIPoint(TreeNode node)
        {
            if (node.Tag is IType aiPoint)
            {
                if (node.Parent?.Tag is AIWorld_Type1 parentGroup)
                {
                    parentGroup.AIPoints.Remove(aiPoint);
                    if (parentGroup.World != null)
                    {
                        parentGroup.World.AIPoints.Remove(aiPoint);
                        parentGroup.World.RequestPrimitiveBatchUpdate();
                    }
                }
                else if (node.Parent?.Tag is AIWorld parentWorld)
                {
                    parentWorld.AIPoints.Remove(aiPoint);
                    parentWorld.RequestPrimitiveBatchUpdate();
                }
                node.Parent?.Nodes.Remove(node);
            }
        }

        private void DuplicateButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.SelectedNode;
            FrameObjectBase newEntry = null;
            //new safety net
            if (FrameResource.IsFrameType(node.Tag))
            {
                if (node.Tag is FrameObjectSingleMesh || node.Tag is FrameObjectModel)
                {
                    DialogResult result = MessageBox.Show("$DUPLICATE_MATERIAL_BLOCK", "Toolkit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    //we don't want to duplicate anymore
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                //is this even needed? hmm.
                if (node.Tag.GetType() == typeof(FrameObjectArea))
                {
                    newEntry = new FrameObjectArea((FrameObjectArea)node.Tag);
                    FrameObjectArea area = (newEntry as FrameObjectArea);
                    RenderBoundingBox RenderBox = RenderableFactory.BuildBoundingBox(area.Bounds, area.WorldTransform);
                    Graphics.InitObjectStack.Add(area.RefID, RenderBox);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectCamera)) newEntry = new FrameObjectCamera((FrameObjectCamera)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectCollision)) newEntry = new FrameObjectCollision((FrameObjectCollision)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectComponent_U005)) newEntry = new FrameObjectComponent_U005((FrameObjectComponent_U005)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectDummy))
                {
                    newEntry = new FrameObjectDummy((FrameObjectDummy)node.Tag);
                    FrameObjectDummy dummy = (newEntry as FrameObjectDummy);
                    RenderBoundingBox RenderBox = RenderableFactory.BuildBoundingBox(dummy.Bounds, dummy.WorldTransform);
                    Graphics.InitObjectStack.Add(dummy.RefID, RenderBox);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectDeflector)) newEntry = new FrameObjectDeflector((FrameObjectDeflector)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectFrame)) newEntry = new FrameObjectFrame((FrameObjectFrame)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectJoint)) newEntry = new FrameObjectJoint((FrameObjectJoint)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectLight)) newEntry = new FrameObjectLight((FrameObjectLight)node.Tag);
                else if (node.Tag.GetType() == typeof(FrameObjectModel))
                {
                    newEntry = new FrameObjectModel((FrameObjectModel)node.Tag);
                    FrameObjectModel mesh = (newEntry as FrameObjectModel);
                    SceneData.FrameResource.DuplicateBlocks(mesh);
                    RenderModel model = RenderableFactory.BuildRenderModelFromFrame(mesh);
                    Graphics.InitObjectStack.Add(mesh.RefID, model);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectSector))
                {
                    newEntry = new FrameObjectSector((FrameObjectSector)node.Tag);
                    FrameObjectSector sector = (newEntry as FrameObjectSector);
                    RenderBoundingBox RenderBox = RenderableFactory.BuildBoundingBox(sector.Bounds, sector.WorldTransform);
                    Graphics.InitObjectStack.Add(sector.RefID, RenderBox);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectSingleMesh))
                {
                    newEntry = new FrameObjectSingleMesh((FrameObjectSingleMesh)node.Tag);
                    FrameObjectSingleMesh mesh = (newEntry as FrameObjectSingleMesh);
                    SceneData.FrameResource.DuplicateBlocks(mesh);
                    RenderModel model = RenderableFactory.BuildRenderModelFromFrame(mesh);
                    Graphics.InitObjectStack.Add(mesh.RefID, model);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectTarget)) newEntry = new FrameObjectTarget((FrameObjectTarget)node.Tag);
                else newEntry = new FrameObjectBase((FrameObjectBase)node.Tag);
                // Try and add the numeric value to the end of the name.
                // Either increment on the numeric value or add it.
                string FrameName = newEntry.Name.String;
                int LastIndex = FrameName.LastIndexOf('_');
                bool bIsValid = false;
                if (LastIndex != -1)
                {
                    int NumericValue = 0;
                    string NameSplit = FrameName.Substring(LastIndex).Remove(0, 1);
                    string LeftSplit = FrameName.Substring(0, LastIndex);
                    bool bHasNumericValue = int.TryParse(NameSplit, out NumericValue);
                    if (bHasNumericValue)
                    {
                        NumericValue = CheckIfDuplicationContainsString(LeftSplit);
                        string NumericValueStringed = string.Format("_{0}", NumericValue);
                        newEntry.Name.Set(LeftSplit + NumericValueStringed);
                        bIsValid = true;
                    }
                }
                if (!bIsValid)
                {
                    int NewNumericValue = CheckIfDuplicationContainsString(FrameName);
                    string NumericString = string.Format("_{0}", NewNumericValue);
                    newEntry.Name.Set(newEntry.Name.String + NumericString);
                }
                TreeNode tNode = new TreeNode(newEntry.ToString());
                tNode.Tag = newEntry;
                tNode.Name = newEntry.RefID.ToString();
                //fix for objects with -1 on root.
                if (newEntry.ParentIndex2.Index == -1) dSceneTree.AddToTree(tNode, frameResourceRoot);
                else dSceneTree.AddToTree(tNode, dSceneTree.Find(newEntry.ParentIndex2.RefID.ToString(), true)[0]);
                SceneData.FrameResource.FrameObjects.Add(newEntry.RefID, newEntry);
                dSceneTree.SelectedNode = tNode;
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                Collision.Placement placement = new Collision.Placement((Collision.Placement)node.Tag);
                int pIdxName = 0;
                int.TryParse(node.Text, out pIdxName);
                pIdxName++;
                int refID = RefManager.GetNewRefID();
                TreeNode child = new TreeNode();
                child.Text = pIdxName.ToString();
                child.Name = refID.ToString();
                child.Tag = placement;
                dSceneTree.AddToTree(child, node.Parent);
                RenderInstance instance = new RenderInstance();
                instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                instance.SetTransform(placement.Transform);
                Graphics.InitObjectStack.Add(refID, instance);
            }
            else if (node.Tag is Instance instance)
            {
                TranslokatorNewInstance(node.Parent, instance);
            }
        }

        private int CheckIfDuplicationContainsString(string Key)
        {
            int NewNumericValue = 0;
            if (NamesAndDuplicationStore.ContainsKey(Key))
            {
                NewNumericValue = ++NamesAndDuplicationStore[Key];
            }
            else
            {
                NamesAndDuplicationStore.Add(Key, NewNumericValue);
            }
            return NewNumericValue;
        }

        private void Export3DButton_Click(object sender, EventArgs e)
        {
            ModelWrapper WrapperObject = null;
            if (dSceneTree.SelectedNode.Tag.GetType() == typeof(Collision.CollisionModel))
            {
                WrapperObject = ExportCollision(dSceneTree.SelectedNode.Tag as Collision.CollisionModel);
            }
            else if (dSceneTree.SelectedNode.Tag.GetType() == typeof(FrameHeaderScene))
            {
                WrapperObject = ExportScene(dSceneTree.SelectedNode.Tag as FrameHeaderScene);
            }
            else if (dSceneTree.SelectedNode.Text == "Collision Data")
            {
                WrapperObject = ExportCollisions(dSceneTree.SelectedNode);
            }
            else
            {
                WrapperObject = Export3DFrame();
            }
            MT_ObjectBundle CurrentBundle = new MT_ObjectBundle();
            CurrentBundle.Objects = new MT_Object[1];
            CurrentBundle.Objects[0] = WrapperObject.ModelObject;
            FrameResourceModelExporter ModelExporter = new FrameResourceModelExporter(CurrentBundle);
            if (ModelExporter.ShowDialog() != DialogResult.OK)
            {
                ModelExporter.Dispose();
                return;
            }
            // Now we should choose on a name
            if (SaveFileDialog != null)
            {
                SaveFileDialog.Reset();
            }
            SaveFileDialog.FileName = CurrentBundle.Objects[0].ObjectName;
            SaveFileDialog.RestoreDirectory = true;
            SaveFileDialog.Filter = "GLTF File (Binary) (*.glb)|*.glb|GLTF File (ASCII) (*.gltf)|*.gltf*";
            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            WrapperObject.ExportObject(SaveFileDialog.FileName, 0);
        }

        private ModelWrapper ExportCollision(Collision.CollisionModel data)
        {
            Collision.Placement[] TempPlacements = new Collision.Placement[1];
            TempPlacements[0] = new Collision.Placement();
            MT_Object CollisionObject = MT_Object.TryBuildObject(data, TempPlacements);
            ModelWrapper WrapperObject = new ModelWrapper();
            WrapperObject.ModelObject = CollisionObject;
            return WrapperObject;
        }

        private ModelWrapper ExportCollisions(TreeNode CollisionRoot)
        {
            MT_Object RootObject = new MT_Object();
            RootObject.ObjectName = "COLLISION_ROOT";
            RootObject.ObjectType = MT_ObjectType.Dummy;
            List<MT_Object> ChildObjects = new List<MT_Object>();
            foreach (TreeNode CollisionNode in CollisionRoot.Nodes)
            {
                // Skip non collision models
                Collision.CollisionModel CurrentModel = (CollisionNode.Tag as Collision.CollisionModel);
                if (CurrentModel == null)
                {
                    continue;
                }
                List<Collision.Placement> Placements = new List<Collision.Placement>();
                foreach (TreeNode PlacementNode in CollisionNode.Nodes)
                {
                    if (PlacementNode.Tag.GetType() == typeof(Collision.Placement))
                    {
                        Placements.Add(PlacementNode.Tag as Collision.Placement);
                    }
                }
                // construct collision using model and placements
                MT_Object NewCollisionObject = MT_Object.TryBuildObject(CurrentModel, Placements.ToArray());
                ChildObjects.Add(NewCollisionObject);
            }
            RootObject.Children = ChildObjects.ToArray();
            RootObject.ObjectFlags |= MT_ObjectFlags.HasChildren;
            ModelWrapper WrapperObject = new ModelWrapper();
            WrapperObject.ModelObject = RootObject;
            return WrapperObject;
        }

        private ModelWrapper Export3DFrame()
        {
            FrameObjectBase FrameObject = (dSceneTree.SelectedNode.Tag as FrameObjectBase);
            ModelWrapper ModelWrapperObject = null;
            if (FrameObject is FrameObjectSingleMesh)
            {
                FrameObjectSingleMesh SingleMesh = (FrameObject as FrameObjectSingleMesh);
                IndexBuffer[] indexBuffers = new IndexBuffer[SingleMesh.Geometry.LOD.Length];
                VertexBuffer[] vertexBuffers = new VertexBuffer[SingleMesh.Geometry.LOD.Length];
                //we need to retrieve buffers first.
                for (int c = 0; c != SingleMesh.Geometry.LOD.Length; c++)
                {
                    indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(SingleMesh.Geometry.LOD[c].IndexBufferRef.Hash);
                    vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(SingleMesh.Geometry.LOD[c].VertexBufferRef.Hash);
                }
                // Construct wrapper (based on model)
                if (FrameObject is FrameObjectModel)
                {
                    ModelWrapperObject = new ModelWrapper(FrameObject as FrameObjectModel, indexBuffers, vertexBuffers);
                }
                else
                {
                    ModelWrapperObject = new ModelWrapper(FrameObject as FrameObjectSingleMesh, indexBuffers, vertexBuffers);
                }
            }
            else
            {
                ModelWrapperObject = new ModelWrapper(FrameObject);
            }
            return ModelWrapperObject;
        }

        private ModelWrapper ExportScene(FrameHeaderScene Scene)
        {
            ModelWrapper ModelWrapperObject = new ModelWrapper(Scene);
            return ModelWrapperObject;
        }

        private void AddButtonOnClick(object sender, EventArgs e)
        {
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_FRADD"));
            form.LoadOption(new ControlOptionFrameAdd());
            if (form.ShowDialog() == DialogResult.OK)
            {
                ControlOptionFrameAdd window = (form.control as ControlOptionFrameAdd);
                FrameResourceObjectType selection = window.GetSelectedType();
                CreateNewEntry(selection, form.GetInputText(), window.GetAddToNameTable());
            }
        }

        private void AddSceneFolderButton_Click(object sender, EventArgs e)
        {
            var scene = SceneData.FrameResource.AddSceneFolder("NEW_SCENE");
            TreeNode node = new TreeNode(scene.ToString());
            node.Tag = scene;
            node.Name = scene.RefID.ToString();
            dSceneTree.AddToTree(node, frameResourceRoot);
        }

        // TODO: Need to cleanup this function, it's atrocious.
        private void ValidateCollisionFile()
        {
            // Check if we need to create a collisions folder
            if (SceneData.Collisions == null)
            {
                DialogResult result = MessageBox.Show(Language.GetString("$NO_COL_FILE_CREATE_NEW"), "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SceneData.Collisions = new Collision();
                    SceneData.Collisions.Name = Path.Combine(SceneData.ScenePath, "Collisions_0.col");
                    TreeNode node = new TreeNode("Collision Data");
                    node.Tag = "Folder";
                    collisionRoot = node;
                    dSceneTree.AddToTree(node);
                    collisionRoot.Collapse(false);
                }
                else
                {
                    MessageBox.Show(Language.GetString("$CANNOT_CREATE_COL"), "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    return;
                }
            }
        }

        private Collision.CollisionModel CreateCollision(Collision.CollisionModel collisionModel)
        {
            ulong CollisionHash = collisionModel.Hash;
            Collision.CollisionModel CollisionModel = null;
            if (!SceneData.Collisions.Models.ContainsKey(CollisionHash))
            {
                // Create a new renderable for collision object
                RenderStaticCollision collision = new RenderStaticCollision();
                collision.ConvertCollisionToRender(collisionModel.Hash, collisionModel.Mesh);
                RenderStorageSingleton.Instance.StaticCollisions.TryAdd(collisionModel.Hash, collision);
                // Push it onto the collisions dictionary
                SceneData.Collisions.Models.Add(collisionModel.Hash, collisionModel);
                CollisionModel = collisionModel;
                // Create a new TreeNode for the CollisionModel
                TreeNode CollisionNode = new TreeNode(CollisionHash.ToString());
                CollisionNode.Text = CollisionHash.ToString();
                CollisionNode.Name = CollisionHash.ToString();
                CollisionNode.Tag = collisionModel;
                dSceneTree.AddToTree(CollisionNode, collisionRoot);
            }
            else
            {
                // Get the model if it exists
                CollisionModel = SceneData.Collisions.Models[CollisionHash];
            }
            return CollisionModel;
        }

        private Collision.Placement CreatePlacement(Collision.CollisionModel ColModel, Vector3 Position, Quaternion Rotation)
        {
            // Create a new placement for this mesh
            Collision.Placement placement = new Collision.Placement();
            placement.Hash = ColModel.Hash;
            placement.Position = Position;
            placement.RotationDegrees = Vector3.Zero;
            // Try and find the collision node
            TreeNode ExistingCollisionNode = dSceneTree.GetTreeNode(ColModel.Hash.ToString(), collisionRoot, true);
            if (ExistingCollisionNode == null)
            {
                // Create a new TreeNode for the CollisionModel
                TreeNode CollisionNode = new TreeNode(ColModel.Hash.ToString());
                CollisionNode.Text = ColModel.Hash.ToString();
                CollisionNode.Name = ColModel.Hash.ToString();
                CollisionNode.Tag = ColModel;
                ExistingCollisionNode = CollisionNode;
                dSceneTree.AddToTree(CollisionNode, collisionRoot);
            }
            // Add new Placement object
            int refID = RefManager.GetNewRefID();
            TreeNode child = new TreeNode();
            child.Text = ExistingCollisionNode.Nodes.Count.ToString();
            child.Name = refID.ToString();
            child.Tag = placement;
            dSceneTree.AddToTree(child, ExistingCollisionNode);
            dSceneTree.SelectedNode = child;
            // Complete it
            RenderInstance instance = new RenderInstance();
            instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
            instance.SetTransform(placement.Transform);
            Graphics.InitObjectStack.Add(refID, instance);
            SceneData.Collisions.Placements.Add(placement);
            return placement;
        }

        private void CameraToolsOnValueChanged(object sender, EventArgs e)
        {
            Graphics.Camera.Position = new Vector3(Convert.ToSingle(PositionXTool.Value), Convert.ToSingle(PositionYTool.Value), Convert.ToSingle(PositionZTool.Value));
        }

        private void OnViewTopButtonClicked(object sender, EventArgs e)
        {
            Graphics.Camera.SetRotation(0.0f, 180.0f);
            lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnViewFrontButtonClicked(object sender, EventArgs e)
        {
            Graphics.Camera.SetRotation(90.0f, 90.0f);
            lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnKeyUpDockedPanel(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bHideChildren = false;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                dSceneTree.DeleteButton.PerformClick();
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dSceneTree.SelectedNode.PrevVisibleNode != null)
                {
                    dSceneTree.SelectedNode = dSceneTree.SelectedNode.PrevVisibleNode;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (dSceneTree.SelectedNode.NextVisibleNode != null)
                {
                    dSceneTree.SelectedNode = dSceneTree.SelectedNode.NextVisibleNode;
                }
            }
            e.Handled = true;
        }

        private void OnKeyDownDockedPanel(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bHideChildren = true;
            }
            e.Handled = true;
        }

        private void SwitchMode(bool isSelectMode)
        {
            bSelectMode = isSelectMode;
            CurrentModeButton.Text = (bSelectMode) ? "Select Mode" : "Edit Mode";
        }

        private void EditLighting_Click(object sender, EventArgs e)
        {
            dPropertyGrid.SetObject(Graphics.WorldSettings);
        }

        private void Button_TestConvert_Click(object sender, EventArgs e)
        {
            ConvertBuffer(1);
        }

        private void Button_TestConvert32_Click(object sender, EventArgs e)
        {
            ConvertBuffer(2);
        }

        private void ConvertBuffer(int format)
        {
            var frames = SceneData.FrameResource.FrameObjects;
            var geoms = SceneData.FrameResource.FrameGeometries;
            var mats = SceneData.FrameResource.FrameMaterials;
            var indexbuffer = SceneData.IndexBufferPool.Buffers;
            foreach (var geom in geoms)
            {
                foreach (var lod in geom.Value.LOD)
                {
                    lod.SplitInfo.IndexStride = (format == 1 ? 2 : 4);
                }
            }
            foreach (var buffer in indexbuffer)
            {
                buffer.Value.SetFormat(format);
            }
            Save();
        }

        private void ConvertFrameToRender(FrameObjectBase parent)
        {
            IRenderer asset = BuildRenderObjectFromFrame(parent, null);
            if (asset != null)
            {
                Graphics.InitObjectStack.TryAdd(parent.RefID, asset);
            }
        }

        private void ConvertNodeToFrame(TreeNode node)
        {

            if (node?.Tag == null) return;

            if (node.Tag is FrameHeaderScene || node.Tag is string tagStr && tagStr == "Folder")
            {
                foreach (TreeNode child in node.Nodes)
                {
                    ConvertNodeToFrame(child);
                }
                return;
            }

            if (node.Tag is FrameObjectBase frameObj)
            {

                ConvertFrameToRender(frameObj);

                foreach (TreeNode child in node.Nodes)
                {
                    ConvertNodeToFrame(child);
                }
            }
        }

        private void Button_ImportFrame_OnClicked(object sender, EventArgs e)
        {
            if (FrameBrowser.ShowDialog() == DialogResult.OK)
            {
                string Filename = FrameBrowser.FileName;
                if (FrameBrowser.FilterIndex.Equals(1))
                {
                    PopulateImportedData(Filename);
                }
                else
                {
                    TreeNode parent = SceneData.FrameResource.ReadFramesFromImport(Filename);
                    dSceneTree.AddToTree(parent, frameResourceRoot);
                    ConvertNodeToFrame(parent);
                }
            }
        }

        private void Button_DumpTexture_Click(object sender, EventArgs e)
        {
            List<string> AllTextures = new List<string>();
            // Get header scene name
            string HeaderSceneName = SceneData.FrameResource.Header.SceneName.String;
            if (!string.IsNullOrEmpty(HeaderSceneName))
            {
                if (!AllTextures.Contains(HeaderSceneName))
                {
                    AllTextures.Add(HeaderSceneName);
                }
            }
            // Iterate through FrameObjects
            foreach (var Frame in SceneData.FrameResource.FrameObjects)
            {
                // We can only take textures from SingleMesh
                var SingleMesh = (Frame.Value as FrameObjectSingleMesh);
                if (SingleMesh != null)
                {
                    // Store OM texture
                    if (!AllTextures.Contains(SingleMesh.OMTextureHash.String))
                    {
                        AllTextures.Add(SingleMesh.OMTextureHash.String);
                    }
                    // Collect textures from FrameMaterial object.
                    List<string> CollectedTextures = SingleMesh.Material.CollectAllTextureNames();
                    if (CollectedTextures != null)
                    {
                        foreach (var Texture in CollectedTextures)
                        {
                            if (!AllTextures.Contains(Texture))
                            {
                                AllTextures.Add(Texture);
                            }
                        }
                    }
                }
            }
            File.WriteAllLines("AllTextures.txt", AllTextures.ToArray());
        }

        private void UpdatePositionElement(Vector3 InPosition)
        {
            PositionXTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
            PositionYTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
            PositionZTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
            PositionXTool.Value = (decimal)InPosition.X;
            PositionYTool.Value = (decimal)InPosition.Y;
            PositionZTool.Value = (decimal)InPosition.Z;
            PositionXTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
            PositionYTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
            PositionZTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
        }

        private void Button_ImportBundle_OnClick(object sender, EventArgs e)
        {
            if (MeshBrowser.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            if (Path.Exists(MeshBrowser.FileName) == false)
            {
                return;
            }
            // pass to importer
            FrameResourceModelImporter modelForm = new FrameResourceModelImporter(MeshBrowser.FileName);
            DialogResult Result = modelForm.ShowDialog();
            if (Result != DialogResult.OK)
            {
                modelForm.Dispose();
                return;
            }
            // TODO: In an ideal world this would not live in MapEditor.cs
            // and probably live within FrameResourceModelImporter.
            // Only ask we they want to save the materials if we have some.
            if (modelForm.NewMaterials.Count > 0)
            {
                if (MessageBox.Show(Language.GetString("$Q_IMPORT_MATERIALS"), "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Manager will handle adding for us.
                    MaterialsManager.AddMaterialsToLibrary(modelForm.NewMaterials);
                }
            }
            // Continue with the importing of the bundle
            foreach (MT_Object ModelObject in modelForm.CurrentBundle.Objects)
            {
                ConstructFrameFromImportedObject(ModelObject, frameResourceRoot);
            }
            modelForm.Dispose();
        }

        private void ConstructFrameFromImportedObject(MT_Object ObjectInfo, TreeNode Parent, FrameObjectBase topLevelFrame = null)
        {
            ModelWrapper Wrapper = new ModelWrapper();
            Wrapper.ModelObject = ObjectInfo;
            TreeNode FrameNode = null;

            if (ObjectInfo.ObjectType == MT_ObjectType.Scene)
            {
                var scene = SceneData.FrameResource.AddSceneFolder(ObjectInfo.ObjectName);
                FrameNode = new TreeNode(scene.ToString());
                FrameNode.Tag = scene;
                FrameNode.Name = scene.RefID.ToString();
                dSceneTree.AddToTree(FrameNode, frameResourceRoot);
            }
            else
            {
                FrameObjectBase NewFrame = FrameFactory.ConstructFrameByObjectType(ObjectInfo.ObjectType, SceneData.FrameResource);
                if (NewFrame != null)
                {
                    if (topLevelFrame == null)
                    {
                        topLevelFrame = NewFrame;
                    }

                    Matrix4x4 LocalTransform = MatrixUtils.SetMatrix(ObjectInfo.Rotation, ObjectInfo.Scale, ObjectInfo.Position);
                    NewFrame.LocalTransform = LocalTransform;
                    NewFrame.Name.Set(ObjectInfo.ObjectName);
                    NewFrame.IsOnFrameTable = true;

                    if (ObjectInfo.ObjectType == MT_ObjectType.StaticMesh)
                    {
                        FrameObjectSingleMesh NewMesh = (NewFrame as FrameObjectSingleMesh);
                        NewMesh.CreateMeshFromRawModel(Wrapper);
                        CreateMeshBuffers(Wrapper);
                    }
                    else if (ObjectInfo.ObjectType == MT_ObjectType.RiggedMesh)
                    {
                        FrameObjectModel NewMesh = (NewFrame as FrameObjectModel);
                        NewMesh.CreateMeshFromRawModel(Wrapper);
                        CreateMeshBuffers(Wrapper);
                    }

                    FrameNode = new TreeNode(NewFrame.Name.ToString());
                    FrameNode.Tag = NewFrame;
                    FrameNode.Name = NewFrame.RefID.ToString();
                    dSceneTree.AddToTree(FrameNode, Parent);

                    if (Parent.Tag is FrameHeaderScene parentScene)
                    {
                        if (topLevelFrame != null && topLevelFrame != NewFrame)
                        {
                            SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2, NewFrame, topLevelFrame);
                        }
                    }
                    else if (Parent.Tag is FrameEntry parentEntry)
                    {
                        SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex1, NewFrame, parentEntry);
                        if (topLevelFrame != null && topLevelFrame != NewFrame)
                        {
                            SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2, NewFrame, topLevelFrame);
                        }
                    }
                    if (NewFrame is FrameObjectFrame frameFrame && !string.IsNullOrEmpty(ObjectInfo.ActorHash))
                    {
                        frameFrame.ActorHash.Set(ObjectInfo.ActorHash);
                    }
                    if (ObjectInfo.ObjectType == MT_ObjectType.ItemDesc && NewFrame is FrameObjectCollision collisionFrame)
                    {
                        collisionFrame.Hash = ObjectInfo.CollisionHash;
                    }

                    IRenderer Renderer = BuildRenderObjectFromFrame(NewFrame, null);
                    if (Renderer != null)
                    {
                        Graphics.InitObjectStack.Add(NewFrame.RefID, Renderer);
                    }
                }
            }

            if (ObjectInfo.ObjectFlags.HasFlag(MT_ObjectFlags.HasCollisions))
            {
                ValidateCollisionFile();
                Collision.CollisionModel collisionModel = new CollisionModelBuilder().BuildFromMTCollision(ObjectInfo.ObjectName, ObjectInfo.Collision);
                CreateCollision(collisionModel);
                foreach (MT_CollisionInstance ColInstance in ObjectInfo.Collision.Instances)
                {
                    CreatePlacement(collisionModel, ColInstance.Position, ColInstance.Rotation);
                }
            }

            if (ObjectInfo.ObjectFlags.HasFlag(MT_ObjectFlags.HasChildren))
            {
                foreach (MT_Object Child in ObjectInfo.Children)
                {
                    ConstructFrameFromImportedObject(Child, FrameNode, topLevelFrame);
                }
            }
        }

        private void TranslokatorNewInstanceButton_Click(object sender, EventArgs e)
        {
            TranslokatorNewInstance(dSceneTree.SelectedNode, null);
        }

        private void TranslokatorNewInstance(TreeNode parentObj, Instance old)
        {
            Instance newInstance = (old == null) ? new Instance() : new Instance(old);
            newInstance.RefID = RefManager.GetNewRefID();
            TreeNode newInstanceNode = new TreeNode(parentObj.Text + " " + parentObj.Nodes.Count.ToString());
            newInstanceNode.Tag = newInstance;
            newInstanceNode.Name = newInstance.RefID.ToString();
            Object parent = parentObj.Tag as Object;
            FrameObjectBase frameref = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(parent.Name.Hash);
            if (frameref != null && frameref.HasMeshObject())
            {
                for (int i = 0; i < frameref.Children.Count; i++)
                {
                    InstanceTranslokatorPart(Graphics.Assets, frameref.Children[i], Matrix4x4.Identity, newInstance, true);
                }
            }
            else
            {
                Graphics.InstanceGizmo.InstanceTranslokator(newInstance, Graphics.GetId3D11Device());
            }
            dSceneTree.AddToTree(newInstanceNode, parentObj);
        }

        private void UpdateInstanceVisualisation(TreeNode instanceNode, Object trObject, bool visibility)
        {
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(trObject.Name.Hash);
            Instance instance = instanceNode.Tag as Instance;
            if (visibility)
            {
                if (groupRef != null && groupRef.HasMeshObject())
                {
                    for (int i = 0; i < groupRef.Children.Count; i++)
                    {
                        InstanceTranslokatorPart(Graphics.Assets, groupRef.Children[i], Matrix4x4.Identity, instance, true);
                    }
                }
                else
                {
                    Graphics.InstanceGizmo.InstanceTranslokator(instance, Graphics.GetId3D11Device());
                }
            }
            else
            {
                if (groupRef != null && groupRef.HasMeshObject())
                {
                    Graphics.DeleteInstance(groupRef, instance.RefID);
                }
                else
                {
                    Graphics.DeleteInstance(instance.RefID);
                }
            }
        }

        private void ActorEntryNewTRObjectButton_Click(object sender, EventArgs e)
        {
            TreeNode ActorNode = dSceneTree.SelectedNode;
            ActorEntry actor = ActorNode.Tag as ActorEntry;
            if (ActorNode == null || actor == null)
            {
                return;
            }
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(actor.FrameNameHash);
            if (groupRef == null)//todo: once multisds is added, tweak this
            {
                if (MessageBox.Show("There is no matching Frame: " + actor.FrameName + " in FrameResource contents. If you intend to reference Frame of this name, it is not present. Do you want to continue?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            TreeNode ogNode = dSceneTree.GetObjectGroupByActorType(translokatorRoot, actor.ActorTypeID);
            if (ogNode == null)
            {
                //create objectgroup if not present
                ObjectGroup newOG = new ObjectGroup();
                newOG.ActorType = (ActorTypes)actor.ActorTypeID;
                TreeNode newOGNode = new TreeNode(String.Format("Object Group: [{0}]", newOG.ActorType));
                newOGNode.Tag = newOG;
                dSceneTree.AddToTree(newOGNode, translokatorRoot.Nodes[0]);
                ogNode = newOGNode;
                Log.WriteLine("New Translokator ObjectGroup:" + newOG.ActorType, LoggingTypes.MESSAGE, LogCategoryTypes.FUNCTION);
            }
            if (dSceneTree.ObjectGroupHasObject(ogNode, actor.FrameNameHash))
            {
                ToolkitAssert.Ensure(!dSceneTree.ObjectGroupHasObject(ogNode, actor.FrameNameHash), "Error: The Object: " + actor.FrameName + " is already present.");
                return;
            }
            else
            {
                Object newObj = new Object();
                newObj.Name.Set(actor.FrameName);
                TreeNode objNode = new TreeNode(newObj.Name.ToString());
                objNode.Tag = newObj;
                dSceneTree.AddToTree(objNode, ogNode);
                Log.WriteLine("New Translokator Object:" + newObj.Name.String, LoggingTypes.MESSAGE, LogCategoryTypes.FUNCTION);
            }
        }

        private void DeleteTRInstance(TreeNode instanceNode)
        {
            Instance instance = instanceNode.Tag as Instance;
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>((instanceNode.Parent.Tag as Object).Name.Hash);
            dSceneTree.RemoveNode(instanceNode);
            if (groupRef != null)
            {
                Graphics.DeleteInstance(groupRef, instance.RefID);
            }
            else
            {
                Graphics.DeleteInstance(instance.RefID);
            }
        }

        private void DeleteTRObject(TreeNode objectNode)
        {
            while (objectNode.Nodes.Count > 0)
            {
                DeleteTRInstance(objectNode.FirstNode);
            }
            dSceneTree.RemoveNode(objectNode);
        }

        private void RebuildTranslokatorGrids()
        {
            SceneData.Translokator.RebuildGridData();
            Graphics.BuildTranslokatorGrid(SceneData.Translokator);
            TreeNode gridsNode = null;
            foreach (TreeNode node in translokatorRoot.Nodes)
            {
                if (node.Text.Equals("Grids", StringComparison.InvariantCultureIgnoreCase))
                {
                    gridsNode = node;
                    break;
                }
            }
            for (int i = 0; i < gridsNode.Nodes.Count; i++)
            {
                TreeNode child = gridsNode.Nodes[i];

                if (child.Tag is Grid)
                {
                    Graphics.SetTranslokatorGridEnabled(i, child.Checked && child.CheckIfParentsAreValid());
                }
            }
        }

        private void TRRebuildObjectButton_Click(object sender, EventArgs e)
        {
            TreeNode ObjectNode = dSceneTree.SelectedNode;
            Object obj = ObjectNode.Tag as Object;
            if (ObjectNode == null || obj == null || ObjectNode.Nodes.Count == 0)
            {
                return;
            }
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(obj.Name.Hash);
            foreach (TreeNode instanceNode in ObjectNode.Nodes)//deleting all instances under selected object and rebuilding them
            {
                Instance instance = instanceNode.Tag as Instance;
                if (groupRef != null && groupRef.HasMeshObject())
                {
                    Graphics.DeleteInstance(instance.RefID);//in case the object didnt have mesh before, so there are no duplicates
                    Graphics.DeleteInstance(groupRef, instance.RefID);//maybe add optionable bool to delete in rendermodel so it doesnt reload every instance here
                    for (int i = 0; i < groupRef.Children.Count; i++)
                    {
                        InstanceTranslokatorPart(Graphics.Assets, groupRef.Children[i], Matrix4x4.Identity, instance, true);
                    }
                }
                else
                {
                    Graphics.DeleteInstance(instance.RefID);
                    Graphics.InstanceGizmo.InstanceTranslokator(instance, Graphics.GetId3D11Device());
                }
            }
        }

        private void OnFrameRemoved(object sender, OnFrameRemovedArgs e)
        {
            Graphics.DeleteAsset(e.FrameRefID);
        }

        private void CopyXYZ_ButtonClick(object sender, EventArgs e)
        {
            decimal value1 = PositionXTool.Value;
            decimal value2 = PositionYTool.Value;
            decimal value3 = PositionZTool.Value;
            string result = $"{value1} {value2} {value3}";
            Clipboard.SetText(result);
        }

        private void ImportAIWorldXMLButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                openDialog.Title = "Select AIWorld XML file to import";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    TreeNode selectedNode = dSceneTree.SelectedNode;
                    AIWorld targetWorld = null;
                    if (selectedNode?.Tag is AIWorld world)
                    {
                        targetWorld = world;
                    }
                    else if (selectedNode?.Parent?.Tag is AIWorld parentWorld)
                    {
                        targetWorld = parentWorld;
                    }
                    else if (AIWorldRoot != null && AIWorldRoot.Nodes.Count > 0)
                    {
                        targetWorld = AIWorldRoot.Nodes[0].Tag as AIWorld;
                    }
                    if (targetWorld == null)
                    {
                        MessageBox.Show("Please select an AIWorld node to import into.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ImportAIWorldFromXML(openDialog.FileName, targetWorld);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("AIWorld imported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error importing AIWorld: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportAIWorldXMLButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode selectedNode = dSceneTree.SelectedNode;
                AIWorld targetWorld = null;
                if (selectedNode?.Tag is AIWorld world)
                {
                    targetWorld = world;
                }
                else if (selectedNode?.Parent?.Tag is AIWorld parentWorld)
                {
                    targetWorld = parentWorld;
                }
                if (targetWorld == null)
                {
                    MessageBox.Show("Please select an AIWorld node to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                saveDialog.Title = "Save AIWorld as XML";
                saveDialog.FileName = $"{targetWorld.PartName}_AIWorld.xml";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ExportAIWorldToXML(targetWorld, saveDialog.FileName);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show($"AIWorld exported to {saveDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error exporting AIWorld: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportAIWorldFromXML(string xmlFilePath, AIWorld targetWorld)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);
                targetWorld.AIPoints.Clear();
                XmlNode worldNode = xmlDoc.SelectSingleNode("AIWorld");
                if (worldNode != null)
                {
                    if (worldNode.SelectSingleNode("PartName") != null) targetWorld.PartName = worldNode.SelectSingleNode("PartName").InnerText;
                    if (worldNode.SelectSingleNode("KynogonString") != null) targetWorld.KynogonString = worldNode.SelectSingleNode("KynogonString").InnerText;
                    if (worldNode.SelectSingleNode("OriginStream") != null) targetWorld.OriginStream = worldNode.SelectSingleNode("OriginStream").InnerText;
                    XmlNodeList aiPointNodes = worldNode.SelectNodes("AIPoints/AIPoint");
                    foreach (XmlNode pointNode in aiPointNodes)
                    {
                        ushort typeID = ushort.Parse(pointNode.Attributes["TypeID"].Value);
                        IType newPoint = AIWorld_Factory.ConstructByTypeID(targetWorld, typeID);
                        if (newPoint != null)
                        {
                            switch (typeID)
                            {
                                case 1:
                                    var type1 = newPoint as AIWorld_Type1;
                                    if (type1 != null)
                                    {
                                        if (pointNode.SelectSingleNode("Unk01") != null) type1.Unk01 = byte.Parse(pointNode.SelectSingleNode("Unk01").InnerText);
                                        XmlNodeList childPoints = pointNode.SelectNodes("ChildPoints/ChildPoint");
                                        foreach (XmlNode childNode in childPoints)
                                        {
                                            ushort childTypeID = ushort.Parse(childNode.Attributes["TypeID"].Value);
                                            IType childPoint = AIWorld_Factory.ConstructByTypeID(targetWorld, childTypeID);
                                            if (childPoint != null)
                                            {
                                                LoadAIPointPropertiesByType(childPoint, childNode);
                                                type1.AIPoints.Add(childPoint);
                                            }
                                        }
                                    }
                                    break;
                                case 4:
                                    var type4 = newPoint as AIWorld_Type4;
                                    if (type4 != null)
                                    {
                                        LoadVector3Property(type4, "Position", pointNode);
                                        LoadVector3Property(type4, "Rotation", pointNode);
                                        LoadVector3Property(type4, "Direction", pointNode);
                                        if (pointNode.SelectSingleNode("Unk0") != null) type4.Unk0 = byte.Parse(pointNode.SelectSingleNode("Unk0").InnerText);
                                        if (pointNode.SelectSingleNode("ID") != null) type4.ID = uint.Parse(pointNode.SelectSingleNode("ID").InnerText);
                                        if (pointNode.SelectSingleNode("LinkID_Left") != null) type4.LinkID_Left = uint.Parse(pointNode.SelectSingleNode("LinkID_Left").InnerText);
                                        if (pointNode.SelectSingleNode("LinkID_Right") != null) type4.LinkID_Right = uint.Parse(pointNode.SelectSingleNode("LinkID_Right").InnerText);
                                        if (pointNode.SelectSingleNode("Length") != null) type4.Length = float.Parse(pointNode.SelectSingleNode("Length").InnerText, CultureInfo.InvariantCulture);
                                        if (pointNode.SelectSingleNode("Flags") != null) type4.Flags = byte.Parse(pointNode.SelectSingleNode("Flags").InnerText);
                                        if (pointNode.SelectSingleNode("Unk7") != null) type4.Unk7 = byte.Parse(pointNode.SelectSingleNode("Unk7").InnerText);
                                        if (pointNode.SelectSingleNode("Unk9") != null) type4.Unk9 = byte.Parse(pointNode.SelectSingleNode("Unk9").InnerText);

                                        //XmlNode unk8Node = pointNode.SelectSingleNode("Unk8");
                                        //if (unk8Node != null)
                                        //{
                                        //    XmlNodeList valueNodes = unk8Node.SelectNodes("Value");
                                        //    type4.Unk8 = new uint[valueNodes.Count];
                                        //    for (int i = 0; i < valueNodes.Count; i++)
                                        //    {
                                        //        type4.Unk8[i] = uint.Parse(valueNodes[i].InnerText);
                                        //    }
                                        //}
                                    }
                                    break;
                                case 7:
                                    var type7 = newPoint as AIWorld_Type7;
                                    if (type7 != null)
                                    {
                                        LoadVector3Property(type7, "Position", pointNode);
                                        LoadVector3Property(type7, "Direction", pointNode);
                                        LoadVector3Property(type7, "Unk2", pointNode);
                                        LoadVector3Property(type7, "Minimum", pointNode);
                                        LoadVector3Property(type7, "Maximum", pointNode);
                                        if (pointNode.SelectSingleNode("Unk0") != null) type7.Unk0 = ushort.Parse(pointNode.SelectSingleNode("Unk0").InnerText);
                                        if (pointNode.SelectSingleNode("Unk3") != null) type7.Unk3 = uint.Parse(pointNode.SelectSingleNode("Unk3").InnerText);
                                    }
                                    break;
                                case 8:
                                    var type8 = newPoint as AIWorld_Type8;
                                    if (type8 != null)
                                    {
                                        LoadType9Properties(type8, pointNode);
                                        if (pointNode.SelectSingleNode("Unk6") != null) type8.Unk6 = uint.Parse(pointNode.SelectSingleNode("Unk6").InnerText);
                                    }
                                    break;
                                case 9:
                                    var type9 = newPoint as AIWorld_Type9;
                                    if (type9 != null)
                                    {
                                        LoadType9Properties(type9, pointNode);
                                    }
                                    break;
                                case 11:
                                    var type11 = newPoint as AIWorld_Type11;
                                    if (type11 != null)
                                    {
                                        LoadVector3Property(type11, "Unk1", pointNode);
                                        LoadVector3Property(type11, "Unk2", pointNode);
                                        LoadVector3Property(type11, "Unk3", pointNode);
                                        if (pointNode.SelectSingleNode("Unk0") != null) type11.Unk0 = byte.Parse(pointNode.SelectSingleNode("Unk0").InnerText);
                                        if (pointNode.SelectSingleNode("Unk4") != null) type11.Unk4 = uint.Parse(pointNode.SelectSingleNode("Unk4").InnerText);
                                    }
                                    break;

                            }
                            targetWorld.AIPoints.Add(newPoint);
                        }
                    }
                }
                UpdateAIWorldTreeNode(targetWorld);
                targetWorld.RequestPrimitiveBatchUpdate();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to import AIWorld from XML: {ex.Message}", ex);
            }
        }

        private void LoadType9Properties(AIWorld_Type9 type9, XmlNode parentNode)
        {
            LoadVector3Property(type9, "Position", parentNode);
            if (parentNode.SelectSingleNode("Unk0") != null) type9.Unk0 = byte.Parse(parentNode.SelectSingleNode("Unk0").InnerText);
            if (parentNode.SelectSingleNode("Unk1") != null) type9.Unk1 = uint.Parse(parentNode.SelectSingleNode("Unk1").InnerText);
            if (parentNode.SelectSingleNode("Unk3") != null) type9.Unk3 = float.Parse(parentNode.SelectSingleNode("Unk3").InnerText, CultureInfo.InvariantCulture);
            if (parentNode.SelectSingleNode("Unk4") != null) type9.Unk4 = float.Parse(parentNode.SelectSingleNode("Unk4").InnerText, CultureInfo.InvariantCulture);
            XmlNode unk5Node = parentNode.SelectSingleNode("Unk5");
            if (unk5Node != null)
            {
                XmlNodeList valueNodes = unk5Node.SelectNodes("Value");
                type9.Unk5 = new uint[valueNodes.Count];
                for (int i = 0; i < valueNodes.Count; i++)
                {
                    type9.Unk5[i] = uint.Parse(valueNodes[i].InnerText);
                }
            }
        }

        private void LoadAIPointPropertiesByType(IType aiPoint, XmlNode parentNode)
        {
            if (aiPoint is AIWorld_Type4 type4)
            {
                LoadVector3Property(type4, "Direction", parentNode);
                if (parentNode.SelectSingleNode("Flag") != null) type4.Flags = byte.Parse(parentNode.SelectSingleNode("Flag").InnerText);
                if (parentNode.SelectSingleNode("ID") != null) type4.ID = uint.Parse(parentNode.SelectSingleNode("ID").InnerText);
                if (parentNode.SelectSingleNode("Length") != null) type4.Length = float.Parse(parentNode.SelectSingleNode("Length").InnerText, CultureInfo.InvariantCulture);
                if (parentNode.SelectSingleNode("LinkID_Left") != null) type4.LinkID_Left = uint.Parse(parentNode.SelectSingleNode("LinkID_Left").InnerText);
                if (parentNode.SelectSingleNode("LinkID_Right") != null) type4.LinkID_Right = uint.Parse(parentNode.SelectSingleNode("LinkID_Right").InnerText);
                LoadVector3Property(type4, "Position", parentNode);
                LoadVector3Property(type4, "Rotation", parentNode);
                if (parentNode.SelectSingleNode("Unk0") != null) type4.Unk0 = byte.Parse(parentNode.SelectSingleNode("Unk0").InnerText);
                if (parentNode.SelectSingleNode("Unk7") != null) type4.Unk7 = byte.Parse(parentNode.SelectSingleNode("Unk7").InnerText);
                //if (parentNode.SelectSingleNode("Unk0") != null) type4.Unk0 = byte.Parse(parentNode.SelectSingleNode("Unk0").InnerText);
                if (parentNode.SelectSingleNode("Unk9") != null) type4.Unk9 = uint.Parse(parentNode.SelectSingleNode("Unk9").InnerText);
            }
            else if (aiPoint is AIWorld_Type7 type7)
            {
                LoadVector3Property(type7, "Position", parentNode);
                LoadVector3Property(type7, "Direction", parentNode);
                LoadVector3Property(type7, "Minimum", parentNode);
                LoadVector3Property(type7, "Maximum", parentNode);
                if (parentNode.SelectSingleNode("Unk0") != null) type7.Unk0 = ushort.Parse(parentNode.SelectSingleNode("Unk0").InnerText);
            }
            else if (aiPoint is AIWorld_Type9 type9)
            {
                LoadVector3Property(type9, "Position", parentNode);
                if (parentNode.SelectSingleNode("Unk0") != null) type9.Unk0 = byte.Parse(parentNode.SelectSingleNode("Unk0").InnerText);
                if (parentNode.SelectSingleNode("Unk1") != null) type9.Unk1 = uint.Parse(parentNode.SelectSingleNode("Unk1").InnerText);
            }
            else if (aiPoint is AIWorld_Type11 type11)
            {
                LoadVector3Property(type11, "Unk1", parentNode);
                if (parentNode.SelectSingleNode("Unk0") != null) type11.Unk0 = byte.Parse(parentNode.SelectSingleNode("Unk0").InnerText);
            }
        }

        private void ExportAIWorldToXML(AIWorld world, string xmlFilePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(xmlDeclaration);
                XmlElement rootElement = xmlDoc.CreateElement("AIWorld");
                xmlDoc.AppendChild(rootElement);
                AddXmlElement(xmlDoc, rootElement, "PartName", world.PartName);
                AddXmlElement(xmlDoc, rootElement, "KynogonString", world.KynogonString);
                AddXmlElement(xmlDoc, rootElement, "OriginStream", world.OriginStream);
                XmlElement aiPointsElement = xmlDoc.CreateElement("AIPoints");
                rootElement.AppendChild(aiPointsElement);
                foreach (IType aiPoint in world.AIPoints)
                {
                    XmlElement pointElement = xmlDoc.CreateElement("AIPoint");
                    ushort typeID = AIWorld_Factory.GetIDByType(aiPoint);
                    pointElement.SetAttribute("TypeID", typeID.ToString());
                    switch (typeID)
                    {
                        case 1:
                            var type1 = aiPoint as AIWorld_Type1;
                            if (type1 != null)
                            {
                                AddXmlElement(xmlDoc, pointElement, "Unk01", type1.Unk01.ToString());
                                if (type1.AIPoints.Count > 0)
                                {
                                    XmlElement childPointsElement = xmlDoc.CreateElement("ChildPoints");
                                    pointElement.AppendChild(childPointsElement);
                                    foreach (IType childPoint in type1.AIPoints)
                                    {
                                        ushort childTypeID = AIWorld_Factory.GetIDByType(childPoint);
                                        XmlElement childElement = xmlDoc.CreateElement("ChildPoint");
                                        childElement.SetAttribute("TypeID", childTypeID.ToString());
                                        SaveAIPointPropertiesByType(childPoint, xmlDoc, childElement);
                                        childPointsElement.AppendChild(childElement);
                                    }
                                }
                            }
                            break;
                    }
                    aiPointsElement.AppendChild(pointElement);
                }
                using (XmlTextWriter writer = new XmlTextWriter(xmlFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    xmlDoc.Save(writer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to export AIWorld to XML: {ex.Message}", ex);
            }
        }

        private void AddXmlElement(XmlDocument xmlDoc, XmlElement parent, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlElement element = xmlDoc.CreateElement(name);
                element.InnerText = value;
                parent.AppendChild(element);
            }
        }

        private void AddVector3Element(XmlDocument xmlDoc, XmlElement parent, string name, Vector3 vector)
        {
            XmlElement vectorElement = xmlDoc.CreateElement(name);
            AddXmlElement(xmlDoc, vectorElement, "X", vector.X.ToString(CultureInfo.InvariantCulture));
            AddXmlElement(xmlDoc, vectorElement, "Y", vector.Y.ToString(CultureInfo.InvariantCulture));
            AddXmlElement(xmlDoc, vectorElement, "Z", vector.Z.ToString(CultureInfo.InvariantCulture));
            parent.AppendChild(vectorElement);
        }

        private void LoadVector3Property(object obj, string propertyName, XmlNode parentNode)
        {
            XmlNode vectorNode = parentNode.SelectSingleNode(propertyName);
            if (vectorNode != null)
            {
                try
                {
                    float x = float.Parse(vectorNode.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                    float y = float.Parse(vectorNode.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);
                    float z = float.Parse(vectorNode.SelectSingleNode("Z").InnerText, CultureInfo.InvariantCulture);
                    var property = obj.GetType().GetProperty(propertyName);
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(obj, new Vector3(x, y, z));
                    }
                }
                catch { }
            }
        }

        private void SaveAIPointPropertiesByType(IType aiPoint, XmlDocument xmlDoc, XmlElement parentElement)
        {
            ushort typeID = AIWorld_Factory.GetIDByType(aiPoint);
            switch (typeID)
            {
                case 4:
                    var type4 = aiPoint as AIWorld_Type4;
                    if (type4 != null)
                    {
                        AddVector3Element(xmlDoc, parentElement, "Direction", type4.Direction);
                        AddXmlElement(xmlDoc, parentElement, "Flag", type4.Flags.ToString());
                        AddXmlElement(xmlDoc, parentElement, "ID", type4.ID.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Length", type4.Length.ToString());
                        AddXmlElement(xmlDoc, parentElement, "LinkID_Left", type4.LinkID_Left.ToString());
                        AddXmlElement(xmlDoc, parentElement, "LinkID_3", type4.LinkID_Right.ToString());
                        AddVector3Element(xmlDoc, parentElement, "Position", type4.Position);
                        AddVector3Element(xmlDoc, parentElement, "Rotation", type4.Rotation);
                        AddXmlElement(xmlDoc, parentElement, "Unk0", type4.Unk0.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk7", type4.Unk7.ToString());
                        //AddXmlElement(xmlDoc, parentElement, "Unk8", type4.Unk8.ToString());        //Value UInt32 нужно будет потом считать                                                              
                        AddXmlElement(xmlDoc, parentElement, "Unk9", type4.Unk9.ToString());
                    }
                    break;
                case 7:
                    var type7 = aiPoint as AIWorld_Type7;
                    if (type7 != null)
                    {
                        AddVector3Element(xmlDoc, parentElement, "Maximum", type7.Maximum);
                        AddVector3Element(xmlDoc, parentElement, "Minimum", type7.Minimum);
                        AddVector3Element(xmlDoc, parentElement, "Direction", type7.Direction);
                        AddVector3Element(xmlDoc, parentElement, "Position", type7.Position);
                        AddXmlElement(xmlDoc, parentElement, "Unk0", type7.Unk0.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk2", type7.Unk2.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk3", type7.Unk3.ToString());
                    }
                    break;
                case 8:
                    var type8 = aiPoint as AIWorld_Type8;
                    if (type8 != null)
                    {
                        AddVector3Element(xmlDoc, parentElement, "Position", type8.Position);
                        AddXmlElement(xmlDoc, parentElement, "Unk0", type8.Unk0.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk1", type8.Unk1.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk3", type8.Unk3.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk4", type8.Unk4.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk5", type8.Unk5.ToString());      //Value UInt32 нужно будет потом считать
                        AddXmlElement(xmlDoc, parentElement, "Unk6", type8.Unk6.ToString());
                    }
                    break;
                case 9:
                    var type9 = aiPoint as AIWorld_Type9;
                    if (type9 != null)
                    {
                        AddVector3Element(xmlDoc, parentElement, "Position", type9.Position);
                        AddXmlElement(xmlDoc, parentElement, "Unk0", type9.Unk0.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk1", type9.Unk1.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk3", type9.Unk3.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk4", type9.Unk4.ToString());
                        AddXmlElement(xmlDoc, parentElement, "Unk5", type9.Unk5.ToString());      //Value UInt32 нужно будет потом считать
                    }
                    break;
                case 11:
                    var type11 = aiPoint as AIWorld_Type11;
                    if (type11 != null)
                    {
                        AddXmlElement(xmlDoc, parentElement, "Unk0", type11.Unk0.ToString());
                        AddVector3Element(xmlDoc, parentElement, "Unk1", type11.Unk1);
                        AddVector3Element(xmlDoc, parentElement, "Unk2", type11.Unk2);
                        AddVector3Element(xmlDoc, parentElement, "Unk3", type11.Unk3);
                        AddXmlElement(xmlDoc, parentElement, "Unk4", type11.Unk4.ToString());
                    }
                    break;
            }
        }

        private void UpdateAIWorldTreeNode(AIWorld world)
        {
            foreach (TreeNode rootNode in dSceneTree.TreeView.Nodes)
            {
                if (rootNode.Tag == world)
                {
                    rootNode.Nodes.Clear();
                    foreach (IType aiPoint in world.AIPoints)
                    {
                        TreeNode pointNode = aiPoint.PopulateTreeNode();
                        if (pointNode != null)
                        {
                            rootNode.Nodes.Add(pointNode);
                            if (aiPoint is AIWorld_Type1 group)
                            {
                                foreach (IType childPoint in group.AIPoints)
                                {
                                    TreeNode childNode = childPoint.PopulateTreeNode();
                                    pointNode.Nodes.Add(childNode);
                                }
                            }
                        }
                    }
                    rootNode.Expand();
                    break;
                }
            }
        }

        private void RotateAIGroupZButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode selectedNode = dSceneTree.SelectedNode;
                List<IType> pointsToRotate = new List<IType>();
                AIWorld targetWorld = null;
                string groupName = "";
                Vector3 centerPoint = Vector3.Zero;
                if (selectedNode?.Tag is AIWorld world)
                {
                    targetWorld = world;
                    pointsToRotate.AddRange(world.AIPoints);
                    groupName = $"AIWorld '{world.PartName}'";
                    centerPoint = CalculateCenterPoint(world.AIPoints);
                }
                else if (selectedNode?.Tag is AIWorld_Type1 group)
                {
                    targetWorld = group.World;
                    pointsToRotate.AddRange(group.AIPoints);
                    groupName = $"Group '{selectedNode.Text}'";
                    centerPoint = CalculateCenterPoint(group.AIPoints);
                }
                else if (selectedNode?.Parent?.Tag is AIWorld worldFromParent)
                {
                    targetWorld = worldFromParent;
                    pointsToRotate.AddRange(worldFromParent.AIPoints);
                    groupName = $"AIWorld '{worldFromParent.PartName}'";
                    centerPoint = CalculateCenterPoint(worldFromParent.AIPoints);
                }
                else
                {
                    MessageBox.Show("Please select an AIWorld or AIWorld_Type1 group to rotate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (pointsToRotate.Count == 0)
                {
                    MessageBox.Show("No AI points to rotate.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                using (RotateAIGroupForm rotateForm = new RotateAIGroupForm(groupName, pointsToRotate.Count, centerPoint))
                {
                    if (rotateForm.ShowDialog() == DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        float angleDegrees = rotateForm.AngleDegrees;
                        Vector3 rotationCenter = rotateForm.RotationCenter;
                        bool rotateChildren = rotateForm.RotateChildren;
                        bool updateRotations = rotateForm.UpdateRotations;
                        RotateAIGroupZ(pointsToRotate, angleDegrees, rotationCenter, rotateChildren, updateRotations);
                        UpdateAIWorldTreeNode(targetWorld);
                        targetWorld.RequestPrimitiveBatchUpdate();
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show($"Rotated {pointsToRotate.Count} AI points by {angleDegrees}° around ({rotationCenter.X}, {rotationCenter.Y}, {rotationCenter.Z})", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error rotating AI group: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Vector3 CalculateCenterPoint(List<IType> aiPoints)
        {
            if (aiPoints.Count == 0) return Vector3.Zero;
            Vector3 sum = Vector3.Zero;
            int count = 0;
            foreach (IType point in aiPoints)
            {
                Vector3 position = GetAIPointPosition(point);
                if (position != Vector3.Zero)
                {
                    sum += position;
                    count++;
                }
            }
            return count > 0 ? sum / count : Vector3.Zero;
        }

        private Vector3 GetAIPointPosition(IType aiPoint)
        {
            if (aiPoint is AIWorld_Type9 type9) return type9.Position;
            else if (aiPoint is AIWorld_Type4 type4) return type4.Position;
            else if (aiPoint is AIWorld_Type7 type7) return type7.Position;
            else if (aiPoint is AIWorld_Type11 type11) return type11.Unk1;
            else if (aiPoint is AIWorld_Type1 type1) return CalculateCenterPoint(type1.AIPoints);
            return Vector3.Zero;
        }

        private void RotateAIGroupZ(List<IType> pointsToRotate, float angleDegrees, Vector3 rotationCenter, bool rotateChildren = true, bool updateRotations = true)
        {
            foreach (IType point in pointsToRotate)
            {
                RotateAIPointZ(point, angleDegrees, rotationCenter, rotateChildren, updateRotations);
            }
        }

        private void RotateAIPointZ(IType aiPoint, float angleDegrees, Vector3 rotationCenter, bool rotateChildren, bool updateRotations)
        {
            float angleRadians = angleDegrees * (MathF.PI / 180f);
            RotatePointAroundZ(aiPoint, angleRadians, rotationCenter, updateRotations);
            if (aiPoint is AIWorld_Type1 type1 && rotateChildren)
            {
                foreach (IType childPoint in type1.AIPoints)
                {
                    RotateAIPointZ(childPoint, angleDegrees, rotationCenter, rotateChildren, updateRotations);
                }
            }
        }

        private void RotatePointAroundZ(IType aiPoint, float angleRadians, Vector3 rotationCenter, bool updateRotations)
        {
            float cosA = MathF.Cos(angleRadians);
            float sinA = MathF.Sin(angleRadians);
            if (aiPoint is AIWorld_Type9 type9)
            {
                Vector3 relativePos = type9.Position - rotationCenter;
                float newX = relativePos.X * cosA - relativePos.Y * sinA;
                float newY = relativePos.X * sinA + relativePos.Y * cosA;
                type9.Position = new Vector3(newX, newY, relativePos.Z) + rotationCenter;
            }
            else if (aiPoint is AIWorld_Type4 type4)
            {
                Vector3 relativePos = type4.Position - rotationCenter;
                float newX = relativePos.X * cosA - relativePos.Y * sinA;
                float newY = relativePos.X * sinA + relativePos.Y * cosA;
                type4.Position = new Vector3(newX, newY, relativePos.Z) + rotationCenter;
                if (updateRotations)
                {
                    type4.Rotation = new Vector3(type4.Rotation.X, type4.Rotation.Y, type4.Rotation.Z + (angleRadians * (180f / MathF.PI)));
                    Vector3 dir = type4.Direction;
                    float newDirX = dir.X * cosA - dir.Y * sinA;
                    float newDirY = dir.X * sinA + dir.Y * cosA;
                    type4.Direction = new Vector3(newDirX, newDirY, dir.Z);
                }
            }
            else if (aiPoint is AIWorld_Type7 type7)
            {
                Vector3 relativePos = type7.Position - rotationCenter;
                float newX = relativePos.X * cosA - relativePos.Y * sinA;
                float newY = relativePos.X * sinA + relativePos.Y * cosA;
                type7.Position = new Vector3(newX, newY, relativePos.Z) + rotationCenter;
                if (updateRotations)
                {
                    Vector3 dir = type7.Direction;
                    float newDirX = dir.X * cosA - dir.Y * sinA;
                    float newDirY = dir.X * sinA + dir.Y * cosA;
                    type7.Direction = new Vector3(newDirX, newDirY, dir.Z);
                    Vector3 boxCenter = (type7.Minimum + type7.Maximum) * 0.5f;
                    Vector3 relativeBoxCenter = boxCenter - rotationCenter;
                    float newBoxCenterX = relativeBoxCenter.X * cosA - relativeBoxCenter.Y * sinA;
                    float newBoxCenterY = relativeBoxCenter.X * sinA + relativeBoxCenter.Y * cosA;
                    Vector3 newBoxCenter = new Vector3(newBoxCenterX, newBoxCenterY, relativeBoxCenter.Z) + rotationCenter;
                    Vector3 boxSize = type7.Maximum - type7.Minimum;
                    type7.Minimum = newBoxCenter - (boxSize * 0.5f);
                    type7.Maximum = newBoxCenter + (boxSize * 0.5f);
                }
            }
            else if (aiPoint is AIWorld_Type11 type11)
            {
                type11.Unk1 = RotateVectorAroundZ(type11.Unk1, angleRadians, rotationCenter);
                type11.Unk2 = RotateVectorAroundZ(type11.Unk2, angleRadians, rotationCenter);
                type11.Unk3 = RotateVectorAroundZ(type11.Unk3, angleRadians, rotationCenter);
            }
            ApplyRotationToVectorProperties(aiPoint, angleRadians, rotationCenter, updateRotations);
        }

        private Vector3 RotateVectorAroundZ(Vector3 vector, float angleRadians, Vector3 rotationCenter)
        {
            Vector3 relativeVec = vector - rotationCenter;
            float cosA = MathF.Cos(angleRadians);
            float sinA = MathF.Sin(angleRadians);
            float newX = relativeVec.X * cosA - relativeVec.Y * sinA;
            float newY = relativeVec.X * sinA + relativeVec.Y * cosA;
            return new Vector3(newX, newY, relativeVec.Z) + rotationCenter;
        }

        private void ApplyRotationToVectorProperties(IType aiPoint, float angleRadians, Vector3 rotationCenter, bool updateRotations)
        {
            var properties = aiPoint.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(Vector3) && property.CanRead && property.CanWrite)
                {
                    string propName = property.Name;
                    if (propName == "Position" || propName == "Rotation" || propName == "Direction" || propName == "Minimum" || propName == "Maximum" || propName == "Unk1" || propName == "Unk2" || propName == "Unk3") continue;
                    try
                    {
                        Vector3 currentValue = (Vector3)property.GetValue(aiPoint);
                        Vector3 newValue = RotateVectorAroundZ(currentValue, angleRadians, rotationCenter);
                        property.SetValue(aiPoint, newValue);
                    }
                    catch { }
                }
            }
        }

        private void MoveAIGroupButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode selectedNode = dSceneTree.SelectedNode;
                List<IType> pointsToMove = new List<IType>();
                AIWorld targetWorld = null;
                string groupName = "";
                if (selectedNode?.Tag is AIWorld world)
                {
                    targetWorld = world;
                    pointsToMove.AddRange(world.AIPoints);
                    groupName = $"AIWorld '{world.PartName}'";
                }
                else if (selectedNode?.Tag is AIWorld_Type1 group)
                {
                    targetWorld = group.World;
                    pointsToMove.AddRange(group.AIPoints);
                    groupName = $"Group '{selectedNode.Text}'";
                }
                else if (selectedNode?.Parent?.Tag is AIWorld worldFromParent)
                {
                    targetWorld = worldFromParent;
                    pointsToMove.AddRange(worldFromParent.AIPoints);
                    groupName = $"AIWorld '{worldFromParent.PartName}'";
                }
                else if (selectedNode?.Parent?.Parent?.Tag is AIWorld worldFromGrandParent)
                {
                    targetWorld = worldFromGrandParent;
                    pointsToMove.AddRange(worldFromGrandParent.AIPoints);
                    groupName = $"AIWorld '{worldFromGrandParent.PartName}'";
                }
                else
                {
                    MessageBox.Show("Please select an AIWorld or AIWorld_Type1 group to move.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (pointsToMove.Count == 0)
                {
                    MessageBox.Show("No AI points to move.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                MoveAIGroupForm moveForm = new MoveAIGroupForm(groupName, pointsToMove.Count);
                if (moveForm.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Vector3 offset = moveForm.GetOffset();
                    bool moveChildren = moveForm.MoveChildren;
                    MoveAIGroup(pointsToMove, offset, moveChildren);
                    UpdateAIWorldTreeNode(targetWorld);
                    targetWorld.RequestPrimitiveBatchUpdate();
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show($"Moved {pointsToMove.Count} AI points by ({offset.X}, {offset.Y}, {offset.Z})", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                moveForm.Dispose();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error moving AI group: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MoveAIGroup(List<IType> pointsToMove, Vector3 offset, bool moveChildren = true)
        {
            foreach (IType point in pointsToMove)
            {
                MoveAIPoint(point, offset, moveChildren);
            }
        }

        private void MoveAIPoint(IType aiPoint, Vector3 offset, bool moveChildren = true)
        {
            if (aiPoint is AIWorld_Type9 type9)
            {
                type9.Position += offset;
            }
            else if (aiPoint is AIWorld_Type4 type4)
            {
                type4.Position += offset;
            }
            else if (aiPoint is AIWorld_Type7 type7)
            {
                type7.Position += offset;
            }
            else if (aiPoint is AIWorld_Type11 type11)
            {
                type11.Unk1 += offset;
                type11.Unk2 += offset;
                type11.Unk3 += offset;
            }
            else if (aiPoint is AIWorld_Type1 type1 && moveChildren)
            {
                foreach (IType childPoint in type1.AIPoints)
                {
                    MoveAIPoint(childPoint, offset, moveChildren);
                }
            }
        }

        private void PasteXYZ_ButtonClick(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText()) return;
            string text = Clipboard.GetText().Trim();
            string[] parts = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                try
                {
                    decimal x = decimal.Parse(parts[0], CultureInfo.InvariantCulture);
                    decimal y = decimal.Parse(parts[1], CultureInfo.InvariantCulture);
                    decimal z = decimal.Parse(parts[2], CultureInfo.InvariantCulture);
                    PositionXTool.Value = x;
                    PositionYTool.Value = y;
                    PositionZTool.Value = z;
                }
                catch { }
            }
            else { }
        }

        private void DeleteActorEntry(ActorEntry actorEntry)
        {
            if (actorEntry == null)
                return;

            Actor owningActor = null;
            foreach (var actor in SceneData.Actors)
            {
                if (actor.Items.Contains(actorEntry))
                {
                    owningActor = actor;
                    break;
                }
            }

            if (owningActor == null)
            {
                ToolkitAssert.Ensure(false, "Could not find owning actor file for ActorEntry.");
                return;
            }

            owningActor.Items.Remove(actorEntry);

            var defsToRemove = owningActor.Definitions
                .Where(d => d.FrameNameHash == actorEntry.FrameNameHash)
                .ToList();
            foreach (var def in defsToRemove)
                owningActor.Definitions.Remove(def);

            if (actorEntry.FrameNameHash != 0)
            {
                var frame = SceneData.FrameResource.GetObjectByHash<FrameObjectFrame>(actorEntry.FrameNameHash);
                if (frame != null && frame.Item == actorEntry)
                {
                    frame.Item = null;
                }
            }

            TreeNode actorNode = null;
            TreeNode[] foundNodes = dSceneTree.TreeView.Nodes.Find("actor_" + actorEntry.EntityName, true);
            if (foundNodes.Length > 0)
            {
                actorNode = foundNodes[0];
                foreach (TreeNode child in actorNode.Nodes)
                {
                    if (int.TryParse(child.Name, out int refID))
                    {
                        Graphics.DeleteAsset(refID);
                        if (RefIDToActorEntry.ContainsKey(refID))
                            RefIDToActorEntry.Remove(refID);
                    }
                }
                dSceneTree.RemoveNode(actorNode);
            }

            var keysToRemove = RefIDToActorEntry.Where(kvp => kvp.Value == actorEntry).Select(kvp => kvp.Key).ToList();
            foreach (var key in keysToRemove)
            {
                RefIDToActorEntry.Remove(key);
            }
        } 
    }
}