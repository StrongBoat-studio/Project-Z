using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisController : MonoBehaviour
{
    [SerializeField] private List<DialogueController> _dialogueControllers;
    private DialogueHolder _dialogueHolder;
    private QuestObjective _questObjective;
    private Animator _animator;

    private void Awake()
    {
        _dialogueHolder = GetComponentInChildren<DialogueHolder>();
        _questObjective = GetComponentInChildren<QuestObjective>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestLineManager.Instance.Quests.Count < 1) return;

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title== "Have a conversation with Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[0]);
            SetQuestObjective(1, 0);
            return;
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[1]);
            SetQuestObjective(2, 0);
            return;
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Return to Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[2]);
            SetQuestObjective(2, 2);
            return;
        }

        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to the Crew")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[0]);
            SetQuestObjective(3, 2);
            ChangePosition(new Vector2(-1.5f, -0.7f));
            return;
        }
    }

    private void SetQuestObjective(int questId, int taskId)
    {
        _questObjective.QuestID = questId;
        _questObjective.QuestTaskID = taskId;
    }
    private void ChangePosition(Vector2 vector)
    {
        transform.position = vector;
    }
}
