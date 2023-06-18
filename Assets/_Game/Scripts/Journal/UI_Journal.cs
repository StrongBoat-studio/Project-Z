using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Journal : MonoBehaviour
{
    public enum App 
    {
        Inventory = 1,
        Notes = 2,
        Quests = 3,
        Monitor = 4
    }

    public void JournalTabOpened(int app)
    {
        if(GetComponents<QuestObjective>().Length <= 0) return;
        if(QuestLineManager.Instance == null) 
        {
            Debug.Log("QuestLineManager Instance is null");
            return;
        }
        foreach(QuestObjective qo in GetComponents<QuestObjective>())
        {
            QuestLineManager.Instance.CheckJournalQuest(qo, app);
        }
    }
}
