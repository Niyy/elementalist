using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    jump, walljump, dash, special, land
}

public class PlayerAudio : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip jump, walljump, dash, special, land;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void playAudio(SoundType sound)
    {
        audioSource.clip = null;
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
            case SoundType.special:
                audioSource.clip = special;
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
