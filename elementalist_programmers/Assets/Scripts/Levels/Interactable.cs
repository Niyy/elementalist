using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public RoomManager room_manager;
    public InteractableChoice interactable_choice;
    private AudioSource audioSource;
    public AudioClip sound;
    private Renderer obj_renderer;
    public enum InteractableChoice
    {
        Collectable,
        Trap,
        Pressure_Plate
    };


    private GameObject level_manager;


    private void Awake()
    {
        level_manager = GameObject.Find("LevelManager");
        if (interactable_choice != InteractableChoice.Trap)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            obj_renderer = GetComponent<Renderer>();
        }
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            switch (interactable_choice)
            {
                case InteractableChoice.Collectable:
                    ActivateCollectable();
                    break;
                case InteractableChoice.Trap:
                    ActivateTrap(col);
                    break;
                case InteractableChoice.Pressure_Plate: 
                    ActivatePressurePlate();
                    break;
            }
        }
    }


    private void ActivateTrap(Collider col)
    {
        col.GetComponent<PlayerController>().PlayerDeath();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void ActivateCollectable()
    {
        level_manager.GetComponent<LevelManager>().AddToCollectedCoins();
        if (room_manager.switchs.Contains(this.gameObject))
        {
            room_manager.switchs.Remove(this.gameObject);
        }
        audioSource.Play();
        obj_renderer.enabled = false;
        this.enabled = false;
    }

    private void ActivatePressurePlate()
    {
        room_manager.switchs.Remove(this.gameObject);
        PlayAudio();
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }
}
