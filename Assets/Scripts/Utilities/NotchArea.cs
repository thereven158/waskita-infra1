using UnityEngine;

public class NotchArea : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Screen.safeArea.y);
    }
}
