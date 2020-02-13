using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public string myLevel;

   public void goToThisLevel()
    {
        SceneManager.LoadScene(myLevel);
    }
}
