using UnityEngine;

public class PrepareState : ICombatState
{
    private readonly CombatStateMachine _stateMachine;

    public PrepareState(CombatStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        Debug.Log("Prepare State Enter");
        
    }

    public void UpdateState()
    {
        Debug.Log("Prepare State Update");
    }

    public void ExitState()
    {
        Debug.Log("Prepare State Exit");
    }

}
