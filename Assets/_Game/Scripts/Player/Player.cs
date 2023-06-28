using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private int _hp = 100;

    private Inventory _inventory;
    [SerializeField] private RectTransform _uiInventory;

    [SerializeField] DialogueController _lowHP;

    private void Awake()
    {
        GameManager.Instance.player = transform;
        GameManager.Instance.movement = this.GetComponent<Movement>();

        _playerInput = new PlayerInput();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        Debug.Log("Player Start");
        _inventory = new Inventory();

        _uiInventory.GetComponent<UI_Inventory>().SetInventory(_inventory);
        _uiInventory.GetComponent<UI_Inventory>().SetPlayer(transform);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            TakeDamage(20);
        }
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

    private void TakeDamage(int dmg)
    {
        _hp -= dmg;

        if(_hp <= 20)
        {
            _lowHP.Play();
        }
        else if(_hp <= 0)
        {
            Debug.Log("Dead");
        }
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public int GetHP()
    {
        return _hp;
    }

    public void SetHP(int hp)
    {
        _hp = hp;
    }
}
