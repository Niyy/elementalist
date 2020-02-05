﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    //array of players
    GameObject[] players;
    //list of players from the array
    public List<GameObject> playerList;


    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            playerList.Add(player);
        }
    }


}