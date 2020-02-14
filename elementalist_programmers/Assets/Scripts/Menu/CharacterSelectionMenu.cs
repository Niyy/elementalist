using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectionMenu : MonoBehaviour
{
    private struct Character
    {
        public bool free;
        PlayerInput playerInput;
        public Character(bool _free, PlayerInput _playerInput)
        {
            free = _free;
            playerInput = _playerInput;
        }
    }
    private Character[] characters = new Character[] { new Character(true, null), new Character(true, null), new Character(true, null), new Character(true, null) };
    public GameObject[] prefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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

    void BeginGame()
    {
        for(int i = 0; i < characters.Length; i++)
        {

        }
    }
}
