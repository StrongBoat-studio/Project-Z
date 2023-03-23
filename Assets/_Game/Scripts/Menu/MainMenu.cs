using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform _btnStart;
    [SerializeField] private Transform _btnOptions;
    [SerializeField] private Transform _btnCredits;
    [SerializeField] private Transform _btnExit;

    private void Awake()
    {
        Debug.Log("Main menu awake");
        // Load GameManagers scene at startup (when menu is loaded)
        // Don't load if GameManagers scene is already present
        // Clear game state stack and set MainMenu gamestate
        for (int i = 0; i < SceneManager.sceneCount; i++)
            if (SceneManager.GetSceneAt(i).buildIndex == (int)SceneRegister.Scenes.GameManagers) return;

        SceneManager.LoadSceneAsync((int)SceneRegister.Scenes.GameManagers, LoadSceneMode.Additive);

        GameStateManager.Instance.ResetGameStateStack();
        GameStateManager.Instance.SetState(GameStateManager.GameState.MainMenu);
    }

    public void BtnStart()
    {
        Debug.Log("start");
        Destroy(GameObject.Find("EventSystem"));

        SceneManager.LoadSceneAsync(
            (int)SceneRegister.Scenes.SampleScene,
            LoadSceneMode.Additive
        ).completed += delegate
        {
            GameStateManager.Instance.ResetGameStateStack();
            GameStateManager.Instance.SetState(GameStateManager.GameState.Gameplay);
            SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.MainMenu);
            AudioManager.Instance.InitializeMainTheme(FMODEvents.Instance.MainTheme);
        };
    }

    public void BtnOptions()
    {
        SceneManager.LoadSceneAsync(
            (int)SceneRegister.Scenes.OptionsMenu,
            LoadSceneMode.Additive
        );
    }

    public void BtnCredits()
    {
        SceneManager.LoadSceneAsync(
            (int)SceneRegister.Scenes.OptionsMenu,
            LoadSceneMode.Additive
        );
    }

    public void BtnExit()
    {
        Application.Quit();
    }

    public void PlayHover()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.UIButtonHover, transform.position);
        }
    }
}
