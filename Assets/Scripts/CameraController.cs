using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.RotateAround(new Vector3(4, 0, 4), Vector3.right, 90);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
