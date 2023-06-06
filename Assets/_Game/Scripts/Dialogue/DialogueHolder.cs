using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueHolder : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private DialogueController _dialogue;

    void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    public void CursorClick()
    {
        _dialogue.Play();

        //Update quests
        if(GetComponent<QuestObjective>() == null) return;
        if(QuestLineManager.Instance == null) return;
        QuestLineManager.Instance.CheckQuest(GetComponent<QuestObjective>());
    }   

    public void CursorEnter()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }
}
