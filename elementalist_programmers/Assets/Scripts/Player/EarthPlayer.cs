﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EarthPlayer : PlayerController
{
    [Header("Engrim's Powers")]
    public GameObject rock_prefab;
    public int max_rocks = 4;
    public float rock_respawn_rate = 3.0f;


    private List<GameObject> rock_list;
    private float current_respawn_rate;

    BoxCollider[] player_collider;
    PlayerCollision playerCollision;
    bool surface_blocked = false;
    Renderer player_renderer;
    GameObject dirt;
    float collider_height;


    public override void Awake()
    {
        base.Awake();

        rock_list = new List<GameObject>();

        current_respawn_rate = 0.0f;
        
        player_collider = GetComponents<BoxCollider>();
        playerCollision = GetComponent<PlayerCollision>();
        player_renderer = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        collider_height = player_collider[0].size.y * transform.localScale.y;
    }

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

        
        if(!death_status)
        {
            RespawnRocks();
        }
        else if(rock_list.Count > 0)
        {
            foreach(GameObject rock in rock_list)
            {
                rock_list.Remove(rock);
                Destroy(rock);
            }
        }
        Vector3 nxt_position = this.transform.position + new Vector3(facing * 0.2f, 0.0f, 0.0f);
        Debug.DrawRay(nxt_position, Vector3.down * (collider_height / 2f + 0.1f), Color.blue);
    }

    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!grounded && is_secondary_moving)
        {
            Unbury();
        }
    }


    private void RespawnRocks()
    {
        if(rock_list.Count < max_rocks)
        {
            if(current_respawn_rate >= rock_respawn_rate)
            {
                GameObject new_rock = Instantiate(rock_prefab, this.transform);
                float start_radius;

                if(rock_list.Count > 0)
                {
                    start_radius = rock_list[rock_list.Count - 1].GetComponent<Projectile>().GetAngle() - (360 / max_rocks);
                }
                else
                {
                    start_radius = 0;
                }

                new_rock.GetComponent<Projectile>().SetPlayerPosition(this.gameObject);
                new_rock.GetComponent<Projectile>().SetStartAngle(start_radius);
                new_rock.transform.position = new Vector3(new_rock.transform.position.x, 
                                                            new_rock.transform.position.y, 0.0f);
                rock_list.Add(new_rock);
                current_respawn_rate = 0;
            }
            else
            {
                current_respawn_rate += Time.deltaTime;
            }
        }
    }

    
    private void Attack()
    {

    }

    public override void OnDash(InputValue value)
    {
        OnDig(value);
    }

    public void OnDig(InputValue value)
    {
        if (value.isPressed && grounded)
        {
            is_secondary_moving = true;
            player_collider[0].enabled = false;
            player_collider[1].enabled = true;
            surface_blocked = false;
            player_renderer.enabled = false;
        }
        else if(is_secondary_moving)
        {
            Unbury();
        }
    }

    private void Unbury()
    {
        if (!playerCollision.HeadCollision())
        {
            print("unbury");
            is_secondary_moving = false;
            player_collider[1].enabled = false;
            player_collider[0].enabled = true;
            player_renderer.enabled = true;
        }
        else
        {
            print("surface_blocked");
            surface_blocked = true;
        }
        
    }

    public override void EngageSecondaryMovement()
    {
        if (is_secondary_moving && !surface_blocked)
        {
            direction = new Vector3(move.x, move.y, 0f);
            if (direction.x != 0)
            {
                facing = Mathf.Sign(direction.x);
            }
            Vector3 next_position = this.transform.position + new Vector3(facing * 0.2f, 0.0f, 0.0f);
            RaycastHit check_down;
            int layer_mask = 1 << 8;
            layer_mask = ~layer_mask;
            if (Physics.Raycast(next_position, Vector3.down, out check_down, collider_height/2f+.01f,layer_mask))
            {
                print("ground");
                rigbod.velocity = (new Vector3(direction.x * moveSpeed, rigbod.velocity.y));
            }
            else
            {
                print("no ground");
                rigbod.velocity = Vector3.zero;
            }
            
        }
        else if(is_secondary_moving && surface_blocked)
        {
            Unbury();
        }
    }


    public override void PlayerDeath(float death_time = 0)
    {
        if(rock_list.Count <= 0)
        {
            base.PlayerDeath();
        }
        else
        {
            GameObject rock = rock_list[0];
            rock_list.RemoveAt(0);
            current_respawn_rate = 0;

            Destroy(rock);
        }
    }
}
