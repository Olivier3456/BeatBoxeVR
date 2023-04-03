using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource multiplierAudioSource;
    [SerializeField] private AudioSource perfectAudioSource;


        


    public void PlayMultiplierSoundWithCustomPitch(int multiplier)
    {
        multiplierAudioSource.pitch = 0.5f + (multiplier * 0.15f);
        multiplierAudioSource.Play();
    }


    public void PlayPerfectShotSound()
    {
        perfectAudioSource.Play();
    }
}
