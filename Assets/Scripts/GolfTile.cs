using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfTile : MonoBehaviour
{
    public int x, y;
    private void OnMouseDown()
    {
        FindObjectOfType<ASPLevelHandler>().GolfTileClicked(this);
    }
}
