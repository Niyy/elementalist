using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float radius = 2.0f;


    private float current_angle;
    private float max_amount;
    private bool not_velocity_loss;
    private Vector3 projectile_velocity;
    private GameObject player_position;
    private new Rigidbody rigidbody;


    protected void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }


    protected void Start()
    {
        Debug.Log("current_angle " + current_angle);

        if(current_angle > 0)
        {
            this.transform.position = new Vector3(radius * Mathf.Cos(current_angle * Mathf.Deg2Rad),
                                                radius * Mathf.Sin(current_angle * Mathf.Deg2Rad),
                                                0.0f) + player_position.transform.position;
            current_angle += 1.0f;
            this.GetComponent<Collider>().enabled = false;
        }
    }


    protected void Update()
    {
        IdleMovement();
        ApplyVelocity();
    }

    
    protected void FixedUpdate()
    {
    }


    protected void IdleMovement()
    {
        Debug.Log(radius);
        if(this.transform.parent != null)
        {
            this.transform.position = new Vector3(radius * Mathf.Cos(current_angle * Mathf.Deg2Rad),
                                                radius * Mathf.Sin(current_angle * Mathf.Deg2Rad),
                                                0.0f) + player_position.transform.position 
                                                + new Vector3(0.0f, player_position.transform.localScale.y * 0.5f, 0.0f);
            current_angle += 1.0f;

            if(current_angle > 360)
            {
                current_angle = 0;
            }

            this.GetComponent<Collider>().enabled = false;
        }
    }


    protected void ApplyVelocity()
    {
        if(not_velocity_loss)
        {
            rigidbody.velocity = projectile_velocity;
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if(!collider.tag.Equals("Player") && !collider.tag.Equals("Projectile"))
        {
            Debug.Log("Dead: " + collider.name);
            Destroy(this.gameObject);
        }
    }


    public void SetPlayerPosition(GameObject new_pos)
    {
        this.player_position = new_pos;
    }


    public void Release()
    {
        this.GetComponent<Collider>().enabled = true;
        Debug.Log("Thrown: " + Time.time);
    }


    public void SetStartAngle(float start_radius)
    {
        current_angle = start_radius;
    }


    public void SetNoVelocity(bool set, Vector3 velocity)
    {
        not_velocity_loss = set;
        projectile_velocity = velocity;
    }


    public float GetAngle()
    {
        return current_angle;
    }


    public float GetRadius()
    {
        return radius;
    }
}
