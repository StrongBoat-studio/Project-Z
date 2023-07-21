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
    [SerializeField] private LoadScreen _levelTransition;

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
            AudioManager.Instance != null &&
            GameSaveManager.Instance != null
        );

        FindObjectOfType<LoadGame>().StartGame();
    }
}
