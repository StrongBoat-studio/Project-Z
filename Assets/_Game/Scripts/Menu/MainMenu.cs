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
        Destroy(GameObject.Find("EventSystem"));

        SceneManager.LoadSceneAsync(
            (int)SceneRegister.Scenes.GameManagers,
            LoadSceneMode.Additive
        ).completed += delegate {
            SceneManager.LoadSceneAsync(
                (int)SceneRegister.Scenes.SampleScene,
                LoadSceneMode.Additive
            ).completed += delegate {
                SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.MainMenu);
            };
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
}
