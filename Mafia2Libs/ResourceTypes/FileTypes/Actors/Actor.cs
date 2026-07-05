using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utils.Logging;
using Utils.StringHelpers;
using XBOX;
using XBOX.ActorFile;

namespace ResourceTypes.Actors
{
    public class Actor
    {
        private bool _isBigEndian;
        public bool IsBigEndian => _isBigEndian;

        List<ActorDefinition> definitions;
        List<ActorEntry> items;
        string pool;
        //temp_unk start
        int filesize; //size of sector in bits. After this integer (so filesize - 4)
        short const6; //always 6
        short const2; //2 or 0
        byte[] unk02; //only full when const 2 == 0;
        int const16; //always 16
        int size;
        int unk12;
        int unk14;
        int unk13;
        List<ActorExtraData> extraData;
        string fileName;

        public List<ActorDefinition> Definitions
        {
            get { return definitions; }
        }
        public List<ActorEntry> Items
        {
            get { return items; }
        }
        public List<ActorExtraData> ExtraData
        {
            get { return extraData; }
            set { extraData = value; }
        }

        public Actor(FileInfo InFileInfo)
        {
            fileName = InFileInfo.FullName;
            definitions = new List<ActorDefinition>();
            items = new List<ActorEntry>();
            extraData = new List<ActorExtraData>();

            using (var fs = File.OpenRead(InFileInfo.FullName))
            {
                byte[] header = new byte[4];
                fs.Read(header, 0, 4);
                _isBigEndian = (header[0] == 0x00);
            }

            using (var reader = new EndianBinaryReader(File.OpenRead(InFileInfo.FullName), _isBigEndian))
            {
                ReadFromFile(reader);
            }
        }

        public Actor() : base()
        {
            definitions = new List<ActorDefinition>();
            items = new List<ActorEntry>();
            extraData = new List<ActorExtraData>();
        }

        public Actor(string InFilename) : this()
        {
            fileName = InFilename;

            const16 = 16;
            const2 = 2;
            const6 = 6;
        }

        public Actor(FileInfo InFileInfo, bool isBigEndian)
        {
            _isBigEndian = isBigEndian;
            fileName = InFileInfo.FullName;
            using (var reader = new EndianBinaryReader(File.OpenRead(InFileInfo.FullName), _isBigEndian))
            {
                ReadFromFile(reader);
            }
        }

        private string BuildDefinitions()
        {
            if (Definitions.Count == 0)
            {
                // TODO: Check if this is correct?
                return string.Empty;
            }

            // First we generate the Dictionary to store definition names
            Dictionary<string, int> NameToOffsetLookup = new Dictionary<string, int>();
            foreach (ActorDefinition Definition in Definitions)
            {
                string Name = Definition.Name;
                if (NameToOffsetLookup.ContainsKey(Name) == false)
                {
                    NameToOffsetLookup.Add(Name, -1);
                }
            }

            // Next we iterate through all of the actors and fill in the buffer pool
            // It seems like the official tools also did this, so we will do the same.
            // We want to replicate their pipeline as much as possible.
            string OutBufferPool = "<scene>\0";
            foreach (ActorEntry CurrentEntry in items)
            {
                string NameToSave = CurrentEntry.DefinitionName;
                if (NameToOffsetLookup.ContainsKey(NameToSave) && NameToOffsetLookup[NameToSave] == -1)
                {
                    int StartOffset = OutBufferPool.Length;
                    OutBufferPool += NameToSave;
                    OutBufferPool += '\0';
                    NameToOffsetLookup[CurrentEntry.DefinitionName] = StartOffset;
                }
            }

            // Now we iterate through the definition list again, and update offsets
            foreach (ActorDefinition Definition in Definitions)
            {
                Definition.NamePos = (ushort)NameToOffsetLookup[Definition.Name];
            }

            return OutBufferPool;
        }

        private void Sanitize()
        {
            var ordered = definitions.OrderBy(d => d.FrameNameHash);
            definitions = ordered.ToList();

            Dictionary<short, short> reorganisedKeys = new Dictionary<short, short>();
            extraData.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].DataID == -1)
                {
                    // Skip, no data is present
                    continue;
                }

