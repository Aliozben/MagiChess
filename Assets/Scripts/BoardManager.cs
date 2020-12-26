using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

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
    private Client client;
    public int[] passantMove { get; set; }
    public bool isWhiteTurn = true;
    public bool playerIsWhite;
    public GameObject upgradePanel;
    public int turnCount = 1;
    Command com;
    SpellCards spells;
    private bool isSpellMove;
    private string spellName;
    private SpellManager cooldownManager;
    public List<GameObject> spelledPrefabs;
    private Vector3 pointStart;
    private Vector3 pointMid;
    private Vector3 pointEnd;
    private Vector3 pointSM;
    private Vector3 pointME;
    public RectTransform chatPanel;
    private TextMeshPro turnText;
    [SerializeField] int moveHeight;
    private float interpolateAmount = 1;
    [SerializeField] SpriteRenderer loadingSprite;
    private float newPosTimer = .6f;
    private Vector3 newPos;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public ParticleSystem[] moveAnimations = new ParticleSystem[4];
    private ParticleSystem auraAnim;
    private ParticleSystem spawnAnim;
    public float shake_decay;
    public float shake_intensity;
    private float temp_shake_intensity = 0;
    private SoundManager soundManager;
    private void Start() {
        client = FindObjectOfType<Client>();
        com = FindObjectOfType<Command>();
        spells = FindObjectOfType<SpellCards>();
        cooldownManager = FindObjectOfType<SpellManager>();
        soundManager = FindObjectOfType<SoundManager>();
        turnText = GameObject.Find("TurnText").GetComponent<TextMeshPro>();
        playerIsWhite = client.isHost;
        Instance = this;
        spawnAllChessmen();
        cooldownManager.spellButtonsEnable(isWhiteTurn == playerIsWhite);
    }
    public void spellHighlight(string spellName) {
        BoardHighlights.Instance.hideHighlights();
        if (isSpellMove) {
            isSpellMove = false;
        } else {
            isSpellMove = true;
            this.spellName = spellName;
            switch (spellName) {
                case "Upgrade":
                    allowedMoves = spells.possibleUpgrade();
                    break;
                case "Stun":
                    allowedMoves = spells.possibleStun();
                    break;
            }
            BoardHighlights.Instance.highlightAllowedMoves(allowedMoves);
        }
    }
    List<spelledPiece> spelledPieces = new List<spelledPiece>();
    public void actionSpell(string spellName, int x, int y) {
        spelledPiece sp = new spelledPiece();
        sp.spellName = spellName;
        sp.newID = pieceID;
        if (spellName == "Upgrade") {
            selectedPiece = chessMen[x, y];
            string type = selectedPiece.GetType().ToString();
            switch (type) {
                case "Queen": sp.pieceType = 1; break;
                case "Rook": sp.pieceType = 2; break;
                case "Bishop": sp.pieceType = 3; break;
                case "Knight": sp.pieceType = 4; break;
                case "Pawn": sp.pieceType = 5; break;
            }
            if (!selectedPiece.isItWhite)
                sp.pieceType += 6;
            sp.endSpellTurn = turnCount + 4;
            activeChessmen.Remove(selectedPiece.gameObject);
            Destroy(selectedPiece.gameObject);
            spawnSpellMan(spellName, x, y);
        } else if (spellName == "Stun") {
            chessMen[x, y].enabled = false;
            spawnSpellMan(spellName, x, y);
            sp.endSpellTurn = turnCount + 3;
        }
        sp.x = x;
        sp.y = y;
        spelledPieces.Add(sp);
        BoardHighlights.Instance.hideHighlights();
        isSpellMove = false;
    }
    private void spellCheck() {
        for (int i = spelledPieces.Count - 1; i >= 0; i--) {
            spelledPiece sp = spelledPieces[i];
            if (sp.spellName == "Upgrade") {
                if (sp.endSpellTurn == turnCount) {
                    foreach (ChessPieces cs in chessMen) {
                        if (cs != null && cs.id == sp.newID) {
                            activeChessmen.Remove(cs.gameObject);
                            spawnChessman(sp.pieceType, cs.currentX, cs.currentY);
                            Destroy(cs.gameObject);
                            spelledPieces.Remove(sp);
                            break;
                        }
                    }
                } else if (sp.endSpellTurn == turnCount + 2) {
                    Destroy(tempSpellObjects[sp.x, sp.y].GetComponent<Cooldown>());
                    Destroy(tempSpellObjects[sp.x, sp.y].transform.GetChild(1).gameObject);
                    chessMen[sp.x, sp.y] = tempSpellObjects[sp.x, sp.y].GetComponent<ChessPieces>();
                    chessMen[sp.x, sp.y].setPosition(sp.x, sp.y);
                    tempSpellObjects[sp.x, sp.y] = null;
                }
            } else if (sp.spellName == "Stun") {
                if (sp.endSpellTurn == turnCount) {
                    Destroy(chessMen[sp.x, sp.y].gameObject.GetComponent<Cooldown>());
                    chessMen[sp.x, sp.y].gameObject.GetComponent<ChessPieces>().enabled = true;
                    chessMen[sp.x, sp.y] = chessMen[sp.x, sp.y].gameObject.GetComponent<ChessPieces>();
                    tempSpellObjects[sp.x, sp.y] = null;
                }
            }
        }
    }
    private bool isPanelOpen;
    public void panelState(bool _panelState) {
        isPanelOpen = _panelState;
    }
    private void Update() {
        updateSelection();
        if (isSpellMove) {
            if (Input.GetMouseButtonDown(0) && !isPanelOpen) {
                if (selectionX >= 0 && selectionY >= 0) {
                    if (allowedMoves[selectionX, selectionY]) {
                        com.sendSpell(spellName, selectionX, selectionY);
                        cooldownManager.startCooldown(spellName);
                    } else
                        BoardHighlights.Instance.hideHighlights();
                    isSpellMove = false;
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0) && !isPanelOpen) {
                if (selectionX >= 0 && selectionY >= 0 && !stillPieceMoving) {
                    if (selectedPiece == null) {
                        if (chessMen[selectionX, selectionY] == null) {
                            return;
                        }
                        if (chessMen[selectionX, selectionY].isItWhite != isWhiteTurn || playerIsWhite != isWhiteTurn) {
                            return;
                        }
                        selectChessPiece(selectionX, selectionY);
                    } else {
                        com.sendMove(selectedPiece.currentX, selectedPiece.currentY, selectionX, selectionY);
                    }
                }
            }
        }
        if (stillPieceMoving) {
            if (temp_shake_intensity > 0) {
                selectedPiece.transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
                selectedPiece.transform.rotation = new Quaternion(
                    originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                    originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                    originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                    originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
                temp_shake_intensity -= shake_decay;
            } else if (temp_shake_intensity < 0) {
                selectedPiece.transform.position = newPos;
                soundManager.moveLoading.Stop();
                newPosTimer -= Time.deltaTime;
                if (newPosTimer <= 0f) {
                    newPosTimer = .6f;
                    selectedPiece.transform.position = getTileCenter((int)newPos.x, (int)newPos.z);
                    spawnAnim.transform.position = selectedPiece.transform.position;
                    spawnAnim.Play();
                    selectedPiece = null;
                    stillPieceMoving = false;
                }
            }
        }
    }
    private void startMoveAnim(int x, int y) {
        if (selectedPiece.isItWhite) {
            auraAnim = moveAnimations[0];
            spawnAnim = moveAnimations[1];
        } else {
            auraAnim = moveAnimations[2];
            spawnAnim = moveAnimations[3];
        }
        newPos = new Vector3(x, 15f, y);
        stillPieceMoving = true;
        auraAnim.transform.position = selectedPiece.transform.position;
        auraAnim.Play();
        soundManager.moveLoading.Play();
        originPosition = selectedPiece.transform.position;
        originRotation = selectedPiece.transform.rotation;
        temp_shake_intensity = shake_intensity;
    } 
    private void selectChessPiece(int x, int y) {
        allowedMoves = chessMen[x, y].possibleMove();
        selectedPiece = chessMen[x, y];
        selectedPiece.GetComponent<Outline>().enabled = true;
        BoardHighlights.Instance.highlightAllowedMoves(allowedMoves);
    }
    public void pawnUpgrade(int currentX, int currentY, int upgrade) {
        //Black Upgrade
        if (currentY == 0)
            upgrade += 6;
        selectedPiece = chessMen[currentX, currentY];
        activeChessmen.Remove(selectedPiece.gameObject);
        Destroy(selectedPiece.gameObject);
        spawnChessman(upgrade, currentX, currentY);
        swapTurn();
    }
    private void enPassantMove(int x, int y, ChessPieces c) {
        if (x == passantMove[0] && y == passantMove[1]) {
            if (isWhiteTurn)
                c = chessMen[x, y - 1];
            else
                c = chessMen[x, y + 1];
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
    }
    private bool stillPieceMoving;
    private bool isMoveRepeating;
    public void moveChessPiece(int selectionX, int selectionY, int x, int y) {
        selectChessPiece(selectionX, selectionY);
        selectedPiece.GetComponent<Outline>().enabled = false;
        if (allowedMoves[x, y]) {
            startMoveAnim(x, y);
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
                if (y == 7 || y == 0) {
                    if (isWhiteTurn == playerIsWhite) {
                        upgradePanel.SetActive(true);
                        panelState(true);
                    }
                    isMoveRepeating = true;
                }

                if (selectedPiece.currentY == 1 && y == 3) {
                    passantMove[0] = x;
                    passantMove[1] = y - 1;
                } else if (selectedPiece.currentY == 6 && y == 4) {
                    passantMove[0] = x;
                    passantMove[1] = y + 1;
                }
            }
            if (selectedPiece.GetType() == typeof(King) && c != null) {
                castlingMove(x, y, c);
                isMoveRepeating = true;
            }
            chessMen[selectedPiece.currentX, selectedPiece.currentY] = null;
            selectedPiece.setPosition(x, y);

            if (selectedPiece.GetType() == typeof(King))
                selectedPiece.GetComponent<King>().everMoved = true;
            else if (selectedPiece.GetType() == typeof(Rook))
                selectedPiece.GetComponent<Rook>().everMoved = true;
            chessMen[x, y] = selectedPiece;
            if (!isMoveRepeating)
                swapTurn();
        } else {
            selectedPiece = null;
        }
        BoardHighlights.Instance.hideHighlights();
        isMoveRepeating = false;
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, LayerMask.GetMask("chessPlane"))) {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        } else {
            selectionX = -1;
            selectionY = -1;
        }
    }
    private int pieceID = 100;
    private void spawnChessman(int index, int x, int y) {
        GameObject go = Instantiate(chessmanPrefabs[index], getTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        chessMen[x, y] = go.GetComponent<ChessPieces>();
        chessMen[x, y].setPosition(x, y);
        chessMen[x, y].id = pieceID;
        pieceID++;
        activeChessmen.Add(go);
    }
    GameObject[,] tempSpellObjects = new GameObject[8, 8];
    private void spawnSpellMan(string spellName, int x, int y) {
        if (spellName == "Upgrade") {
            int index = (isWhiteTurn) ? 0 : 1;
            GameObject go = Instantiate(spelledPrefabs[index], getTileCenter(x, y), Quaternion.identity) as GameObject;
            go.transform.SetParent(transform);
            GameObject go2 = Instantiate(spelledPrefabs[2], getTileCenter(x, y), Quaternion.identity) as GameObject;
            go2.transform.SetParent(go.transform);
            chessMen[x, y] = go.GetComponent<Cooldown>();
            chessMen[x, y].isItWhite = playerIsWhite;
            chessMen[x, y].setPosition(x, y);
            tempSpellObjects[x, y] = go;
            tempSpellObjects[x, y].GetComponent<Master>().id = pieceID;
            activeChessmen.Add(go);
        } else if (spellName == "Stun") {
            GameObject go = Instantiate(spelledPrefabs[2], getTileCenter(x, y), Quaternion.identity) as GameObject;
            //go.name = chessMen[x, y].GetType().ToString();
            tempSpellObjects[x, y] = go;
            chessMen[x, y].gameObject.GetComponent<ChessPieces>().enabled = false;
            chessMen[x, y] = chessMen[x, y].gameObject.AddComponent<Cooldown>();
            //chessMen[x, y] = go2.GetComponent<ChessPieces>();
            chessMen[x, y].setPosition(x, y);
            chessMen[x, y].isItWhite = !isWhiteTurn;
        }
        pieceID++;
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
    private void swapTurn() {
        turnCount++;
        spellCheck();
        isWhiteTurn = !isWhiteTurn;
        if (isWhiteTurn) {
            turnText.text = "White is playing..";
            turnText.color = new Color(110, 125, 122);
            loadingSprite.color = new Color(169, 176, 174);
        } else {
            turnText.text = "Black is playing..";
            turnText.color = new Color(0, 0, 0);
            loadingSprite.color = new Color(0, 0, 0);
        }
        cooldownManager.spellButtonsEnable(isWhiteTurn == playerIsWhite);
    }
}
public struct spelledPiece {
    public string spellName;
    public int endSpellTurn;
    public int pieceType;
    public int newID;
    public int x;
    public int y;
}