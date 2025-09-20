using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerContext
{
    public Action<PlayerState> ChangePlayerState;
    public Func<float, Action, Coroutine> WaitAndPerformFunction;
    public Func<Action, Coroutine> WaitFrameAndPerformFunction;
    public MonoBehaviour coroutineHolder;
    public AnimationManager animationManager;
    public PlayerMovement playerMovement;
    public AudioEventPlayer audioEventPlayer;
    //public PlayerChecks checks;
    //public PlayerCombat combat;
    //public PlayerCollisions collisions;
}
