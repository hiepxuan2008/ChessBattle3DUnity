using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    public enum MODE { PLAYER_VS_PLAYER, VISUALIZE, PLAYER_VS_AI }

    public MODE GameMode = MODE.PLAYER_VS_PLAYER;
    public string VisualizePath = null;

    public void StartGame()
    {
        LevelManager.LoadLevel("MainGame");
    }
}
