using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour {
    private BoardManager BM;
    public Button[] spellButtons;
    public Dictionary<string, SpellCooldowns> spellCDs;
    private Dictionary<string, int> spellIndex;
    void Start() {
        BM = BoardManager.Instance;
        spellCDs = new Dictionary<string, SpellCooldowns>();
        spellIndex = new Dictionary<string, int>();

        for (int i = 0; i < spellButtons.Length; i++) {
            SpellCooldowns scd = new SpellCooldowns();
            scd.spellButton = spellButtons[i];
            spellIndex.Add(spellButtons[i].tag, i);
            spellCDs.Add(spellButtons[i].tag, scd);
        }
    }
    void Update() {
        foreach (SpellCooldowns scd in spellCDs.Values) {
            if (scd.inCooldown) {
                scd.timer -= Time.deltaTime;
                string spellName = scd.spellButton.tag;
                int index = spellIndex[spellName];
                Button butt = spellButtons[index];
                butt.GetComponentInChildren<Text>().text = ((int)scd.timer + 1) + "s\n-----\n" + (scd.resetTurn) + "Turn";
                butt.GetComponent<Image>().fillAmount = 1 - ((100 * scd.timer) / scd.cooldownTime) / 100f;
                bool isTimeOver = scd.timer <= 0f;
                bool EnoughTurnPast = BM.turnCount >= scd.resetTurn;
                if (isTimeOver || EnoughTurnPast) {
                    scd.inCooldown = false;
                    scd.timer = scd.cooldownTime;
                    buttonInteractable(spellName, true);
                }
            }
        }
    }
    public void startCooldown(string spellName) {
        SpellCooldowns scd = spellCDs[spellName];
        switch (spellName) {
            case "Stun":
                scd.cooldownTime = 90f;
                scd.resetTurn = BM.turnCount + 6;
                break;
            case "Upgrade":
                scd.cooldownTime = 160f;
                scd.resetTurn = BM.turnCount + 8;
                break;
            default:
                scd.cooldownTime = 3f;
                scd.resetTurn = 5;
                break;
        }
        scd.timer = scd.cooldownTime;
        buttonInteractable(spellName, false);
        scd.inCooldown = true;
    }
    private void buttonInteractable(string spellName, bool activate) {
        Button butt = spellButtons[spellIndex[spellName]];
        if (activate) {
            butt.interactable = true;
            butt.GetComponentInChildren<Text>().text = "";
            butt.GetComponent<Image>().fillAmount = 1;
            butt.transition = Selectable.Transition.ColorTint;
        } else {
            butt.transition = Selectable.Transition.None;
            butt.interactable = false;
        }
    }
    public void spellButtonsEnable(bool activate) {
        foreach (Button butt in spellButtons) {
            butt.interactable = activate;
        }
    }
}
public class SpellCooldowns {
    public int resetTurn;
    public float timer;
    public Button spellButton;
    public bool inCooldown;
    public float cooldownTime;
}
