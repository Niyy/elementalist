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
                audioSource.clip = electric_dash;
                break;
            default:
                audioSource.clip = null;
                break;
        }
        base.playAudio(sound);
    }
}
