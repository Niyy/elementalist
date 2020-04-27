using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAudio : PlayerAudio
{
    public AudioClip rock_form;
    public override void playAudio(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.rockform:
                audioSource.clip = rock_form;
                break;
            default:
                audioSource.clip = null;
                break;
        }
        base.playAudio(sound);
    }
}
