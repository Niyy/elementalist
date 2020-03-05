using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossDoor : MonoBehaviour
{
    public bool open = false;
    bool opened = false;
    private void Update()
    {
        if(opened)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("i qiut");
                Application.Quit();
                
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(open)
            {
                print("you WIN ");
                LevelManager._instance.winState();
                opened = true;
              
                Time.timeScale = 0;
            }
        }
    }


}
