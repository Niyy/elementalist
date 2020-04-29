using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    jump, walljump, dash, special, land, dig, rockform, rockbreak, underground, hover, death, revive
}

public class PlayerAudio : MonoBehaviour
{
    [HideInInspector]
    public AudioSource[] audioSource;
    public AudioClip jump, walljump, dash, land;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponents<AudioSource>();
    }

    public virtual void playAudio(SoundType sound)
    {
        
        switch (sound)
        {
            case SoundType.jump:
                audioSource[0].clip = jump;
                audioSource[0].Play();
                break;
            case SoundType.walljump:
                audioSource[0].clip = walljump;
                audioSource[0].Play();
                break;
            case SoundType.dash:
                audioSource[0].clip = dash;
                audioSource[0].Play();
                break;
            case SoundType.land:
                audioSource[0].clip = land;
                audioSource[0].Play();
                break;
            default:
                break;
        }
        
    }


}
