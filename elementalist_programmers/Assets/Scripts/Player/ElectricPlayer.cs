using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricPlayer : PlayerController
{
    bool st_running = false;

    [Header("Electric Variables")]
    public float maxSpeed = 15;
    public float special_speed = 25f;
    public float special_dash_time = 0.3f;
   
    bool is_special_dashing = false;
    bool special_reset = true;
    float current_special_movement_time;
    Vector2 special_movement_target;
    Vector2 special_movement_velocity;

    public Material player_mat;
    public Material special_mat;

    bool enemy_hit = false;
    protected override void Awake()
    {
   
        base.Awake();
    }
    public override void FixedUpdate()
    {
        GainSpeed();
        if (grounded)
        {
            special_reset = true;
        }
        base.FixedUpdate();
        if (!death_status)
        {
            EngageSpecialMovement();
        }
    }

    private void GainSpeed()
    {
        if (rigbod.velocity == new Vector3(0f, 0f))
        {
            if (!st_running) StartCoroutine("StopTimer");
        }
        else
        {
            if (st_running)
            {
                StopCoroutine("StopTimer");
                st_running = false;
            }
            if (moveSpeed < maxSpeed) moveSpeed += 0.02f;
        }
    }

    public IEnumerator StopTimer()
    {
        st_running = true;
        yield return new WaitForSeconds(0.2f);
        moveSpeed = dSpeed;
        print("leaving coroutine");
    }

    public void OnSpecial(InputValue value)
    {
        if (!death_status)
        {
            if (!is_secondary_moving && !is_special_dashing && special_reset && !on_wall && !stunned &&!trapped
                && Vector2.Distance(this.transform.position, retical.transform.position) >= 0.1f)
            {
                current_special_movement_time = 0;
                special_movement_velocity = new Vector2(move.x, move.y) * special_speed;
                grounded = false;

                //Physics.IgnoreLayerCollision(8, 9, true);
                rigbod.velocity = special_movement_velocity;
                is_special_dashing = true;
                wall_jump = false;
                attacking = true;
              //  if(transform.childCount != 0)
            //    transform.GetChild(0).GetComponent<Renderer>().material = special_mat;
            }
        }
    }

    public override void AttackHit()
    {
        enemy_hit = true;
    }

    protected void EngageSpecialMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(facing * 0.75f, 0.0f, 0.0f);

        if (is_special_dashing)
        {

            if (current_special_movement_time < special_dash_time && !stunned)
            {
                current_special_movement_time += Time.deltaTime;
                rigbod.velocity = special_movement_velocity;
            }
            else
            {
                //Physics.IgnoreLayerCollision(8, 9, false);
                is_special_dashing = false;
                if(!stunned)rigbod.velocity = Vector2.zero;
                attacking = false;
                special_reset = enemy_hit;
                enemy_hit = false;
                //if (transform.childCount != 0)
                   // transform.GetChild(0).GetComponent<Renderer>().material = player_mat;
            }
        }
    }
}
