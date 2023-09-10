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
        Context.nextAttackId = NumberDrawing();

        switch (Context.nextAttackId)
        {
            case 1:
                {
                    Context.SetChaseSpeed(3f);
                    SetChasePoints(0.5f, -0.5f);

                    break;
                }
            case 2:
                {
                    Context.SetChaseSpeed(3f);
                    SetChasePoints(1.1f, -1.1f);

                    break;
                }
            case 3:
                {
                    Context.SetChaseSpeed(3.5f);
                    SetChasePoints(1.6f, -1.55f);

                    break;
                }
            case 4:
                {
                    //tutaj bêdzie qte
                    break;
                }
            default:
                {
                    SetChasePoints(0.5f, -0.5f);

                    break;
                }
        }

        if(Context.nextAttackId==1)
        {
            Context.BoxCollider2D.size = new Vector2(Context.BoxCollider2D.size.x, 3.0f);
        }
        else
        {
            Context.BoxCollider2D.size = new Vector2(Context.BoxCollider2D.size.x, 3.54f);
        }
    }

    private void SetChasePoints(float back, float front)
    {
        Context.AddBack = back;
        Context.AddFront = front;

        Context.SetPlayerChaseAPoints();
    }

    private IEnumerator NextAttack()
    {
        if (Context.nextAttackId == 3)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        
        ChooseAttack();
        Context.Mutant.gameObject.GetComponent<BossChaseWithAI>().canAttack = true;
        Context.MonoBehaviour.StartCoroutine(NextAttack());
    }

    private int NumberDrawing()
    {
        int number = Random.Range(1, 5);

        return number;
    }
}
