using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeList : MonoBehaviour {
    public GameObject itemGO;
    public GameObject listGO;
    public ScrollRect scroll;

	// Use this for initialization
	void Start () {
        Debug.Log("VisualizeList!");
        string path = "Data/";
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            GameObject item = (GameObject)Instantiate(itemGO, listGO.transform);
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.GetComponentInChildren<Text>().text = file.Name;
            item.GetComponent<VisualizeItem>().filePath = file.DirectoryName + "\\" + file.Name;
            item.GetComponent<VisualizeItem>().fileName = file.Name;
        }

        scroll.verticalNormalizedPosition = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
