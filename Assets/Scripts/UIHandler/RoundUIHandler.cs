using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundUIHandler : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text roundText;
    public int Round { set { roundText.text = value.ToString(); } }
    
}
