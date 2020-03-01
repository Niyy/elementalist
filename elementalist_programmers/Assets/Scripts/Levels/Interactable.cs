using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public RoomManager room_manager;
    public InteractableChoice interactable_choice;
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
    }


    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag.Equals("Player"))
        {
            switch(interactable_choice)
            {
                case InteractableChoice.Collectable: ActivateCollectable();
                    break;
                case InteractableChoice.Trap: ActivateTrap();
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
        if (room_manager.switchs.Contains(this.gameObject))
        {
            room_manager.switchs.Remove(this.gameObject);
        }    
        Destroy(this.gameObject);
    }
}
