using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMutantSounds : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantIdle, transform.position);
        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantGrowl, transform.position);

        NotesApp noteApp = GameManager.Instance.player.GetComponent<NotesApp>();
        noteApp.AddNote(noteApp.notesRegister[4]);
    }
}
