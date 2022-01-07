using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabMovesUIHandler : MovesUIHandler
{
    [SerializeField] private TabMove[] tabs;
    

    protected override void displayMovesList()
    {
        int[] tabDisplay = new int[tabs.Length];
        for(int i = 1; i < _movesList.Count; i += 1)
        {
            tabDisplay[i - 1] = _movesList[i];
        }

        for(int i = 0; i < tabs.Length; i += 1)
        {
            tabs[i].SetMoveCount(tabDisplay[i]);
        }
    }
}
