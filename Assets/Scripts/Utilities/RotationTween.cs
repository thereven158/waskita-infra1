using DG.Tweening;
using UnityEngine;

namespace Agate.Util
{
    public class RotationTween : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _endRotation = default;
        [SerializeField]
        private float _duration = default;
        void Start()
        {
            transform.DORotate(_endRotation, _duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}