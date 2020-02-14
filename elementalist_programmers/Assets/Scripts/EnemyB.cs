using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    [Header("B-Tier Variables")]
    [Range(-1, 1)]
    public int direction;
    public float patrol_wait_timer;

    private float current_patrol_timer;


    private Rigidbody rigidbody;
    private float lowJumpMultiplier = 2f;


    // Start is called before the first frame update
    void Start()
    {
        current_patrol_timer = 0;
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EngageMovement();
    }


    private void EngageMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(direction * 0.75f, 0.0f, 0.0f);
        RaycastHit check_down;

        if (!Physics.Raycast(next_position, Vector2.down * 2.0f, out check_down, 1.0f))
        {
            direction = -direction;
        }
        else
        {
            this.rigidbody.velocity = new Vector2(speed * direction, this.rigidbody.velocity.y);
        }

        Debug.DrawRay(next_position, Vector2.down, Color.green);
    }
}
