using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room0Door : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private RoomLoader _roomLoader;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _roomLoader = GetComponent<RoomLoader>();
    }

    void Update()
    {
        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Find: The Isolated Saliva of Subject “0”")
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    private void SetActive(bool active)
    {
        _spriteRenderer.enabled = active;
        _boxCollider2D.enabled = active;
        _roomLoader.enabled = active;
    }
}
