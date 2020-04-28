using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{


    private Rigidbody rb;
    private Vector2 movement;
    private GameObject detector;
    private GameObject rig;
    private Vector3 player_pos;
    //ublic float detectionRadius = 5f;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        detector = transform.GetChild(0).gameObject;
        rig = transform.GetChild(1).gameObject;
        //WhoAmIKilling();
    }

    private void Update()
    {
        death();
        if(target && !frozen)
        {
            Vector3 direction = target.transform.position - transform.position;

            direction.Normalize();
            movement = direction;
        }
      
        AnimationHandler();
    }
    private void FixedUpdate()
    {
        moveChar(movement);
    }

    void moveChar(Vector2 direction)
    {
        //rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        rb.velocity = Vector3.Lerp(rb.velocity, direction * speed, Time.deltaTime);
        player_pos = direction;
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Freeze") && !frozen)
        {
            Freeze();
            movement = Vector3.zero;
        }
        if(collider.tag.Equals("Player") && !frozen)
        {
            if(collider.GetComponent<PlayerController>().GetAttackStatus() == true)
            {
                collider.GetComponent<PlayerController>().AttackHit();
                alive = false;
                death();
            }
            else
            {
                collider.GetComponent<PlayerController>().PlayerDeath();
                detector.GetComponent<LineOfSight>().Retarget();
            }
        }
        
    }


    protected void AnimationHandler()
    {
        float rotation_y = 180;
        float rotation_z = 0;

        if(player_pos != Vector3.zero)
        {
            rotation_z = Mathf.Rad2Deg * Mathf.Atan2(player_pos.y, player_pos.x);

            if(Mathf.Abs(rotation_z) <= 90)
            {
                rotation_y = 90;
                rotation_z = Mathf.Rad2Deg * Mathf.Atan2(-player_pos.y, player_pos.x);
            }
            else if(Mathf.Abs(rotation_z) > 90)
            {
                rotation_y = 270;
                rotation_z = Mathf.Rad2Deg * Mathf.Atan2(-player_pos.y, -player_pos.x);
            }
        }

        rig.transform.rotation = Quaternion.Euler(rotation_z, rotation_y, 0.0f);

    }
}
