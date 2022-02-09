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

    public static int Round = 0;

    [SerializeField] UnityEngine.UI.Text roundText;

    // Start is called before the first frame update
    void Start()
    {
        Round += 1;
        roundText.text = Round.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver && !gamePlayMode && board.Ready)
        {
            startRound();
        }else if (!gameOver && gamePlayMode && moveFinder.GetEndLoc() == currentPos && player.Stopped)
        {
            int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
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
        board.AddToMemory(destination);
        setPlayerDestination(board.GetTilePos(destination));
    }

    void setPlayerPosition(Vector2Int pos)
    {
        player.transform.position = board.GetTilePos(pos);
        
    }

    public void GolfTileClicked(GolfTile tile)
    {
        if (gameOver) return;
        if (player.Stopped && ValidMove(tile.pos))
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

    public bool ValidMove(Vector2Int targetTilePos)
    {
        if (gameOver) return false;
        if (player.Stopped && moveFinder.ValidMove(currentPos, targetTilePos))
        {
            int distance = 0;
            if (currentPos.x == targetTilePos.x)
            {
                distance = Mathf.Abs(currentPos.y - targetTilePos.y);
            }
            else if (currentPos.y == targetTilePos.y)
            {
                distance = Mathf.Abs(currentPos.x - targetTilePos.x);
            }
            else
            {
                Debug.LogWarning("Diagonal moves not implemented");
            }

            if (checkMoves(distance))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    private bool checkMoves(int distance)
    {
        if (distance < movesList.Count && movesList[distance] > 0) return true;
        else return false;
    }
}
