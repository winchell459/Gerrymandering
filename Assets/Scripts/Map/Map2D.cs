using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public abstract class Map2D : Map
    {
        protected abstract float getElementSpacing();
        private float elementSpacing { get { return getElementSpacing(); } }

        //override public abstract void DisplayMap(Dictionary<string, List<List<string>>> answerset, MapKey mapKey);
        override public void AdjustCamera()
        {
            Camera cam = Camera.main;
            float aspect = cam.aspect;
            float size = cam.orthographicSize;

            float boardSizeHeight = height * elementSpacing / 2 + (elementSpacing - 1) / 2;
            float boardSizeWidth = width * elementSpacing / 2 + (elementSpacing - 1) / 2;

            float boardAspect = boardSizeWidth / boardSizeHeight;

            float boardSizeX = boardSizeWidth / aspect;
            float boardSize = aspect < boardAspect ? boardSizeX : boardSizeHeight;

            cam.orthographicSize = boardSize;

            float y = height / 2 * (1 + (elementSpacing - 1));
            float x = width / 2 * (1 + (elementSpacing - 1));
            if (width % 2 == 0) x -= (1 + (elementSpacing - 1)) / 2;
            if (height % 2 == 0) y -= (1 + (elementSpacing - 1)) / 2;



            cam.transform.position = new Vector3(x, y, cam.transform.position.z);
        }
    }
}

