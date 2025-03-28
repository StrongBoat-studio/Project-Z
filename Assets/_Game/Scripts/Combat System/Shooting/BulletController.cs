using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _speed = 20f;
    private Rigidbody2D _rigidbody2D;
    private WalkerStateManager _walkerStateManager;
    private JumperStateManager _jumperStateManager;
    private BossStateManager _bossStateManager;
    private Transform _player;

    [SerializeField] private GameObject _this;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.velocity = transform.right * _speed;
    }

    private void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if(transform.position.x>_player.position.x+6f || transform.position.x < _player.position.x - 6f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _walkerStateManager = collision.GetComponent<WalkerStateManager>();
        _jumperStateManager = collision.GetComponent<JumperStateManager>();
        _bossStateManager = collision.GetComponent<BossStateManager>();

        if (_walkerStateManager != null)
        {
            if(_walkerStateManager.GetLife()>0)
            {
                _walkerStateManager.TakeDamage(20, 1);
            }
            Destroy(gameObject);
        }

        if(_jumperStateManager!=null)
        {
            if (_jumperStateManager.GetLife() > 0)
            {
                _jumperStateManager.TakeDamage(20, 1);
            } 
            Destroy(gameObject);
        }

        if (_bossStateManager != null)
        {
            if (_bossStateManager.GetLife() > 0)
            {
                _bossStateManager.TakeDamage(20, 1);
            }

            Destroy(gameObject);
        }
    }
}
