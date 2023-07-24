using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerDeath : MonoBehaviour
{
    [SerializeField] private WalkerStateManager _walkerStateManager;
    [SerializeField] private GameObject _deadWalker;
    private Transform _deadMutants;


    private void Awake()
    {
        _deadMutants = GameObject.FindGameObjectWithTag("DeadMutants").GetComponent<Transform>();
    }

    public void WalkerDeathFunction()
    {
        _walkerStateManager.DestroyWalker();
        _deadWalker.transform.localScale = _walkerStateManager.GetScale();
        Instantiate(_deadWalker, _walkerStateManager.GetPosition(), Quaternion.identity, _deadMutants);
    }

    public void AttackPlayer()
    {
        GameManager.Instance.Player.TakeDamage(10);
    }
}
