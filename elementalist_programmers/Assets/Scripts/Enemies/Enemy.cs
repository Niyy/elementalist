using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Variables")]
    public float speed = 0f;
    // health but its one hit kill so just a true or false
    public bool alive = true;
    //allows the Level desginer to set if they want the enemy to target one indvidual player or not
    public bool findTarget = false;
    //Who am I trying to kill
    [HideInInspector] public GameObject target;
    //Enemys speed
    public RoomManager myRoom;
    //if enemy is currently frozen
    protected bool frozen = false;
    public float melt_time = 30f;
    public Material ice_material;







    // Start is called before the first frame update
    void Start()
    {
        WhoAmIKilling();
    }

    // Update is called once per frame
    void Update()
    {
        death();
    }
   

    public void Freeze()
    {
        frozen = true;
        GameObject icecube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        icecube.transform.position = transform.position;
        icecube.transform.parent = transform.parent;
        if(GetComponent<Renderer>() != null)
        {
            icecube.transform.localScale = GetComponent<Renderer>().bounds.size;
        }
        else
        {
            icecube.transform.localScale = transform.gameObject.GetComponentInChildren<Renderer>().bounds.size;
        }
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
        icecube.GetComponent<Renderer>().material = ice_material;
        alive = false;
        Rigidbody ice_rb = icecube.AddComponent<Rigidbody>();
        ice_rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        transform.parent = icecube.transform;
        //icecube.GetComponent<Renderer>().material.color = new Color(130f/255f, 245f/255f, 207f/255f, 60f/255f);
        StartCoroutine(Melt());
    }

    private IEnumerator Melt()
    {
        print("Melt in " + melt_time);
        yield return new WaitForSeconds(melt_time);
        Destroy(transform.parent.gameObject);
        print("melt");
    }

    public void WhoAmIKilling()
    {
        if (findTarget == true)
        {
            int amountOfPlayers = PlayerManager.Instance.playerList.Count;
            int myTarget = Random.Range(0, amountOfPlayers);
            print(myTarget);
            target = PlayerManager.Instance.playerList[myTarget];
            print(target);
        }
    }

    public void death()
    {
        if (alive == false)
        {
            if(myRoom.enemys.Contains(this.gameObject))
            {
                print("Found Gameobject");
            }
            else
            {
                print("Didnt find");

            }
            myRoom.enemys.Remove(this.gameObject);
            if (!frozen)
            {
                Debug.Log("I should have died.");
                Destroy(this.gameObject);
            }
        }
    }
}
