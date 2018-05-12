using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax
{
    BoardManager board;
    GameManager gameManager;

    List<TileData> tilesWithPieces = new List<TileData>();
    List<TileData> blackPieces = new List<TileData>();
    List<TileData> whitePieces = new List<TileData>();

    List<MoveData> moves = new List<MoveData>();
    Stack<MoveData> moveStack = new Stack<MoveData>();
    MoveData bestMove;

    MoveHeuristic weight = new MoveHeuristic();
    TileData[,] localBoard = new TileData[8, 8];

    int whiteScore = 0;
    int blackScore = 0;
    int maxDepth = 3;
    bool fakeLose = false;

    public int MaxDepth
    {
        get
        {
            return maxDepth;
        }
        set
        {
            maxDepth = value;
        }
    }

    public static MiniMax Instance = null;

    public MoveData GetMove()
    {
        board = BoardManager.Instance;
        gameManager = GameManager.Instance;
        bestMove = CreateMove(board.GetTileFromBoard(new Vector2(0, 0)), board.GetTileFromBoard(new Vector2(0, 0)));
        CalculateMinMax(maxDepth, int.MinValue, int.MaxValue, true);
        return bestMove;
    }

    int CalculateMinMax(int depth, int alpha, int beta, bool max)
    {
        GetBoardState();

        if (depth == 0)
        {
            return Evaluate();
        }
        if (max)
        {
            int score = int.MinValue;
            List<MoveData> allMoves = GetMoves(ChessPiece.PlayerTeam.BLACK);
            foreach (MoveData move in allMoves)
            {
                moveStack.Push(move);

                DoFakeMove(move.firstPosition, move.secondPosition);

                score = CalculateMinMax(depth - 1, alpha, beta, false);

                UndoFakeMove();

                if (score > alpha)
                {
                    move.score = score;
                    if (move.score > bestMove.score && depth >= maxDepth)
                    {
                         bestMove = move;
                    }
                    alpha = score;
                }
                if (score >= beta)
                {
                    break;
                }
            }
            return alpha;
        }
        else
        {
            int score = int.MaxValue;
            List<MoveData> allMoves = GetMoves(ChessPiece.PlayerTeam.WHITE);
            foreach (MoveData move in allMoves)
            {
                moveStack.Push(move);

                DoFakeMove(move.firstPosition, move.secondPosition);

                score = CalculateMinMax(depth - 1, alpha, beta, true);

                UndoFakeMove();

                if (score < beta)
                {
                    move.score = score;
                    beta = score;
                }
                if (score <= alpha)
                {
                    break;
                }
            }
            return beta;
        }
    }

    void UndoFakeMove()
    {
        MoveData tempMove = moveStack.Pop();
        TileData movedTo = tempMove.secondPosition;
        TileData movedFrom = tempMove.firstPosition;
        ChessPiece pieceKilled = tempMove.pieceKilled;
        ChessPiece pieceMoved = tempMove.pieceMoved;

        movedFrom.CurrentPiece = movedTo.CurrentPiece;

        if (pieceKilled != null)
        {
            movedTo.CurrentPiece = pieceKilled;
        }
        else
        {
            movedTo.CurrentPiece = null;
        }
    }

    void DoFakeMove(TileData currentTile, TileData targetTile)
    {
        if (targetTile.CurrentPiece != null)
        {
            if (targetTile.CurrentPiece.Type == ChessPiece.PieceType.KING &&
                targetTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
            {
                Debug.Log("King is being targeted!");
                fakeLose = true;
            }
            else
            {
                fakeLose = false;
            }
        }

        targetTile.SwapFakePieces(currentTile.CurrentPiece);
        currentTile.CurrentPiece = null;
    }

    List<MoveData> GetMoves(ChessPiece.PlayerTeam team)
    {
        List<MoveData> turnMove = new List<MoveData>();
        List<TileData> pieces = new List<TileData>();

        if (team == ChessPiece.PlayerTeam.BLACK)
        {
            pieces = blackPieces;
        }
        else
        {
            pieces = whitePieces;
        }

        foreach (TileData tile in pieces)
        {
            MoveFunction movement = new MoveFunction(board);
            List<MoveData> pieceMoves = movement.GetMoves(tile.CurrentPiece, tile.Position);

            foreach (MoveData move in pieceMoves)
            {
                MoveData newMove = CreateMove(move.firstPosition, move.secondPosition);
                turnMove.Add(newMove);
            }
        }
        return turnMove;
    }

    int Evaluate()
    {
        float pieceDifference = 0;
        float whiteWeight = 0;
        float blackWeight = 0;

        foreach (TileData tile in whitePieces)
        {
            whiteWeight += weight.GetBoardWeight(tile.CurrentPiece.Type, tile.CurrentPiece.chessPosition, ChessPiece.PlayerTeam.WHITE);
        }
        foreach (TileData tile in blackPieces)
        {
            blackWeight += weight.GetBoardWeight(tile.CurrentPiece.Type, tile.CurrentPiece.chessPosition, ChessPiece.PlayerTeam.BLACK);
        }
        pieceDifference = (blackScore + (blackWeight / 100)) - (whiteScore + (whiteWeight / 100));
        return Mathf.RoundToInt(pieceDifference * 100);
    }

    void GetBoardState()
    {
        blackPieces.Clear();
        whitePieces.Clear();
        blackScore = 0;
        whiteScore = 0;
        tilesWithPieces.Clear();

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                localBoard[x, y] = board.GetTileFromBoard(new Vector2(x, y));
                if (localBoard[x, y].CurrentPiece != null && localBoard[x, y].CurrentPiece.Type != ChessPiece.PieceType.NONE)
                {
                    tilesWithPieces.Add(localBoard[x, y]);
                }
            }
        }
        foreach (TileData tile in tilesWithPieces)
        {
            if (tile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
            {
                blackScore += weight.GetPieceWeight(tile.CurrentPiece.Type);
                blackPieces.Add(tile);
            }
            else
            {
                whiteScore += weight.GetPieceWeight(tile.CurrentPiece.Type);
                whitePieces.Add(tile);
            }
        }
    }

    MoveData CreateMove(TileData tile, TileData move)
    {
        MoveData tempMove = new MoveData
        {
            firstPosition = tile,
            pieceMoved = tile.CurrentPiece,
            secondPosition = move
        };

        if (move.CurrentPiece != null)
        {
            tempMove.pieceKilled = move.CurrentPiece;
        }

        return tempMove;
    }
}
