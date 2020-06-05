using A3.UniqueId;
using System;
using UnityEngine;

namespace Agate.WaskitaInfra1.Animations
{
    [RequireComponent(typeof(UniqueId))]
    public class AnimationSceneControl : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private UniqueId _id = default;
        [HideInInspector]
        [SerializeField]
        private Animator _animator = default;
        public Action OnFinish;
        public Action OnStart;
        public string UniqueID => _id.uniqueId;
        public bool IsPlaying => _animator.enabled;
        private void OnValidate()
        {
            _id = _id ?? GetComponent<UniqueId>();
            _animator = _animator ?? GetComponent<Animator>();
        }
        public void OnAnimationFinish()
        {
            OnFinish?.Invoke();
        }
        public void OnAnimationStart()
        {
            OnStart?.Invoke();
        }
        public void Pause()
        {
            _animator.enabled = false;
        }
        public void Play()
        {
            _animator.enabled = true;
        }
    }
}

