using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;

public class GolfMap : Map2D
{
    [SerializeField] private float tileSpacing = 1;
    protected override float getElementSpacing()
    {
        return tileSpacing;
    }
    public override void AdjustCamera()
    {
        throw new System.NotImplementedException();
    }

    public override void DisplayMap(Dictionary<string, List<List<string>>> answerset, MapKey mapKey)
    {
        throw new System.NotImplementedException();
    }
}
