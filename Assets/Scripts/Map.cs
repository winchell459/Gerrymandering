using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapPixel PixelPrefab;
    private MapPixel[,] map;
    private int width, height;
    public float PixelSpacing = 1.1f;

    public void DisplayMap(Dictionary<string, List<List<string>>> answerset)
    {
        foreach(List<string> widths in answerset["width"])
        {
            if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
        }
        foreach (List<string> h in answerset["height"])
        {
            if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
        }

        map = new MapPixel[width, height];

        foreach(List<string> island in answerset["island"])
        {
            int x = int.Parse(island[1]);
            int y = int.Parse(island[2]);
            Color type = Color.white;

            switch (island[0])
            {
                case "purple":
                    type = Color.magenta;
                    break;
                case "green":
                    type = Color.green;
                    break;
                case "blue":
                    type = Color.blue;
                    break;
                case "orange":
                    type = new Color(255 / 255f, 165 / 255f, 0);
                    break;
                case "red":
                    type = Color.red;
                    break;
                default:

                    break;
            }
          
            MapPixel pixel = Instantiate(PixelPrefab, transform).GetComponent<MapPixel>();
            pixel.SetPixel(x * PixelSpacing, y * PixelSpacing, type);
            pixel.AddNote(island);
        }
        
    }
}
