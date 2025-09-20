using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringReference
{
    public bool useConstant = true;
    public string constantValue;
    public StringValue variable;

    public string value
    {
        get { return useConstant ? constantValue : variable.value; }
        set
        {
            if (useConstant) constantValue = value;
            else variable.value = value;
        }
    }
}
