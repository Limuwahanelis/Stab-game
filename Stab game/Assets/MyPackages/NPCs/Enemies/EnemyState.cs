using System;
using UnityEngine;

public abstract class EnemyState:NPCState
{
    protected EnemyContext _enemyContext;

    public EnemyState(GetState function):base(function)
    {
    }

    public virtual void SetUpState(EnemyContext context)
    {
        _enemyContext = context;
    }
    public virtual void Attack(PlayerCombat.AttackModifiers modifier = PlayerCombat.AttackModifiers.NONE) { }
}