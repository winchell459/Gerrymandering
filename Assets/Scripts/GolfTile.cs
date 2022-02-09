using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfTile : MonoBehaviour
{
    public int x, y;
    public Vector2Int pos {get {return new Vector2Int(x,y);}}
    public GolfASP.tile_types type;
    public SpriteRenderer sprite;
    public Color defaultShade, hoverShade;

    private void Start()
    {
        sprite.color = defaultShade;
    }
    private void OnMouseDown()
    {
        FindObjectOfType<GameHandler>().GolfTileClicked(this);
    }

    private void OnMouseEnter()
    {
        if(FindObjectOfType<GameHandler>().ValidMove(pos))
        sprite.color = hoverShade;
    }

    private void OnMouseExit()
    {
        sprite.color = defaultShade;
    }
}
