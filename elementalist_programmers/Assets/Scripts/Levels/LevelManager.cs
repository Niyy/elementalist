using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static public LevelManager _instance;
    static public LevelManager Instance { get { return _instance; } }

    public int coins = 0;
    

    public GameObject[] roomsCenter;

    
   [HideInInspector] public List<Vector3> roomsPosInLevel;

    public GameObject camTarget;

    private void Awake()
    {//singleton
        if(_instance !=null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        //takes center of each level and adds the transform to a list
        foreach(GameObject room in roomsCenter)
        {
            roomsPosInLevel.Add(room.transform.position);
        }
        //finds what the cam is looksing at
        camTarget = GameObject.Find("CamTarget");

        camTarget.transform.position = roomsPosInLevel[0];
    }






    public void AddToCollectedCoins(int amount_add = 1)
    {
        coins += amount_add;
    }
}
