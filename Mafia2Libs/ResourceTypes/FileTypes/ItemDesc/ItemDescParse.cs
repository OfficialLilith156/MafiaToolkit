using ResourceTypes.Actors;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils;
using Utils.Logging;
using Utils.VorticeUtils;

namespace ResourceTypes.ItemDesc
{
    // Collision Type 10 is 72 bytes long.
    // In ItemDesc_564 of city_crash, there is the 0xFF at the end and then a count; for instance this is 2.
    // it then followings with a 1, 10, and the a 1.0f.
    public class ItemDescLoader
    {
        public ulong FrameRef { get; set; }
        public byte UnkByte1 { get; set; }
        public CollisionTypes ColType { get; set; }
        public ulong IdHash { get; set; }
        public short ColMaterial { get; set; }

        [Browsable (false)]
        public Matrix4x4 Matrix { get; set; }
        public byte UnkByte2 { get; set; }
        public object[] Collisions { get; set; }

        public string FileName { get; private set; }

        public ItemDescLoader(string fileName)
        {
            Log.WriteLine("Trying to Parse: " + fileName, LoggingTypes.WARNING, LogCategoryTypes.FUNCTION);
            FileName = Path.GetFileName(fileName);

            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            //OverwriteConvexWithCooked("cooked.bin", fileName);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            FrameRef = reader.ReadUInt64();
            UnkByte1 = reader.ReadByte();
            ColType = (CollisionTypes)reader.ReadByte();
            IdHash = reader.ReadUInt64();
            ColMaterial = reader.ReadInt16();
            Matrix = MatrixUtils.ReadFromFile(reader);
            UnkByte2 = reader.ReadByte();

            if (ColType == CollisionTypes.Box)
                Collisions = new object[] { new CollisionBox(reader) };
            else if (ColType == CollisionTypes.Sphere)
                Collisions = new object[] { new CollisionSphere(reader) };
            else if (ColType == CollisionTypes.Capsule)
                Collisions = new object[] { new CollisionCapsule(reader) };
            else if (ColType == CollisionTypes.Convex)
                Collisions = new object[] { new CollisionConvex(reader) };
            else
                Log.WriteLine("Failed to parse collision type " + ColType, LoggingTypes.WARNING, LogCategoryTypes.FUNCTION);
        }

        public void OverwriteConvexWithCooked(string cookedName, string output)
        {
            if (ColType == CollisionTypes.Convex)
            {
                //FBXHelper.CookConvexCollision("uncooked.bin", "cooked.bin");
                byte[] data = File.ReadAllBytes(cookedName);

                using (BinaryWriter writer = new BinaryWriter(File.Open(output, FileMode.Create)))
                {
                    writer.Write(FrameRef);
                    writer.Write(UnkByte1);
                    writer.Write((byte)ColType);
                    writer.Write(IdHash);
                    writer.Write(ColMaterial);
                    Matrix.WriteToFile(writer);
                    writer.Write(UnkByte2);
                    writer.Write((ushort)data.Length);
                    writer.Write(data);
                }
                if (File.Exists("cooked.bin")) File.Delete("cooked.bin");
                if (File.Exists("uncooked.bin")) File.Delete("uncooked.bin");
                Log.WriteLine("Recooked ItemDesc", LoggingTypes.MESSAGE, LogCategoryTypes.APPLICATION);
            }
            
        }
        public void WriteToFile(BinaryWriter writer)
        {
   
            writer.Write(FrameRef);
            writer.Write(UnkByte1);
            writer.Write((byte)ColType);
            writer.Write(IdHash);
            writer.Write(ColMaterial);
            Matrix.WriteToFile(writer);

            writer.Write(UnkByte2);

            if (Collisions != null && Collisions.Length > 0)
            {
                foreach (var col in Collisions)
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
                            Log.WriteLine("Unknown collision type", LoggingTypes.WARNING, LogCategoryTypes.FUNCTION);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", FrameRef, ColType);
        }
    }
}
