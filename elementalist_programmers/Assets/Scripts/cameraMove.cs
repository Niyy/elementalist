using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{

    public Camera main_Camera;

    public float distance_Moved = 0;

    public bool door_Closed = false;


    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("door_Right") && door_Closed == false)
        {
            print("go right");
            main_Camera.transform.Translate(distance_Moved, 0, 0);
            door_Closed = true;
            StartCoroutine(openDoor());
        }
        if (other.CompareTag("door_Left") && door_Closed == false)
        {
            print("go left");
            main_Camera.transform.Translate(-distance_Moved, 0, 0);
            door_Closed = true;
            StartCoroutine(openDoor());

        }
        if (other.CompareTag("door_Up") && door_Closed == false)
        {
            print("go up");
            main_Camera.transform.Translate(0, distance_Moved, 0);
            door_Closed = true;
            StartCoroutine(openDoor());

        }
        if (other.CompareTag("door_Down") && door_Closed == false)
        {
            print("go down");
            main_Camera.transform.Translate(0, -distance_Moved, 0);
            door_Closed = true;
            StartCoroutine(openDoor());

        }
    }

    public IEnumerator openDoor()
    {
        yield return new WaitForSeconds(1f);
        print("you can open a door again");
        door_Closed = false;
    }

    
}
