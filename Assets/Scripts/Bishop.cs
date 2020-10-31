using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPieces
{
    public override bool[,] possibleMove() {
        bool[,] r = new bool[8, 8];

        ChessPieces c;
        int i, j;
        //TopLeft
        i = currentX;
        j = currentY; 
        while(true) {
            i--;
            j++;
            if(i < 0 || j > 7)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if(c == null)
                r[i, j] = true;
            else {
                if(isItWhite!=c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }
        //TopRight
        i = currentX;
        j = currentY;
        while(true) {
            i++;
            j++;
            if(i > 7 || j > 7)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if(c == null)
                r[i, j] = true;
            else {
                if(isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }
        //BottomLeft
        i = currentX;
        j = currentY;
        while(true) {
            i--;
            j--;
            if(i < 0 || j < 0)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if(c == null)
                r[i, j] = true;
            else {
                if(isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }
        //BottomRight
        i = currentX;
        j = currentY;
        while(true) {
            i++;
            j--;
            if(i > 7 || j < 0)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if(c == null)
                r[i, j] = true;
            else {
                if(isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }

        return r;
    }
}
