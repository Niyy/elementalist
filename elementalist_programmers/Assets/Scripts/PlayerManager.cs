using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
<<<<<<< HEAD
=======

    //save instance of a player
>>>>>>> 73b9b5459527f9c3a8295233cab76c861c32e184
    //array of players
    public List<GameObject> playerList;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //playerList.Add();
    }
    public void GetPlayers(GameObject player)
    {
        playerList.Add(player);
    }
<<<<<<< HEAD
=======

    List<GameObject> GetPlayerList()
    {
        return playerList;
    }

>>>>>>> 73b9b5459527f9c3a8295233cab76c861c32e184
}
