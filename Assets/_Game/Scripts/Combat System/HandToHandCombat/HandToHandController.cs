using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class HandToHandController : MonoBehaviour
{
    [Header("Hand-To-Hand Combat Settings")]
    [Range(0f, 10f)] [SerializeField] private float _attackRange;
    [Range(0f, 10f)] [SerializeField] private int _demage = 5;
    [SerializeField] GameObject _emptyHand;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] WeaponSwitching _weapon;

    private Collider2D _hitEnemie;
    private PlayerInput _playerInput;
    private WalkerStateManager _walkerStateManager;

    //To see if I can hit
    private bool _canHit = true;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.HandToHand.Enable();
        _playerInput.HandToHand.Hit.performed += Hit;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _canHit = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void Hit(InputAction.CallbackContext context)
    {
        //Detrct enemies in range of attack
        _hitEnemie = Physics2D.OverlapCircle(_emptyHand.transform.position, _attackRange, _enemyLayer);

        //Chacking if I click on something interactive
        Vector2 mousePos = _playerInput.HandToHand.MousePosition.ReadValue<Vector2>();
        RaycastHit2D[] hits = Physics2D.RaycastAll(FindObjectOfType<Camera>().ScreenToWorldPoint(mousePos), Vector2.zero);

        //Chack all conditions
        if (hits.Any(x => x.collider.GetComponent<IInteractable>() != null) == false && _hitEnemie != null && _canHit == true && _weapon.GetWeapon() == 0)
        {
            _walkerStateManager = _hitEnemie.GetComponent<WalkerStateManager>();
            if (_walkerStateManager != null)
            {
                _walkerStateManager.TakeDamage(_demage, 0);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_emptyHand.transform.position, _attackRange);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.Stab.Stab.performed -= Hit;
    }
}
