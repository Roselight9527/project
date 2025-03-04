using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFxDefination : MonoBehaviour
{
    public PlayAudioEventSO playAudioEvent;
    public AudioClip audioClip;
    private Enemy enemy ;
    public bool playOnEnable;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void OnEnable()
    {
        enemy.onDamageTaken += PlayHurtSound;
    }
    private void OnDisable()
    {
        enemy.onDamageTaken -= PlayHurtSound;
    }

    private void PlayHurtSound()
    {
        PlayAudioClip();
    }

    public void PlayAudioClip()
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}
