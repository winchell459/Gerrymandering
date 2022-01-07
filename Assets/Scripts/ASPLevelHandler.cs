using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASPLevelHandler : MonoBehaviour
{
    [SerializeField] private Map.MapKey mapKey;
    private GolfASP golfASP;
    private GolfMoveFinder moveFinder;
    private bool waitingForASP;
    public GameObject Player;

    private float tileSpacing { get { return FindObjectOfType<Map.MapBoard>().tileSpacing; } }

    [SerializeField] private Vector2Int currentPos;
    [SerializeField] private List<int> movesList;

    // Start is called before the first frame update
    void Start()
    {
        golfASP = GetComponent<GolfASP>();
        moveFinder = GetComponent<GolfMoveFinder>();
        startGolfASP();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingForASP && golfASP.SolverDone)
        {
            FindObjectOfType<Map.Map>().DisplayMap(golfASP.answerSet, mapKey);
            FindObjectOfType<Map.Map>().AdjustCamera();
            movesList = moveFinder.MovesList;
            setPlayerDestination(moveFinder.GetStartLoc(), true);
            FindObjectOfType<UIHandler>().MovesList = movesList;
            waitingForASP = false;
        }
    }

    void setPlayerDestination(Vector2 destination)
    {
        Player.GetComponent<Ball>().SetDestination(destination);
        
    }

    void setPlayerDestination(Vector2Int destination)
    {
        currentPos = destination;
        setPlayerDestination(new Vector2(destination.x * tileSpacing, destination.y * tileSpacing));
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
        Vector2 position = new Vector2(pos.x * tileSpacing, pos.y * tileSpacing);
        Player.transform.position = position;
        //setPlayerDestination(position);
    }

    private void startGolfASP()
    {
        waitingForASP = true;
        golfASP.StartJob();
    }

    public void GolfTileClicked(GolfTile tile)
    {
        if (moveFinder.ValidMove(currentPos, tile.pos))
        {
            int distance = 0;
            if(currentPos.x == tile.pos.x)
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
                movesList[distance] -= 1;
                setPlayerDestination(new Vector2Int(tile.x, tile.y));

                FindObjectOfType<UIHandler>().MovesList = movesList;

                string moves = "";
                for (int i = 0; i < movesList.Count; i += 1)
                {
                    if (movesList[i] > 0) moves += movesList[i] + " - " + i + " jumps ";
                }
                Debug.Log(moves);
            }
            
        }
        
    }

    private bool checkMoves(int distance)
    {
        if (distance <= movesList.Count && movesList[distance] > 0)
        {
            return true;
        }
        else return false;
    }
}
