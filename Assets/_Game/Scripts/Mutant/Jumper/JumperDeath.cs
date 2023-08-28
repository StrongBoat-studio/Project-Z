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

    public void AttackPlayer()
    {
        GameManager.Instance.Player.TakeDamage(10);

        if(GetComponent<Animator>().GetBool("IsAttack2")==true)
        {
            GetComponent<Animator>().SetBool("IsAttack2", false);
        }

        if (GetComponent<Animator>().GetBool("IsAttack1") == true)
        {
            GetComponent<Animator>().SetBool("IsAttack1", false);
        }
    }
}
