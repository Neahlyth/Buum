using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip steep;
    public AudioClip explosion;
    public AudioClip risingCheckP;


    public void SoundSteep ()
    {
        audioSource.clip = steep;
        audioSource.Play();
    }

    public void SoundExplosion ()
    {
        audioSource.clip = explosion;
        audioSource.Play();
    }
    public void SoundRisingCheckPoint()
    {
        audioSource.clip = risingCheckP;
        audioSource.Play();
    }
}
