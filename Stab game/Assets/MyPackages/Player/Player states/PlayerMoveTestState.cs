using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTestState : PlayerState
{
    public static Type StateType { get => typeof(PlayerMoveTestState); }
    public PlayerMoveTestState(GetState function) : base(function)
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
    public override void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            _context.playerMovement.Stop();
            return;
        }
        _context.playerMovement.Move(direction, GlobalEnums.MoveType.RUN,true);
    }
    public override void InterruptState()
    {
     
    }
}