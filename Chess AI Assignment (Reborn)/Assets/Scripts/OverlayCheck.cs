using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayCheck : MonoBehaviour
{
    public GameObject killHighlight;
    public GameObject moveHighlight;
    public GameObject selectHighlight;
    public GameObject lastHighlight;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RemoveObject("Highlight");
        }
    }

    public void RemoveObject(string text)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(text);
        foreach (GameObject GO in objects)
        {
            Destroy(GO);
        }
    }
}
