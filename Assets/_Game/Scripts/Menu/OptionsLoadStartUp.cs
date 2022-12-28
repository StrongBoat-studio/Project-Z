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
                    Screen.fullScreen
                );
                bf.Serialize(file, data);
            
                file.Close();
            }
            file.Close();

            Debug.Log(data.resWidth + " " + data.resHeight + " " + data.resRefreshRate + " " + data.qualLevel + " " + data.isFullscreen);

            Screen.SetResolution(data.resWidth, data.resHeight, data.isFullscreen, data.resRefreshRate);
            QualitySettings.SetQualityLevel(data.qualLevel);
            
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
                Screen.fullScreen
            );
            bf.Serialize(file, defualtData);
            
            file.Close();
        } 
    }
}
