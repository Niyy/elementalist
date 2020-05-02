using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public float jumpForce = 7f;
   [HideInInspector] public float djump;
    public float fallMultiplier = 2.5f;
    [HideInInspector] public float dFall;
    public float lowJumpMultiplier = 2f;
    public bool grounded;
    public int jump_max = 1;
    protected float jump_cool_down;


    private int jump_count;
    protected float current_jump_cool_down;


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
    public GameObject ui_retical;
    protected GameObject retical;

    // Secondary Movement variables
    [Header("Secondary Movement Variables")]
    public float secondary_speed = 20;
    public float secondary_movement_time = 0.2f;
    public SecondaryMovementTypes secondary_movement;
    public enum SecondaryMovementTypes
    {
        Dash,
        Roll
    }


    protected bool is_secondary_moving;
    protected bool secondary_reset = true;
    protected float current_secondary_movement_time;
    protected float dash_cool_down;
    protected float current_dash_cool_down;
    protected float neutral_position;
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
    public bool debugging;
    //bool unsaved = true;


    // Animator
    protected Animator animator;
    protected float take_off_time;
    protected float idle_break_clip_max;
    protected float idle_break_clip_length;
    protected float idle_clip_length;

    //Traped in sand
    public bool trapped = false;


    // Key Buffer
    [Header("Player Forgivness Variables")]
    public float max_keypress_time;


    private enum InputType 
    {
        Jump,
        No_Press
    }
    private InputType last_keypress;
    private float current_keypress_time;


    protected virtual void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
        is_secondary_moving = false;
        Debug.Log("Secondary movement type: " + secondary_movement);

        stunned = false;
        death_status = false;
        attacking = false;
        current_jump_cool_down = jump_cool_down;

        last_keypress = InputType.No_Press;

        stunned_counter = stunned_wait_timer;

        animator = GetComponentInChildren<Animator>();
        child = this.transform.GetChild(0).gameObject;
        player_camera = Camera.main;
        retical = new GameObject("Reticle_" + this.gameObject.name);
        retical.transform.parent = transform;
        canvas = GameObject.Find("/SceneManagement/Canvas").GetComponent<Canvas>();
        ui_retical = Instantiate(ui_retical_prefab);
        ui_retical.name = "UI_Reticle_" + this.gameObject.name;
        ui_retical.transform.SetParent(canvas.transform, false);

        animator.SetBool("landed", true);

        FindAnimationTimes();
        ResetAnimationState();
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
        if ((grounded || wall_sliding))
        {
            wall_jump = false;
            wall_jump_interpolant = 0f;
            jump_count = 0;
        }

        
        if(grounded)
        {
            current_jump_cool_down += Time.deltaTime;
        }

        if(!death_status)
        {
            Move();
            EngageSecondaryMovement();
            CheckForLastKeyPress();
        }
        Revive();
    }

    protected void Move()
    {
        if (!is_secondary_moving || !stunned)
        {
            direction = new Vector3(move.x, move.y, 0f);
            if (direction.x != 0)
            {
                facing = Mathf.Sign(direction.x);
                neutral_position = 0;
            }
            else if(neutral_position < 5)
            {
                neutral_position++;
                Debug.Log("Neutral: " + neutral_position);
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
                current_jump_cool_down = 0;
                new_jump = false;
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

        DashAnimation(player_collision);
        JumpAnimation(player_collision);
        WallSlideAnimation(player_collision);
        RunningAnimation(player_collision);
        IdleBreakAnimation(player_collision);

        animator.gameObject.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }


    protected void DashAnimation(PlayerCollision player_collision)
    {
        if(is_secondary_moving)
        {
            animator.SetBool("dash", true);
        }
        else if(!is_secondary_moving && animator.GetBool("dash"))
        {
            animator.SetBool("dash", false);
        }
    }


    protected void JumpAnimation(PlayerCollision player_collision)
    {
        if(grounded && animator.GetBool("in_air"))
        {
            animator.SetBool("in_air", false);
            animator.SetBool("jumping", false);
            animator.SetBool("landed", true);
        }
        else if(!grounded && animator.GetBool("jumping") && !animator.GetBool("landed") && !wall_push)
        {
            animator.SetBool("in_air", true);
        }
        else if(current_jump_cool_down >= jump_cool_down && new_jump && animator.GetBool("landed"))
        {
            animator.SetBool("jumping", true);
            animator.SetBool("landed", false);
        }
        else if(animator.GetBool("landed") && !grounded)
        {
            animator.SetBool("in_air", true);
            animator.SetBool("landed", false);
        }
    }


    protected void WallSlideAnimation(PlayerCollision player_collision)
    {
        if(!animator.GetBool("holding_on_wall") && on_wall
            && !player_collision.on_ground)
        {
            animator.SetBool("holding_on_wall", true);
        }
        else if(animator.GetBool("holding_on_wall") && (!on_wall || player_collision.on_ground))
        {
            animator.SetBool("holding_on_wall", false);
        }
    }


    protected void RunningAnimation(PlayerCollision player_collision)
    {
        if(!animator.GetBool("running") && !is_secondary_moving && rigbod.velocity != new Vector3(0.0f, rigbod.velocity.y, 0.0f)
            && (animator.GetBool("landed")))
        {
            animator.SetBool("running", true);
        }
        else if(animator.GetBool("running") && ((rigbod.velocity == new Vector3(0.0f, rigbod.velocity.y, 0.0f) 
                && neutral_position > 4) || player_collision.on_wall))
        {
            animator.SetBool("running", false);
        }
    }


    protected void IdleBreakAnimation(PlayerCollision player_collision)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).length >= idle_clip_length &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetBool("idle_break", true);
            Debug.Log("idle_break on");
        }
        else if(animator.GetBool("idle_break") && idle_break_clip_length >= idle_break_clip_max)
        {
            animator.SetBool("idle_break", false);
        }
    }


    protected void ResetAnimationState()
    {
        animator.SetBool("idle_break", false);
        animator.SetBool("running", false);
        animator.SetBool("holding_on_wall", false);
        animator.SetBool("jumping", false);
        animator.SetBool("dash", false);
        animator.SetBool("in_air", false);

        animator.SetBool("landed", true);
    }


    public virtual void OnJump(InputValue value)
    {
        JumpCall();
    }


    protected virtual void JumpCall()
    {
        if(!death_status && !is_secondary_moving)
        {
            if (grounded && current_jump_cool_down >= jump_cool_down)
            {
                rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
                jump_count++;
                grounded = false;
                new_jump = true;
            }
            else if (wall_sliding || (GetComponent<PlayerCollision>().on_wall && wall_push))
            {
                wall_sliding = false;
                rigbod.velocity = (new Vector3(wall_jump_force * wall_jump_direction.x * -facing, wall_jump_force * wall_jump_direction.y));

                facing *= -1f;
                wall_jump = true;
                jump_count=0;
            }
            else if (jump_count < jump_max - 1 && (current_jump_cool_down >= jump_cool_down || !grounded))
            {
                rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
                jump_count++; 
                grounded = false;
                new_jump = true;
            }
            else if (current_jump_cool_down < jump_cool_down)
            {
                last_keypress = InputType.Jump;
                current_keypress_time = 0;
                Debug.Log("Gave the player input: " + last_keypress);
            }
        }
    }


    protected void EngageJump()
    {
        if(!death_status && !is_secondary_moving)
        {
            if (grounded && current_jump_cool_down >= jump_cool_down)
            {
                rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
                jump_count++;
                grounded = false;
                new_jump = true;
                last_keypress = InputType.No_Press;
            }
            else if (wall_sliding || (GetComponent<PlayerCollision>().on_wall && wall_push))
            {
                wall_sliding = false;
                rigbod.velocity = (new Vector3(wall_jump_force * wall_jump_direction.x * -facing, wall_jump_force * wall_jump_direction.y));

                facing *= -1f;
                wall_jump = true;
                jump_count=0;
                last_keypress = InputType.No_Press;
            }
            else if (jump_count < jump_max - 1 && (current_jump_cool_down >= jump_cool_down || !grounded))
            {
                rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
                jump_count++; 
                grounded = false;
                new_jump = true;
                last_keypress = InputType.No_Press;
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
            if (!is_secondary_moving && current_dash_cool_down >= dash_cool_down 
                && secondary_reset && !on_wall && !stunned && !trapped
                && Vector2.Distance(this.transform.position, retical.transform.position) >= 0.1f)
            {
                current_secondary_movement_time = 0;
                if (secondary_movement == SecondaryMovementTypes.Roll && grounded)
                {
                    secondary_movement_velocity = new Vector2(Mathf.Sign(facing), 0.0f) * secondary_speed;
                    Debug.Log("Rolling!");
                }
                else if(secondary_movement == SecondaryMovementTypes.Dash)
                {
                    secondary_movement_velocity = move.normalized * secondary_speed;
                    grounded = false;
                }
                else
                {
                    return;
                }

                gameObject.layer = 11;
                //Physics.IgnoreLayerCollision(8, 9, true);
                rigbod.velocity = secondary_movement_velocity;
                is_secondary_moving = true;
                wall_jump = false;
                current_dash_cool_down = 0;
            }
        }
    }


    public virtual void EngageSecondaryMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(facing * 0.75f, 0.0f, 0.0f);

        if (is_secondary_moving)
        {
            RaycastHit check_down;

            if (secondary_movement == SecondaryMovementTypes.Roll && grounded
            && !Physics.Raycast(next_position, Vector2.down * 2.0f, out check_down, 1.0f))
            {
                current_secondary_movement_time = secondary_movement_time;
                Debug.Log("Rolling.");
            }
            else if (secondary_movement == SecondaryMovementTypes.Dash 
                    && current_secondary_movement_time < secondary_movement_time)
            {
                current_secondary_movement_time += Time.deltaTime;
                rigbod.velocity = secondary_movement_velocity;
            }
            else
            {
                gameObject.layer = 8;
                //Physics.IgnoreLayerCollision(8, 9, false);
                is_secondary_moving = false;
                secondary_reset = false;
                rigbod.velocity = Vector2.zero;
            }
        }
        else
        {
            current_dash_cool_down += Time.deltaTime;
        }

        //Debug.Log("current_dash_cool_down: " + current_dash_cool_down);
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
        respawn_point = newSpawnPoint;
    }


    protected void Revive()
    {
        if(reviver && Vector3.Distance(this.transform.position, reviver.transform.position) >= 2.0f)
        {
            death_status = false;
            //this.GetComponent<MeshRenderer>().enabled = true;
            child.SetActive(true);
            ui_retical.SetActive(true);
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
    }


    protected void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag.Equals("Player") && col.GetComponent<PlayerController>().IsSecondary())
        {
            reviver = col.gameObject;
        }
    }


    protected virtual void FindAnimationTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Jump_Landing":
                Debug.Log("Jump_Landing clip.Length: " + clip.length);
                    jump_cool_down = clip.length;
                    break;
                case "Slide":
                    dash_cool_down = clip.length;
                    break;
                case "Dash":
                    dash_cool_down = clip.length;
                    break;
                case "Idle":
                    idle_clip_length = clip.length;
                    break;
                default:
                    break;
            }
        }
    }


    protected void CheckForLastKeyPress()
    {
        if(current_keypress_time <= max_keypress_time && last_keypress != InputType.No_Press)
        {
            switch(last_keypress)
            {
                case InputType.Jump:
                    EngageJump();
                    break;
            }
        }
        else
        {
            last_keypress = InputType.No_Press;
        }

        current_keypress_time += Time.deltaTime;
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

    private void OnDisable()
    {
        if(ui_retical != null)
        {
            ui_retical.SetActive(false);
        }
    }
}