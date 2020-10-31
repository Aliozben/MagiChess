using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPieces {


    public override bool[,] possibleMove() {
        bool[,] r = new bool[8, 8];
        ChessPieces c, c2;
        int[] passant = BoardManager.Instance.passantMove;
        //White move
        if (isItWhite) {
            //Diagonal Right
            if (currentX != 7 && currentY != 7) {

                if (passant[0] == currentX + 1 && passant[1] == currentY + 1)
                    r[currentX + 1, currentY + 1] = true;
                c = BoardManager.Instance.chessMen[currentX + 1, currentY + 1];
                if (c != null && !c.isItWhite)
                    r[currentX + 1, currentY + 1] = true;

            }
            //Diagonal Left
            if (currentX != 0 && currentY != 7) {
                if (passant[0] == currentX - 1 && passant[1] == currentY + 1)
                    r[currentX - 1, currentY + 1] = true;
                c = BoardManager.Instance.chessMen[currentX - 1, currentY + 1];
                if (c != null && !c.isItWhite)
                    r[currentX - 1, currentY + 1] = true;

            }
            //Midlle
            if (currentY <= 7) {

                c = BoardManager.Instance.chessMen[currentX, currentY + 1];
                if (c == null)
                    r[currentX, currentY + 1] = true;
            }

            //Middle on first move 
            if (currentY == 1) {
                c = BoardManager.Instance.chessMen[currentX, currentY + 1];
                c2 = BoardManager.Instance.chessMen[currentX, currentY + 2];
                if (c == null & c2 == null)
                    r[currentX, currentY + 2] = true;
            }
        }
        // Blue move
        else {
            //Diagonal Right
            if (currentX != 7 && currentY != 0) {
                if (passant[0] == currentX + 1 && passant[1] == currentY - 1)
                    r[currentX + 1, currentY - 1] = true;
                c = BoardManager.Instance.chessMen[currentX + 1, currentY - 1];
                if (c != null && c.isItWhite)
                    r[currentX + 1, currentY - 1] = true;

            }
            //Diagonal Left
            if (currentX != 0 && currentY != 0) {
                if (passant[0] == currentX + 1 && passant[1] == currentY + 1)
                    r[currentX - 1, currentY - 1] = true;
                c = BoardManager.Instance.chessMen[currentX - 1, currentY - 1];
                if (c != null && c.isItWhite)
                    r[currentX - 1, currentY - 1] = true;

            }
            //Midlle
            if (currentY != 0) {

                c = BoardManager.Instance.chessMen[currentX, currentY - 1];
                if (c == null)
                    r[currentX, currentY - 1] = true;
            }
            //Middle on first move 
            if (currentY == 6) {
                c = BoardManager.Instance.chessMen[currentX, currentY - 1];
                c2 = BoardManager.Instance.chessMen[currentX, currentY - 2];
                if (c == null & c2 == null)
                    r[currentX, currentY - 2] = true;
            }
        }
        return r;
    }
}
