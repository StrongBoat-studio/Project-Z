using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }

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
    }

    public GameData currentSave;

    public void SaveJson()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";

        if(File.Exists(savePath) == false)
        {
            File.Create(savePath).Close();
        }

        string json = JsonUtility.ToJson(currentSave);
        File.WriteAllText(savePath, json);
    }

    public void LoadJson()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";

        if(File.Exists(savePath) == false)
        {
            currentSave = new GameData();
            return;
        }
        
        currentSave = JsonUtility.FromJson<GameData>(File.ReadAllText(savePath));
    }
}
