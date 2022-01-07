using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabMove : MonoBehaviour
{
    [SerializeField] int count;
    int maxCount = 9;
    int maxLength = 400;
    public void SetMoveCount(int count)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(count * maxLength / maxCount, rect.sizeDelta.y);
        this.count = count;
    }

    
}
