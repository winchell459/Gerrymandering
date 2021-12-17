using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfMoveFinder : MonoBehaviour
{
    private GolfBoardTile[,] moveGraph;
    public void GenerateMoveGraph(Dictionary<string, List<List<string>>> answerset, int minMoves, int maxMoves, int minJump, int maxJump)
    {
        int height = UtilityASP.GetMaxInt(answerset["height"]);
        int width = UtilityASP.GetMaxInt(answerset["width"]);

        moveGraph = new GolfBoardTile[width, height];
        

        foreach(List<string> tileStr in answerset["tile"])
        {
            int x = int.Parse(tileStr[0]);
            int y = int.Parse(tileStr[1]);
            GolfASP.tile_types tile_Type = (GolfASP.tile_types) System.Enum.Parse(typeof(GolfASP.tile_types), tileStr[2]);

            moveGraph[x - 1, y - 1] = new GolfBoardTile();
            moveGraph[x - 1, y - 1].tileType = tile_Type;
            moveGraph[x - 1, y - 1].x = x - 1;
            moveGraph[x - 1, y - 1].y = y - 1;
        }

        for (int i = 0; i < width; i += 1)
        {
            for (int j = 0; j < height; j += 1)
            {
                Vector2Int start = new Vector2Int(i, j);
                for(int r = minJump; r <= maxJump; r += 1)
                {
                    if(i + r < width)
                    {
                        Vector2Int end = new Vector2Int(i + r, j);
                        if(validMove(end) && validMove(start, end))
                        {
                            moveGraph[start.x, start.y].moves.Add(moveGraph[end.x, end.y]);
                        }
                    }
                    if(i - r >= 0)
                    {
                        Vector2Int end = new Vector2Int(i - r, j);
                        if (validMove(end) && validMove(start, end))
                        {
                            moveGraph[start.x, start.y].moves.Add(moveGraph[end.x, end.y]);
                        }
                    }
                    if(j + r < height)
                    {
                        Vector2Int end = new Vector2Int(i, j + r);
                        if (validMove(end) && validMove(start, end))
                        {
                            moveGraph[start.x, start.y].moves.Add(moveGraph[end.x, end.y]);
                        }
                    }
                    if(j - r >= 0)
                    {
                        Vector2Int end = new Vector2Int(i, j - r);
                        if (validMove(end) && validMove(start, end))
                        {
                            moveGraph[start.x, start.y].moves.Add(moveGraph[end.x, end.y]);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < width; i += 1)
        {
            for (int j = 0; j < height; j += 1)
            {
                moveGraph[i, j].printMoves();
            }
        }

    }

    private bool validMove(Vector2Int end)
    {
        if(moveGraph[end.x,end.y].tileType == GolfASP.tile_types.air || moveGraph[end.x, end.y].tileType == GolfASP.tile_types.obstacle)
        {
            return false;
        }
        else { return true; }
    }

    private bool validMove(Vector2Int start, Vector2Int end)
    {
        if(start.x == end.x)
        {
            int dif = end.y - start.y;
            for(int i = 1; i < Mathf.Abs(dif); i += 1)
            {
                Vector2Int check = new Vector2Int(start.x, start.y);
                if(dif > 0)
                {
                    check += new Vector2Int(0,  i);
                }else
                    check += new Vector2Int(0, - i);

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
}

public class GolfBoardTile
{
    public int x, y;
    public List<GolfBoardTile> moves = new List<GolfBoardTile>();
    public List<int> moveDistance = new List<int>();
    public GolfASP.tile_types tileType;

    public void printMoves()
    {
        string movesStr = "";
        foreach (GolfBoardTile tile in moves)
        {
            movesStr += "(" + tile.x + ", " + tile.y + ") ";
        }

        Debug.Log($"({x}, {y}) " + movesStr);
    }
}
