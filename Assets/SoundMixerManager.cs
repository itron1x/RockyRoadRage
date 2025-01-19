using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioClip soundFXTestClip;
    
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;

    void Start(){
        SaveSystem.LoadVolume();
        float globalVolume = RaceInfoSystem.GetInstance().GlobalVolume;
        float musicVolume = RaceInfoSystem.GetInstance().MusicVolume;
        float fxVolume = RaceInfoSystem.GetInstance().FxVolume;
        SetMasterVolume(globalVolume);
        SetMusicVolume(musicVolume);
        SetSoundFXVolume(fxVolume);
        
        masterSlider.value = RaceInfoSystem.GetInstance().GlobalVolume;
        musicSlider.value = RaceInfoSystem.GetInstance().MusicVolume;
        fxSlider.value = RaceInfoSystem.GetInstance().FxVolume;
    }
    
    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        RaceInfoSystem.GetInstance().GlobalVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        RaceInfoSystem.GetInstance().MusicVolume = volume;
    }

    public void SetSoundFXVolume(float volume)
    {
        mixer.SetFloat("SoundFXVolume", Mathf.Log10(volume) * 20f);
        RaceInfoSystem.GetInstance().FxVolume = volume;
    }

    public void PlayFXTest()
    {
        SoundManager.Instance.PlaySoundFX(soundFXTestClip, 1f, transform);
    }

    public void SaveVolume(){
        SaveSystem.SaveVolume();
    }
}
