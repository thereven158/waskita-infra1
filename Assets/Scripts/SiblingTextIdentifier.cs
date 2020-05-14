using TMPro;
using UnityEngine;

public class SiblingTextIdentifier : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    private void Awake()
    {
        _text.text = "" + (char)( 0x41 + transform.GetSiblingIndex());
    }
}