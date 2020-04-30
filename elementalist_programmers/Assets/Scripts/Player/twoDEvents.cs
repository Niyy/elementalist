using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoDEvents : MonoBehaviour
{
   
    public void Destroy()
    {
        Destroy(this.gameObject);
       
    }

    public void setFalse()
    {
        this.gameObject.SetActive(false);
    }



}
