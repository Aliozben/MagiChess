using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }
    public ChessPieces[,] chessMen { set; get; }
    private ChessPieces selectedPiece;
    private const float TILE_SIZE = 1f;
    private const float TILE_OFFSET = .5f;
    private int selectionX = -1;
    private int selectionY = -1;
    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessmen;
    public int[] passantMove { get; set; }
    public bool isWhiteTurn = true;
    public bool playerIsWhite;
    private Client client;
    private void Start() {
        client = FindObjectOfType<Client>();
        playerIsWhite = client.isHost;
        Instance = this;
        spawnAllChessmen();
    }
    private void Update() {
        DrawChessboard();
        updateSelection();
        if (Input.GetMouseButtonDown(0)) {
            if (selectionX >= 0 && selectionY >= 0) {
                if (selectedPiece == null) {
                    Debug.Log("selec1");
                    if (chessMen[selectionX, selectionY] == null) {
                        return;
                    }
                    if (chessMen[selectionX, selectionY].isItWhite != isWhiteTurn || playerIsWhite != isWhiteTurn) {
                        return;
                    }
                    selectChessPiece(selectionX, selectionY);
                } else {
                    sendMove(selectedPiece.currentX, selectedPiece.currentY, selectionX, selectionY);
                }
            }
        }
    }
    private void selectChessPiece(int x, int y) {
        allowedMoves = chessMen[x, y].possibleMove();
        selectedPiece = chessMen[x, y];
        selectedPiece.GetComponent<Outline>().enabled = true;
        BoardHighlights.Instance.highlightAllowedMoves(allowedMoves);
    }

    private void enPassantMove(int x, int y, ChessPieces c) {
        if (x == passantMove[0] && y == passantMove[1]) {
            if (isWhiteTurn)
                c = chessMen[x, y - 1];
            else
                c = chessMen[x, y - 1];
            activeChessmen.Remove(c.gameObject);
            Destroy(c.gameObject);
        }
        for (int i = 0; i < passantMove.Length; i++) {
            passantMove[i] = -1;
        }
    }

    private void castlingMove(int x, int y, ChessPieces c) {
        if (y == 0) {
            if (x == 0)
                moveChessPiece(x, y, 1, 0);
            else if (x == 7)
                moveChessPiece(x, y, 6, 0);
            selectedPiece = chessMen[4, 0];
        } else if (y == 7) {
            if (x == 0)
                moveChessPiece(x, y, 1, 7);
            else if (x == 7)
                moveChessPiece(x, y, 6, 7);
            selectedPiece = chessMen[4, 7];
        }
        // For fixing the issue that playing in a row
        isWhiteTurn = !isWhiteTurn;
    }
    private void sendMove(int selectionX, int selectionY, int x, int y) {
        string msg = "CMOV|";
        msg += selectedPiece.currentX.ToString() + "|";
        msg += selectedPiece.currentY.ToString() + "|";
        msg += x.ToString() + "|";
        msg += y.ToString();
        client.send(msg);
    }
    public void moveChessPiece(int selectionX, int selectionY, int x, int y) {
        selectChessPiece(selectionX, selectionY);
        Debug.Log(selectedPiece.GetType());
        if (allowedMoves[x, y]) {
            ChessPieces c = chessMen[x, y];
            if (c != null && c.isItWhite != isWhiteTurn) {
                if (c.GetType() == typeof(King)) {
                    gameOver();
                }
                activeChessmen.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            enPassantMove(x, y, c);

            if (selectedPiece.GetType() == typeof(Pawn)) {
                if (y == 7) {
                    activeChessmen.Remove(selectedPiece.gameObject);
                    Destroy(selectedPiece.gameObject);
                    spawnChessman(1, x, y);
                    selectedPiece = chessMen[x, y];
                } else if (y == 0) {
                    activeChessmen.Remove(selectedPiece.gameObject);
                    Destroy(selectedPiece.gameObject);
                    spawnChessman(7, x, y);
                    selectedPiece = chessMen[x, y];
                }
                if (selectedPiece.currentY == 1 && y == 3) {
                    passantMove[0] = x;
                    passantMove[1] = y - 1;
                } else if (selectedPiece.currentY == 6 && y == 4) {
                    passantMove[0] = x;
                    passantMove[1] = y + 1;
                }
            }
            if (selectedPiece.GetType() == typeof(King) && c != null)
                castlingMove(x, y, c);
            chessMen[selectedPiece.currentX, selectedPiece.currentY] = null;
            selectedPiece.transform.position = getTileCenter(x, y);
            selectedPiece.setPosition(x, y);

            if (selectedPiece.GetType() == typeof(King))
                selectedPiece.GetComponent<King>().everMoved = true;
            else if (selectedPiece.GetType() == typeof(Rook))
                selectedPiece.GetComponent<Rook>().everMoved = true;
            chessMen[x, y] = selectedPiece;
            isWhiteTurn = !isWhiteTurn;
        }
        selectedPiece.GetComponent<Outline>().enabled = false;
        BoardHighlights.Instance.hideHighlights();
        selectedPiece = null;
    }

    private void gameOver() {
        if (isWhiteTurn)
            Debug.Log("White won");
        else
            Debug.Log("Black Won");

        restartGame();
    }

    private void restartGame() {
        foreach (GameObject go in activeChessmen)
            Destroy(go);
        isWhiteTurn = true;
        BoardHighlights.Instance.hideHighlights();
        spawnAllChessmen();
    }

    private Vector3 getTileCenter(int x, int y) {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    private void spawnAllChessmen() {
        activeChessmen = new List<GameObject>();
        chessMen = new ChessPieces[8, 8];
        passantMove = new int[2] { -1, -1 };

        //WHITE TEAM
        //King
        spawnChessman(0, 4, 0);
        //Queen
        spawnChessman(1, 3, 0);
        //Rook
        spawnChessman(2, 0, 0);
        spawnChessman(2, 7, 0);
        //Bishop
        spawnChessman(3, 2, 0);
        spawnChessman(3, 5, 0);
        //Horse
        spawnChessman(4, 6, 0);
        spawnChessman(4, 1, 0);

        //BLACK TEAM
        //King
        spawnChessman(6, 4, 7);
        //Queen
        spawnChessman(7, 3, 7);
        //Rook
        spawnChessman(8, 0, 7);
        spawnChessman(8, 7, 7);
        //Bishop
        spawnChessman(9, 2, 7);
        spawnChessman(9, 5, 7);
        //Horse
        spawnChessman(10, 6, 7);
        spawnChessman(10, 1, 7);
        //Pawns
        for (int i = 0; i < 8; i++) {
            spawnChessman(5, i, 1);
            spawnChessman(11, i, 6);
        }
    }
    private void updateSelection() {
        if (!Camera.main)
            return;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("chessPlane"))) {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        } else {
            selectionX = -1;
            selectionY = -1;
        }
    }
    private void spawnChessman(int index, int x, int y) {
        GameObject go = Instantiate(chessmanPrefabs[index], getTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        chessMen[x, y] = go.GetComponent<ChessPieces>();
        chessMen[x, y].setPosition(x, y);
        activeChessmen.Add(go);
    }
    private void DrawChessboard() {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heigthLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++) {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++) {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heigthLine);
            }
        }
        if (selectionX >= 0 && selectionY >= 0) {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

        }
    }
}