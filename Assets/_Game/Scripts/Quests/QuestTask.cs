using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestTask
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public bool IsCompleted { get; private set; } = false;

    public void Complete()
    {
        IsCompleted = true;
    }
}
