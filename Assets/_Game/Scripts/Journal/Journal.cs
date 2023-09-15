using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Journal : MonoBehaviour
{
    private PlayerInput _playerInput;
    public bool IsOpen { get; private set; } = false;

    [SerializeField]
    private RectTransform _journal;

    [SerializeField]
    private RectTransform _lifeMonitor;

    [SerializeField]
    private RectTransform _minimapScreen;

    [SerializeField]
    private RectTransform _dictatorScreen;

    [SerializeField]
    private RectTransform _minimapText;
    private float _minimapTextStartX;
    private float _minimapTextEndX;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Journal.Enable();
        _playerInput.Journal.Open.performed += OnJournalOpen;
        _playerInput.Journal.ExclusiveClose.performed += OnJournalExculusiveClose;
        _playerInput.Journal.OpenMinimap.performed += OnMinimapOpen;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        _minimapTextStartX = _minimapText.anchoredPosition.x;
        _minimapTextEndX = -_minimapText.anchoredPosition.x;
    }

    private void OnDestroy()
    {
        _playerInput.Journal.Open.performed -= OnJournalOpen;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnJournalOpen(InputAction.CallbackContext context)
    {
        if (IsOpen == true)
            GameStateManager.Instance.ResetLastState();
        else
            GameStateManager.Instance.SetState(GameStateManager.GameState.Journal);

        if ((QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Open the Mission Log"))
        {
            _dictatorScreen.gameObject.SetActive(true);
        }
        else
        {
            _dictatorScreen.gameObject.SetActive(false);
        }
    }

    private void OnMinimapOpen(InputAction.CallbackContext context)
    {
        UI_Journal uij = _journal.GetComponent<UI_Journal>();

        if(IsOpen == true && uij.CurrentApp == (int)UI_Journal.App.Minimap)
        {
            uij.BackButtonAction();
            GameStateManager.Instance.ResetLastState();
        }
        else if(IsOpen == true && uij.CurrentApp != (int)UI_Journal.App.Minimap)
        {
            uij.JournalTabOpened((int)UI_Journal.App.Minimap);
            uij.OpenMinimapShortcut();
        }
        else
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Journal);
            uij.JournalTabOpened((int)UI_Journal.App.Minimap);
            uij.OpenMinimapShortcut();
        }
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.Journal && IsOpen == false)
        {
            _journal
                .DOAnchorPosX(0f, .25f, true)
                .OnComplete(() =>
                {
                    _minimapText
                        .DOAnchorPosX(_minimapTextEndX, .25f, true)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            _minimapText
                                .DOAnchorPosX(_minimapTextStartX, .25f, true)
                                .SetDelay(1f)
                                .SetEase(Ease.InOutSine);
                        });
                });

            IsOpen = true;

            if (_lifeMonitor.GetComponent<UI_LifeMonitor>().gameObject.activeSelf)
            {
                _lifeMonitor.GetComponent<UI_LifeMonitor>().StartAudio();
            }

            //Update minimap marker
            _minimapScreen.GetComponent<UI_Minimap>().UpdateMarker();
        }
        else if (newGameState != GameStateManager.GameState.Journal && IsOpen == true)
        {
            _journal.DOAnchorPosX(
                -_journal.rect.width
                    - (
                        _journal
                            .GetComponentInParent<Canvas>()
                            .gameObject.GetComponent<RectTransform>()
                            .rect.width - _journal.rect.width
                    ) / 2,
                .25f,
                true
            ).OnComplete(() => {
                _minimapText.DOComplete();
            });
            IsOpen = false;

            if (_lifeMonitor.GetComponent<UI_LifeMonitor>().gameObject.activeSelf)
            {
                _lifeMonitor.GetComponent<UI_LifeMonitor>().StopAudio();
            }
        }

        if (
            newGameState == GameStateManager.GameState.Paused
            || newGameState == GameStateManager.GameState.Crafting
            || newGameState == GameStateManager.GameState.Dialogue
            || newGameState == GameStateManager.GameState.Loading
        )
        {
            _playerInput.Journal.Disable();
        }
        else
            _playerInput.Journal.Enable();
    }

    private void OnJournalExculusiveClose(InputAction.CallbackContext context)
    {
        if (IsOpen == true)
            GameStateManager.Instance.ResetLastState();
    }
}
