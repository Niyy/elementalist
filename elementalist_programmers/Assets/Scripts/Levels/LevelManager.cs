using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static public LevelManager _instance;
    static public LevelManager Instance { get { return _instance; } }

    
    public int coins = 0;
    public int coinsAmount = 4;
    public BossDoor bossDoor;

    public Text keyFrags;
    //public Text win;
    public Text bossDoorT;

    public GameObject[] roomsCenter;


    [HideInInspector] public List<Vector3> roomsPosInLevel;

    public GameObject camTarget;

    private void Awake()
    {//singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        //takes center of each level and adds the transform to a list
        foreach (GameObject room in roomsCenter)
        {
            roomsPosInLevel.Add(room.transform.position);
        }
        //finds what the cam is looksing at
        camTarget = GameObject.Find("CamTarget");

        camTarget.transform.position = roomsPosInLevel[0];
    }
    private void Start()
    {
        foreach (var item in roomsCenter)
        {
            item.SetActive(false);
        }
        roomsCenter[0].SetActive(true);

        //win.text = " ";
    }

    private void Update()
    {
        openBossDoor();
        setText();
    }

    public int getCurrentRoom()
    {
        int index = 0;
        for (int i = 0; i < roomsCenter.Length; i++)
        {
            if (roomsCenter[i].activeSelf == true)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public void AddToCollectedCoins(int amount_add = 1)
    {
        coins += amount_add;
    }

    void openBossDoor()
    {
        if (coins >= coinsAmount)
        {
            bossDoor.open = true;
        }
    }

    void setText()
    {
        keyFrags.text = ": " + coins + "/4";
        if (coins < 4)
            bossDoorT.text = "";
        else
            bossDoorT.text = "Next World Unlocked!";
    }

    public void winState()
    {
       // win.text = "YOU WIN!\nPress escape to quit";

    }
}
