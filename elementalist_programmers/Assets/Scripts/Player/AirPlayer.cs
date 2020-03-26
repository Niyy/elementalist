using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirPlayer : PlayerController
{
    [Header("Wynn Variables")]
    [Range(0, 180)]
    public float attack_angle;
    public GameObject air_shot_prefab;


    private float attack_range;


    protected override void Awake()
    {
        base.Awake();
        attack_range = retical_radius;
    }


    protected override void Start()
    {
        if(attack_angle != 0)
        {
            attack_angle = Mathf.Deg2Rad * (attack_angle / 2);
        }

        base.Start();
    }


    protected override void Update() 
    {
        base.Update();
    }


    public void OnSpecial(InputValue value)
    {
        AirAttack();
    }


    private void AirAttack()
    {
        float theta = Mathf.Atan2(retical.transform.position.y - this.transform.position.y, 
                                    retical.transform.position.x - this.transform.position.x);
        float angle_one = theta + attack_angle;
        float angle_two = theta - attack_angle;

        float x_top = attack_range * Mathf.Cos(angle_one);
        float y_top = attack_range * Mathf.Sin(angle_one);
        float x_bottom = attack_range * Mathf.Cos(angle_two);
        float y_bottom = attack_range * Mathf.Sin(angle_two);
    
        Vector3 draw_angle_top = this.transform.position + new Vector3(x_top, y_top, 0.0f);
        Vector3 draw_angle_bottom = this.transform.position + new Vector3(x_bottom, y_bottom, 0.0f);

        if(debugging)
        {
            Debug.DrawLine(this.transform.position, draw_angle_top);
            Debug.DrawLine(this.transform.position, draw_angle_bottom);

            Debug.DrawLine(this.transform.position, retical.transform.position, Color.green);
        }

        List<GameObject> shots = new List<GameObject>();
        shots.Add(Instantiate(air_shot_prefab, this.transform.position, Quaternion.Euler(0.0f, 0.0f, angle_one)));
        shots.Add(Instantiate(air_shot_prefab, this.transform.position, Quaternion.Euler(0.0f, 0.0f, angle_two)));
    }
}
