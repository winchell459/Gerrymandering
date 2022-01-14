using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    bool gamePlayMode;
    bool gameOver;
    [SerializeField] private Ball player;
    GolfMoveFinder moveFinder;
    ASPLevelHandler board;

    [SerializeField] private Vector2Int currentPos;
    [SerializeField] private List<int> movesList;

    static private int round = 0;
    private int turnCount = 0;

    static float remainingTime = 60;
    [SerializeField] float remainingTimeStart = 60;

    [SerializeField] GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        if (remainingTime <= 0) remainingTime = remainingTimeStart;
        round += 1;
        FindObjectOfType<UIHandler>().TimeRemainig = remainingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver && gamePlayMode && player.Stopped && moveFinder.GetEndLoc() == currentPos)
        {
            reloadScene();
            //gameOver = true;
        }else if(!gameOver && !gamePlayMode && FindObjectOfType<ASPLevelHandler>().Ready)
        {
            moveFinder = FindObjectOfType<GolfMoveFinder>();
            board = FindObjectOfType<ASPLevelHandler>();
            movesList = moveFinder.MovesList;
            setPlayerDestination(moveFinder.GetStartLoc(), true);
            
            gamePlayMode = true;
            updateUIHandler();
        }else if(!gameOver && gamePlayMode)
        {
            remainingTime -= Time.deltaTime;
            FindObjectOfType<UIHandler>().TimeRemainig = remainingTime;
            if (remainingTime <= 0)
            {
                gameOver = true;
                gameOverPanel.SetActive(true);
            }
        }
    }

  
    

    public void GolfTileClicked(GolfTile tile)
    {
        if (gameOver) return;
        if (player.Stopped && moveFinder.ValidMove(currentPos, tile.pos))
        {
            int distance = 0;
            if (currentPos.x == tile.pos.x)
            {
                distance = Mathf.Abs(currentPos.y - tile.pos.y);
            }
            else if (currentPos.y == tile.pos.y)
            {
                distance = Mathf.Abs(currentPos.x - tile.pos.x);
            }
            else
            {
                Debug.LogWarning("Diagonal moves not implemented");
            }

            if (checkMoves(distance))
            {
                if (player.Stopped)
                {
                    movesList[distance] -= 1;
                    player.MovingForward = true;
                    setPlayerDestination(new Vector2Int(tile.x, tile.y));

                    turnCount += 1;
                    updateUIHandler();
                }



                string moves = "";
                for (int i = 0; i < movesList.Count; i += 1)
                {
                    if (movesList[i] > 0) moves += movesList[i] + " - " + i + " jumps ";
                }
                Debug.Log(moves);
            }

        }
        else if (player.MovingForward && !player.Stopped && tile.x == currentPos.x && tile.y == currentPos.y)
        {
            Debug.Log($"Double Clicked: {tile}");
            FindObjectOfType<Ball>().MovingForward = true;
            setPlayerPosition(new Vector2Int(tile.x, tile.y));
        }
    }

    private bool checkMoves(int distance)
    {
        if (distance < movesList.Count && movesList[distance] > 0)
        {
            return true;
        }
        else return false;
    }

    void setPlayerDestination(Vector2 destination)
    {
        player.SetDestination(destination);

    }

    void setPlayerDestination(Vector2Int destination)
    {
        currentPos = destination;
        board.AddToMemory(currentPos);
        setPlayerDestination(board.GetTilePos(destination));
        Debug.Log(currentPos);
    }

    void setPlayerDestination(Vector2Int destination, bool setPosition)
    {
        if (setPosition)
        {
            setPlayerPosition(destination);
        }
        setPlayerDestination(destination);
    }

    void setPlayerPosition(Vector2Int pos)
    {
        Vector2 position = board.GetTilePos(pos);
        player.transform.position = position;
        //setPlayerDestination(position);
    }

    private void updateUIHandler()
    {
        FindObjectOfType<UIHandler>().MovesList = movesList;
        FindObjectOfType<UIHandler>().Round = round;
    }

    public void UndoMoveButton()
    {
        if (gameOver) return;
        undoMove();
    }

    public void QuitGameButton()
    {
        round = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void reloadScene()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void undoMove()
    {
        if (turnCount > 0 && player.Stopped)
        {
            Vector2Int lastMove = board.GetLastMove();
            int distance = (int)lastMove.magnitude;
            movesList[distance] += 1;

            board.UndoMemory();

            player.MovingForward = false;
            setPlayerDestination(currentPos - lastMove);

            turnCount -= 1;
            updateUIHandler();
        }
        else if (!player.MovingForward && !player.Stopped)
        {
            setPlayerPosition(currentPos);
        }
    }
}
