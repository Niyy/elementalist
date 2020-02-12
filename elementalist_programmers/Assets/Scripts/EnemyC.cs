using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{

   
    private Rigidbody rb;
    private Vector2 movement;
    

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        WhoAmIKilling();
    }

    private void Update()
    {
        death();
        Vector3 direction = target.transform.position - transform.position;
        
        direction.Normalize();
        movement = direction;
    }
    private void FixedUpdate()
    {
        moveChar(movement);
    }

    void moveChar(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }



}
