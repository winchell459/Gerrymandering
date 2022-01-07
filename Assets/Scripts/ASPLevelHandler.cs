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

            setPlayerPosition(moveFinder.GetStartLoc());

            waitingForASP = false;
        }
    }

    void setPlayerDestination(Vector2 destination)
    {
        Player.GetComponent<Ball>().SetDestination(destination);
    }

    void setPlayerDestination(Vector2Int destination)
    {
        setPlayerDestination(new Vector2(destination.x * tileSpacing, destination.y * tileSpacing));
    }

    void setPlayerPosition(Vector2Int pos)
    {
        Player.transform.position = new Vector2(pos.x * tileSpacing, pos.y * tileSpacing);
        setPlayerDestination(Player.transform.position);
    }

    private void startGolfASP()
    {
        waitingForASP = true;
        golfASP.StartJob();
    }

    public void GolfTileClicked(GolfTile tile)
    {
        setPlayerDestination(new Vector2Int(tile.x, tile.y));
    }
}
