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
            file = File.OpenRead(destination);
            OptionsMenuSave data = (OptionsMenuSave)bf.Deserialize(file);

            Screen.SetResolution(data.resWidth, data.resHeight, data.isFullscreen, data.resRefreshRate);
            QualitySettings.SetQualityLevel(data.qualLevel);

            file.Close();
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
