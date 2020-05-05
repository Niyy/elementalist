using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider SfxSlider;

    void Start()
    {
        MusicSlider.value = AudioManager.Instance.musicVol;
        SfxSlider.value = AudioManager.Instance.sfxVol;
    }
}
