using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour
{
    public static Tooltip singleton;
    TextMeshProUGUI text;
    RectTransform backgroundRectTransform;
    RectTransform rectTransform;
    [SerializeField]
    RectTransform CanvasTransform;
    private void Awake()
    {
        singleton = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();
        
        CanvasTransform = transform.parent.GetComponent<RectTransform>();

        HideToolTip();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 anchoredPos = mousePos / CanvasTransform.localScale.x;
        if(anchoredPos.x + backgroundRectTransform.rect.width > CanvasTransform.rect.width)
        {
            anchoredPos.x = CanvasTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if(anchoredPos.y + backgroundRectTransform.rect.height > CanvasTransform.rect.height)
        {
            anchoredPos.y = CanvasTransform.rect.height - backgroundRectTransform.rect.height;
        }
        rectTransform.anchoredPosition = anchoredPos;

    }

    void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        SetText(tooltipString);
    }

    void SetText(string tooltipString)
    {
        text.SetText(tooltipString);
        float textPaddingSize = 2;
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        Vector2 backgroundSize = textSize + paddingSize;
        backgroundRectTransform.sizeDelta = backgroundSize;
    }


    void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void HideSingletonTooltip()
    {
        singleton.HideToolTip();
    }

    public static void ShowSingletonTooltip(string tooltipString)
    {
        singleton.ShowTooltip(tooltipString);
    }
}
