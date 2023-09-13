using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _arms;
    [SerializeField] private List<Sprite> _playerHandsSpritesList;
    [SerializeField] private Vector2[] _firePointPosition = new Vector2[9];
    [SerializeField] private Vector2[] _firePointRotation = new Vector2[9];
    private Dictionary<int, Sprite> _playerHandsSpritesDictiorary = new Dictionary<int, Sprite>();

    private Movement _movement;
    [SerializeField] private Transform _firePoint;
    private WeaponSwitching _weapon;
    private PlayerInput _playerInput;
    [SerializeField] private Vector2 mousePos;
    private SpriteRenderer _spriteRenderer;

    private bool _canShoot=true;
    [SerializeField] private int _whichArm=5;

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
        _weapon = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<WeaponSwitching>();
        _spriteRenderer = _arms.GetComponent<SpriteRenderer>();

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

        WhichArm();
        SwitchArm();
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _canShoot = newGameState == GameStateManager.GameState.Gameplay;
    }

    private void Shoot (InputAction.CallbackContext context)
    {
        if (GameManager.Instance.Player != null)
        {
            if (GameManager.Instance.Player._numberOfAmmo <= 0)
            {
                if (FMODEvents.Instance != null)
                {
                    AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.NoAmmo, transform.position);
                    return;
                }
            }
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(FindObjectOfType<Camera>().ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hits.Any(x => x.collider.GetComponent<IInteractable>() != null) == false && _canShoot==true && _weapon.GetWeapon()==1 && !_movement.GetMovementStates().Contains(Movement.MovementState.Crouching) && !_movement.GetMovementStates().Contains(Movement.MovementState.Running)) 
        {
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            
            //Gunshot audio
            if(FMODEvents.Instance != null)
            {
                AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.Shot, transform.position);
            }

            if(GameManager.Instance.Player!=null)
            {
                GameManager.Instance.Player.AmmoDown();
            }
        }
    }

    private void WhichArm()
    {
        if(mousePos.y<80)
        {
            _whichArm = 9;
        }
        else if(mousePos.y>=80 && mousePos.y<130)
        {
            _whichArm = 8;
        }
        else if(mousePos.y>=130 && mousePos.y<220)
        {
            _whichArm = 7;
        }
        else if (mousePos.y >= 220 && mousePos.y < 320)
        {
            _whichArm = 6;
        }
        else if (mousePos.y >= 320 && mousePos.y < 360)
        {
            _whichArm = 5;
        }
        else if (mousePos.y >= 360 && mousePos.y < 510)
        {
            _whichArm = 4;
        }
        else if (mousePos.y >= 510 && mousePos.y < 720)
        {
            _whichArm = 3;
        }
        else if (mousePos.y >= 720 && mousePos.y < 840)
        {
            _whichArm = 2;
        }
        else
        {
            _whichArm = 1;
        }
    }

    private void SwitchArm()
    {
        _spriteRenderer.sprite = _playerHandsSpritesDictiorary[_whichArm];
        _firePoint.transform.localPosition = _firePointPosition[_whichArm - 1];

        if (this.transform.localScale.x==-1)
        {
            _firePoint.localRotation = new Quaternion(_firePointRotation[_whichArm - 1].y, -_firePointRotation[_whichArm - 1].x, 0f, 0f);
        }
        else if (this.transform.localScale.x == 1)
        {
            _firePoint.localRotation = new Quaternion(_firePointRotation[_whichArm - 1].x, _firePointRotation[_whichArm - 1].y, 0f, 0f);
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.Shooting.Shoot.performed -= Shoot;
    }
}
