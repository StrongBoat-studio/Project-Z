using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform _firePoint;
    [SerializeField] WeaponSwitching _weapon;
    [SerializeField] GameObject _bulletPrefab;

    private bool _canShoot=true;

    private void Awake()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Fire1") && _weapon.GetWeapon()==1)
        {
            Shoot();
        }

    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _canShoot = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void Shoot()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        RaycastHit2D[] hits = Physics2D.RaycastAll(FindObjectOfType<Camera>().ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hits.Any(x => x.collider.GetComponent<IInteractable>() != null) == false && _canShoot==true)
        {
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        }
    }

    private void OnDestroy()
    {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
