using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerDeath : MonoBehaviour
{
    [SerializeField] private WalkerStateManager _walkerStateManager;
    [SerializeField] private GameObject _deadWalker;
    [SerializeField] private Transform _trash;
    private Transform _deadMutants;
    public Vector2 trashPosition=new Vector2(0.21f, -0.18f);


    private void Awake()
    {
        _deadMutants = GameObject.FindGameObjectWithTag("DeadMutants").GetComponent<Transform>();
    }

    private void Update()
    {
        if(trashPosition==Vector2.zero)
        {
            trashPosition = new Vector2(0.21f, -0.18f);
        }
        _trash.localPosition = trashPosition;
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
    public void PickUpFalse()
    {
        GetComponent<Animator>().SetBool("IsPickUp", false);
        GetComponentInParent<WalkerStateManager>().isPickUp = false;
        GetComponentInParent<WalkerPatrolling>().enabled = true;
        GetComponentInParent<WalkerChaseWithAI>().enabled = true;
    }
}
