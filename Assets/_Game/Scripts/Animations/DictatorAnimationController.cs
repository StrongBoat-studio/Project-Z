using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictatorAnimationController : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private Animator _animator;

    private bool _isDialogue;
    private void Awake()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _isDialogue = newGameState == GameStateManager.GameState.Dialogue;
    }

    // Update is called once per frame
    void Update()
    {
        IsDialogue();
    }

    private void IsDialogue()
    {
        if(_isDialogue)
        {
            _animator.SetBool("IsDialogue", true);
        }
        else
        {
            _animator.SetBool("IsDialogue", false);
        }
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
