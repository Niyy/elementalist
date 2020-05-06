using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossDoor : MonoBehaviour
{
    public bool open = false;
    bool opened = false;
    levelsCompleated inst = new levelsCompleated();
    public int indexLevelCompleated;
    private void Awake()
    {
        if (SaveSystem.load() == null)
        {
            inst = new levelsCompleated();
        }
        else
        {
            inst = SaveSystem.load();
        }
    }

    private void Update()
    {
        if (opened)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("i quit");
                Application.Quit();

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (open)
        {
            print("you WIN ");
            LevelManager._instance.winState();
            opened = true;

            Time.timeScale = 0;
            WorldSelect();
        }
    }
    public void WorldSelect()
    {
        SceneManager.MoveGameObjectToScene(SceneManagement.Instance.gameObject, SceneManager.GetActiveScene());
        Time.timeScale = 1;
        inst.unlockedArray[indexLevelCompleated] = true;
        SaveSystem.saveLevels(inst);
        SceneManager.LoadScene("World Select");

    }
}