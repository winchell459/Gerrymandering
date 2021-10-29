using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapPixel PixelPrefab;
    private MapPixel[,] map;
    private int width, height;
    public float PixelSpacing = 1.1f;

    public void DisplayMap(Dictionary<string, List<List<string>>> answerset, MapKey mapKey)
    {
        DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.pixelKey, mapKey.xIndex, mapKey.yIndex, mapKey.pixelTypeIndex, mapKey.colorDict);
    }
    public void DisplayMap(Dictionary<string, List<List<string>>> answerset, string widthKey, string heightKey, string pixelKey, int xIndex, int yIndex, int pixelTypeIndex, MapColorKey colorDict)
    {
        foreach (List<string> widths in answerset[widthKey])
        {
            if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
        }
        foreach (List<string> h in answerset[heightKey])
        {
            if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
        }

        map = new MapPixel[width, height];

        foreach (List<string> pixelASP in answerset[pixelKey])
        {
            int x = int.Parse(pixelASP[xIndex]) - 1;
            int y = int.Parse(pixelASP[yIndex]) - 1;

            string pixelType = pixelASP[pixelTypeIndex];

            MapPixel pixel = Instantiate(PixelPrefab, transform).GetComponent<MapPixel>();
            pixel.SetPixel(x * PixelSpacing, y * PixelSpacing, colorDict[pixelType]);
            pixel.AddNote(pixelASP);
        }
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

        float y = height / 2 * (1 + (PixelSpacing - 1));
        float x = width / 2 * (1 + (PixelSpacing - 1));
        if (width % 2 == 0) x -= (1 + (PixelSpacing - 1) )/ 2;
        if (height % 2 == 0) y -= (1 + (PixelSpacing - 1)) / 2;



        cam.transform.position = new Vector3(x, y, cam.transform.position.z);
    }
}



