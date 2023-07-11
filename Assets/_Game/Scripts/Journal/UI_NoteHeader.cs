using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_NoteHeader : MonoBehaviour
{
    [SerializeField] private Note _note;
    [SerializeField] private TextMeshProUGUI _title;

    private void Start()
    {
        _title.text = _note.noteTitle;
    }

    public void Init(Note note)
    {
        _note = note;
    }

    public void OpenNote()
    {
        UI_NotesApp app = GetComponentInParent<UI_NotesApp>();
        app.Note.GetComponent<UI_Note>().Init(_note);
        app.Note.gameObject.SetActive(true);
        app.CloseList();

        GetComponentInParent<UI_Journal>().SetBackButtonAction(() => {
            app.OpenList();
            app.Note.gameObject.SetActive(false);
        });
    }
}
