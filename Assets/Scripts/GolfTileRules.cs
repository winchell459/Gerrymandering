using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "GolfTileRule", menuName = "ScriptableObjects/GolfTileRule")]
public class GolfTileRules : ASPTileRules
{
    
    

    public override string getTileRules()
    {
        //UpdateTileNeighborsState();
        string aspCode = $@"
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.grass}).
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.hole}).
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.obstacle}).
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.start}).

            :- Count = {{tile(XX,YY, air): tile(XX +1, YY, air), tile(XX, YY+1, air),tile(XX +1, YY+1, air),tile(XX +1, YY+2, air)}}, Count == 0.
        ";
        List<bool[]> missingRules = getMissingRules(Tiles);
        Debug.Log(missingRules.Count + " : missingRules");
        foreach (bool[] missingTile in missingRules)
        {
            string missing = getMissingStuffs(missingTile) + "\n";
            Debug.Log(missing);
            
            aspCode += $@":- tile(XX,YY,{GolfASP.tile_types.air}), 
                        {getNot(missingTile[0])} ground(XX-1,YY+1), 
                        {getNot(missingTile[1])} ground(XX,YY+1), 
                        {getNot(missingTile[2])} ground(XX+1,YY+1), 
                        {getNot(missingTile[3])} ground(XX-1,YY), 
                        {getNot(missingTile[4])} ground(XX+1,YY), 
                        {getNot(missingTile[5])} ground(XX-1,YY-1), 
                        {getNot(missingTile[6])} ground(XX,YY-1), 
                        {getNot(missingTile[7])} ground(XX+1,YY-1)
                        .
            ";
        }


        return aspCode;
    }

    

    string getNot(bool isEmpty)
    {
        if (!isEmpty) return "not";
        else return "";
    }

    string getMissingStuffs(bool[] missing)
    {
        string[] m = new string[8];
        for(int i = 0; i < 8; i += 1)
        {
            if (missing[i]) m[i] = "1";
            else m[i] = "0";
        }
        string blockPattern = $"{m[0]}{m[1]}{m[2]}\n{m[3]}0{m[4]}\n{m[5]}{m[6]}{m[7]}\n";
        return blockPattern;
    }

    public override Sprite GetSprite(bool[] neighbors)
    {
        Sprite sprite = null;
        foreach(TileRule tileRule in Tiles)
        {
            if (isMatching(tileRule, neighbors) && !sprite) sprite = tileRule.tileSprite;
            else if (isMatching(tileRule, neighbors)) Debug.LogWarning("Multiple sprites matching.");
        }
        return sprite;
    }

    bool isMatching(TileRule tile, bool[] neighbors)
    {
        bool match = true;
        for (int i = 0; i < 8; i += 1)
        {
            //if (tile.mustHave[i] && tile.emptyPlacement[i] != neighbors[i]) match = false;

            if (tile.neighbors[i] != TileNeighbors.State.none && neighbors[i] != (tile.neighbors[i] == TileNeighbors.State.filled)) match = false;
            
        }
        return match;
    }

}
