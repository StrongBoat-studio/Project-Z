using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimuraController : MonoBehaviour
{
    [SerializeField] private List<LeaderDialogue> _dialogueControllers;
    [SerializeField] private DialogueHolder _dialogueHolder;
    [SerializeField] private QuestObjective _questObjective;
    [SerializeField] private List<string> _crewDialogues;

    private void Awake()
    {
        this.gameObject.SetActive(GameManager.Instance.ShowShimura());

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
            DialogueManager.Instance.OnDialogueStart += OnDialogueStart;
        }
    }

    private void OnDialogueEnd()
    {
        this.gameObject.GetComponentInChildren<Animator>().SetBool("IsTalking", false);
    }

    private void OnDialogueStart()
    {
        if (_crewDialogues.Contains(QuestLineManager.Instance.Quests[0].Tasks[0].Title))
        {
            this.gameObject.GetComponentInChildren<Animator>().SetBool("IsTalking", true);
        }
    }

    void Update()
    {
        CheckQuestAndSetComponents("Have a conversation with Boris", 1, 1);
        CheckQuestAndSetComponents("Talk to Shimura", 1, 1);
        CheckQuestAndSetComponents("Open the Mission Log", 1, 1);
        CheckQuestAndSetComponents("Talk to Boris", 1, 1);
        CheckQuestAndSetComponents("Press the Button", 1, 1);
        CheckQuestAndSetComponents("Return to Boris", 1, 1);
        CheckQuestAndSetComponents("Re-enter the Cryochamber", 1, 1);
        CheckQuestAndSetComponents("Monitor the Mutation Process", 1, 1);
        CheckQuestAndSetComponents("Take care of Shimura", 4, 0);
        CheckQuestAndSetComponents("Communicate with Boris", 4, 0);
        CheckQuestAndSetComponents("Talk to the Crew", 3, 2);
        CheckQuestAndSetComponents("Turn off the Alarm", 3, 2);
        CheckQuestAndSetComponents("Wait in the Cryochamber", 3, 2);


        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Find the source of the gunshot sound")
        {
            GameManager.Instance.showShimura = false;
        }
    }

    private void CheckQuestAndSetComponents(string questName, int questID, int taskID)
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title != questName) return;

        foreach (LeaderDialogue leaderDialogue in _dialogueControllers)
        {
            if (leaderDialogue.questName.Equals(QuestLineManager.Instance.Quests[0].Tasks[0].Title))
            {
                SetQuestObjective(questID, taskID);

                _dialogueHolder.SetQuestDialogueController(leaderDialogue.questDialogue);
                _dialogueHolder.SetIdleDialogueController(leaderDialogue.idleDialogue);

                return;
            }
        }
    }

    private void SetQuestObjective(int questId, int taskId)
    {
        _questObjective.QuestID = questId;
        _questObjective.QuestTaskID = taskId;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
        DialogueManager.Instance.OnDialogueStart -= OnDialogueStart;
    }
}
