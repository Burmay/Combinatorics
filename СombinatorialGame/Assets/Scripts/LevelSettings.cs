using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public int LevelNumber { get; protected set; }
}

public interface IData
{
    public int GetHeight();
    public int GetWidth();
    public ConditionExitLvl GetCondition();
    public int GetElementsPerStroke();
    public Vector3 GetProbabilityEl();
}
