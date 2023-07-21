using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private LoadScreen _levelTransition;

    public void StartGame()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return StartCoroutine(_levelTransition.ShowScreen()); 
        _levelTransition.UpdateProgress(10f);

        //Load game save
        GameSaveManager.Instance.LoadJson();
        _levelTransition.UpdateProgress(15f);

        // Init managers 
        GameManager.Instance.Reset();
        DialogueManager.Instance.Reset();
        AudioManager.Instance.InitializeMainTheme(FMODEvents.Instance.MainTheme);
        _levelTransition.UpdateProgress(20f);

        // Load/Unload scenes
        Queue<(SceneRegister.Scenes scene, bool load)> ops = new Queue<(SceneRegister.Scenes scene, bool load)>();
        ops.Enqueue(
            GameSaveManager.Instance != null ? (
                GameSaveManager.Instance.currentSave.locationIndex >= 0 ? 
                ((SceneRegister.Scenes)GameSaveManager.Instance.currentSave.locationIndex, true) : 
                (SceneRegister.Scenes.ParadGround, true)
            ) : (
                (SceneRegister.Scenes.ParadGround, true)
            )
        );
        ops.Enqueue((SceneRegister.Scenes.Player, true));
        _levelTransition.UpdateProgress(25f);

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
        _levelTransition.UpdateProgress(70f);

        Debug.Log("Game done loading");
        SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.MainMenu);
        _levelTransition.UpdateProgress(90f);

        //Load data after location load
        GameSaveManager.Instance.LoadData();
        _levelTransition.UpdateProgress(100f);
        yield return StartCoroutine(_levelTransition.HideScreen());

        //Set gameplay state
        Debug.LogWarning("Reset game state");
        GameStateManager.Instance.ResetGameStateStack();
        GameStateManager.Instance.SetState(GameStateManager.GameState.Gameplay);
        yield return null;
    }
}
