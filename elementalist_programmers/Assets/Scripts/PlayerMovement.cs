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


    private float retical_radius = 3f;
    private int direction;
    private Rigidbody rigbod;
    private Camera cam;


    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
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


    private void ReticleMovement()
    {
        RaycastHit hit;
        Vector3 left_stick_position = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * retical_radius;

        if(Physics.Raycast(this.transform.position, left_stick_position, out hit, retical_radius))
        {
            retical.transform.position = hit.point;
        }
        else
        {
            left_stick_position += this.transform.position;
            retical.transform.position = left_stick_position;
        }

        if(left_stick_position != this.transform.position)
        {
            retical.transform.position += Vector3.forward * -9.0f;
        }
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


    private void Debugger()
    {
        RaycastHit hit;
        Vector3 left_stick_position = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * retical_radius;

        Debug.DrawRay(this.transform.position, left_stick_position, Color.yellow);
    }
}

