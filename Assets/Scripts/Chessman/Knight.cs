using UnityEngine;
using System.Collections;
using System;

public class Knight : Chessman {
    public override string Annotation()
    {
        return "N";
    }

    public override bool[,] PossibleEat()
    {
        return PossibleMove();
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];

        // UpLeft
        KnightMove(CurrentX - 1, CurrentY + 2, ref moves);

        // UpRight
        KnightMove(CurrentX + 1, CurrentY + 2, ref moves);

        // RightUp
        KnightMove(CurrentX + 2, CurrentY + 1, ref moves);

        // RightDown
        KnightMove(CurrentX + 2, CurrentY - 1, ref moves);

        // DownLeft
        KnightMove(CurrentX - 1, CurrentY - 2, ref moves);

        // DownRight
        KnightMove(CurrentX + 1, CurrentY - 2, ref moves);

        // LeftUp
        KnightMove(CurrentX - 2, CurrentY + 1, ref moves);

        // DownRight
        KnightMove(CurrentX - 2, CurrentY - 1, ref moves);

        return moves;
    }

    private void KnightMove(int x, int y, ref bool[,] moves)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            Chessman c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
                moves[x, y] = true;
            else if (c.isWhite != isWhite)
                moves[x, y] = true;
        }
    }
}
