using UnityEngine;

public class HeavyAttackState : HeavyBaseState
{
    private float _time=1f;
    public override void EnterState(HeavyStateManager heavy)
    {
        Debug.Log("Attack State");
        Context.M.SetActive(true);
    }

    public override void UpdateState(HeavyStateManager heavy)
    {
        AttackP();
        AttackM();

        if (Context.Distance > Context.DistanceChase)
        {
            heavy.SwitchState(heavy.PoToPoState);
            Context.M.SetActive(false);
        }
    }

    private void AttackP()
    {
        if(Input.GetKeyDown(KeyCode.M)) 
        {
            Context.MLife -= 10;
        }
    }

    private void AttackM()
    {
        _time-= Time.deltaTime;
        if(_time<=0)
        {
            Context.Life-= 10;
            _time = 1f;
        }

    }

}
