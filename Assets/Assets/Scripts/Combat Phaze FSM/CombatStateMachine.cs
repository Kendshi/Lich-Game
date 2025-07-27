using System.Collections.Generic;
using System;

public class CombatStateMachine
{
    private Dictionary<Type, ICombatState> _combatStates;
    private ICombatState _currentState;

    public CombatStateMachine()
    {
        _combatStates = new Dictionary<Type, ICombatState>()
        {
            [typeof(PrepareState)] = new PrepareState(this),
            [typeof(ActionState)] = new ActionState(this)
        };
    }

    public void EnterIn<TState>() where TState : ICombatState
    {
        if (_combatStates.TryGetValue(typeof(TState), out ICombatState combatState))
        {
            _currentState?.ExitState();
            _currentState = combatState;
           _currentState?.EnterState();
        }
    }
}
