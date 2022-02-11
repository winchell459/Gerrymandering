using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASPTileRules : ScriptableObject
{
    [System.Serializable]
    public struct TileRule
    {
        public string name { get { return tileSprite.name; } }
        public TileNeighbors.State[] neighbors;
        //public bool[] emptyPlacement;
        //public bool[] mustHave;
        public Sprite tileSprite;
    }

    public TileRule[] Tiles;

    public abstract string getTileRules();
    public abstract Sprite GetSprite(bool[] neighbors);


    protected List<bool[]> getMissingRules(TileRule[] tileRules)
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
            foreach (TileRule tileRule in tileRules)
            {
                bool found = true;
                for (int j = 0; j < 8; j += 1)
                {
                    //if (tileRule.mustHave[j]/* && !tileRule.emptyPlacement[j]*/ && permutation[j] != tileRule.emptyPlacement[j]) found = false;

                    if (tileRule.neighbors[j] != TileNeighbors.State.none && permutation[j] != (tileRule.neighbors[j] == TileNeighbors.State.filled)) found = false;
                }
                if (found) missing = false;
            }
            if (missing) missingRules.Add(permutation);
        }

        return missingRules;
    }

    protected bool[] getPermutation(int num)
    {
        bool[] permutation = new bool[8];
        int index = 7;
        while (index >= 0)
        {
            int placeValue = num / (int)Mathf.Pow(2, index);
            if (placeValue == 1) permutation[index] = true;
            num = num % (int)Mathf.Pow(2, index);
            index -= 1;
        }
        return permutation;
    }
}

[System.Serializable]
public class TileNeighbors
{
    public enum State
    {
        none,
        filled,
        empty
    }
    public State[] neighbors = new State[8];
}