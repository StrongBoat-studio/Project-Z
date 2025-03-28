using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisInGreenhouse : MonoBehaviour
{
    private Animator _animator;
    private DialogueHolder _dialogueHolder;

    private void Awake()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
        _dialogueHolder = gameObject.GetComponentInChildren<DialogueHolder>();

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to Boris")
        {
            this.gameObject.SetActive(true);
            _animator.SetBool("IsWalk", false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private bool _canStart = true;

    private void Update()
    {
        if (GameManager.Instance.movement.IsGrounded() && _canStart && GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.Gameplay)
        {
            _canStart = false;
            StartDialogue();
        }

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to Boris")
        {
            this.gameObject.SetActive(true);
            _animator.SetBool("IsWalk", false);
        }

        if (GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.Gameplay && QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Find the source of the gunshot sound")
        {
            Destroy(this.gameObject);
        }
    }

    void StartDialogue()
    {
        _dialogueHolder.PlayDialogueQuest();
        QuestLineManager.Instance.Quests[0].Tasks[0].Complete();
        QuestLineManager.Instance.CheckQuest(GetComponentInChildren<QuestObjective>());
    }
}
