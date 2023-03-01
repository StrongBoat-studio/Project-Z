using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class RoomLoader : MonoBehaviour, IInteractable
{
    [Tooltip("This scene")]
    [SerializeField] private SceneRegister.Scenes _originScene;

    [Tooltip("ID of this door (can be repeted on different scene")]
    [SerializeField] private int _id;

    [Tooltip("Tageted scene for destination door")]
    [SerializeField] private SceneRegister.Scenes _targetScene;

    [Tooltip("Door on target scene")]
    [SerializeField] private int _targetDoorId;

    private LocalKeyword _OUTLINE_ON;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
        DontDestroyOnLoad(this.gameObject);
    }

    public void CursorClick()
    {
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

        StartCoroutine("HandleSceneSwap", unload);
    }

    public void CursorEnter()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }

    private IEnumerator HandleSceneSwap(List<Scene> unload)
    {
        //Destroy current global light
        Destroy(FindObjectsOfType<Light2D>().First(x => x.lightType == Light2D.LightType.Global).gameObject);

        //Async load new scene
        List<AsyncOperation> ops = new List<AsyncOperation>();
        ops.Add(SceneManager.LoadSceneAsync((int)_targetScene, LoadSceneMode.Additive));

        //Wait for destination scene to load
        yield return new WaitUntil(() => ops.TrueForAll(x => x.isDone == true));
        
        //Clear ops and unload old scene
        ops.Clear();
        foreach(Scene s in unload)
        {
            ops.Add(SceneManager.UnloadSceneAsync(s));
        }

        //Wait for unload to finish
        Debug.Log(ops);
        yield return new WaitUntil(() => ops.TrueForAll(x => x.isDone == true));

        //Find door and teleport player to them
        List<RoomLoader> doors = new List<RoomLoader>();
        doors.AddRange(FindObjectsOfType<RoomLoader>());
        var matchingDoors = doors.FindAll(x => x.GetOriginDoor().scene == _targetScene && x.GetOriginDoor().id == _targetDoorId);
        if(matchingDoors.Count > 1)
        {
            Debug.LogWarning($"There are multiple ({matchingDoors.Count}) RoomLoaders with the same IDs! Teleporting to first match!");
            if(FindObjectOfType<Player>() != null)
            {
                FindObjectOfType<Player>().transform.position = matchingDoors[0].transform.position;
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }
            
            // if(GameManager.Instance != null && GameManager.Instance.player != null)
            // {
            //     GameManager.Instance.player
            // }
            // else
            // {
            //     Debug.LogAssertion("Player not found in GameMangers instance or GameManager Instance is null!");
            // }
        }

        Debug.Log("Done loading scene");
        Destroy(gameObject);
        yield return null;
    }

    public (int id, SceneRegister.Scenes scene) GetOriginDoor()
    {
        return (_id, _originScene);
    }

    public (int id, SceneRegister.Scenes scene) GetTargetDoor()
    {
        return (_targetDoorId, _targetScene);
    }
}
