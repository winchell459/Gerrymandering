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
        string aspCode = $@"
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.grass}).
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.hole}).
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.obstacle}).
            ground(XX,YY) :- tile(XX,YY,{GolfASP.tile_types.start}).

        ";
        List<bool[]> missingRules = getMissingRules(Tiles);
        foreach (bool[] missingTile in missingRules)
        {
            
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

    List<bool[]> getMissingRules(TileRule[] tileRules)
    {
        List<bool[]> missingRules = new List<bool[]>();
        for (int i = 0; i < 256; i += 1)
        {
            bool[] permutation = getPermutation(i);
            //string debug = "";
            //for(int j = 0; j < 8; j += 1)
            //{
            //    debug += permutation[j] + ", ";
            //}
            //Debug.Log(debug);
            bool missing = true;
            foreach(TileRule tileRule in tileRules)
            {
                bool found = true;
                for(int j = 0; j < 8; j += 1)
                {
                    if (permutation[j] != tileRule.emptyPlacement[j]) found = false;
                }
                if (found) missing = false;
            }
            if (missing) missingRules.Add(permutation);
        }

        return missingRules;
    }

    bool[] getPermutation(int num)
    {
        bool[] permutation = new bool[8];
        int index = 7;
        while(index >= 0)
        {
            int placeValue = num / (int)Mathf.Pow(2, index);
            if (placeValue == 1) permutation[index] = true;
            num = num % (int)Mathf.Pow(2, index);
            index -= 1;
        }
        return permutation;
    }

    string getNot(bool isEmpty)
    {
        if (isEmpty) return "not";
        else return "";
    }
}
