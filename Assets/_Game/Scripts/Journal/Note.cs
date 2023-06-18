using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Note", menuName = "ScriptableObjects/Note")]
public class Note : ScriptableObject
{
    public int id;
    public string noteTitle;
    public TextAsset noteText;
}
