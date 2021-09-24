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
            int x = int.Parse(island[1]) - 1;
            int y = int.Parse(island[2]) - 1;
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
        AdjustCamera();
    }

    void AdjustCamera()
    {
        Camera cam = Camera.main;
        float aspect = cam.aspect;
        float size = cam.orthographicSize;

        float boardSizeHeight = height * PixelSpacing / 2 + (PixelSpacing - 1) / 2;
        float boardSizeWidth = width * PixelSpacing / 2 + (PixelSpacing - 1) / 2;

        float boardAspect = boardSizeWidth / boardSizeHeight;

        float boardSizeX = boardSizeWidth / aspect;
        float boardSize = aspect < boardAspect ? boardSizeX : boardSizeHeight;

        cam.orthographicSize = boardSize;

        float y = height / 2 - (PixelSpacing - 1) / 2;
        float x = width / 2 - (PixelSpacing - 1) / 2;

        cam.transform.position = new Vector3(x, y, cam.transform.position.z);
    }
}
