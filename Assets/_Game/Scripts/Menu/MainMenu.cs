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
    [SerializeField] private Transform _btnControls;
    [SerializeField] private Transform _btnCredits;
    [SerializeField] private Transform _btnExit;

    private void Awake()
    {
        // Load GameManagers scene at startup (when menu is loaded)
        // Don't load if GameManagers scene is already present
        // Clear game state stack and set MainMenu gamestate
        bool gameManagerPresent = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == (int)SceneRegister.Scenes.GameManagers) 
            {
                gameManagerPresent = true;
                break;
            }
        }

        if(gameManagerPresent == false)
            SceneManager.LoadSceneAsync((int)SceneRegister.Scenes.GameManagers, LoadSceneMode.Additive);
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

    public void BtnControls()
    {
        SceneManager.LoadSceneAsync(
          (int)SceneRegister.Scenes.ControlsMenu,
          LoadSceneMode.Additive  
        );
    }

    public void BtnCredits()
    {
        SceneManager.LoadSceneAsync(
            (int)SceneRegister.Scenes.CreditsMenu,
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
        yield return new WaitUntil( () => 
            GameManager.Instance != null &&
            DialogueManager.Instance != null &&
            GameStateManager.Instance != null &&
            AudioManager.Instance != null
        );
        // Init managers 
        GameManager.Instance.Reset();
        DialogueManager.Instance.Reset();

        GameStateManager.Instance.ResetGameStateStack();
        GameStateManager.Instance.SetState(GameStateManager.GameState.Gameplay);
        AudioManager.Instance.InitializeMainTheme(FMODEvents.Instance.MainTheme);

        // Load/Unload scenes
        Queue<(SceneRegister.Scenes scene, bool load)> ops = new Queue<(SceneRegister.Scenes scene, bool load)>();
        ops.Enqueue((SceneRegister.Scenes.ParadGround, true));
        ops.Enqueue((SceneRegister.Scenes.Player, true));

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

        Debug.Log("Game done loading");
        SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.MainMenu);

        //Try load game
        if(GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.LoadJson();

            //Load Inventory
            GameManager.Instance.player.GetComponent<Player>().GetInventory().LoadSave(
                GameSaveManager.Instance.currentSave.inventoryItems
            );
        }
        yield return null;
    }
}
