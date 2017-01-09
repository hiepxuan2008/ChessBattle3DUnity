using UnityEngine;
using System.Collections;

public class Rook : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        Chessman c;

        // Right
        int i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
            {
                moves[i, CurrentY] = true;
            } else
            {
                if (c.isWhite != isWhite)
                    moves[i, CurrentY] = true;
                break;
            }
        }

        // Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
            {
                moves[i, CurrentY] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                    moves[i, CurrentY] = true;
                break;
            }
        }

        // Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
            {
                moves[CurrentX, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                    moves[CurrentX, i] = true;
                break;
            }
        }

        // Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
            {
                moves[CurrentX, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                    moves[CurrentX, i] = true;
                break;
            }
        }

        return moves;
    }
}
