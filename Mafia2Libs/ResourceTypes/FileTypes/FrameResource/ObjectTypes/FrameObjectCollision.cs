using Rendering.Core;
using Rendering.Factories;
using Rendering.Graphics;
using ResourceTypes.ItemDesc;
using System;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectCollision : FrameObjectBase
    {
        private ulong _Hash;
        public ItemDescLoader ItemDesc;

        public ulong Hash {
            get { return _Hash; }
            set {
                _Hash = value;
                GetUsedItemDesc();
            }
        }

        public string ItemDescFileName {
            get {
                return ItemDesc?.FileName;
            }
        }

        public FrameObjectCollision(FrameResource OwningResource) : base(OwningResource)
        {
            _Hash = 0;
            ItemDesc = null;
        }

        public FrameObjectCollision(FrameObjectCollision other) : base(other)
        {
            _Hash = other._Hash;
            ItemDesc = other.ItemDesc;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            _Hash = reader.ReadUInt64(isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(_Hash);
        }

        public override void ConstructRenderable()
        {
            GetUsedItemDesc();
            if (ItemDesc == null) return;

            IRenderer collisionRenderer = BuildCollisionRendererFromItemDesc(ItemDesc);
            if (collisionRenderer != null)
            {
                Matrix4x4 combined = WorldTransform * ItemDesc.Matrix;
                collisionRenderer.SetTransform(combined);

                if (RenderAdapter == null)
                    RenderAdapter = new RenderableAdapter();
                RenderAdapter.InitAdaptor(collisionRenderer, this);
            }
        }

        private IRenderer BuildCollisionRendererFromItemDesc(ItemDescLoader item)
        {
            switch (item.ColType)
            {
                case CollisionTypes.Box:
                    var box = item.Collisions[0] as CollisionBox;
                    return RenderableFactory.BuildBoundingBoxFromBox(box, Matrix4x4.Identity);
                case CollisionTypes.Sphere:
                    var sphere = item.Collisions[0] as CollisionSphere;
                    return RenderableFactory.BuildBoundingSphere(sphere, Matrix4x4.Identity);
                case CollisionTypes.Capsule:
                    var capsule = item.Collisions[0] as CollisionCapsule;
                    return RenderableFactory.BuildBoundingCapsule(capsule, Matrix4x4.Identity);
                case CollisionTypes.Convex:
                    var convex = item.Collisions[0] as CollisionConvex;
                    var renderCol = new RenderStaticCollision();
                    renderCol.ConvertCollisionToRender(convex);
                    return renderCol;
                default:
                    return null;
            }
        }

        // TODO: Move this to a different location.
        // It would be better if this didn't access SceneData.
        public void GetUsedItemDesc()
        {
            foreach(ItemDescLoader ItemDesc in OwningResource.SceneData.ItemDescs)
            {
                if(ItemDesc.FrameRef == _Hash)
                {
                    this.ItemDesc = ItemDesc;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}