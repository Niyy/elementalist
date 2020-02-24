using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private GameObject playerManager;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerManager = GameObject.Find("PlayerManager");
        playerManager.GetComponent<PlayerManager>().RemovePlayers();
        SceneManager.LoadScene(1);
    }
}
