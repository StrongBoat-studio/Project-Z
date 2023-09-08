using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack1()
    {
        _animator.SetBool("IsAttacking1", false);
    }

    public void Attack2()
    {
        _animator.SetBool("IsAttacking2", false);
    }

    public void Attack3()
    {
        _animator.SetBool("IsAttacking3", false);
    }

    public void Push()
    {
        _animator.SetBool("IsPushing", false);
    }
}
