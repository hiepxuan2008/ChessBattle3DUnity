using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeItem : MonoBehaviour {
    public string filePath;
    public string fileName;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartVisualize()
    {
        Debug.Log("StartVisualize" + filePath);
        GameManager.Instance.GameMode = GameManager.MODE.VISUALIZE;
        GameManager.Instance.VisualizePath = filePath;
        GameManager.Instance.StartGame();
    }
}
