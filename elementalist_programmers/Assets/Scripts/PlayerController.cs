using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    protected Vector2 move;
    private Rigidbody rigbod;
    protected float moveSpeed = 10f;

    public bool jump = false;
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public GameObject retical;
    private float retical_radius = 2.5f;

    private void Awake()
    {
        rigbod = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Jump.canceled += ctx => jump = false;
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    void Update()
    {
        Move();
        Airborn();
        ReticleMovement();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(move.x, move.y, 0f);
        rigbod.velocity = (new Vector3(direction.x * moveSpeed, rigbod.velocity.y));
    }

    private void Jump()
    {
        jump = true;
        rigbod.velocity = (new Vector3(rigbod.velocity.x, jumpForce));
    }

    private void Airborn()
    {
        if (rigbod.velocity.y < 0)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigbod.velocity.y > 0 && !jump)
        {
            rigbod.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ReticleMovement()
    {
        RaycastHit hit;
        Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * retical_radius;

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

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
