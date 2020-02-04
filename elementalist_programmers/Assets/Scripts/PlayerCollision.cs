using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCollision : MonoBehaviour
{
    public bool on_ground, on_wall;
    public float ray_height = 1.0f;
    public float ray_width = 0.501f;
    public float col_face = 1;
    bool left_col = false;
    bool right_col = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Ray y_Ray = new Ray(transform.position, Vector3.down);
        Ray left_ray = new Ray(transform.position, Vector3.left);
        Ray right_ray = new Ray(transform.position, Vector3.right);
        on_ground = Physics.Raycast(y_Ray, out hit, ray_height);
        left_col = Physics.Raycast(left_ray, out hit, ray_width);
        right_col = Physics.Raycast(right_ray, out hit, ray_width);
        if (right_col)
        {
            col_face = 1;
        }
        else if (left_col)
        {
            col_face = -1;
        }
        else
        {
            col_face = 0;
        }
        on_wall = (right_col || left_col );
    }
}
