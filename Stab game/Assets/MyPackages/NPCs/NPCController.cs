using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCController : MonoBehaviour
{
    [SerializeField, HideInInspector] protected string _initialStateType;
    protected Dictionary<Type, NPCState> _npcStates = new Dictionary<Type, NPCState>();
    protected NPCState _currentNPCState;
}
