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

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
    }

    public override void InterruptState()
    {
     
    }
}