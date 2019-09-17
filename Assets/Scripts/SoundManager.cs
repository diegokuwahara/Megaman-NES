using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource megamanSounds;
    public AudioSource shootingSounds;
    public AudioSource musicSource;

    public static SoundManager instance = null;

    public enum ESource
    {
        Enemy,
        Megaman,
        Shooting
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlaySound(ESource source, AudioClip clip)
    {
        switch (source)
        {
            case ESource.Megaman:
                megamanSounds.clip = clip;
                megamanSounds.Play();
                break;
            case ESource.Shooting:
                shootingSounds.clip = clip;
                shootingSounds.Play();
                break;
            default:
                break;

        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
