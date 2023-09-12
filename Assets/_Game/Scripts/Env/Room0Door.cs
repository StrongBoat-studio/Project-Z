using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Room0Door : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;
    [SerializeField] private DialogueController _door0;
    [SerializeField] private GameObject _roomLoader;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private int _sceneId = 18;
    private Scene[] _scenes;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    void Update()
    {
        _scenes = SceneManager.GetAllScenes();

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Find: The Isolated Saliva of Subject \"0\"")
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }
    }

    private void OnDialogueEnd()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Find: The Isolated Saliva of Subject \"0\"")
        {
            if(CanLoadScene())
            {
                _roomLoader.SetActive(true);
                Destroy(this.gameObject);
            } 
        }
    }

    private bool CanLoadScene()
    {
        for(int i =0; i<_scenes.Length; i++)
        {
            if(_scenes[i].buildIndex==_sceneId)
            {
                return true;
            }
        }

        return false;
    }

    private void SetActive(bool active)
    {
        _spriteRenderer.enabled = active;
        _boxCollider2D.enabled = active;
    }

    public void CursorClick()
    {
        _door0.Play();
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

    private void OnDestroy()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
    }
}
