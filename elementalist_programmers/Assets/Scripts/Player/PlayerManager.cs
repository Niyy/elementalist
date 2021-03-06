﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum Playmode { singleplayer, multiplayer }

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    //list of players
    [HideInInspector]
    public List<GameObject> playerList;
    public GameObject[] selectors;
    [HideInInspector]
    public GameObject[] characters;
    [HideInInspector]
    public bool character_select = true;
    [HideInInspector]
    public Playmode mode;
    public GameObject instructions = null;
    public AudioClip death;
    public AudioClip game_over;
    AudioSource audioSource;

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

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
        audioSource = GetComponent<AudioSource>();
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
        //List<GameObject> unselected = new List<GameObject>();
        for (int i = playerList.Count - 1; i >= 0; i--)
        {
            if (playerList[i].transform.childCount < 2)
            {
                Destroy(playerList[i]);
                playerList.RemoveAt(i);
                //unselected.Add(player);
            }
            else
            {
                playerList[i].transform.GetChild(1).gameObject.GetComponent<PlayerController>().Neutralize();
                //playerList[i].GetComponent<DontDestroyOnLoad>().enabled = true;
                playerList[i].transform.parent = SceneManagement.Instance.transform;
                //player.transform.GetChild(1).gameObject.SetActive(false);
                Destroy(playerList[i].transform.GetChild(0).gameObject);
            }
        }
        if(instructions != null)
        {
            instructions.SetActive(false);
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
            DeathSound(game_over);
            foreach (GameObject player in playerList)
            {
                player.transform.GetChild(0).GetComponent<PlayerController>().PlayerReset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            DeathSound(death);
        }
    }

    public void SceneExit()
    {
        foreach(GameObject player in playerList)
        {
            player.GetComponent<DontDestroyOnLoad>().enabled = false;
        }
    }

    public void DeathSound(AudioClip sound = null)
    {
        if(sound == null)
        {
            sound = game_over;
        }
        audioSource.clip = sound;
        audioSource.Play();
    }
}
