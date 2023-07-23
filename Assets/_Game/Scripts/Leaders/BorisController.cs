using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisController : MonoBehaviour
{
    [SerializeField] private List<DialogueController> _dialogueControllers;
    [SerializeField] private Item item;
    private DialogueHolder _dialogueHolder;
    private QuestObjective _questObjective;
    private Animator _animator;
    private Player _player = null;

    private bool giveItem = false;
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
            ChangePosition(new Vector2(2.4f, -0.7f));
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

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Take care of Shimura")
        {
            ChangePosition(new Vector2(-4.8f, -0.7f));
            transform.localScale = new Vector2(-1, 1);
            return;
        }

        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Communicate with Boris")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[1]);
            SetQuestObjective(4, 1); 
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Approach the Greenhouse")
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (giveItem == false)
            {
                _player.AddItem(item);
                giveItem = true;
            }

            if(GameStateManager.Instance.GetCurrentState()==GameStateManager.GameState.Gameplay)
            {
                _animator.SetBool("IsTalking", false);
                _animator.SetBool("IsWalk", true);
                transform.localScale = new Vector2(1, 1);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-8.8f, -0.6f, 0), Time.deltaTime);

                if(transform.position.x==-8.8f)
                {
                    this.gameObject.SetActive(false);
                }
            }
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
