using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipSource : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 10)]
    public string text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowSingletonTooltip(text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideSingletonTooltip();
    }

    private void OnMouseEnter()
    {
        
    }

    private void OnMouseExit()
    {
        
    }
}
