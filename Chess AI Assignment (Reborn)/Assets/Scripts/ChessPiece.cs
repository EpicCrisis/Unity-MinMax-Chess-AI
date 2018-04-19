using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public enum PieceType
    {
        NONE = -1,
        PAWN,
        BISHOP,
        KNIGHT,
        ROOK,
        QUEEN,
        KING,
    };

    public enum PlayerTeam
    {
        NONE = -1,
        WHITE,
        BLACK,
    };

    [SerializeField] private PieceType type = PieceType.NONE;
    [SerializeField] private PlayerTeam team = PlayerTeam.NONE;

    public PieceType Type
    {
        get
        {
            return type;
        }
    }
    public PlayerTeam Team
    {
        get
        {
            return team;
        }
    }

    public Sprite chessSprite = null;
    public Vector2 chessPosition = Vector2.zero;
    private Vector2 moveTo = Vector2.zero;
    private GameManager manager;

    private MoveFactory factory = new MoveFactory(BoardManager.Instance);
    private List<Move> moves = new List<Move>();

    private bool hasMoved = false;
    public bool HasMoved
    {
        get
        {
            return hasMoved;
        }
    }

    OverlayCheck overlay = new OverlayCheck();
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && team == PlayerTeam.WHITE && manager.playerTurn)
        {
            moves.Clear();
            overlay.RemoveObjects("Highlight");

            moves = factory.GetMoves(this, chessPosition);
            foreach (Move move in moves)
            {

            }
        }
    }
}
