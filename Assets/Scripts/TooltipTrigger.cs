using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string header;
    public string cooldown;
    public string content;
    public void OnPointerEnter(PointerEventData eventData) {
        TooltipSystem.showTooltip(header,cooldown,content);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipSystem.hideTooltip();
    }

}
