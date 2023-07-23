using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryochamberController : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private QuestObjective _questObjective;
    private RoomLoader _roomLoader;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _questObjective = GetComponent<QuestObjective>();
        _roomLoader = GetComponent<RoomLoader>();
    }

    void Update()
    {
        if(CheckQuest("Re-enter the Cryochamber"))
        {
            SetScene(SceneRegister.Scenes.BlackScreen1);
            SetQuestObjective(2, 3);
            return;
        }

        if(CheckQuest("Wait in the Cryochamber"))
        {
            SetScene(SceneRegister.Scenes.BlackScreen2);
            SetQuestObjective(3, 4);
            return;
        }
    }

    private bool CheckQuest(string QuestName)
    {
        if (QuestLineManager.Instance.Quests.Count < 1) return false;

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == QuestName)
        {
            _boxCollider2D.enabled = true;
            _questObjective.enabled = true;
            return true;
        }
        else
        {
            _boxCollider2D.enabled = false;
            _questObjective.enabled = false;

            return false;
        }
    }

    private void SetScene(SceneRegister.Scenes targetScene)
    {
        _roomLoader.SetTargetScene(targetScene);
    }

    private void SetQuestObjective(int questId, int taskId)
    {
        _questObjective.QuestID = questId;
        _questObjective.QuestTaskID = taskId;
    }
}
