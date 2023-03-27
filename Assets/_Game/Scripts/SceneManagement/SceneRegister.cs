using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Rendering.Universal;

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
        StartCoroutine(HandleSceneSwap(roomLoader));
    }

    private IEnumerator HandleSceneSwap(RoomLoader roomLoader)
    {
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

        //Destroy current global light
        //Destroy(FindObjectsOfType<Light2D>().First(x => x.lightType == Light2D.LightType.Global).gameObject);

        //Async load new scene
        List<AsyncOperation> ops = new List<AsyncOperation>();
        ops.Add(SceneManager.LoadSceneAsync((int)roomLoader.GetTargetDoor().scene, LoadSceneMode.Additive));

        //Wait for destination scene to load
        yield return new WaitUntil(() => ops.TrueForAll(x => x.isDone == true));

        //Clear ops and unload old scene
        ops.Clear();
        foreach (Scene s in unload)
        {
            ops.Add(SceneManager.UnloadSceneAsync(s));
        }

        //Wait for unload to finish
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

                RaycastHit2D[] rhit = Physics2D.RaycastAll(matchingDoors[0].transform.position, Vector2.down);
                RaycastHit2D rhitGround = rhit.First(x => x.transform.gameObject.layer == 7);
                //Scale 
                player.transform.position = new Vector2(
                    //x set to <RoomLoader>'s position
                    matchingDoors[0].transform.position.x,
                    //y set to raycast hit y pos + 1/2 of player's collider + 1 pixel (1/PPU) to avoid clipping
                    rhitGround.point.y + (player.GetComponent<Collider2D>().bounds.extents.y + 0.03125f)
                );
            }
            else
            {
                Debug.LogWarning("Player not found on scene!");
            }
        }
        yield return null;
    }
}
