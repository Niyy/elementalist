using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //save instance of a player
    //array of players
    GameObject[] players;
    public List<GameObject> playerList;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //playerList.Add();
    }
    void GetPlayers()
    {

    }

    List<GameObject> GetPlayerList()
    {
        return playerList;
    }

}
