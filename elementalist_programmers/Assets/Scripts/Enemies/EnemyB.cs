using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    [Header("Movement Variables")]
    public float offset = 0.75f;
    public float search_arch;
    public float sight_distance;


    private float cur_search_arch;
    private float arch_adder;

    
    [Header("B-Tier Variables")]
    [Range(-1, 1)]
    public int direction;
    public float patrol_wait_timer;


    private float current_patrol_timer;
    private new Rigidbody rigidbody;


    [Header("Attack Variables")]
    public float recoil_speed;
    public bool defending = false;


    // Start is called before the first frame update
    void Start()
    {
        current_patrol_timer = patrol_wait_timer;
        cur_search_arch = 0;
        rigidbody = this.GetComponent<Rigidbody>();

        if(search_arch == 0)
        {
            arch_adder = 0;
        }
        else
        {
            arch_adder = search_arch / 16.0f;
        }
        
        arch_adder = search_arch / 16.0f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!frozen)
        {
            EngageMovement();
            SearchLineOfSight();
        }
        
    }


    private void EngageMovement()
    {
        Vector2 next_position = this.transform.position + new Vector3(direction * offset, 0.0f, 0.0f);
        RaycastHit check_down;

        if(current_patrol_timer < patrol_wait_timer)
        {
            current_patrol_timer += Time.deltaTime;
            this.rigidbody.velocity = Vector3.zero;
            defending = true;
        }
        else
        {
            defending = false;
            if (!Physics.Raycast(next_position, Vector2.down * 2.0f, out check_down, 1.0f))
            {
                direction = -direction;
                current_patrol_timer = 0;
            }
            else
            {
                this.rigidbody.velocity = Vector3.Lerp(new Vector2(speed * direction, this.rigidbody.velocity.y), rigidbody.velocity, Time.deltaTime);
            }

            Debug.Log(check_down.collider.tag);
        }

        Debug.DrawRay(next_position, Vector2.down * 2.0f, Color.yellow);
    }


    private void SearchLineOfSight()
    {
        RaycastHit line_of_sight;
        Vector3 search_point = new Vector3(direction * Mathf.Cos(cur_search_arch * Mathf.Deg2Rad), 
                                            Mathf.Sin(cur_search_arch * Mathf.Deg2Rad), 0.0f);

        if(Physics.Raycast(this.transform.position, search_point, out line_of_sight, sight_distance))
        {
            if(line_of_sight.collider.gameObject.tag.Equals("Player"))
            {
                current_patrol_timer = 0;
                Debug.DrawLine(this.transform.position, line_of_sight.point, Color.red);
            }
            else
            {
                Debug.DrawLine(this.transform.position, line_of_sight.point, Color.green);
                cur_search_arch += arch_adder;
            }
        }
        else
        {
            Debug.DrawRay(this.transform.position, search_point * sight_distance, Color.yellow);
            cur_search_arch += arch_adder;
        }


        if(cur_search_arch > search_arch || cur_search_arch <= -search_arch)
        {
            arch_adder = -arch_adder;
        }
    }


    private void OnCollisionEnter(Collision collision) 
    {
        if (!frozen)
        {
            if (collision.collider.gameObject.tag.Equals("Player"))
            {
                GameObject player = collision.gameObject;

                if (Mathf.Sign(direction) == -Mathf.Sign(player.transform.position.x - this.transform.position.x))
                {
                    if (player.GetComponent<PlayerController>().GetAttackStatus() == true)
                    {
                        player.GetComponent<PlayerController>().AttackHit();
                        Hit();
                    }
                    else
                    {
                        player.GetComponent<PlayerController>().PlayerDeath();
                    }

                }
                else
                {
                    player.GetComponent<PlayerController>().Recoil(recoil_speed, direction);
                }
            }
            else
            {
                direction = -direction;
                current_patrol_timer = 0;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!frozen)
        {
            if (other.gameObject.tag.Equals("Freeze"))
            {
                Freeze();
            }

            if (other.gameObject.tag.Equals("Projectile"))
            {
                GameObject projectile = other.gameObject;

                if (Mathf.Sign(direction) == -Mathf.Sign(projectile.transform.position.x - this.transform.position.x))
                {
                    alive = false;
                    death();
                    Debug.Log("hurg");
                }
                Debug.Log("blurg.");
            }
        }
    }


    private void Hit()
    {
        Destroy(this.gameObject);
    }
}
