using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelect : MonoBehaviour
{
    public float radius = 10;
    public GameObject[] Worlds;
    public PlayerControls controls;
    public Vector2 move;
    public int choice;
    float last_move_time;
    float angle;
    public float rotation;
    Quaternion rotation_start;
    bool rotating;
    public float timeDuration=1;
    float time_start;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Select.performed += ctx => SelectWorld();
        controls.Menu.Move.performed += ctx => { move = ctx.ReadValue<Vector2>(); MoveSelection(); };
        controls.Gameplay.Jump.canceled += ctx => move = Vector2.zero;
        if (Worlds.Length!=0)
        angle = 360 / Worlds.Length;
        for (int world = 0; world < Worlds.Length; world++)
        {
            print(angle);
            var q = Quaternion.AngleAxis(angle * world, Vector3.down);
            print(q);
            GameObject wrld=Instantiate(Worlds[world],transform.position + q * Vector3.back * radius, Quaternion.identity);
            wrld.transform.parent = transform;
        }

    }


    void FixedUpdate()
    {
        if (rotating)
        {
            float u = (Time.time - time_start) / timeDuration;
            if(u >= 1)
            {
                u = 1;
                rotating = false;
            }
            transform.rotation = Quaternion.Slerp(rotation_start, Quaternion.Euler(0, rotation, 0), u);
        }
    }

    void MoveSelection()
    {
        float time = Time.unscaledTime;
        if (time < last_move_time + 0.2)
        {
            return;
        }
        last_move_time = time;
        if (move.x > 0f)
        {
            choice++;
            if (choice > Worlds.Length - 1)
            {
                choice = 0;
            }
            rotation += angle;
        }
        else if (move.x < 0f)
        {
            choice--;
            if (choice < 0)
            {
                choice = (Worlds.Length - 1);
            }
            rotation -= angle;
        }
        if (rotation % 360 == 0)
        {
            rotation = 0;
        }
        rotation_start = transform.localRotation;
        time_start = Time.time;
        rotating = true;
    }

    void SelectWorld()
    {
        
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
