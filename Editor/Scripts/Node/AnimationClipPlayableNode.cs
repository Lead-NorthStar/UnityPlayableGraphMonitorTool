﻿using System.Text;
using UnityEngine.Animations;
using UnityEngine.Playables;
#if UNITY_2021_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEditor.UIElements;
#endif

namespace GBG.PlayableGraphMonitor.Editor.Node
{
    public class AnimationClipPlayableNode : PlayableNode
    {
        private readonly ProgressBar _progressBar;


        public AnimationClipPlayableNode(Playable playable) : base(playable)
        {
            _progressBar = new ProgressBar();
            // insert between title and port container
            titleContainer.parent.Insert(1, _progressBar);
        }

        public override void Update()
        {
            if (Playable.IsValid())
            {
                var clipPlayable = (AnimationClipPlayable)Playable;
                var clip = clipPlayable.GetAnimationClip();
                var duration = clip ? clip.length : float.PositiveInfinity;
                var progress = (float)(Playable.GetTime() / duration) % 1.0f * 100;
                _progressBar.SetValueWithoutNotify(progress);
                _progressBar.MarkDirtyRepaint();
            }

            base.Update();
        }


        protected override void AppendStateDescriptions(StringBuilder descBuilder)
        {
            base.AppendStateDescriptions(descBuilder);

            if (Playable.IsValid())
            {
                var clipPlayable = (AnimationClipPlayable)Playable;
                descBuilder.Append("ApplyFootIK: ").AppendLine(clipPlayable.GetApplyFootIK().ToString())
                    .Append("ApplyPlayableIK: ").AppendLine(clipPlayable.GetApplyPlayableIK().ToString())
                    .AppendLine();

                var clip = clipPlayable.GetAnimationClip();
                descBuilder.Append("Clip: ").AppendLine(clip ? clip.name : "None");
                if (clip)
                {
                    descBuilder.Append("IsLooping: ").AppendLine(clip.isLooping.ToString())
                        .Append("Length: ").Append(clip.length.ToString("F3")).AppendLine("(s)");
                }
            }
        }
    }
}