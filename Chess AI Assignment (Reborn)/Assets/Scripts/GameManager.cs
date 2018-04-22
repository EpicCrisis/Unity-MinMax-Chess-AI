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
    bool isWhiteWin = false;
    float timer = 0.0f;
    int turnCount = 0;

    [Header("===Queen Sprites===")]
    public Sprite queen_White;
    public Sprite queen_Black;

    void Start()
    {
        overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
        board = BoardManager.Instance;
        uiManager = UIManager.Instance;

        board.SetupBoard();

        uiManager.TurnCount(turnCount);
        uiManager.PlayerTurnText(playerTurn);
    }

    private void Update()
    {
        if (kingDead)
        {
            //Make a restart button.
            uiManager.GameRestart(kingDead, isWhiteWin);
        }
        else if (!kingDead)
        {
            if (!playerTurn && timer >= 1.0f)
            {
                timer = 0.0f;
                Move move = minMax.GetMove();
                DoAIMove(move);

                //playerTurn = !playerTurn;
                //uiManager.PlayerTurnText(playerTurn);
            }
            else if (!playerTurn)
            {
                timer += Time.deltaTime;
            }
        }
    }

    void DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        SwapPieces(move);
    }

    public void SwapPieces(Move move)
    {
        overlay.RemoveObject("Highlight");
        overlay.RemoveObject("LastTag");

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        LastMoveTag(move);

        firstTile.CurrentPiece.MovePiece(new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y));

        ConvertPawn(firstTile, move);

        CheckKingDeath(secondTile);

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.chessPosition = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        turnCount++;
        playerTurn = !playerTurn;

        uiManager.TurnCount(turnCount);
        uiManager.PlayerTurnText(playerTurn);
    }

    void CheckKingDeath(Tile _secondTile)
    {
        if (_secondTile.CurrentPiece != null)
        {
            if (_secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)
            {
                kingDead = true;
                if (_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
                {
                    isWhiteWin = true;
                }
                else if (_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
                {
                    isWhiteWin = false;
                }
            }
            Destroy(_secondTile.CurrentPiece.gameObject);
        }
    }

    //Special rule, pawn becomes queen.
    void ConvertPawn(Tile _firstTile, Move _move)
    {
        if (_firstTile.CurrentPiece.Type == ChessPiece.PieceType.PAWN)
        {
            if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
            {
                if (_move.secondPosition.Position.y == 7)
                {
                    _firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.QUEEN, queen_White);
                }
            }
            else if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
            {
                if (_move.secondPosition.Position.y == 0)
                {
                    _firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.QUEEN, queen_Black);
                }
            }
        }
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

    public bool KingDead
    {
        get
        {
            return kingDead;
        }
        set
        {
            kingDead = value;
        }
    }
}
