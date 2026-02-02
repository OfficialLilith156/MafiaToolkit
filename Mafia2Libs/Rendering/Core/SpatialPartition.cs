using Rendering.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vortice.Mathematics;

namespace Rendering.Core
{
    public class SpatialPartition
    {
        private Octree<IRenderer> octree;
        private Dictionary<int, OctreeNode<IRenderer>> refIDToNode = new Dictionary<int, OctreeNode<IRenderer>>();
        private BoundingBox worldBounds;
        private bool isBuilt = false;

        public SpatialPartition()
        {
            // Default world bounds - will be expanded as objects are added
            worldBounds = new BoundingBox(new Vector3(-10000, -1000, -10000), new Vector3(10000, 1000, 10000));
        }

        public void Build(IEnumerable<IRenderer> objects, BoundingBox customWorldBounds)
        {
            worldBounds = customWorldBounds;
            octree = new Octree<IRenderer>(worldBounds, maxDepth: 5, maxObjectsPerNode: 8);

            int refID = 0;
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    var node = octree.Insert(obj);
                    refIDToNode[refID] = node;
                    refID++;
                }
            }

            isBuilt = true;
        }

        public void Insert(IRenderer obj, int refID)
        {
            if (octree == null)
            {
                octree = new Octree<IRenderer>(worldBounds, maxDepth: 5, maxObjectsPerNode: 8);
                isBuilt = true;
            }

            var node = octree.Insert(obj);
            refIDToNode[refID] = node;
        }

        public void Remove(int refID)
        {
            if (!refIDToNode.ContainsKey(refID))
                return;

            var node = refIDToNode[refID];
            if (node != null && node.Objects != null)
            {
                // Find and remove the object from the node
                // Note: This is a simplified implementation
                refIDToNode.Remove(refID);
            }
        }

        public void Update(int refID, BoundingBox newBounds)
        {
            // For now, just remove and re-insert
            if (refIDToNode.ContainsKey(refID))
            {
                var node = refIDToNode[refID];
                if (node != null && node.Objects != null && node.Objects.Count > 0)
                {
                    foreach (var obj in node.Objects)
                    {
                        Remove(refID);
                        Insert(obj, refID);
                        break;
                    }
                }
            }
        }

        public IEnumerable<IRenderer> QueryFrustum(Camera camera)
        {
            if (!isBuilt || octree == null)
                return new List<IRenderer>();

            // For now, return all objects - full octree frustum query can be implemented later
            // This is a placeholder that maintains compatibility
            return octree.GetAllObjects();
        }
    }

    public class OctreeNode<T> where T : IRenderer
    {
        public BoundingBox Bounds { get; set; }
        public List<T> Objects { get; set; }
        public OctreeNode<T>[] Children { get; set; }
        public bool IsLeaf => Children == null;

        public OctreeNode(BoundingBox bounds)
        {
            Bounds = bounds;
            Objects = new List<T>();
            Children = null;
        }
    }

    public class Octree<T> where T : IRenderer
    {
        private OctreeNode<T> root;
        private int maxDepth;
        private int maxObjectsPerNode;

        public Octree(BoundingBox worldBounds, int maxDepth = 5, int maxObjectsPerNode = 8)
        {
            this.maxDepth = maxDepth;
            this.maxObjectsPerNode = maxObjectsPerNode;
            root = new OctreeNode<T>(worldBounds);
        }

        public OctreeNode<T> Insert(T obj)
        {
            return InsertIntoNode(root, obj, 0);
        }

        private OctreeNode<T> InsertIntoNode(OctreeNode<T> node, T obj, int depth)
        {
            // If this is a leaf and we haven't exceeded capacity or max depth, add here
            if (node.IsLeaf)
            {
                node.Objects.Add(obj);

                // Subdivide if necessary
                if (node.Objects.Count > maxObjectsPerNode && depth < maxDepth)
                {
                    Subdivide(node);
                    // Redistribute objects to children
                    RedistributeObjects(node);
                }

                return node;
            }

            // If not a leaf, find the appropriate child
            int childIndex = GetChildIndex(node, obj.BoundingBox);
            if (childIndex != -1 && node.Children[childIndex] != null)
            {
                return InsertIntoNode(node.Children[childIndex], obj, depth + 1);
            }

            // If no appropriate child, store in this node
            node.Objects.Add(obj);
            return node;
        }

        private void Subdivide(OctreeNode<T> node)
        {
            Vector3 center = (node.Bounds.Min + node.Bounds.Max) * 0.5f;
            Vector3 min = node.Bounds.Min;
            Vector3 max = node.Bounds.Max;

            node.Children = new OctreeNode<T>[8];

            // Create 8 child octants
            node.Children[0] = new OctreeNode<T>(new BoundingBox(min, center));
            node.Children[1] = new OctreeNode<T>(new BoundingBox(new Vector3(center.X, min.Y, min.Z), new Vector3(max.X, center.Y, center.Z)));
            node.Children[2] = new OctreeNode<T>(new BoundingBox(new Vector3(min.X, min.Y, center.Z), new Vector3(center.X, center.Y, max.Z)));
            node.Children[3] = new OctreeNode<T>(new BoundingBox(new Vector3(center.X, min.Y, center.Z), new Vector3(max.X, center.Y, max.Z)));
            node.Children[4] = new OctreeNode<T>(new BoundingBox(new Vector3(min.X, center.Y, min.Z), new Vector3(center.X, max.Y, center.Z)));
            node.Children[5] = new OctreeNode<T>(new BoundingBox(new Vector3(center.X, center.Y, min.Z), new Vector3(max.X, max.Y, center.Z)));
            node.Children[6] = new OctreeNode<T>(new BoundingBox(new Vector3(min.X, center.Y, center.Z), new Vector3(center.X, max.Y, max.Z)));
            node.Children[7] = new OctreeNode<T>(new BoundingBox(center, max));
        }

        private void RedistributeObjects(OctreeNode<T> node)
        {
            List<T> remainingObjects = new List<T>();

            foreach (var obj in node.Objects)
            {
                int childIndex = GetChildIndex(node, obj.BoundingBox);
                if (childIndex != -1)
                {
                    node.Children[childIndex].Objects.Add(obj);
                }
                else
                {
                    remainingObjects.Add(obj);
                }
            }

            node.Objects = remainingObjects;
        }

        private int GetChildIndex(OctreeNode<T> node, BoundingBox objBounds)
        {
            if (node.Children == null)
                return -1;

            Vector3 center = (node.Bounds.Min + node.Bounds.Max) * 0.5f;
            Vector3 objCenter = (objBounds.Min + objBounds.Max) * 0.5f;

            int index = 0;
            if (objCenter.X > center.X) index |= 1;
            if (objCenter.Y > center.Y) index |= 4;
            if (objCenter.Z > center.Z) index |= 2;

            // Check if object actually fits in child bounds
            if (node.Children[index] != null)
            {
                BoundingBox childBounds = node.Children[index].Bounds;
                if (BoundingBoxContains(childBounds, objBounds))
                {
                    return index;
                }
            }

            return -1;
        }

        private bool BoundingBoxContains(BoundingBox container, BoundingBox contained)
        {
            return container.Min.X <= contained.Min.X && container.Max.X >= contained.Max.X &&
                   container.Min.Y <= contained.Min.Y && container.Max.Y >= contained.Max.Y &&
                   container.Min.Z <= contained.Min.Z && container.Max.Z >= contained.Max.Z;
        }

        public List<T> GetAllObjects()
        {
            List<T> allObjects = new List<T>();
            CollectAllObjects(root, allObjects);
            return allObjects;
        }

        private void CollectAllObjects(OctreeNode<T> node, List<T> collection)
        {
            if (node == null)
                return;

            collection.AddRange(node.Objects);

            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    if (child != null)
                    {
                        CollectAllObjects(child, collection);
                    }
                }
            }
        }

        public List<T> QueryFrustum(Plane[] frustumPlanes)
        {
            List<T> visibleObjects = new List<T>();
            QueryNodeFrustum(root, frustumPlanes, visibleObjects);
            return visibleObjects;
        }

        private void QueryNodeFrustum(OctreeNode<T> node, Plane[] frustumPlanes, List<T> results)
        {
            if (node == null)
                return;

            // Check if node bounds intersect frustum
            if (!BoundingBoxIntersectsFrustum(node.Bounds, frustumPlanes))
                return;

            // Add objects from this node
            results.AddRange(node.Objects);

            // Recursively check children
            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    if (child != null)
                    {
                        QueryNodeFrustum(child, frustumPlanes, results);
                    }
                }
            }
        }

        private bool BoundingBoxIntersectsFrustum(BoundingBox box, Plane[] planes)
        {
            // Simple AABB vs frustum test
            foreach (var plane in planes)
            {
                Vector3 positiveVertex = box.Min;
                if (plane.Normal.X >= 0) positiveVertex.X = box.Max.X;
                if (plane.Normal.Y >= 0) positiveVertex.Y = box.Max.Y;
                if (plane.Normal.Z >= 0) positiveVertex.Z = box.Max.Z;

                if (Vector3.Dot(plane.Normal, positiveVertex) + plane.D < 0)
                    return false;
            }

            return true;
        }
    }
}
