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
        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title== "Have a conversation with Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[0]);
            _questObjective.QuestID = 1;
            _questObjective.QuestTaskID = 0;
            return;
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[1]);
            _questObjective.QuestID = 1;
            _questObjective.QuestTaskID = 0;
            return;
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Return to Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[2]);
            _questObjective.QuestID = 1;
            _questObjective.QuestTaskID = 2;
            return;
        }
    }
}
