using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPieces : MonoBehaviour {
    public int currentX { get; set; }
    public int currentY { get; set; }
    public bool isItWhite;

    public void setPosition (int x, int y) {
        currentX = x;
        currentY = y;
    }

    public virtual bool[,] possibleMove(){
        return new bool[8,8];
    }
}