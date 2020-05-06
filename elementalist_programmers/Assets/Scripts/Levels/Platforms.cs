using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [Header("Chouse Platform")]
    public Platform current_Plat;


    [Header("Sand Trap - Can only have a single trigger")]
    public float sandSpeed;
    public float sandJump;
    public float sandFall;

    [Header("Crumbling - Needs 2 collider trigger first")]
    public float waitToCrumble = 0f;
    public float timeToRespawn = 0f;
    public bool crumbled = false;
    public bool animating = false;
    public Animator crumbleAnimation;

    [Header("Phasing - Can only have one physical collider")]
    public float timeActive;
    public float timeAway;

    public enum Platform
    {
        SandTrap,
        Crumbling,
        Phasing
    };

    private void Start()
    {
        if (current_Plat == Platform.Crumbling)
        {
            crumbleAnimation = GetComponent<Animator>();
        }
    }

    private void OnEnable()
    {
        if (current_Plat == Platform.Phasing)
            StartCoroutine(phasing());
    }

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
                    StartCoroutine(crumble());
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
    IEnumerator crumble()
    {
        if(crumbled == false)
        {
            crumbled = true;
            yield return new WaitForSeconds(.1f);
            crumbleAnimation.SetBool("crumbling", true);
            yield return new WaitForSeconds(waitToCrumble);
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;
            crumbleAnimation.SetBool("crumbling", false);
            yield return new WaitForSeconds(timeToRespawn);
            this.GetComponent<BoxCollider>().enabled = true;
            this.GetComponent<MeshRenderer>().enabled = true;
            crumbled = false;
        }
    }
    IEnumerator phasing()
    {
        yield return new WaitForSeconds(timeActive);
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(timeAway);
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(phasing());
    }

}
