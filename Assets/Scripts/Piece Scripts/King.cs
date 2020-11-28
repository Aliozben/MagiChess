using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPieces {
    public bool everMoved;
    public override bool[,] possibleMove() {
        bool[,] r = new bool[8, 8];
        ChessPieces c;
        int i, j;
        //Top side
        i = currentX - 1;
        j = currentY + 1;
        if (currentY != 7) {
            for (int k = 0; k < 3; k++) {
                if (i >= 0 && i <= 7) {
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
                if (i >= 0 && i <= 7) {
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
        if (currentX < 7) {
            c = BoardManager.Instance.chessMen[currentX + 1, currentY];
            if (c == null)
                r[currentX + 1, currentY] = true;
            else if (isItWhite != c.isItWhite)
                r[currentX + 1, currentY] = true;
        }

        //Castling Move
        if (isItWhite) {
            if (!everMoved) {
                //Long Castling
                c = BoardManager.Instance.chessMen[0, 0];
                if (c != null) {
                    if (c.GetType() == typeof(Rook) && !c.GetComponent<Rook>().everMoved) {
                        bool isPathclear = true;
                        for (int k = 1; k < 4; k++) {
                            c = BoardManager.Instance.chessMen[k, 0];
                            if (c != null) {
                                isPathclear = false;
                                break;
                            }
                        }
                        if (isPathclear)
                            r[0, 0] = true;
                    }
                }
                //Short Castling
                c = BoardManager.Instance.chessMen[0, 7];
                if (c != null) {
                    if (c.GetType() == typeof(Rook) && !c.GetComponent<Rook>().everMoved) {
                        bool isPathclear = true;
                        for (int k = 6; k > 4; k--) {
                            c = BoardManager.Instance.chessMen[k, 0];
                            if (c != null) {
                                isPathclear = false;
                                break;
                            }
                        }
                        if (isPathclear)
                            r[7, 0] = true;
                    }
                }
            }
        }
        if (!isItWhite) {
            if (!everMoved) {
                //Long Castling
                c = BoardManager.Instance.chessMen[7, 0];
                if (c != null) {
                    if (c.GetType() == typeof(Rook) && !c.GetComponent<Rook>().everMoved) {
                        bool isPathclear = true;
                        for (int k = 1; k < 4; k++) {
                            c = BoardManager.Instance.chessMen[k, 7];
                            if (c != null) {
                                isPathclear = false;
                                break;
                            }
                        }
                        if (isPathclear)
                            r[0, 7] = true;
                    }
                }
                //Short Castling
                c = BoardManager.Instance.chessMen[7, 7];
                if (c != null) {
                    if (c.GetType() == typeof(Rook) && !c.GetComponent<Rook>().everMoved) {
                        bool isPathclear = true;
                        for (int k = 6; k > 4; k--) {
                            c = BoardManager.Instance.chessMen[k, 7];
                            if (c != null) {
                                isPathclear = false;
                                break;
                            }
                        }
                        if (isPathclear)
                            r[7, 7] = true;
                    }
                }
            }
        }

        return r;
    }
}
