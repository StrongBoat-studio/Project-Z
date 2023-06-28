using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueController
{
    [SerializeField] private TextAsset _dialogueTextAsset;

    public void Play()
    {
        if(_dialogueTextAsset != null)
            DialogueManager.Instance.EnqueueStory(_dialogueTextAsset);
        else
            Debug.LogWarning("Dialogue is not set");
    }

    public void SetDialogue(TextAsset dialogue)
    {
        _dialogueTextAsset = dialogue;
    }

    public TextAsset GetDialogue()
    {
        return _dialogueTextAsset;
    }
}
