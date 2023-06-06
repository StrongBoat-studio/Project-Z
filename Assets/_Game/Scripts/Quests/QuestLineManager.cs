using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLineManager : MonoBehaviour
{
    public static QuestLineManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private List<Quest> _quests;

    private void Update()
    {
        //Debugging
        if(Input.GetKeyDown(KeyCode.Keypad7))
        {
            ValidateQuests();
        }

        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            _quests[0].CompleteTask(1);
            ValidateQuests();
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            _quests[0].CompleteTask(2);
            ValidateQuests();
        }
        if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            _quests[0].CompleteTask(3);
            ValidateQuests();
        }
    }

    public void ValidateQuests()
    {
        bool check = true;

        while(_quests.Count > 0 && check == true)
        {
            _quests[0].ValidateTasks();
            if(_quests[0].IsCompleted == true) _quests.RemoveAt(0);
            else check = false;
        }
    }

    public void CheckQuest(QuestObjective qo)
    {
        if(qo.QuestID != _quests[0].ID) return;
        _quests[0].CompleteTask(qo.QuestTaskID);
    }
}
