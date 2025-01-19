using System;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool loop;
    [SerializeField] private float volume = 1f;
    public void Start()
    {
        SoundManager.Instance.PlayMusic(audioClip, volume, transform, loop);
    }
}
