using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossCollision : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.GetComponentInParent<bossOne>().health--;
        }
    }
}
