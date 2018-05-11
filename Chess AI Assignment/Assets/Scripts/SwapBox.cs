using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapBox : MonoBehaviour
{
    public MoveData move;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && move != null)
        {
            gameManager.SwapPieces(move);
            gameManager.TempMove = move;
        }
    }
}
