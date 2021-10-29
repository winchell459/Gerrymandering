using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapKey", menuName = "Map/MapKey")]
public class MapKey : ScriptableObject
{
    public string widthKey = "width";
    public string heightKey = "height";
    public string pixelKey = "block";
    public int xIndex = 0;
    public int yIndex = 2;
    public int pixelTypeIndex = 3;

    public MapColorKey colorDict;
    //public MapObjectKey<Color> colorDict;

    public MapObjectKey<GameObject> objectDict;
    public MapObjectKey<Sprite> spriteDict;
    

}
[System.Serializable]
public class MapColorKey
{
    public MapColor[] mapColors;

    public Color this[string key]
    {
        get => FindColor(key);
    }
    Color FindColor(string key)
    {
        Color color = Color.white;
        foreach (MapColor mapColor in mapColors)
        {
            if (key == mapColor.key) color = mapColor.color;
        }
        return color;
    }
}
[System.Serializable]
public class MapColor
{
    public string key;
    public Color color = Color.white;
}

[System.Serializable]
public class MapObjectKey<T>
{
    public MapObject<T>[] mapObjects;
    public T this[string key]
    {
        get => FindObject(key);
    }
    T FindObject(string key)
    {
        T obj = default;
        foreach (MapObject<T> mapObject in mapObjects)
        {
            if (key == mapObject.key) obj = mapObject.obj;
        }
        return obj;
    }
}

[System.Serializable]
public class MapObject<T>
{
    public string key;
    public T obj;
}



