using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHeuristic
{
    int[,] PawnWhiteBoardWeight = new int[,]
    {
        { 5,  5,  5,  5,  5,  5,  5,  5},
        {30, 30, 30, 30, 30, 30, 30, 30},
        {10, 10, 20, 30, 30, 20, 10, 10},
        { 5,  5, 10, 25, 25, 10,  5,  5},
        { 0,  0,  0, 20, 20,  0,  0,  0},
        { 5, -5,-10,  0,  0,-10, -5,  5},
        { 5, 10, 10,-20,-20, 10, 10,  5},
        { 5,  5,  5,  5,  5,  5,  5,  5}
    };

    int[,] PawnBlackBoardWeight = new int[,]
    {
        { 0,  0,  0,  0,  0,  0,  0,  0},
        { 5, 10, 10,-20,-20, 10, 10,  5},
        { 5, -5,-10,  0,  0,-10, -5,  5},
        { 0,  0,  0, 20, 20,  0,  0,  0},
        { 5,  5, 10, 25, 25, 10,  5,  5},
        {10, 10, 20, 30, 30, 20, 10, 10},
        {30, 30, 30, 30, 30, 30, 30, 30},
        { 5,  5,  5,  5,  5,  5,  5,  5},
    };

    int[,] BishopWhiteBoardWeight = new int[,]
    {
        {-20,-10,-10,-10,-10,-10,-10,-20},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-10,  0,  5, 10, 10,  5,  0,-10},
        {-10,  5,  5, 10, 10,  5,  5,-10},
        {-10,  0, 10, 10, 10, 10,  0,-10},
        {-10, 10, 10, 10, 10, 10, 10,-10},
        {-10,  5,  0,  0,  0,  0,  5,-10},
        {-20,-10,-10,-10,-10,-10,-10,-20}
    };

    int[,] BishopBlackBoardWeight = new int[,]
    {
        {-20,-10,-10,-10,-10,-10,-10,-20},
        {-10,  5,  0,  0,  0,  0,  5,-10},
        {-10, 10, 10, 10, 10, 10, 10,-10},
        {-10,  0, 10, 10, 10, 10,  0,-10},
        {-10,  5,  5, 10, 10,  5,  5,-10},
        {-10,  0,  5, 10, 10,  5,  0,-10},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-20,-10,-10,-10,-10,-10,-10,-20}
    };

    int[,] KnightWhiteBoardWeight = new int[,]
    {
        {-50,-40,-30,-30,-30,-30,-40,-50},
        {-40,-20,  0,  0,  0,  0,-20,-40},
        {-30,  0, 10, 15, 15, 10,  0,-30},
        {-30,  5, 15, 25, 25, 15,  5,-30},
        {-30,  0, 15, 25, 25, 15,  0,-30},
        {-30,  5, 10, 15, 15, 10,  5,-30},
        {-40,-20,  0,  5,  5,  0,-20,-40},
        {-50,-40,-30,-30,-30,-30,-40,-50}
    };

    int[,] KnightBlackBoardWeight = new int[,]
    {
        {-50,-40,-30,-30,-30,-30,-40,-50},
        {-40,-20,  0,  5,  5,  0,-20,-40},
        {-30,  5, 10, 15, 15, 10,  5,-30},
        {-30,  0, 15, 25, 25, 15,  0,-30},
        {-30,  5, 15, 25, 25, 15,  5,-30},
        {-30,  0, 10, 15, 15, 10,  0,-30},
        {-40,-20,  0,  0,  0,  0,-20,-40},
        {-50,-40,-30,-30,-30,-30,-40,-50}
    };

    int[,] RookWhiteBoardWeight = new int[,]
    {
        { 0,  0,  0,  0,  0,  0,  0,  0},
        { 5, 10, 10, 10, 10, 10, 10,  5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        { 0,  0,  0,  5,  5,  0,  0,  0}
    };

    int[,] RookBlackBoardWeight = new int[,]
    {
        { 0,  0,  0,  5,  5,  0,  0,  0},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        { 5, 10, 10, 10, 10, 10, 10,  5},
        { 0,  0,  0,  0,  0,  0,  0,  0}
    };

    int[,] QueenWhiteBoardWeight = new int[,]
    {
        {-20,-10,-10, -5, -5,-10,-10,-20},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-10,  0,  5, 10, 10,  5,  0,-10},
        { -5,  0, 10, 15, 15, 10,  0, -5},
        {  0,  0, 10, 15, 15, 10,  0, -5},
        {-10,  5,  5, 10, 10,  5,  0,-10},
        {-10,  0,  5,  0,  0,  0,  0,-10},
        {-20,-10,-10, -5, -5,-10,-10,-20}
    };

    int[,] QueenBlackBoardWeight = new int[,]
    {
        {-20,-10,-10, -5, -5,-10,-10,-20},
        {-10,  0,  5,  0,  0,  0,  0,-10},
        {-10,  5,  5,  5,  5,  5,  0,-10},
        {  0,  0,  5,  5,  5,  5,  0, -5},
        { -5,  0,  5,  5,  5,  5,  0, -5},
        {-10,  0,  5,  5,  5,  5,  0,-10},
        {-10,  0,  0,  0,  0,  0,  0,-10},
        {-20,-10,-10, -5, -5,-10,-10,-20}
    };

    int[,] KingWhiteBoardWeight =
    {
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-20,-30,-30,-40,-40,-30,-30,-20},
        {-10,-20,-20,-20,-20,-20,-20,-10},
        { 20, 20,  0,  0,  0,  0, 20, 20},
        { 20, 30, 10,  0,  0, 10, 30, 20}
    };

    int[,] KingBlackBoardWeight =
    {
        { 20, 30, 10,  0,  0, 10, 30, 20},
        { 20, 20,  0,  0,  0,  0, 20, 20},
        {-10,-20,-20,-20,-20,-20,-20,-10},
        {-20,-30,-30,-40,-40,-30,-30,-20},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
        {-30,-40,-40,-50,-50,-40,-40,-30},
    };

    public int GetBoardWeight(ChessPiece.PieceType type, Vector2 position, ChessPiece.PlayerTeam team)
    {
        switch (type)
        {
            case ChessPiece.PieceType.PAWN:
                if (team == ChessPiece.PlayerTeam.WHITE)
                {
                    return PawnWhiteBoardWeight[(int)position.x, (int)position.y];
                }
                else
                {
                    return PawnBlackBoardWeight[(int)position.x, (int)position.y];
                }
            case ChessPiece.PieceType.ROOK:
                if (team == ChessPiece.PlayerTeam.WHITE)
                {
                    return RookWhiteBoardWeight[(int)position.x, (int)position.y];
                }
                else
                {
                    return RookBlackBoardWeight[(int)position.x, (int)position.y];
                }
            case ChessPiece.PieceType.KNIGHT:
                if (team == ChessPiece.PlayerTeam.WHITE)
                {
                    return KnightWhiteBoardWeight[(int)position.x, (int)position.y];
                }
                else
                {
                    return KnightBlackBoardWeight[(int)position.x, (int)position.y];
                }
            case ChessPiece.PieceType.BISHOP:
                if (team == ChessPiece.PlayerTeam.WHITE)
                {
                    return BishopWhiteBoardWeight[(int)position.x, (int)position.y];
                }
                else
                {
                    return BishopBlackBoardWeight[(int)position.x, (int)position.y];
                }
            case ChessPiece.PieceType.QUEEN:
                if (team == ChessPiece.PlayerTeam.WHITE)
                {
                    return QueenWhiteBoardWeight[(int)position.x, (int)position.y];
                }
                else
                {
                    return QueenBlackBoardWeight[(int)position.x, (int)position.y];
                }
            case ChessPiece.PieceType.KING:
                if (team == ChessPiece.PlayerTeam.WHITE)
                {
                    return KingWhiteBoardWeight[(int)position.x, (int)position.y];
                }
                else
                {
                    return KingBlackBoardWeight[(int)position.x, (int)position.y];
                }
            default:
                return -1;
        }
    }

    public int GetPieceWeight(ChessPiece.PieceType type)
    {
        switch (type)
        {
            case ChessPiece.PieceType.PAWN:
                return 1;
            case ChessPiece.PieceType.ROOK:
                return 5;
            case ChessPiece.PieceType.KNIGHT:
                return 3;
            case ChessPiece.PieceType.BISHOP:
                return 3;
            case ChessPiece.PieceType.QUEEN:
                return 9;
            case ChessPiece.PieceType.KING:
                return 1000000;
            default:
                return -1;
        }
    }
}
