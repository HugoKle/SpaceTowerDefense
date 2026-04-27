using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioMixer audioMixer;

    public bool continued = false;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Load settings from PlayerPrefs
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bool isMuted = PlayerPrefs.GetInt("Mute", 0) == 1;
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1;
        float flySpeed = PlayerPrefs.GetFloat("FlySpeed", 5f);
        bool isGodMode = PlayerPrefs.GetInt("GodMode", 0) == 1;
        bool noClip = PlayerPrefs.GetInt("NoClip", 0) == 1;
        bool levelSkip = PlayerPrefs.GetInt("LevelSkip", 0) == 1;

        // Apply loaded settings
        audioMixer.SetFloat("MasterVolume", masterVolume);
        musicAudioSource.volume = musicVolume;
        sfxAudioSource.volume = sfxVolume;
        AudioListener.pause = isMuted;
        Screen.fullScreen = isFullscreen;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != (int)Level.Start)
        {
            PlayerPrefs.SetInt("Level", scene.buildIndex);
        }
        // Re-apply settings when a new scene is loaded
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);    
        bool isMuted = PlayerPrefs.GetInt("Mute", 0) == 1;
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1;



        audioMixer.SetFloat("MasterVolume", masterVolume);
        musicAudioSource.volume = Mathf.Pow(10f, musicVolume / 20f); ;
        sfxAudioSource.volume = Mathf.Pow(10f, sfxVolume / 20f); ;
        AudioListener.pause = isMuted;
        Screen.fullScreen = isFullscreen;
    }

   
}
