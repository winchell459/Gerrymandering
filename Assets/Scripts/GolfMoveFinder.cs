using System.Collections.Generic;
using UnityEngine;

public class GolfMoveFinder : MonoBehaviour
{
    private GolfBoardTile[,] moveGraph;
    private List<List<GolfBoardTile>> golfBoardTilePaths = new List<List<GolfBoardTile>>();
    private GolfBoardTile startTile, endTile;
    private List<int> movesList = new List<int>();

    public List<int> MovesList { get { return movesList; } }
    public Vector2Int GetStartLoc()
    {
        return new Vector2Int(startTile.x, startTile.y);
    }

    public Vector2Int GetEndLoc()
    {
        return new Vector2Int(endTile.x, endTile.y);
    }

    public void GenerateMoves(Dictionary<string, List<List<string>>> answerset, int minMoves, int maxMoves, int minJump, int maxJump)
    {
        
        GenerateMoveGraph(answerset, minJump, maxJump);
        GeneratePaths(minMoves, maxMoves);
        movesList = new List<int>();
        for (int i = 0; i <= maxJump; i+= 1)
        {
            movesList.Add(0);
        }
        foreach(List<GolfBoardTile> path in golfBoardTilePaths)
        {
            printPath(path);
            List<int> pathMoves = new List<int>();
            for (int i = 0; i <= maxJump; i += 1)
            {
                pathMoves.Add(0);
            }
            for(int i = 0; i < path.Count - 1; i += 1)
            {
                int jump = Mathf.Abs(path[i].x - path[i + 1].x) + Mathf.Abs(path[i].y - path[i + 1].y);
                pathMoves[jump] += 1;
            }
            for(int i = 0; i < pathMoves.Count; i += 1)
            {
                movesList[i] = Mathf.Max(movesList[i], pathMoves[i]);
            }
        }
        string moves = "";
        for(int i = 0; i < movesList.Count; i+=1)
        {
            if (movesList[i] > 0) moves += movesList[i]+ " - " + i + " jumps ";
        }
        Debug.Log(moves);
        Debug.Log($"movesList.Count: {movesList.Count} minMoves: {minMoves} maxMoves:{maxMoves} minJumps{minJump}  maxJumps{maxJump}");
    }

    public void GeneratePaths(/*Dictionary<string, List<List<string>>> answerset,*/ int minMoves, int maxMoves/*, int minJump, int maxJump*/)
    {
        
        List<GolfBoardTile> startPath = new List<GolfBoardTile>();
        startPath.Add(startTile);
        GeneratePaths(startPath, minMoves, maxMoves);
        //foreach(List<GolfBoardTile> path in golfBoardTilePaths)
        //{
        //    Debug.Log("Solution:");
        //    printPath(path);
        //}
    }

    void printPath(List<GolfBoardTile> path)
    {
        string pathList = "";
        foreach (GolfBoardTile tile in path)
        {
            pathList += "(" + tile.x + ", " + tile.y + ") ";
        }
        Debug.Log(pathList);
    }

    private void GeneratePaths(List<GolfBoardTile> path, int minMoves, int maxMoves)
    {
        //printPath(path);
        //valid if lenght < minMoves then add
        //invalid if lenght >= maxMoves then exit
        //valid if lenght >= minMoves and lenght <= maxMoves and neighbour is endTile return path to endTile
        //invalid if neighbour is already in path list 
        GolfBoardTile current = path[path.Count - 1];
        if (current == endTile)
        {
            if (path.Count -1 >= minMoves && path.Count -1 <= maxMoves) golfBoardTilePaths.Add( path);
        }
        else if(path.Count -1 < maxMoves)
        {
            foreach(GolfBoardTile frontier in current.moves)
            {
                if(!path.Contains(frontier))
                {
                    List<GolfBoardTile> newPath = new List<GolfBoardTile>(path);
                    newPath.Add(frontier);
                    GeneratePaths(newPath, minMoves, maxMoves);
                }
            }
        }

    }
    public void GenerateMoveGraph(Dictionary<string, List<List<string>>> answerset, /*int minMoves, int maxMoves,*/ int minJump, int maxJump)
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

            if (tile_Type == GolfASP.tile_types.start) startTile = moveGraph[x - 1, y - 1];
            if (tile_Type == GolfASP.tile_types.hole) endTile = moveGraph[x - 1, y - 1];
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

        //for (int i = 0; i < width; i += 1)
        //{
        //    for (int j = 0; j < height; j += 1)
        //    {
        //        moveGraph[i, j].printMoves();
        //    }
        //}

    }

    

    private bool validMove(Vector2Int end)
    {
        if(moveGraph[end.x,end.y].tileType == GolfASP.tile_types.air || moveGraph[end.x, end.y].tileType == GolfASP.tile_types.obstacle)
        {
            return false;
        }
        else { return true; }
    }

    public bool ValidMove(Vector2Int start, Vector2Int end)
    {
        if (validMove(end))
            return validMove(start, end);
        else
            return false;

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
        else if(start.y == end.y)
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
        else
        {
            return false;
        }


        return true;
    }


}

public class GolfBoardTile
{
    public int x, y;
    public List<GolfBoardTile> moves = new List<GolfBoardTile>();
    //public List<int> moveDistance = new List<int>();
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
