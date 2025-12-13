using System.IO;

namespace ResourceTypes.ItemDesc
{
    public class CollisionCapsule
    {
        float[] floats = new float[2];
        public float Radius => floats[0];
        public float HalfHeight => floats[1];

        public float Height => floats[1] * 2f;
        public float FullHeight => Height + Radius * 2f;

        public CollisionCapsule(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            floats[0] = reader.ReadSingle();
            floats[1] = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(floats[0]);
            writer.Write(floats[1]);
        }
    }
}
