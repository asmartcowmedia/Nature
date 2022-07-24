using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private new AudioSource audio;

    [SerializeField] private AudioClip
        combat,
        ambiance;

    private void Start()
    {
        audio.Stop();
        audio.clip = ambiance;
        audio.Play();
    }

    public void TriggerCombat()
    {
        audio.Stop();
        audio.clip = combat;
        audio.Play();
    }

    public void TriggerAmbiance()
    {
        audio.Stop();
        audio.clip = ambiance;
        audio.Play();
    }
}