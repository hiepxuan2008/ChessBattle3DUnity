using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public Animator mainMenuAnim;
    public Animator visualizeMenuAnim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnVisualize()
    {
        mainMenuAnim.SetBool("IsAppear", false);
        visualizeMenuAnim.SetBool("IsAppear", true);
    }

    public void OnVisualizeClose()
    {
        mainMenuAnim.SetBool("IsAppear", true);
        visualizeMenuAnim.SetBool("IsAppear", false);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
