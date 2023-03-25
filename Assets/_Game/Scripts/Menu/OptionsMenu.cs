using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        //Set resolurion dropdown
        _resolutions = new List<Resolution>();
        _resolutions.AddRange(Screen.resolutions);

        List<string> _resolutionsText = _resolutions.ConvertAll(
            new Converter<Resolution, string>(delegate (Resolution res) { return res.ToString(); })
        );

        //Add resolution options to dropdown
        _resolutionsDropdown.ClearOptions();
        _resolutionsDropdown.AddOptions(_resolutionsText);

        //Get save data
        string destination = Application.persistentDataPath + "/options.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        if (File.Exists(destination))
        {
            //Open setting file
            OptionsMenuSave data;
            file = File.OpenRead(destination);

            try
            {
                //Check if deserialization didn't encouter any excpetions
                data = (OptionsMenuSave)bf.Deserialize(file);
            }
            catch
            {
                //Deseriazlization ended with and expetion (file could have been modified)
                //Open settings file to write
                file.Close();
                file = File.OpenWrite(destination);

                //Create a new settings data with current screen settings
                data = new OptionsMenuSave(
                    Screen.currentResolution.width,
                    Screen.currentResolution.height,
                    Screen.currentResolution.refreshRate,
                    QualitySettings.GetQualityLevel(),
                    true,
                    1f,1f,1f,1f
                );
                bf.Serialize(file, data);
            
                file.Close();
            }
            file.Close();

            //Find resolution based on settings
            _resolutionsDropdown.value = _resolutions.FindIndex(x =>
                x.width == data.resWidth &&
                x.height == data.resHeight &&
                x.refreshRate == data.resRefreshRate
            );
            _resolutionsDropdown.RefreshShownValue();

            //Set fullscreen toggle
            _fullscreenToggle.isOn = data.isFullscreen;

            //Set quality dropdown
            _qualityDropdown.value = data.qualLevel;
            _qualityDropdown.RefreshShownValue();

            //Set audio volume
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.masterVolume = data.masterVolume;
                AudioManager.Instance.sfxVolume = data.sfxVolume;
                AudioManager.Instance.musicVolume = data.musicVolume;
                AudioManager.Instance.ambienceVolume = data.ambienceVolume;

                _masterVolumeSlider.value = data.masterVolume;
                _sfxVolumeSlider.value = data.sfxVolume;
                _musicVolumeSlider.value = data.musicVolume;
                _ambienceVolumeSlider.value = data.ambienceVolume;
            }
        }
        else
        {
            //If file does not exist (or was deleted during gameplay), create it and save current screen data as options data
            file = File.Create(destination);

            var defualtData = new OptionsMenuSave(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                Screen.currentResolution.refreshRate,
                QualitySettings.GetQualityLevel(),
                true,
                1f,1f,1f,1f
            );
            bf.Serialize(file, defualtData);

            file.Close();

            //Find resolution based on settings
            _resolutionsDropdown.value = _resolutions.FindIndex(x =>
                x.width == defualtData.resWidth &&
                x.height == defualtData.resHeight &&
                x.refreshRate == defualtData.resRefreshRate
            );
            _resolutionsDropdown.RefreshShownValue();

            //Set fullscreen toggle
            _fullscreenToggle.isOn = defualtData.isFullscreen;

            //Set quality dropdown
            _qualityDropdown.value = defualtData.qualLevel;
            _qualityDropdown.RefreshShownValue();

            //Set audio volume
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.masterVolume = defualtData.masterVolume;
                AudioManager.Instance.sfxVolume = defualtData.sfxVolume;
                AudioManager.Instance.musicVolume = defualtData.musicVolume;
                AudioManager.Instance.ambienceVolume = defualtData.ambienceVolume;

                _masterVolumeSlider.value = defualtData.masterVolume;
                _sfxVolumeSlider.value = defualtData.sfxVolume;
                _musicVolumeSlider.value = defualtData.musicVolume;
                _ambienceVolumeSlider.value = defualtData.ambienceVolume;
            }
        }
    }

    public void SetResolution()
    {
        Screen.SetResolution(
            _resolutions[_resolutionsDropdown.value].width,
            _resolutions[_resolutionsDropdown.value].height,
            Screen.fullScreen,
            _resolutions[_resolutionsDropdown.value].refreshRate
        );
        SaveOptions();
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = _fullscreenToggle.isOn;
        SaveOptions();
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(_qualityDropdown.value);
        SaveOptions();
    }

    public void MenuButton()
    {
        SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.OptionsMenu);
    }

    public void SetMasterVolume()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.masterVolume = _masterVolumeSlider.value;
            SaveOptions();
        }
    }

    public void SetSFXVolume()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.sfxVolume = _sfxVolumeSlider.value;
            SaveOptions();
        }
    }

    public void SetMusicVolume()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.musicVolume = _musicVolumeSlider.value;
            SaveOptions();
        }
    }

    public void SetAmbienceVolume()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.ambienceVolume = _ambienceVolumeSlider.value;
            SaveOptions();
        }
    }

    private void SaveOptions()
    {
        string destination = Application.persistentDataPath + "/options.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        OptionsMenuSave data = new OptionsMenuSave(
            _resolutions[_resolutionsDropdown.value].width,
            _resolutions[_resolutionsDropdown.value].height,
            _resolutions[_resolutionsDropdown.value].refreshRate,
            _qualityDropdown.value,
            _fullscreenToggle.isOn,
            (AudioManager.Instance != null) ? AudioManager.Instance.masterVolume : 1f,
            (AudioManager.Instance != null) ? AudioManager.Instance.sfxVolume : 1f,
            (AudioManager.Instance != null) ? AudioManager.Instance.musicVolume : 1f,
            (AudioManager.Instance != null) ? AudioManager.Instance.ambienceVolume : 1f
        );
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();

        Debug.Log(
            $"Save: Res {data.resWidth}x{data.resHeight}, FS {data.isFullscreen}, Qual {data.qualLevel}, Vols ma{data.masterVolume}/mu{data.musicVolume}/sfx{data.sfxVolume}/am{data.ambienceVolume}"
        );
    }
}

