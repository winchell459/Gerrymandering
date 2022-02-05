using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GolfTileRule", menuName = "ScriptableObjects/GolfTileRule")]
public class GolfTileRules : ScriptableObject
{
    [System.Serializable]
    public struct TileRule
    {
        public string name { get { return tileSprite.name; } }
        public bool[] emptyPlacement;
        public Sprite tileSprite;
    }

    public TileRule[] Tiles;
    
    public string getTileRules()
    {
        string aspCode = "";
        foreach (TileRule tileRule in Tiles)
        {

        }

        return aspCode;
    }
}
