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
        ValidateQuests();
    }

    public void CheckJournalQuest(QuestObjective qo, int app)
    {
        if(qo.QuestID != _quests[0].ID) return;
        if((int)_quests[0].Tasks[0].JournalAppToOpen == app) _quests[0].CompleteTask(qo.QuestTaskID);
        ValidateQuests();
    }

    public void CheckQuestItems(List<Item> items)
    {
        string completionHint = $"";
        if(items.FindIndex(x => x.itemType == _quests[0].Tasks[0].ItemToCollect.itemType) == -1)
        {
            completionHint += $"{_quests[0].Tasks[0].ItemToCollect.itemType.ToString()} 0/{_quests[0].Tasks[0].ItemToCollect.amount}\n";
            return;
        }

        Item.ItemType iType = _quests[0].Tasks[0].ItemToCollect.itemType;
        int iAmount = 0;
        foreach(Item i in items.FindAll(x => x.itemType == iType))
        {
            iAmount += i.amount;
        }
        if(iAmount >= _quests[0].Tasks[0].ItemToCollect.amount) _quests[0].Tasks[0].Complete();
        else completionHint += $"{_quests[0].Tasks[0].ItemToCollect.itemType.ToString()} {iAmount}/{_quests[0].Tasks[0].ItemToCollect.amount}";

        _quests[0].Tasks[0].CompletionHint = completionHint;
        ValidateQuests();
    }
}
