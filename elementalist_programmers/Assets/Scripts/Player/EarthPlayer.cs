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
    public float rock_speed = 1;
    public bool straight_shot = false;


    private List<GameObject> rock_list;
    private float current_respawn_rate;
    private bool currently_attacking;

    public override void Awake()
    {
        base.Awake();

        rock_list = new List<GameObject>();

        current_respawn_rate = 0.0f;
        currently_attacking = false;
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
    }

    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        currently_attacking = false;
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

    
    public void OnSpecial(InputValue value)
    {
        if (!death_status)
        {
            if(rock_list.Count > 0 && !currently_attacking)
            {
                Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * rock_speed;
                GameObject rock = rock_list[0];
                float highest = rock.transform.position.y;

                foreach(GameObject rock_check in rock_list)
                {
                    if(rock_check.transform.position.y >= highest)
                    {
                        rock = rock_check;
                    }
                }

                if(left_stick_position.x != 0.0f || left_stick_position.y != 0.0f)
                {
                    rock_list.Remove(rock);
                    rock.transform.parent = null;

                    rock.GetComponent<Rigidbody>().velocity = left_stick_position;
                    rock.GetComponent<Projectile>().Release();
                    currently_attacking = true;
                }
            }
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
