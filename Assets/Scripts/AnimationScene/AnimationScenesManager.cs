using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.WaskitaInfra1.Animations
{
    public class AnimationScenesManager : MonoBehaviour
    {
        public Action<AnimationSceneControl> OnStop;
        public Action<AnimationSceneControl> OnStart;
        private Action OnPlayingAnimStop;
        [SerializeField]
        private List<AnimationSceneControl> _animationScenes = default;
        private AnimationSceneControl _playingAnim;
        private Dictionary<string, AnimationSceneControl> _animationDictionary;

        private AnimationSceneControl PlayingAnim
        {
            get => _playingAnim;
            set
            {
                if (_playingAnim)
                {
                    _playingAnim.OnStart = null;
                    _playingAnim.OnFinish = null;
                    _playingAnim.gameObject.SetActive(false);
                }

                _playingAnim = value;
            }
        }
        private void Awake()
        {
            _animationDictionary = new Dictionary<string, AnimationSceneControl>();
            foreach (AnimationSceneControl anim in _animationScenes)
                _animationDictionary.Add(anim.UniqueID, anim);
        }

        private void OnStopAnimation(AnimationSceneControl animControl)
        {
            OnStop?.Invoke(animControl);
        }
        private void OnStartAnimation(AnimationSceneControl animControl)
        {
            OnStart?.Invoke(animControl);
        }

        public void ResumeAnimation()
        {
            if (!PlayingAnim) return;
            PlayingAnim.Play();
        }
        public void PlayAnimation(AnimationSceneControl animControl, Action OnStart = null, Action OnStop = null)
        {
            AnimationSceneControl correspondingAnim = GetAnimation(animControl);
            if (PlayingAnim)
                StopAnimation();
            PlayingAnim = correspondingAnim;
            OnPlayingAnimStop = OnStop;
            PlayingAnim.OnStart = () => OnStartAnimation(PlayingAnim);
            PlayingAnim.OnStart += OnStart;
            PlayingAnim.OnFinish = () => StopAnimation(true);
            PlayingAnim.OnAnimationStart();
            PlayingAnim.gameObject.SetActive(true);
            PlayingAnim.Play();
        }
        public void StopAnimation(bool keepActive = true)
        {
            if (!PlayingAnim) return;
            OnPlayingAnimStop?.Invoke();
            OnStopAnimation(PlayingAnim);
            OnPlayingAnimStop = null;
            PauseAnimation();
            if (keepActive) return;
            PlayingAnim = null;
        }

        public void PauseAnimation()
        {
            _playingAnim.Pause();
        }
        private AnimationSceneControl GetAnimation(AnimationSceneControl animControl)
        {
            if (!_animationDictionary.TryGetValue(animControl.UniqueID, out AnimationSceneControl corspAnim))
                throw new KeyNotFoundException();
            return corspAnim;
        }
    }

}
