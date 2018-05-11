using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFunction
{
    BoardManager board;
    List<MoveData> moves = new List<MoveData>();
    Dictionary<ChessPiece.PieceType, System.Action> pieceToFunction =
        new Dictionary<ChessPiece.PieceType, System.Action>();

    private ChessPiece piece;
    private ChessPiece.PieceType type;
    private ChessPiece.PlayerTeam team;
    private Vector2 position;

    public List<MoveData> GetMoves(ChessPiece _piece, Vector2 _position)
    {
        piece = _piece;
        type = _piece.Type;
        team = _piece.Team;
        position = _position;

        foreach (KeyValuePair<ChessPiece.PieceType, System.Action> P in pieceToFunction)
        {
            if (type == P.Key)
            {
                P.Value.Invoke();
            }
        }

        return moves;
    }

    void GetPawnMoves()
    {
        if (piece.Team == ChessPiece.PlayerTeam.BLACK)
        {
            int limit = piece.HasMoved ? 2 : 3;
            GenerateMove(limit, new Vector2(0, -1));

            Vector2 diagLeft = new Vector2(position.x - 1, position.y - 1);
            Vector2 diagRight = new Vector2(position.x + 1, position.y - 1);

            TileData dl = null;
            TileData dr = null;

            if (IsOnBoard(diagLeft))
            {
                dl = board.GetTileFromBoard(diagLeft);
            }
            if (IsOnBoard(diagRight))
            {
                dr = board.GetTileFromBoard(diagRight);
            }

            if (dl != null && ContainsPiece(dl) && IsEnemy(dl))
            {
                CheckAndStoreMove(diagLeft);
            }
            if (dr != null && ContainsPiece(dr) && IsEnemy(dr))
            {
                CheckAndStoreMove(diagRight);
            }
        }
        else if (piece.Team == ChessPiece.PlayerTeam.WHITE)
        {
            int limit = piece.HasMoved ? 2 : 3;
            GenerateMove(limit, new Vector2(0, 1));
            
            Vector2 diagLeft = new Vector2(position.x - 1, position.y + 1);
            Vector2 diagRight = new Vector2(position.x + 1, position.y + 1);

            TileData dl = null;
            TileData dr = null;

            if (IsOnBoard(diagLeft))
            {
                dl = board.GetTileFromBoard(diagLeft);
            }
            if (IsOnBoard(diagRight))
            {
                dr = board.GetTileFromBoard(diagRight);
            }

            if (dl != null && ContainsPiece(dl) && IsEnemy(dl))
            {
                CheckAndStoreMove(diagLeft);
            }
            if (dr != null && ContainsPiece(dr) && IsEnemy(dr))
            {
                CheckAndStoreMove(diagRight);
            }
        }
    }

    void GetRookMoves()
    {
        GenerateMove(9, new Vector2( 0,  1));
        GenerateMove(9, new Vector2( 0, -1));
        GenerateMove(9, new Vector2( 1,  0));
        GenerateMove(9, new Vector2(-1,  0));
    }

    void GetKnightMoves()
    {
        Vector2 move;

        move = new Vector2(position.x + 1, position.y - 2);
        CheckAndStoreMove(move);
        move = new Vector2(position.x + 1, position.y + 2);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 1, position.y + 2);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 1, position.y - 2);
        CheckAndStoreMove(move);

        move = new Vector2(position.x + 2, position.y + 1);
        CheckAndStoreMove(move);
        move = new Vector2(position.x + 2, position.y - 1);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 2, position.y + 1);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 2, position.y - 1);
        CheckAndStoreMove(move);
    }

    void GetBishopMoves()
    {
        GenerateMove(9, new Vector2( 1,  1));
        GenerateMove(9, new Vector2( 1, -1));
        GenerateMove(9, new Vector2(-1,  1));
        GenerateMove(9, new Vector2(-1, -1));
    }

    void GetQueenMoves()
    {
        GetBishopMoves();
        GetRookMoves();
    }

    void GetKingMoves()
    {
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                CheckAndStoreMove(new Vector2(position.x + x, position.y + y));
            }
        }
    }
    
    void GenerateMove(int limit, Vector2 direction)
    {
        for (int i = 1; i < limit; ++i)
        {
            Vector2 move = position + direction * i;
            if (IsOnBoard(move) && ContainsPiece(board.GetTileFromBoard(move)))
            {
                if (IsEnemy(board.GetTileFromBoard(move)) && type != ChessPiece.PieceType.PAWN)
                {
                    CheckAndStoreMove(move);
                }
                break;
            }
            CheckAndStoreMove(move);
        }
    }

    void CheckAndStoreMove(Vector2 move)
    {
        if (IsOnBoard(move) && (!ContainsPiece(board.GetTileFromBoard(move)) || 
            IsEnemy(board.GetTileFromBoard(move))))
        {
            MoveData M = new MoveData
            {
                firstPosition = board.GetTileFromBoard(position),
                pieceMoved = piece,
                secondPosition = board.GetTileFromBoard(move)
            };

            if (M.secondPosition != null)
            {
                M.pieceKilled = M.secondPosition.CurrentPiece;
            }

            moves.Add(M);
        }
    }

    public MoveFunction(BoardManager _board)
    {
        board = _board;
        pieceToFunction.Add(ChessPiece.PieceType.PAWN, GetPawnMoves);
        pieceToFunction.Add(ChessPiece.PieceType.ROOK, GetRookMoves);
        pieceToFunction.Add(ChessPiece.PieceType.KNIGHT, GetKnightMoves);
        pieceToFunction.Add(ChessPiece.PieceType.BISHOP, GetBishopMoves);
        pieceToFunction.Add(ChessPiece.PieceType.QUEEN, GetQueenMoves);
        pieceToFunction.Add(ChessPiece.PieceType.KING, GetKingMoves);
    }

    bool IsOnBoard(Vector2 point)
    {
        if (point.x >= 0 && point.y >= 0 && point.x < 8 && point.y < 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool ContainsPiece(TileData tile)
    {
        if (!IsOnBoard(tile.Position))
        {
            return false;
        }

        if (tile.CurrentPiece != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsEnemy(TileData tile)
    {
        if (team != tile.CurrentPiece.Team)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
