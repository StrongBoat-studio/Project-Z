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

        _storyQueue = new Queue<Story>();
        _playerInput = new PlayerInput();
        _playerInput.Dialogue.Continue.performed += OnDialogueContinue;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        _playerInput.Dialogue.Continue.performed -= OnDialogueContinue;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private GameStateManager.GameState _prevGameState;
    private Queue<Story> _storyQueue;
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
    ///Starts dialogue if any other dialogue isn't playing, enables dialogue UI and changes game state
    ///If there is a dialogue playing, it adds it to queue
    ///</summary>
    ///<param name="storyTextAsset">Ink's story text asset</param>
    public void EnqueueStory(TextAsset storyTextAsset)
    {
        if (_storyQueue.Count <= 0 && _currentStory == null)
        {
            EnterDialogueMode();
            _currentStory = new Story(storyTextAsset.text);
            ContinueStory();
            Debug.Log("Playing story!");
        }
        else
        {
            _storyQueue.Enqueue(new Story(storyTextAsset.text));
            Debug.Log("Enqueued story!");
        }
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
    ///Ends story
    ///if there are any dialogues enqueued, starts a new dialogue, 
    ///if not, exits dialogue mode
    ///</summary>
    private void EndStory()
    {
        if (_storyQueue.Count > 0)
        {
            _currentStory = _storyQueue.Dequeue();
            ContinueStory();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    ///<summary>
    ///Sets game state to Dialogue and enables dialogue UI
    ///</summary>
    private void EnterDialogueMode()
    {
        _prevGameState = GameStateManager.Instance.GetState();
        GameStateManager.Instance.SetState(GameStateManager.GameState.Dialogue);
        _canvas.gameObject.SetActive(true);
    }

    ///<summary>
    ///Sets back recent gamestate and disables dialogue UI
    ///</summary>
    private void ExitDialogueMode()
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
                case "item":
                    if (GameManager.Instance.player == null)
                    {
                        Debug.Log("Player not set in GameManager singleton.");
                        break;
                    }

                    string itemTypeString = t.Split(':')[1].Split(',')[0];
                    int itemAmount = int.Parse(t.Split(':')[1].Split(',')[1]);
                    foreach (Item.ItemType type in System.Enum.GetValues(typeof(Item.ItemType)))
                    {
                        if (type.ToString() != itemTypeString) continue;
                        Player player = GameManager.Instance.player.GetComponent<Player>();
                        
                        if(ItemRegister.Instance.items.Find(x => x.itemType == type).stackable)
                        {
                            Item item = ItemRegister.Instance.GetNewItem(type);
                            item.amount = itemAmount;
                            if(!player.GetInventory().AddItem(item))
                            {
                                Debug.Log("Dialoge gave no items! Inventory is full!");
                            }
                        }
                        else
                        {
                            for(int i = 0; i < itemAmount; i++)
                            {
                                Item item = ItemRegister.Instance.GetNewItem(type);
                                item.amount = 1;
                                if(!player.GetInventory().AddItem(item))
                                {
                                    Debug.Log($"Dialogue gave {i+1} items out of {itemAmount}. Inventory is full");
                                }
                            }
                        }
                    }
                    break;
                default:
                    Debug.Log("Unrecognized event: " + t);
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
