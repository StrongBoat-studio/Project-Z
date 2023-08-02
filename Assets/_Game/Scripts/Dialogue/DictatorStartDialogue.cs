using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictatorStartDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private DialogueHolder _dialogueHolder;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool _canStart = true;

    private void Update()
    {
        if(GameManager.Instance.movement.IsGrounded() && _canStart && GameStateManager.Instance.GetCurrentState()==GameStateManager.GameState.Gameplay)
        {
            _canStart = false;
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Listen to the speech")
        {
            _dialogueHolder.PlayDialogueQuest();
            QuestLineManager.Instance.Quests[0].Tasks[0].Complete();
            QuestLineManager.Instance.CheckQuest(GetComponent<QuestObjective>());
        }

        Destroy(_dialogueHolder);
        Destroy(this);
    }
}
