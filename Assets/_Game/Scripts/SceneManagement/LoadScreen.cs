using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _completionBar;
    [SerializeField] private TextMeshProUGUI _completionText;

    public IEnumerator ShowScreen()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Loading);
        _completionBar.fillAmount = 0f;
        _completionText.text = "0%";
        _canvasGroup.blocksRaycasts = true;
        yield return _canvasGroup.DOFade(1f, .5f).WaitForCompletion();
    }

    public void UpdateProgress(float completionPercent)
    {
        _completionBar.fillAmount = completionPercent / 100f;
        _completionText.text = $"{completionPercent}%";
    }

    public IEnumerator HideScreen()
    {
        yield return _canvasGroup.DOFade(0f, .5f).WaitForCompletion();
        _canvasGroup.blocksRaycasts = false;
        GameStateManager.Instance.ResetLastState();
        yield break;
    }
}
