using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAttackState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss attack state");
        StartAttack();
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (Context.DistanceAttack < Context.GetDistance())
        {
            boss.SwitchState(boss.ChaseState);
            Context.MonoBehaviour.StopAllCoroutines();
        }
    }

    private void StartAttack()
    {
        ChooseAttack();

        Context.MonoBehaviour.StartCoroutine(NextAttack());
    }

    private void ChooseAttack()
    {
        switch (NumberDrawing())
        {
            case 1:
                {
                    Context.Animator.SetBool("IsAttacking1", true);
                    break;
                }
            case 2:
                {
                    Context.Animator.SetBool("IsAttacking2", true);
                    break;
                }
            case 3:
                {
                    Context.Animator.SetBool("IsAttacking3", true);
                    break;
                }
            case 4:
                {
                    //tutaj bêdzie qte
                    break;
                }
            default:
                {
                    Context.Animator.SetBool("IsAttacking1", true);
                    break;
                }
        }
    }

    private IEnumerator NextAttack()
    {
        yield return new WaitForSeconds(2f);

        ChooseAttack();
        Context.MonoBehaviour.StartCoroutine(NextAttack());
    }

    private int NumberDrawing()
    {
        int number = Random.Range(1, 5);

        return number;
    }
}
