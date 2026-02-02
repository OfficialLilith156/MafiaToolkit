using Rendering.Core;
using Rendering.Input;
using ResourceTypes.FrameResource;
using ResourceTypes.Translokator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolkit.Core;
using Utils.Models;
using Utils.Settings;
using Utils.VorticeUtils;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class UpdateSelectedEventArgs : EventArgs
    {
        public int RefID { get; set; }
    }

    public struct PickOutParams
    {
        public int LowestRefID { get; set; }
        public int LowestInstanceID { get; set; }
        public Vector3 WorldPosition { get; set; }
    }

    public class GraphicsClass
    {
        public InputClass Input { get; private set; }
        public WorldSettings WorldSettings { get; set; }
        public Camera Camera { get; set; }
        public Dictionary<int, IRenderer> InitObjectStack { get; set; }
        public Profiler Profile { get; set; }

        public EventHandler<UpdateSelectedEventArgs> OnSelectedObjectUpdated;

        public Dictionary<int, IRenderer> Assets { get; private set; }
        private int selectedID;
        private Dictionary<int, int> selectedInstances;//refframe refid, instance refid
        private RenderBoundingBox selectionBox;
        private RenderModel sky;
        private RenderModel clouds;
        private GizmoTool TranslationGizmo;
        public InstanceGizmo InstanceGizmo;

        private DirectX11Class D3D;

        private TranslokatorSpatialGrid translokatorGrid;
        private SpatialGrid[] navigationGrids;

        // Local batches for objects passed through
        private PrimitiveBatch LineBatch = null;
        private PrimitiveBatch BBoxBatch = null;
        private int NumBVHToBuild = 0;
        private int NumBVHBuilt = 0;
        private List<Task> BVHBuildingTasks = new();
        public PrimitiveManager OurPrimitiveManager { get; private set; }


        public GraphicsClass()
        {
            InitObjectStack = new Dictionary<int, IRenderer>();
            Profile = new Profiler();
            Assets = new Dictionary<int, IRenderer>();
            selectionBox = new RenderBoundingBox();
            translokatorGrid = new TranslokatorSpatialGrid();
            navigationGrids = new SpatialGrid[0];
            OurPrimitiveManager = new PrimitiveManager();

            OnSelectedObjectUpdated += OnSelectedObjectHasUpdated;

            // Create bespoke batches for any lines or boxes passed in via the construct stack
            string LineBatchID = string.Format("Graphics_LineBatcher_{0}", RefManager.GetNewRefID());
            LineBatch = new PrimitiveBatch(PrimitiveType.Line, LineBatchID);

            string BBoxBatchID = string.Format("Graphics_BBoxBatcher_{0}", RefManager.GetNewRefID());
            BBoxBatch = new PrimitiveBatch(PrimitiveType.Box, BBoxBatchID);

            OurPrimitiveManager.AddPrimitiveBatch(LineBatch);
            OurPrimitiveManager.AddPrimitiveBatch(BBoxBatch);
        }

        public bool PreInit(IntPtr WindowHandle)
        {
            D3D = new DirectX11Class();
            if (!D3D.Init(WindowHandle))
            {
                MessageBox.Show("Failed to initialize DirectX11!");
            }
            Profile.Init();
            if(!RenderStorageSingleton.Instance.IsInitialised())
            {
                bool result = RenderStorageSingleton.Instance.Initialise(D3D);
                var structure = new M2TStructure();
                //import gizmo
                RenderModel gizmo = new RenderModel();
                structure.ReadFromM2T("Resources/GizmoModel.m2t");
                gizmo.ConvertMTKToRenderModel(structure);
                gizmo.InitBuffers(D3D.Device, D3D.DeviceContext);
                gizmo.DoRender = true;
                TranslationGizmo = new GizmoTool(gizmo);

                sky = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/sky_backdrop.m2t");
                sky.ConvertMTKToRenderModel(structure);
                sky.InitBuffers(D3D.Device, D3D.DeviceContext);

                clouds = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/weather_clouds.m2t");
                clouds.ConvertMTKToRenderModel(structure);
                clouds.InitBuffers(D3D.Device, D3D.DeviceContext);

                RenderModel instancePlaceholder = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/Translokator.m2t");
                instancePlaceholder.ConvertMTKToRenderModel(structure);
                instancePlaceholder.InitBuffers(D3D.Device,D3D.DeviceContext);
                InstanceGizmo = new InstanceGizmo(instancePlaceholder);
            }

            selectionBox.SetColour(System.Drawing.Color.Red);
            selectionBox.Init(new BoundingBox(new Vector3(0.5f), new Vector3(-0.5f)));          
            selectionBox.DoRender = false;
            return true;
        }

        public bool InitScene(int width, int height)
        {
            WorldSettings = new WorldSettings();
            WorldSettings.SetupLighting();
            Camera = new Camera();
            Camera.Position = new Vector3(0.0f, 0.0f, 15.0f);
            Camera.SetProjectionMatrix(width, height);
            ClearRenderStack();
            selectionBox.InitBuffers(D3D.Device, D3D.DeviceContext);
            TranslationGizmo.InitBuffers(D3D.Device, D3D.DeviceContext);
            sky.InitBuffers(D3D.Device, D3D.DeviceContext);
            sky.DoRender = WorldSettings.RenderSky;
            clouds.InitBuffers(D3D.Device, D3D.DeviceContext);
            clouds.DoRender = WorldSettings.RenderClouds;
            InstanceGizmo.InitBuffers(D3D.Device, D3D.DeviceContext);
            InstanceGizmo.InstanceModel.GetBVHBuildingTask(); // Maybe this function should be added to the IRenderer class instead? probably
            
            Input = new InputClass();
            Input.Init();
            return true;
        }

        public void BuildTranslokatorGrid(TranslokatorLoader translokator)
        {
            translokatorGrid.Build(translokator);
            translokatorGrid.Initialise(D3D.Device, D3D.DeviceContext);
        }

        public TreeNode SetNavigationGrid(ResourceTypes.Navigation.OBJData[] data)
        {
            TreeNode[] Grids = new TreeNode[data.Length];
            navigationGrids = new SpatialGrid[data.Length];

            for(int i = 0; i < navigationGrids.Length; i++)
            {
                navigationGrids[i] = new SpatialGrid(this, data[i].runtimeMesh);
                navigationGrids[i].Initialise(D3D.Device, D3D.DeviceContext);
                Grids[i] = navigationGrids[i].GetTreeNodes();
                Grids[i].Text = string.Format("Grid: {0}", i);
                Grids[i].Tag = data[i].runtimeMesh;
            }

            TreeNode Parent = new TreeNode("Navigation Grids");
            Parent.Nodes.AddRange(Grids);
            return Parent;
        }

        public PickOutParams Pick(int sx, int sy, int Width, int Height)
        {
            float lowest = float.MaxValue;
            int lowestRefID = -1;
            int lowestInstanceID = -1;
            Vector3 WorldPosIntersect = Vector3.Zero;

            Ray ray = Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(Width, Height));

            int index = 0;
            foreach (KeyValuePair<int, IRenderer> model in Assets)
            {
                if (!model.Value.DoRender)
                {
                    continue;
                }

                Matrix4x4 vWM = Matrix4x4.Identity;
                Matrix4x4.Invert(model.Value.Transform, out vWM);
                var localRay = new Ray(
                    Vector3Utils.TransformCoordinate(ray.Position, vWM),
                    Vector3.TransformNormal(ray.Direction, vWM)
                );

                if (model.Value is RenderModel mesh)
                {
                    var bbox = mesh.BoundingBox;

                    if (mesh.InstanceTransforms.Count > 0)
                    {
                        // We cannot use the per triangle picking method on instances as
                        // it can take several minutes to complete even on good hardware.
                        // We just have to deal with the potential picking ray miss.
                        if (!mesh.BVH.FinishedBuilding)
                        {
                            continue;
                        }

                        foreach (var transform in mesh.InstanceTransforms)
                        {
                            var transposed = Matrix4x4.Transpose(transform.Value);

                            Matrix4x4 tvWM = Matrix4x4.Identity;
                            Matrix4x4.Invert(transposed, out tvWM);
                            var localInstanceRay = new Ray(
                                Vector3Utils.TransformCoordinate(ray.Position, tvWM),
                                Vector3.TransformNormal(ray.Direction, tvWM)
                            );

                            if (localInstanceRay.Intersects(bbox) == 0.0f) continue;

                            var bvhInstanceIntersect = mesh.BVH.Intersect(ray, localInstanceRay);

                            if (bvhInstanceIntersect.distance < lowest)
                            {
                                lowest = bvhInstanceIntersect.distance;
                                lowestRefID = model.Key;
                                lowestInstanceID = transform.Key;
                                WorldPosIntersect = bvhInstanceIntersect.pos;
                            }
                        }
                        
                    }

                    if (localRay.Intersects(bbox) == 0.0f) continue; // Pick doesn't seem to work when the camera is inside the bounding volume

                    if (!mesh.BVH.FinishedBuilding)
                    {
                        var triangleIntersect = PerTriangleRayIntersect(ray, localRay, mesh);

                        if (triangleIntersect.distance < lowest)
                        {
                            lowest = triangleIntersect.distance;
                            lowestRefID = model.Key;
                            lowestInstanceID = -1;
                            WorldPosIntersect = triangleIntersect.pos;
                        }

                        continue;
                    }

                    var bvhIntersect = mesh.BVH.Intersect(ray, localRay);

                    if (bvhIntersect.distance < lowest)
                    {
                        lowest = bvhIntersect.distance;
                        lowestRefID = model.Key;
                        lowestInstanceID = -1;
                        WorldPosIntersect = bvhIntersect.pos;
                    }
                }
                if (model.Value is RenderInstance instance)
                {
                    RenderStaticCollision collision = instance.GetCollision();
                    var bbox = collision.BoundingBox;

                    if (localRay.Intersects(bbox) == 0.0f) continue;

                    for (var i = 0; i < collision.Indices.Length / 3; i++)
                    {
                        var v0 = collision.Vertices[collision.Indices[i * 3]].Position;
                        var v1 = collision.Vertices[collision.Indices[i * 3 + 1]].Position;
                        var v2 = collision.Vertices[collision.Indices[i * 3 + 2]].Position;
                        float t;

                        if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out t)) continue;

                        if (t < 0.0f || float.IsNaN(t))
                        {
                            //if (SceneData.FrameResource.FrameObjects.ContainsKey(model.Key))
                            //{
                            //    var frame = (SceneData.FrameResource.FrameObjects[model.Key] as FrameObjectBase);
                            //    Utils.Logging.Log.WriteLine(string.Format("The toolkit has failed to analyse a model: {0} {1}", frame.Name, t));
                            //}
                        }

                        var worldPosition = ray.Position + t * ray.Direction;
                        var distance = (worldPosition - ray.Position).LengthSquared();

                        if (distance < lowest)
                        {
                            lowest = distance;
                            lowestRefID = model.Key;
                            lowestInstanceID = -1;
                            WorldPosIntersect = worldPosition;
                        }
                    }
                }

                index++;
            }
            
            foreach (var transform in InstanceGizmo.InstanceModel.InstanceTransforms)
            {
                var transposed = Matrix4x4.Transpose(transform.Value);

                Matrix4x4 tvWM = Matrix4x4.Identity;
                Matrix4x4.Invert(transposed, out tvWM);
                var localInstanceRay = new Ray(
                    Vector3Utils.TransformCoordinate(ray.Position, tvWM),
                    Vector3.TransformNormal(ray.Direction, tvWM)
                );

                if (localInstanceRay.Intersects(InstanceGizmo.InstanceModel.BoundingBox) == 0.0f) continue;

                var bvhInstanceIntersect = InstanceGizmo.InstanceModel.BVH.Intersect(ray, localInstanceRay);

                if (bvhInstanceIntersect.distance < lowest)
                {
                    lowest = bvhInstanceIntersect.distance;
                    lowestRefID = -2;
                    lowestInstanceID = transform.Key;
                    WorldPosIntersect = bvhInstanceIntersect.pos;
                }
            }
            

            PickOutParams OutputParams = new PickOutParams();
            OutputParams.LowestRefID = lowestRefID;
            OutputParams.LowestInstanceID = lowestInstanceID;
            OutputParams.WorldPosition = WorldPosIntersect;

            return OutputParams;
        }

        private (float distance, Vector3 pos) PerTriangleRayIntersect(Ray ray, Ray localRay, RenderModel mesh)
        {
            (float distance, Vector3 pos) val = (float.MaxValue, Vector3.Zero);

            for (var i = 0; i < mesh.LODs[0].Indices.Length / 3; i++)
            {
                var v0 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3]].Position;
                var v1 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3 + 1]].Position;
                var v2 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3 + 2]].Position;
                float t;

                if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out t)) continue;

                var worldPosition = ray.Position + t * ray.Direction;
                var distance = (worldPosition - ray.Position).LengthSquared();

                if (distance < val.distance)
                {
                    val.distance = distance;
                    val.pos = worldPosition;
                }
            }

            return val;
        }

        public void Frame()
        {
            ClearRenderStack();
            Render();
            Profile.Update();
        }

        public bool UpdateInput()
        {
            bool bCameraUpdated = false;
            float Multiplier = ToolkitSettings.CameraSpeed;

            if (Input.IsKeyDown(Keys.ShiftKey))
            {
                Multiplier *= 2.0f;
            }

            float speed = Profile.DeltaTime * Multiplier;

            if (Input.IsKeyDown(Keys.A))
            {
                Camera.Position -= Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(0), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.D))
            {
                Camera.Position += Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(0), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.W))
            {
                Camera.Position -= Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(2), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.S))
            {
                Camera.Position += Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(2), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.Q))
            {
                Camera.Position.Z += speed;
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.E))
            {
                Camera.Position.Z -= speed;
                bCameraUpdated = true;
            }

            return bCameraUpdated;
        }

        public bool Render()
        {
            if (NumBVHBuilt < NumBVHToBuild)
            {
                UpdateBVHQueue();
            }

            D3D.BeginScene(0.0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach (BaseShader Shader in RenderStorageSingleton.Instance.ShaderManager.shaders.Values)
            {
                Shader.InitCBuffersFrame(D3D.DeviceContext, Camera, WorldSettings);
            }

            foreach (IRenderer RenderEntry in Assets.Values)
            {
                RenderEntry.UpdateBuffers(D3D.Device, D3D.DeviceContext);
                RenderEntry.Render(D3D.Device, D3D.DeviceContext, Camera);    
            }
            
            //navigationGrids[0].Render(D3D.Device, D3D.DeviceContext, Camera);
            foreach (var grid in navigationGrids)
            {
                grid.Render(D3D.Device, D3D.DeviceContext, Camera);
            }

            OurPrimitiveManager.RenderPrimitives(D3D.Device, D3D.DeviceContext, Camera);

            translokatorGrid.Render(D3D.Device, D3D.DeviceContext, Camera);
            selectionBox.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            selectionBox.Render(D3D.Device, D3D.DeviceContext, Camera);
            TranslationGizmo.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            TranslationGizmo.Render(D3D.Device, D3D.DeviceContext, Camera);
            clouds.DoRender = WorldSettings.RenderClouds;
            clouds.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            clouds.Render(D3D.Device, D3D.DeviceContext, Camera);
            sky.DoRender = WorldSettings.RenderSky;
            sky.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            sky.Render(D3D.Device, D3D.DeviceContext, Camera);
            InstanceGizmo.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            InstanceGizmo.Render(D3D.Device, D3D.DeviceContext, Camera);
            

            D3D.EndScene();
            return true;
        }

        private void ClearRenderStack()
        {
            foreach (KeyValuePair<int, IRenderer> asset in InitObjectStack)
            {
                asset.Value.InitBuffers(D3D.Device, D3D.DeviceContext);

                if (asset.Value is RenderBoundingBox)
                {
                    BBoxBatch.AddObject(asset.Key, asset.Value);
                }
                else if (asset.Value is RenderLine)
                {
                    LineBatch.AddObject(asset.Key, asset.Value);
                }
                else if (asset.Value is RenderModel)
                {
                    BVHBuildingTasks.Add(((RenderModel)asset.Value).GetBVHBuildingTask());
                    NumBVHToBuild++;

                    Assets.Add(asset.Key, asset.Value);
                }
                else
                {
                    Assets.Add(asset.Key, asset.Value);
                }
            }

            InitObjectStack.Clear();
        }

        private void UpdateBVHQueue()
        {
            // Clear completed BVH tasks
            NumBVHBuilt += BVHBuildingTasks.RemoveAll(t => t.IsCompleted);
        }

        public void SelectEntry(int id)
        {
            IRenderer NewObject = GetAsset(id);
            IRenderer OldObject = GetAsset(selectedID);

            if (selectedID == id)
            {
                return;
            }

            if (NewObject != null)
            {
                if (OldObject != null)
                {
                    OldObject.Unselect();
                }

                if (selectedInstances != null)
                {
                    foreach (var selinst in selectedInstances)
                    {
                        RenderModel model = Assets[selinst.Key] as RenderModel;
                        model.UnselectInstance();
                    }
                    selectedInstances.Clear();
                }
                InstanceGizmo.Unselect();

                TranslationGizmo.OnSelectEntry(NewObject.Transform, true);
                NewObject.Select();
                selectionBox.DoRender = true;
                selectionBox.SetTransform(NewObject.Transform);
                selectionBox.Update(NewObject.BoundingBox);
                selectedID = id;
            }
        }
        
        public void SelectInstance(int instanceId)
        {
            IRenderer SelectedEntry = GetAsset(selectedID);
            if (SelectedEntry != null)
            {
                SelectedEntry.Unselect();
            }

            if (selectedInstances != null)
            {
                foreach (var selinst in selectedInstances)
                {
                    RenderModel model = Assets[selinst.Key] as RenderModel;
                    model.UnselectInstance();
                }
                selectedInstances.Clear();
            }
            InstanceGizmo.Unselect();

            selectedInstances = new Dictionary<int, int>();
            
            foreach (var asset in Assets)
            {
                if (asset.Value is RenderModel model && model.ContainsInstanceTransform(instanceId))
                {
                    selectedInstances.Add(asset.Key, instanceId);
                    model.SelectInstance(instanceId);
                }

            }

            if (selectedInstances.Count > 0)
            {
                RenderModel model = Assets[selectedInstances.First().Key] as RenderModel;
                TranslationGizmo.OnSelectEntry(Matrix4x4.Transpose(model.InstanceTransforms[selectedInstances.First().Value]) , true);
            }
            else
            {
                InstanceGizmo.Select(instanceId);
                TranslationGizmo.OnSelectEntry(Matrix4x4.Transpose(InstanceGizmo.InstanceModel.InstanceTransforms[instanceId]) , true);
            }
        }

        public IRenderer GetAsset(int RefID)
        {
            if (Assets.ContainsKey(RefID))
            {
                return Assets[RefID];
            }

            IRenderer ObjectInPrimitive = OurPrimitiveManager.GetObject(RefID);
            if(ObjectInPrimitive != null)
            {
                return ObjectInPrimitive;
            }

            return OurPrimitiveManager.GetObject(RefID);
        }

        public bool DeleteAsset(int RefID)
        {
            if (Assets.ContainsKey(RefID))
            {
                // ensure that dx11 related objects are properly destroyed
                IRenderer FoundAsset = Assets[RefID];
                FoundAsset.Shutdown();

                return Assets.Remove(RefID);
            }

            // TODO: The owner if a 'PrimitiveBatch' is pretty ambiguous right now.
            return OurPrimitiveManager.RemoveObject(RefID);
        }

        public void SetAssetVisibility(int RefID, bool bVisibility)
        {
            IRenderer ObjectAsset = GetAsset(RefID);
            if (ObjectAsset != null)
            {
                ObjectAsset.DoRender = bVisibility;
            }
        }

        public void MoveGizmo(int sx, int sy, int Width, int Height)
        {
            TranslationGizmo.ManipulateGizmo(Camera, sx, sy, Width, Height);
        }

        // Gizmo mode management
        public void SetGizmoMode(GizmoMode mode)
        {
            TranslationGizmo.SetMode(mode);
        }

        public GizmoMode GetGizmoMode()
        {
            return TranslationGizmo.GetMode();
        }

        // Gizmo state queries
        public bool IsGizmoActive()
        {
            return TranslationGizmo.IsActive();
        }

        public GizmoAxis PickGizmoAxis(int sx, int sy, int width, int height)
        {
            return TranslationGizmo.PickAxis(Camera, sx, sy, width, height);
        }

        // Gizmo manipulation
        public void StartGizmoManipulation(GizmoAxis axis, int sx, int sy, int width, int height)
        {
            TranslationGizmo.StartManipulation(axis, Camera, sx, sy, width, height);
        }

        public void UpdateGizmoManipulation(int sx, int sy, int width, int height)
        {
            Vector3 delta = TranslationGizmo.UpdateManipulation(Camera, sx, sy, width, height);

            if (delta != Vector3.Zero)
            {
                ApplyGizmoDelta(delta);
            }
        }

        public void EndGizmoManipulation()
        {
            TranslationGizmo.EndManipulation();
        }

        private void ApplyGizmoDelta(Vector3 delta)
        {
            IRenderer selected = GetAsset(selectedID);
            if (selected == null) return;

            GizmoMode mode = TranslationGizmo.GetMode();
            Matrix4x4 currentTransform = selected.Transform;

            switch (mode)
            {
                case GizmoMode.Translate:
                    currentTransform.Translation += delta;
                    break;

                case GizmoMode.Rotate:
                    // delta contains rotation angles in radians for each axis
                    Matrix4x4 rotationX = Matrix4x4.CreateRotationX(delta.X);
                    Matrix4x4 rotationY = Matrix4x4.CreateRotationY(delta.Y);
                    Matrix4x4 rotationZ = Matrix4x4.CreateRotationZ(delta.Z);
                    Matrix4x4 combinedRotation = rotationX * rotationY * rotationZ;

                    // Apply rotation around object's current position
                    Vector3 position = currentTransform.Translation;
                    currentTransform.Translation = Vector3.Zero;
                    currentTransform = currentTransform * combinedRotation;
                    currentTransform.Translation = position;
                    break;

                case GizmoMode.Scale:
                    // delta contains scale factors for each axis
                    Vector3 currentScale = new Vector3(
                        new Vector3(currentTransform.M11, currentTransform.M12, currentTransform.M13).Length(),
                        new Vector3(currentTransform.M21, currentTransform.M22, currentTransform.M23).Length(),
                        new Vector3(currentTransform.M31, currentTransform.M32, currentTransform.M33).Length()
                    );
                    Vector3 newScale = currentScale + delta;
                    newScale = Vector3.Max(newScale, new Vector3(0.01f)); // Prevent zero/negative scale

                    // Apply scale change
                    Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(
                        newScale.X / currentScale.X,
                        newScale.Y / currentScale.Y,
                        newScale.Z / currentScale.Z
                    );
                    Vector3 pos = currentTransform.Translation;
                    currentTransform.Translation = Vector3.Zero;
                    currentTransform = currentTransform * scaleMatrix;
                    currentTransform.Translation = pos;
                    break;
            }

            selected.SetTransform(currentTransform);
            selectionBox.SetTransform(currentTransform);

            // Notify listeners that the selected object has been updated
            OnSelectedObjectUpdated?.Invoke(this, new UpdateSelectedEventArgs { RefID = selectedID });
        }

        public void OnResize(int width, int height)
        {
            Camera.SetProjectionMatrix(width, height);
        }

        public void RotateCamera(float deltaX, float deltaY)
        {
            Camera.Pitch(deltaY);
            Camera.Yaw(deltaX);
        }

        private void OnSelectedObjectHasUpdated(object Sender, UpdateSelectedEventArgs Args)
        {
            if(selectedID == Args.RefID)
            {
                IRenderer RenderAsset = GetAsset(Args.RefID);
                selectionBox.SetTransform(RenderAsset.Transform);
                selectionBox.Update(RenderAsset.BoundingBox);

                // TODO: Improve this. We're not actually selecting an entry.
                // Gizmo should be scrapped and re-attempted.
                Matrix4x4 TempTransform = Matrix4x4.Identity;
                TempTransform.Translation = selectionBox.Transform.Translation;
                TranslationGizmo.OnSelectEntry(TempTransform, true);
            }
        }

        public void Shutdown()
        {
            WorldSettings.Shutdown();
            WorldSettings = null;
            Camera = null;

            foreach (IRenderer RenderAsset in Assets.Values)
            {
                RenderAsset.Shutdown();
            }

            foreach (SpatialGrid grid in navigationGrids)
            {
                grid?.Shutdown();
            }

            OurPrimitiveManager?.Shutdown();
            OurPrimitiveManager = null;
            navigationGrids = null;
            translokatorGrid?.Shutdown();
            translokatorGrid = null;
            selectionBox.Shutdown();
            selectionBox = null;
            TranslationGizmo.Shutdown();
            TranslationGizmo = null;
            clouds.Shutdown();
            clouds = null;
            sky.Shutdown();
            sky = null;
            Assets = null;
            D3D?.Shutdown();
            D3D = null;
            selectedInstances = null;
            InstanceGizmo.Shutdown();
        }


        public void UpdateInstanceBuffers(List<RenderModel> renderModels)
        {
            foreach (var model in renderModels)
            {
                model.ReloadInstanceBuffer(D3D.Device);
            }
        }

        public string GetStatusBarText()
        {
            if (BVHBuildingTasks.Count == 0)
            {
                return "";
            }

            //return Utils.Language.Language.GetString("$BUILDING_BVH"); //Keeps printing missing text in debug build and slowing things down
            return $"Building BVH: {NumBVHBuilt}/{NumBVHToBuild}";
        }

        public ID3D11Device GetId3D11Device()
        {
            return D3D.Device;
        }
        public void ToggleD3DFillMode() => D3D.ToggleFillMode();
        public void ToggleD3DCullMode() => D3D.ToggleCullMode();
        
        public void DeleteInstance(FrameObjectBase frame,int InstanceRefID)
        {
            if (Assets.ContainsKey(frame.RefID))
            {
                RenderModel asset = Assets[frame.RefID] as RenderModel;
                asset.RemoveInstance(InstanceRefID,D3D.Device);
            }

            if (frame.Children.Count > 0)
            {
                foreach (FrameObjectBase child in frame.Children)
                {
                    DeleteInstance(child,InstanceRefID);
                }            
            }
        }
        public void DeleteInstance(int InstanceRefID)
        {
            InstanceGizmo.InstanceModel.RemoveInstance(InstanceRefID,D3D.Device);
        }

        public void SetTranslokatorGridEnabled(int index, bool enabled)
        {
            translokatorGrid.SetGridEnabled(index, enabled);
        }
    }
}