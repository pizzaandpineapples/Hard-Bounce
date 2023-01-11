using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BackgroundReactiveTextColor : MonoBehaviour, IPointerEnterHandler
{
    private Image backgroudImage;
    private TextMeshProUGUI text;
    public Button button;
    private float grayScale;
    // Start is called before the first frame update
    void Start()
    {
        backgroudImage = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        
        grayScale = button.colors.normalColor.grayscale;
        UpdateTextColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //do your stuff when $$anonymous$$ghlighted
        grayScale = button.colors.highlightedColor.grayscale;
        UpdateTextColor();
    }
    public void UpdateTextColor()
    {
         //grayScale = backgroudImage.color.grayscale;
        Debug.Log(grayScale);
        text.color = grayScale < 0.5f ? Color.white : Color.black;
    }
}
