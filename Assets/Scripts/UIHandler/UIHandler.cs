using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIHandler : MonoBehaviour
{
    [SerializeField] protected List<int> _movesList;
    public List<int> MovesList { get { return _movesList; } set { setMovesList(value); } }

    [SerializeField] protected int _round;
    public int Round { get { return _round; } set { setRound(value); } }

    [SerializeField] protected float _timeRemainig;
    public float TimeRemainig { set { setTimeRemaining(value); } }

    virtual protected void setMovesList(List<int> movesList)
    {
        _movesList = movesList;
        FindObjectOfType<MovesUIHandler>().MovesList = movesList;
    }

    virtual protected void setRound(int round)
    {
        _round = round;
        FindObjectOfType<RoundUIHandler>().Round = round;
    }

    virtual protected void setTimeRemaining(float timeRemaining)
    {
        _timeRemainig = timeRemaining;
        FindObjectOfType<RoundUIHandler>().TimeRemaining = timeRemaining;
    }


}
