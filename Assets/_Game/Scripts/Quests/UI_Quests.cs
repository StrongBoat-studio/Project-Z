using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Quests : MonoBehaviour
{
    [SerializeField]
    private RectTransform _questTitle;

    [SerializeField]
    private RectTransform _questTaskContainer;

    [SerializeField]
    private RectTransform _questTaskPrefab;

    [SerializeField]
    private RectTransform _questTaskPrefabLast;

    private void Awake()
    {
        if (QuestLineManager.Instance == null)
        {
            Debug.Log("QusetLineManager is null");
            return;
        }

        QuestLineManager.Instance.OnQuestUpdate += OnQuestUpdate;
    }

    private void OnDestroy()
    {
        QuestLineManager.Instance.OnQuestUpdate -= OnQuestUpdate;
    }

    public void RefreshQuests()
    {
        for (int i = 0; i < _questTaskContainer.childCount; i++)
        {
            Destroy(_questTaskContainer.GetChild(i).gameObject);
        }

        if (QuestLineManager.Instance.Quests.Count > 0)
        {
            Quest currentQuest = QuestLineManager.Instance.Quests[0];
            _questTitle.GetComponent<TextMeshProUGUI>().text = currentQuest.Title;
            for (int i = 0; i < currentQuest.Tasks.Count; i++)
            {
                RectTransform quest = null;
                if (i != currentQuest.Tasks.Count - 1)
                {
                    quest = Instantiate(_questTaskPrefab, _questTaskContainer);
                }
                else
                {
                    quest = Instantiate(_questTaskPrefabLast, _questTaskContainer);
                }

                string taskText = currentQuest.Tasks[i].Title;
                taskText += /*(currentQuest.Tasks[i].DisplayCompletionHint == true) ?*/
                    " " + currentQuest.Tasks[i].CompletionHint; // : "";
                quest.GetComponentInChildren<TextMeshProUGUI>().text = taskText;
            }
        }
        else
        {
            _questTitle.GetComponent<TextMeshProUGUI>().text = "No tasks available";
        }

        //Hard coded, can't find issue in UI
        _questTaskContainer.GetComponent<VerticalLayoutGroup>().spacing = -3;
    }

    private void OnQuestUpdate()
    {
        RefreshQuests();
    }
}
