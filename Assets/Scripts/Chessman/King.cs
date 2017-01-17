using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class King : Chessman {
    public override string Annotation()
    {
        return "K";
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
        int[] dy = { 1, 1, 1, 0, -1, -1, -1, 0 };
        int x, y;

        bool[,] enemyMoves = GetAllEnemyMoves(BoardManager.Instance.GetAllChessmans());

        for (int i = 0; i < 8; i++)
        {
            x = CurrentX + dx[i];
            y = CurrentY + dy[i];

            if (x < 0 || x > 7 || y < 0 || y > 7 || enemyMoves[x, y] == true)
                continue;

            Chessman c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
                moves[x, y] = true;
            else if (c.isWhite != isWhite)
                moves[x, y] = true;
        }

        return moves;
    }

    private bool[,] GetAllEnemyMoves(List<GameObject> enemies)
    {
        List<bool[,]> listMoves = new List<bool[,]>();
        foreach (GameObject go in enemies)
        {
            if (go != null)
            {
                Chessman chessman = go.GetComponent<Chessman>();
                if (chessman.isWhite != isWhite)
                {
                    listMoves.Add(chessman.PossibleEat());
                }
            }
        }

        bool[,] result = new bool[8, 8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                foreach (bool[,] moves in listMoves)
                {
                    if (moves[i, j])
                    {
                        result[i, j] = true;
                        break;
                    }
                }
            }
        }

        return result;
    }

    public override bool[,] PossibleEat()
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
