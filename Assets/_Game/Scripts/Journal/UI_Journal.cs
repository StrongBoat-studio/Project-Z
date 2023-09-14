using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Journal : MonoBehaviour
{
    public enum App
    {
        Inventory = 1,
        Notes = 2,
        Quests = 3,
        Monitor = 4,
        Minimap = 5,
    }

    [SerializeField]
    private RectTransform _backButton;

    [SerializeField]
    private RectTransform _homeScreen;

    [SerializeField]
    private RectTransform _inventoryScreen;

    [SerializeField]
    private RectTransform _notesScreen;

    [SerializeField]
    private RectTransform _questsScreen;

    [SerializeField]
    private RectTransform _lifeMonitor;

    [SerializeField]
    private RectTransform _minimapScreen;

    private Action _backButtonAction = null;
    public int CurrentApp { get; private set; } = 0;

    private Stack<Action> _backButtonHistory = new Stack<Action>();

    public void JournalTabOpened(int app)
    {
        CurrentApp = app;

        SetBackButtonAction(() =>
        {
            _homeScreen.gameObject.SetActive(true);
            _inventoryScreen.gameObject.SetActive(false);
            _notesScreen.gameObject.SetActive(false);
            _questsScreen.gameObject.SetActive(false);
            _lifeMonitor.gameObject.SetActive(false);
            _minimapScreen.gameObject.SetActive(false);
        });

        if (GetComponents<QuestObjective>().Length <= 0)
            return;
        if (QuestLineManager.Instance == null)
        {
            Debug.Log("QuestLineManager Instance is null");
            return;
        }
        foreach (QuestObjective qo in GetComponents<QuestObjective>())
        {
            QuestLineManager.Instance.CheckJournalQuest(qo, app);
        }
    }

    public void SetBackButtonAction(Action btnClickAction)
    {
        Action a = () =>
        {
            btnClickAction();
            if (_backButtonHistory.Count > 0)
            {
                _backButtonHistory.Pop();
            }
            if (_backButtonHistory.Count > 0)
            {
                _backButton.GetComponent<Button>().onClick.RemoveAllListeners();
                _backButton
                    .GetComponent<Button>()
                    .onClick.AddListener(() => _backButtonHistory.Peek()());
            }
            else
                ResetBackButtonStack();
        };

        _backButtonAction = a;

        _backButtonHistory.Push(a);
        _backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _backButton.GetComponent<Button>().onClick.AddListener(() => a());
    }

    public void ResetBackButtonStack()
    {
        _backButtonHistory = new Stack<Action>();
        _backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        CurrentApp = 0;
    }

    public void BackButtonAction()
    {
        _backButtonAction();
    }

    public void OpenMinimapShortcut()
    {
        _homeScreen.gameObject.SetActive(false);
        _inventoryScreen.gameObject.SetActive(false);
        _notesScreen.gameObject.SetActive(false);
        _questsScreen.gameObject.SetActive(false);
        _lifeMonitor.gameObject.SetActive(false);
        _minimapScreen.gameObject.SetActive(true);
    }
}
