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
    public int _numberOfAmmo = 7;
    [SerializeField] private Image _playerHealthBar;
    [SerializeField] private Image _playerAmmoBar;

    private Inventory _inventory;
    [SerializeField] private RectTransform _uiInventory;

    [SerializeField] private DialogueController _lowHP;
    [SerializeField] private DialogueController _argueWithBoris;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        GameManager.Instance.player = transform;
        GameManager.Instance.movement = this.GetComponent<Movement>();
        GameManager.Instance.Player = this;

        _playerInput = new PlayerInput();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        SetFillAmountForAmmo();
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
                //_lowHP.Play();
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

    public void StartArgueWithBorisDialogue()
    {
        _argueWithBoris.Play();
    }

    public void AmmoDown()
    {
        _numberOfAmmo -= 1;
        SetFillAmountForAmmo();
    }

    private void SetFillAmountForAmmo()
    {
        switch (_numberOfAmmo)
        {
            case 7:
                _playerAmmoBar.fillAmount = 1;
                break;

            case 6:
                _playerAmmoBar.fillAmount = 0.9f;
                break;

            case 5:
                _playerAmmoBar.fillAmount = 0.7f;
                break;

            case 4:
                _playerAmmoBar.fillAmount = 0.6f;
                break;

            case 3:
                _playerAmmoBar.fillAmount = 0.4f;
                break;

            case 2:
                _playerAmmoBar.fillAmount = 0.3f;
                break;

            case 1:
                _playerAmmoBar.fillAmount = 0.15f;
                break;

            case 0:
                _playerAmmoBar.fillAmount = 0f;
                break;
        }
    }

    public int GetAmmo()
    {
        return _numberOfAmmo;
    }

    public void SetAmmo(int ammo)
    {
        _numberOfAmmo = ammo;
        SetFillAmountForAmmo();
    }
}
