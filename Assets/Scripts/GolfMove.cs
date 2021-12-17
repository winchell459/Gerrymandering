using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolfMove
{
    public abstract bool validMove(GolfBoardTile[,] moveGraph, Vector2Int start, Vector2Int end);
    public abstract List<GolfBoardTile> getValidMoves(GolfBoardTile[,] moveGraph, Vector2Int start);
    public abstract bool validStart(GolfASP.tile_types tileType);

    protected bool validMove(GolfASP.tile_types tileType)
    {
        if (tileType == GolfASP.tile_types.air || tileType == GolfASP.tile_types.obstacle)
        {
            return false;
        }
        else { return true; }
    }
}

