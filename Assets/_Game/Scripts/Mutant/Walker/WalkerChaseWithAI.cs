using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerChaseWithAI : MonoBehaviour
{
    //Components use for patrolling
    private WalkerStateManager _walkerStateManager;


    [Header("Chase Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed;


    //Variable for chase
    private WalkerBaseState _currensState;
    private List<WalkerBaseState> _walkerBaseStates = new List<WalkerBaseState>();


    private void Awake()
    {
        //downloading the appropriate components
        _walkerStateManager = GetComponent<WalkerStateManager>();

        //Adding states to the list
        _walkerBaseStates.Add(_walkerStateManager.ChaseState);
    }

    private void FixedUpdate()
    {
        _currensState = _walkerStateManager.GetWalkerState();

        if(_walkerBaseStates.Contains(_currensState))
        {

        }
    }
}
