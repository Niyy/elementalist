using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    protected Vector2 move;
    private Rigidbody rigbod;
    protected float moveSpeed = 10f;
    protected float wall_slide_speed = -2.0f;
    public int facing_direction = 1;

    public bool held_jump = false;
    public bool jump = false;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool grounded;
    public bool wall_sliding;

    public bool wall_jump = false;
    public bool new_jump = false;
    public Vector3 wall_jump_direction = (new Vector3(1.0f, 1.1f, 0.0f));
    public float wall_jump_force = 13f;

    public GameObject retical;
    private float retical_radius = 2.5f;

    private void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
        controls = new PlayerControls();


        controls.Gameplay.Jump.performed += ctx => { held_jump = true; Jump(); };
        controls.Gameplay.Jump.canceled += ctx => held_jump = false;
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private void Start()
    {
        wall_jump_direction.Normalize();
    }

    void Update()
    {
        grounded = GetComponent<Collision>().on_ground;
        //to wallslide player must not be grounded, they must be traveling down the wall, and must press against the wall or already be wall sliding
        wall_sliding = (GetComponent<Collision>().on_wall && !grounded && rigbod.velocity.y < 0 && (wall_sliding || move.x/facing_direction > 0));
        if((grounded || wall_sliding) && !new_jump)
        {
            wall_jump = false;
            jump = false;
        }
        else
        {
            new_jump = false;
        }
        Move();
        ReticleMovement();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(move.x, move.y, 0f);
        if (move.x > 0)
        {
            facing_direction = 1;
        }
        else if (move.x < 0)
        {
            facing_direction = -1;
        }
        if (!wall_jump)
        {
            rigbod.velocity = (new Vector3(direction.x * moveSpeed, rigbod.velocity.y));
        }
        else
        {
            rigbod.velocity = Vector3.Lerp(rigbod.velocity, (new Vector3(direction.x * moveSpeed, rigbod.velocity.y)), .7f * Time.deltaTime);
        }
        Airborn();
        if (wall_sliding)
        {
            rigbod.velocity = (new Vector3(rigbod.velocity.x, rigbod.velocity.y));
            if(rigbod.velocity.y < wall_slide_speed)
            {
                rigbod.velocity = (new Vector3(rigbod.velocity.x, wall_slide_speed, 0.0f));
            }
        }
    }

    private void Jump()
    {
        if (grounded)
        {
            rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
            jump = true;
            grounded = false;
            new_jump = true;
        }
        else if (wall_sliding || (GetComponent<Collision>().on_wall && (move.x/ facing_direction) > 0))
        {
            wall_sliding = false;
            rigbod.velocity = (new Vector3(wall_jump_force * wall_jump_direction.x * -facing_direction, wall_jump_force * wall_jump_direction.y));

            facing_direction *= -1;
            wall_jump = true;
            new_jump = true;
        }        
    }


    private void Airborn()
    {
        if (rigbod.velocity.y < 0 && !wall_sliding)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigbod.velocity.y > 0 && !held_jump && jump)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ReticleMovement()
    {
        RaycastHit hit;
        Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * retical_radius;

        if(Physics.Raycast(this.transform.position, left_stick_position, out hit, retical_radius))
        {
            retical.transform.position = hit.point;
        }
        else
        {
            left_stick_position += this.transform.position;
            retical.transform.position = left_stick_position;
        }

        if(left_stick_position != this.transform.position)
        {
            retical.transform.position += Vector3.forward * -9.0f;
        }
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
