﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class EarthPlayer : PlayerController
{
    [Header("Engrim's Powers")]
    public GameObject rock_prefab;
    public int max_rocks = 4;
    public float rock_respawn_rate = 3.0f;
    public float rock_speed = 1;
    public bool straight_shot = false;
    public Vector2 launch_vector;


    private List<GameObject> rock_list;
    private float current_respawn_rate;
    private bool currently_attacking;

    BoxCollider[] player_collider;
    
    bool surface_blocked = false;
    Renderer player_renderer;
    GameObject dirt;
    float collider_height;
    EarthAudio earthAudio;


    public GameObject twoD_Burried;


    //Animation clip
    private float arise_clip_current;
    private float arise_clip_length;


    protected override void Awake()
    {
        base.Awake();

        rock_list = new List<GameObject>();

        current_respawn_rate = 0.0f;
        currently_attacking = false;
        player_collider = GetComponents<BoxCollider>();
        player_renderer = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        
        currently_attacking = false;
        collider_height = player_collider[0].size.y * transform.localScale.y;
        earthAudio = GetComponent<EarthAudio>();

        twoD_Burried.SetActive(false);


    }


    protected override void Update()
    {
        base.Update();

        if(!death_status)
        {
            RespawnRocks();
            CheckDigAnimationDone();
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
        if(!grounded && is_secondary_moving)
        {
            Unbury();
        }
        currently_attacking = false;
    }


    private void RespawnRocks()
    {
        if(rock_list.Count < max_rocks)
        {
            if(current_respawn_rate >= rock_respawn_rate)
            {
                GameObject new_rock = Instantiate(rock_prefab, this.transform);
                float start_radius = 0;

                if(rock_list.Count > 0)
                {
                    try
                    {
                        start_radius = rock_list[rock_list.Count - 1].GetComponent<Projectile>().GetAngle() - (360 / max_rocks);
                    }
                    catch(ArgumentOutOfRangeException e)
                    {
                        rock_list.Clear();
                        Debug.Log("I threw a complaint."); // Don't delete this until Austin says.
                    }
                }

                new_rock.GetComponent<Projectile>().SetPlayerPosition(this.gameObject);
                new_rock.GetComponent<Projectile>().SetStartAngle(start_radius);
                rock_list.Add(new_rock);
                current_respawn_rate = 0;
            }
            else
            {
                current_respawn_rate += Time.deltaTime;
            }
        }
    }


    private Vector2 FindThrowVector(GameObject rock)
    {
        if(launch_vector != Vector2.zero && grounded)
        {
            return new Vector2(launch_vector.x * facing, launch_vector.y);
        }

        return Vector2.zero;
    }

    
    public void OnSpecial(InputValue value)
    {
        if (!death_status)
        {
            if(rock_list.Count > 0 && !currently_attacking)
            {
                Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * rock_speed;
                Vector3 throw_vector;
                GameObject rock = rock_list[0];
                float highest = rock.transform.position.y;
                int index = 0;

                if(left_stick_position.x != 0.0f || left_stick_position.y != 0.0f)
                {
                    foreach(GameObject rock_check in rock_list)
                    {
                        if(rock_check != null && rock_check.transform.position.y >= highest)
                        {
                            rock = rock_check;
                        }
                    }
                    index = rock_list.IndexOf(rock);
                    rock_list.Remove(rock);
                    rock.transform.parent = null;
                    throw_vector = FindThrowVector(rock);

                    rock.transform.position = this.transform.position;

                    rock.GetComponent<Rigidbody>().velocity = left_stick_position + throw_vector;
                    rock.GetComponent<Projectile>().Release();
                    currently_attacking = true;
                    earthAudio.playAudio(SoundType.rockform);
                }
            }
        }
    }

    public override void OnDash(InputValue value)
    {
        OnDig(value);
    }

    public void OnDig(InputValue value)
    {
        if (value.isPressed && grounded && current_jump_cool_down >= jump_cool_down)
        {
            Instantiate(dashAnimation, feetPos, Quaternion.identity);
            gameObject.layer = 11;
            is_secondary_moving = true;
            player_collider[0].enabled = false;
            player_collider[1].enabled = true;
            surface_blocked = false;
            earthAudio.playAudio(SoundType.dig);
            current_dash_cool_down = 0;
        }
        else if(is_secondary_moving)
        {
            Unbury();
        }
    }

    // Ned was honourable
    private void CheckDigAnimationDone()
    {
        if(current_dash_cool_down >= dash_cool_down && is_secondary_moving)
            player_renderer.enabled = false;
        else if(current_dash_cool_down <= dash_cool_down)
            current_dash_cool_down += Time.deltaTime;
    }


    private void Unbury()
    {
        if (!player_collision.HeadCollision())
        {

            twoD_Burried.SetActive(false);

            earthAudio.Unbury();

            gameObject.layer = 8;
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

            twoD_Burried.SetActive(true);

            direction = new Vector3(move.x, move.y, 0f);
            if (direction.x != 0)
            {
                facing = Mathf.Sign(direction.x);
            }
            Vector3 next_position = this.transform.position + new Vector3(facing * 0.2f, 0.0f, 0.0f);
            RaycastHit check_down;
            int layer_mask = ~((1 << 10) | (1 << 8) | (1 << 11));
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
        else if (current_jump_cool_down <= jump_cool_down)
        {
            current_jump_cool_down += Time.deltaTime;
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


    protected override void FindAnimationTimes()
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
                case "Earthdash":
                    dash_cool_down = clip.length;
                    break;
                case "Eartharise":
                    arise_clip_length = clip.length;
                    break;
                default:
                    break;
            }
        }
    }


    protected override void ResetAnimationState()
    {
        animator.SetBool("idle_break", false);
        base.ResetAnimationState();
    }
}
