using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAudio : PlayerAudio
{
    public AudioClip burst, hover;
    public override void playAudio(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.special:
                audioSource[0].clip = burst;
                audioSource[0].Play();
                break;
            case SoundType.hover:
                audioSource[1].clip = hover;
                audioSource[1].loop = true;
                audioSource[1].Play();
                break;
            default:
                base.playAudio(sound);
                break;
        }
        
    }

    public void stopHover()
    {
        audioSource[1].Stop();
        audioSource[1].loop = false;
    }
}
