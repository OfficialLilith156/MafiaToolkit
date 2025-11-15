using Rendering.Core;
using Rendering.Graphics;
using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Utils.VorticeUtils;
using Vortice.Mathematics;
using static SharpGLTF.Scenes.LightBuilder;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type4 : IType
    {
        public byte Unk0 { get; set; }

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

        private Vector3 _rotation;
        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                NotifyUpdate();
            }
        }
        public uint ID { get; set; }
        public uint LinkID_2 { get; set; }
        public uint LinkID_3 { get; set; }

        private Vector3 _direction;
        public Vector3 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                NotifyUpdate();
            }
        }
        public float Length { get; set; }
        public byte Flags { get; set; }
        public byte Unk7 { get; set; }
        public uint[] Unk8 { get; set; }
        public uint Unk9 { get; set; }

        public AIWorld_Type4(AIWorld InWorld) : base(InWorld)
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Direction = Vector3.Zero;
            Unk8 = new uint[0];
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk0 = Reader.ReadByte();
            Position = Vector3Utils.ReadFromFile(Reader);
            Rotation = Vector3Utils.ReadFromFile(Reader);
            ID = Reader.ReadUInt32();
            LinkID_2 = Reader.ReadUInt32();
            LinkID_3 = Reader.ReadUInt32();
            Direction = Vector3Utils.ReadFromFile(Reader);
            Length = Reader.ReadSingle();
            Flags = Reader.ReadByte();
            Unk7 = Reader.ReadByte();

            ushort Size = Reader.ReadUInt16();
            Unk8 = new uint[Size];
            for (int i = 0; i < Unk8.Length; i++)
            {
                Unk8[i] = Reader.ReadUInt32();
            }

            Unk9 = Reader.ReadUInt32();
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk0);
            Position.WriteToFile(Writer);
            Rotation.WriteToFile(Writer);
            Writer.Write(ID);
            Writer.Write(LinkID_2);
            Writer.Write(LinkID_3);
            Direction.WriteToFile(Writer);
            Writer.Write(Length);
            Writer.Write(Flags);
            Writer.Write(Unk7);

            Writer.Write((ushort)Unk8.Length);
            foreach(uint Value in Unk8)
            {
                Writer.Write(Value);
            }

            Writer.Write(Unk9);
        }

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Type 4: ");
            Writer.WriteLine("Unk0: {0}", Unk0);
            Writer.WriteLine("Position: {0}", Position);
            Writer.WriteLine("Rotation: {0}", Rotation);
            Writer.WriteLine("ID: {0}", ID);
            Writer.WriteLine("Link_2: {0}", LinkID_2);
            Writer.WriteLine("Link_3: {0}", LinkID_3);
            Writer.WriteLine("Direction: {0}", Direction);
            Writer.WriteLine("Length: {0}", Length);
            Writer.WriteLine("Flags: {0}", Flags);
            Writer.WriteLine("Unk7: {0}", Unk7);

            Writer.WriteLine("Unk8 Size: {0}", Unk8.Length);
            foreach(uint Value in Unk8)
            {
                Writer.WriteLine("Value: {0}", Value);
            }
        }

        public override void ConstructRenderable(PrimitiveBatch BBoxBatcher)
        {
            base.ConstructRenderable(BBoxBatcher);

            RenderBoundingBox navigationBox = new RenderBoundingBox();
            navigationBox.SetColour(System.Drawing.Color.White);
            navigationBox.Init(new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f)));
            Vector3 rotRad = new Vector3(
                Rotation.X * (MathF.PI / 180f),
                Rotation.Y * (MathF.PI / 180f),
                Rotation.Z * (MathF.PI / 180f)
            );

            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(rotRad.Y, rotRad.X, rotRad.Z);
            rotationMatrix.Translation = Position;

            navigationBox.SetTransform(rotationMatrix);
            BBoxBatcher.AddObject(RefID, navigationBox);
        }

        public override TreeNode PopulateTreeNode()
        {
            base.PopulateTreeNode();

            TreeNode ThisNode = new TreeNode();
            ThisNode.Text = string.Format("Type4: {0}", ID);
            ThisNode.Name = RefID.ToString();
            ThisNode.Tag = this;

            return ThisNode;
        }

        public override Vector3 GetPosition()
        {
            return Position;
        }
    }
}
