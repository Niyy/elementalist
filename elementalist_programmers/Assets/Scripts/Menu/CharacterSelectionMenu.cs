using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectionMenu : MonoBehaviour
{
    //this originally contained more variables, it can be cleaned up later to just be a variable instead of struct
    private struct Character
    {
        public bool free;
        public GameObject parent;
        public Character(bool _free, GameObject _parent)
        {
            free = _free;
            parent = _parent;
        }
    }
    public GameObject playerManager;
    private Character[] characters = new Character[] { new Character(true, null), new Character(true, null), new Character(true, null), new Character(true, null) };
    public GameObject[] prefabs;

    private void Start()
    {
        GameObject playerManager = GameObject.Find("PlayerManager");
    }

    public bool CharacterAvailable(int choice, GameObject parent)
    {
        if (characters[choice].free)
        {
            characters[choice] = new Character(false, parent);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveSelection(int choice)
    {
        characters[choice] = new Character(true, null);
    }

    public void BeginGame()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            if (!characters[i].free)
            {
                prefabs[i].GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY;
                prefabs[i].transform.parent = characters[i].parent.transform;
                characters[i].parent.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
                //PlayerInput.Instantiate(prefabs[i], -1, "PlayerControls", -1, characters[i].playerInput.devices[0]);
                Debug.Log("freeing character");
                //Debug.Log(characters[i].playerInput.user);
                //player.GetComponent<PlayerInput>().user.Equals(characters[i].playerInput.user);
                //playerManager.GetComponent<PlayerManager>().GetPlayers(player);
                
                playerManager.GetComponent<PlayerManager>().GetPlayers(characters[i].parent);
            }
                
        }
    }
}
