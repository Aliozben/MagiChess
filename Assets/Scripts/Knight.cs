using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPieces {
    public override bool[,] possibleMove() {
        bool[,] r = new bool[8, 8];
        //UpLeft
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
        if(x >= 0 && x < 8 && y >= 0 && y < 8) {
            c = BoardManager.Instance.chessMen[x, y];
            if(c == null)
                r[x, y] = true;
            else if(isItWhite != c.isItWhite)
                r[x, y] = true;
        }
    }
}
