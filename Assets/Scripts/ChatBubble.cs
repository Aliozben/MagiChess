using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBubble : MonoBehaviour {

    public static void createChatBubble(RectTransform parent, RectTransform bubble, bool playerIsWhite, string text) {
        RectTransform chatbubble = Instantiate(bubble, parent);
        float posY = Command.bubblePos;
        chatbubble.anchoredPosition = new Vector2(-150f, posY);
        chatbubble.GetComponent<ChatBubble>().setup(playerIsWhite, text);
        posY += 30f;
        Destroy(chatbubble.gameObject, 4f);
        if (posY >= 320f)
            posY = 50f;
        Command.bubblePos = posY;
    }
    private static List<RectTransform> bubbles;
    public static RectTransform bubble;
    [SerializeField] private Sprite blackIcon;
    [SerializeField] private Sprite whiteIcon;
    private Image background;
    private Image icon;
    private TextMeshProUGUI textBox;
    private void Awake() {
        background = transform.Find("Background").GetComponent<Image>();
        icon = transform.GetChild(0).Find("Icon").GetComponent<Image>();
        textBox = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    private void setup(bool playerIsWhite, string _text) {
        textBox.SetText(_text);
        textBox.ForceMeshUpdate();
        Vector2 textSize = textBox.GetRenderedValues(false);
        RectTransform rtBackground = background.GetComponent<RectTransform>();
        float lenght = textSize.x + 10f;
        rtBackground.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, lenght + 30f);
        icon.sprite = getIconSprite(playerIsWhite);
    }
    private Sprite getIconSprite(bool playerIsWhite) {
        switch (playerIsWhite) {
            default:
            case false: return blackIcon;
            case true: return whiteIcon;
        }
    }
}
