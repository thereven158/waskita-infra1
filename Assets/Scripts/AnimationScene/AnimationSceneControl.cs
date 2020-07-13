using A3.UniqueId;
using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Agate.WaskitaInfra1.Animations
{
    /// <summary>
    /// Control class that represent animation scene. 
    /// Animation Scene refer to animation that played as a video with Start and finish trigger that attached through animation event
    /// </summary>
    [RequireComponent(typeof(UniqueId))]
    public class AnimationSceneControl : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private UniqueId _id = default;
        [HideInInspector]
        [SerializeField]
        private Animator _animator = default;
        /// <summary>
        /// Event that triggered when animation finished
        /// </summary>
        public Action OnFinish;
        /// <summary>
        /// event that triggered when animation started
        /// </summary>
        public Action OnStart;
        /// <summary>
        /// Unique Identifier for animation scene achieved through guid
        /// </summary>
        public string UniqueID => _id.uniqueId;
        /// <summary>
        /// identifier if animation is playing
        /// </summary>
        public bool IsPlaying => _animator.enabled;
        private void OnValidate()
        {
            _id = _id ?? GetComponent<UniqueId>();
            _animator = _animator ?? GetComponent<Animator>();
        }
        /// <summary>
        /// animation event function that should be called at the end of animation
        /// </summary>
        public void OnAnimationFinish()
        {
            OnFinish?.Invoke();
        }
        /// <summary>
        /// animation event function that should be called at the start of animation
        /// </summary>
        public void OnAnimationStart()
        {
            OnStart?.Invoke();
        }
        /// <summary>
        /// shorthand for ToggleAnimation(false)
        /// </summary>
        public void Pause()
        {
            ToggleAnimation(false);
        }
        /// <summary>
        /// shorthand for ToggleAnimation(true)
        /// </summary>
        public void Play()
        {
            ToggleAnimation(true);
        }
        /// <summary>
        /// Toggle Animation state (play / pause) animator enabled state
        /// </summary>
        /// <param name="toggle">target state true for play, false for pause </param>
        public void ToggleAnimation(bool toggle)
        {
            _animator.enabled = toggle;
        }
    }
}

