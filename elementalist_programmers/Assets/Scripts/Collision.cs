using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Collision : MonoBehaviour
{
    public bool on_ground, on_wall;
    public float ray_height = 1.0f;
    public float ray_width = 0.501f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray y_Ray = new Ray(transform.position, Vector3.down);
        Ray left_ray = new Ray(transform.position, Vector3.left);
        Ray right_ray = new Ray(transform.position, Vector3.right);
        on_ground = Physics.Raycast(y_Ray, out hit, ray_height);
        on_wall = (Physics.Raycast(left_ray, out hit, ray_width) || Physics.Raycast(right_ray, out hit, ray_width));
    }
}
