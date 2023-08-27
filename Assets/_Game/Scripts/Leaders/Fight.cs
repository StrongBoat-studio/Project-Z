using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] private GameObject _deadBorisPrefab;
    [SerializeField] private GameObject _fight;

    private bool _isQTEStarted = false;

    private GameObject _playerSprite;
    private GameObject _playerWeaponHolder;
    private Movement _movement;

    private GameObject _deadBoris;

    private void Awake()
    {
        if (QTEManager.Instance != null)
        {
            QTEManager.Instance.OnWinQTE += OnWinQTE;
            QTEManager.Instance.OnFailQTE += OnFailQTE;
        }
    }

    private void Update()
    {
        if(_isQTEStarted)
        {
            QTEManager.Instance.QTEAction(QTEManager.Caller.FaightController, 2);
        }
    }

    public void StartQTE()
    {
        if (_isQTEStarted) return;

        QTEManager.Instance.QTEStart(QTEManager.Caller.FaightController, 3);
        _isQTEStarted = true;
    }

    private void OnWinQTE(QTEManager.Caller caller)
    {
        if (caller == QTEManager.Caller.FaightController)
        {
            GetComponent<Animator>().SetBool("JurijWin", true);
        }
    }

    private void OnFailQTE(QTEManager.Caller caller)
    {
        if (caller == QTEManager.Caller.FaightController)
        {
            GetComponent<Animator>().SetBool("BorisWin", true);
        }
    }

    public void DeadBoris()
    {
        _deadBoris = Instantiate(_deadBorisPrefab, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Leaders").GetComponent<Transform>());
        _deadBoris.GetComponent<Transform>().localScale = _fight.GetComponent<Transform>().localScale;

        _playerSprite = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        _playerWeaponHolder = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;

        _playerSprite.SetActive(true);
        _playerWeaponHolder.SetActive(true);

        _movement = GameManager.Instance.movement;
        _movement.CanMove(true);

        QuestLineManager.Instance.CheckQuest(GetComponentInParent<QuestObjective>());

        GameManager.Instance.Player.StartArgueWithBorisDialogue();

        Destroy(this.gameObject);
    }

    public void DeadJurij()
    {
        _movement = GameManager.Instance.movement;
        _movement.CanMove(true);
        GameManager.Instance.Player.TakeDamage(1000);

        _playerSprite = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        _playerWeaponHolder = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;

        _playerSprite.SetActive(true);
        _playerWeaponHolder.SetActive(true);
    }

    private void OnDestroy()
    {
        QTEManager.Instance.OnWinQTE -= OnWinQTE;
        QTEManager.Instance.OnFailQTE -= OnFailQTE;
    }
}
