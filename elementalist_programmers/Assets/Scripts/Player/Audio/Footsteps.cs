using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] steps;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayStep()
    {
        AudioClip stepClip = steps[Random.Range(0, steps.Length)];
        audioSource.clip = stepClip;
        audioSource.Play();
    }
}
