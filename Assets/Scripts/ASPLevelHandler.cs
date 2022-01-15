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

    // Start is called before the first frame update
    void Start()
    {
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
        golfASP.StartJob();
    }

    
}
