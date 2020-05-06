using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class levelsCompleated
{
    public bool[] unlockedArray;

    public levelsCompleated()
    {
        unlockedArray = new bool[4];
        unlockedArray[0] = true;
        unlockedArray[1] = false;
        unlockedArray[2] = false;
        unlockedArray[3] = false;
    }
}
