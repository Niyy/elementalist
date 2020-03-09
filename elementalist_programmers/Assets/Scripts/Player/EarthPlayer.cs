using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlayer : PlayerController
{
    [Header("Engrim's Powers")]
    public GameObject rock_prefab;
    public int max_rocks = 4;
    public float rock_respawn_rate = 3.0f;


    private List<GameObject> rock_list;
    private float current_respawn_rate;

    protected override void Awake()
    {
        base.Awake();

        rock_list = new List<GameObject>();

        current_respawn_rate = 0.0f;
    }

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();
    }

    
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        RespawnRocks();
    }


    private void RespawnRocks()
    {
        if(rock_list.Count < 4)
        {
            if(current_respawn_rate >= rock_respawn_rate)
            {
                GameObject new_rock = Instantiate(rock_prefab, this.transform);
                new_rock.GetComponent<Projectile>().SetPlayerPosition(this.gameObject);
                new_rock.transform.position = new Vector3(new_rock.transform.position.x, 
                                                            new_rock.transform.position.y, -5.0f);
                rock_list.Add(new_rock);
                current_respawn_rate = 0;
            }
            else
            {
                current_respawn_rate += Time.deltaTime;
            }
        }
    }
}
