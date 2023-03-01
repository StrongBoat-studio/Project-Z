using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform _btnStart;
    [SerializeField] private Transform _btnOptions;
    [SerializeField] private Transform _btnCredits;
    [SerializeField] private Transform _btnExit;

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

    private IEnumerator StartGame()
    {
        Queue<(SceneRegister.Scenes scene, bool load)> ops = new Queue<(SceneRegister.Scenes scene, bool load)>();
        ops.Enqueue((SceneRegister.Scenes.GameManagers, true));
        ops.Enqueue((SceneRegister.Scenes.Player, true));
        ops.Enqueue((SceneRegister.Scenes.SampleScene, true));
        ops.Enqueue((SceneRegister.Scenes.MainMenu, false));

        Destroy(GameObject.Find("EventSystem"));

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
        yield return null;
    }
}
