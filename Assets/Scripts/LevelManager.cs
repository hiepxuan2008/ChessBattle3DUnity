using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static void LoadLevel(string name) {
		//Debug.Log ("Level load requested for: " + name);
		//Application.LoadLevel (name);
        SceneManager.LoadScene(name);
	}

	public static void QuitRequest(){
		//Debug.Log ("I want to quit!");
		Application.Quit ();
	}
}
