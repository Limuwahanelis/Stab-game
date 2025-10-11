using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    public static Type StateType { get => typeof(PlayerAttackingState); }
    public PlayerAttackingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        PerformInputCommand();
    }
    public override void Attack(PlayerCombat.AttackModifiers attackModifier = PlayerCombat.AttackModifiers.NONE)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(HelperClass.MousePos);
        pos.z = 0;
        _context.attackTarget.position = pos;
    }
    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        Vector3 pos = Camera.main.ScreenToWorldPoint(HelperClass.MousePos);
        pos.z = 0;
        _context.attackTarget.position = pos;
        _context.animationManager.PlayAnimation("Attack");
    }

    public override void InterruptState()
    {
     
    }
}