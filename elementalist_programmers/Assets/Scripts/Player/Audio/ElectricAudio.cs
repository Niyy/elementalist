using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAudio : PlayerAudio
{
    public AudioClip electric_dash;
    public override void playAudio(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.special:
                audioSource[0].clip = electric_dash;
                audioSource[0].Play();
                break;
            default:
                audioSource[0].clip = null;
                break;
        }
        base.playAudio(sound);
    }
}
