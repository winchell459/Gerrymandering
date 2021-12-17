using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfMoveFinder : MonoBehaviour
{
    private GolfBoardTile[,] moveGraph;
    private List<List<GolfBoardTile>> movesList = new List<List<GolfBoardTile>>();

    public void GenerateMovesList(Dictionary<string, List<List<string>>> answerset, int minMoves, int maxMoves, int minJump, int maxJump)
    {
        GenerateMoveGraph(answerset, minJump, maxJump);

    }
    public void GenerateMoveGraph(Dictionary<string, List<List<string>>> answerset, int minJump, int maxJump)
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

        List<GolfMove> golveMoves = new List<GolfMove>();
        for(int r = minJump; r <= maxJump; r += 1)
        {
            GolfMoveJump jumpMove = new GolfMoveJump();
            jumpMove.jumpDistance = r;
            golveMoves.Add(jumpMove);
        }

        for (int i = 0; i < width; i += 1)
        {
            for (int j = 0; j < height; j += 1)
            {
                Vector2Int start = new Vector2Int(i, j);
                
                
                foreach(GolfMove golfMove in golveMoves)
                {
                    if (!golfMove.validStart(moveGraph[start.x, start.y].tileType)) continue;
                    foreach (GolfBoardTile tile in golfMove.getValidMoves(moveGraph, start))
                    {
                        moveGraph[start.x, start.y].AddMove(tile, golfMove);
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
    //private bool validStart(Vector2Int start)
    //{
    //    return !(moveGraph[start.x, start.y].tileType == GolfASP.tile_types.air || moveGraph[start.x, start.y].tileType == GolfASP.tile_types.obstacle);
    //}

    

    
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
        //foreach (GolfBoardTile tile in moves)
        for(int i = 0; i < moves.Count; i+=1)
        {
            GolfBoardTile tile = moves[i];
            movesStr += "(" + tile.x + ", " + tile.y + ") : " + moveDistance[i] + " ";
        }

        Debug.Log($"({x}, {y}) " + movesStr);
    }

    public void AddMove(GolfBoardTile tile, int distance)
    {
        moves.Add(tile);
        moveDistance.Add(distance);
    }

    public void AddMove(GolfBoardTile tile, GolfMove golfMove)
    {
        moves.Add(tile);
        moveDistance.Add(((GolfMoveJump) golfMove).jumpDistance);
    }
}


