using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirPlayer : PlayerController
{
    [Header("Wynn Variables")]
    [Range(0, 180)]
    public float attack_angle;
    public float shot_speed;
    public GameObject air_shot_prefab;


    private float attack_range;
    Vector3 draw_angle_top;
    Vector3 draw_angle_bottom;
    Vector3 draw_angle_mid;


    protected override void Awake()
    {
        base.Awake();
        attack_range = retical_radius;
    }


    protected override void Start()
    {
        base.Start();

        if(attack_angle != 0)
        {
            attack_angle = Mathf.Deg2Rad * (attack_angle / 2);
        }
    }


    protected override void Update() 
    {
        base.Update();
        Debugger();
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
        float x_mid = attack_range * Mathf.Cos(theta);
        float y_mid = attack_range * Mathf.Sin(theta);
    
        draw_angle_top = this.transform.position + new Vector3(x_top, y_top, 0.0f);
        draw_angle_bottom = this.transform.position + new Vector3(x_bottom, y_bottom, 0.0f);
        draw_angle_mid = this.transform.position + new Vector3(x_mid, y_mid, 0.0f);

        List<GameObject> shots = new List<GameObject>();
        shots.Add(Instantiate(air_shot_prefab, draw_angle_top / 2, Quaternion.Euler(0.0f, 0.0f, angle_one * Mathf.Rad2Deg)));
        shots.Add(Instantiate(air_shot_prefab, draw_angle_bottom / 2, Quaternion.Euler(0.0f, 0.0f, angle_two * Mathf.Rad2Deg)));
        shots.Add(Instantiate(air_shot_prefab, draw_angle_mid / 2, Quaternion.Euler(0.0f, 0.0f, theta * Mathf.Rad2Deg)));

        // shots[0].GetComponent<Projectile>().SetStartAngle(angle_one);
        // shots[1].GetComponent<Projectile>().SetStartAngle(angle_two);

        shots[0].GetComponent<Projectile>().SetNoVelocity(true, new Vector3(x_top, y_top, 0.0f));
        shots[1].GetComponent<Projectile>().SetNoVelocity(true, new Vector3(x_bottom, y_bottom, 0.0f));
        shots[2].GetComponent<Projectile>().SetNoVelocity(true, new Vector3(x_mid, y_mid, 0.0f));

        shots[0].GetComponent<Projectile>().SetPlayerPosition(this.gameObject);
        shots[1].GetComponent<Projectile>().SetPlayerPosition(this.gameObject);
    }


    private void Debugger()
    {
        if(debugging)
        {
            float theta = Mathf.Atan2(retical.transform.position.y - this.transform.position.y, 
                                    retical.transform.position.x - this.transform.position.x);
            float angle_one = theta + attack_angle;
            float angle_two = theta - attack_angle;

            float x_top = attack_range * Mathf.Cos(angle_one);
            float y_top = attack_range * Mathf.Sin(angle_one);
            float x_bottom = attack_range * Mathf.Cos(angle_two);
            float y_bottom = attack_range * Mathf.Sin(angle_two);
        
            draw_angle_top = this.transform.position + new Vector3(x_top, y_top, 0.0f);
            draw_angle_bottom = this.transform.position + new Vector3(x_bottom, y_bottom, 0.0f);

            Debug.DrawLine(this.transform.position, draw_angle_top);
            Debug.DrawLine(this.transform.position, draw_angle_bottom);

            Debug.DrawLine(this.transform.position, retical.transform.position, Color.green);
        }
    }
}
