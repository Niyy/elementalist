﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject retical;
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;


    private int retical_radius = 3;
    private int direction;
    private Rigidbody rigbod;


    // Start is called before the first frame update
    void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        ReticleMovement();

        Debugger();
    }


    private void Debugger()
    {
        Vector3 left_stick = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        Vector3 position = this.transform.position;
        position.z = -9;

        Debug.Log("X " + Input.GetAxis("Horizontal") + "Y " + Input.GetAxis("Vertical"));
        Debug.DrawRay(position, left_stick * 5, Color.red);
    }


    private void ReticleMovement()
    {
        Vector3 left_stick_position = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * retical_radius;
        left_stick_position += this.transform.position;

        retical.transform.position = left_stick_position;
    }


    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, y, 0f);
        rigbod.velocity = (new Vector3(direction.x * moveSpeed, rigbod.velocity.y));
    }


    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
        }
        if (rigbod.velocity.y < 0)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigbod.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}

