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
        if(GetComponents<QuestObjective>().Length <= 0) return;
        if(QuestLineManager.Instance == null) 
        {
            Debug.Log("QuestLineManager Instance is null");
            return;
        }
        foreach(QuestObjective qo in GetComponents<QuestObjective>())
        {
            QuestLineManager.Instance.CheckQuest(qo);
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
}
