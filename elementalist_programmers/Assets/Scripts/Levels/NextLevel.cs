using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Alex
public class NextLevel : MonoBehaviour
{
    //says if parent door is open
    public bool closedDoor = true;
    //Spawnpoint in the next room
    public GameObject spawnNextRoom;
    Vector3 nextRoomSpawnLocation;
    //the Parent door
    public GameObject currentDoor;
    //camrea in the next room
    public GameObject nextRoomCam;
    //Current Rooms cam
    public GameObject oldCam;

    AudioListener oldCams;
    AudioListener newCams;
    //the closed and open material for the parent door
    public Material closed;
    public Material open;

    bool opning;


    private void Start()
    {
    
        nextRoomSpawnLocation = spawnNextRoom.transform.position;
        LoadCams();

    }
    private void Update()
    {
        CheckDoorMaterial();
    }

    //changes the doors apearence if it is open or closed
    void CheckDoorMaterial()
    {
        if (closedDoor == false)
        {
            currentDoor.GetComponent<Renderer>().material = open;
        }

        if (closedDoor == true)
            currentDoor.GetComponent<Renderer>().material = closed;
    }



    /// <summary>
    /// This will change to the next room
    /// first it will disable the players movement (need player movement done first)
    /// next it will fade out (Fade will be added when I get to that I have it done in another project I can pull it)
    /// next it switches the cams (done)
    /// next it disables player animation (if animater is active you cant transform.Translate)
    /// - (need player to have animations)(might not have to do this we will see)
    /// next it all moves all players
    /// next it will reenable player animation(need player animations)
    /// next players will be able to move(Player movement needs to be compleated)
    /// Final fade back in and Doors will be closed again(fade from another project will be in soon and close door done)
    /// </summary>
    public IEnumerator MoveToNextRoom()
    {
        if (opning == false)
        {
            opning = true;
            StartCoroutine(FindObjectOfType<FadeInOut>().transistion());
            yield return new WaitForSeconds(1);
            nextRoomCam.SetActive(true);
            newCams.enabled = true;
            print("Next Rooms cam is up");
            oldCams.enabled = false;
            oldCam.SetActive(false);

            foreach (GameObject player in FindObjectOfType<PlayerManager>().playerList)
            {
                player.transform.position = nextRoomSpawnLocation;
                print("I moved a player");
            }
            closedDoor = true;
            opning = false;
        }
    }


    void LoadCams()
    {
        oldCams = oldCam.GetComponent<AudioListener>();
        oldCams.enabled = false;
        if (oldCam.tag != "MainCamera")
        {
            oldCam.SetActive(false);
        }
        newCams = nextRoomCam.GetComponent<AudioListener>();
        newCams.enabled = false;
        nextRoomCam.SetActive(false);
        if (nextRoomCam.tag == "MainCamera")
            nextRoomCam.SetActive(true);
        if (oldCam.tag == "MainCamera")
            oldCams.enabled = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag.Equals("Player") 
        //    && closedDoor == false)
        //{
        //    StartCoroutine(MoveToNextRoom());
        //}


    }
}
