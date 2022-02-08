using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapBoard : Map2D
    {
        private GolfTile[,] map;
        public float tileSpacing = 1.1f;

        override public void DisplayMap(Dictionary<string, List<List<string>>> answerset, MapKey mapKey)
        {
            DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.pixelKey, mapKey.xIndex, mapKey.yIndex, mapKey.pixelTypeIndex, ((MapKeyBoard)mapKey).boardDict);
        }
        public void DisplayMap(Dictionary<string, List<List<string>>> answerset, string widthKey, string heightKey, string tileKey, int xIndex, int yIndex, int pixelTypeIndex, MapObjectKey<GameObject> tileDict)
        {
            foreach (List<string> widths in answerset[widthKey])
            {
                if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
            }
            foreach (List<string> h in answerset[heightKey])
            {
                if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
            }

            map = new GolfTile[width, height];

            foreach (List<string> pixelASP in answerset[tileKey])
            {
                int x = int.Parse(pixelASP[xIndex]) - 1;
                int y = int.Parse(pixelASP[yIndex]) - 1;

                string pixelType = pixelASP[pixelTypeIndex];

                GameObject pixel = Instantiate(tileDict[pixelType], transform);
                pixel.GetComponent<GolfTile>().x = x;
                pixel.GetComponent<GolfTile>().y = y;
                pixel.GetComponent<GolfTile>().type = (GolfASP.tile_types)System.Enum.Parse(typeof(GolfASP.tile_types), pixelType);
                pixel.transform.position = new Vector2(x * tileSpacing, y * tileSpacing);

                map[x, y] = pixel.GetComponent<GolfTile>();
            }

            GolfTileRules tileRules = FindObjectOfType<GolfASP>().golfTileRules;

            for(int h = 0; h < height; h += 1)
            {
                for (int w = 0; w < width; w += 1)
                {
                    if(map[w, h] && map[w,h].type == GolfASP.tile_types.air)
                    {
                        bool[] neighbors = new bool[8];
                        
                        neighbors[0] = w > 0 && h < height - 1 && map[w - 1, h + 1].type == GolfASP.tile_types.air ? false : true;
                        neighbors[1] = /*w > 0 && */ h < height - 1 && map[w, h + 1].type == GolfASP.tile_types.air ? false : true;
                        neighbors[2] = w < width - 1 && h < height - 1 && map[w + 1, h + 1].type == GolfASP.tile_types.air ? false : true;

                        neighbors[3] = w > 0 /*&& h < height - 1 */ && map[w - 1, h].type == GolfASP.tile_types.air ? false : true;
                        neighbors[4] = w < width - 1 /*&& h < height - 1 */ && map[w + 1, h].type == GolfASP.tile_types.air ? false : true;

                        neighbors[5] = w > 0 && h > 0 && map[w - 1, h - 1].type == GolfASP.tile_types.air ? false : true;
                        neighbors[6] = /*w > 0 &&*/ h >0 && map[w, h - 1].type == GolfASP.tile_types.air ? false : true;
                        neighbors[7] = w < width - 1 && h>0 && map[w + 1, h - 1].type == GolfASP.tile_types.air ? false : true;

                        map[w, h].sprite.sprite =  tileRules.GetSprite(neighbors);
                    }
                }
            }
            
        }

        override public void AdjustCamera()
        {
            Camera cam = Camera.main;
            float aspect = cam.aspect;
            float size = cam.orthographicSize;

            float boardSizeHeight = height * tileSpacing / 2 + (tileSpacing - 1) / 2;
            float boardSizeWidth = width * tileSpacing / 2 + (tileSpacing - 1) / 2;

            float boardAspect = boardSizeWidth / boardSizeHeight;

            float boardSizeX = boardSizeWidth / aspect;
            float boardSize = aspect < boardAspect ? boardSizeX : boardSizeHeight;

            cam.orthographicSize = boardSize;

            float y = height / 2 * (1 + (tileSpacing - 1));
            float x = width / 2 * (1 + (tileSpacing - 1));
            if (width % 2 == 0) x -= (1 + (tileSpacing - 1)) / 2;
            if (height % 2 == 0) y -= (1 + (tileSpacing - 1)) / 2;



            cam.transform.position = new Vector3(x, y, cam.transform.position.z);
        }

        
    }
}