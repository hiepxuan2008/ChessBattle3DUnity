using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour
{

    Animator cameraAnimator;
    
    int index = 0;
    private string[] cameraTrigger = { "WhiteTurn", "RightMotion", "BlackTurn", "LeftMotion"};

    Animator pauseMenuAnimator;
    GameObject clickMash;

    bool isShowPauseMenu = false;

    // Use this for initialization
    void Start()
    {
        cameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
        if (cameraAnimator == null)
        {
            Debug.Log("Can't find camera animator.");
        }

        pauseMenuAnimator = GameObject.Find("Pause Menu").GetComponent<Animator>();
        if (pauseMenuAnimator == null)
        {
            Debug.Log("Can't find pause menu animator.");
        }

        clickMash = GameObject.Find("ClickMask");
        if (clickMash == null)
        {
            Debug.Log("Can't find ClickMask.");
        } else
        {
            clickMash.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isShowPauseMenu)
            {
                pauseMenuAnimator.SetTrigger("ShowPauseMenu");
                isShowPauseMenu = true;
                clickMash.SetActive(true);
            } else
            {
                pauseMenuAnimator.SetTrigger("HidePauseMenu");
                isShowPauseMenu = false;
                clickMash.SetActive(false);
            }
        }
    }

    // side: left, right, white, black
    public void MoveCamera(string side)
    {
        if (side == "left")
        {
            index--;
            if (index < 0)
                index = 3;
            
        } else
        {
            index++;
            if (index > 3)
                index = 0;
        }
        cameraAnimator.SetTrigger(cameraTrigger[index]);
    }

    public void ExitGame()
    {
        LevelManager.QuitRequest();
    }

    public void LoadMainMenu()
    {
        LevelManager.LoadLevel("MainMenu");
    }
}
