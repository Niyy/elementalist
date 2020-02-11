using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag.Equals("Player"))
        {
            col.GetComponent<PlayerController>().SetRespawnPoint(this.gameObject);
        }
    }
}
