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

    public Sprite chessSprite = null;
    public Vector2 chessPosition;
    private Vector2 moveTo;

    private MoveFactory factory = new MoveFactory(BoardManager.Instance);
    private List<Move> moves = new List<Move>();

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
        moveTo = transform.position;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
    }

    void Update()
    {
        transform.position = moveTo;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && team == PlayerTeam.WHITE && gameManager.playerTurn)
        {
            moves.Clear();
            overlay.RemoveObjects("Highlight");

            moves = factory.GetMoves(this, chessPosition);
            foreach (Move move in moves)
            {
                if (move.pieceKilled == null)
                {
                    GameObject GO = Instantiate(overlay.moveHighlight);
                    GO.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
                    GO.GetComponent<Container>().move = move;
                    GO.transform.parent = transform;
                }
                else if (move.pieceKilled != null)
                {
                    GameObject GO = Instantiate(overlay.killHighlight);
                    GO.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
                    GO.GetComponent<Container>().move = move;
                    GO.transform.parent = transform;
                }
            }
            GameObject currentGO = Instantiate(overlay.selectHighlight);
            currentGO.transform.position = transform.position;
            currentGO.transform.parent = transform;
        }
    }

    public void MovePiece(Vector2 position)
    {
        moveTo = position;
    }
}
