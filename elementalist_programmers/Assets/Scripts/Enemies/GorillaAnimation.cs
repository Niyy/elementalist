using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaAnimation : MonoBehaviour
{

    Animator animator;
    public EnemyB enemyb; 
    public bool defending = false;
    public int direction;
    private Vector3 size;
    private GameObject AnimGorilla;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyb = GetComponent<EnemyB>();
        direction = enemyb.direction;
        AnimGorilla = transform.GetChild(0).transform.gameObject;
        size = AnimGorilla.transform.localScale;
    }

    
    // Update is called once per frame
    void Update()
    {
	    animator.SetBool("Defense", enemyb.defending);
        if (direction != enemyb.direction)
        {
            direction = enemyb.direction;
            if (direction == -1){
            AnimGorilla.transform.localScale = new Vector3(-size.x, size.y, -size.z);
			}
            else {
       AnimGorilla.transform.localScale = size;     
			}
            
		}
    }
}
