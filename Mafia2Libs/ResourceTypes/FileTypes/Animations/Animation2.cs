using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.ModelHelpers.ModelExporter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Logging;

namespace ResourceTypes.Animation2
{
    public class Animation2
    {
        public Header Header { get; set; } = new();
        public bool IsDataPresent { get; set; } = false; //Not confirmed? //If true, UnkShorts00 can't be empty
        public Event[] PrimaryEvents { get; set; } = new Event[0];
        public Event[] SecondaryEvents { get; set; } = new Event[0];
        public ushort Unk00 { get; set; }
        public ushort Unk01 { get; set; }
        public AnimTrack[] Tracks { get; set; } = new AnimTrack[0];
        public short[] UnkShorts00 { get; set; } = new short[0];
        public short[] UnkShorts01 { get; set; } = new short[0];

        public Animation2()
        {

        }

        public Animation2(string fileName)
        {
            Read(fileName);
        }

        public Animation2(Stream s)
        {
            Read(s);
        }

        public Animation2(BinaryReader br)
        {
            Read(br);
        }

        public void Read(string fileName)
        {
            using (MemoryStream ms = new(File.ReadAllBytes(fileName)))
            {
                Read(ms);
            }
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            Header = new(br);

            IsDataPresent = br.ReadBoolean();

            PrimaryEvents = new Event[Header.NumPrimaryEvents];

            for (int i = 0; i < PrimaryEvents.Length; i++)
            {
                PrimaryEvents[i] = new(br);
            }

            SecondaryEvents = new Event[Header.NumSecondaryEvents];

            for (int i = 0; i < SecondaryEvents.Length; i++)
            {
                SecondaryEvents[i] = new(br);
            }

            Unk00 = br.ReadUInt16();
            Unk01 = br.ReadUInt16();
            short Count2 = br.ReadInt16();

            ToolkitAssert.Ensure(Header.Count == Count2, "Animation2: Count 1 != Count 2");

            Count2 = Header.RootBoneID != 0 ? (short)(Count2 + 1) : Count2;

            Tracks = new AnimTrack[Count2];

            for (int i = 0;i < Tracks.Length; i++)
            {
                Tracks[i] = new(br);
            }

            UnkShorts00 = new short[Unk01];
            UnkShorts01 = new short[Header.Count];

            for (int i = 0; i < UnkShorts00.Length; i++)
            {
                UnkShorts00[i] = br.ReadInt16();
            }

            for (int i = 0; i < UnkShorts01.Length; i++)
            {
                UnkShorts01[i] = br.ReadInt16();
            }

            ToolkitAssert.Ensure(br.BaseStream.Position == br.BaseStream.Length, "Animation2: Failed to reach EOF.");
        }

        public void WriteToFile(string fileName)
        {
            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    Write(bw);
                }

                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Write(BinaryWriter bw)
        {
            int Count = Header.RootBoneID != 0 ? (Tracks.Length - 1) : Tracks.Length;
            Header.Count = (short)Count;
            Header.NumPrimaryEvents = (short)PrimaryEvents.Length;
            Header.NumSecondaryEvents = (short)SecondaryEvents.Length;

            Header.Write(bw);
            bw.Write(IsDataPresent);

            foreach (var val in PrimaryEvents)
            {
                val.Write(bw);
            }

            foreach (var val in SecondaryEvents)
            {
                val.Write(bw);
            }

            bw.Write(Unk00);
            bw.Write(Unk01);
            bw.Write((short)Count);

            foreach (var track in Tracks)
            {
                track.Write(bw);
            }

            if (UnkShorts00.Length < Unk01)
            {
                short[] newUnk00Shorts = new short[Unk01];
                Array.Copy(UnkShorts00, 0, newUnk00Shorts, 0, UnkShorts00.Length);
                UnkShorts00 = newUnk00Shorts;
            }

            if (UnkShorts01.Length < Count)
            {
                short[] newUnk01Shorts = new short[Count];
                Array.Copy(UnkShorts01, 0, newUnk01Shorts, 0, UnkShorts01.Length);
                UnkShorts01 = newUnk01Shorts;
            }

            for (int i = 0; i < Unk01; i++)
            {
                bw.Write(UnkShorts00[i]);
            }

            for (int i = 0; i < Count; i++)
            {
                bw.Write(UnkShorts01[i]);
            }
        }

        public MT_Animation ConvertToAnimation()
        {
            MT_Animation NewAnimation = new MT_Animation();
            NewAnimation.Tracks = new MT_AnimTrack[Tracks.Length];

            for (int z = 0; z < Tracks.Length; z++)
            {
                AnimTrack Track = Tracks[z];

                MT_AnimTrack NewTrack = new MT_AnimTrack();
                NewAnimation.Tracks[z] = NewTrack;

                NewTrack.BoneID = Track.BoneID;
                NewTrack.BoneName = NewTrack.BoneID.ToString();
                NewTrack.Duration = Track.Duration;

                var rotKeys = new List<MT_RotKey>();
                foreach (var kf in Track.KeyFrames)
                    rotKeys.Add(new MT_RotKey(kf));

                NewTrack.RotKeyFrames = rotKeys
                    .GroupBy(k => k.Time)
                    .Select(g => g.Last())  
                    .OrderBy(k => k.Time)   
                    .ToArray();

                var posKeys = new List<MT_PosKey>();
                foreach (var kf in Track.Positions.KeyFrames)
                    posKeys.Add(new MT_PosKey(kf));

                NewTrack.PosKeyFrames = posKeys
                    .GroupBy(k => k.Time)
                    .Select(g => g.Last())
                    .OrderBy(k => k.Time)
                    .ToArray();
            }

            return NewAnimation;
        }

        public void ConvertFromAnimation(MT_Animation InAnimation)
        {
            Header.Hash = FNV64.Hash(InAnimation.AnimName);
            Header.Duration = InAnimation.Duration;
            Header.Count = (short)InAnimation.Tracks.Length;

            List<AnimTrack> NewTracks = new List<AnimTrack>();
            List<short> UnkShorts01List = new List<short>();

            for (short i = 0; i < InAnimation.Tracks.Length; i++)
            {
                MT_AnimTrack Track = InAnimation.Tracks[i];
                AnimTrack NewTrack = new AnimTrack();

                NewTrack.KeyFrames = Track.RotKeyFrames.Select(k => k.AsPair()).ToArray();
                NewTrack.Positions.KeyFrames = Track.PosKeyFrames.Select(k => k.AsPair()).ToArray();

                int posFlag = NewTrack.Positions.KeyFrames.Length > 0 ? 1 : 0;
                int rotFlag = NewTrack.KeyFrames.Length > 0 ? 2 : 0;
                int posDataFlag = NewTrack.Positions.KeyFrames.Length > 0 ? 2 : 0;
                int rotDataFlag = NewTrack.KeyFrames.Length > 0 ? 1 : 0;

                NewTrack.Flags = (byte)(0x20 | (posFlag | rotFlag));
                NewTrack.DataFlags = (byte)(0x8 | (posDataFlag | rotDataFlag));
                NewTrack.BoneID = Track.BoneID;
                NewTrack.Duration = Track.Duration;

                NewTracks.Add(NewTrack);
                UnkShorts01List.Add(i);
            }

            Tracks = NewTracks.ToArray();
            UnkShorts01 = UnkShorts01List.ToArray();

            Unk01 = (ushort)Tracks.Length;
            UnkShorts00 = new short[Unk01];
            for (int i = 0; i < UnkShorts00.Length; i++) UnkShorts00[i] = 0;

            IsDataPresent = true;
            Unk00 = 0;
        }
    }
}
