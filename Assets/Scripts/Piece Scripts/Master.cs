using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : ChessPieces {
    public spelledPiece sp; 
    public override bool[,] possibleMove() {
        bool[,] r = new bool[8, 8];
        ChessPieces c;
        int i, j;

        //Right
        i = currentX;
        while (true) {
            i++;
            if (i > 7) {
                break;
            }
            c = BoardManager.Instance.chessMen[i, currentY];
            if (c == null)
                r[i, currentY] = true;
            else {
                if (c.isItWhite != isItWhite)
                    r[i, currentY] = true;
                break;
            }
        }
        //Left
        i = currentX;
        while (true) {
            i--;
            if (i < 0) {
                break;
            }
            c = BoardManager.Instance.chessMen[i, currentY];
            if (c == null)
                r[i, currentY] = true;
            else {
                if (c.isItWhite != isItWhite)
                    r[i, currentY] = true;
                break;
            }
        }
        //Up
        i = currentY;
        while (true) {
            i++;
            if (i > 7) {
                break;
            }
            c = BoardManager.Instance.chessMen[currentX, i];
            if (c == null)
                r[currentX, i] = true;
            else {
                if (c.isItWhite != isItWhite)
                    r[currentX, i] = true;
                break;
            }
        }
        //Down
        i = currentY;
        while (true) {
            i--;
            if (i < 0) {
                break;
            }
            c = BoardManager.Instance.chessMen[currentX, i];
            if (c == null)
                r[currentX, i] = true;
            else {
                if (c.isItWhite != isItWhite)
                    r[currentX, i] = true;
                break;
            }
        }
        i = currentX;
        j = currentY;
        while (true) {
            i--;
            j++;
            if (i < 0 || j > 7)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }
        //TopRight
        i = currentX;
        j = currentY;
        while (true) {
            i++;
            j++;
            if (i > 7 || j > 7)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }
        //BottomLeft
        i = currentX;
        j = currentY;
        while (true) {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }
        //BottomRight
        i = currentX;
        j = currentY;
        while (true) {
            i++;
            j--;
            if (i > 7 || j < 0)
                break;
            c = BoardManager.Instance.chessMen[i, j];
            if (c == null)
                r[i, j] = true;
            else {
                if (isItWhite != c.isItWhite)
                    r[i, j] = true;

                break;
            }
        }//UpLeft
        knightMove(currentX - 1, currentY + 2, ref r);
        //UpRight
        knightMove(currentX + 1, currentY + 2, ref r);
        //RightUp
        knightMove(currentX + 2, currentY + 1, ref r);
        //RightDown
        knightMove(currentX + 2, currentY - 1, ref r);
        //DownRight
        knightMove(currentX + 1, currentY - 2, ref r);
        //DownLeft
        knightMove(currentX - 1, currentY - 2, ref r);
        //LeftDown
        knightMove(currentX - 2, currentY - 1, ref r);
        //LeftUp
        knightMove(currentX - 2, currentY + 1, ref r);
        return r;
    }
    public void knightMove(int x, int y, ref bool[,] r) {

        ChessPieces c;
        if (x >= 0 && x < 8 && y >= 0 && y < 8) {
            c = BoardManager.Instance.chessMen[x, y];
            if (c == null)
                r[x, y] = true;
            else if (isItWhite != c.isItWhite)
                r[x, y] = true;
        }
    }
}
