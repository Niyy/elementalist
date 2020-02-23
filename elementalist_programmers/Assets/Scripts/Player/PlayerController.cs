﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Respawn Variables
    [Header("Respawn Variables")]
    public GameObject respawn_point;


    // Movement Variables
    [Header("Movement Variables")]
    public float moveSpeed = 7f;
    public float stunned_wait_timer;


    //[SerializeField]
    public PlayerInput playerInput;
    PlayerControls controls;
    protected Vector2 move;
    private Rigidbody rigbod;
    private bool stunned;
    private float stunned_counter;
    private Vector3 stunned_forces;


    [Header("Jumping Variables")]
    public bool held_jump = false;
    public bool jump = false;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool grounded;


    [Header("Wall Jumping Variables")]
    public bool wall_jump = false;
    public bool new_jump = false;
    public Vector3 wall_jump_direction = (new Vector3(.7f, 1f, 0.0f));
    public float wall_jump_force = 10f;
    public bool on_wall;
    public bool wall_push = false;
    public bool wall_sliding;
    protected float wall_slide_speed = -2.0f;
    int player_id;

    // Retical variables
    [Header("Reticle Variables")]
    public GameObject ui_retical_prefab;


    private float retical_radius = 2.5f;
    private GameObject ui_retical;
    private GameObject retical;


    // Secondary Movement variables
    [Header("Secondary Movement Variables")]
    public float secondary_speed;
    public float secondary_movement_time;
    public SecondaryMovementTypes secondary_movement = SecondaryMovementTypes.Dash;
    public enum SecondaryMovementTypes
    {
        Dash,
        Roll
    }


    private bool is_secondary_moving = false;
    private bool secondary_reset = true;
    private float current_secondary_movement_time;
    private Vector2 secondary_movement_target;
    private Vector2 secondary_movement_velocity;

    PlayerInput inputAction;

    // Direction variable
    [Header("Direction Variables")]
    [Range(-1, 1)]
    public float facing;
    

    private Vector3 direction;


    //Combat variables
    private bool attacking;


    // Camera for player
    Camera player_camera;


    // Canvas
    Canvas canvas;


    // Death Variables
    private float death_timer;
    private float current_timer;
    public bool death_status;

    bool unsaved = true;


    [Header("Testing variables")]
    public bool death_test = false;
    

    // Start is called before the first frame update
    private void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
        //controls = new PlayerControls();
        //controls.Gameplay.Dash.performed += ctx => ImplementSecondaryMovement();
        //controls.Gameplay.Jump.performed += ctx => { held_jump = true;};
        //controls.Gameplay.Jump.canceled += ctx => held_jump = false;
        //controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        //if (unsaved)
        //{
        //    GameObject playerManager = GameObject.Find("PlayerManager");
        //    unsaved = false;
        //    playerManager.GetComponent<PlayerManager>().GetPlayers(this.gameObject);
        //}

        ui_retical = GameObject.Find("/Canvas/UI_Retical");
        player_camera = Camera.main;
        retical = new GameObject("Reticle_" + this.gameObject.name);
        canvas = GameObject.Find("/Canvas").GetComponent<Canvas>();

        stunned = false;
        death_status = false;
        attacking = false;

        stunned_counter = stunned_wait_timer;
    }
    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        direction = Vector2.right;
        wall_jump_direction.Normalize();
    }


    private void Update()
    {
        ReticleMovement();
        TestFunctions();
    }


    private void FixedUpdate()
    {
        on_wall = GetComponent<PlayerCollision>().on_wall;
        grounded = GetComponent<PlayerCollision>().on_ground;

        if (grounded)
        {
            secondary_reset = true;
        }

        wall_push = (move.x * facing > 0) && (GetComponent<PlayerCollision>().col_face == facing);
        //to wallslide player must not be grounded, they must be traveling down the wall, and must press against the wall or already be wall sliding
        wall_sliding = (GetComponent<PlayerCollision>().on_wall && !grounded && rigbod.velocity.y < 0 && (wall_sliding || wall_push));
        if ((grounded || wall_sliding) && !new_jump)
        {
            wall_jump = false;
            jump = false;
        }
        else
        {
            new_jump = false;
        }

        
        Move();
        EngageSecondaryMovement();
    }

    private void Move()
    {
        if (!is_secondary_moving && !stunned)
        {
            direction = new Vector3(move.x, move.y, 0f);
            if (direction.x != 0)
            {
                facing = Mathf.Sign(direction.x);
            }
            if (!wall_jump)
            {
                rigbod.velocity = (new Vector3(direction.x * moveSpeed, rigbod.velocity.y));
            }
            else
            {
                rigbod.velocity = Vector3.Lerp(rigbod.velocity, (new Vector3(direction.x * moveSpeed, rigbod.velocity.y)), .7f * Time.fixedDeltaTime);
            }
            Airborn();
            if (wall_sliding)
            {
                rigbod.velocity = (new Vector3(rigbod.velocity.x, rigbod.velocity.y));
                if (rigbod.velocity.y < wall_slide_speed)
                {
                    rigbod.velocity = (new Vector3(rigbod.velocity.x, wall_slide_speed, 0.0f));
                }
            }
        }
        else
        {
            StunnedActions();
        }
    }

    private void OnJump(InputValue value)
    {
        if (grounded)
        {
            rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
            jump = true;
            print(jump);
            grounded = false;
            new_jump = true;
        }
        else if (wall_sliding || (GetComponent<PlayerCollision>().on_wall && wall_push))
        {
            wall_sliding = false;
            rigbod.velocity = (new Vector3(wall_jump_force * wall_jump_direction.x * -facing, wall_jump_force * wall_jump_direction.y));

            facing *= -1f;
            wall_jump = true;
            new_jump = true;
        }
    }

    private void OnJumpPress(InputValue value)
    {
        held_jump = value.isPressed;
    }

    private void Airborn()
    {
        if (rigbod.velocity.y < 0 && !wall_sliding)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigbod.velocity.y > 0 && !held_jump)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ReticleMovement()
    {
        RaycastHit hit;
        Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * retical_radius;

        if (Physics.Raycast(this.transform.position, left_stick_position, out hit, retical_radius))
        {
            retical.transform.position = hit.point;
        }
        else
        {
            left_stick_position += this.transform.position;
            retical.transform.position = left_stick_position;
        }

        ui_retical.transform.position = player_camera.WorldToScreenPoint(retical.transform.position);
    }


    private void OnDash()
    {
        if(!death_status)
        {
            if (!is_secondary_moving && secondary_reset && !on_wall && !stunned
                && Vector2.Distance(this.transform.position, retical.transform.position) >= 0.1f)
            {
                current_secondary_movement_time = 0;
                if (secondary_movement == SecondaryMovementTypes.Roll)
                {
                    secondary_movement_velocity = new Vector2(Mathf.Sign(facing), 0.0f) * secondary_speed;
                }
                else
                {
                    secondary_movement_velocity = new Vector2(move.x, move.y) * secondary_speed;
                    grounded = false;
                }

                Physics.IgnoreLayerCollision(8, 9, true);
                rigbod.velocity = secondary_movement_velocity;
                is_secondary_moving = true;
                wall_jump = false;
            }
        }
    }


    private void EngageSecondaryMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(facing * 0.75f, 0.0f, 0.0f);

        if (is_secondary_moving)
        {
            RaycastHit check_down;

            if (secondary_movement == SecondaryMovementTypes.Roll
            && !Physics.Raycast(next_position, Vector2.down * 2.0f, out check_down, 1.0f))
            {
                current_secondary_movement_time = secondary_movement_time;
            }

            if (current_secondary_movement_time < secondary_movement_time)
            {
                current_secondary_movement_time += Time.deltaTime;
                rigbod.velocity = secondary_movement_velocity;
            }
            else
            {
                Physics.IgnoreLayerCollision(8, 9, false);
                is_secondary_moving = false;
                secondary_reset = false;
                rigbod.velocity = Vector2.zero;
            }
        }

        Debug.DrawRay(next_position, Vector2.down, Color.green);
    }


    private void StunnedActions()
    {
        if(stunned_counter == 0)
        {
            rigbod.velocity = stunned_forces;
        }

        if(stunned_counter < stunned_wait_timer && rigbod.velocity != Vector3.zero)
        {
            stunned_counter += Time.deltaTime;
        }
        else
        {
            stunned = false;
        }
    }


    private void TestFunctions()
    {
        TestDeath();
    }


    private void TestDeath()
    {
        if(death_test)
        {
            PlayerDeath();
            death_test = false;
        }
    }


    public void SetRespawnPoint(GameObject newSpawnPoint)
    {
        Debug.Log("Setting new spawn position. " + newSpawnPoint.name);
        respawn_point = newSpawnPoint;
    }


    private void Revive()
    {
        death_status = false;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<Collider>().isTrigger = false;
        rigbod.isKinematic = false;
    }


    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag.Equals("Player") && col.GetComponent<PlayerController>())
        {
            Revive();
        }

        Debug.Log("Someone there.");
    }


    public bool IsSecondary()
    {
        return is_secondary_moving;
    }


    public void PlayerDeath(float death_time = 0.0f)
    {
        death_status = true;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().isTrigger = true;
        rigbod.isKinematic = true;
    }


    public void Recoil(float recoil_speed, int enemy_direction)
    {
        stunned_forces = new Vector3(enemy_direction * recoil_speed, rigbod.velocity.y, 0.0f);
        stunned = true;
        stunned_counter = 0;
    }


    public bool GetAttackStatus()
    {
        return attacking;
    }


    public bool IsInVulnerable()
    {
        return is_secondary_moving;
    }


    //void OnEnable()
    //{
    //    controls.Gameplay.Enable();
    //}

    //private void OnDisable()
    //{
     //   controls.Gameplay.Disable();
    //}
}