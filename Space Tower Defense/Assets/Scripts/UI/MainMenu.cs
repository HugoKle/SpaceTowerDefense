using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    


    private Button exitButton;
    private Button settingsButton;

    private VisualElement settingsMenu;
    private VisualElement mainMenu;

    [SerializeField] Level intro;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] LevelExit levelExit;




    private UIDocument _document;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {



        VisualElement root = _document.rootVisualElement;
        settingsMenu = root.Q<VisualElement>("SettingsMenu");
        VisualElement settingsRoot = settingsMenu.Q<VisualElement>();
        mainMenu = root.Q<VisualElement>("MainMenu");
        VisualElement settingsPanel = settingsRoot.Q<VisualElement>().Q<VisualElement>();


        settingsButton = mainMenu.Q<Button>("SettingsButton");
        exitButton = settingsPanel.Q<Button>();

        settingsButton.RegisterCallback<ClickEvent>(evt => EnableSettings());
        exitButton.RegisterCallback<ClickEvent>(evt => ExitSettings());







        Button quitButton = root.Q<Button>("QuitButton");
        quitButton.RegisterCallback<ClickEvent>(evt => Application.Quit());

        Button startButton = root.Q<Button>("StartButton");
        startButton.RegisterCallback<ClickEvent>(evt => Time.timeScale = 1);
        startButton.RegisterCallback<ClickEvent>(evt => levelExit.StartLevelCoroutine(intro));
        startButton.RegisterCallback<ClickEvent>(evt => PlayerPrefs.DeleteKey("CheckpointID"));
        startButton.RegisterCallback<ClickEvent>(evt => PlayerPrefs.DeleteKey("PlayerPosX"));
        startButton.RegisterCallback<ClickEvent>(evt => PlayerPrefs.DeleteKey("PlayerPosY"));
        startButton.RegisterCallback<ClickEvent>(evt => PlayerPrefs.DeleteKey("Level"));


        Slider masterVolume = settingsPanel.Q<Slider>("MasterVolume");
        Slider musicVolume = settingsPanel.Q<Slider>("MusicVolume");
        Slider sfxVolume = settingsPanel.Q<Slider>("SFXVolume");

        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1);
        audioMixer.SetFloat("MasterVolume", masterVol);
        masterVolume.value = Mathf.Pow(10f, masterVol/ 20f);
        masterVolume.RegisterCallback<ChangeEvent<float>>(evt => audioMixer.SetFloat("MasterVolume", Mathf.Log10(evt.newValue) * 20));
        masterVolume.RegisterCallback<ChangeEvent<float>>(evt => PlayerPrefs.SetFloat("MasterVolume", Mathf.Log10(evt.newValue) * 20));


        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1); ;
        audioMixer.SetFloat("MusicVolume", musicVol);
        musicVolume.value = Mathf.Pow(10f, musicVol / 20f);
        musicVolume.RegisterCallback<ChangeEvent<float>>(evt => audioMixer.SetFloat("MusicVolume", Mathf.Log10(evt.newValue) * 20));
        musicVolume.RegisterCallback<ChangeEvent<float>>(evt => PlayerPrefs.SetFloat("MusicVolume", Mathf.Log10(evt.newValue) * 20));


        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1); ;
        audioMixer.SetFloat("SFXVolume", sfxVol);
        sfxVolume.value = Mathf.Pow(10f, sfxVol / 20f);
        sfxVolume.RegisterCallback<ChangeEvent<float>>(evt => audioMixer.SetFloat("SFXVolume", Mathf.Log10(evt.newValue) * 20));
        sfxVolume.RegisterCallback<ChangeEvent<float>>(evt => PlayerPrefs.SetFloat("SFXVolume", Mathf.Log10(evt.newValue) * 20));


        Toggle muteToggle = settingsPanel.Q<Toggle>("MuteToggle");
        muteToggle.RegisterValueChangedCallback(evt => PlayerPrefs.SetInt("Mute", evt.newValue ? 1 : 0));
        muteToggle.value = AudioListener.pause;
        muteToggle.RegisterCallback<ChangeEvent<bool>>(evt => AudioListener.pause = evt.newValue);

        Toggle fullscreenToggle = settingsPanel.Q<Toggle>("FullscreenToggle");
        fullscreenToggle.RegisterCallback<ChangeEvent<bool>>(evt => PlayerPrefs.SetInt("Fullscreen", evt.newValue ? 1 : 0));
        fullscreenToggle.value = Screen.fullScreen;
        fullscreenToggle.RegisterValueChangedCallback(evt => Screen.fullScreen = evt.newValue);

    }



    void EnableSettings()
    {
        settingsMenu.style.display = DisplayStyle.Flex;
    }

    void ExitSettings()
    {
        PlayerPrefs.Save();
        settingsMenu.style.display = DisplayStyle.None;
    }
}
