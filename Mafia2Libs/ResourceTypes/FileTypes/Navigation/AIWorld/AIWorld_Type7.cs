using Rendering.Core;
using Rendering.Graphics;
using System;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type7 : IType //Obstacles
    {
        public ushort Unk0 { get; set; }

        private Vector3 _position;
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                NotifyUpdate();
            }
        }

        private Vector3 _direction;
        public Vector3 Direction
        {
            get => _direction;
            set
            {
                if (value.Length() > 0.0001f)
                    _direction = Vector3.Normalize(value);
                else
                    _direction = Vector3.UnitZ;
                NotifyUpdate();
            }
        }
        public Vector3 Unk2 { get; set; }
        public uint Unk3 { get; set; }

        private Vector3 _minimum;
        private Vector3 _maximum;

        [Category("BBox Test")]
        public Vector3 Minimum
        {
            get => new Vector3(_minimum.X, _minimum.Z, _minimum.Y);
            set
            {
                _minimum = new Vector3(value.X, value.Z, value.Y);
                UpdateUnk2FromBBox();
                NotifyUpdate();
            }
        }

        [Category("BBox Test")]
        public Vector3 Maximum
        {
            get => new Vector3(_maximum.X, _maximum.Z, _maximum.Y);
            set
            {
                _maximum = new Vector3(value.X, value.Z, value.Y);
                UpdateUnk2FromBBox();
                NotifyUpdate();
            }
        }

        public AIWorld_Type7(AIWorld InWorld) : base(InWorld)
        {
            Position = Vector3.Zero;
            Direction = new Vector3(0, 0, 1);
            bIsVisible = true;

         
            
            Unk2 = new Vector3(0.5f, 0.5f, 0.5f);

      
            _minimum = new Vector3(-Unk2.X, -Unk2.Z, -Unk2.Y);
            _maximum = new Vector3(Unk2.X, Unk2.Z, Unk2.Y);
        }
        private void UpdateUnk2FromBBox()
        {
            Vector3 halfSize = (Maximum - Minimum) * 0.5f;
            const float MIN_HALF = 0.01f;
            if (MathF.Abs(halfSize.X) < MIN_HALF) halfSize.X = MIN_HALF;
            if (MathF.Abs(halfSize.Y) < MIN_HALF) halfSize.Y = MIN_HALF;
            if (MathF.Abs(halfSize.Z) < MIN_HALF) halfSize.Z = MIN_HALF;

            Unk2 = new Vector3(halfSize.X, halfSize.Z, halfSize.Y);
        }

        private void UpdateBBoxFromUnk2()
        {
            _minimum = new Vector3(
                -Unk2.X,
                -Unk2.Z,
                -Unk2.Y
            );

            _maximum = new Vector3(
                 Unk2.X,
                 Unk2.Z,
                 Unk2.Y
            );
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);
            Unk0 = Reader.ReadUInt16();
            Position = Vector3Utils.ReadFromFile(Reader);
            Direction = Vector3Utils.ReadFromFile(Reader);
            Unk2 = Vector3Utils.ReadFromFile(Reader);
            Unk3 = Reader.ReadUInt32();
            UpdateBBoxFromUnk2();
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk0);
            Position.WriteToFile(Writer);
            Direction.WriteToFile(Writer);

         
            UpdateUnk2FromBBox();

            Unk2.WriteToFile(Writer);
            Writer.Write(Unk3);
        }

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Type 7:");
            Writer.WriteLine($"Unk0: {Unk0}");
            Writer.WriteLine($"Position: {Position}");
            Writer.WriteLine($"Direction: {Direction}");
            Writer.WriteLine($"Unk2: {Unk2}");
            Writer.WriteLine($"Unk3: {Unk3}");
            Writer.WriteLine($"BBox Min: {Minimum}");
            Writer.WriteLine($"BBox Max: {Maximum}");
        }

        public override void ConstructRenderable(PrimitiveBatch BBoxBatcher)
        {
            base.ConstructRenderable(BBoxBatcher);

            if (!bIsVisible)
                return;


            Vector3 minWorld = new Vector3(_minimum.X, _minimum.Z, _minimum.Y);
            Vector3 maxWorld = new Vector3(_maximum.X, _maximum.Z, _maximum.Y);

            if (MathF.Abs(maxWorld.X - minWorld.X) < 0.0001f) maxWorld.X = minWorld.X + 0.01f;
            if (MathF.Abs(maxWorld.Y - minWorld.Y) < 0.0001f) maxWorld.Y = minWorld.Y + 0.01f;
            if (MathF.Abs(maxWorld.Z - minWorld.Z) < 0.0001f) maxWorld.Z = minWorld.Z + 0.01f;

            RenderBoundingBox navigationBox = new RenderBoundingBox();
            navigationBox.SetColour(System.Drawing.Color.Green);
            BoundingBox BBox = new BoundingBox(minWorld, maxWorld);

            Vector3 dir = Direction;
            if (dir.LengthSquared() < 0.0001f) dir = Vector3.UnitZ;
            dir = Vector3.Normalize(dir);

            Vector3 forward = Vector3.Normalize(dir);
            Vector3 up = Vector3.UnitY;
            Quaternion rotation = Quaternion.CreateFromRotationMatrix(
                Matrix4x4.CreateLookAt(Vector3.Zero, forward, up)
            );

            Matrix4x4 RotationMatrix = MatrixUtils.CreateFromDirection(Direction);
            RotationMatrix.Translation = Position;

            navigationBox.Init(BBox);
            navigationBox.SetTransform(RotationMatrix);
            BBoxBatcher.AddObject(RefID, navigationBox);
        }

        public override TreeNode PopulateTreeNode()
        {
            TreeNode node = new TreeNode();
            node.Text = $"Obstacles (Unk3:{Unk3}, ID:{ID})";
            node.Name = RefID.ToString();
            node.Tag = this;

            return node;
        }



        public override Vector3 GetPosition()
        {
            return Position;
        }
    }
}
