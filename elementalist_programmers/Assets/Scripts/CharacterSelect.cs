using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    private Vector3[] selections = new Vector3[] { new Vector3(-4.5f,2f,0), new Vector3(-1.5f, 2f, 0), new Vector3(1.5f, 2f, 0), new Vector3(4.5f, 2f, 0) }; 
    Vector2 move;
    Vector3 position;
    int choice;

    private void OnMove(InputValue value)
    {
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
        Debug.Log(choice);
        transform.position = selections[choice];
    }

    private void Start()
    {
        transform.position = selections[0];
    }

    private void Awake()
    {
        Debug.Log("onenable length: " + selections.Length);
        
        choice = 0;
    }
}
