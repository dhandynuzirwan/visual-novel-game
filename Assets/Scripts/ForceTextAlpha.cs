using UnityEngine;
using TMPro;

public class ForceTextAlpha : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Awake() { text = GetComponent<TextMeshProUGUI>(); }
    void Update()
    {
        if (text.color.a < 1.0f)
        {
            Color newColor = text.color;
            newColor.a = 1.0f;
            text.color = newColor;
        }
    }
}