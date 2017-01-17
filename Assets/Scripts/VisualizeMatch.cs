using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class VisualizeMatch : MonoBehaviour {
    List<String> data;
    int step = 0;
    int result = 0;
    MainGameManager gameManager;
    

    public VisualizeMatch(MainGameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public void LoadMatchData()
    {
        data = new List<string>();
        string fileName = GameManager.Instance.VisualizePath;
        try
        {
            string line;
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            do
            {
                line = theReader.ReadLine();
                if (line != null)
                {
                    Debug.Log(line);
                    if (line.Length > 0 && line[0] != '1')
                        continue;

                    String[] parts = line.Split(' ');
                    foreach (String part in parts)
                    {
                        if (!part.Contains("."))
                            data.Add(part);
                    }
                    String strResult = data[data.Count - 1];
                    data.RemoveAt(data.Count - 1);
                    if (strResult.Equals("0-1"))
                        result = -1; //Black win
                    else if (strResult.Equals("1-0"))
                        result = 1; //White win
                    else
                        result = 0; //Tie
                }
            } while (line != null);

            theReader.Close();
        }
        catch (Exception e)
        {
            Debug.Log("{0}\n" + e.Message);
        }
    }

    public void Visualize()
    {
        for (int i = 0; i < data.Count; i++)
        {
            Move(i%2, data[i]);
        }
    }

    public void VisualizeNextStep()
    {
        if (step == data.Count)
        {
            Debug.Log("End of game!");
            Debug.Log("Result: " + result);
            gameManager.isVisualize = false;
            gameManager.EndGame(result);
            return;
        }
        Move(step % 2, data[step]);
        step++;
    }

    public String Normalize(String move)
    {
        return move.Replace("+", "").Replace("x", "");
    }

    public void Move(int turn, String move)
    {
        move = Normalize(move);
        char f = move[0];

        if (move == "O-O")
        {
            gameManager.KingSideCastling(turn);
        }
        else if (move == "O-O-O")
        {
            gameManager.QueenSideCastling(turn);
        }
        else
        {
            int n = move.Length;
            int rank = move[n - 1] - '1'; //hang
            int file = move[n - 2] - 'a'; //cot
            Location dest = new Location(file, rank);
            Location src;

            if ("KQBNR".Contains(f.ToString())) {
		        String disam = "";
		        if (n == 4)
			        disam += move[1];
		        src = gameManager.Find(turn, f, dest, disam);
	        } else { //Pawn
		        String disam = "";
		        if (n == 3)
			        disam += move[0];
		        src = gameManager.Find(turn, 'P', dest, disam); //Find Pawn
	        }
            gameManager.Move(src, dest);
        }
    }
}
