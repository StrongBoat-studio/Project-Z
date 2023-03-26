using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SettingsFileHandler
{
    public SettingsData LoadFromFile(string path, string fname)
    {
        if(!File.Exists(path + '/' + fname)) 
        {
            File.Create(path + '/' + fname).Close();
            return null;
        }
        
        BinaryFormatter bf = new BinaryFormatter();
        try 
        {
            FileStream fs = File.OpenRead(path + '/' + fname);
            SettingsData data = (SettingsData)bf.Deserialize(fs);
            fs.Close();
            return data;
        }
        catch
        {
            // File is probably corrupted, delete and create new one, empty
            File.Delete(path + '/' + fname);
            File.Create(path + '/' + fname).Close();
            return null;
        }
    } 

    public void SaveToFile(string path, string fname, SettingsData data)
    {
        if(!File.Exists(path + '/' + fname)) File.Create(path + '/' + fname).Close();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.OpenWrite(path + '/' + fname);
        try 
        {
            bf.Serialize(fs, data);
        }
        catch
        {
            Debug.LogError("Serialization Error");
        } 
        fs.Close();
    }
}
