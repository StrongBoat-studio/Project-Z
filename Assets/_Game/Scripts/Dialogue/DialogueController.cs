using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueController
{
    [SerializeField] private TextAsset _dialogueTextAsset;

    public void Play()
    {
        DialogueManager.Instance.EnqueueStory(_dialogueTextAsset);
    }
}
