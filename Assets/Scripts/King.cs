using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPieces {
    public override bool[,] possibleMove() {
        bool[,] r = new bool[8, 8];
        ChessPieces c;
        int i, j;
        //Top side
        i = currentX - 1;
        j = currentY + 1;
        if (currentY != 7) {
            for (int k = 0; k < 3; k++) {
                if (i >= 0 || i <= 7) {
                    c = BoardManager.Instance.chessMen[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isItWhite != c.isItWhite)
                        r[i, j] = true;

                }
                i++;
            }
        }
        //Down side
        i = currentX - 1;
        j = currentY - 1;
        if (currentY != 0) {
            for (int k = 0; k < 3; k++) {
                if (i >= 0 || i <= 7) {
                    c = BoardManager.Instance.chessMen[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isItWhite != c.isItWhite)
                        r[i, j] = true;

                }
                i++;
            }
        }
        // Left side
        if (currentX > 0) {
            c = BoardManager.Instance.chessMen[currentX - 1, currentY];
            if (c == null)
                r[currentX - 1, currentY] = true;
            else if (isItWhite != c.isItWhite)
                r[currentX - 1, currentY] = true;
        }

        //Right side 
        if (currentX < 8) {
            c = BoardManager.Instance.chessMen[currentX + 1, currentY];
            if (c == null)
                r[currentX + 1, currentY] = true;
            else if (isItWhite != c.isItWhite)
                r[currentX + 1, currentY] = true;
        }

        return r;
    }
}
