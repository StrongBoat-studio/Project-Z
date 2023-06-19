using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] private List<Sprite> _playerHandsSpritesList;
    private Dictionary<int, Sprite> _playerHandsSpritesDictiorary = new Dictionary<int, Sprite>();

    private Movement _movement;
    private Transform _firePoint;
    private WeaponSwitching _weapon;
    private PlayerInput _playerInput;
    [SerializeField] private Vector2 mousePos;

    private bool _canShoot=true;

    private void Awake()
    {
        //Activating player input for shooting
        _playerInput = new PlayerInput();
        _playerInput.Shooting.Enable();
        _playerInput.Shooting.Shoot.performed += Shoot;

        //New instance for checking the game state
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        //Downloading the appropriate components
        _movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        _firePoint = GameObject.FindGameObjectWithTag("FirePoint").GetComponent<Transform>();
        _weapon = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<WeaponSwitching>();

        
        //Dictionary Initialization 
        int i = 1;
        foreach (Sprite _sprite in _playerHandsSpritesList)
        {
            _playerHandsSpritesDictiorary.Add(i, _sprite);
            i++;
        }
    }

    private void Update()
    {
        mousePos = _playerInput.Shooting.MousePosition.ReadValue<Vector2>();
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _canShoot = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void Shoot (InputAction.CallbackContext context)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(FindObjectOfType<Camera>().ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hits.Any(x => x.collider.GetComponent<IInteractable>() != null) == false && _canShoot==true && _weapon.GetWeapon()==1 && !_movement.GetMovementStates().Contains(Movement.MovementState.Crouching) && !_movement.GetMovementStates().Contains(Movement.MovementState.Running)) 
        {
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.Shooting.Shoot.performed -= Shoot;
    }
}
