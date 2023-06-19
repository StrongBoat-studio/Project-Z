using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] Transform _firePoint;
    [SerializeField] WeaponSwitching _weapon;
    [SerializeField] GameObject _bulletPrefab;
    private PlayerInput _playerInput;

    private bool _canShoot=true;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Shooting.Enable();
        _playerInput.Shooting.Shoot.performed += Shoot;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _canShoot = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void Shoot (InputAction.CallbackContext context)
    {
        Vector2 mousePos = _playerInput.Shooting.MousePosition.ReadValue<Vector2>();
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
