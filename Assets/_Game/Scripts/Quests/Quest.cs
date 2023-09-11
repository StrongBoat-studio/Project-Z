using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public enum TaskType
    {
        Talk,
        GoToLocation,
        OpenJournal,
        CollectItems
    }

    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public List<QuestTask> Tasks { get; private set; }
    [field: SerializeField] public bool IsCompleted { get; private set; } = false;

    public bool ValidateTasks()
    {
        bool updated = false;

        while (Tasks.Count > 0 && Tasks[0].IsCompleted == true) 
        {
            Tasks.RemoveAt(0);
            updated = true;
        }
        if (Tasks.Count <= 0) 
        {
            IsCompleted = true;
            updated = true;
        }

        return updated;
    }

    ///<summary>
    ///Tries to complete task with given ID
    ///</summary>
    ///<returns>
    ///True if task with given ID was completed, false otherwise
    ///</returns>
    public bool CompleteTask(int taskID)
    {
        if (Tasks.Count <= 0) return false;

        if (Tasks[0].ID != taskID)
        {
            Debug.Log("This isn't the next task");
            return false;
        }
        Tasks[0].Complete();
        //ValidateTasks();
        return true;
    }
    
    public void CompleteQuest(bool load = false)
    {
        IsCompleted = true;
        if(FMODEvents.Instance != null && load == false)
        {
            AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.QuestFinish, Vector3.zero);
        }
    }
}
