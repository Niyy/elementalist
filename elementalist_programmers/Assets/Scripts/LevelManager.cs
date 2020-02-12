using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int coins = 0;

    public int coinsNeededForBoss;

    public NextLevel bossDoor;

    public List<GameObject> roomsInLevel;

    private void Update()
    {
        //if (coins >= coinsNeededForBoss)
           // bossDoor.closedDoor = false;
    }

}
