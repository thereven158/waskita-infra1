using DG.Tweening;
using UnityEngine;

namespace Agate.Util
{
    public class RotationTween : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _endRotation;
        [SerializeField]
        private float _duration;
        void Start()
        {
            transform.DORotate(_endRotation, _duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}