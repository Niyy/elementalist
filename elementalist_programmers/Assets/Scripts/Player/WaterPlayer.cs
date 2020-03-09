using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterPlayer : PlayerController
{
    // Update is called once per frame
    bool hover = true;
    public float hover_multiplayer = .2f;
    bool hovering = false;
    public float hover_time = 4f;
    public float hover_elapsed_time;

    public float special_speed = 25f;
    public float special_dash_time = 0.3f;
    bool is_special_dashing = false;
    bool special_reset = true;
    float current_special_movement_time;
    Vector2 special_movement_target;
    Vector2 special_movement_velocity;

    bool enemy_hit = false;

    public override void FixedUpdate()
    {
        if (grounded)
        {
            special_reset = true;
            hover = false;
            hover_elapsed_time = 0f;
        }
        base.FixedUpdate();
        if (!death_status)
        {
            EngageSpecialMovement();
        }
    }

    public void OnSpecial(InputValue value)
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

        if (hover && held_jump && hover_elapsed_time < hover_time)
        {
            if (!hovering)
            {
                hovering = true;
                rigbod.velocity = new Vector3(rigbod.velocity.x, 0f);
            }
            rigbod.AddForce(-Physics.gravity+Vector3.up* hover_multiplayer);
            hover_elapsed_time += 0.02f;
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
}
