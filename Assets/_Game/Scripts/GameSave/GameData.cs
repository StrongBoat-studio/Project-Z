using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public struct QuestState
    {
        public QuestState(int id, int[] tasks)
        {
            this.id = id;
            this.tasks = tasks;
        }

        public int id;
        public int[] tasks;
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
    public LevelManager[] levelManagers;

    [Header("Journal")]
    public int[] journalNotes;

    [Header("Inventory")]
    public InventoryItemState[] inventoryItems;

    [Header("Player")]
    public int playerHp;

    [Header("Quests")]
    public QuestState[] quests;
}
