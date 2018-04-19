using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Tile firstPosition = null;
    public Tile secondPosition = null;
    public ChessPiece pieceMoved = null;
    public ChessPiece pieceKilled = null;
    public int score = -100000000;
}
