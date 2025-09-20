using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventSO : ScriptableObject
{
    public abstract void Raise();
}