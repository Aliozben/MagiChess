using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour {
    BoardManager BM;
    Client client;

    public GameObject game;
    public RectTransform game1;
    public static float bubblePos = 50f;

    private void Start() {
        BM = BoardManager.Instance;

        client = FindObjectOfType<Client>();
    }
    public void sendChatBubble() {
        string text = GameObject.Find("BubbleText").transform.GetChild(2).GetComponent<Text>().text;
        RectTransform transform = game.GetComponent<RectTransform>();
        string msg = "CCHAT|";
        msg += BM.playerIsWhite + "|";
        msg += text + "|";
        client.send(msg);
    }
    public void commandUpgrade(int upgrade) {
        string msg = "CUPGR|";
        for (int i = 0; i <= 7; i++) {
            if (BM.chessMen[i, 7].GetType() == typeof(Pawn)) {
                msg += i.ToString() + "|";
                msg += "7|";
                break;
            } else if (BM.chessMen[i, 0].GetType() == typeof(Pawn)) {
                msg += i.ToString() + "|";
                msg += "0|";
                break;
            }
        }
        msg += upgrade.ToString() + "|";
        client.send(msg);
    }
    public void sendMove(int currentX, int currentY, int x, int y) {
        string msg = "CMOV|";
        msg += currentX.ToString() + "|";
        msg += currentY.ToString() + "|";
        msg += x.ToString() + "|";
        msg += y.ToString();
        client.send(msg);
    }
    public void sendSpell(string spellName, int x, int y) {
        string msg = "CSPELL|";
        msg += spellName + "|";
        msg += x.ToString() + "|";
        msg += y.ToString() + "|";
        client.send(msg);
    }
}
