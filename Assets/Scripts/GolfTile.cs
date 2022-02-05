using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfTile : MonoBehaviour
{
    public int x, y;
    public Vector2Int pos {get {return new Vector2Int(x,y);}}
    public GolfASP.tile_types type;
    public SpriteRenderer sprite;
    private void OnMouseDown()
    {
        FindObjectOfType<GameHandler>().GolfTileClicked(this);
    }
}
