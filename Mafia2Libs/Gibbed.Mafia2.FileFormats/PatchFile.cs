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
        // Parallel to 'resources': true when the entry is a binary delta against the base SDS
        // resource (classic Mafia II) rather than a standalone/full resource (Mafia II DE).
        public bool[] ResourceIsDelta;

        public const uint Signature = 0xD010F0F;
        public const uint Signature2 = 0xF0F0010D;
        private int UnkCount1;
        public int[] UnkInts1;
        private int UnkCount2;
        public int[] UnkInts2;
        private int UnkTotal; //UnkCount1 and UnkCount2 added together.

        private int numTypes;
        private ResourceType[] Types;

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
            // Version 1 patches (e.g. Xbox 360 .path files, big-endian) store only a single index
            // set; the second set was added in version 2 (PC classic delta patches). Reading a
            // second set on a version 1 file would consume UnkTotal as a bogus count and corrupt
            // the rest of the parse.
            indexes.Add("/nUnkSet1:");
            if (version >= 2)
            {
                UnkCount2 = reader.ReadValueS32(endian);
                UnkInts2 = new int[UnkCount2];
                for (int i = 0; i != UnkCount2; i++)
                {
                    UnkInts2[i] = reader.ReadValueS32(endian);
                    indexes.Add(UnkInts2[i].ToString());
                }
            }
            else
            {
                UnkInts2 = new int[0];
            }

            UnkTotal = reader.ReadValueS32(endian);

            //if (UnkCount1 + UnkCount2 != UnkTotal)
            //throw new FormatException();        

            if (UnkTotal == 0)
                return;

            int pos = (int)reader.Position;

            var blockStream = BlockReaderStream.FromStream(reader, endian);
            reader.Position = pos;

            // Decompress the whole block stream into memory. Patch entries are laid out
            // back-to-back as [26-byte resource header][4-byte FNV32 hash of that header][payload].
            //
            // Mafia II (DE) patches store a full resource, so payload == Size-30. But classic
            // Mafia II patches store a *binary delta* against the base SDS resource: the 26-byte
            // header (and its hash) are copied verbatim from the base resource (so Size stays the
            // original, huge value) while the payload is much smaller. Trusting Size-30 there makes
            // the reader run off the end of the stream (BlockReaderStream throws
            // "Operation is not valid due to the current state of the object").
            //
            // Instead we split entries by locating the FNV32 header hashes, which works for both
            // games and for full/delta payloads alike.
            byte[] data;
            using (var full = new MemoryStream())
            {
                blockStream.SaveUncompressed(full);
                data = full.ToArray();
            }

            resources = new ResourceEntry[UnkTotal];
            ResourceIsDelta = new bool[UnkTotal];

            int offset = 0;
            for (int i = 0; i < UnkTotal; i++)
            {
                if (offset + 30 > data.Length || IsHeaderAt(data, offset, endian) == false)
                {
                    throw new FormatException("Failed to locate patch resource entry header.");
                }

                Archive.ResourceHeader resourceHeader;
                using (var header = new MemoryStream(data, offset, 26, false))
                {
                    resourceHeader = Archive.ResourceHeader.Read(header, endian, 19);
                }
                if (resourceHeader.Size < 30)
                {
                    throw new FormatException();
                }

                // Find where this entry ends (== start of the next header, or end of stream for
                // the last entry). Fast path: full-resource entries end exactly at Size bytes.
                int payloadStart = offset + 30;
                int end;
                if (i == UnkTotal - 1)
                {
                    end = data.Length;
                }
                else
                {
                    int fast = offset + (int)resourceHeader.Size;
                    if (fast + 30 <= data.Length && IsHeaderAt(data, fast, endian))
                    {
                        end = fast; // full resource replacement
                    }
                    else
                    {
                        end = FindNextHeader(data, payloadStart, endian);
                        if (end < 0)
                        {
                            throw new FormatException("Failed to locate next patch resource entry.");
                        }
                    }
                }

                int payloadLength = end - payloadStart;
                byte[] payload = new byte[payloadLength];
                Array.Copy(data, payloadStart, payload, 0, payloadLength);

                // If the stored payload is smaller than the declared resource size, this is a
                // binary delta against the base SDS resource rather than a standalone resource.
                ResourceIsDelta[i] = payloadLength < (int)resourceHeader.Size - 30;

                resources[i] = new Archive.ResourceEntry()
                {
                    TypeId = (int)resourceHeader.TypeId,
                    Version = resourceHeader.Version,
                    Data = payload,
                    SlotRamRequired = resourceHeader.SlotRamRequired,
                    SlotVramRequired = resourceHeader.SlotVramRequired,
                    OtherRamRequired = resourceHeader.OtherRamRequired,
                    OtherVramRequired = resourceHeader.OtherVramRequired,
                };

                offset = end;
            }
        }

        // True when the 26 bytes at 'offset' are a resource header immediately followed by their
        // own FNV32 hash (the delimiter the engine writes between patch entries).
        private static bool IsHeaderAt(byte[] data, int offset, Endian endian)
        {
            if (offset < 0 || offset + 30 > data.Length)
            {
                return false;
            }

            // The 26-byte resource header is stored in the file's endianness, but its trailing
            // FNV32 checksum is written as the raw little-endian uint on both PC and Xbox 360
            // (big-endian) patches. Accept either byte order to be safe.
            uint computed = Illusion.FileFormats.Hashing.FNV32.Hash(data, offset, 26);
            return computed == ReadU32(data, offset + 26, Endian.Little)
                || computed == ReadU32(data, offset + 26, Endian.Big);
        }

        private static int FindNextHeader(byte[] data, int start, Endian endian)
        {
            for (int p = start; p + 30 <= data.Length; p++)
            {
                if (IsHeaderAt(data, p, endian))
                {
                    return p;
                }
            }
            return -1;
        }

        private static uint ReadU32(byte[] data, int offset, Endian endian)
        {
            uint value = (uint)(data[offset] | (data[offset + 1] << 8) | (data[offset + 2] << 16) | (data[offset + 3] << 24));
            if (endian == Endian.Big)
            {
                value = (value >> 24) | ((value >> 8) & 0xFF00) | ((value << 8) & 0xFF0000) | (value << 24);
            }
            return value;
        }
    }
}