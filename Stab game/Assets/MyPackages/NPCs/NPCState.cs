using System;
using UnityEngine;

public abstract class NPCState
{
    public delegate NPCState GetState(Type state);
    protected NPCContext _npcContext;
    protected GetState _getType;

    public NPCState(GetState function)
    {
        _getType = function;
    }
    public virtual void SetUpState(NPCContext context)
    {
        _npcContext = context;
    }

    public abstract void Update();
    public virtual void FixedUpdate() { }
    public virtual void Hit(DamageInfo damageInfo) { }//ChangeState(_enemyContext.enemyHitState.GetType()); }

    public abstract void InterruptState();
    public void ChangeState(Type newStateType)
    {
        NPCState state = _getType(newStateType);
        _npcContext.ChangeNPCState(state);
        state.SetUpState(_npcContext);
    }
}
