using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardManager : MonoBehaviour {

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> ChessmanPrefabs;
    private List<GameObject> activeChessmans;

    private float dx = 0f;
    private float dy = 0.5f;
    private float dz = 0.3f;

    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;
    private bool isWhiteTurn = true;

    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    public Material selectedMat;
    private Material previousMat;

    public int[] EnPassantMove { set; get; }

    private void Start()
    {
        Instance = this;
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };
        SpawnAllChessmans();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("ChessPlan")))
        {
            selectionX = (int) hit.point.x;
            selectionY = (int) hit.point.z;
        } else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);

            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw selection
        if (selectionX >= 0  && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject obj = Instantiate(
            ChessmanPrefabs[index], 
            AdjustChessmanPosition(GetTileCenter(x, y)), 
            ChessmanPrefabs[index].transform.rotation) as GameObject;

        obj.transform.SetParent(this.transform);
        Chessmans[x, y] = obj.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessmans.Add(obj);
    }

    private Vector3 GetTileCenter(int x, int z)
    {
        Vector3 origin = new Vector3();
        origin.x = TILE_SIZE * x + TILE_OFFSET;
        origin.z = TILE_SIZE * z + TILE_OFFSET;

        return origin;  
    }

    private Vector3 AdjustChessmanPosition(Vector3 pos)
    {
        return pos + new Vector3(dx, dy, dz);
    }

    private void SpawnAllChessmans()
    {
        activeChessmans = new List<GameObject>();

        // White team
        // King
        SpawnChessman(0, 3, 0);
        // Queen
        SpawnChessman(1, 4, 0);
        // Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);
        // Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);
        // Knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);
        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnChessman(5, i, 1);

        // Black team
        // King
        SpawnChessman(6, 4, 7);
        // Queen
        SpawnChessman(7, 3, 7);
        // Rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);
        // Bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);
        // Knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);
        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnChessman(11, i, 6);
    }

    void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
            return;
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        bool hasAtLeastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    break;
                }

        if (!hasAtLeastOneMove)
            return;

        selectedChessman = Chessmans[x, y];

        // change chessman material
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;

        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            // Eat chessman
            if (c != null && c.isWhite != isWhiteTurn)
            {
                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }

                activeChessmans.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isWhiteTurn)
                    c = Chessmans[x, y - 1];
                else
                    c = Chessmans[x, y + 1];
                activeChessmans.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            // EnPassant
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                if (y == 7)
                {
                    activeChessmans.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(1, x, y);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0)
                {
                    activeChessmans.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(7, x, y);
                    selectedChessman = Chessmans[x, y];
                }

                if (selectedChessman.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if (selectedChessman.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = AdjustChessmanPosition(GetTileCenter(x, y));
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }

        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        BoardHighlights.Instance.HideHighlights();
        selectedChessman = null;
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White team win");
        else
            Debug.Log("Black team win");

        foreach (GameObject go in activeChessmans)
        {
            Destroy(go);
        }

        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessmans();
    }
}
