using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueHolder : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private TextAsset _dialogueTextAsset;

    void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    public void CursorClick()
    {
        DialogueManager.Instance.StartStory(_dialogueTextAsset);
    }

    public void CursorEnter()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }
}
