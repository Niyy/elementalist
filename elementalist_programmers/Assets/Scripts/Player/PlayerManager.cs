using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    //array of players
    public List<GameObject> playerList;
    public GameObject[] selectors;
    public GameObject[] characters;
    public bool character_select = false;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

        //playerList.Add();
    }
    public void SetPlayers(GameObject player)
    {
        playerList.Add(player);
        if (character_select)
        {
            GameObject selector = Instantiate(selectors[playerList.Count - 1]);
            selector.transform.parent = playerList[playerList.Count - 1].transform;
        }
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
                player.transform.GetChild(1).gameObject.GetComponent<PlayerController>().Neutralize();
                player.GetComponent<DontDestroyOnLoad>().enabled = true;
                player.transform.GetChild(0).gameObject.SetActive(false);
                player.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

    }

}
