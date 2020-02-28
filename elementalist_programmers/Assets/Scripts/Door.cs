using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int roomIndex;

    public bool open = false;

    public GameObject spawnPoint;

    private void Update()
    {
        if (open)
        openDoor();
        open = false;
    }

    void openDoor()
    {
        print("Door is open");
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    public void closeDoor()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            LevelManager._instance.camTarget.transform.position = LevelManager._instance.roomsPosInLevel[roomIndex];

            foreach (GameObject player in FindObjectOfType<PlayerManager>().playerList)
            {
                player.transform.position = spawnPoint.transform.position;
                print("I moved a player");
            }
           
            closeDoor();
        }
    }
}
