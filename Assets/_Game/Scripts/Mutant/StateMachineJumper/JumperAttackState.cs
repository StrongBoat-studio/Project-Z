using UnityEngine;

public class JumperAttackState : JumperBaseState 
{
    private float _time = 1f;
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Attack State");
        Context.M.SetActive(true);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        AttackP();
        AttackM();

        if (Context.Distace > Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.PoToPoState);
            Context.M.SetActive(false);
        }
    }

    private void AttackP()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Context.MLife -= 10;
        }
    }

    private void AttackM()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Context.PLife -= 10;
            _time = 1f;
        }

    }
}
