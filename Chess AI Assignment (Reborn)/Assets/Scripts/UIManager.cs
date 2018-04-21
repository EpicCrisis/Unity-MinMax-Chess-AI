using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TurnText;

    public static UIManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void PlayerTurnText(bool _playerTurn)
    {
        if (_playerTurn)
        {
            TurnText.text = "White Turn";
        }
        else if (!_playerTurn)
        {
            TurnText.text = "Black Turn";
        }
    }
}
