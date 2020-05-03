using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public Transform Flame;
    public RoomManager room_manager;
    public InteractableChoice interactable_choice;
    public enum InteractableChoice
    {
        Collectable,
        Trap,
        Pressure_Plate
    };


    private GameObject level_manager;

    private void Start()
    {
        if (Flame != null)
        {
            Flame.GetComponent<ParticleSystem>().enableEmission = false;
        }
        
    }
    private void Awake()
    {
        level_manager = GameObject.Find("LevelManager");
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            if (Flame != null)
            {
                Flame.GetComponent<ParticleSystem>().enableEmission = true;
            }
            switch (interactable_choice)
            {
                case InteractableChoice.Collectable:
                    ActivateCollectable();
                    break;
                case InteractableChoice.Trap:
                    ActivateTrap();
                    break;
                case InteractableChoice.Pressure_Plate: 
                    ActivatePressurePlate();
                    break;
            }
            
        }
    }


    private void ActivateTrap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void ActivateCollectable()
    {
        level_manager.GetComponent<LevelManager>().AddToCollectedCoins();
        Destroy(this.gameObject);
    }

    private void ActivatePressurePlate()
    {
        room_manager.switchs.Remove(this.gameObject);
        GetComponent<CapsuleCollider>().enabled = false;

    }
}
