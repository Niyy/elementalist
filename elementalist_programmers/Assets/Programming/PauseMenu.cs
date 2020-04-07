using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    PlayerControls controls;
    public GameObject PauseUI;
    public GameObject PlayerCanvas;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Pause.performed += ctx => Pause();
    }

    public void Pause()
    {
        PauseUI.SetActive(!PauseUI.activeSelf);

        if (PauseUI.activeSelf)
        {
            PlayerCanvas.SetActive(false);
            Time.timeScale = 0; 
        }
        else
        {
            PlayerCanvas.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public void MainMenu()
    {
        SceneManager.MoveGameObjectToScene(SceneManagement.Instance.gameObject, SceneManager.GetActiveScene());
        SceneManager.LoadScene("MainMenu");
 
    }



    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }



}
