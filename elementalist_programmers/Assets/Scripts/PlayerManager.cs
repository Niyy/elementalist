using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //save instance of a player
    //array of players
    public List<GameObject> playerList;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //playerList.Add();
    }
    public void GetPlayers(GameObject player)
    {
        playerList.Add(player);
    }

    List<GameObject> GetPlayerList()
    {
        return playerList;
    }

}
