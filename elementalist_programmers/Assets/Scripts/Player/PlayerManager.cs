using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Playmode { singleplayer, multiplayer }

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    //list of players
    public List<GameObject> playerList;
    public GameObject[] selectors;
    public GameObject[] characters;
    public bool character_select = false;

    public Playmode mode;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //playerList.Add();
    }

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

    public void SetPlayers(GameObject player)
    {
        playerList.Add(player);
        if (character_select)
        {
            GameObject selector = Instantiate(selectors[playerList.Count - 1]);
            selector.transform.parent = playerList[playerList.Count - 1].transform;
        }
        else
        {
            playerList[playerList.Count - 1].GetComponent<DontDestroyOnLoad>().enabled = true;
        }
        ModeCheck();
    }

    public void ModeCheck()
    {
        if (playerList.Count > 1)
        {
            mode = Playmode.multiplayer;
        }
        else
            mode = Playmode.singleplayer;
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
                player.transform.GetChild(1).gameObject.SetActive(false);
                Destroy(player.transform.GetChild(0).gameObject);
            }
        }
        ModeCheck();
    }

    public void LivingPlayersCheck()
    {
        bool players_remain = false;
        foreach(GameObject player in playerList)
        {
            if (!player.transform.GetChild(0).GetComponent<PlayerController>().death_status)
            {
                players_remain = true;
            }
        }
        if (!players_remain)
        {
            foreach (GameObject player in playerList)
            {
                player.transform.GetChild(0).GetComponent<PlayerController>().PlayerReset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
