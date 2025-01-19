using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioClip soundFXTestClip;
    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSoundFXVolume(float volume)
    {
        mixer.SetFloat("SoundFXVolume", Mathf.Log10(volume) * 20f);
    }

    public void PlayFXTest()
    {
        SoundManager.Instance.PlaySoundFX(soundFXTestClip, 1f, transform);
    }
}
