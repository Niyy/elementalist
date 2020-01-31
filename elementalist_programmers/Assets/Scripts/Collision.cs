using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Collision : MonoBehaviour
{
    public bool on_ground, on_wall;
    public float ray_height;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray yRay = new Ray(transform.position, Vector3.down);
        on_ground = Physics.Raycast(yRay, out hit, ray_height);
    }
}
