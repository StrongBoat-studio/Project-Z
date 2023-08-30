using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using DG.Tweening;
using TMPro;

public class UI_QuestOverlay : MonoBehaviour
{
    [SerializeField]
    private RectTransform _questContainer;

    [SerializeField]
    private RectTransform _questTitle;

    [SerializeField]
    private RectTransform _questTaskTitle;

    void Start()
    {
        if (QuestLineManager.Instance == null)
        {
            Debug.Log("QusetLineManager is null");
            return;
        }

        QuestLineManager.Instance.OnQuestUpdate += OnQuestUpdate;
    }

    void Update() { }

    private void OnDestroy()
    {
        QuestLineManager.Instance.OnQuestUpdate -= OnQuestUpdate;
    }

    private void OnQuestUpdate()
    {
        StartCoroutine(UpdateQuestVisuals());
    }

    private IEnumerator UpdateQuestVisuals()
    {
        yield return _questContainer.GetComponent<CanvasGroup>().DOFade(0f, .3f).SetEase(Ease.InOutSine).WaitForCompletion();
        _questTitle.GetComponent<TextMeshProUGUI>().text = QuestLineManager.Instance?.Quests[0].Title;
        _questTaskTitle.GetComponent<TextMeshProUGUI>().text = QuestLineManager.Instance?.Quests[0].Tasks[0].Title;
        yield return _questContainer.GetComponent<CanvasGroup>().DOFade(1f, .3f).SetEase(Ease.InOutSine).WaitForCompletion();
    }
}
