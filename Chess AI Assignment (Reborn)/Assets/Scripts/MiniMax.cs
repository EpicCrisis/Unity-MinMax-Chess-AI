using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax 
{
    BoardManager board;
    GameManager gameManager;

    List<Tile> tilesWithPieces = new List<Tile>();
    List<Tile> blackPieces = new List<Tile>();
    List<Tile> whitePieces = new List<Tile>();

    List<Move> moves = new List<Move>();
    Stack<Move> moveStack = new Stack<Move>();
    Move bestMove;

    MoveWeight weight = new MoveWeight();
    Tile[,] localBoard = new Tile[8, 8];

    int whiteScore = 0;
    int blackScore = 0;
    int maxDepth = 3;

    public Move GetMove()
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
            List<Move> allMoves = GetMoves(ChessPiece.PlayerTeam.BLACK);
            foreach (Move move in allMoves)
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
            List<Move> allMoves = GetMoves(ChessPiece.PlayerTeam.WHITE);
            foreach (Move move in allMoves)
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
        Move tempMove = moveStack.Pop();
        Tile movedTo = tempMove.secondPosition;
        Tile movedFrom = tempMove.firstPosition;
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

    void DoFakeMove(Tile currentTile, Tile targetTile)
    {
        targetTile.SwapFakePieces(currentTile.CurrentPiece);
        currentTile.CurrentPiece = null;
    }

    List<Move> GetMoves(ChessPiece.PlayerTeam team)
    {
        List<Move> turnMove = new List<Move>();
        List<Tile> pieces = new List<Tile>();

        if (team == ChessPiece.PlayerTeam.BLACK)
        {
            pieces = blackPieces;
        }
        else
        {
            pieces = whitePieces;
        }

        foreach (Tile tile in pieces)
        {
            MoveFactory factory = new MoveFactory(board);
            List<Move> pieceMoves = factory.GetMoves(tile.CurrentPiece, tile.Position);

            foreach (Move move in pieceMoves)
            {
                Move newMove = CreateMove(move.firstPosition, move.secondPosition);
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

        foreach (Tile tile in whitePieces)
        {
            whiteWeight += weight.GetBoardWeight(tile.CurrentPiece.Type, tile.CurrentPiece.chessPosition, ChessPiece.PlayerTeam.WHITE);
        }
        foreach (Tile tile in blackPieces)
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
        foreach (Tile tile in tilesWithPieces)
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

    Move CreateMove(Tile tile, Tile move)
    {
        Move tempMove = new Move
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
