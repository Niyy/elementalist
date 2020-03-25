using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void SurveyPlease()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSchYp9iN26vjy5UBgMMVG_96ef1LzLXi7jVDHo9gFtVzx05hw/viewform");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");

        Application.Quit();

    }
}
