﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    [Header("B-Tier Variables")]
    [Range(-1, 1)]
    public int direction;
    public float patrol_wait_timer;


    private float current_patrol_timer;
    private new Rigidbody rigidbody;
    private float lowJumpMultiplier = 2f;


    [Header("Movement Variables")]
    public float offset = 0.75f;
    public float search_arch;
    public float sight_distance;


    private float cur_search_arch;
    private float arch_adder;


    // Start is called before the first frame update
    void Start()
    {
        current_patrol_timer = patrol_wait_timer;
        cur_search_arch = 0;
        rigidbody = this.GetComponent<Rigidbody>();
        arch_adder = search_arch / 16.0f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        EngageMovement();
        SearchLineOfSight();
    }


    private void Update()
    {
       
    }


    private void EngageMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(direction * offset, 0.0f, 0.0f);
        RaycastHit check_down;

        if(current_patrol_timer < patrol_wait_timer)
        {
            current_patrol_timer += Time.deltaTime;
            this.rigidbody.velocity = new Vector2(0.0f, this.rigidbody.velocity.y);
        }
        else
        {
            if (!Physics.Raycast(next_position, Vector2.down * 2.0f, out check_down, 1.0f))
            {
                direction = -direction;
                current_patrol_timer = 0;
            }
            else
            {
                this.rigidbody.velocity = new Vector2(speed * direction, this.rigidbody.velocity.y);
            }
        }

        Debug.DrawRay(next_position, Vector2.down * 2.0f, Color.yellow);
    }


    private void SearchLineOfSight()
    {
        RaycastHit line_of_sight;
        Vector3 search_point = new Vector3(direction * Mathf.Cos(cur_search_arch * Mathf.Deg2Rad), 
                                            Mathf.Sin(cur_search_arch * Mathf.Deg2Rad), 0.0f);

        if(Physics.Raycast(this.transform.position, search_point, out line_of_sight, sight_distance))
        {
            if(line_of_sight.collider.gameObject.tag.Equals("Player"))
            {
                current_patrol_timer = 0;
                Debug.DrawLine(this.transform.position, line_of_sight.point, Color.red);
            }
            else
            {
                Debug.DrawLine(this.transform.position, line_of_sight.point, Color.green);
                cur_search_arch += arch_adder;
            }
        }
        else
        {
            Debug.DrawRay(this.transform.position, search_point * sight_distance, Color.yellow);
            Debug.Log("Current angle: " + cur_search_arch);
            cur_search_arch += arch_adder;
        }


        if(cur_search_arch > search_arch || cur_search_arch <= -search_arch)
        {
            arch_adder = -arch_adder;
        }
    }
}