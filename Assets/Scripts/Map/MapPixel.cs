using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapPixel : Map2D
    {
        public Pixel PixelPrefab;
        //private Pixel[,] map;

        //[SerializeField] Transform mapPrefabHolder;
        [SerializeField] int buildCount = 0;
        [SerializeField] int buildOffsetCount = 1;
        [SerializeField] int buildCountWidth = 1;
        //[SerializeField] int buildCountY = 1;
        private int BuildCount(int width)
        {
            int count = (buildCount % buildCountWidth) * (width + buildOffsetCount);
            

            return count;
        }
        private int BuildCountY(int height)
        {
            int count = (buildCount / buildCountWidth) * (height + buildOffsetCount);


            return -count;
        }

        public float PixelSpacing = 1.1f;
        protected override float getElementSpacing()
        {
            return PixelSpacing;
        }

        override public void DisplayMap(Dictionary<string, List<List<string>>> answerset, MapKey mapKey)
        {
            DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.pixelKey, mapKey.xIndex, mapKey.yIndex, mapKey.pixelTypeIndex, ((MapKeyPixel)mapKey).colorDict);
        }
        public void DisplayMap(Dictionary<string, List<List<string>>> answerset, string widthKey, string heightKey, string pixelKey, int xIndex, int yIndex, int pixelTypeIndex, MapObjectKey<Color> colorDict)
        {
            foreach (List<string> widths in answerset[widthKey])
            {
                if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
            }
            foreach (List<string> h in answerset[heightKey])
            {
                if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
            }

            //map = new Pixel[width, height];

            foreach (List<string> pixelASP in answerset[pixelKey])
            {
                int x = int.Parse(pixelASP[xIndex]) - 1 + BuildCount(width);
                int y = int.Parse(pixelASP[yIndex]) - 1 + BuildCountY(height);

                string pixelType = pixelASP[pixelTypeIndex];

                Pixel pixel = Instantiate(PixelPrefab, transform).GetComponent<Pixel>();
                pixel.SetPixel(x * PixelSpacing, y * PixelSpacing, colorDict[pixelType]);
                pixel.AddNote(pixelASP);
            }
            buildCount += 1;
        }

        
    }

}
