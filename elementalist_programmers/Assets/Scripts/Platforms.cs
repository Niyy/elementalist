using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{


    public Platform current_Plat;

    public enum Platform
    {
        SandTrap,
        Crumbling,
        Phasing
    };

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (current_Plat)
            {
                case Platform.SandTrap:
                    SandTrap(other.GetComponent<PlayerController>());
                    print("traped");
                    break;
                case Platform.Crumbling:
                    break;
                case Platform.Phasing:
                    break;

            }

        }
    }



    void SandTrap(PlayerController me)
    {
        
    }

}
