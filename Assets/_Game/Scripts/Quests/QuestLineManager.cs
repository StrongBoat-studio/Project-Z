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

    public delegate void QuestUpdateHandler();
    public event QuestUpdateHandler OnQuestUpdate;

    [SerializeField] private List<Quest> _quests;
    public List<Quest> Quests { get => _quests; }
    private bool _isFinised = false;
    public bool IsFinised { get => _isFinised; }

    private void Update()
    {
        //Debugging
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            ValidateQuests();
        }
    }

    public void ValidateQuests()
    {
        bool check = true;
        bool updated = false;

        while (_quests.Count > 0 && check == true)
        {
            bool q = _quests[0].ValidateTasks();
            Debug.Log($"Task validation: {q}");
            if(q == true)
            {
                updated = true;
            }

            if (_quests[0].IsCompleted == true)
            {
                _quests.RemoveAt(0);
                RefreshQuestItems();
                if(_quests.Count <= 0) _isFinised = true;
                updated = true;
            }
            else check = false;
        }

        if(updated == true) 
        {
            OnQuestUpdate?.Invoke();
        }
    }

    private void RefreshQuestItems()
    {
        if (_quests.Count <= 0) return;
        if (_quests[0].Tasks.FindIndex(x => x.TaskType == Quest.TaskType.CollectItems) == -1) return;
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager.Instance is null");
            return;
        }

        //Force update quest items if quest has task of CollecItems type
        CheckQuestItems(GameManager.Instance.player.GetComponent<Player>().GetInventory().Items);
    }

    ///<summary>
    ///Tries to update quest/complete task based on QuestObjective passed
    ///</summary>
    ///<returns>
    ///True if task was completed, false otherwise
    ///</returns>
    public bool CheckQuest(QuestObjective qo)
    {
        if (qo.QuestID != _quests[0].ID) return false;
        bool taskCompleted = _quests[0].CompleteTask(qo.QuestTaskID);
        ValidateQuests();
        return taskCompleted;
    }

    public void CheckJournalQuest(QuestObjective qo, int app)
    {
        if (qo.QuestID != _quests[0].ID) return;
        if ((int)_quests[0].Tasks[0].JournalAppToOpen == app) _quests[0].CompleteTask(qo.QuestTaskID);
        ValidateQuests();
    }

    public void CheckQuestItems(List<Item> items)
    {
        if (_quests.Count <= 0) return;

        for (int i = 0; i < _quests[0].Tasks.Count; i++)
        {
            string completionHint = $"";
            if (_quests[0].Tasks[i].TaskType != Quest.TaskType.CollectItems) continue;

            if (items.FindIndex(x => x.itemType == _quests[0].Tasks[i].ItemToCollect.itemType) == -1)
            {
                //completionHint += $" (0/{_quests[0].Tasks[i].ItemToCollect.amount})";
            }
            else
            {
                Item.ItemType iType = _quests[0].Tasks[i].ItemToCollect.itemType;
                int itemAmount = 0;
                foreach (Item item in items.FindAll(x => x.itemType == iType))
                {
                    itemAmount += item.amount;
                }
                if (itemAmount >= _quests[0].Tasks[i].ItemToCollect.amount) 
                {
                    _quests[0].Tasks[i].Complete();
                    completionHint += $" ({itemAmount}/{_quests[0].Tasks[i].ItemToCollect.amount})";
                }
                else completionHint += $" ({itemAmount}/{_quests[0].Tasks[i].ItemToCollect.amount})";
            }

            _quests[0].Tasks[i].CompletionHint = completionHint;
        }

        ValidateQuests();
    }

}
