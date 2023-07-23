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
        if(CheckQuest("Press the Button"))
        {
            _dialogueHolder.SetQuestDialogueController(_button);
            SetQuestObjective(2, 1);

            return;
        }

        if(CheckQuest("Turn off the Alarm"))
        {
            _dialogueHolder.SetQuestDialogueController(_alarm);
            SetQuestObjective(3, 3);

            return;
        }
    }

    private bool CheckQuest(string QuestName)
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == QuestName)
        {
            _dialogueHolder.enabled = true;
            _boxCollider2D.enabled = true;
            _questObjective.enabled = true;
            return true;
        }
        else
        {
            _dialogueHolder.enabled = false;
            _boxCollider2D.enabled = false;
            _questObjective.enabled = false;

            return false;
        }
    }
    private void SetQuestObjective(int questId, int taskId)
    {
        _questObjective.QuestID = questId;
        _questObjective.QuestTaskID = taskId;
    }
}
