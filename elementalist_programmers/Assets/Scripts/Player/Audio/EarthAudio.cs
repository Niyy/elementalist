using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAudio : PlayerAudio
{
    public AudioClip rock_form, dig, underground_movement;
    public override void playAudio(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.rockform:
                audioSource[1].clip = rock_form;
                audioSource[1].Play();
                break;
            case SoundType.dig:
                audioSource[0].clip = dig;
                audioSource[0].Play();
                playAudio(SoundType.underground);
                break;
            case SoundType.underground:
                audioSource[1].clip = underground_movement;
                audioSource[1].loop = true;
                audioSource[1].Play();
                break;
            default:
                audioSource[0].clip = null;
                break;
        }
        base.playAudio(sound);
    }

    public void Unbury()
    {
        
        audioSource[1].Stop();
        audioSource[1].loop = false;
    }
}
