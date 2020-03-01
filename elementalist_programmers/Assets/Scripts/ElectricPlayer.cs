using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricPlayer : PlayerController
{
    // Update is called once per frame
    bool st_running = false;

    float special_speed = 25f;
    bool is_special_dashing = false;
    bool special_reset = true;
    float current_special_movement_time;
    Vector2 special_movement_target;
    Vector2 special_movement_velocity;

    public override void FixedUpdate()
    {
        if (rigbod.velocity == new Vector3(0f,0f))
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
            if(moveSpeed < 20f) moveSpeed += 0.02f;
        }
        base.FixedUpdate();
    }

    public IEnumerator StopTimer()
    {
        st_running = true;
        yield return new WaitForSeconds(0.2f);
        moveSpeed = 10f;
        print("leaving coroutine");
    }

    public void OnSpecial(InputValue value)
    {
        if (!death_status)
        {
            if (!is_secondary_moving && secondary_reset && !on_wall && !stunned
                && Vector2.Distance(this.transform.position, retical.transform.position) >= 0.1f)
            {
                current_secondary_movement_time = 0;
                secondary_movement_velocity = new Vector2(move.x, move.y) * secondary_speed;
                grounded = false;

                Physics.IgnoreLayerCollision(8, 9, true);
                rigbod.velocity = secondary_movement_velocity;
                is_special_dashing = true;
                wall_jump = false;
            }
        }
    }
}
