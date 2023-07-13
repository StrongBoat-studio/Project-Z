using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private DialogueHolder _dialogueHolder;
    private BoxCollider2D _boxCollider2D;
    private QuestObjective _questObjective;

    private void Awake()
    {
        _dialogueHolder = GetComponent<DialogueHolder>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _questObjective = GetComponent<QuestObjective>();
    }

    void Update()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Press the Button")
        {
            _dialogueHolder.enabled = true;
            _boxCollider2D.enabled = true;
            _questObjective.enabled = true;
        }
        else
        {
            _dialogueHolder.enabled = false;
            _boxCollider2D.enabled = false;
            _questObjective.enabled = false;
        }
    }
}
