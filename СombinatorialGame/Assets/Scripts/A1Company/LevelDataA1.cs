using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataA1 : MonoBehaviour
{
    [SerializeField] public int CountLevel { get; private set; }
    [SerializeField] public int[] Height { get; private set; }
    [SerializeField] public int[] Width { get; private set; }
    [SerializeField] public ConditionExitLvl[] Condition { get; private set; }
    [SerializeField] public Vector3[] PlayerPos { get; private set; }
    [SerializeField] public Vector3[] PortalPos { get; private set; }
    [SerializeField] public LevelEnemy[] Enemy { get; private set; }
    [SerializeField] public LevelStalker[] Stalker { get; private set; }
    [SerializeField] public LevelFirstElement[] FirstElement { get; private set; }
    [SerializeField] public LevelTwoElement[] TwoElement { get; private set; }

    private void Awake()
    {
        SetFirstElement();
        SetEnemy();
        SetTwoElement();
        SetStalker();
    }

    void SetFirstElement()
    {
        FirstElement = new LevelFirstElement[CountLevel];

        FirstElement[0] = new LevelFirstElement();
        FirstElement[0].Type[0] = ElementFirstType.fire;
        FirstElement[0].ElementPos[0] = new Vector3(1, 2, 0);
    }

    void SetEnemy()
    {
        Enemy = new LevelEnemy[CountLevel];

        Enemy[0].Pos[0] = new Vector3(2, 1, 0);
    }

    void SetTwoElement()
    {
        TwoElement = new LevelTwoElement[CountLevel];

        TwoElement[0].IsLevelTwoElemet = false;
    }

    void SetStalker()
    {
        Stalker = new LevelStalker[CountLevel];

        Stalker[0].IsStalker = false;
    }

    public bool IsEnd(int level)
    {
        if (level >= CountLevel) return true;
        else return false;
    }

    public int GetHeight(int index) => Height[index];
    public int GetWidth(int index) => Width[index];
    public ConditionExitLvl GetCondition(int index) => Condition[index];
    public Vector3 GetPlayerPos (int index) => PlayerPos[index];
    public Vector3 GetPortalPos(int index) => PortalPos[index];
    public LevelEnemy GetEnemy(int index) => Enemy[index];
    public LevelStalker GetStalker(int index)
    {
        if (Stalker[index].IsStalker)
        {
            return Stalker[index];
        }
        else return  null;
    }
    public LevelFirstElement GetFirstElements(int index) => FirstElement[index];
    public LevelTwoElement GetTwoElement(int index)
    {
        if (TwoElement[index].IsLevelTwoElemet)
        {
            return TwoElement[index];
        }
        else return null;
    }
}

public class LevelEnemy
{
    [SerializeField] public Vector3[] Pos { get; set; }
}

public class LevelStalker
{
    public bool IsStalker { get; set; }
    public Vector3[] StalkerPos { get;set; }
}

public class LevelFirstElement
{
    public ElementFirstType[] Type { get; set; }
    public Vector3[] ElementPos { get; set; }
}

public class LevelTwoElement
{
    public bool IsLevelTwoElemet { get; set; }
    public ElementTwoType[] Type { get; set; }
    public Vector3[] ElementPos { get; set; }
}

public enum ElementFirstType
{
    fire,
    water,
    stone
}

public enum ElementTwoType
{
    fire,
    water,
    stone,
    steam,
    plant,
    lava
}

