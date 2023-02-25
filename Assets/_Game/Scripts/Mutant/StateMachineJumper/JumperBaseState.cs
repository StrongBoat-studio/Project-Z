using UnityEngine;

public abstract class JumperBaseState 
{
    JumperStateManager context;

    public JumperStateManager Context { get { return context; } set { context = value; } }
        
    public abstract void EnterState(JumperStateManager jumper);

    public abstract void UpdateState(JumperStateManager jumper);


}
