using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameBGM : MonoBehaviour {
    public Animator animator;
    private bool isFirst = true;
    public AudioClip preAudio;
    public AudioClip backAudio;

    // Use this for initialization
    void Start () {
        GetComponent<AudioSource>().clip = preAudio;
        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (isFirst)
        {
            if (animator.IsInTransition(0))
            {
                GetComponent<AudioSource>().clip = backAudio;
                GetComponent<AudioSource>().Play();
                isFirst = false;
            }
        }
    }
}
