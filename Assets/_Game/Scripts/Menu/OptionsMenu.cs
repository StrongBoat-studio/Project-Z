using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class OptionsMenu : MonoBehaviour
{
    // Resolution
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;
    private List<Resolution> _resolutions;

    // Fullscreen
    [SerializeField] private Toggle _fullscreenToggle;

    // Quality
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    // Audio
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _ambienceVolumeSlider;

    private void Start()
    {
        if(SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager is null!");
            return;
        }

        //Set resolurion dropdown
        _resolutions = new List<Resolution>();
        _resolutions.AddRange(Screen.resolutions);

        List<string> _resolutionsText = _resolutions.ConvertAll(
            new Converter<Resolution, string>(delegate (Resolution res) { return res.ToString(); })
        );

        //Add resolution options to dropdown
        _resolutionsDropdown.ClearOptions();
        _resolutionsDropdown.AddOptions(_resolutionsText);

        //Find resolution based on settings
        _resolutionsDropdown.value = _resolutions.FindIndex(x =>
            x.width == SettingsManager.Instance.settingsData.resWidth &&
            x.height == SettingsManager.Instance.settingsData.resHeight &&
            x.refreshRate == SettingsManager.Instance.settingsData.resRefreshRate
        );
        _resolutionsDropdown.RefreshShownValue();
        Debug.Log($"SM {SettingsManager.Instance.settingsData.isFullscreen}, {SettingsManager.Instance.settingsData.qualLevel}");

        //Set fullscreen toggle
        _fullscreenToggle.isOn = SettingsManager.Instance.settingsData.isFullscreen;

        //Set quality dropdown
        _qualityDropdown.value = SettingsManager.Instance.settingsData.qualLevel;
        _qualityDropdown.RefreshShownValue();

        //Set audio volume
        if (AudioManager.Instance != null)
        {
            _masterVolumeSlider.value = SettingsManager.Instance.settingsData.masterVolume;
            _sfxVolumeSlider.value = SettingsManager.Instance.settingsData.sfxVolume;
            _musicVolumeSlider.value = SettingsManager.Instance.settingsData.musicVolume;
            _ambienceVolumeSlider.value = SettingsManager.Instance.settingsData.ambienceVolume;   
        }
        else Debug.LogError("AudioMananger is null!");
    }

    public void SetResolution()
    {
        Screen.SetResolution(
            _resolutions[_resolutionsDropdown.value].width,
            _resolutions[_resolutionsDropdown.value].height,
            Screen.fullScreen,
            _resolutions[_resolutionsDropdown.value].refreshRate
        );
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = _fullscreenToggle.isOn;
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(_qualityDropdown.value);
    }

    public void SetAudio(string bus)
    {
        if(AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager in null");
            return;
        }

        switch(bus)
        {
            case "Master":
                AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.Master, _masterVolumeSlider.value);
                break;
            case "Music":
                AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.Music, _musicVolumeSlider.value);
                break;
            case "SFX":
                AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.SFX, _sfxVolumeSlider.value);
                break;
            case "Ambience":
                AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.Ambience, _ambienceVolumeSlider.value);
                break;
        }
    }

    public void MenuButton()
    {
        SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.OptionsMenu);
    }

    public void SaveOptions()
    {
        if (SettingsManager.Instance == null) 
        {
            Debug.LogError("No SettingsManager in scene!");
            return;
        }

        SettingsManager.Instance.settingsData.resWidth = _resolutions[_resolutionsDropdown.value].width;
        SettingsManager.Instance.settingsData.resHeight = _resolutions[_resolutionsDropdown.value].height;
        SettingsManager.Instance.settingsData.resRefreshRate = _resolutions[_resolutionsDropdown.value].refreshRate;
        SettingsManager.Instance.settingsData.isFullscreen = _fullscreenToggle.isOn;
        SettingsManager.Instance.settingsData.qualLevel = _qualityDropdown.value;

        if (AudioManager.Instance != null)
        {
            SettingsManager.Instance.settingsData.masterVolume = _masterVolumeSlider.value;
            SettingsManager.Instance.settingsData.sfxVolume = _sfxVolumeSlider.value;
            SettingsManager.Instance.settingsData.musicVolume = _musicVolumeSlider.value;
            SettingsManager.Instance.settingsData.ambienceVolume = _ambienceVolumeSlider.value;
        }
        else Debug.LogError("No AudioManager in scene!");

        SettingsManager.Instance.SaveSettings();
        SettingsManager.Instance.LoadSettings();
    }
}

