using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimuraController : MonoBehaviour
{
    [SerializeField] private List<DialogueController> _dialogueControllers;
    private DialogueHolder _dialogueHolder;
    private Animator _animator;
    private void Awake()
    {
        _dialogueHolder = GetComponentInChildren<DialogueHolder>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestLineManager.Instance.Quests.Count < 1) return;

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Talk to Shimura")
        {
            _dialogueHolder.SetQuestDialogueController(_dialogueControllers[0]);
        }
    }
}
