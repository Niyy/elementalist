using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectionMenu : MonoBehaviour
{
    private struct Character
    {
        public bool free;
        public PlayerInput playerInput;
        public Character(bool _free, PlayerInput _playerInput)
        {
            free = _free;
            playerInput = _playerInput;
        }
    }
    public GameObject playerManager;
    private Character[] characters = new Character[] { new Character(true, null), new Character(true, null), new Character(true, null), new Character(true, null) };
    public GameObject[] prefabs;

    
    public bool CharacterAvailable(int choice, PlayerInput playerInput)
    {
        if (characters[choice].free)
        {
            characters[choice] = new Character(false, playerInput);
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
                PlayerInput.Instantiate(prefabs[i], -1, "PlayerControls", -1, characters[i].playerInput.devices[0]);
                //Debug.Log(characters[i].playerInput.devices[0]);
                //Debug.Log(characters[i].playerInput.user);
                //player.GetComponent<PlayerInput>().user.Equals(characters[i].playerInput.user);
                //playerManager.GetComponent<PlayerManager>().GetPlayers(player);
            }
                
        }
    }
}
