using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueHolder : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;

    [Tooltip("Dialogue holder's ID on scene")]
    public int npcSceneID;
    [SerializeField] private DialogueController _dialogueIdle;
    [SerializeField] private DialogueController _dialogueQuest;

    void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
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
                _dialogueQuest.Play();
            else if (_dialogueIdle != null)
                _dialogueIdle.Play();
            else
                Debug.Log("NPC has no dialogues attached");
        }
    }   

    public void CursorEnter()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
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
}
