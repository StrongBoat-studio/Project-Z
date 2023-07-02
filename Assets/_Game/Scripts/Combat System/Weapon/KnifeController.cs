using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;

public class KnifeController : MonoBehaviour
{
    [Header("Knife Settings")]
    [Range(0f, 10f)] [SerializeField] private float _attackRange;
    [Range(0f, 20f)] [SerializeField] private int _demage = 10;
    [SerializeField] GameObject _knife;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] WeaponSwitching _weapon;
    [SerializeField] private Animator _animator;

    private Collider2D _hitEnemie;
    private PlayerInput _playerInput;
    private WalkerStateManager _walkerStateManager;
    private JumperStateManager _jumperStateManager;
    private float _time = 0.5f;
    private bool _isTime = false;

    //To see if I can stab
    private bool _canStab = true;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Stab.Enable();
        _playerInput.Stab.Stab.performed += Stab;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        if(_isTime)
        {
            _time -= Time.deltaTime;
        }
        if(_time<=0)
        {
            _animator.SetBool("IsKnife", false);
            _isTime = false;
        }
            
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _canStab = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void Stab(InputAction.CallbackContext context)
    {
        //Detrct enemies in range of attack
        _hitEnemie = Physics2D.OverlapCircle(_knife.transform.position, _attackRange, _enemyLayer);

        //Chacking if I click on something interactive
        Vector2 mousePos = _playerInput.Stab.MousePosition.ReadValue<Vector2>();
        RaycastHit2D[] hits = Physics2D.RaycastAll(FindObjectOfType<Camera>().ScreenToWorldPoint(mousePos), Vector2.zero);

        //Make animation
        if(hits.Any(x => x.collider.GetComponent<IInteractable>() != null) == false && _canStab == true && _weapon.GetWeapon() == 2)
        {
            KnifeAnim();
        }

        //Chack all conditions
        if (hits.Any(x => x.collider.GetComponent<IInteractable>() != null) == false && _hitEnemie != null && _canStab==true && _weapon.GetWeapon() == 2)
        {
            _walkerStateManager = _hitEnemie.GetComponent<WalkerStateManager>();
            _jumperStateManager = _hitEnemie.GetComponent<JumperStateManager>();

            if (_walkerStateManager != null)
            {
                if (_walkerStateManager.GetLife() > 0)
                {
                    _walkerStateManager.TakeDamage(_demage, 2);
                }
            }

            if (_jumperStateManager != null)
            {
                if(_jumperStateManager.GetLife()>0)
                {
                    _jumperStateManager.TakeDamage(_demage, 2);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_knife.transform.position, _attackRange);
    }

    private void KnifeAnim()
    {
        _time = 0.5f;
        _animator.SetBool("IsKnife", true);
        _isTime = true;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.Stab.Stab.performed -= Stab;
    }
}
