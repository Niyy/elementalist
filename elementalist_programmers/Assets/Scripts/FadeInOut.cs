using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public Animator anim;

    public bool fadddddde = false;

    public int transitionTime = 2;

    // Update is called once per frame
    void Update()
    {
        
    }

   public void loadLevel()
    {
        anim.SetTrigger("FadeOut");
    }

    public IEnumerator transistion()
    {
        loadLevel();

        yield return new WaitForSeconds(transitionTime);
        
        anim.SetTrigger("ReFade");
       
    }

}
