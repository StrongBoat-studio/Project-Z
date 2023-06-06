using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public List<QuestTask> Tasks { get; private set; }
    [field: SerializeField] public bool IsCompleted { get; private set; } = false;

    public void ValidateTasks()
    {
        while(Tasks.Count > 0 && Tasks[0].IsCompleted == true) Tasks.RemoveAt(0);
        if(Tasks.Count <= 0) IsCompleted = true;
    }

    public void CompleteTask(int taskID)
    {
        if(Tasks.Count <= 0) return;

        if(Tasks[0].ID != taskID)
        {
            Debug.Log("This isn't the next task");
            return;
        }
        Tasks[0].Complete();
        ValidateTasks();
    }
}
