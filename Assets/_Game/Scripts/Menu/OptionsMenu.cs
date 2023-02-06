using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OptionsMenu : MonoBehaviour
{
    private OptionsMenuSave _defaultSettings;
    private OptionsMenuSave _currentSettings;

    //Resolution
    private readonly (int width, int height) _baseResolution = (640, 360);
    [SerializeField] private TMP_Dropdown _resScaleDropdown;
    private List<(int width, int height)> _resolutions;

    //Fullscreen
    [SerializeField] private TMP_Dropdown _fullscreenModeDropdown;
    private readonly List<FullScreenMode> _fullScreenModes = new List<FullScreenMode> {
        FullScreenMode.ExclusiveFullScreen, //0
        FullScreenMode.FullScreenWindow,    //1
        FullScreenMode.Windowed             //2
    };

    //Quality
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    private void Start()
    {
        _defaultSettings = new OptionsMenuSave(
            1,
            Screen.currentResolution.refreshRate,
            QualitySettings.GetQualityLevel(),
            (int)FullScreenMode.ExclusiveFullScreen
        );

        # region Dropdowns update
        //Set resolurion dropdown
        _resolutions = new List<(int w, int h)>();
        for (int i = 0; i < Mathf.FloorToInt(Screen.resolutions[Screen.resolutions.Length - 1].width / _baseResolution.width); i++)
        {
            _resolutions.Add((_baseResolution.width * (i + 1), _baseResolution.height * (i + 1)));
        }

        List<string> _resolutionsText = _resolutions.ConvertAll(
            new Converter<(int width, int height), string>(delegate ((int width, int height) res)
            {
                //WIDTHxHEIGHT (SCALEx)
                return res.width.ToString() + "x" + res.height.ToString() + " (" + (res.width / _baseResolution.width).ToString() + "x)";
            })
        );

        //Add resolution options to dropdown
        _resScaleDropdown.ClearOptions();
        _resScaleDropdown.AddOptions(_resolutionsText);
        #endregion

        //Get save data
        string destination = Application.persistentDataPath + "/options.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        if (File.Exists(destination))
        {
            //Open setting file
            file = File.OpenRead(destination);

            //Check for deserialization errors
            try
            {
                _currentSettings = (OptionsMenuSave)bf.Deserialize(file);
            }
            catch
            {
                //Deseriazlization ended with and expetion (file could have been modified)
                //Override settings file with default 
                file.Close();
                file = File.OpenWrite(destination);
                bf.Serialize(file, _defaultSettings);
                _currentSettings = _defaultSettings;
                file.Close();
            }
            file.Close();
        }
        else
        {
            //If file does not exist (or was deleted during gameplay), create new file with default settings
            file = File.Create(destination);
            bf.Serialize(file, _defaultSettings);
            _currentSettings = _defaultSettings;
            file.Close();
        }

        ApplySettings();
    }

    public void SetResolution()
    {
        _currentSettings.resScale = _resScaleDropdown.value;
        ApplySettings();
        SaveOptions();
    }

    public void SetFullscrenMode()
    {
        _currentSettings.fullscreenMode = _fullscreenModeDropdown.value;
        ApplySettings();
        SaveOptions();
    }

    public void SetQuality()
    {
        _currentSettings.qualLevel = _qualityDropdown.value;
        ApplySettings();
        SaveOptions();
    }

    public void MenuButton()
    {
        SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.OptionsMenu);
    }

    private void SaveOptions()
    {
        string destination = Application.persistentDataPath + "/options.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, _currentSettings);
        file.Close();

        Debug.Log(
            "Save: " + _currentSettings.resScale + "x, " + _currentSettings.resRefreshRate + "Hz, Quality: " + _currentSettings.qualLevel + ", Fullscren" + _fullScreenModes[_currentSettings.fullscreenMode].ToString()
        );
    }

    private void ApplySettings()
    {
        if(_fullScreenModes[_currentSettings.fullscreenMode] == FullScreenMode.ExclusiveFullScreen ||
        _fullScreenModes[_currentSettings.fullscreenMode] == FullScreenMode.FullScreenWindow)
        {
            Screen.SetResolution(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                _fullScreenModes[_currentSettings.fullscreenMode],
                Screen.currentResolution.refreshRate
            );

            //Scale is irrelevant when game is in fullscreen, disable it
            _resScaleDropdown.value = _resolutions.Count() - 1;
            _resScaleDropdown.interactable = false;
        }
        else
        {
            _resScaleDropdown.interactable = true;

            Screen.SetResolution(
                _baseResolution.width * (_currentSettings.resScale + 1),
                _baseResolution.height * (_currentSettings.resScale + 1),
                _fullScreenModes[_currentSettings.fullscreenMode],
                Screen.currentResolution.refreshRate
            );

            //Find resolution based on settings
            _resScaleDropdown.value = _resolutions.FindIndex(x =>
                x.width == _baseResolution.width * (_currentSettings.resScale + 1) &&
                x.height == _baseResolution.height * (_currentSettings.resScale + 1)
            );
        }

        //Set resoultion dropdown
        _resScaleDropdown.RefreshShownValue();

        //Set fullscreen dropdown
        _fullscreenModeDropdown.value = _currentSettings.fullscreenMode;
        _fullscreenModeDropdown.RefreshShownValue();

        //Set quality dropdown
        _qualityDropdown.value = _currentSettings.qualLevel;
        _qualityDropdown.RefreshShownValue();
    }
}

