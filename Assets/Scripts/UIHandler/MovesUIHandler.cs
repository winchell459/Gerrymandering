﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesUIHandler : MonoBehaviour
{
    [SerializeField] protected List<int> _movesList;
    public List<int> MovesList { get { return _movesList; } set { setMovesList(value); } }

    virtual protected void setMovesList(List<int> movesList)
    {
        _movesList = movesList;
        displayMovesList();
    }

    virtual protected void displayMovesList()
    {

    }
}
