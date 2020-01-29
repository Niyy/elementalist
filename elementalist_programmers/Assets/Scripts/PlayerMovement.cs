using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject retical;


    private int retical_radius = 5;
    private int direction;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        Debugger();
    }


    private void Debugger()
    {
        Vector3 left_stick = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        Vector3 position = this.transform.position;
        position.z = -9;

        Debug.Log("X " + Input.GetAxis("Horizontal") + "Y " + Input.GetAxis("Vertical"));
        Debug.DrawRay(position, left_stick * 5, Color.red);
    }


    private void Reticle()
    {
        Vector3 left_stick = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        Vector3 position = this.transform.position;


    }
}
