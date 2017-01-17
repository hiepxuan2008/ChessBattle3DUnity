using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardManager : MonoBehaviour
{
    private const float DELAY_TIME = 1.5F;
    private const float ROTATE_TIME = 0.1F;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> ChessmanPrefabs;
    private List<GameObject> activeChessmans;

    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;
    private bool isWhiteTurn = true;

    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    public int[] EnPassantMove { set; get; }

    public ButtonManager buttonManager;
    public AudioClip cellHoverAudio;
    public AudioClip pieceSelectedAudio;

    private void Start()
    {
        Instance = this;
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };
        SpawnAllChessmans();
        ShowPowerEffectAllChessmans();
        GameObject.Find("MainGameManager").GetComponent<MainGameManager>().HideWinText();
    }

    private void ShowPowerEffectAllChessmans()
    {
        foreach (GameObject piece in activeChessmans)
        {
            piece.GetComponent<Chessman>().PlayPowerEffectFor(5.0f);
        }
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();
        HighLightMouseHoverCell();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (GameManager.Instance.GameMode == GameManager.MODE.PLAYER_VS_PLAYER)
                {
                    if (selectedChessman == null)
                    {
                        // select the chessman
                        SelectChessman(selectionX, selectionY);
                    }
                    else
                    {
                        if (Chessmans[selectionX, selectionY] && Chessmans[selectionX,selectionY].isWhite == isWhiteTurn) //The same team, reselect
                        {
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
        }
    }

    
    private void PlayCellHoverSound()
    {
        GetComponent<AudioSource>().PlayOneShot(cellHoverAudio);
    }

    private void PlayPieceSelectedSound()
    {
        GetComponent<AudioSource>().PlayOneShot(pieceSelectedAudio);
    }

    private int oldSelectionX, oldSelectionY;
    private void HighLightMouseHoverCell()
    {
        if (GameManager.Instance.GameMode != GameManager.MODE.VISUALIZE)
        {
            if (selectionX != -1 && selectionY != -1)
            {
                if (oldSelectionX != selectionX || oldSelectionY != selectionY)
                {
                    BoardHighlights.Instance.ShowHoverHighlight(new Vector3(selectionX + 0.5f, 0, selectionY + 0.5f));
                    PlayCellHoverSound();
                    oldSelectionX = selectionX;
                    oldSelectionY = selectionY;
                }
            }
            else
            {
                BoardHighlights.Instance.HideHoverHighlight();
            }
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out hit, 25f,
            LayerMask.GetMask("ClickMask")))
        {
            selectionX = -1;
            selectionY = -1;
            //Debug.Log("Click on mask");
        }
        else if (Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out hit, 25f,
            LayerMask.GetMask("ChessPlan")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
            //Debug.Log("Click on Chessboard");
        } else {
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
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    public List<GameObject> GetAllChessmans()
    {
        return activeChessmans;
    }

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject obj = Instantiate(
            ChessmanPrefabs[index],
            GetTileCenter(x, y),
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

    //private Vector3 AdjustChessmanPosition(Vector3 pos)
    //{
    //    return pos + new Vector3(dx, dy, dz);
    //}

    private void SpawnAllChessmans()
    {
        activeChessmans = new List<GameObject>();

        // White team
        // King
        SpawnChessman(0, 4, 0);
        // Queen
        SpawnChessman(1, 3, 0);
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

    public void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
            return;
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        PlayPieceSelectedSound();

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
        //previousMat = selectedChessman.GetComponentInChildren<MeshRenderer>().material;
        //selectedMat.mainTexture = previousMat.mainTexture;
        //selectedChessman.GetComponentInChildren<MeshRenderer>().material = selectedMat;

        BoardHighlights.Instance.HideHighlights();
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
        BoardHighlights.Instance.HighlightSelected(new Vector3(x + 0.5f, 0, y + 0.5f));
    }

    public void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            if (chessMate != null)
            {
                BoardHighlights.Instance.HideCheckedHighlight();
                chessMate.HidePowerEffect();
                chessMate = null;
            }

            Chessman c = Chessmans[x, y];
            float delays = 0f;

            // Check EnPassantMove
            ProcessEnPassantMove(c, x, y, out delays);

            // Eat chessman
            bool isEatChess = false;
            if (c != null && c.isWhite != isWhiteTurn)
            {
                delays = DELAY_TIME;
                c.RotateEach(ROTATE_TIME);
                c.DestroyAfter(delays);
                
                BoardHighlights.Instance.ShowKillerHighlight(new Vector3(x + 0.5f, 0, y + 0.5f));
                BoardHighlights.Instance.HideKillerHighlightAfter(delays);
                isEatChess = true;

                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }
            }

            BoardHighlights.Instance.ShowHoverHighlight(new Vector3(x+0.5f, 0, y+0.5f));

            // check if pawn step on final line
            ProcessIfPawnStepOnFinalLine(x, y);

            // Move selected chessman to x, y position
            MoveSelectedChessmanTo(x, y, delays);

            // Checkmate
            if (!IsCheckmate(Chessmans[x, y].PossibleMove()) && isEatChess)
            {
                selectedChessman.PlayPowerEffectFor(DELAY_TIME);
            }
            ProcessCheckmate(x, y);

            // Change turn
            isWhiteTurn = !isWhiteTurn;
        }

        BoardHighlights.Instance.HideHighlights();
        selectedChessman = null;
    }

    private void ProcessIfPawnStepOnFinalLine(int x, int y)
    {
        if (selectedChessman.GetType() == typeof(Pawn))
        {
            // check if pawn steps on final line, it becomes Queen
            if (y == 7) // white team
            {
                int currentX = selectedChessman.CurrentX, currentY = selectedChessman.CurrentY;
                // remove selected chessman
                activeChessmans.Remove(selectedChessman.gameObject);
                Destroy(selectedChessman.gameObject);

                // spawn new chessman
                SpawnChessman(1, currentX, currentY);
                selectedChessman = Chessmans[currentX, currentY];

                // rotate the chessman
                selectedChessman.RotateEach(ROTATE_TIME);
            }
            else if (y == 0) // black team
            {
                int currentX = selectedChessman.CurrentX, currentY = selectedChessman.CurrentY;
                // remove selected chessman
                activeChessmans.Remove(selectedChessman.gameObject);
                Destroy(selectedChessman.gameObject);

                // spawn new chessman
                SpawnChessman(7, currentX, currentY);
                selectedChessman = Chessmans[currentX, currentY];

                // rotate the chessman
                // selectedChessman.RotateEach(ROTATE_TIME);
            }
        }
    }

    private void MoveSelectedChessmanTo(int x, int y, float delays)
    {
        Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;

        selectedChessman.MoveAfter(delays, GetTileCenter(x, y));

        selectedChessman.SetPosition(x, y);
        Chessmans[x, y] = selectedChessman;
    }

    private Chessman chessMate;

    private void ProcessCheckmate(int x, int y)
    {
        bool[,] allowedMoves = Chessmans[x, y].PossibleMove();
        if (IsCheckmate(allowedMoves))
        {
            Chessman kingPos = GetKingPos(!isWhiteTurn);
            BoardHighlights.Instance.ShowCheckedHighlight(new Vector3(kingPos.CurrentX + 0.5f, 0, kingPos.CurrentY + 0.5f));
            selectedChessman.ShowPowerEffect();
            chessMate = selectedChessman;
            OnChecked();

            //if (isWhiteTurn)
            //    Debug.Log("Black team is checkmated");
            //else
            //    Debug.Log("White team is checkmated");
        }
    }

    private void OnChecked()
    {
        GameObject mainGameMangerGO = GameObject.Find("MainGameManager");
        if (mainGameMangerGO != null)
        {
            mainGameMangerGO.GetComponent<MainGameManager>().OnChecked();
        }
    }

    private void ProcessEnPassantMove(Chessman c, int x, int y, out float delay)
    {
        delay = 0;
        if (x == EnPassantMove[0] && y == EnPassantMove[1])
        {
            if (isWhiteTurn)
                c = Chessmans[x, y - 1];
            else
                c = Chessmans[x, y + 1];

            c.RotateEach(ROTATE_TIME);
            c.DestroyAfter(DELAY_TIME);

            //selectedChessman.RotateEach(ROTATE_TIME);

            delay = DELAY_TIME;
        }

        EnPassantMove[0] = -1;
        EnPassantMove[1] = -1;
        // EnPassant
        if (selectedChessman.GetType() == typeof(Pawn))
        {
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
    }

    private Chessman GetKingPos(bool isWhite)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ( Chessmans[i, j] != null && Chessmans[i, j].GetType() == typeof(King)
                    && Chessmans[i, j].isWhite == isWhite)
                {
                    return Chessmans[i,j];
                }
            }
        }
        return null;
    }

    private bool IsCheckmate(bool[,] allowedMoves)
    {
        if (allowedMoves.Length == 0)
            return false;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j] && Chessmans[i, j] != null && Chessmans[i, j].GetType() == typeof(King))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            GameObject.Find("MainGameManager").GetComponent<MainGameManager>().EndGame(1);
        else
            GameObject.Find("MainGameManager").GetComponent<MainGameManager>().EndGame(-1);

        foreach (GameObject go in activeChessmans)
        {
            Destroy(go);
        }

        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessmans();
    }

    public Location Find(int turn, char c, Location dst, string disam)
    {
        Location lo = new Location();
        bool isWhite = (turn == 0);
        foreach (GameObject chessObj in activeChessmans)
        {
            Chessman chess = chessObj.GetComponent<Chessman>();
            if ((chess.isWhite == isWhite) && (chess.Annotation().Equals(c.ToString())))
            {
                if (chess.CanGo(dst.x, dst.y))
                {
                    if (disam.Length == 1) //Disambiguating moves
                    {
                        if (disam[0] >= '1' && disam[0] <= '9') //rank have to the same
                        {
                            if (chess.CurrentY == (disam[0] - '1'))
                            {
                                return new Location(chess.CurrentX, chess.CurrentY);
                            }
                        }
                        else if (disam[0] >= 'a' && disam[0] <= 'z') //file have to the same
                        {
                            if (chess.CurrentX == (disam[0] - 'a'))
                            {
                                return new Location(chess.CurrentX, chess.CurrentY);
                            }
                        }
                        else
                        {
                            Debug.Log("Unexpected result! " + disam);
                        }
                    }
                    else
                    {
                        return new Location(chess.CurrentX, chess.CurrentY);
                    }
                }
            }
        }

        return lo;
    }

    public void KingSideCastling(int turn)
    {
        bool isWhite = (turn == 0);
        int rank = isWhite ? 0 : 7;
        if (Chessmans[7, rank] && Chessmans[7, rank].Annotation().Equals("R")
            && Chessmans[4, rank] && Chessmans[4, rank].Annotation().Equals("K"))
        {
            MoveFromTo(7, rank, 5, rank);
            MoveFromTo(4, rank, 6, rank);
            Chessmans[5, rank].PlayPowerEffectFor(2.0f);
            Chessmans[6, rank].PlayPowerEffectFor(2.0f);
        }
    }

    private GameObject GetChessmanObj(int x, int y)
    {
        foreach (GameObject pieceObj in activeChessmans)
        {
            Chessman piece = pieceObj.GetComponent<Chessman>();
            if (piece.CurrentX == x && piece.CurrentY == y)
                return pieceObj;
        }
        return null;
    }

    private void MoveFromTo(int srcX, int srcY, int dstX, int dstY)
    {
        Chessman chess = Chessmans[srcX, srcY];

        Chessmans[srcX, srcY] = null;
        chess.transform.position = GetTileCenter(dstX, dstY);
        chess.SetPosition(dstX, dstY);
        Chessmans[dstX, dstY] = chess;
    }

    public void QueenSideCastling(int turn)
    {
        bool isWhite = (turn == 0);
        int rank = isWhite ? 0 : 7;
        if (Chessmans[0, rank] && Chessmans[0, rank].Annotation().Equals("R")
            && Chessmans[4, rank] && Chessmans[4, rank].Annotation().Equals("K"))
        {
            MoveFromTo(0, rank, 3, rank);
            MoveFromTo(4, rank, 2, rank);
        }
    }
}
