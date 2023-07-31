using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisController : MonoBehaviour
{
    [SerializeField] private List<DialogueController> _dialogueControllers;
    [SerializeField] private Item item;
    [SerializeField] private LeaderController _leaderController;
    private DialogueHolder _dialogueHolder;
    private QuestObjective _questObjective;
    private Animator _animator;
    private Player _player = null;
    private SpriteRenderer _spriteRenderer;
    private Color32 targetColor = new Color32(255, 255, 255, 0);
    private BoxCollider2D _boxCollider2D;

    private bool giveItem = false;
    private float _target;
    private float _current;
    private int count = 0;
    private void Awake()
    {
        GameManager.Instance.boris = this.gameObject;

        if(!GameManager.Instance.ShowBoris())
        {
            this.gameObject.SetActive(false);
        }

        _dialogueHolder = GetComponentInChildren<DialogueHolder>();
        _questObjective = GetComponentInChildren<QuestObjective>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestLineManager.Instance.Quests.Count < 1) return;

        CheckQuestAndSetComponents("Have a conversation with Boris", true, new Vector2(2.4f, -0.7f), 0, 1, 0);

        CheckQuestAndSetComponents("Talk to Boris", false, transform.position, 1, 2, 0);

        CheckQuestAndSetComponents("Return to Boris", false, transform.position, 2, 2, 2);

        CheckQuestAndSetComponents("Talk to the Crew", true, new Vector2(-1.5f, -0.7f), 0, 3, 2);

        CheckQuestAndSetComponents("Communicate with Boris", false, transform.position, 1, 4, 1);

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Take care of Shimura")
        {
            ChangePosition(new Vector2(-4.8f, -0.7f));
            transform.localScale = new Vector2(-1, 1);
            return;
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Communicate with Boris")
        {
            _leaderController.enabled = false;
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Approach the Greenhouse")
        {

            GiveKnife();

            if(GameStateManager.Instance.GetCurrentState()==GameStateManager.GameState.Gameplay)
            {
                PlayWalkingAnimation();
                transform.localScale = new Vector2(1, 1);

                TransparencyAnimation();
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-8.8f, -0.6f, 0), Time.deltaTime);

                EnabledComponents();

                Hide();
            }
        }
    }

    private void CheckQuestAndSetComponents(string questName, bool isChangingPosition, Vector2 targetPosition, int dialogueHolderId, int questID, int taskID)
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == questName)
        {
            if(isChangingPosition)
            {
                ChangePosition(targetPosition);
            }
            
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[dialogueHolderId]);
            SetQuestObjective(questID, taskID);
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

    private void PlayWalkingAnimation()
    {
        _animator.SetBool("IsTalking", false);
        _animator.SetBool("IsWalk", true);
    }

    private void TransparencyAnimation()
    {
        if (count == 10) count = 0;
        if (count == 0 && transform.position.x < -6.0f)
        {
            _target = _target == 0 ? 1 : 0;
            _current = Mathf.MoveTowards(_current, _target, 0.5f * Time.deltaTime);
            _spriteRenderer.color = Color32.Lerp(_spriteRenderer.color, targetColor, _current);
        }
        count++;
    }

    private void EnabledComponents()
    {
        _dialogueHolder.enabled = false;
        _questObjective.enabled = false;
        _boxCollider2D.enabled = false;
    }

    private void Hide()
    {
        if (transform.position.x == -8.8f)
        {
            this.gameObject.SetActive(false);
            _leaderController.enabled = true;
        }
    }

    private void GiveKnife()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (giveItem == false)
        {
            _player.AddItem(item);
            giveItem = true;
        }
    }

    private void OnDestroy()
    {
        
    }
}
