using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesApp : MonoBehaviour
{
    public List<Note> testNotes;
    public List<Note> Notes { get; private set; }

    public delegate void OnNoteAddedHandler();
    public event OnNoteAddedHandler OnNoteAdded;

    private void Start()
    {
        Notes = new List<Note>();

        //Test
        AddNote(testNotes[0]);
        AddNote(testNotes[1]);
        AddNote(testNotes[2]);
        AddNote(testNotes[3]);
    }

    public void AddNote(Note note)
    {
        if (Notes.FindIndex(x => x.id == note.id) != -1)
        {
            Debug.Log($"Note with this id ({note.id}) is already added to player's journal");
            return;
        }

        Notes.Add(note);
        OnNoteAdded?.Invoke();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Notes.Add(testNotes[2]);
            OnNoteAdded?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Notes.RemoveAt(Notes.Count - 1);
            OnNoteAdded?.Invoke();
        }
    }
}
