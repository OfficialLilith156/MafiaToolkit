using ResourceTypes.Navigation.Traffic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderRoad : IRenderer
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RenderLine Spline { get; set; }
        public BoundingBox BBox { get; set; }

        private List<Triangle> worldTriangles = new List<Triangle>();

        private struct Triangle { public Vector3 A, B, C; }

        Render2DPlane[] Planes;

        public RenderRoad()
        {
            DoRender = true;
            Transform = Matrix4x4.Identity;
            Planes = new Render2DPlane[0];
            Spline = new RenderLine();
        }

        public void Init(IRoadDefinition RoadDefinition, IRoadSpline RoadSpline)
        {
            float LeftLanesWidth = 0.0f;
            for(int i = 0; i < RoadDefinition.OppositeLanesCount; i++)
            {
                LeftLanesWidth += RoadDefinition.Lanes[i].Width;
            }

            Planes = new Render2DPlane[RoadDefinition.Lanes.Count];
            float CurrentOffset = -LeftLanesWidth;
            for(int i = 0; i < RoadDefinition.Lanes.Count; i++)
            {
                ILaneDefinition LaneDefinition = RoadDefinition.Lanes[i];

                Color LaneColour = Color.White;
                switch (LaneDefinition.LaneType)
                {
                    case LaneType.MainRoad:
                        LaneColour = Color.Chartreuse;
                        break;
                    case LaneType.Byroad:
                        LaneColour = Color.Fuchsia;
                        break;
                    case LaneType.ExclImpassable:
                        LaneColour = Color.DimGray;
                        break;
                    case LaneType.EmptyRoad:
                        LaneColour = Color.Yellow;
                        break;
                    case LaneType.Parking:
                        LaneColour = Color.CornflowerBlue;
                        break;
                }

                float zOffset = RoadDefinition.Direction == RoadDirection.Backwards ? -0.5f : -1.0f;

                Vector3[] Points = RoadSpline.Points.Select(v => new Vector3(v.X, v.Y, v.Z + zOffset)).ToArray();
                if (RoadDefinition.Direction == RoadDirection.Towards)
                {               
                    Points = Points.Reverse().ToArray();
                }    

                Render2DPlane Lane = new Render2DPlane();
                Lane.Init(LaneDefinition, Points, LaneDefinition.Width, CurrentOffset, zOffset, LaneColour);
                Planes[i] = Lane;

                CurrentOffset += LaneDefinition.Width;
            }

            Spline = new RenderLine();
            Spline.SetUnselectedColour(System.Drawing.Color.White);
            Spline.Init(RoadSpline.Points.ToArray());
            BBox = BoundingBox.CreateFromPoints(RoadSpline.Points.ToArray());

            BuildTriangles();
        }

        private void BuildTriangles()
        {
            worldTriangles.Clear();
            foreach (var plane in Planes)
            {
                for (int i = 0; i < plane.Indices.Length; i += 3)
                {
                    int i1 = plane.Indices[i];
                    int i2 = plane.Indices[i + 1];
                    int i3 = plane.Indices[i + 2];

                    Vector3 v1 = plane.Vertices[i1].Position;
                    Vector3 v2 = plane.Vertices[i2].Position;
                    Vector3 v3 = plane.Vertices[i3].Position;

                    v1 = Vector3.Transform(v1, Transform);
                    v2 = Vector3.Transform(v2, Transform);
                    v3 = Vector3.Transform(v3, Transform);

                    worldTriangles.Add(new Triangle { A = v1, B = v2, C = v3 });
                }
            }
        }
        public void UpdateFromDefinition(IRoadDefinition roadDef)
        {
            float t = roadDef.MaxSpawnedCars / 10f;
            var color = new Vector4(t, 1 - t, 0, 1);
        }

        public float? Raycast(Ray ray)
        {
            float? closest = null;
            foreach (var tri in worldTriangles)
            {
                if (RayIntersectsTriangle(ray, tri.A, tri.B, tri.C, out float distance))
                {
                    if (!closest.HasValue || distance < closest.Value)
                        closest = distance;
                }
            }
            return closest;
        }
        private static bool RayIntersectsTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out float distance)
        {
            distance = 0;
            const float EPS = 1e-8f;

            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;
            Vector3 h = Vector3.Cross(ray.Direction, edge2);
            float a = Vector3.Dot(edge1, h);
            if (a > -EPS && a < EPS) return false;

            float f = 1.0f / a;
            Vector3 s = ray.Position - v0;
            float u = f * Vector3.Dot(s, h);
            if (u < 0.0f || u > 1.0f) return false;

            Vector3 q = Vector3.Cross(s, edge1);
            float v = f * Vector3.Dot(ray.Direction, q);
            if (v < 0.0f || u + v > 1.0f) return false;

            float t = f * Vector3.Dot(edge2, q);
            if (t > EPS)
            {
                distance = t;
                return true;
            }
            return false;
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext context)
        {
            Spline.InitBuffers(d3d, context);

            foreach (Render2DPlane plane in Planes)
            {
                plane.InitBuffers(d3d, context);
            }
        }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }
            //if (!camera.CheckBBoxFrustum(Transform, BoundingBox))
            //    return;

            //if (!camera.CheckBBoxFrustum(Transform, BoundingBox))
            //    return;

            Spline.Render(device, deviceContext, camera);

            foreach (Render2DPlane plane in Planes)
            {
                plane.Render(device, deviceContext, camera);
            }
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix; 
            BuildTriangles();
        }

        public override void Shutdown()
        {
            Spline.Shutdown();

            foreach (Render2DPlane plane in Planes)
            {
                plane.Shutdown();
            }
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            Spline.UpdateBuffers(device, deviceContext);

            foreach (Render2DPlane plane in Planes)
            {
                plane.UpdateBuffers(device, deviceContext);
            }
        }

        public override void Select()
        {
            Spline.Select();
            foreach (var plane in Planes)
                plane.Select();
        }

        public override void Unselect()
        {
            Spline.Unselect();
            foreach (var plane in Planes)
                plane.Unselect();
        }
    }
}
