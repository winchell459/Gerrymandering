using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    bool gamePlayMode;
    bool gameOver;
    [SerializeField] private Ball player;
    GolfMoveFinder moveFinder;
    [SerializeField] ASPLevelHandler board;

    [SerializeField] private Vector2Int currentPos;
    [SerializeField] private List<int> movesList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver && !gamePlayMode && board.Ready)
        {
            startRound();
        }
    }

    void startRound()
    {
        moveFinder = FindObjectOfType<GolfMoveFinder>();
        movesList = moveFinder.MovesList;
        setPlayerDestination(moveFinder.GetStartLoc(), true);
        gamePlayMode = true;
    }

    void setPlayerDestination(Vector2 destination)
    {
        player.SetDestination(destination);
    }
    /// <summary>
    /// set player destination and position
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="setPosition"></param>
    void setPlayerDestination(Vector2Int destination, bool setPosition)
    {
        if (setPosition)
        {
            setPlayerPosition(destination);
        }
        setPlayerDestination(destination);
    }

    void setPlayerDestination(Vector2Int destination)
    {
        currentPos = destination;
        setPlayerDestination(board.GetTilePos(destination));
    }

    void setPlayerPosition(Vector2Int pos)
    {
        player.transform.position = board.GetTilePos(pos);
        
    }

    public void GolfTileClicked(GolfTile tile)
    {
        if (gameOver) return;
        if (player.Stopped && moveFinder.ValidMove(currentPos, tile.pos))
        {
            int distance = 0;
            if(currentPos.x == tile.pos.x)
            {
                distance = Mathf.Abs(currentPos.y - tile.pos.y);
            }else if(currentPos.y == tile.pos.y)
            {
                distance = Mathf.Abs(currentPos.x - tile.pos.x);
            }
            else
            {
                Debug.LogWarning("Diagonal moves not implemented");
            }

            if (checkMoves(distance))
            {
                movesList[distance] -= 1;
                setPlayerDestination(new Vector2Int(tile.x, tile.y));
            }

        }

        
    }

    private bool checkMoves(int distance)
    {
        if (distance < movesList.Count && movesList[distance] > 0) return true;
        else return false;
    }
}