                if (!reorganisedKeys.ContainsKey(items[i].DataID))
                {
                    extraData.Add(items[i].Data);
                    reorganisedKeys.Add(items[i].DataID, (short)(extraData.Count - 1));
                    items[i].DataID = reorganisedKeys[items[i].DataID];
                }
                else
                {
                    items[i].DataID = reorganisedKeys[items[i].DataID];
                }
            }
            reorganisedKeys.Clear();
        }

        public ActorEntry CreateActorEntry(ActorTypes type, string name)
        {
            ActorExtraData NewExtraData = null;
            if (RequiresExtraData(type))
            {
                NewExtraData = new ActorExtraData();
                NewExtraData.BufferType = type;
                NewExtraData.Data = ActorFactory.CreateExtraData(type);
            }

            ActorEntry entry = ActorFactory.CreateActorItem(type, name);
            entry.DataID = (short)(NewExtraData != null ? ExtraData.Count : -1);
            entry.Data = NewExtraData;

            ExtraData.Add(NewExtraData);
            Items.Add(entry);
            return entry;
        }

        public ActorDefinition CreateActorDefinition(ActorEntry entry)
        {
            ActorDefinition definition = new ActorDefinition();
            definition.Name = entry.DefinitionName;
            definition.FrameNameHash = entry.FrameNameHash;
            Definitions.Add(definition);
            return definition;
        }

        public void ReadFromFile(EndianBinaryReader reader)
        {
            try
            {
                long startPos = reader.BaseStream.Position;
                long fileLen = reader.BaseStream.Length;
                // MessageBox.Show($"Start: pos={startPos}, len={fileLen}", "Debug");

                int poolLength = reader.ReadInt32();
                // if (poolLength > reader.BaseStream.Length - 4)
                //throw new Exception($"poolLength={poolLength} exceeds file length, file may be corrupted or wrong format.");
                // MessageBox.Show($"poolLength={poolLength}, pos={reader.BaseStream.Position}", "Debug");
                pool = new string(reader.ReadChars(poolLength));



                int hashesLength = reader.ReadInt32();
                // MessageBox.Show($"hashesLength={hashesLength}, pos={reader.BaseStream.Position}", "Debug");
                definitions = new List<ActorDefinition>();

                for (int i = 0; i < hashesLength; i++)
                {
                    var definition = new ActorDefinition();
                    definition.ReadFromFile(reader);
                    int pos = definition.NamePos;
                    definition.Name = pool.Substring(pos, pool.IndexOf('\0', pos) - pos);
                    definitions.Add(definition);
                }

                long actorDataOffset = reader.BaseStream.Position + 4;

                filesize = reader.ReadInt32();
                if (_isBigEndian)
                {
                    const2 = reader.ReadInt16();
                    const6 = reader.ReadInt16();
                }
                else
                {
                    const6 = reader.ReadInt16();
                    const2 = reader.ReadInt16();
                }
                const16 = reader.ReadInt32();
                size = reader.ReadInt32();
                unk12 = reader.ReadInt32();
                unk13 = reader.ReadInt32();
                unk14 = reader.ReadInt32();

                // MessageBox.Show($"filesize={filesize}, const2={const2}, const6={const6}, const16={const16}, size={size}, unk12={unk12}, unk13={unk13}, unk14={unk14}, pos={reader.BaseStream.Position}", "Debug");

                if (const2 == 2)
                {
                    int count = (unk14 - 8) / 4;
                    //MessageBox.Show($"const2=2, count={count}, seek={unk14 - 12}, pos={reader.BaseStream.Position}", "Debug");
                    reader.BaseStream.Seek(unk14 - 12, SeekOrigin.Current);
                    extraData = new List<ActorExtraData>();
                    for (int i = 0; i < count; i++)
                    {
                        var extra = new ActorExtraData();
                        extra.ReadFromFile(reader, _isBigEndian);
                        extraData.Add(extra);
                    }
                    //MessageBox.Show($"After extraData, pos={reader.BaseStream.Position}", "Debug");
                }
                else
                {
                    // const2 == 0
                    int bytesToSkip = size - unk14;
                    //if (bytesToSkip < 0)
                    //    throw new Exception($"Negative bytesToSkip: {bytesToSkip}, size={size}, unk14={unk14}");
                    //MessageBox.Show($"const2!=2, skip {bytesToSkip} bytes, pos={reader.BaseStream.Position}", "Debug");
                    unk02 = reader.ReadBytes(bytesToSkip);
                    //MessageBox.Show($"After skip, pos={reader.BaseStream.Position}", "Debug");
                }

                int itemCount = reader.ReadInt32();
                //MessageBox.Show($"itemCount={itemCount}, pos={reader.BaseStream.Position}", "Debug");
                reader.BaseStream.Seek(itemCount * 4, SeekOrigin.Current);

                items = new List<ActorEntry>();
                for (int i = 0; i < itemCount; i++)
                {
                    var entry = new ActorEntry();
                    entry.ReadFromFile(reader);
                    if (entry.DataID != -1)
                        entry.Data = ExtraData[entry.DataID];
                    items.Add(entry);
                }

                int numCutscenes = reader.ReadInt32();
                if (numCutscenes > 0)
                {
                    long endPosition = 0;
                    for (int i = 0; i < numCutscenes; i++)
                    {
                        uint offset = reader.ReadUInt32();
                        long currentPosition = reader.BaseStream.Position;
                        reader.BaseStream.Seek(actorDataOffset + offset, SeekOrigin.Begin);
                        string cutsceneName = StringHelpers.ReadString(reader);
                        ushort cutscene_unk01 = reader.ReadUInt16();
                        endPosition = reader.BaseStream.Position;
                        reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
                    }
                    reader.BaseStream.Position = endPosition;
                }

                ToolkitAssert.Ensure(reader.BaseStream.Position == reader.BaseStream.Length, "Not at end.");
                //MessageBox.Show($"Success! Final pos={reader.BaseStream.Position}", "Debug");
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"ERROR: {ex.Message}\nPosition: {reader.BaseStream.Position}\nLength: {reader.BaseStream.Length}", "Error");
                throw;
            }
        }

        public void WriteToFile()
        {
            Sanitize();
            pool = BuildDefinitions();

            // Write the file
            using (EndianBinaryWriter writer = new EndianBinaryWriter(File.Open(fileName, FileMode.Create), _isBigEndian))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(EndianBinaryWriter writer)
        {
            Dictionary<int, int> sanitizedIDs = new Dictionary<int, int>();

            // Cutscenes to save at the end of the file.
            List<ActorEntry> cutsceneEntries = new List<ActorEntry>();

            Sanitize();
            pool = BuildDefinitions();

            writer.Write(pool.Length);
            StringHelpers.WriteString(writer, pool, false);
            writer.Write(definitions.Count);
            for (int i = 0; i < definitions.Count; i++)
            {
                definitions[i].WriteToFile(writer);
            }

            long instancePos = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(const6);
            writer.Write(const2);
            writer.Write(const16);
            writer.Write(int.MinValue); //size
            writer.Write(int.MinValue); //unk12

            int instanceOffset = ((extraData.Count * sizeof(int)) + 8);
            writer.Write(0);

            //could do it so we seek to offset and save each one, but that would decrease performance. 
            for (int i = 0; i < extraData.Count; i++)
            {
                writer.Write(instanceOffset);
                instanceOffset += (extraData[i].Data != null ? extraData[i].Data.GetSize() : extraData[i].GetDataInBytes().Length) + 8;
            }

            for (int i = 0; i < extraData.Count; i++)
            {
                extraData[i].WriteToFile(writer);
            }

            int itemOffset = instanceOffset + (items.Count * sizeof(int)) + 16;
            long itemPos = writer.BaseStream.Position;
            writer.Write(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                writer.Write(itemOffset);
                itemOffset += items[i].CalculateSize();
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].WriteToFile(writer);

                // We need to store cutscenes so we can save them for later.
                ActorTypes actorType = (ActorTypes)items[i].ActorTypeID;
                if (actorType == ActorTypes.C_Cutscene)
                {
                    cutsceneEntries.Add(items[i]);
                }
            }

            // Now we try and save the cutscene data which is stored at the bottom of the file.
            long cutsceneEntryOffset = writer.BaseStream.Position;
            long[] cutsceneOffsets = new long[cutsceneEntries.Count];
            writer.Write(cutsceneEntries.Count);

            // TODO: Maybe consider doing one loop rather than two.
            for (int i = 0; i < cutsceneEntries.Count; i++)
            {
                ActorEntry cutscene = cutsceneEntries[i];

                // Save our 'TEMP' offset; this stored the ptr to the string.
                cutsceneOffsets[i] = writer.BaseStream.Position;
                writer.Write(-1);
            }

            for (int i = 0; i < cutsceneEntries.Count; i++)
            {
                ActorEntry cutscene = cutsceneEntries[i];

                // Now we save the cutscene entity name and save the new offset;
                uint nameOffset = (uint)writer.BaseStream.Position;
                StringHelpers.WriteString(writer, cutscene.EntityName);
                writer.Write((ushort)0);

                long completeOffset = writer.BaseStream.Position;

                // Update our new offset and then return to our original offset;
                writer.BaseStream.Seek(cutsceneOffsets[i], SeekOrigin.Begin);
                uint newOffset = (uint)(nameOffset - (instancePos + 4));
                writer.Write(newOffset);

                writer.BaseStream.Seek(completeOffset, SeekOrigin.Begin);
            }

            //for that unknown value.
            long endPos = cutsceneEntryOffset - instancePos - 4;
            long instanceLength = writer.BaseStream.Position - instancePos - 4;
            long unk = writer.BaseStream.Position - itemPos;
            long size = instanceLength - unk;

            writer.BaseStream.Seek(instancePos, SeekOrigin.Begin);
            writer.Write((int)(instanceLength));
            writer.Write(const6);
            writer.Write(const2);
            writer.Write(const16);
            writer.Write((int)size); //size
            writer.Write((int)(endPos)); //unk12
        }

        public static bool RequiresExtraData(ActorTypes InActorType)
        {
            // Quicker to test whether it is not C_Car or C_Train
            return (InActorType != ActorTypes.C_Car && InActorType != ActorTypes.C_Train);
        }
    }
}