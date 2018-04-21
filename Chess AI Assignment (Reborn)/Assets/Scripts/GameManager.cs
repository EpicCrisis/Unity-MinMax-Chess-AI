using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MiniMax minMax = new MiniMax();
    UIManager uiManager;
    BoardManager board;
    OverlayCheck overlay;

    public bool playerTurn = true;
    bool kingDead = false;
    float timer = 0.0f;

    void Start()
    {
        overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
        board = BoardManager.Instance;
        uiManager = UIManager.Instance;

        board.SetupBoard();

        uiManager.PlayerTurnText(playerTurn);
    }

    private void Update()
    {
        if (kingDead)
        {
            //Make a restart button.
        }
        if (!playerTurn && timer >= 1.0f)
        {
            timer = 0.0f;
            Move move = minMax.GetMove();
            DoAIMove(move);
        }
        else if (!playerTurn)
        {
            timer += Time.deltaTime;
        }
    }

    void DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == ChessPiece.PieceType.KING)
        {
            SwapPieces(move);
            kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }

    public void SwapPieces(Move move)
    {
        overlay.RemoveObject("Highlight");
        overlay.RemoveObject("LastTag");

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        LastMoveTag(move);

        firstTile.CurrentPiece.MovePiece(new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y));

        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)
            {
                kingDead = true;
            }
            Destroy(secondTile.CurrentPiece.gameObject);
        }
        
        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.chessPosition = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        playerTurn = !playerTurn;
        uiManager.PlayerTurnText(playerTurn);
    }

    void LastMoveTag(Move move)
    {
        GameObject GOfrom = Instantiate(overlay.lastHighlight);
        GOfrom.transform.position = new Vector2(move.firstPosition.Position.x, move.firstPosition.Position.y);
        GOfrom.transform.parent = transform;

        GameObject GOto = Instantiate(overlay.lastHighlight);
        GOto.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
        GOto.transform.parent = transform;
    }
}
