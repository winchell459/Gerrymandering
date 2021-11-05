using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "MapKey", menuName = "Map/MapKeyPixel")]
    public class MapKeyPixel : MapKey
    {
        public MapObjectKey<Color> colorDict;
    }
}

