using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelector : MonoBehaviour
{
    PlayerInput playerInput;
    private Vector3[] selections = new Vector3[] { new Vector3(-4.5f,4f,0), new Vector3(-1.5f, 4f, 0), new Vector3(1.5f, 4f, 0), new Vector3(4.5f, 4f, 0) }; 
    Vector2 move;
    Vector3 position;
    int choice;
    bool selected = false;
    bool starting = true;
    GameObject CharSel;

    private void Awake()
    {
        choice = 0;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        transform.position = selections[0];
        CharSel = GameObject.Find("CharacterSelect");
    }

    private void Update()
    {
        transform.position = selections[choice];
        starting = false;
    }

    private void OnMove(InputValue value)
    {
        if (selected || starting)
        {
            return;
        }
        move = value.Get<Vector2>();
        if (move.x > 0f)
        {
            choice++;
            if(choice > selections.Length - 1)
            {
                choice = 0;
            }
        }
        else if (move.x < 0f)
        {
            choice--;
            if(choice < 0)
            {
                choice = (selections.Length - 1);
            }
        }
        
    }

    private void OnSelect()
    {
        if (selected)
        {
            return;
        }
        print("onselect");
        selected = CharSel.GetComponent<CharacterSelectionMenu>().CharacterAvailable(choice,playerInput);
    }

    private void OnBack()
    {
        print("onback");
    }


}
