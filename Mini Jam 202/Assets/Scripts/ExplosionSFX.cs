using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip explosionClip;
    void Start()
    {
        sfxSource.clip = explosionClip;
        sfxSource.loop = false;
        sfxSource.pitch = 1;
        sfxSource.pitch += Random.Range(-0.25f, 0.25f);
        sfxSource.Play();

        
    }


}
