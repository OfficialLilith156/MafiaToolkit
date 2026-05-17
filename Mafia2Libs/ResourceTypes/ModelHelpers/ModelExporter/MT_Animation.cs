using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Utils.Models;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_RotKey
    {
        public float Time { get; set; }
        public Quaternion Value { get; set; }

        public MT_RotKey()
        {
            Time = 0.0f;
            Value = Quaternion.Identity;
        }

        public MT_RotKey((float, Quaternion) InValue)
        {
            Time = InValue.Item1;
            Value = InValue.Item2;
        }

        public (float, Quaternion) AsPair()
        {
            return (Time, Value);
        }

        public override string ToString()
        {
            return string.Format("[{0}] - [{1}]", Time, Value.ToString());
        }
    }

    public class MT_PosKey
    {
        public float Time { get; set; }
        public Vector3 Value { get; set; }

        public MT_PosKey()
        {
            Time = 0.0f;
            Value = Vector3.Zero;
        }

        public MT_PosKey((float, Vector3) InValue)
        {
            Time = InValue.Item1;
            Value = InValue.Item2;
        }

        public (float, Vector3) AsPair()
        {
            return (Time, Value);
        }

        public override string ToString()
        {
            return string.Format("[{0}] - [{1}]", Time, Value.ToString());
        }
    }

    public class MT_AnimTrack
    {
        public SkeletonBoneIDs BoneID { get; set; }
        public string BoneName { get; set; }
        public float Duration { get; set; }
        public MT_RotKey[] RotKeyFrames { get; set; }
        public MT_PosKey[] PosKeyFrames { get; set; }

        public MT_AnimTrack()
        {
            BoneName = string.Empty;
            BoneID = SkeletonBoneIDs.BaseRef;

            RotKeyFrames = new MT_RotKey[0];
            PosKeyFrames = new MT_PosKey[0];
        }

        public override string ToString()
        {
            return string.Format("[Name: {0}] [Duration: {1}]", BoneID, Duration);
        }
    }

    public class MT_Animation : IValidator
    {
        public string AnimName { get; set; }
        public float Duration { get; set; }
        public MT_AnimTrack[] Tracks { get; set; }

        public MT_Animation()
        {
            Tracks = new MT_AnimTrack[0];
        }

        public void BuildAnimation(Animation InAnimation)
        {
            // start porting data
            AnimName = InAnimation.Name;
            Duration = InAnimation.Duration;

            //Tracks = new MT_AnimTrack[InAnimation.Channels.Count];

            Dictionary<SkeletonBoneIDs, MT_AnimTrack> tracks = new();

            for (int z = 0; z < InAnimation.Channels.Count; z++)
            {
                // New channel (or track for Mafia II) and cache in local obj
                AnimationChannel CurrentChannel = InAnimation.Channels[z];
                
                // TODO: Need to resolve issue with missing bones!
                SkeletonBoneIDs BoneID = SkeletonBoneIDs.BaseRef;
                Enum.TryParse<SkeletonBoneIDs>(CurrentChannel.TargetNode.Name, out BoneID);

                if (!tracks.ContainsKey(BoneID))
                {
                    tracks.Add(BoneID, new MT_AnimTrack());
                }

                MT_AnimTrack NewAnimTrack = tracks[BoneID];

                NewAnimTrack.Duration = InAnimation.Duration;
                NewAnimTrack.BoneName = CurrentChannel.TargetNode.Name;
                NewAnimTrack.BoneID = BoneID;

                // Convert Position
                IAnimationSampler<Vector3> PosSampler = CurrentChannel.GetTranslationSampler();
                if (PosSampler != null)
                {
                    List<MT_PosKey> PositionKeyList = new List<MT_PosKey>();

                    IEnumerable<(float, Vector3)> PosKeys = PosSampler.GetLinearKeys();
                    Array.ForEach<(float, Vector3)>(PosKeys.ToArray(), (delegate ((float, Vector3) Item) { PositionKeyList.Add(new MT_PosKey(Item)); }));

                    NewAnimTrack.PosKeyFrames = PositionKeyList
                     .GroupBy(k => k.Time)
                     .Select(g => g.Last())
                     .OrderBy(k => k.Time)
                     .ToArray();
                }

                // Convert Rotation
                IAnimationSampler<Quaternion> RotSampler = CurrentChannel.GetRotationSampler();
                if (RotSampler != null)
                {
                    List<MT_RotKey> RotationKeyList = new List<MT_RotKey>();

                    IEnumerable<(float, Quaternion)> RotKeys = RotSampler.GetLinearKeys();
                    Array.ForEach<(float, Quaternion)>(RotKeys.ToArray(), (delegate ((float, Quaternion) Item) { RotationKeyList.Add(new MT_RotKey(Item)); }));

                    NewAnimTrack.RotKeyFrames = RotationKeyList
                     .GroupBy(k => k.Time)
                     .Select(g => g.Last())
                     .OrderBy(k => k.Time)
                     .ToArray();
                }

            }

            Tracks = tracks.Values.ToArray();
            Optimize();
        }
        public void Optimize(float rotationTolerance = 0.00001f, float positionTolerance = 0.00001f)
        {
            foreach (var track in Tracks)
            {
                if (track.RotKeyFrames?.Length > 2)
                    track.RotKeyFrames = OptimizeRotationKeys(track.RotKeyFrames, rotationTolerance);
                if (track.PosKeyFrames?.Length > 2)
                    track.PosKeyFrames = OptimizePositionKeys(track.PosKeyFrames, positionTolerance);
            }
        }

        private static MT_RotKey[] OptimizeRotationKeys(MT_RotKey[] keys, float tolerance)
        {
            if (keys.Length <= 2) return keys;

            var result = new List<MT_RotKey> { keys[0] };
            int last = 0;

            for (int i = 1; i < keys.Length - 1; i++)
            {
                float t = (keys[i].Time - keys[last].Time) / (keys[i + 1].Time - keys[last].Time);
                Quaternion interpolated = Quaternion.Slerp(keys[last].Value, keys[i + 1].Value, t);
                float angleDiff = 1.0f - Math.Abs(Quaternion.Dot(interpolated, keys[i].Value));
                if (angleDiff > tolerance)
                {
                    result.Add(keys[i]);
                    last = i;
                }
            }
            result.Add(keys[^1]);
            return result.ToArray();
        }

        private static MT_PosKey[] OptimizePositionKeys(MT_PosKey[] keys, float tolerance)
        {
            if (keys.Length <= 2) return keys;

            var result = new List<MT_PosKey> { keys[0] };
            int last = 0;

            for (int i = 1; i < keys.Length - 1; i++)
            {
                float t = (keys[i].Time - keys[last].Time) / (keys[i + 1].Time - keys[last].Time);
                Vector3 interpolated = Vector3.Lerp(keys[last].Value, keys[i + 1].Value, t);
                float error = Vector3.Distance(interpolated, keys[i].Value);
                if (error > tolerance)
                {
                    result.Add(keys[i]);
                    last = i;
                }
            }
            result.Add(keys[^1]);
            return result.ToArray();
        }
        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            // TODO
            return true;
        }
    }
}
