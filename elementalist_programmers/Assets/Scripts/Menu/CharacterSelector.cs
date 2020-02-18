using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelector : MonoBehaviour
{
    private Vector3[] selections = new Vector3[] { new Vector3(-4.5f,4f,1f), new Vector3(-1.5f, 4f, 1f), new Vector3(1.5f, 4f, 1f), new Vector3(4.5f, 4f, 1f) }; 
    Vector2 move;
    Vector3 position;
    int choice;
    bool selected = false;
    bool starting = true;
    bool selecting = true;
    GameObject CharSel;

    private void Awake()
    {
        choice = 0;
    }

    private void Start()
    {
        transform.position = selections[0];
        CharSel = GameObject.Find("CharacterSelect");
    }

    private void Update()
    {
        
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
        transform.position = selections[choice];

    }

    private void OnSelect()
    {
        if (selected || starting)
        {
            return;
        }
        selected = CharSel.GetComponent<CharacterSelectionMenu>().CharacterAvailable(choice, this.transform.parent.gameObject);
        transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
    }

    private void OnBack()
    {
        if (!selected || starting)
        {
            return;
        }
        else
        {
            selected = false;
            CharSel.GetComponent<CharacterSelectionMenu>().RemoveSelection(choice);
            transform.position = selections[choice];
        }
    }

    private void OnStartGame()
    {
        if (!selected || !selecting)
        {
            return;
        }
        else
        {
            selecting = false;
            CharSel.GetComponent<CharacterSelectionMenu>().BeginGame();
        }
    }
}
