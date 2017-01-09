using UnityEngine;
using System.Collections;

public class Pawn : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        int[] e = BoardManager.Instance.EnPassantMove;
        Chessman c1, c2;

        // white team moves
        if (isWhite)
        {
            // Diagonal left
            if (CurrentX != 0 && CurrentY != 7)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY + 1)
                    moves[CurrentX - 1, CurrentY + 1] = true;

                c1 = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c1 != null && !c1.isWhite)
                {
                    moves[CurrentX - 1, CurrentY + 1] = true; 
                }
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                    moves[CurrentX + 1, CurrentY + 1] = true;

                c1 = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
                if (c1 != null && !c1.isWhite)
                {
                    moves[CurrentX + 1, CurrentY + 1] = true;
                }
            }

            // Middle
            if (CurrentY != 7)
            {
                c1 = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                if (c1 == null)
                {
                    moves[CurrentX, CurrentY + 1] = true;
                }
            }

            // Middle on first move
            if (CurrentY == 1)
            {
                c1 = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 2];

                if (c1 == null && c2 == null)
                {
                    moves[CurrentX, CurrentY + 2] = true;
                }
            }
        } else
        {
            // Diagonal left
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY + 1)
                    moves[CurrentX - 1, CurrentY - 1] = true;

                c1 = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c1 != null && c1.isWhite)
                {
                    moves[CurrentX - 1, CurrentY - 1] = true;
                }
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                    moves[CurrentX + 1, CurrentY - 1] = true;

                c1 = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c1 != null && c1.isWhite)
                {
                    moves[CurrentX + 1, CurrentY - 1] = true;
                }
            }

            // Middle
            if (CurrentY != 0)
            {
                c1 = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                if (c1 == null)
                {
                    moves[CurrentX, CurrentY - 1] = true;
                }
            }

            // Middle on first move
            if (CurrentY == 6)
            {
                c1 = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                c2 = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 2];

                if (c1 == null && c2 == null)
                {
                    moves[CurrentX, CurrentY - 2] = true;
                }
            }
        }

        return moves;
    }
}
