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
    float collider_height;

    BoxCollider player_collider;
    // Start is called before the first frame update
    void Start()
    {
        player_collider = GetComponent<BoxCollider>();
        collider_height = player_collider.size.y * transform.localScale.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int layer_mask = 1 << 8;
        layer_mask = ~layer_mask;
        RaycastHit  hit_ground,
                    hit_left,
                    hit_right;
        Ray y_ray = new Ray(transform.position, Vector3.down);
        Ray left_y_ray = new Ray((transform.position - new Vector3(player_collider.size.x / 2, 0f)), Vector3.down);
        Ray right_y_ray = new Ray((transform.position + new Vector3(player_collider.size.x / 2, 0f)), Vector3.down);
        Ray left_ray = new Ray(transform.position, Vector3.left);
        Ray right_ray = new Ray(transform.position, Vector3.right);
        mid_ground_col = Physics.Raycast(y_ray, out hit_ground, ray_height, layer_mask);
        left_ground_col = Physics.Raycast(left_y_ray, out hit_ground, ray_height, layer_mask);
        right_ground_col = Physics.Raycast(right_y_ray, out hit_ground, ray_height, layer_mask);
        left_col = Physics.Raycast(left_ray, out hit_left, ray_width);
        right_col = Physics.Raycast(right_ray, out hit_right, ray_width);

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
        Debug.DrawRay(transform.position, Vector3.up * (collider_height / 2), Color.red);
        Debug.DrawRay((transform.position - new Vector3(player_collider.size.x / 2, collider_height / 2)), Vector3.up * collider_height, Color.red);
        Debug.DrawRay((transform.position - new Vector3(-player_collider.size.x / 2, collider_height / 2)), Vector3.up * collider_height, Color.red);
    }

    public bool HeadCollision()
    {
        
        RaycastHit hit_ceiling;
        Ray y_ray = new Ray(transform.position, Vector3.up);
        Ray left_y_ray = new Ray((transform.position - new Vector3(player_collider.size.x / 2, collider_height/2)), Vector3.up);
        Ray right_y_ray = new Ray((transform.position - new Vector3((-player_collider.size.x) / 2, collider_height/2 - 0.1f)), Vector3.up);
        bool mid_head_col = Physics.Raycast(y_ray, out hit_ceiling, collider_height / 2 -0.1f);
        bool left_head_col = Physics.Raycast(left_y_ray, out hit_ceiling, collider_height-0.1f);
        bool right_head_col = Physics.Raycast(right_y_ray, out hit_ceiling, collider_height -0.2f);
        if (mid_head_col) Debug.Log("Middle");
        if (right_head_col) Debug.Log("Right");
        if (left_head_col) Debug.Log("Left");
        if (mid_head_col || left_head_col || right_head_col)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
