using Rendering.Graphics;
using System;
using System.Numerics;
using Utils.VorticeUtils;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Core
{
    public enum GizmoMode
    {
        Translate,
        Rotate,
        Scale
    }

    public enum GizmoAxis
    {
        None = -1,
        X = 0,
        Y = 1,
        Z = 2
    }

    public class GizmoTool
    {
        // Material hash to axis mapping
        private const ulong MATERIAL_RED = 1337;    // X-axis
        private const ulong MATERIAL_GREEN = 1339;  // Y-axis
        private const ulong MATERIAL_BLUE = 1338;   // Z-axis

        // State
        private bool bIsActive;
        private GizmoMode CurrentMode = GizmoMode.Translate;
        private GizmoAxis SelectedAxis = GizmoAxis.None;
        private GizmoAxis HoveredAxis = GizmoAxis.None;

        // Variables for rendering
        private RenderModel GizmoModel;

        // Variables when calculating the delta
        private Vector3 PreviousIntersection;
        private Vector3 CurrentIntersection;
        private Vector3 GizmoPosition;

        // Event for when gizmo transform changes (to update selected object)
        public event EventHandler<Vector3> OnTransformDelta;

        public GizmoTool(RenderModel InModel)
        {
            GizmoModel = InModel;
            PreviousIntersection = Vector3.Zero;
            CurrentIntersection = Vector3.Zero;
            GizmoPosition = Vector3.Zero;
            bIsActive = false;
        }

        public void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            GizmoModel.InitBuffers(d3d, d3dContext);
        }

        public void UpdateBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            GizmoModel.UpdateBuffers(d3d, d3dContext);
        }

        public void Render(ID3D11Device d3d, ID3D11DeviceContext d3dContext, Camera camera)
        {
            GizmoModel.Render(d3d, d3dContext, camera);
        }

        public void OnSelectEntry(Matrix4x4 newTransform, bool bDoRender)
        {
            GizmoModel.SetTransform(newTransform);
            GizmoModel.DoRender = bDoRender;
            GizmoPosition = newTransform.Translation;
        }

        public void Shutdown()
        {
            GizmoModel.Shutdown();
        }

        // Mode management
        public void SetMode(GizmoMode mode)
        {
            CurrentMode = mode;
        }

        public GizmoMode GetMode() => CurrentMode;

        // State queries
        public bool IsActive() => bIsActive;
        public GizmoAxis GetSelectedAxis() => SelectedAxis;
        public GizmoAxis GetHoveredAxis() => HoveredAxis;

        // Pick which axis the mouse is hovering over
        public GizmoAxis PickAxis(Camera camera, int pointX, int pointY, int width, int height)
        {
            if (!GizmoModel.DoRender)
                return GizmoAxis.None;

            Ray cameraRay = camera.GetPickingRay(new Vector2(pointX, pointY), new Vector2(width, height));
            Vector3 worldPos = Vector3.Zero;
            int partIndex = GetMaterialPartIndex(ref worldPos, cameraRay);

            if (partIndex < 0 || partIndex >= GizmoModel.LODs[0].ModelParts.Length)
                return GizmoAxis.None;

            ulong materialHash = GizmoModel.LODs[0].ModelParts[partIndex].MaterialHash;
            return MaterialHashToAxis(materialHash);
        }

        // Start manipulation when mouse is pressed on an axis
        public void StartManipulation(GizmoAxis axis, Camera camera, int pointX, int pointY, int width, int height)
        {
            if (axis == GizmoAxis.None)
                return;

            SelectedAxis = axis;
            bIsActive = true;
            GizmoPosition = GizmoModel.Transform.Translation;

            Ray cameraRay = camera.GetPickingRay(new Vector2(pointX, pointY), new Vector2(width, height));
            PreviousIntersection = ProjectRayOntoAxisPlane(cameraRay, axis, GizmoPosition, camera.Position);
        }

        // Update manipulation during mouse drag
        public Vector3 UpdateManipulation(Camera camera, int pointX, int pointY, int width, int height)
        {
            if (!bIsActive || SelectedAxis == GizmoAxis.None)
                return Vector3.Zero;

            Ray cameraRay = camera.GetPickingRay(new Vector2(pointX, pointY), new Vector2(width, height));
            CurrentIntersection = ProjectRayOntoAxisPlane(cameraRay, SelectedAxis, GizmoPosition, camera.Position);

            Vector3 delta = Vector3.Zero;

            switch (CurrentMode)
            {
                case GizmoMode.Translate:
                    delta = CalculateTranslationDelta();
                    break;
                case GizmoMode.Rotate:
                    delta = CalculateRotationDelta(camera.Position);
                    break;
                case GizmoMode.Scale:
                    delta = CalculateScaleDelta();
                    break;
            }

            PreviousIntersection = CurrentIntersection;

            // Update gizmo position for translation
            if (CurrentMode == GizmoMode.Translate && delta != Vector3.Zero)
            {
                GizmoPosition += delta;
                Matrix4x4 transform = GizmoModel.Transform;
                transform.Translation = GizmoPosition;
                GizmoModel.SetTransform(transform);
            }

            return delta;
        }

        // End manipulation when mouse is released
        public void EndManipulation()
        {
            bIsActive = false;
            SelectedAxis = GizmoAxis.None;
            PreviousIntersection = Vector3.Zero;
            CurrentIntersection = Vector3.Zero;
        }

        // Update hovered axis for visual feedback
        public void UpdateHover(Camera camera, int pointX, int pointY, int width, int height)
        {
            if (bIsActive)
                return; // Don't change hover while manipulating

            HoveredAxis = PickAxis(camera, pointX, pointY, width, height);
        }

        private Vector3 CalculateTranslationDelta()
        {
            Vector3 rawDelta = CurrentIntersection - PreviousIntersection;
            return ConstrainToAxis(rawDelta, SelectedAxis);
        }

        private Vector3 CalculateRotationDelta(Vector3 cameraPosition)
        {
            // Calculate rotation angle based on arc movement around gizmo center
            Vector3 prevDir = Vector3.Normalize(PreviousIntersection - GizmoPosition);
            Vector3 currDir = Vector3.Normalize(CurrentIntersection - GizmoPosition);

            // Cross product gives rotation axis and sine of angle
            Vector3 cross = Vector3.Cross(prevDir, currDir);
            float sinAngle = cross.Length();
            float cosAngle = Vector3.Dot(prevDir, currDir);
            float angle = MathF.Atan2(sinAngle, cosAngle);

            // Determine rotation direction based on axis
            Vector3 axisDir = GetAxisDirection(SelectedAxis);
            if (Vector3.Dot(cross, axisDir) < 0)
                angle = -angle;

            // Return rotation in radians for each axis (only selected axis has value)
            return SelectedAxis switch
            {
                GizmoAxis.X => new Vector3(angle, 0, 0),
                GizmoAxis.Y => new Vector3(0, angle, 0),
                GizmoAxis.Z => new Vector3(0, 0, angle),
                _ => Vector3.Zero
            };
        }

        private Vector3 CalculateScaleDelta()
        {
            // Scale based on distance change from gizmo center
            float prevDist = (PreviousIntersection - GizmoPosition).Length();
            float currDist = (CurrentIntersection - GizmoPosition).Length();

            if (prevDist < 0.001f)
                return Vector3.Zero;

            float scaleFactor = (currDist - prevDist) / prevDist;

            // Return scale delta for selected axis
            return SelectedAxis switch
            {
                GizmoAxis.X => new Vector3(scaleFactor, 0, 0),
                GizmoAxis.Y => new Vector3(0, scaleFactor, 0),
                GizmoAxis.Z => new Vector3(0, 0, scaleFactor),
                _ => Vector3.Zero
            };
        }

        private Vector3 ConstrainToAxis(Vector3 delta, GizmoAxis axis)
        {
            return axis switch
            {
                GizmoAxis.X => new Vector3(delta.X, 0, 0),
                GizmoAxis.Y => new Vector3(0, delta.Y, 0),
                GizmoAxis.Z => new Vector3(0, 0, delta.Z),
                _ => delta
            };
        }

        private Vector3 GetAxisDirection(GizmoAxis axis)
        {
            return axis switch
            {
                GizmoAxis.X => Vector3.UnitX,
                GizmoAxis.Y => Vector3.UnitY,
                GizmoAxis.Z => Vector3.UnitZ,
                _ => Vector3.Zero
            };
        }

        private Vector3 ProjectRayOntoAxisPlane(Ray cameraRay, GizmoAxis axis, Vector3 gizmoPos, Vector3 cameraPos)
        {
            Vector3 axisDir = GetAxisDirection(axis);

            // Create a plane that contains the axis and faces the camera
            Vector3 toCamera = Vector3.Normalize(cameraPos - gizmoPos);
            Vector3 planeNormal = Vector3.Cross(axisDir, Vector3.Cross(toCamera, axisDir));

            if (planeNormal.LengthSquared() < 0.0001f)
            {
                // Camera is looking along the axis, use perpendicular plane
                planeNormal = toCamera;
            }
            else
            {
                planeNormal = Vector3.Normalize(planeNormal);
            }

            // Plane equation: dot(planeNormal, point - gizmoPos) = 0
            float denom = Vector3.Dot(planeNormal, cameraRay.Direction);
            if (MathF.Abs(denom) < 0.0001f)
            {
                // Ray parallel to plane
                return PreviousIntersection;
            }

            float t = Vector3.Dot(planeNormal, gizmoPos - cameraRay.Position) / denom;
            if (t < 0)
            {
                // Intersection behind camera
                return PreviousIntersection;
            }

            Vector3 hitPoint = cameraRay.Position + cameraRay.Direction * t;

            // Project hit point onto the axis line
            float axisT = Vector3.Dot(hitPoint - gizmoPos, axisDir);
            return gizmoPos + axisDir * axisT;
        }

        private GizmoAxis MaterialHashToAxis(ulong materialHash)
        {
            return materialHash switch
            {
                MATERIAL_RED => GizmoAxis.X,
                MATERIAL_GREEN => GizmoAxis.Y,
                MATERIAL_BLUE => GizmoAxis.Z,
                _ => GizmoAxis.None
            };
        }

        private int GetMaterialPartIndex(ref Vector3 worldPosition, Ray cameraRay)
        {
            var lowest = float.MaxValue;
            var partIndex = -1;

            Matrix4x4 invertedWM = Matrix4x4.Identity;
            Matrix4x4.Invert(GizmoModel.Transform, out invertedWM);
            var localRay = new Ray(
                Vector3Utils.TransformCoordinate(cameraRay.Position, invertedWM),
                Vector3.Normalize(Vector3.TransformNormal(cameraRay.Direction, invertedWM))
            );

            var bbox = GizmoModel.BoundingBox;

            // Check if ray misses bounding box entirely
            if (localRay.Intersects(bbox) == 0.0f)
            {
                return -1;
            }

            for (var i = 0; i < GizmoModel.LODs[0].ModelParts.Length; i++)
            {
                var modelPart = GizmoModel.LODs[0].ModelParts[i];
                var startIndex = (int)(modelPart.StartIndex / 3);
                var numFaces = (int)modelPart.NumFaces;

                for (var x = startIndex; x < startIndex + numFaces; x++)
                {
                    var idx0 = GizmoModel.LODs[0].Indices[x * 3];
                    var idx1 = GizmoModel.LODs[0].Indices[x * 3 + 1];
                    var idx2 = GizmoModel.LODs[0].Indices[x * 3 + 2];

                    var v0 = GizmoModel.LODs[0].Vertices[idx0].Position;
                    var v1 = GizmoModel.LODs[0].Vertices[idx1].Position;
                    var v2 = GizmoModel.LODs[0].Vertices[idx2].Position;

                    if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out float t))
                    {
                        continue;
                    }

                    if (t < 0 || float.IsNaN(t))
                    {
                        continue;
                    }

                    worldPosition = cameraRay.Position + t * cameraRay.Direction;
                    var distance = (worldPosition - cameraRay.Position).LengthSquared();
                    if (distance < lowest)
                    {
                        lowest = distance;
                        partIndex = i;
                    }
                }
            }

            return partIndex;
        }

        // Legacy method for backward compatibility
        public void ManipulateGizmo(Camera camera, int pointX, int pointY, int width, int height)
        {
            if (!bIsActive)
            {
                GizmoAxis axis = PickAxis(camera, pointX, pointY, width, height);
                if (axis != GizmoAxis.None)
                {
                    StartManipulation(axis, camera, pointX, pointY, width, height);
                }
            }
            else
            {
                UpdateManipulation(camera, pointX, pointY, width, height);
            }
        }

        public void Activate() => bIsActive = true;
        public void Deactivate() => EndManipulation();
    }
}
