using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _sfh = new SettingsFileHandler();
        _defaultSettingsData = new SettingsData(
            Screen.currentResolution.width,
            Screen.currentResolution.height,
            Screen.currentResolution.refreshRate,
            QualitySettings.GetQualityLevel(),
            true,
            1f,1f,1,1f
        );

        LoadSettings();
    }

    private SettingsFileHandler _sfh;
    private SettingsData _defaultSettingsData;
    public SettingsData settingsData;

    public void SaveSettings()
    {
        _sfh.SaveToFile(Application.persistentDataPath, "/settings.dat", settingsData);
    }

    public void LoadSettings()
    {
        // If LoadFromFile returns null, use defualt values and save them
        settingsData = _sfh.LoadFromFile(Application.persistentDataPath, "/settings.dat");
        if(settingsData == null) CreateNewSettings();
        
        if(AudioManager.Instance != null) 
        {
            AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.Master, settingsData.masterVolume);
            AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.Music, settingsData.musicVolume);
            AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.SFX, settingsData.sfxVolume);
            AudioManager.Instance.UpdateVolume(AudioManager.VolumeBus.Ambience, settingsData.ambienceVolume);
        }

        Screen.SetResolution(settingsData.resWidth, settingsData.resHeight, settingsData.isFullscreen, settingsData.resRefreshRate);
        QualitySettings.SetQualityLevel(settingsData.qualLevel);
    }

    public void CreateNewSettings()
    {
        _sfh.SaveToFile(Application.persistentDataPath, "/settings.dat", _defaultSettingsData);
        settingsData = _defaultSettingsData;
    }
}
