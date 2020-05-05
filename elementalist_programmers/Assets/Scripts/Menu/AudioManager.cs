using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    public float musicVol = 1;
    public float sfxVol = 1;


    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetMusicVolume(float slider)
    {
        musicVol = slider;

        musicMixer.SetFloat("MusicVol", Mathf.Log10(slider) * 20);
    }

    public void SetSfxVolume(float slider)
    {
        sfxVol = slider;
        sfxMixer.SetFloat("SfxVol", Mathf.Log10(slider) * 20);
    }
}
