using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial Part 1");

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("World Select");

    }

    

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    

    public void MainMenuScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {

        Application.Quit();

    }

    public void ButtonAudio()
    {
        AudioManager.Instance.playAudio();
    }

    public void SetMusicVolume(float slider)
    {
        AudioManager.Instance.SetMusicVolume(slider);

    }

    public void SetSfxVolume(float slider)
    {
        AudioManager.Instance.SetSfxVolume(slider);

    }
}
