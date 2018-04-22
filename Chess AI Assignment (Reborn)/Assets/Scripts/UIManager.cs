using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text TurnText;
    public Text WinnerText;
    public Text TurnCountText;

    public Button RestartButton;

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

    public void GameRestart(bool _kingDead, bool _isWhiteWin)
    {
        if (_kingDead)
        {
            RestartButton.gameObject.SetActive(true);

            if (_isWhiteWin)
            {
                WinnerText.text = "White Wins!";
            }
            else if (!_isWhiteWin)
            {
                WinnerText.text = "Black Wins!";
            }
        }
        else
        {
            return;
        }
    }

    public void TurnCount(int _turnCount)
    {
        TurnCountText.text = "Turn Count: " + _turnCount.ToString();
    }

    public void ResetLevel(int _level)
    {
        SceneManager.LoadScene(_level);
    }
}
