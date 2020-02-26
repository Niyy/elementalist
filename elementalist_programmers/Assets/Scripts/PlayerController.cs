using System;
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
    public float stunned_wait_timer;

    PlayerControls controls;
    protected Vector2 move;
    public Rigidbody rigbod;
    public float moveSpeed = 10f;
    protected bool stunned;
    protected float stunned_counter;
    protected Vector3 stunned_forces;


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

    // Retical variables
    [Header("Reticle Variables")]
    public GameObject retical;


    protected float retical_radius = 2.5f;
    protected GameObject ui_retical;


    // Secondary Movement variables
    [Header("Secondary Movement Variables")]
    protected float secondary_speed;
    protected float secondary_movement_time;
    protected SecondaryMovementTypes secondary_movement = SecondaryMovementTypes.Dash;
    protected enum SecondaryMovementTypes
    {
        Dash,
        Roll
    }


    protected bool is_secondary_moving = false;
    protected bool secondary_reset = true;
    protected float current_secondary_movement_time;
    protected Vector2 secondary_movement_target;
    protected Vector2 secondary_movement_velocity;


    // Direction variable
    [Header("Direction Variables")]
    [Range(-1, 1)]
    public float facing;
    

    protected Vector3 direction;


    //Combat variables
    protected bool attacking;


    // Camera for player
    protected Camera player_camera;


    // Canvas
    protected Canvas canvas;


    // Death Variables
    protected float death_timer;
    protected float current_timer;


    // Start is called before the first frame update
    protected void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Gameplay.Dash.performed += ctx => ImplementSecondaryMovement();
        controls.Gameplay.Jump.performed += ctx => { held_jump = true; Jump(); };
        controls.Gameplay.Jump.canceled += ctx => held_jump = false;
        controls.Gameplay.Move.performed += ctx => { move = ctx.ReadValue<Vector2>(); print("move"); } ;
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        ui_retical = GameObject.Find("/Canvas/UI_Retical");
        player_camera = Camera.main;
        retical = GameObject.Find("/Reticle");
        canvas = GameObject.Find("/Canvas").GetComponent<Canvas>();
        stunned = false;

        stunned_counter = stunned_wait_timer;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        direction = Vector2.right;
        wall_jump_direction.Normalize();
    }


    protected void Update()
    {
        ReticleMovement();
    }


    public virtual void FixedUpdate()
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

    protected void Move()
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

    protected void Jump()
    {
        if (grounded)
        {
            rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
            jump = true;
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


    protected void Airborn()
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

    protected void ReticleMovement()
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


    protected void ImplementSecondaryMovement()
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


    protected void EngageSecondaryMovement()
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


    public bool IsInVulnerable()
    {
        return is_secondary_moving;
    }


    protected void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    protected void OnDisable()
    {
        controls.Gameplay.Disable();
    }


    protected void StunnedActions()
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


    public void SetRespawnPoint(GameObject newSpawnPoint)
    {
        Debug.Log("Setting new spawn position. " + newSpawnPoint.name);
        respawn_point = newSpawnPoint;
    }


    public void PlayerDeath(float death_time)
    {

    }


    public void Recoil(float recoil_speed, int enemy_direction)
    {
        stunned_forces = new Vector3(enemy_direction * recoil_speed, rigbod.velocity.y, 0.0f);
        stunned = true;
        stunned_counter = 0;
        Debug.Log("Shouldn't I be flying back. " + rigbod.velocity);
    }


    public bool GetAttackStatus()
    {
        return attacking;
    }
}