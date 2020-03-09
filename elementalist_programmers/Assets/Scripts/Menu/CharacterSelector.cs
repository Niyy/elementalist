using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelector : MonoBehaviour
{
    //private Vector3[] selections = new Vector3[] { new Vector3(-4.5f,4f,1f), new Vector3(-1.5f, 4f, 1f), new Vector3(1.5f, 4f, 1f), new Vector3(4.5f, 4f, 1f) };
    private List<Vector3> selections;
    Vector2 move;
    Vector3 position;
    int choice;
    bool selected = false;
    bool starting = true;
    bool selecting = true;
    GameObject CharSel;
    CharacterSelectionMenu CharSelMenu;
    float last_move_time = 0f;
    public float add_height = 0f;
    public float selection_drop = 1f;


    private void Awake()
    {
        choice = 0;
    }


    private void Start()
    {
        selections = new List<Vector3> { };
        CharSel = GameObject.Find("CharacterSelect");
        CharSelMenu = CharSel.GetComponent<CharacterSelectionMenu>();
        foreach (GameObject character in CharSelMenu.prefabs)
        {
            selections.Add(character.transform.position + (Vector3.up * character.GetComponent<Renderer>().bounds.size.y) + (Vector3.up * add_height));
        }
        transform.position = selections[0];
    }

    private void Update()
    {
        starting = false;
    }

    private void OnMove(InputValue value)
    {
        float time = Time.unscaledTime;
        if (selected || starting || time < last_move_time + 0.2)
        {
            return;
        }
        last_move_time = time;
        move = value.Get<Vector2>();
        if (move.x > 0f)
        {
            choice++;
            if(choice > selections.Count - 1)
            {
                choice = 0;
            }
        }
        else if (move.x < 0f)
        {
            choice--;
            if(choice < 0)
            {
                choice = (selections.Count - 1);
            }
        }
        transform.position = selections[choice];

    }

    private void OnSelect()
    {
        if (starting)
        {
            return;
        }
        if (selected)
        {
            OnBack();
            return;
        }
        selected = CharSelMenu.CharacterAvailable(choice, this.transform.parent.gameObject);
        if (selected)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - selection_drop, transform.position.z);
        }
        
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
            CharSelMenu.RemoveSelection(choice);
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
            CharSel.GetComponent<DetectUsers>().DisableListening();
            CharSelMenu.BeginGame();
        }
    }
}
