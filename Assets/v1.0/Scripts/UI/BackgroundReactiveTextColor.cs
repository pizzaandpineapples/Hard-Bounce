using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BackgroundReactiveTextColor : MonoBehaviour
{
    private Image backgroudImage;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        backgroudImage = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        UpdateTextColor();
    }

    public void UpdateTextColor()
    {
        var grayScale = backgroudImage.color.grayscale;
        text.color = grayScale < 0.5f ? Color.black : Color.white;
    }
}
