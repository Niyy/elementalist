using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlayer : PlayerController
{
    [Header("Wynn Variables")]
    [Range(0, 180)]
    public float attack_angle;


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
        AirAttack();
    }


    private void AirAttack()
    {
        float theta = Mathf.Atan2(retical.transform.position.y - this.transform.position.y, 
                                    retical.transform.position.x - this.transform.position.x);

        float x = attack_range * Mathf.Cos(theta + attack_angle);
        float y = attack_range * Mathf.Sin(theta + attack_angle);
        Vector3 draw_angle = this.transform.position + new Vector3(x, y, 0.0f);
        Debug.DrawLine(this.transform.position, draw_angle);

        x = attack_range * Mathf.Cos(theta - attack_angle);
        y = attack_range * Mathf.Sin(theta - attack_angle);
        draw_angle = this.transform.position + new Vector3(x, y, 0.0f);
        Debug.DrawLine(this.transform.position, draw_angle);

        Debug.DrawLine(this.transform.position, retical.transform.position, Color.green);
    }
}
