using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float max_radius = 2.0f;
    private float radius;
    private float current_angle;
    private GameObject player_position;


    protected void Awake()
    {
        current_angle = 0;
    }


    protected void Start()
    {
        radius = Random.Range(0, max_radius);
    }

    
    protected void FixedUpdate()
    {
        IdleMovement();
    }


    protected void IdleMovement()
    {
        this.transform.position = new Vector3(radius * Mathf.Cos(current_angle * Mathf.Deg2Rad),
                                            radius * Mathf.Sin(current_angle * Mathf.Deg2Rad),
                                            0.0f) + player_position.transform.position;
        current_angle += 1.0f;

        if(current_angle > 360)
        {
            current_angle = 0;
        }
    }


    protected void Release()
    {

    }


    protected void Destroy()
    {

    }


    public void SetPlayerPosition(GameObject new_pos)
    {
        this.player_position = new_pos;
    }
}
