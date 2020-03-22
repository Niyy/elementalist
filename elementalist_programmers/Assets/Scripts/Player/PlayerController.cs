using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [HideInInspector] public float dSpeed;


    //[SerializeField]
    //PlayerControls controls;
    protected Vector2 move;
    protected Rigidbody rigbod;
    protected bool stunned;
    protected float stunned_counter;
    protected Vector3 stunned_forces;


    [Header("Jumping Variables")]
    public bool held_jump = false;
    public bool jump = false;
    public float jumpForce = 7f;
   [HideInInspector] public float djump;
    public float fallMultiplier = 2.5f;
    [HideInInspector] public float dFall;
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
    protected float wall_jump_interpolant = 0f;
    public float wj_interpolant_gain = 0.1f;

    // Retical variables
    [Header("Reticle Variables")]
    public GameObject ui_retical_prefab;



    protected float retical_radius = 2.5f;
    protected GameObject ui_retical;
    protected GameObject retical;

    // Secondary Movement variables
    [Header("Secondary Movement Variables")]
    public float secondary_speed = 20;
    public float secondary_movement_time = 0.2f;
    public SecondaryMovementTypes secondary_movement = SecondaryMovementTypes.Dash;
    public enum SecondaryMovementTypes
    {
        Dash,
        Roll
    }


    protected bool is_secondary_moving = false;
    protected bool secondary_reset = true;
    protected float current_secondary_movement_time;
    protected Vector2 secondary_movement_target;
    protected Vector2 secondary_movement_velocity;

    PlayerInput inputAction;

    // Direction variable
    [Header("Direction Variables")]
    [Range(-1, 1)]
    public float facing;
    

    protected Vector3 direction;


    //Combat variables
    protected bool attacking;


    // Camera for player
    [HideInInspector]public Camera player_camera;


    // Canvas
    protected Canvas canvas;


    // Death Variables
    protected float death_timer;
    protected GameObject reviver;
    protected GameObject child;
    protected float current_timer;
    public bool death_status;
    [Header("Testing variables")]
    public bool death_test = false;
    //bool unsaved = true;


    // Animator
    protected Animator animator;

    //Traped in sand
    public bool trapped = false;


    private void OnEnable()
    {
        player_camera = Camera.main;
        print("onenable");
    }

    public virtual void Awake()
    {
        rigbod = GetComponent<Rigidbody>();

        //old input method
        //controls = new PlayerControls();
        //controls.Gameplay.Dash.performed += ctx => ImplementSecondaryMovement();
        //controls.Gameplay.Jump.performed += ctx => { held_jump = true;};
        //controls.Gameplay.Jump.canceled += ctx => held_jump = false;
        //controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        stunned = false;
        death_status = false;
        attacking = false;

        stunned_counter = stunned_wait_timer;

        animator = GetComponentInChildren<Animator>();
        child = this.transform.GetChild(0).gameObject;
        ui_retical = GameObject.Find("/SceneManagement/Canvas/UI_Retical");
        player_camera = Camera.main;
        retical = new GameObject("Reticle_" + this.gameObject.name);
        retical.transform.parent = transform;
        canvas = GameObject.Find("/SceneManagement/Canvas").GetComponent<Canvas>();
        Debug.Log("Awake set-up done. " + animator);
    }

    public virtual void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        direction = Vector2.right;
        wall_jump_direction.Normalize();
        dSpeed = moveSpeed;
        djump = jumpForce;
        dFall = fallMultiplier;
    }


    protected virtual void Update()
    {
        if(!death_status)
        {
            ReticleMovement();
            TestFunctions();
            AnimationHandler();
        }
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
            wall_jump_interpolant = 0f;
            jump = false;
        }
        else
        {
            new_jump = false;
        }

        if(!death_status)
        {
            Move();
            EngageSecondaryMovement();
        }
        Revive();
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
                rigbod.velocity = Vector3.Lerp(rigbod.velocity, (new Vector3(direction.x * moveSpeed, rigbod.velocity.y)), wall_jump_interpolant * Time.deltaTime);
                if (wall_jump_interpolant * Time.deltaTime < 1.0f)
                {
                    wall_jump_interpolant += wj_interpolant_gain;
                }
                else
                {
                    wall_jump = false;
                }
            }
            if (!grounded)
            {
                Airborn();
            }
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


    protected void AnimationHandler()
    {
        int angle = 0;
        PlayerCollision player_collision = GetComponent<PlayerCollision>();

        if(player_collision.on_wall && !player_collision.on_ground)
        {
            if(facing == 1)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }
        }
        else if(facing == 1)
        {
            angle = 180;
        }


        if(!animator.GetBool("holding_on_wall") && player_collision.on_wall 
            && !player_collision.on_ground)
        {
            animator.SetBool("holding_on_wall", true);
        }
        else if(animator.GetBool("holding_on_wall") && (!player_collision.on_wall 
                || player_collision.on_ground))
        {
            animator.SetBool("holding_on_wall", false);
        }

        if(!animator.GetBool("running") && rigbod.velocity != new Vector3(0.0f, rigbod.velocity.y, 0.0f)
            && player_collision.on_ground)
        {
            animator.SetBool("running", true);
        }
        else if(animator.GetBool("running") && (rigbod.velocity == new Vector3(0.0f, rigbod.velocity.y, 0.0f)
                || !player_collision.on_ground || player_collision.on_wall))
        {
            animator.SetBool("running", false);
        }

        animator.gameObject.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }


    public virtual void OnJump(InputValue value)
    {
        if(!death_status)
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
    }

    protected void OnJumpPress(InputValue value)
    {
        held_jump = value.isPressed;
    }

    public virtual void Airborn()
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

    protected void ReticleMovement()
    {
        RaycastHit hit;
        Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * retical_radius;


        if (Physics.Raycast(this.transform.position, left_stick_position, out hit, retical_radius, ~(1<<10))
            && !hit.collider.gameObject.tag.Equals("Player"))
        {
            retical.transform.position = hit.point;
        }
        else
        {
            left_stick_position += this.transform.position;
            retical.transform.position = left_stick_position;
        }

        ui_retical.transform.position = Camera.main.WorldToScreenPoint(retical.transform.position);
    }


    public virtual void OnDash(InputValue value)
    {
        if(!death_status)
        {
            if (!is_secondary_moving && secondary_reset && !on_wall && !stunned && !trapped
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


    public void Neutralize()
    {
        rigbod.velocity = Vector3.zero;
        move = Vector2.zero;
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


    protected void TestFunctions()
    {
        TestDeath();
    }


    protected void TestDeath()
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


    protected void Revive()
    {
        if(reviver && Vector3.Distance(this.transform.position, reviver.transform.position) >= 2.0f)
        {
            death_status = false;
            //this.GetComponent<MeshRenderer>().enabled = true;
            child.SetActive(true);
            this.GetComponent<Collider>().isTrigger = false;
            rigbod.isKinematic = false;
            reviver = null;
        }
    }

    public void PlayerReset()
    {
        death_status = false;

        //this.GetComponent<MeshRenderer>().enabled = true;
        child.SetActive(true);
        this.GetComponent<Collider>().isTrigger = false;
        rigbod.isKinematic = false;
        Neutralize();
        stunned = false;
        death_status = false;
        attacking = false;

        stunned_counter = stunned_wait_timer;
        gameObject.SetActive(false);
    }


    protected void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag.Equals("Player") && col.GetComponent<PlayerController>().IsSecondary())
        {
            reviver = col.gameObject;
        }
    }


    public bool IsSecondary()
    {
        return is_secondary_moving;
    }


    public virtual void PlayerDeath(float death_time = 0.0f)
    {
        if (PlayerManager.Instance.mode.Equals(Playmode.singleplayer))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerReset();
        }
        else
        {
            death_status = true;
            ui_retical.SetActive(false);
            child.SetActive(false);
            this.GetComponent<Collider>().isTrigger = true;
            rigbod.isKinematic = true;
            PlayerManager.Instance.LivingPlayersCheck();
        } 
    }


    public void Recoil(float recoil_speed, int enemy_direction)
    {
        stunned_forces = new Vector3(enemy_direction * recoil_speed, rigbod.velocity.y, 0.0f);
        print("stun: " + stunned_forces);
        stunned = true;
        stunned_counter = 0;
    }


    public bool GetAttackStatus()
    {
        return attacking;
    }

    public virtual void AttackHit()
    {

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