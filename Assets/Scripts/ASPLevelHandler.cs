using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASPLevelHandler : MonoBehaviour
{
    [SerializeField] private Map.MapKey mapKey;
    private GolfASP golfASP;
    private bool waitingForASP;
    public bool Ready { get { return !waitingForASP; } }

    private float tileSpacing { get { return FindObjectOfType<Map.MapBoard>().TileSpacing; } }

    static ASPMemory<MoveEvents> memory;
    [SerializeField] private ASPMemory<MoveEvents> _memory;
    public static bool newMemory = true;

    // Start is called before the first frame update
    void Start()
    {
        handleMemorySetup();
        golfASP = GetComponent<GolfASP>();
        startGolfASP();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingForASP && golfASP.SolverDone)
        {
            FindObjectOfType<Map.Map>().DisplayMap(golfASP.answerSet, mapKey);
            FindObjectOfType<Map.Map>().AdjustCamera();

            waitingForASP = false;
        }
    }

    public Vector2 GetTilePos(Vector2Int tileIndex)
    {
        return new Vector2(tileIndex.x * tileSpacing, tileIndex.y * tileSpacing);
    }

    private void startGolfASP()
    {
        waitingForASP = true;
        golfASP.StartJob(memory);
    }

    private void handleMemorySetup()
    {
        if (newMemory)
        {
            newMemory = false;
            memory = new ASPMemory<MoveEvents>();
            memory.Events = new MoveEvents();
        }
        memory.Events.MovesPath.Add(new MoveEvent());
        _memory = memory;
    }
    
    public void AddToMemory(Vector2Int nextPos)
    {
        memory.Events.AddMove(nextPos);
    }
}


[System.Serializable]
public class MoveEvents
{
    public List<MoveEvent> MovesPath;
    public MoveEvents()
    {
        MovesPath = new List<MoveEvent>();
    }

    public void AddMove(Vector2Int move)
    {
        int currentMoveEvent = MovesPath.Count - 1;
        MovesPath[currentMoveEvent].Moves.Add(move);
    }

    public void RemoveMove()
    {
        int currentMoveEvent = MovesPath.Count - 1;

        if (MovesPath[currentMoveEvent].Moves.Count > 0) {
            int currentMove = MovesPath[currentMoveEvent].Moves.Count - 1;
            MovesPath[currentMoveEvent].Moves.RemoveAt(currentMove);
        }
    }

    public Vector2Int GetLastMove()
    {
        int currentMoveEvent = MovesPath.Count - 1;

        if (MovesPath[currentMoveEvent].Moves.Count > 0)
        {
            int currentMove = MovesPath[currentMoveEvent].Moves.Count - 1;
            return MovesPath[currentMoveEvent].Moves[currentMove];
        }

        else
            return default;
    }

    public string GetMoves()
    {
        string aspMoves = "";
        foreach(MoveEvent moveEvent in MovesPath)
        {
            for(int i = 1; i < moveEvent.Moves.Count; i += 1)
            {
                Vector2Int start = moveEvent.Moves[i - 1];
                Vector2Int end = moveEvent.Moves[i];
                aspMoves += $" :- move({start.x + 1},{start.y + 1},{end.x + 1},{end.y + 1}).  \n";
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