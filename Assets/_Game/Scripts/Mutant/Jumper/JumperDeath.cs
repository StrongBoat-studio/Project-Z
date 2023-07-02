using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperDeath : MonoBehaviour
{
    [SerializeField] private JumperStateManager _jumperStateManager;
    [SerializeField] private GameObject _deadJumper;
    private Transform _deadMutants;


    private void Awake()
    {
        _deadMutants = GameObject.FindGameObjectWithTag("DeadMutants").GetComponent<Transform>();
    }

    public void JumperDeathFunction()
    {
        _jumperStateManager.DestroyJumper();
        _deadJumper.transform.localScale = _jumperStateManager.GetScale();
        Instantiate(_deadJumper, _jumperStateManager.GetPosition(), Quaternion.identity, _deadMutants);
    }
}
