using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] List<AudioClip> clips;

    public void PlaySound(int clipNumber)
    {
        audioSources[0].clip = clips[clipNumber];
        audioSources[0].Play();
    }
}
