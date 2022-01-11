﻿using System.Collections;
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

    static ASPMemory<MoveEvents> memory;
    [SerializeField] private ASPMemory<MoveEvents> _memory;
    static private int round;
    // Start is called before the first frame update
    void Start()
    {
        round += 1;
        if (memory == null)
        {
            //???should move to parent class or to within ASPMemory???
            memory = new ASPMemory<MoveEvents>();
            
            memory.Events = new MoveEvents();
        }
        _memory = memory;

        golfASP = GetComponent<GolfASP>();
        moveFinder = GetComponent<GolfMoveFinder>();
        addNewMemory();
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
            FindObjectOfType<UIHandler>().Round = round;
            waitingForASP = false;
        }
    }

    public void ReloadSceneButton()
    {
        reloadScene();
    }
    private void reloadScene()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void addNewMemory()
    {

        memory.Events.MovesPath.Add(new MoveEvent());
    }

    void addToMemory(Vector2Int nextPos)
    {
        memory.Events.MovesPath[memory.Events.MovesPath.Count - 1].Moves.Add(nextPos);
    }

    void setPlayerDestination(Vector2 destination)
    {
        Player.GetComponent<Ball>().SetDestination(destination);
        
    }

    void setPlayerDestination(Vector2Int destination)
    {
        currentPos = destination;
        addToMemory(currentPos);
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
        golfASP.StartJob(memory);
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
        if (distance < movesList.Count && movesList[distance] > 0)
        {
            return true;
        }
        else return false;
    }
}

[System.Serializable]
public class ASPMemory<T>
{
    public T Events;
    public float Weight;

    
}

[System.Serializable]
public class MoveEvents
{
    public List<MoveEvent> MovesPath;
    public MoveEvents()
    {
        MovesPath = new List<MoveEvent>();
    }

    public string GetMoves()
    {
        string aspMoves = "";
        foreach(MoveEvent moveEvent in MovesPath)
        {
            for(int i = 1; i < moveEvent.Moves.Count; i += 1)
            {
                Vector2Int start = moveEvent.Moves[i-1];
                Vector2Int end = moveEvent.Moves[i];
                aspMoves += $":- move({start.x},{start.y},{end.x},{end.y}). \n";
            }
        }
        return aspMoves;
    }
}
[System.Serializable]
public class MoveEvent
{
    public List<Vector2Int> Moves;
    public MoveEvent()
    {
        Moves = new List<Vector2Int>();
    }
}
