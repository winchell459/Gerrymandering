using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public abstract class Map3D : Map
    {
        override public abstract void DisplayMap(Dictionary<string, List<List<string>>> answerset, MapKey mapKey);
    }
}

