using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueController _searchTheBody;

    private BoxCollider2D _boxCollider2D;

    bool _isDialoguePlay = false;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Search the body")
        {
            _boxCollider2D.enabled = true;
        }
        else
        {
            _boxCollider2D.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isDialoguePlay==false)
        {
            _searchTheBody.Play();
            _isDialoguePlay = true;
            NotesApp noteApp = GameManager.Instance.player.GetComponent<NotesApp>();
            noteApp.AddNote(noteApp.notesRegister[5]);
        }
        
        Destroy(this.gameObject);
    }
}
