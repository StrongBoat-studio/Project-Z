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
        StartCoroutine("StartGame");
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
    
    private IEnumerator StartGame()
    {
        // Load/Unload scenes
        Queue<(SceneRegister.Scenes scene, bool load)> ops = new Queue<(SceneRegister.Scenes scene, bool load)>();
        ops.Enqueue((SceneRegister.Scenes.Player, true));
        ops.Enqueue((SceneRegister.Scenes.SampleScene, true));
        ops.Enqueue((SceneRegister.Scenes.MainMenu, false));

        while (ops.Count > 0)
        {
            AsyncOperation aop;
            var op = ops.Dequeue();
            if(op.load == true)
            {
                aop = SceneManager.LoadSceneAsync((int)op.scene, LoadSceneMode.Additive);
            }
            else
            {
                aop = SceneManager.UnloadSceneAsync((int)op.scene);
            }

            yield return new WaitUntil(() => aop.isDone == true);
        }

        // Init managers 
        GameManager.Instance.Reset();
        DialogueManager.Instance.Reset();

        GameStateManager.Instance.ResetGameStateStack();
        GameStateManager.Instance.SetState(GameStateManager.GameState.Gameplay);
        AudioManager.Instance.InitializeMainTheme(FMODEvents.Instance.MainTheme);

        Debug.Log("Game done loading");
        yield return null;
    }
}
