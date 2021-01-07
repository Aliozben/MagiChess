using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour {
    private static TooltipSystem instance;
    public Tooltip tooltip;
    public void Awake() {
        instance = this;
    }
    public static void showTooltip(string header, string cooldown, string content) {
        instance.tooltip.SetText(header, cooldown, content);
        instance.tooltip.gameObject.SetActive(true);

    }
    public static void hideTooltip() {
        instance.tooltip.gameObject.SetActive(false);
    }
}
