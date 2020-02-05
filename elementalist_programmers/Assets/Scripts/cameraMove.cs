using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{

    public GameObject main_Camera;

    public float distance_Moved = 50;

    public bool door_Closed = true;

    public int leverSwitched;
    public int waterCans;

    

    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("door") && door_Closed == false)
        {
            print("go right");
          //  FindObjectOfType<PlayerMovement>().canMove = false;
            FindObjectOfType<FadeInOut>().loadLevel();
            StartCoroutine(moveCamra());
            
        }
        // if (other.CompareTag("door_Left") && door_Closed == false)
        // {
        //     print("go left");
        //     main_Camera.transform.Translate(-distance_Moved, 0, 0);
        //     door_Closed = true;
        //     StartCoroutine(openDoor());
        //
        // }
        // if (other.CompareTag("door_Up") && door_Closed == false)
        // {
        //     print("go up");
        //     main_Camera.transform.Translate(0, distance_Moved, 0);
        //     door_Closed = true;
        //     StartCoroutine(openDoor());
        //
        // }
        // if (other.CompareTag("door_Down") && door_Closed == false)
        // {
        //     print("go down");
        //     main_Camera.transform.Translate(0, -distance_Moved, 0);
        //     door_Closed = true;
        //     StartCoroutine(openDoor());
        //
        // }
    }

    public IEnumerator openDoor()
    {
        yield return new WaitForSeconds(1f);
        print("you can open a door again");
        door_Closed = false;
    }
    public IEnumerator moveCamra()
    {


        StartCoroutine(FindObjectOfType<FadeInOut>().transistion());
        yield return new WaitForSeconds(1.5f);
     //   FindObjectOfType<PlayerMovement>().anima.enabled = false;
        main_Camera.transform.Translate(distance_Moved, 0, 0);
        this.transform.Translate(4, 0, 0);
     //   FindObjectOfType<PlayerMovement>().canMove = true;
     //   FindObjectOfType<PlayerMovement>().anima.enabled = true;

        door_Closed = true;

    }

}
