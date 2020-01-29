using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigbod;
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();   
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

