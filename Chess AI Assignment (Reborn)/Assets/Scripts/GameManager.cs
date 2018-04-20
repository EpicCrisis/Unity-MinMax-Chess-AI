using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MiniMax minMax = new MiniMax();
    BoardManager board;
    OverlayCheck overlay;

    public bool playerTurn = true;
    bool kingDead = false;

    void Start()
    {
        overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
        board = BoardManager.Instance;
        board.SetupBoard();
    }

    private void Update()
    {
        if (kingDead)
        {
            //Make a restart button.
        }
        if (!playerTurn)
        {
            Move move = minMax.GetMove();
            DoAIMove(move);
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
        overlay.RemoveObjects("Highlight");

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

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
    }
}
