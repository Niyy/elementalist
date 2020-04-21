 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class ShowColliders : MonoBehaviour 
 {
     void OnDrawGizmos() 
     {
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
     }
 }
