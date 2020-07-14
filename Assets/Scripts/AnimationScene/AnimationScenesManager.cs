using System;
using System.Collections.Generic;
using UnityEngine;

namespace A3.AnimationScene
{
    /// <summary>
    /// Animation Scenes Player / Manager manage playing sequence / handling for animation scene controls
    /// </summary>
    public class AnimationScenesManager : MonoBehaviour
    {
        
        [SerializeField]
        private List<AnimationSceneControl> _animationScenes = default;
        
        // Use ActiveAnim Property for most part
        private AnimationSceneControl _activeAnim;
        private Dictionary<string, AnimationSceneControl> _animationDictionary;
        private bool _isPlaying;
        
        
        public Action<AnimationSceneControl> OnStop;
        public Action<AnimationSceneControl> OnStart;
        private Action _onActiveAnimStop;

        #region Private Methods

        private AnimationSceneControl ActiveAnim
        {
            get => _activeAnim;
            set
            {
                if(_activeAnim) _activeAnim.ResetState();
                _activeAnim = value;
            }
        }
        
        private void Awake()
        {
            _animationDictionary = new Dictionary<string, AnimationSceneControl>();
            foreach (AnimationSceneControl anim in _animationScenes)
                _animationDictionary.Add(anim.UniqueId, anim);
        }

        private AnimationSceneControl GetAnimation(AnimationSceneControl animControl)
        {
            if (!_animationDictionary.TryGetValue(animControl.UniqueId, out AnimationSceneControl corspAnim))
                throw new KeyNotFoundException();
            return corspAnim;
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Play an registered animation within the system
        /// </summary>
        /// <param name="animControl">animation to play</param>
        /// <param name="onStart">action that invoked when animation starts</param>
        /// <param name="onStop">action that invoked when animation stop</param>
        public void PlayAnimation(AnimationSceneControl animControl, Action onStart = null, Action onStop = null)
        {
            AnimationSceneControl correspondingAnim = GetAnimation(animControl);
            StopAnimation();
            _isPlaying = true;
            ActiveAnim = correspondingAnim;
            _onActiveAnimStop = onStop;
            ActiveAnim.OnStart = () => OnStart?.Invoke(ActiveAnim); 
            ActiveAnim.OnStart += onStart;
            ActiveAnim.OnFinish = () => StopAnimation();
            ActiveAnim.Play();
        }
        
        /// <summary>
        /// Stop active Animation
        /// </summary>
        /// <param name="keepActive">keep animation object active</param>
        public void StopAnimation(bool keepActive = true)
        {
            if (!ActiveAnim) return;
            if (_isPlaying) OnStop?.Invoke(ActiveAnim);
            _onActiveAnimStop?.Invoke();
            _onActiveAnimStop = null;
            PauseAnimation();
            _isPlaying = false;
            if (keepActive) return;
            ActiveAnim = null;
        }

        /// <summary>
        /// Pause active Animation
        /// </summary>
        public void PauseAnimation() 
            => _activeAnim.Pause();

        /// <summary>
        /// Resume Paused active animation
        /// </summary>
        public void ResumeAnimation()
        {
            if (!ActiveAnim) return;
            ActiveAnim.Play();
        }
        
        #endregion

    }

}
