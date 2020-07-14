using System;
using UnityEngine;

namespace A3.AnimationScene
{
    /// <summary>
    /// Control class that represent animation scene. 
    /// Animation Scene refer to animation that played as a video with Start and finish trigger that attached through animation event
    ///
    /// by default animation scene are inactive game object that activated when plays.
    /// </summary>
    [RequireComponent(typeof(UniqueId.UniqueId))]
    public class AnimationSceneControl : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private UniqueId.UniqueId _id = default;

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
        public string UniqueId => _id.uniqueId;

        /// <summary>
        /// identifier if animation is playing
        /// </summary>
        public bool IsPlaying => _animator.enabled && gameObject.activeSelf;

        /// <summary>
        /// animation event function that should be called at the end of animation
        /// </summary>
        public void OnAnimationFinish()
            => OnFinish?.Invoke();

        /// <summary>
        /// animation event function that should be called at the start of animation
        /// </summary>
        public void OnAnimationStart()
            => OnStart?.Invoke();

        /// <summary>
        /// shorthand for ToggleAnimation(false)
        /// </summary>
        public void Pause() 
            => ToggleAnimation(false);

        /// <summary>
        /// Play Animation
        /// </summary>
        public void Play()
        {
            gameObject.SetActive(true);
            ToggleAnimation(true);
        }

        /// <summary>
        /// Toggle Animation state (play / pause) animator enabled state
        /// </summary>
        /// <param name="toggle">target state true for play, false for pause </param>
        public void ToggleAnimation(bool toggle) 
            => _animator.enabled = toggle;

        /// <summary>
        /// Reset state by cleaning trigger and setting game object to false (resetting unity animation state)
        /// </summary>
        public void ResetState()
        {
            OnStart = null;
            OnFinish = null;
            gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            _id = _id ? _id : GetComponent<UniqueId.UniqueId>();
            _animator = _animator ? _animator : GetComponent<Animator>();
        }
    }
}