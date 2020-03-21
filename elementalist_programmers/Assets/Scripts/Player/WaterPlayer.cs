using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterPlayer : PlayerController
{
    
    [Header("Hover Variables")]
    bool hover = true;
    public float max_hover_speed = 7f;
    public float max_hover_multiplayer = 4f;
    float hover_multiplayer;
    bool hovering = false;
    public float max_hover_time = 4f;
    public float hover_elapsed_time;
    public GameObject water_bar;
    private float water_bar_size;

    [Header("Dash Variables")]
    public float special_speed = 25f;
    public float special_dash_time = 0.3f;
    bool is_special_dashing = false;
    bool special_reset = true;
    float current_special_movement_time;
    Vector2 special_movement_target;
    Vector2 special_movement_velocity;

    [Header("Special Variables")]
    public float wave_force = 20f;
    public float wave_up_force = 1f;
    public float wave_radius = 3f;
    //public float wave_cooldown = 3f;
    private bool using_wave = false;
    private Vector3 wave_pos;
    [HideInInspector]
    public Collider[] colliders;

    bool enemy_hit = false;

    public override void Awake()
    {
        water_bar_size = water_bar.transform.localScale.y;
        base.Awake();
    }

    public override void FixedUpdate()
    {
        if (grounded)
        {
            special_reset = true;
            hover = false;
            
            hover_multiplayer = 0f;
            //water_bar.transform.localScale = new Vector3(water_bar.transform.localScale.x, water_bar_size);
            if (hover_elapsed_time > 0f)
            {
                hover_elapsed_time -= 0.04f;
            }
            if(hover_elapsed_time < 0f)
            {
                hover_elapsed_time = 0f;
            }
            float time_ratio = 1f - hover_elapsed_time / max_hover_time;
            water_bar.transform.localScale = new Vector3(water_bar.transform.localScale.x, time_ratio * water_bar_size);
        }
        base.FixedUpdate();
        if (!death_status)
        {
            EngageSpecialMovement();
            if (using_wave)
            {
                WavePush();
            }
        }
    }

    public override void OnMove(InputValue value)
    {
        base.OnMove(value);
        Debug.Log("move:" + move);
    }

    public override void OnDash(InputValue value)
    {
        if (!death_status)
        {
            if (!is_secondary_moving && !is_special_dashing && special_reset && !on_wall && !stunned
                && Vector2.Distance(this.transform.position, retical.transform.position) >= 0.1f)
            {
                current_special_movement_time = 0;
                special_movement_velocity = new Vector2(move.x, move.y) * secondary_speed;
                grounded = false;

                Physics.IgnoreLayerCollision(8, 9, true);
                rigbod.velocity = special_movement_velocity;
                is_special_dashing = true;
                wall_jump = false;
                attacking = true;
                transform.Find("Aura").gameObject.SetActive(true);
            }
        }
    }

    public override void AttackHit()
    {
        enemy_hit = true;
    }

    public override void Airborn()
    {

        if (hover && held_jump && hover_elapsed_time < max_hover_time)
        {
            if (!hovering)
            {
                hovering = true;
                //if (rigbod.velocity.y > 0)
                //{
                //    rigbod.velocity = Vector3.zero;
                //}
            }
            if(rigbod.velocity.y < 0)
            {
                rigbod.AddForce(-Physics.gravity + Vector3.up * 8f * -rigbod.velocity.y + Vector3.up);
            }
            else if (rigbod.velocity.y < max_hover_speed)
            {
                rigbod.AddForce(-Physics.gravity+Vector3.up * hover_multiplayer);
            }
            if (hover_multiplayer < max_hover_multiplayer)
                hover_multiplayer += 0.1f;
            hover_elapsed_time += 0.02f;
            float time_ratio =  1f - hover_elapsed_time / max_hover_time;
            water_bar.transform.localScale = new Vector3(water_bar.transform.localScale.x, time_ratio * water_bar_size);
        }
        else
        {
            hovering = false;
            base.Airborn();
        }
    }

    public override void OnJump(InputValue value)
    {
        base.OnJump(value);
        if (!death_status)
        {
            //hover if player holds jump again
            if(!grounded && !(wall_sliding || wall_push) )
            {
                hover = true;
            }
        }
    }

    protected void EngageSpecialMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(facing * 0.75f, 0.0f, 0.0f);

        if (is_special_dashing)
        {

            if (current_special_movement_time < secondary_movement_time && !stunned)
            {
                current_special_movement_time += Time.deltaTime;
                rigbod.velocity = special_movement_velocity;
            }
            else
            {
                Physics.IgnoreLayerCollision(8, 9, false);
                is_special_dashing = false;
                if(!stunned)rigbod.velocity = Vector2.zero;
                attacking = false;
                special_reset = enemy_hit;
                enemy_hit = false;
                transform.Find("Aura").gameObject.SetActive(false);
            }
        }
    }


    public void OnSpecial()
    {
        using_wave = true;
    }

    public void WavePush()
    {
        wave_pos = transform.position;
        colliders = Physics.OverlapSphere(wave_pos, wave_radius);
        if(max_hover_time == 0f)
        {
            max_hover_time = 0.0001f;
        }
        float time_ratio = 1f - hover_elapsed_time / max_hover_time;
        if (time_ratio == 1f)
        {
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null && !hit.CompareTag("Player"))
                {
                    rb.AddExplosionForce(wave_force, wave_pos, wave_radius, wave_up_force, ForceMode.Impulse);
                }
            }
            
            hover_elapsed_time = max_hover_time;
            water_bar.transform.localScale = new Vector3(water_bar.transform.localScale.x, 0f);
        }
        using_wave = false;

    }
}
