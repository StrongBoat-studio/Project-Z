using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestTask
{
    [System.Serializable]
    public struct QuestItem
    {
        public Item.ItemType itemType;
        public int amount;
    }

    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public bool DisplayCompletionHint { get; private set; }
    [field: SerializeField] public string CompletionHint { get; set; }
    [field: SerializeField] public bool IsCompleted { get; private set; } = false;
    [field: SerializeField] public Quest.TaskType TaskType { get; private set; }

    //Type: CollectItems
    [field: SerializeField] public QuestItem ItemToCollect { get; private set; }

    //Type: Journal
    [field: SerializeField] public UI_Journal.App JournalAppToOpen { get; private set; }

    public void Complete(bool load = false)
    {
        IsCompleted = true;
        if(FMODEvents.Instance != null && load == false)
        {
            AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.QuestFinish, Vector3.zero);
        }
    }
}
