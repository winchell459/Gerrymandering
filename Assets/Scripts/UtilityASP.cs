using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityASP 
{
    public static int GetMaxInt(List<List<string>> set)
    {
        int max = int.MinValue;
        foreach(List<string> num in set)
        {
            if(max < int.Parse(num[0]))
            {
                max = int.Parse(num[0]);
            }
        }

        return max;
    }
}
