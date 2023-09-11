using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class JammedDoor : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;
    [SerializeField] private DialogueController _door;
    [SerializeField] private GameObject _doorObj;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    private void Update()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Return to the Lab")
        {
            _doorObj.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    public void CursorClick()
    {
        _door.Play();
    }

    public void CursorEnter(bool canInteract)
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
        GetComponent<SpriteRenderer>().material.SetColor("_Color", canInteract ? _canInteractColor : _cannotInteractColor);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }
}
