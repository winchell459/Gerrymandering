using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfMoveJump : GolfMove
{
    public int jumpDistance;
    public override List<GolfBoardTile> getValidMoves(GolfBoardTile[,] moveGraph, Vector2Int start)
    {
        int width = moveGraph.GetUpperBound(0) + 1;
        int height = moveGraph.GetUpperBound(1) + 1;
        int i = start.x;
        int j = start.y;
        int r = jumpDistance;

        List<GolfBoardTile> validMoveTiles = new List<GolfBoardTile>();
        if (i + r < width)
        {
            Vector2Int end = new Vector2Int(i + r, j);
            if (validMove(moveGraph[end.x, end.y].tileType) && validMove(moveGraph, start, end))
            {
                //moveGraph[start.x, start.y].AddMove(moveGraph[end.x, end.y], r);
                validMoveTiles.Add(moveGraph[end.x, end.y]);
            }
        }
        if (i - r >= 0)
        {
            Vector2Int end = new Vector2Int(i - r, j);
            if (validMove(moveGraph[end.x, end.y].tileType) && validMove(moveGraph, start, end))
            {
                //moveGraph[start.x, start.y].AddMove(moveGraph[end.x, end.y], r);
                validMoveTiles.Add(moveGraph[end.x, end.y]);
            }
        }
        if (j + r < height)
        {
            Vector2Int end = new Vector2Int(i, j + r);
            if (validMove(moveGraph[end.x, end.y].tileType) && validMove(moveGraph, start, end))
            {
                //moveGraph[start.x, start.y].AddMove(moveGraph[end.x, end.y], r);
                validMoveTiles.Add(moveGraph[end.x, end.y]);
            }
        }
        if (j - r >= 0)
        {
            Vector2Int end = new Vector2Int(i, j - r);
            if (validMove(moveGraph[end.x, end.y].tileType) && validMove(moveGraph, start, end))
            {
                //moveGraph[start.x, start.y].AddMove(moveGraph[end.x, end.y], r);
                validMoveTiles.Add(moveGraph[end.x, end.y]);
            }
        }

        return validMoveTiles;
    }
    public override bool validMove(GolfBoardTile[,] moveGraph, Vector2Int start, Vector2Int end)
    {
        if (start.x == end.x)
        {
            int dif = end.y - start.y;
            for (int i = 1; i < Mathf.Abs(dif); i += 1)
            {
                Vector2Int check = new Vector2Int(start.x, start.y);
                if (dif > 0)
                {
                    check += new Vector2Int(0, i);
                }
                else
                    check += new Vector2Int(0, -i);

                if (moveGraph[check.x, check.y].tileType == GolfASP.tile_types.obstacle) return false;
            }
        }
        else
        {
            int dif = end.x - start.x;
            for (int i = 1; i < Mathf.Abs(dif); i += 1)
            {
                Vector2Int check = new Vector2Int(start.x, start.y);
                if (dif > 0)
                {
                    check += new Vector2Int(i, 0);
                }
                else
                    check += new Vector2Int(-i, 0);

                if (moveGraph[check.x, check.y].tileType == GolfASP.tile_types.obstacle) return false;
            }
        }


        return true;
    }

    public override bool validStart(GolfASP.tile_types tileType)
    {
        return !(tileType == GolfASP.tile_types.air || tileType == GolfASP.tile_types.obstacle);
    }
}
