﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.SetPlayers(this.gameObject);
    }
}
