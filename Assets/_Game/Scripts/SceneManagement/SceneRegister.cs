using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SceneRegister : MonoBehaviour
{
    public enum Scenes
    {
        MainMenu = 0,
        OptionsMenu = 1,
        CreditsMenu = 2,
        GameManagers = 3,
        SampleScene = 4,
        SampleSceneTestTP = 5,
        Player = 6,
        Comms = 7,
        ParadGround = 8,
        Greenhouse = 9,
        ControlsMenu = 10,
        Bridge = 11,
        Entrance = 12,
        Lab = 13,
        LeadersRoom = 14,
        EarthLeadersRoom = 15,
        Cutscenka = 16,
    }

    public static SceneRegister Instance { get; private set; }

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

    public List<Scenes> perserverLevelSwap;

    public void LoadNextLevel(RoomLoader roomLoader)
    {
        //Save data
        if (GameSaveManager.Instance != null)
        {
            //Save new location (destination of door)
            GameSaveManager.Instance.currentSave.locationIndex = (int)roomLoader.GetTargetDoor().scene;
        }
        else
        {
            Debug.Log("GameSaveManager is null. Can't save gameData");
        }

        StartCoroutine(HandleSceneSwap(roomLoader));
    }

    private IEnumerator HandleSceneSwap(RoomLoader roomLoader)
    {
        if (GameSaveManager.Instance == null)
        {
            Debug.Log("Can't save data, try again");
            yield return null;
        }

        //Save origin location's level data
        if (FindObjectOfType<LevelManager>() != null)
        {
            SaveLevelManagerData(FindObjectOfType<LevelManager>().GetLevelData());
        }

        //Find scenes to unload
        List<Scene> unload = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneRegister.Instance.perserverLevelSwap.FindIndex(
                scene => (int)scene == SceneManager.GetSceneAt(i).buildIndex
            ) == -1)
            {
                unload.Add(SceneManager.GetSceneAt(i));
            }
        }

        //Async unload old scenes
        List<AsyncOperation> ops = new List<AsyncOperation>();
        foreach (Scene s in unload)
        {
            ops.Add(SceneManager.UnloadSceneAsync(s));
        }

        //Wait for unload to finish
        yield return new WaitUntil(() => ops.TrueForAll(x => x.isDone == true));

        //Clear ops and async load new scenes
        ops.Clear();
        ops.Add(SceneManager.LoadSceneAsync((int)roomLoader.GetTargetDoor().scene, LoadSceneMode.Additive));

        //Wait for destination scene to load
        yield return new WaitUntil(() => ops.TrueForAll(x => x.isDone == true));

        //Find door and teleport player to them
        List<RoomLoader> doors = new List<RoomLoader>();
        doors.AddRange(FindObjectsOfType<RoomLoader>());
        var matchingDoors = doors.FindAll(x => x.GetOriginDoor().scene == roomLoader.GetTargetDoor().scene && x.GetOriginDoor().id == roomLoader.GetTargetDoor().id);
        if (matchingDoors.Count <= 0)
        {
            Debug.LogWarning($"There isn't any matching <RoomLoader> on current scene to tp! Doing nothing to the player");
        }
        else
        {
            if (matchingDoors.Count > 1)
            {
                Debug.LogWarning($"There are multiple ({matchingDoors.Count}) <RoomLoader>s with the same IDs! Teleporting to first match!");
            }

            if (FindObjectOfType<Player>() != null)
            {
                Player player = FindObjectOfType<Player>(); // Player ref is set

                Vector3 spawnPos = new Vector2(
                    matchingDoors[0].GetSpawnPosition().x,
                    matchingDoors[0].GetSpawnPosition().y + player.GetComponent<Collider2D>().bounds.size.y / 2 + 1/16
                );
                player.transform.position = spawnPos;

                //Set player spawn for save
                GameSaveManager.Instance.currentSave.spawnPosition = spawnPos;
            }
            else
            {
                Debug.LogWarning("Player not found on scene!");
            }
        }

        //When scene loading is done, save data to json file
        GameSaveManager.Instance.SaveJson();
        yield return null;
    }

    public void SaveLevelManagerData(LevelManagerData lmd)
    {
        int idx = GameSaveManager.Instance.currentSave.levelManagerDatas.FindIndex(
            x => x.sceneIndex == lmd.sceneIndex
        );

        if (idx != -1)
        {
            //Update level manager's data
            GameSaveManager.Instance.currentSave.levelManagerDatas[idx] = lmd;
        }
        else
        {
            //Add new level manager data if it did not exist in save
            GameSaveManager.Instance.currentSave.levelManagerDatas.Add(lmd);
        }
    }
}
