using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisController : MonoBehaviour
{
    [SerializeField] private List<LeaderDialogue> _dialogueControllers;
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
    public bool _canFight = false;

    //Variables for fight
    private Transform _targetPlayer;

    //DotPro
    public float dotPro;
    private Vector2 _directionDot;
    private Vector2 _checkVector;

    private void Awake()
    {
        _targetPlayer = GameManager.Instance.player;

        if(!GameManager.Instance.ShowBoris())
        {
            this.gameObject.SetActive(false);
        }

        _dialogueHolder = GetComponentInChildren<DialogueHolder>();
        _questObjective = GetComponentInChildren<QuestObjective>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _boxCollider2D = GetComponentInChildren<BoxCollider2D>();

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
            DialogueManager.Instance.OnDialogueStart += OnDialogueStart;
        }
            
    }

    private void OnDialogueEnd()
    {
        if (_canFight == true && QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Argue with Boris")
        {
            _animator.SetBool("IsSitting", false);
            _animator.SetBool("IsStand", true);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnDialogueStart()
    {
        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Argue with Boris")
        {
            _canFight = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<LeaderController>().canShowBoris == false) return;
        
        if (QuestLineManager.Instance.Quests.Count < 1) return;

        CheckQuestAndSetComponents("Have a conversation with Boris", true, new Vector2(2.4f, -0.7f), 1, 0);
        CheckQuestAndSetComponents("Talk to Shimura", false, transform.position, 1, 0);
        CheckQuestAndSetComponents("Open the Mission Log", false, transform.position, 1, 0);
        CheckQuestAndSetComponents("Talk to Boris", false, transform.position, 2, 0);
        CheckQuestAndSetComponents("Press the Button", false, transform.position, 2, 0);
        CheckQuestAndSetComponents("Return to Boris", false, transform.position, 2, 2);
        CheckQuestAndSetComponents("Re-enter the Cryochamber", false, transform.position, 2, 2);
        CheckQuestAndSetComponents("Monitor the Mutation Process", false, transform.position, 2, 2);
        CheckQuestAndSetComponents("Talk to the Crew", true, new Vector2(-1.5f, -0.7f), 3, 2);
        CheckQuestAndSetComponents("Turn off the Alarm", false, transform.position, 3, 2);
        CheckQuestAndSetComponents("Wait in the Cryochamber", false, transform.position, 3, 2);
        CheckQuestAndSetComponents("Take care of Shimura", false, transform.position, 3, 2);
        CheckQuestAndSetComponents("Communicate with Boris", false, transform.position, 4, 1);
        CheckQuestAndSetComponents("Approach the Greenhouse", false, transform.position, 4, 2);
        CheckQuestAndSetComponents("Search the body", false, transform.position, 0, 0);
        CheckQuestAndSetComponents("Argue with Boris", false, transform.position, 0, 0);

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Take care of Shimura")
        {
            ChangePosition(new Vector2(-4.8f, -0.7f));
            transform.localScale = new Vector2(-1, 1);
            this.gameObject.transform.GetChild(1).localScale = new Vector2(-1, 1);
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
                this.gameObject.transform.GetChild(1).localScale = new Vector2(1, 1);

                TransparencyAnimation();
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-8.8f, -0.6f, 0), Time.deltaTime);

                EnabledComponents();

                Hide();
            }
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Argue with Boris" || QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Find: The gunpowder")
        {
            CalculateDotPro();
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Search the body" || QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Argue with Boris")
        {
            if (_canFight == false)
            {
                _boxCollider2D.offset = new Vector2(0.85f, _boxCollider2D.offset.y);
                _animator.SetBool("IsSitting", true);
                this.gameObject.transform.GetChild(1).localPosition = new Vector2(0.88f, 0);
            }
        }
    }

    private void CheckQuestAndSetComponents(string questName, bool isChangingPosition, Vector2 targetPosition, int questID, int taskID)
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title != questName) return;

        foreach(LeaderDialogue leaderDialogue in _dialogueControllers)
        {
            if(leaderDialogue.questName.Equals(QuestLineManager.Instance.Quests[0].Tasks[0].Title))
            {
                SetQuestObjective(questID, taskID);

                if (isChangingPosition)
                {
                    ChangePosition(targetPosition);
                }

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

    private void CalculateDotPro()
    {
        if (_targetPlayer == null) return;

        _directionDot = _targetPlayer.position - transform.position;
        _directionDot.Normalize();

        dotPro = Vector2.Dot(_directionDot, CalculateCheckVector());
    }

    private Vector2 CalculateCheckVector()
    {
        return _checkVector = new Vector2(transform.localScale.x, .0f);
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
        DialogueManager.Instance.OnDialogueStart -= OnDialogueStart;
    }
}
