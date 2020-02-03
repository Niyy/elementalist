﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    protected Vector2 move;
    private Rigidbody rigbod;
    protected float moveSpeed = 10f;

    public bool jump = false;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Retical variables
    public GameObject retical;


    private float retical_radius = 2.5f;


    // Secondary Movement variables
    public float secondary_speed;
    public float secondary_movement_time;


    private bool is_secondary_moving = false;
    private float current_secondary_movement_time;
    private Vector2 secondary_movement_target;
    private Vector2 secondary_movement_velocity;

    // Start is called before the first frame update
    private void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Dash.performed += ctx => ImplementSecondaryMovement();
        controls.Gameplay.Jump.canceled += ctx => jump = false;
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }


    private void Update() 
    {
        ReticleMovement();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        Airborn();
        EngageSecondaryMovement();
    }

    private void Move()
    {
        if(!is_secondary_moving)
        {
            Vector3 direction = new Vector3(move.x, move.y, 0f);
            rigbod.velocity = (new Vector3(direction.x * moveSpeed, rigbod.velocity.y));
        }
    }

    private void Jump()
    {
        jump = true;
        rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
    }

    private void Airborn()
    {
        if (rigbod.velocity.y < 0)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigbod.velocity.y > 0 && !jump)
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


    private void ImplementSecondaryMovement()
    {
        current_secondary_movement_time = 0;
        secondary_movement_velocity = new Vector2(move.x * secondary_speed, move.y * secondary_speed);
        rigbod.velocity = secondary_movement_velocity;
        is_secondary_moving = true;
    }


    private void EngageSecondaryMovement()
    {
        if(is_secondary_moving)
        {
            if (current_secondary_movement_time < secondary_movement_time)
            {
                rigbod.velocity = secondary_movement_velocity;
                current_secondary_movement_time += Time.deltaTime;
            }
            else
            {
                is_secondary_moving = false;
                rigbod.velocity = Vector2.zero;
            }
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
