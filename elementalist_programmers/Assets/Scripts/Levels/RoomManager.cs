using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Door> normal_Doors;

    public List<GameObject> enemys;

    public List<GameObject> switchs;

    private void Start()
    {
        for (int i = 0; i < normal_Doors.Count; i++)
        {
            normal_Doors[i].open = true;
        }



    }

    private void Update()
    {
        openNormalDoors();
    }



    void openNormalDoors()
    {
        if (enemys.Count == 0 && switchs.Count == 0)
        {
            for (int i = 0; i < normal_Doors.Count; i++)
            {
                normal_Doors[i].open = true;
            }
        }

    }


   


}
