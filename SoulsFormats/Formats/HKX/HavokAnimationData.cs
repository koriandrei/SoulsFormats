using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SoulsFormats.Havok
{
    public struct NewBlendableTransform
    {
        public Vector3 Translation;
        public Vector3 Scale;
        public Quaternion Rotation;

        public static NewBlendableTransform Identity => new NewBlendableTransform()
        {
            Translation = Vector3.Zero,
            Rotation = Quaternion.Identity,
            Scale = Vector3.One,
        };

        public static NewBlendableTransform Lerp(float lerp, NewBlendableTransform a, NewBlendableTransform b)
        {
            Vector3 translation = Vector3.Lerp(a.Translation, b.Translation, lerp);

            Vector3 scale = Vector3.Lerp(a.Scale, b.Scale, lerp);

            Quaternion rotation = Quaternion.Lerp(a.Rotation, b.Rotation, lerp);

            return new NewBlendableTransform()
            {
                Translation = translation,
                Scale = scale,
                Rotation = rotation,
            };
        }

        public Matrix4x4 GetMatrixScale()
        {
            return Matrix4x4.CreateScale(Scale);
        }

        public Matrix4x4 GetMatrix()
        {
            return
                //Matrix.CreateScale(Scale) *
                Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(Rotation)) *
                //Matrix.CreateFromQuaternion(Rotation) *
                Matrix4x4.CreateTranslation(Translation);
        }
    }


    public class NewRootMotionHandlerData
    {
        //public Matrix CurrentAbsoluteRootMotion = Matrix.Identity;

        public readonly Vector4 Up;
        public readonly Vector4 Forward;
        public readonly float Duration;
        public readonly Vector4[] Frames;

        /// <summary>
        /// The accumulative root motion delta applied by playing the entire anim from the beginning to the end.
        /// </summary>
        public readonly Vector4 LoopDeltaForward;

        /// <summary>
        /// The accumulative root motion delta applied by playing the entire anim in reverse from the end to the beginning.
        /// </summary>
        public readonly Vector4 LoopDeltaBackward;


        public NewRootMotionHandlerData(Vector4 up, Vector4 forward, float duration, Vector4[] frames)
        {
            Up = up;
            Forward = forward;
            Duration = duration;
            Frames = frames;

            LoopDeltaForward = frames[frames.Length - 1] - frames[0];
            LoopDeltaBackward = frames[0] - frames[frames.Length - 1];
        }


        public bool Accumulate = true;

        private Matrix4x4 GetMatrixFromSample(Vector4 sample)
        {
            return Matrix4x4.CreateRotationY(sample.W) *
                Matrix4x4.CreateWorld(
                    new Vector3(sample.X, sample.Y, sample.Z),
                    new Vector3(Forward.X, Forward.Y, -Forward.Z),
                    new Vector3(Up.X, Up.Y, Up.Z));
        }

        private Vector4 GetSample(float frame)
        {
            float frameFloor = (float)Math.Floor(frame % (Frames.Length - 1));
            Vector4 sample = Frames[(int)frameFloor];

            if (frame != frameFloor)
            {
                float frameMod = frame % 1;

                Vector4 nextFrameRootMotion;

                //if (frame >= Frames.Length - 1)
                //    nextFrameRootMotion = Frames[0];
                //else
                //    nextFrameRootMotion = Frames[(int)(frameFloor + 1)];

                nextFrameRootMotion = Frames[(int)(frameFloor + 1)];

                sample = Vector4.Lerp(sample, nextFrameRootMotion, frameMod);
            }

            return sample;
        }

        private static Vector4 AddRootMotion(Vector4 start, Vector4 toAdd, float direction, bool dontAddRotation = false)
        {
            if (!dontAddRotation)
                start.W += toAdd.W;

            Vector3 displacement = Vector3.Transform(new Vector3(toAdd.X, toAdd.Y, toAdd.Z), Matrix4x4.CreateRotationY(direction));
            start.X += displacement.X;
            start.Y += displacement.Y;
            start.Z += displacement.Z;
            return start;
        }

        public (Vector4 Motion, float Direction) UpdateRootMotion(Vector4 currentRootMotion, Vector4 prevFrameData, float currentDirection, float currentFrame, int loopCountDelta, bool forceAbsoluteRootMotion)
        {
            if (forceAbsoluteRootMotion)
                return (currentRootMotion, currentDirection);

            var nextFrameData = GetSample(currentFrame);

            if (Accumulate)
            {
                //currentRootMotion *= GetMatrixFromSample(nextFrameData - prevFrameData);

                currentRootMotion = AddRootMotion(currentRootMotion, nextFrameData - prevFrameData, currentDirection, dontAddRotation: true);

                currentRootMotion.W = nextFrameData.W;

                while (loopCountDelta != 0)
                {
                    if (loopCountDelta > 0)
                    {
                        currentRootMotion = AddRootMotion(currentRootMotion, LoopDeltaForward, currentDirection, dontAddRotation: true);
                        currentDirection += LoopDeltaForward.W;
                        loopCountDelta--;
                    }
                    else if (loopCountDelta < 0)
                    {
                        currentRootMotion = AddRootMotion(currentRootMotion, LoopDeltaBackward, currentDirection, dontAddRotation: true);
                        currentDirection += LoopDeltaBackward.W;
                        loopCountDelta++;
                    }
                }
            }
            else
            {
                currentDirection = 0;
                currentRootMotion = AddRootMotion(Vector4.Zero, nextFrameData, currentDirection);
            }

            // TODO: This code remains of what was in DSAnimStudio.
            // Maybe remove it
            // prevFrameData = nextFrameData;

            //CurrentAbsoluteRootMotion = GetMatrixFromSample(GetSample(currentFrame));

            //if (forceAbsoluteRootMotion)
            //{

            //}
            //else
            //{
            //    CurrentAbsoluteRootMotion *= GetMatrixFromSample(nextFrameData - prevFrameData);
            //}



            return (currentRootMotion, currentDirection);
        }
    }


    public abstract class HavokAnimationData
    {
        public HKX.AnimationBlendHint BlendHint = HKX.AnimationBlendHint.NORMAL;

        public string Name;

        public HKX.HKASkeleton hkaSkeleton;

        public float Duration;
        public float FrameDuration;
        public int FrameCount;

        public HavokAnimationData(string Name, HKX.HKASkeleton skeleton, HKX.HKADefaultAnimatedReferenceFrame refFrame, HKX.HKAAnimationBinding binding)
        {
            this.Name = Name;

            hkaSkeleton = skeleton;
            // TODO: Move root motion
            //if (refFrame != null)
            //{
            //    var rootMotionFrames = new Vector4[refFrame.ReferenceFrameSamples.Size];
            //    for (int i = 0; i < refFrame.ReferenceFrameSamples.Size; i++)
            //    {
            //        rootMotionFrames[i] = new Vector4(
            //            refFrame.ReferenceFrameSamples[i].Vector.X,
            //            refFrame.ReferenceFrameSamples[i].Vector.Y,
            //            refFrame.ReferenceFrameSamples[i].Vector.Z,
            //            refFrame.ReferenceFrameSamples[i].Vector.W);
            //    }
            //    var rootMotionUp = new Vector4(refFrame.Up.X, refFrame.Up.Y, refFrame.Up.Z, refFrame.Up.W);
            //    var rootMotionForward = new Vector4(refFrame.Forward.X, refFrame.Forward.Y, refFrame.Forward.Z, refFrame.Forward.W);

            //    RootMotion = new NewRootMotionHandler(rootMotionUp, rootMotionForward, refFrame.Duration, rootMotionFrames);
            //}

            BlendHint = binding.BlendHint;
        }

        public bool IsAdditiveBlend => BlendHint == HKX.AnimationBlendHint.ADDITIVE ||
            BlendHint == HKX.AnimationBlendHint.ADDITIVE_DEPRECATED;

        public abstract NewBlendableTransform GetBlendableTransformOnFrame(int hkxBoneIndex, float frame);

    }

    public class HavokAnimationData_SplineCompressed : HavokAnimationData
    {
        public HavokAnimationData_SplineCompressed(string name, HKX.HKASkeleton skeleton,
           HKX.HKADefaultAnimatedReferenceFrame refFrame, HKX.HKAAnimationBinding binding, HKX.HKASplineCompressedAnimation anim)
           : base(name, skeleton, refFrame, binding)
        {
            Duration = anim.Duration;// Math.Max(anim.Duration, anim.FrameDuration * anim.FrameCount);
            FrameCount = anim.FrameCount;

            FrameDuration = anim.FrameDuration;

            BlockCount = anim.BlockCount;
            NumFramesPerBlock = anim.FramesPerBlock - 1;

            HkxBoneIndexToTransformTrackMap = new int[skeleton.Bones.Size];
            TransformTrackIndexToHkxBoneMap = new int[binding.TransformTrackToBoneIndices.Size];

            for (int i = 0; i < binding.TransformTrackToBoneIndices.Size; i++)
            {
                TransformTrackIndexToHkxBoneMap[i] = -1;
            }

            for (int i = 0; i < skeleton.Bones.Size; i++)
            {
                HkxBoneIndexToTransformTrackMap[i] = -1;
            }

            for (int i = 0; i < binding.TransformTrackToBoneIndices.Size; i++)
            {
                short boneIndex = binding.TransformTrackToBoneIndices[i].data;
                if (boneIndex >= 0)
                    HkxBoneIndexToTransformTrackMap[boneIndex] = i;
                TransformTrackIndexToHkxBoneMap[i] = boneIndex;
            }

            Tracks = SplineCompressedAnimation.ReadSplineCompressedAnimByteBlock(
                isBigEndian: false, anim.GetData(), anim.TransformTrackCount, anim.BlockCount);
        }

        public List<SplineCompressedAnimation.TransformTrack[]> Tracks;

        // Index into array = hkx bone index, result = transform track index.
        public int[] HkxBoneIndexToTransformTrackMap;

        public int[] TransformTrackIndexToHkxBoneMap;

        public int BlockCount = 1;
        public int NumFramesPerBlock = 255;

        private NewBlendableTransform GetTransformOnSpecificBlockAndFrame(int transformIndex, int block, float frame)
        {
            frame = (frame % FrameCount) % NumFramesPerBlock;

            NewBlendableTransform result = NewBlendableTransform.Identity;
            var track = Tracks[block][transformIndex];
            var skeleTransform = hkaSkeleton.Transforms[TransformTrackIndexToHkxBoneMap[transformIndex]];

            //result.Scale.X = track.SplineScale?.ChannelX == null
            //    ? (IsAdditiveBlend ? 1 : track.StaticScale.X) : track.SplineScale.GetValueX(frame);
            //result.Scale.Y = track.SplineScale?.ChannelY == null
            //    ? (IsAdditiveBlend ? 1 : track.StaticScale.Y) : track.SplineScale.GetValueY(frame);
            //result.Scale.Z = track.SplineScale?.ChannelZ == null
            //    ? (IsAdditiveBlend ? 1 : track.StaticScale.Z) : track.SplineScale.GetValueZ(frame);

            if (track.SplineScale != null)
            {
                result.Scale.X = track.SplineScale.GetValueX(frame)
                    ?? (IsAdditiveBlend ? 1 : skeleTransform.Scale.Vector.X);

                result.Scale.Y = track.SplineScale.GetValueY(frame)
                    ?? (IsAdditiveBlend ? 1 : skeleTransform.Scale.Vector.Y);

                result.Scale.Z = track.SplineScale.GetValueZ(frame)
                    ?? (IsAdditiveBlend ? 1 : skeleTransform.Scale.Vector.Z);
            }
            else
            {
                if (track.Mask.ScaleTypes.Contains(SplineCompressedAnimation.FlagOffset.StaticX))
                    result.Scale.X = track.StaticScale.X;
                else
                    result.Scale.X = IsAdditiveBlend ? 1 : skeleTransform.Scale.Vector.X;

                if (track.Mask.ScaleTypes.Contains(SplineCompressedAnimation.FlagOffset.StaticY))
                    result.Scale.Y = track.StaticScale.Y;
                else
                    result.Scale.Y = IsAdditiveBlend ? 1 : skeleTransform.Scale.Vector.Y;

                if (track.Mask.ScaleTypes.Contains(SplineCompressedAnimation.FlagOffset.StaticZ))
                    result.Scale.Z = track.StaticScale.Z;
                else
                    result.Scale.Z = IsAdditiveBlend ? 1 : skeleTransform.Scale.Vector.Z;
            }

            if (IsAdditiveBlend)
            {
                result.Scale.X *= skeleTransform.Scale.Vector.X;
                result.Scale.Y *= skeleTransform.Scale.Vector.Y;
                result.Scale.Z *= skeleTransform.Scale.Vector.Z;
            }

            //if (result.Scale.LengthSquared() > (Vector3.One * 1.1f).LengthSquared())
            //{
            //    Console.WriteLine(":fatoof:");
            //}

            if (track.SplineRotation != null)//track.HasSplineRotation)
            {
                result.Rotation = track.SplineRotation.GetValue(frame);
            }
            else if (track.HasStaticRotation)
            {
                // We actually need static rotation or Gael hands become unbent among others
                result.Rotation = track.StaticRotation;
            }
            else
            {
                //result.Rotation = IsAdditiveBlend ? Quaternion.Identity : new Quaternion(
                //    skeleTransform.Rotation.Vector.X,
                //    skeleTransform.Rotation.Vector.Y,
                //    skeleTransform.Rotation.Vector.Z,
                //    skeleTransform.Rotation.Vector.W);
            }

            if (IsAdditiveBlend)
            {
                result.Rotation = new Quaternion(
                    skeleTransform.Rotation.Vector.X,
                    skeleTransform.Rotation.Vector.Y,
                    skeleTransform.Rotation.Vector.Z,
                    skeleTransform.Rotation.Vector.W) * result.Rotation;
            }

            if (track.SplinePosition != null)
            {
                result.Translation.X = track.SplinePosition.GetValueX(frame)
                    ?? (IsAdditiveBlend ? 0 : skeleTransform.Position.Vector.X);

                result.Translation.Y = track.SplinePosition.GetValueY(frame)
                    ?? (IsAdditiveBlend ? 0 : skeleTransform.Position.Vector.Y);

                result.Translation.Z = track.SplinePosition.GetValueZ(frame)
                    ?? (IsAdditiveBlend ? 0 : skeleTransform.Position.Vector.Z);
            }
            else
            {
                if (track.Mask.PositionTypes.Contains(SplineCompressedAnimation.FlagOffset.StaticX))
                    result.Translation.X = track.StaticPosition.X;
                else
                    result.Translation.X = IsAdditiveBlend ? 0 : skeleTransform.Position.Vector.X;

                if (track.Mask.PositionTypes.Contains(SplineCompressedAnimation.FlagOffset.StaticY))
                    result.Translation.Y = track.StaticPosition.Y;
                else
                    result.Translation.Y = IsAdditiveBlend ? 0 : skeleTransform.Position.Vector.Y;

                if (track.Mask.PositionTypes.Contains(SplineCompressedAnimation.FlagOffset.StaticZ))
                    result.Translation.Z = track.StaticPosition.Z;
                else
                    result.Translation.Z = IsAdditiveBlend ? 0 : skeleTransform.Position.Vector.Z;
            }

            //result.Translation.X = track.SplinePosition?.GetValueX(frame) ?? (IsAdditiveBlend ? 0 : track.StaticPosition.X);
            //result.Translation.Y = track.SplinePosition?.GetValueY(frame) ?? (IsAdditiveBlend ? 0 : track.StaticPosition.Y);
            //result.Translation.Z = track.SplinePosition?.GetValueZ(frame) ?? (IsAdditiveBlend ? 0 : track.StaticPosition.Z);

            //if (!IsAdditiveBlend && (!track.Mask.PositionTypes.Contains(Havok.SplineCompressedAnimation.FlagOffset.SplineX) &&
            //    !track.Mask.PositionTypes.Contains(Havok.SplineCompressedAnimation.FlagOffset.StaticX)))
            //{
            //    result.Translation.X = skeleTransform.Position.Vector.X;
            //}

            //if (!IsAdditiveBlend && (!track.Mask.PositionTypes.Contains(Havok.SplineCompressedAnimation.FlagOffset.SplineY) &&
            //    !track.Mask.PositionTypes.Contains(Havok.SplineCompressedAnimation.FlagOffset.StaticY)))
            //{
            //    result.Translation.Y = skeleTransform.Position.Vector.Y;
            //}

            //if (!IsAdditiveBlend && (!track.Mask.PositionTypes.Contains(Havok.SplineCompressedAnimation.FlagOffset.SplineZ) &&
            //    !track.Mask.PositionTypes.Contains(Havok.SplineCompressedAnimation.FlagOffset.StaticZ)))
            //{
            //    result.Translation.Z = skeleTransform.Position.Vector.Z;
            //}

            if (IsAdditiveBlend)
            {
                result.Translation.X += skeleTransform.Position.Vector.X;
                result.Translation.Y += skeleTransform.Position.Vector.Y;
                result.Translation.Z += skeleTransform.Position.Vector.Z;
            }

            return result;
        }

        public int GetBlock(float frame)
        {
            return (int)((frame % FrameCount) / NumFramesPerBlock);
        }

        public override NewBlendableTransform GetBlendableTransformOnFrame(int hkxBoneIndex, float frame)
        {
            var track = HkxBoneIndexToTransformTrackMap[hkxBoneIndex];

            if (track == -1)
            {
                var skeleTransform = hkaSkeleton.Transforms[hkxBoneIndex];

                NewBlendableTransform defaultBoneTransformation = new NewBlendableTransform();

                defaultBoneTransformation.Scale.X = skeleTransform.Scale.Vector.X;
                defaultBoneTransformation.Scale.Y = skeleTransform.Scale.Vector.Y;
                defaultBoneTransformation.Scale.Z = skeleTransform.Scale.Vector.Z;

                defaultBoneTransformation.Rotation = new Quaternion(
                    skeleTransform.Rotation.Vector.X,
                    skeleTransform.Rotation.Vector.Y,
                    skeleTransform.Rotation.Vector.Z,
                    skeleTransform.Rotation.Vector.W);

                defaultBoneTransformation.Translation.X = skeleTransform.Position.Vector.X;
                defaultBoneTransformation.Translation.Y = skeleTransform.Position.Vector.Y;
                defaultBoneTransformation.Translation.Z = skeleTransform.Position.Vector.Z;

                return defaultBoneTransformation;
            }

            frame = (frame % FrameCount) % NumFramesPerBlock;

            if (frame >= FrameCount - 1)
            {
                NewBlendableTransform currentFrame = GetTransformOnSpecificBlockAndFrame(track,
                    block: GetBlock(frame), frame: (float)Math.Floor(frame));
                NewBlendableTransform nextFrame = GetTransformOnSpecificBlockAndFrame(track, block: 0, frame: 0);
                currentFrame = NewBlendableTransform.Lerp(frame % 1, currentFrame, nextFrame);
                return currentFrame;
            }
            // Regular frame
            else
            {
                NewBlendableTransform currentFrame = GetTransformOnSpecificBlockAndFrame(track,
                    block: GetBlock(frame), frame);
                return currentFrame;
            }
        }
    }
}
