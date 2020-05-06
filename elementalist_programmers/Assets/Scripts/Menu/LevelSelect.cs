using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public float radius = 10;
    public GameObject[] Worlds;
    private List<LevelProperties> levelProperties;
    public PlayerControls controls;
    public Vector2 move;
    private int choice = 0;
    float last_move_time;
    float angle;
    public float rotation;
    Quaternion rotation_start;
    bool rotating;
    public float timeDuration=1;
    float time_start;
    public Text world_name_ui;
    levelsCompleated inst = new levelsCompleated();

    void Awake()
    {
        if(SaveSystem.load() == null)
        {
            inst = new levelsCompleated();
        }
        else
        {
            inst = SaveSystem.load();
        }



        levelProperties = new List<LevelProperties>();
        controls = new PlayerControls();
        controls.Menu.Select.performed += ctx => SelectWorld();
        controls.Menu.Move.performed += ctx => { move = ctx.ReadValue<Vector2>(); MoveSelection(); };
        controls.Menu.Move.canceled += ctx => move = Vector2.zero;
        if (Worlds.Length!=0)
        angle = 360 / Worlds.Length;
        for (int world = 0; world < Worlds.Length; world++)
        {
            print(angle);
            var q = Quaternion.AngleAxis(angle * world, Vector3.down);
            print(q);
            GameObject wrld=Instantiate(Worlds[world],transform.position + q * Vector3.back * radius, Quaternion.identity);
            wrld.transform.parent = transform;
            wrld.GetComponent<LevelProperties>().unlocked = inst.unlockedArray[world];
            levelProperties.Add(wrld.GetComponent<LevelProperties>()); 
        }
        world_name_ui.text = levelProperties[choice].world_name;
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
            world_name_ui.text = levelProperties[choice].world_name;
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
        if (levelProperties[choice].unlocked)
        {
            SceneManager.LoadScene(levelProperties[choice].scene);
        }
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
