using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class CameraController : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;

    private GameObject _camera;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            _camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Transform>().GetChild(0).gameObject;
            _camera.SetActive(false);
        }
    }

    public void CursorClick()
    {
        _camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Transform>().GetChild(0).gameObject;
        _camera.SetActive(true);

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Check the Camera Recordings")
        {
            QuestLineManager.Instance.Quests[0].Tasks[0].Complete();
            QuestLineManager.Instance.CheckQuest(_camera.GetComponent<QuestObjective>());
        }
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
