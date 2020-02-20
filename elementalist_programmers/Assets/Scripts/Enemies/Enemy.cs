using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Variables")]
    public float speed = 0f;
    // health but its one hit kill so just a true or false
    public bool alive = true;
    //allows the Level desginer to set if they want the enemy to target one indvidual player or not
    public bool findTarget = false;
    //Who am I trying to kill
    public GameObject target;
    //Enemys speed
    public RoomManager myRoom;








    // Start is called before the first frame update
    void Start()
    {
        WhoAmIKilling();
    }

    // Update is called once per frame
    void Update()
    {
        death();



    }

    public void WhoAmIKilling()
    {
        if (findTarget == true)
        {
            int amountOfPlayers = FindObjectOfType<PlayerManager>().playerList.Count;
            int myTarget = Random.Range(0, amountOfPlayers);
            print(myTarget);
            target = FindObjectOfType<PlayerManager>().playerList[myTarget];
            print(target);
        }
    }

    public void death()
    {
        if (alive == false)
        {
            myRoom.enemys.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
