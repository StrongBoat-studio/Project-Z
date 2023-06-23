using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public struct QuestState
    {
        public QuestState(int id, List<int> tasks)
        {
            this.id = id;
            this.tasks = tasks;
        }

        public int id;
        public List<int> tasks;
    }

    [System.Serializable]
    public struct InventoryItemState
    {
        public InventoryItemState(Item.ItemType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public Item.ItemType type;
        public int amount;
    }

    [Header("Level managers")]
    public List<LevelManagerData> levelManagerDatas = new List<LevelManagerData>();

    [Header("Journal")]
    public List<int> journalNotes = new List<int>();

    [Header("Inventory")]
    public List<InventoryItemState> inventoryItems = new List<InventoryItemState>();

    [Header("Player")]
    public int playerHp;
    public float playerStamina;
    public int locationIndex = 8;
    public Vector3 spawnPosition; 

    [Header("Quests")]
    public bool isQuestLineFinised;
    public List<QuestState> quests = new List<QuestState>();
}
