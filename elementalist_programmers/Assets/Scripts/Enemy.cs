using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // health but its one hit kill so just a true or false
    bool alive = true;
    //allows the Level desginer to set if they want the enemy to target one indvidual player or not
    public bool findTarget = false;
    //Who am I trying to kill
    GameObject target;
    //Enemys speed
    public float speed = 0f;







    // Start is called before the first frame update
    void Start()
    {
        whoAmIKilling();
    }

    // Update is called once per frame
    void Update()
    {
        if(alive == false)
        {
            Destroy(this.gameObject);
        }



    }

    void whoAmIKilling()
    {
        if(findTarget == true)
        {
            int amountOfPlayers = FindObjectOfType<PlayerManager>().playerList.Count;
            int myTarget = Random.Range(0, amountOfPlayers);
            print(myTarget);
            target = FindObjectOfType<PlayerManager>().playerList[myTarget];
            print(target);
        }
    }


}
