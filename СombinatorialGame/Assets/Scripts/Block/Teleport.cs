using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Block
{
    public bool Condition { get; set; }
    private ConditionExitLvl _conditionExit;

    private void Start()
    {
        _conditionExit = manager.GetConditionExit(this);
        if (_conditionExit == ConditionExitLvl.No) { Condition = true; }
        else { Condition = false; }
    }

}
