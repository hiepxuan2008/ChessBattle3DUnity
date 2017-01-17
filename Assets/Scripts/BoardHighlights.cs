using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighlights : MonoBehaviour {

	public static BoardHighlights Instance { get; set; }

    public GameObject selectedPref;
    public GameObject highlightPrefab;
    public GameObject hoverHighlightPrefab;
    public GameObject killerHighlighPrefab;

    private List<GameObject> highlights;
    private GameObject parent;
    private GameObject selectedHighlight;
    private GameObject hoverHighlight;
    private GameObject killerHighlight;
    private GameObject checkedHighlight;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
        parent = new GameObject("Highlights");

        selectedHighlight = Instantiate(selectedPref);
        selectedHighlight.SetActive(false);

        hoverHighlight = Instantiate(hoverHighlightPrefab);
        hoverHighlight.SetActive(false);

        killerHighlight = Instantiate(killerHighlighPrefab);
        killerHighlight.SetActive(false);

        checkedHighlight = Instantiate(killerHighlighPrefab);
        checkedHighlight.SetActive(false);
    }

    public GameObject GetHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            go.transform.parent = parent.transform;
            highlights.Add(go);
        }

        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }

    }

    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
        selectedHighlight.SetActive(false);
    }

    public void HighlightSelected(Vector3 position)
    {
        selectedHighlight.transform.position = position;
        selectedHighlight.SetActive(true);
    }

    public void ShowHoverHighlight(Vector3 pos)
    {
        hoverHighlight.transform.position = pos;
        hoverHighlight.SetActive(true);
    }

    public void HideHoverHighlight()
    {
        hoverHighlight.SetActive(false);
    }

    public void ShowKillerHighlight(Vector3 pos)
    {
        killerHighlight.transform.position = pos;
        killerHighlight.SetActive(true);
    }

    public void HideKillerHighlightAfter(float seconds)
    {
        Invoke("HideKillerHighlight", seconds);
    }

    public void HideKillerHighlight()
    {
        killerHighlight.SetActive(false);
    }

    public void ShowCheckedHighlight(Vector3 pos)
    {
        checkedHighlight.transform.position = pos;
        checkedHighlight.SetActive(true);
    }

    public void HideCheckedHighlight()
    {
        checkedHighlight.SetActive(false);
    }
}
