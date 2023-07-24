using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimuraController : MonoBehaviour
{
    [SerializeField] private List<DialogueController> _dialogueControllers;
    [SerializeField] private DialogueHolder _dialogueHolder;
    [SerializeField] private QuestObjective _questObjective;

    // Update is called once per frame

    private void Awake()
    {
        GameManager.Instance.shimura = this.gameObject;
    }

    void Update()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to Shimura")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[0]);
            SetQuestObjective(1, 1);
        }

        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Take care of Shimura")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[1]);
            SetQuestObjective(4, 0);
        }
    }

    private void SetQuestObjective(int questId, int taskId)
    {
        _questObjective.QuestID = questId;
        _questObjective.QuestTaskID = taskId;
    }
}
