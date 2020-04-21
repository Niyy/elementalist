using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossOne : MonoBehaviour
{

    public GameObject grapple1, grapple2, weakPoint, currBranch;

    public Vector3 location, pos1, pos2, spawn1, spawn2, spawn3;

    public bool thinking, close, inJTB, moveArud;

    public float speed = 1f;
    public float distane;

    public int health = 10;

    public GameObject add; // this is a ctier enemy ist spawns

    int coolDown;

    [HideInInspector]
    public enum state
    {
        invPath,
        inv,
        vun
    }

    public state bossState = state.inv;

    private void Awake()
    {
        setObjects();
       // weakPoint.SetActive(false);
        GetComponent<SphereCollider>().enabled = true;
    }


    private void Update()
    {
        if (bossState != state.inv)
        { location = transform.position; }

        if (bossState == state.invPath)
        {
           // transform.position = location;
            transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1.0f));
            StartCoroutine(moveAround());
        }
        if (bossState == state.inv)
        {
            StartCoroutine(jumpToBranch());
            if (thinking == true && inJTB == true && close == false)
            {
                transform.position = Vector3.Lerp(transform.position, currBranch.transform.position, Time.deltaTime * 2f);
                distane = Vector3.Distance(transform.position, currBranch.transform.position);
                if (distane <= .8)
                {
                    close = true;
                }  
            }
            if (thinking == true && inJTB == false && close == true)
            {
                transform.position = Vector3.Lerp(transform.position, pos1, Time.deltaTime * 2f);
                //close = false;
            }

        }
        else if (bossState == state.vun)
        {
            StartCoroutine(hurting());
        }
    }

    IEnumerator moveAround()
    {
        if (moveArud == false)
        {
            moveArud = true;
            yield return new WaitForSeconds(2);
            bossState = state.inv;
            moveArud = false;
        }
    }

    IEnumerator jumpToBranch()
    {
        if (thinking == false)
        {
            thinking = true;
            currBranch = pickBranch();
            inJTB = true;
            yield return new WaitForSeconds(1);
            if (currBranch == grapple1)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (currBranch == grapple2)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            spawnAdds();
            yield return new WaitForSeconds(4);
            inJTB = false;
            yield return new WaitForSeconds(3);
            close = false;
            bossState = state.vun;
            thinking = false;
        }
    }

    IEnumerator hurting()
    {
       // weakPoint.SetActive(true);
        GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(4f);
       // weakPoint.SetActive(false);
        GetComponent<SphereCollider>().enabled = true;
        bossState = state.invPath;
    }

    void setObjects()
    {
        weakPoint = gameObject.transform.GetChild(0).gameObject;
        grapple1 = gameObject.transform.parent.GetChild(1).GetChild(0).gameObject;
        grapple2 = gameObject.transform.parent.GetChild(2).GetChild(0).gameObject;
        location = transform.position;

        spawn1 = gameObject.transform.parent.GetChild(3).transform.position;
        spawn2 = gameObject.transform.parent.GetChild(4).transform.position;
        spawn3 = gameObject.transform.parent.GetChild(5).transform.position;


        pos1 = new Vector3(location.x - 6, location.y, location.z);
        pos2 = new Vector3(location.x + 6, location.y, location.z);
    }

    void spawnAdds()
    {
        Instantiate(add, spawn1, Quaternion.identity);
        Instantiate(add, spawn2, Quaternion.identity);
        Instantiate(add, spawn3, Quaternion.identity);
    }

    GameObject pickBranch()
    {
        GameObject branch;

        float i = Random.value;

        if (i >= .5)
        {
            branch = grapple1;
        }
        else
        {
            branch = grapple2;
        }

        return branch;
    }



}
