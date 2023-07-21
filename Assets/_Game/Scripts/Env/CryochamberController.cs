using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryochamberController : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private QuestObjective _questObjective;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _questObjective = GetComponent<QuestObjective>();
    }

    void Update()
    {
        CheckQuest("Re-enter the Cryochamber");
    }

    private void CheckQuest(string QuestName)
    {
        if (QuestLineManager.Instance.Quests.Count < 1) return;

        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == QuestName)
        {
            _boxCollider2D.enabled = true;
            _questObjective.enabled = true;
        }
        else
        {
            _boxCollider2D.enabled = false;
            _questObjective.enabled = false;
        }
    }
}
