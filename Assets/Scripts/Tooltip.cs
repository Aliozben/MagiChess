using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour {
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI contentText;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public void SetText(string header, string cooldown, string content) {
        headerText.text = "Spell Name: " + header;
        cooldownText.text = "Cooldown: " + cooldown;
        contentText.text = "\nDescription: " + content;
        int cooldownLength = cooldownText.text.Length;
        int contentLength = contentText.text.Length;
        layoutElement.enabled = (cooldownLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }
    private void Update() {
        Vector2 mousePos = Input.mousePosition;
        transform.position = mousePos;
    }
}
