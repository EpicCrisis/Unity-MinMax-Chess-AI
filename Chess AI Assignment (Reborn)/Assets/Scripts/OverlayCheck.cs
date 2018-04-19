using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayCheck : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RemoveObjects("Highlight");
        }
    }

    public void RemoveObjects(string text)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(text);
        foreach (GameObject GO in objects)
        {
            Destroy(GO);
        }
    }
}
