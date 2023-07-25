using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictatorStartDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private DialogueHolder _dialogueHolder;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _material;
    private bool _canStart = true;

    private void Update()
    {
        if (GameManager.Instance.movement == null) return;
        if(GameManager.Instance.movement.IsGrounded() && _canStart)
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

        Destroy(this);
        Destroy(_dialogueHolder);
    }
}
