using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public float sandSpeed;
    public float sandJump;
    public float sandFall;

    public Platform current_Plat;

    public enum Platform
    {
        SandTrap,
        Crumbling,
        Phasing
    };

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (current_Plat)
            {
                case Platform.SandTrap:
                    other.GetComponent<Rigidbody>().velocity = new Vector3(0, -.5f, 0);
                    SandTrapE(other.GetComponent<PlayerController>());
                    print("traped");
                    break;
                case Platform.Crumbling:
                    break;
                default:
                    break;

            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (current_Plat)
            {
                case Platform.SandTrap:
                    other.GetComponent<PlayerController>().grounded = true;
                   // other.GetComponent<Rigidbody>().velocity = new Vector3(0, -.05f, 0); super kill 
                    //other.GetComponent<Rigidbody>().AddForce(-Physics.gravity);
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (current_Plat)
            {
                case Platform.SandTrap:
                    SandTrapL(other.GetComponent<PlayerController>());
                    print("free");
                    break;
                default:
                    break;

            }
        }
    }



    void SandTrapE(PlayerController me)
    {
       
        me.trapped = true;
        me.moveSpeed = sandSpeed;
        me.jumpForce = sandJump;
        me.fallMultiplier = sandFall;
        

    }
    void SandTrapL(PlayerController me)
    {
        me.trapped = false;
        me.moveSpeed = me.dSpeed;
        me.jumpForce = me.djump;
        me.fallMultiplier = me.dFall;
    }

}
