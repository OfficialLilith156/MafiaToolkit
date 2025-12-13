using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.ItemDesc
{
    public class CollisionBox
    {
        private Vector3 vector;

        [Category("Geometry")]
        public Vector3 Extents
        {
            get => vector;
            set
            {
                vector = value;
            }
        }

        [Browsable(false)]
        public Vector3 Size => vector * 2f;

        public CollisionBox() { }

        public CollisionBox(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            vector = Vector3Utils.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        public override string ToString() => $"Box (Extents: {Extents.X:0.###}, {Extents.Y:0.###}, {Extents.Z:0.###})";
    }
}