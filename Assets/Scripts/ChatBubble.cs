using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBubble : MonoBehaviour {
    private Image background;
    private Image icon;
    private TextMeshProUGUI textBox;
    private void Awake() {
        background = transform.Find("Background").GetComponent<Image>();
        icon = transform.Find("Icon").GetComponent<Image>();
        textBox = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    private void Start() {
        setup("seee");
    }
    private void setup(string _text) {
        textBox.SetText(_text);
        textBox.ForceMeshUpdate();
        Vector2 textSize = textBox.GetRenderedValues(false);
        RectTransform rtBackground = background.GetComponent<RectTransform>();
        float lenght = textSize.x + 10f;
        rtBackground.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lenght);
        rtBackground.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, lenght / 2);
        //rtBackground.anchoredPosition.x -= (int)lenght / 2;

    }
}
