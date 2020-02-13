using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenu : MonoBehaviour
{
    private bool[] character_free = new bool[] {true,true,true,true};
    public GameObject[] characters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool CharacterAvailable(int choice)
    {
        if (character_free[choice])
        {
            character_free[choice] = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    void BeginGame()
    {

    }
}
