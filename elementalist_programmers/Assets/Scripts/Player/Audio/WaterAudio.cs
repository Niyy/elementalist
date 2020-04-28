using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAudio : PlayerAudio
{
    public AudioClip burst;
    public override void playAudio(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.special:
                audioSource[0].clip = burst;
                audioSource[0].Play();
                break;
            default:
                base.playAudio(sound);
                break;
        }
        
    }
}
