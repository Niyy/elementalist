using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //save instance of a player
    //array of players
    public List<GameObject> playerList;
    public GameObject[] selectors;
    public GameObject[] characters;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //playerList.Add();
    }
    public void GetPlayers(GameObject player)
    {
        playerList.Add(player);
    }

    public List<GameObject> GetPlayerList()
    {
        return playerList;
    }

}
