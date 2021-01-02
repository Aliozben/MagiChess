using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCards : MonoBehaviour {
    public bool[,] possibleUpgrade() {

        bool[,] r = new bool[8, 8];
        ChessPieces c;

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                c = BoardManager.Instance.chessMen[i, j];
                if (c != null && c.isItWhite == BoardManager.Instance.playerIsWhite) {
                    if (c.GetType() != typeof(Queen) && c.GetType() != typeof(King))
                        r[i, j] = true;
                }
            }
        }
        return r;
    }
    public bool[,] possibleCover() {
        bool[,] r = new bool[8, 8];
        ChessPieces c;
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                c = BoardManager.Instance.chessMen[i, j];
                if (c == null)
                    r[i, j] = true;
            }
        }
        return r;
    }
    public bool[,] possibleStun() {

        bool[,] r = new bool[8, 8];
        ChessPieces c;

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                c = BoardManager.Instance.chessMen[i, j];
                if (c != null && c.isItWhite != BoardManager.Instance.playerIsWhite) {
                    r[i, j] = true;
                }
            }
        }
        return r;
    }
}
