using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{
    private static BoardManager instance = null;
    public static BoardManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BoardManager();
            }
            return instance;
        }
    }

    private TileData[,] board = new TileData[8, 8];

    public void SetupBoard()
    {
        for (int y = 0; y < 8; ++y)
        {
            for (int x = 0; x < 8; ++x)
            {
                board[x, y] = new TileData(x, y);
            }
        }
    }

    public TileData GetTileFromBoard(Vector2 tile)
    {
        return board[(int)tile.x, (int)tile.y];
    }
}
