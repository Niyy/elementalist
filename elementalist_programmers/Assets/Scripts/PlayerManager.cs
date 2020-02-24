using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //save instance of a player
    //array of players
    public struct PlayerStruct
    {
        public GameObject player;
        public int character;

    }
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
        GameObject selector = Instantiate(selectors[playerList.Count - 1]);
        selector.transform.parent = playerList[playerList.Count  - 1].transform;
    }

    public List<GameObject> GetPlayerList()
    {
        return playerList;
    }

    public void RemovePlayers()
    {
        foreach ( GameObject player in playerList )
        {
            if (player.transform.childCount < 2)
            {
                playerList.Remove(player);
            }
            else
            {
                player.GetComponent<DontDestroyOnLoad>().enabled = true;
                player.transform.GetChild(0).gameObject.SetActive(false);
                player.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

    }

}
