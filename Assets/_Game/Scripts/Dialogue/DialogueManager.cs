using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private PlayerInput _playerInput;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _messageText;

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

        _playerInput = new PlayerInput();
        _playerInput.Dialogue.Continue.performed += OnDialogueContinue;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private GameStateManager.GameState _prevGameState;
    private Story _currentStory;
    [SerializeField][Tooltip("Characters per second")][Min(1f)] private float _typeSpeed;
    [SerializeField][Tooltip("Characters per second")][Min(1f)] private float _typeSpeedSkip;
    private float _currentTypeSpeed;
    private bool _isTyping = false;

    private void OnDialogueContinue(InputAction.CallbackContext context)
    {
        ContinueStory();
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if (newGameState != GameStateManager.GameState.Dialogue)
        {
            _playerInput.Dialogue.Disable();
        }
        else
        {
            _playerInput.Dialogue.Enable();
        }
    }

    ///<summary>
    ///Starts dialogue, enables dialogue UI and changes game state
    ///</summary>
    ///<param name="storyTextAsset">Ink's story text asset</param>
    public void StartStory(TextAsset storyTextAsset)
    {
        _prevGameState = GameStateManager.Instance.GetState();
        GameStateManager.Instance.SetState(GameStateManager.GameState.Dialogue);
        _canvas.gameObject.SetActive(true);
        _currentTypeSpeed = _typeSpeed;
        _currentStory = new Story(storyTextAsset.text);
        ContinueStory();
    }


    ///<summary>
    ///Continues strory if possible, if not - ends dialogue
    ///</summary>
    private void ContinueStory()
    {
        if (_currentStory == null) return;
        if (_isTyping == true)
        {
            _currentTypeSpeed = _typeSpeedSkip;
            return;
        }
        if (_currentStory.canContinue)
        {
            _currentTypeSpeed = _typeSpeed;
            string text = _currentStory.Continue();
            ParseTags(_currentStory.currentTags);

            StartCoroutine("TypeMessage", text);
        }
        else
        {
            EndStory();
        }
    }

    ///<summary>
    ///Ends dialogue, disables UI and reverts game state
    ///</summary>
    private void EndStory()
    {
        GameStateManager.Instance.SetState(_prevGameState);
        _currentStory = null;
        _canvas.gameObject.SetActive(false);
    }

    ///<summary>
    ///Performs actions based on tags
    ///</summary>
    ///<param name="tags">List of tags</param>
    private void ParseTags(List<string> tags)
    {
        foreach (var t in tags)
        {
            switch (t.Split(':')[0])
            {
                case "name":
                    _nameText.text = t.Split(':')[1];
                    break;
            }
        }
    }

    ///<summary>
    ///Progressively types message
    ///</summary>
    ///<param name="message">The message to type</param>
    private IEnumerator TypeMessage(string message)
    {
        _isTyping = true;
        _messageText.text = "";
        foreach (char c in message)
        {
            _messageText.text += c;
            yield return new WaitForSeconds(1f / _currentTypeSpeed);
        }
        _isTyping = false;
    }
}
