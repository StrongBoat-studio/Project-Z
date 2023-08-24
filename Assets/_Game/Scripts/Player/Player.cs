using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    public int _helathMax = 100;
    public int _currentHelath = 100;
    [SerializeField] private Image _playerHealthBar;
    
    private Inventory _inventory;
    [SerializeField] private RectTransform _uiInventory;

    [SerializeField] private DialogueController _lowHP;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        GameManager.Instance.player = transform;
        GameManager.Instance.movement = this.GetComponent<Movement>();
        GameManager.Instance.Player = this;

        _playerInput = new PlayerInput();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        GameManager.Instance.player = transform;

        Debug.Log("Player Start");
        _inventory = new Inventory();

        _uiInventory.GetComponent<UI_Inventory>().SetInventory(_inventory);
        _uiInventory.GetComponent<UI_Inventory>().SetPlayer(transform);
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null) GameManager.Instance.player = null;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        Debug.Log("Player Destroy");
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        
    }

    public void TakeDamage(int dmg)
    {
        if (GameManager.Instance.movement.canMove)
        {
            _currentHelath -= dmg;

            _playerHealthBar.fillAmount = Mathf.Clamp(_playerHealthBar.fillAmount - dmg * 0.01f, 0f, 1f);

            if (_currentHelath <= 20 && _currentHelath > 0)
            {
                _lowHP.Play();
            }
            else if (_currentHelath <= 0)
            {
                GameManager.Instance.showBoris = false;
                SceneRegister.Instance.LoadNextLevel(GameObject.FindGameObjectWithTag("LeaderRoomLoader").GetComponent<RoomLoader>());
                _currentHelath = _helathMax;
                _playerHealthBar.fillAmount = 1.0f;
            }
        }
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public int GetHP()
    {
        return _currentHelath;
    }

    public void SetHP(int health)
    {
        _currentHelath = health;
    }

    public void AddItem(Item item)
    {
        _inventory.AddItem(item);
    }

    public void SetVCamConfiner(GameObject colliderObject)
    {
        _virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = colliderObject.GetComponent<PolygonCollider2D>();
    }
}
