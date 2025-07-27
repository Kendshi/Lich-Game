using UnityEngine;

public class ActionState : ICombatState
{
    private readonly CombatStateMachine _stateMachine;

    public ActionState(CombatStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        Debug.Log("Action State Enter");
    }

    public void UpdateState()
    {
        Debug.Log("Action State Update");
    }

    public void ExitState()
    {
        Debug.Log("Action State Exit");
    }

}
