using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{

    EnemyC parent;
    bool found = false;

    private void Start()
    {
        parent = GetComponentInParent<EnemyC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (found == false)
            {
                GetComponent<SphereCollider>().enabled = false;
                parent.target = other.gameObject;
                Destroy(this.gameObject);
                found = true;
            }
        }
    }

}
