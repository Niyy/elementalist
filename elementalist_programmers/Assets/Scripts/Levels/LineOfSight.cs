using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{

    EnemyC parent;
    public bool found = false;

    private void Start()
    {
        parent = GetComponentInParent<EnemyC>();
    }

    public void Retarget()
    {
        GetComponent<SphereCollider>().enabled = true;
        found = false;
        print("retarget");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !other.GetComponent<PlayerController>().death_status)
        {
            if (found == false)
            {
                GetComponent<SphereCollider>().enabled = false;
                parent.target = other.gameObject;
                //Destroy(this.gameObject);
                found = true;
            }
        }
    }
}
