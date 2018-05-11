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

    private GameManager gameManager;
    private OverlayCheck overlay;
    private SpriteRenderer sRend;

    public Vector2 chessPosition;
    private Vector2 moveTo;

    private MoveFunction movement = new MoveFunction(BoardManager.Instance);
    private List<MoveData> moves = new List<MoveData>();

    public List<Sprite> sprites = null;

    private bool hasMoved = false;
    public bool HasMoved
    {
        get
        {
            return hasMoved;
        }
        set
        {
            hasMoved = value;
        }
    }
        
    void Start()
    {
        transform.position = chessPosition;
        moveTo = transform.position;

        sRend = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
    }

    void Update()
    {
        transform.position = moveTo;
    }

    private void OnMouseOver()
    {
        if (!gameManager.KingDead && !gameManager.PlayerMoved)
        {
            if (Input.GetMouseButtonDown(0) && team == PlayerTeam.WHITE && gameManager.playerTurn)
            {
                moves.Clear();
                overlay.RemoveObject("Highlight");

                moves = movement.GetMoves(this, chessPosition);
                foreach (MoveData move in moves)
                {
                    if (move.pieceKilled == null)
                    {
                        GameObject GO = Instantiate(overlay.moveHighlight);
                        GO.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
                        GO.GetComponent<SwapBox>().move = move;
                        GO.transform.parent = transform;
                    }
                    else if (move.pieceKilled != null)
                    {
                        GameObject GO = Instantiate(overlay.killHighlight);
                        GO.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
                        GO.GetComponent<SwapBox>().move = move;
                        GO.transform.parent = transform;
                    }
                }
                GameObject currentGO = Instantiate(overlay.selectHighlight);
                currentGO.transform.position = transform.position;
                currentGO.transform.parent = transform;
            }
        }
    }

    public void MovePiece(Vector2 position)
    {
        moveTo = position;
    }

    public void SetType(int _type, Sprite _sprite)
    {
        type = (PieceType)_type;
        sRend.sprite = _sprite;
    }
}
