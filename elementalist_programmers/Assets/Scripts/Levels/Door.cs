using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int roomIndex;

    public bool open = false;

    public GameObject spawnPoint, arrow;

    private void Start()
    {
        arrow = gameObject.transform.GetChild(1).gameObject;
        arrow.SetActive(false);
    }

    private void Update()
    {
        if (open)
        {
            openDoor();
            arrow.SetActive(true);
        }
       
    }

    void openDoor()
    {
        //print("Door is open");
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

            
            int currRoom = LevelManager._instance.getCurrentRoom();
            LevelManager._instance.roomsCenter[currRoom].SetActive(false);
            LevelManager._instance.roomsCenter[roomIndex].SetActive(true);
            LevelManager._instance.camTarget.transform.position = LevelManager._instance.roomsPosInLevel[roomIndex];

            foreach (GameObject player in FindObjectOfType<PlayerManager>().playerList)
            {
                player.transform.GetChild(0).transform.position = spawnPoint.transform.position;
                //print("I moved a player");
            }
           
            closeDoor();
        }
    }
}
