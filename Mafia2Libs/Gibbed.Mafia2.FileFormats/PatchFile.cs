using Gibbed.Illusion.FileFormats;
using Gibbed.IO;
using Gibbed.Mafia2.FileFormats.Archive;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gibbed.Mafia2.FileFormats
{
    public class PatchFile
    {
        public FileInfo file;
        public ResourceEntry[] resources;

        public const uint Signature = 0xD010F0F;
        public const uint Signature2 = 0xF0F0010D;
        private int UnkCount1;
        public int[] UnkInts1;
        private int UnkCount2;
        public int[] UnkInts2;
        private int UnkTotal; //UnkCount1 and UnkCount2 added together.

        private int numTypes;
        public ResourceType[] Types;

        public void Deserialize(Stream reader, Endian endian)
        {
            int magic = reader.ReadValueS32(endian);
            if (magic != Signature)
            {
                reader.Position -= 4;
                magic = reader.ReadValueS32(endian == Endian.Big ? Endian.Little : Endian.Big);

                if (magic != Signature)
                    return;
                else
                    endian = endian == Endian.Big ? Endian.Little : Endian.Big;
            }

            int version = reader.ReadValueS32(endian);
            //if (version > 1)
            //    return;

            uint magic2 = reader.ReadValueU32(endian);
            if (magic2 != Signature2)
                return;

            int numTypes = reader.ReadValueS32(endian);
            Types = new ResourceType[numTypes];
            for (int i = 0; i < numTypes; i++)
            {
                Types[i] = ResourceType.Read(reader, endian);
                // Добавить логирование для отладки
                Console.WriteLine($"Patch Type {i}: ID={Types[i].Id}, Name={Types[i].Name}");
            }

            List<string> indexes = new List<string>();
            indexes.Add("UnkSet0:");
            UnkCount1 = reader.ReadValueS32(endian);
            UnkInts1 = new int[UnkCount1];
            for (int i = 0; i != UnkCount1; i++)
            {
                UnkInts1[i] = reader.ReadValueS32(endian);
                indexes.Add(UnkInts1[i].ToString());
            }
            indexes.Add("/nUnkSet1:");
            UnkCount2 = reader.ReadValueS32(endian);
            UnkInts2 = new int[UnkCount2];
            for (int i = 0; i != UnkCount2; i++)
            {
                UnkInts2[i] = reader.ReadValueS32(endian);
                indexes.Add(UnkInts2[i].ToString());
            }

            UnkTotal = reader.ReadValueS32(endian);

            //if (UnkCount1 + UnkCount2 != UnkTotal)
            //throw new FormatException();        

            if (UnkTotal == 0)
                return;

            int pos = (int)reader.Position;

            if (reader.Position >= reader.Length)
            {
                resources = new ResourceEntry[0];
                return;
            }

            try
            {
                var blockStream = BlockReaderStream.FromStream(reader, endian);
                reader.Position = pos;

                resources = new ResourceEntry[UnkTotal];
                for (uint i = 0; i < resources.Length; i++)
                {
                    Archive.ResourceHeader resourceHeader;

                    if (reader.Position + 26 > reader.Length)
                    {
                        resources[i] = new Archive.ResourceEntry()
                        {
                            TypeId = 0,
                            Version = 0,
                            Data = new byte[0]
                        };
                        continue;
                    }

                    using (var data = blockStream.ReadToMemoryStream(26))
                    {
                        resourceHeader = Archive.ResourceHeader.Read(data, endian, 19);
                    }

                    if (reader.Position + 4 > reader.Length)
                    {
                        resources[i] = new Archive.ResourceEntry()
                        {
                            TypeId = (int)resourceHeader.TypeId,
                            Version = resourceHeader.Version,
                            Data = new byte[0],
                            SlotRamRequired = resourceHeader.SlotRamRequired,
                            SlotVramRequired = resourceHeader.SlotVramRequired,
                            OtherRamRequired = resourceHeader.OtherRamRequired,
                            OtherVramRequired = resourceHeader.OtherVramRequired,
                        };
                        continue;
                    }

                    blockStream.ReadBytes(4); //checksum i think

                    uint dataSize = 0;
                    if (resourceHeader.Size >= 30)
                    {
                        dataSize = resourceHeader.Size - 30;
                    }

                    if (reader.Position + dataSize > reader.Length)
                    {
                        dataSize = (uint)Math.Max(0, reader.Length - reader.Position);
                    }

                    byte[] resourceData = null;
                    if (dataSize > 0)
                    {
                        resourceData = blockStream.ReadBytes((int)dataSize);
                    }
                    else
                    {
                        resourceData = new byte[0];
                    }

                    resources[i] = new Archive.ResourceEntry()
                    {
                        TypeId = (int)resourceHeader.TypeId,
                        Version = resourceHeader.Version,
                        Data = resourceData,
                        SlotRamRequired = resourceHeader.SlotRamRequired,
                        SlotVramRequired = resourceHeader.SlotVramRequired,
                        OtherRamRequired = resourceHeader.OtherRamRequired,
                        OtherVramRequired = resourceHeader.OtherVramRequired,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading patch file: {ex.Message}");
                resources = new ResourceEntry[Math.Max(0, UnkTotal)];
                for (int i = 0; i < resources.Length; i++)
                {
                    resources[i] = new Archive.ResourceEntry()
                    {
                        TypeId = 0,
                        Version = 0,
                        Data = new byte[0]
                    };
                }
            }
        }
    }
}
