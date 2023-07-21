using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] DialogueController _button;
    [SerializeField] DialogueController _alarm;

    [SerializeField] private DialogueHolder _dialogueHolder;
    private BoxCollider2D _boxCollider2D;
    private QuestObjective _questObjective;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _questObjective = GetComponent<QuestObjective>();
    }

    void Update()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Press the Button")
        {
            Debug.Log("press");
            _dialogueHolder.SetQuestDialogueController(_button);
            _dialogueHolder.enabled = true;
            _boxCollider2D.enabled = true;

            _questObjective.QuestID = 2;
            _questObjective.QuestTaskID = 1;
            _questObjective.enabled = true;
        }
        else if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Turn off the Alarm")
        {
            _dialogueHolder.SetQuestDialogueController(_alarm);
            _dialogueHolder.enabled = true;
            _boxCollider2D.enabled = true;

            _questObjective.QuestID = 3;
            _questObjective.QuestTaskID = 3;
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
