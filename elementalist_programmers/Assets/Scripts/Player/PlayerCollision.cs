using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCollision : MonoBehaviour
{
    public bool on_ground, on_wall;
    public float ray_height = 1.0f;
    public float ray_width = 0.500f;
    public float col_face = 1;
    bool left_col = false;
    bool right_col = false;
    bool mid_ground_col = false;
    bool left_ground_col = false;
    bool right_ground_col = false;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit  hit_ground,
                    hit_left,
                    hit_right;
        int layerMask = ~(1 << 10);
        Ray y_ray = new Ray(transform.position, Vector3.down);
        Ray left_y_ray = new Ray((transform.position - new Vector3(transform.lossyScale.x / 2, 0f)), Vector3.down);
        Ray right_y_ray = new Ray((transform.position + new Vector3(transform.lossyScale.x / 2, 0f)), Vector3.down);
        Ray left_ray = new Ray(transform.position, Vector3.left);
        Ray right_ray = new Ray(transform.position, Vector3.right);
        mid_ground_col = Physics.Raycast(y_ray, out hit_ground, ray_height, layerMask);
        left_ground_col = Physics.Raycast(left_y_ray, out hit_ground, ray_height, layerMask);
        right_ground_col = Physics.Raycast(right_y_ray, out hit_ground, ray_height, layerMask);
        left_col = Physics.Raycast(left_ray, out hit_left, ray_width, layerMask);
        right_col = Physics.Raycast(right_ray, out hit_right, ray_width, layerMask);

        if (mid_ground_col || left_ground_col || right_ground_col)
        {
            on_ground = true;
        }
        else
        {
            on_ground = false;
        }

        
        if (right_col)
        {
            col_face = 1;

            if(hit_right.collider.gameObject.layer == 9)
            {
                right_col = false;
            }
        }
        else if (left_col)
        {
            col_face = -1;

            if(hit_left.collider.gameObject.layer == 9)
            {
                left_col = false;
            }
        }
        else
        {
            col_face = 0;
        }

        on_wall = (right_col || left_col);
    }
}
