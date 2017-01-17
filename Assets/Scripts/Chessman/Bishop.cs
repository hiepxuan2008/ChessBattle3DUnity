using UnityEngine;
using System.Collections;
using System;

public class Bishop : Chessman {
    public override string Annotation()
    {
        return "B";
    }

    public override bool[,] PossibleEat()
    {
        return PossibleMove();
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        Chessman c;
        int i, j;

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
