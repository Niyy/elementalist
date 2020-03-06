using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> players;

    public float character_height = 2f;
    public float character_width = 1f;
    public float gap = 1.25f;

    private void Start()
    {
        players = PlayerManager.Instance.GetPlayerList();
        float i = 0f;
        foreach (GameObject player in players)
        {
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.transform.GetChild(0).transform.position = transform.localPosition + new Vector3(i, 0f);
            //Instantiate(player, transform.localPosition + new Vector3(i,0f), transform.localRotation);
            i+=gap;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 size = new Vector3(1f, 2f);
        for (float i = 0; i < gap*4; i+=gap)
        {
            Gizmos.DrawCube(transform.position + new Vector3(i, 0f), new Vector3(character_width, character_height, character_width));
        }
        //Gizmos.DrawCube(transform.position, new Vector3(1,2,1));
        //Gizmos.DrawIcon(transform.position, "sv_icon_dot10_pix16_gizmo");
    }
    
}
