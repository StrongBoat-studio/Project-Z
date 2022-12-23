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
    //Resolution
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;
    private List<Resolution> _resolutions;

    //Fullscreen
    [SerializeField] private Toggle _fullscreenToggle;

    //Quality
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    private void Start()
    {
        //Get save data
        string destination = Application.persistentDataPath + "/options.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("Couldn't find save file!");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        OptionsMenuSave data = (OptionsMenuSave)bf.Deserialize(file);
        file.Close();

        //Set resolurion dropdown
        _resolutions = new List<Resolution>();
        _resolutions.AddRange(Screen.resolutions);

        List<string> _resolutionsText = _resolutions.ConvertAll(
            new Converter<Resolution, string>(delegate (Resolution res) { return res.ToString(); })
        );

        _resolutionsDropdown.ClearOptions();
        _resolutionsDropdown.AddOptions(_resolutionsText);
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
            _fullscreenToggle.isOn
        );
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();

        Debug.Log(
            "Save: " + data.resWidth + ", " + data.resHeight + ", " + data.qualLevel + ", " + data.isFullscreen
        );
    }

    // private void LoadOptions()
    // {
    //     string destination = Application.persistentDataPath + "/options.dat";
    //     FileStream file;

    //     if (File.Exists(destination)) file = File.OpenRead(destination);
    //     else
    //     {
    //         Debug.LogError("Options save file not found");
    //         return;
    //     }

    //     BinaryFormatter bf = new BinaryFormatter();
    //     OptionsMenuSave data = (OptionsMenuSave)bf.Deserialize(file);
    //     file.Close();

    //     _resolutionsDropdown.value = _resolutions.FindIndex(x => x.width == data.resWidth && x.height == data.resHeight);
    //     _qualityDropdown.value = data.qualLevel;
    //     _fullscreenToggle.isOn = data.isFullscreen;

    //     SetResolution();
    //     SetQuality();
    //     SetFullscreen();

    //     //Debug.Log(_resolutionsDropdown.value);
    //     //Debug.Log(_qualityDropdown.value);
    //     //Debug.Log(_fullscreenToggle.isOn);
    // }
}

