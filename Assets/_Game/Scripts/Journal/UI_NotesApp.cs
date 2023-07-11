using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_NotesApp : MonoBehaviour
{
    [SerializeField] private NotesApp _notesApp;

    [SerializeField] private RectTransform _headers;
    [SerializeField] private RectTransform _headersContainer; 
    [SerializeField] private Transform _noteHeaderPrefab;
    [SerializeField] private Button _backButton;
    [field: SerializeField] public RectTransform Note { get; private set; }

    private void Awake()
    {
        _notesApp.OnNoteAdded += OnNoteAdded;
    }

    private void OnDestroy()
    {
        _notesApp.OnNoteAdded -= OnNoteAdded;
    }

    private void OnNoteAdded()
    {
        RefreshNotes();
    }


    private void RefreshNotes()
    {
        for(int i = 0; i < _headersContainer.childCount; i++)
        {
            Destroy(_headersContainer.GetChild(i).gameObject);
        }

        foreach(Note n in _notesApp.Notes)
        {
            Transform note = Instantiate(_noteHeaderPrefab, _headersContainer);
            note.GetComponent<UI_NoteHeader>().Init(n);
        }
    }

    public void OpenList()
    {
        _headers.gameObject.SetActive(true);
        RefreshNotes();
    }

    public void CloseList()
    {
        _headers.gameObject.SetActive(false);
    }

    public void HideBackButton()
    {
        _backButton.gameObject.SetActive(false);
    }
}
