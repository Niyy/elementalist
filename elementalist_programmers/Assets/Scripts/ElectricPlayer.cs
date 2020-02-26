using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPlayer : PlayerController
{
    // Update is called once per frame
    bool st_running = false;
    public override void FixedUpdate()
    {
        if (rigbod.velocity == new Vector3(0f,0f))
        {
            if (!st_running) StartCoroutine("StopTimer");
        }
        else
        {
            if (st_running)
            {
                StopCoroutine("StopTimer");
                st_running = false;
            }
            if(moveSpeed < 20f) moveSpeed += 0.02f;
        }
        base.FixedUpdate();
    }

    public IEnumerator StopTimer()
    {
        st_running = true;
        yield return new WaitForSeconds(0.2f);
        moveSpeed = 10f;
        print("leaving coroutine");
    }
}
