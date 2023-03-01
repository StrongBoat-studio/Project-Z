using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
