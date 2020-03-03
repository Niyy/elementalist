using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public string nextLevel;

    public void startButton()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void quitButton()
    {
        Application.Quit();
    }






}
