using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "MapKey", menuName = "Map/MapKeyBoard")]
    public class MapKeyBoard : MapKey
    {
        public MapObjectKey<GameObject> boardDict;
    }
}