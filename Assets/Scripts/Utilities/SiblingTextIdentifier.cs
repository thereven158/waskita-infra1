using TMPro;
using UnityEngine;

namespace Agate.Util
{
    public class SiblingTextIdentifier : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text = default;

        private void OnEnable()
        {
            _text.text = "" + (char)(0x41 + transform.GetSiblingIndex());
        }
    }
}