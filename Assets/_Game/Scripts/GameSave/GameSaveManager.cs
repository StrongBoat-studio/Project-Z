using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }

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

    public GameData currentSave;
    private bool _isNewSave = false;

    public void SaveJson()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";

        if (File.Exists(savePath) == false)
        {
            File.Create(savePath).Close();
        }

        //Player data
        GameSaveManager.Instance.currentSave.playerHp = GameManager.Instance.player.GetComponent<Player>().GetHP();
        GameSaveManager.Instance.currentSave.playerStamina = GameManager.Instance.player.GetComponent<Movement>().GetStamina();
        GameSaveManager.Instance.currentSave.playerAmmo = GameManager.Instance.Player.GetAmmo();

        //Notes' ids
        List<int> notes = new List<int>();
        foreach (var note in GameManager.Instance.player.GetComponent<NotesApp>().GetNotes())
        {
            notes.Add(note.id);
        }
        GameSaveManager.Instance.currentSave.journalNotes = notes;

        //Quests
        currentSave.isQuestLineFinised = QuestLineManager.Instance.IsFinised;
        List<GameData.QuestState> quests = new List<GameData.QuestState>();
        foreach (var quest in QuestLineManager.Instance.Quests)
        {
            List<int> tasks = new List<int>();
            foreach (var task in quest.Tasks)
            {
                if (task.IsCompleted == false)
                    tasks.Add(task.ID);
            }
            quests.Add(new GameData.QuestState(quest.ID, tasks));
        }
        GameSaveManager.Instance.currentSave.quests = quests;

        //Inventory items
        List<GameData.InventoryItemState> eqItems = new List<GameData.InventoryItemState>();
        foreach (var item in GameManager.Instance.player.GetComponent<Player>().GetInventory().Items)
        {
            eqItems.Add(new GameData.InventoryItemState(item.itemType, item.amount));
        }
        GameSaveManager.Instance.currentSave.inventoryItems = eqItems;

        //Create json and save
        string json = JsonUtility.ToJson(currentSave);
        File.WriteAllText(savePath, json);
    }

    public void LoadJson()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";

        if (File.Exists(savePath) == false)
        {
            currentSave = new GameData();
            _isNewSave = true;
            return;
        }

        currentSave = JsonUtility.FromJson<GameData>(File.ReadAllText(savePath));
    }

    public void LoadData()
    {
        //Load Inventory
        GameManager.Instance.player.GetComponent<Player>().GetInventory().LoadSave(
            GameSaveManager.Instance.currentSave.inventoryItems
        );
        GameManager.Instance.player.transform.position = currentSave.spawnPosition;

        //Load Quests
        if (currentSave.quests.Count > 0)
        {
            foreach (Quest q in QuestLineManager.Instance.Quests)
            {
                int qIDX = currentSave.quests.FindIndex(x => x.id == q.ID);
                if (qIDX == -1)
                {
                    q.CompleteQuest();
                }
                else
                {
                    foreach (QuestTask qt in q.Tasks)
                    {
                        int qtIDX = currentSave.quests[qIDX].tasks.FindIndex(x => x == qt.ID);
                        if (qtIDX == -1)
                        {
                            qt.Complete();
                        }
                    }
                }
            }
            QuestLineManager.Instance.ValidateQuests();
        }
        else if (currentSave.isQuestLineFinised == true)
        {
            QuestLineManager.Instance.Quests.ForEach(x => x.CompleteQuest());
            QuestLineManager.Instance.ValidateQuests();
        }

        //Load notes
        foreach (int noteID in currentSave.journalNotes)
        {
            NotesApp app = GameManager.Instance.player.GetComponent<NotesApp>();
            app.AddNote(app.notesRegister.Find(x => x.id == noteID));
        }

        //Player data
        if (_isNewSave == false)
        {
            GameManager.Instance.player.GetComponent<Player>().SetHP(currentSave.playerHp);
            GameManager.Instance.player.GetComponent<Movement>().SetStamina(currentSave.playerStamina);
            GameManager.Instance.Player.SetAmmo(currentSave.playerAmmo);
        }
    }
}
