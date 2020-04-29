using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAudio : PlayerAudio
{
    public AudioClip attack;
    public override void playAudio(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.special:
                audioSource[1].clip = attack;
                audioSource[1].Play();
                break;
            default:
                audioSource[0].clip = null;
                base.playAudio(sound);
                break;
        }
    }
}
