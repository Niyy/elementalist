using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    PlayerControls controls;

    public GameObject PauseUI;
    private GameObject PlayerCanvas;
    public GameObject ControllerScreen;
    public GameObject DeathScreen;
    private EventSystem eventSystem;
    public GameObject BackButton;
   

    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        controls = new PlayerControls();
        controls.Gameplay.Pause.performed += ctx => Pause();
        PlayerCanvas = GameObject.Find("Canvas");
        eventSystem.firstSelectedGameObject = GameObject.Find("Resume Button");
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
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
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
 
    }

    public void WorldSelect()
    {
        SceneManager.MoveGameObjectToScene(SceneManagement.Instance.gameObject, SceneManager.GetActiveScene());
        Time.timeScale = 1;
        SceneManager.LoadScene("World Select");

    }

    public void DisplayControlScreen()
    {
        PauseUI.gameObject.SetActive(false);
        ControllerScreen.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(BackButton);
    }

    public void DisplayMainMenu()
    {
        PauseUI.gameObject.SetActive(true);
        ControllerScreen.gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
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
