using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public abstract class Map2D : Map
    {
        [SerializeField] protected float tileSpacing;

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

