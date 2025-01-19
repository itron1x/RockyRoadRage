using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance => _instance;
    [SerializeField] private AudioSource soundFXSourcePrefab;
    [SerializeField] private AudioSource musicPrefab;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(gameObject);
        
    }

    public void PlaySoundFX(AudioClip clip, float volume, Transform spawnTransform) 
    {
        AudioSource audioSource = Instantiate(soundFXSourcePrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, clip.length);
        
    }

    public void PlayMusic(AudioClip clip, float volume, Transform spawnTransform, bool loop = true)
    {
        AudioSource audioSource = Instantiate(musicPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        audioSource.loop = loop;
        if(!loop) Destroy(audioSource.gameObject, clip.length);
    }
    
}