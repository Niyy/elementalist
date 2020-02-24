using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> players;

    private void Start()
    {
        players = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().GetPlayerList();
        float i = 0f;
        foreach (GameObject player in players)
        {
            player.transform.GetChild(1).gameObject.SetActive(true);
            player.transform.GetChild(1).transform.position = transform.localPosition + new Vector3(i, 0f);
            //Instantiate(player, transform.localPosition + new Vector3(i,0f), transform.localRotation);
            i+=1.25f;
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 size = new Vector3(1f, 2f);
        Gizmos.DrawSphere(transform.position, 1);
        //Gizmos.DrawIcon(transform.position, "sv_icon_dot10_pix16_gizmo");
    }
    */
}
