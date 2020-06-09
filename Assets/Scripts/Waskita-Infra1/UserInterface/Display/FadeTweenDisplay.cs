using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Display
{
    [RequireComponent(typeof(Image))]
    public class FadeTweenDisplay : DelayedDisplayBehavior
    {
        [SerializeField]
        private float _fadeDuration = default;
        [SerializeField]
        private Image _image = default;
        public override bool IsOpen => gameObject.activeSelf;

        private void OnValidate()
        {
            _image = _image ?? GetComponent<Image>();
        }

        public override void Close()
        {
            if (!IsOpen) return;

            _image.DOFade(0, _fadeDuration);
            DOVirtual.DelayedCall(_fadeDuration, () => gameObject.SetActive(false));
        }

        public override void Init()
        {
            _image.color = Color.clear;
            gameObject.SetActive(false);
        }

        public override void Open(Action onFinish = null)
        {
            if (IsOpen) return;
            gameObject.SetActive(true);
            _image.DOFade(1, _fadeDuration);
            void OnFinish()
            {
                onFinish?.Invoke();
            }
            DOVirtual.DelayedCall(_fadeDuration, OnFinish);
        }
    }
}