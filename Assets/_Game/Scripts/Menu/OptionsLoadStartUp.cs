using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class OptionsLoadStartUp
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadGraphicalSettings()
    {
        string destination = Application.persistentDataPath + "/options.dat";
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(destination))
        {
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

            Debug.Log(data.resWidth + " " + data.resHeight + " " + data.resRefreshRate + " " + data.qualLevel + " " + data.isFullscreen);

            Screen.SetResolution(data.resWidth, data.resHeight, data.isFullscreen, data.resRefreshRate);
            QualitySettings.SetQualityLevel(data.qualLevel);
            
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.masterVolume = data.masterVolume;
                AudioManager.Instance.sfxVolume = data.sfxVolume;
                AudioManager.Instance.musicVolume = data.musicVolume;
                AudioManager.Instance.ambienceVolume = data.ambienceVolume;
            }
        }
        else
        {
            //If file does not exist, create it and save current screen data as options data
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

            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.masterVolume = 1f;
                AudioManager.Instance.sfxVolume = 1f;
                AudioManager.Instance.musicVolume = 1f;
                AudioManager.Instance.ambienceVolume = 1f;
            }
            
            file.Close();
        } 
    }
}
