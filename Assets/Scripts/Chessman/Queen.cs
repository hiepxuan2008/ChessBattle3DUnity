using UnityEngine;
using System.Collections;

public class Queen : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        Chessman c;
        int i, j;

        // Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
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

        // Top Left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--; j++;
            if (i < 0 || j > 7)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                moves[i, j] = true;
            else if (c.isWhite != isWhite)
            {
                moves[i, j] = true;
                break;
            }
            else
                break;
        }

        // Top Right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++; j++;
            if (i > 7 || j > 7)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                moves[i, j] = true;
            else if (c.isWhite != isWhite)
            {
                moves[i, j] = true;
                break;
            }
            else
                break;
        }

        // Bottom Left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--; j--;
            if (i < 0 || j < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                moves[i, j] = true;
            else if (c.isWhite != isWhite)
            {
                moves[i, j] = true;
                break;
            }
            else
                break;
        }

        // Bottom Right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++; j--;
            if (i > 7 || j < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                moves[i, j] = true;
            else if (c.isWhite != isWhite)
            {
                moves[i, j] = true;
                break;
            }
            else
                break;
        }

        return moves;
    }
}
