using UnityEngine;
using System.Collections;

public class King : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
        int[] dy = { 1, 1, 1, 0, -1, -1, -1, 0 };
        int x, y;

        for (int i = 0; i < 8; i++)
        {
            x = CurrentX + dx[i];
            y = CurrentY + dy[i];

            if (x < 0 || x > 7 || y < 0 || y > 7)
                continue;

            Chessman c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
                moves[x, y] = true;
            else if (c.isWhite != isWhite)
                moves[x, y] = true;

        }

        return moves;
    }
}
