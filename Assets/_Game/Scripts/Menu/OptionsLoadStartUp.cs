using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class OptionsLoadStartUp
{
    private static readonly (int width, int height) _baseResolution = (640, 360);
    private static readonly OptionsMenuSave _defaultSettings = new OptionsMenuSave(
        1,
        Screen.currentResolution.refreshRate,
        QualitySettings.GetQualityLevel(),
        (int)FullScreenMode.ExclusiveFullScreen
    );
    private static OptionsMenuSave _currentSettings;

    private static readonly List<FullScreenMode> _fullScreenModes = new List<FullScreenMode> {
        FullScreenMode.ExclusiveFullScreen, //0
        FullScreenMode.FullScreenWindow,    //1
        FullScreenMode.Windowed             //2
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadGraphicalSettings()
    {
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

    private static void ApplySettings()
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
        }
        else
        {
            Screen.SetResolution(
                _baseResolution.width * _currentSettings.resScale,
                _baseResolution.height * _currentSettings.resScale,
                _fullScreenModes[_currentSettings.fullscreenMode],
                Screen.currentResolution.refreshRate
            );
        }
    }
}
