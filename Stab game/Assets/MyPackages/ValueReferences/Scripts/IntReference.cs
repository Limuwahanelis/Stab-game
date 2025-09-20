using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class IntReference
{
    public bool useConstant = true;
    public int constantValue;
    public IntValue variable;
    public int value
    {
        get { return useConstant ? constantValue : variable.value; }
        set
        {
            if (useConstant) constantValue = value;
            else variable.value = value;
        }
    }
}
