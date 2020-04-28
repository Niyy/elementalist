using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    jump, walljump, dash, special, land, dig, rockform, rockbreak, death, revive
}

public class PlayerAudio : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip jump, walljump, dash, land;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public virtual void playAudio(SoundType sound)
    {
        
        switch (sound)
        {
            case SoundType.jump:
                audioSource.clip = jump;
                break;
            case SoundType.walljump:
                audioSource.clip = walljump;
                break;
            case SoundType.dash:
                audioSource.clip = dash;
                break;
            case SoundType.land:
                audioSource.clip = land;
                break;
            default:
                break;
        }
        audioSource.Play();
    }


}
