using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueHolder : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;

    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;

    [Tooltip("Dialogue holder's ID on scene")]
    public int npcSceneID;
    [SerializeField] private DialogueController _dialogueIdle;
    [SerializeField] private DialogueController _dialogueQuest;
    private Animator _animator;
    private bool _isDialogue;

    void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");

        try
        {
            _animator = GetComponentInChildren<Animator>();
        }
        catch
        {

        }

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _isDialogue = newGameState == GameStateManager.GameState.Dialogue;
    }
    private void Update()
    {
        if(!_isDialogue)
        {
            if(_animator!=null)
                _animator.SetBool("IsTalking", false);
        }
    }

    public void CursorClick()
    {
        //Update quests
        if(GetComponents<QuestObjective>().Length <= 0) return;
        if(QuestLineManager.Instance == null) 
        {
            Debug.Log("QuestLineManager Instance is null");
            return;
        }
        foreach(QuestObjective qo in GetComponents<QuestObjective>())
        {
            bool questTaskCompleted = QuestLineManager.Instance.CheckQuest(qo);

            //Player quest dialogue only if quest was completed
            //Othwerwise play idle dialogue
            if(questTaskCompleted == true && _dialogueQuest != null)
            {
                _dialogueQuest.Play();
                PlayDialogAnimation();
            }
                
            else if (_dialogueIdle != null)
            {
                _dialogueIdle.Play();
                PlayDialogAnimation();
            }
                
            else
                Debug.Log("NPC has no dialogues attached");
        }
    }   

    public void CursorEnter(bool canInteract)
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
        GetComponent<SpriteRenderer>().material.SetColor("_Color", canInteract ? _canInteractColor : _cannotInteractColor);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }

    public void SetDialogues(TextAsset dialogueIdle, TextAsset dialogueQuest = null)
    {
        _dialogueIdle.SetDialogue(dialogueIdle);
        _dialogueQuest.SetDialogue(dialogueQuest);
    }

    public TextAsset GetDialogueIdle()
    {
        return _dialogueIdle.GetDialogue();
    }

    public TextAsset GetDialogueQuest()
    {
        return _dialogueQuest.GetDialogue();
    }
    
    public void PlayDialogueQuest()
    {
        _dialogueQuest.Play();
    }

    public void SetQuestDialogueController(DialogueController dialogue)
    {
        _dialogueQuest = dialogue;
    }

    private void PlayDialogAnimation()
    {
        if (_animator == null) return;

        if (_isDialogue)
        {
            _animator.SetBool("IsTalking", true);
        }
    }
}
