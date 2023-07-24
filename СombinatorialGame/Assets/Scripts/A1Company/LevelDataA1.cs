using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelDataA1 : LevelSettings, IData
{
    public static event Action DataIsReady;

    [SerializeField] public int[] ElementsPerStroke { get; private set; }
    [SerializeField] public int[] Height { get; private set; }
    [SerializeField] public int[] Width { get; private set; }
    [SerializeField] public ConditionExitLvl[] Condition { get; private set; }
    [SerializeField] public Vector2[] PlayerPos { get; private set; }
    [SerializeField] public Vector2[] PortalPos { get; private set; }
    [SerializeField] public LevelEnemy[] Enemy { get; private set; }
    [SerializeField] public LevelStalker[] Stalker { get; private set; }
    [SerializeField] public LevelFirstElement[] FirstElement { get; private set; }
    [SerializeField] public LevelTwoElement[] TwoElement { get; private set; }
    [SerializeField] public Vector2[] KeyPos {  get; private set; }
    [SerializeField] public Vector3[] ProbabiliSpawnElement { get; private set; }
    [SerializeField] public int MaxLevelNumber { get; private set; }
    [SerializeField] private int _setLevelNumber;

    private StorageCompanyInteractor _companyInteractor;

    private void Awake()
    {
        _setLevelNumber = 0;
        base.LevelNumber = 0;
        MaxLevelNumber = 5;
        Init();
        SetLevel();
        StorageCompanyInteractor.LoadStartEvent += GetCompanyInteractor;
    }

    void GetCompanyInteractor()
    {
        _companyInteractor = Game.GetInteractor<StorageCompanyInteractor>();
        StorageCompanyInteractor.LoadOnEvent += GetLevel;
        Game.OnGameInitializedEvent -= GetCompanyInteractor;
    }

    void GetLevel()
    {
        LevelNumber = _companyInteractor._storageCompany.MaxLevel;
        StorageCompanyInteractor.LoadOnEvent -= GetLevel;
        DataIsReady?.Invoke();
    }

    void Init()
    {
        FirstElement = new LevelFirstElement[MaxLevelNumber];
        Enemy = new LevelEnemy[MaxLevelNumber];
        TwoElement = new LevelTwoElement[MaxLevelNumber];
        Stalker = new LevelStalker[MaxLevelNumber];
        Condition = new ConditionExitLvl[MaxLevelNumber];

        Width = new int[MaxLevelNumber];
        Height = new int[MaxLevelNumber];

        PlayerPos = new Vector2[MaxLevelNumber];
        PortalPos = new Vector2[MaxLevelNumber];

        ElementsPerStroke = new int[MaxLevelNumber];
        ProbabiliSpawnElement = new Vector3[MaxLevelNumber];
    }

    void SetLevel()
    {
        FirstLevel();
        NextSetlevel();
        TwoLevel();
        NextSetlevel();
        ThreeLevel();
        NextSetlevel();
        FourLevel();
        NextSetlevel();
        FiveLevel();
        NextSetlevel();
    }

    void NextSetlevel() => _setLevelNumber++;

    void FirstLevel()
    {
        SetField(3, 3);
        SetFirstElement(ElementFirstType.fire, new Vector2(1,1));
        SetEnemy(new Vector2(2,2));
        SetTwoElement(ElementTwoType.lava, new Vector2(1,0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(0,0));
        SetPortalPos(new Vector2(1,2));
        SetElementPerStroke(2);
        SetProbabilityElement(1, 1, 1);
    }

    void TwoLevel()
    {
        SetField(4, 4);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(3);
        SetProbabilityElement(1, 2, 2);
    }

    void ThreeLevel()
    {
        SetField(5, 5);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(4);
        SetProbabilityElement(2, 1, 1);
    }

    void FourLevel()
    {
        SetField(6, 6);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(5);
        SetProbabilityElement(1, 1, 2);
    }

    void FiveLevel()
    {
        SetField(4, 4);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetStalker(new Vector2(3,3));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(3);
        SetProbabilityElement(1, 2, 1);
    }

    void SixLevel()
    {
        SetField(6, 6);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(5);
        SetProbabilityElement(1, 1, 2);
    }

    void SevenLevel()
    {
        SetField(6, 6);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(5);
        SetProbabilityElement(1, 1, 2);
    }

    void EightLevel()
    {
        SetField(6, 6);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(5);
        SetProbabilityElement(1, 1, 2);
    }

    void NineLevel()
    {
        SetField(6, 6);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(5);
        SetProbabilityElement(1, 1, 2);
    }

    void TenLevel()
    {
        SetField(6, 6);
        SetFirstElement(ElementFirstType.fire, new Vector2(0, 0));
        SetEnemy(new Vector2(0, 1));
        SetTwoElement(ElementTwoType.lava, new Vector2(1, 0));
        SetCondition(ConditionExitLvl.No);
        SetPlayerPos(new Vector2(1, 1));
        SetPortalPos(new Vector2(1, 2));
        SetElementPerStroke(5);
        SetProbabilityElement(1, 1, 2);
    }





    public void LevelUp() => LevelNumber++;

    void SetFirstElement(ElementFirstType type, Vector2 pos)
    {
        FirstElement[_setLevelNumber] = new LevelFirstElement();
        FirstElement[_setLevelNumber].AddElement(type, pos);
    }

    void SetEnemy(Vector2 pos)
    {
        Enemy[_setLevelNumber] = new LevelEnemy();
        Enemy[_setLevelNumber].AddEnemy(pos);
    }

    void SetTwoElement(ElementTwoType type, Vector2 pos)
    {
        TwoElement[_setLevelNumber] = new LevelTwoElement();
        TwoElement[_setLevelNumber].AddElement(type, pos);
    }

    void SetStalker(Vector2 pos) 
    {
        Stalker[_setLevelNumber] = new LevelStalker();
        Stalker[_setLevelNumber].AddStalker(pos);
    }

    void SetCondition(ConditionExitLvl condition)
    {
        Condition[_setLevelNumber] = new ConditionExitLvl();
        Condition[_setLevelNumber] = condition;
    }

    void SetField(int wid, int hei)
    {
        Width[_setLevelNumber] = wid;
        Height[_setLevelNumber] = hei;
    }

    void SetPlayerPos(Vector2 pos) => PlayerPos[_setLevelNumber] = pos;

    void SetPortalPos(Vector2 pos) => PortalPos[_setLevelNumber] = pos;

    void SetElementPerStroke(int count) => ElementsPerStroke[_setLevelNumber] = count; 

    void SetProbabilityElement(float fire, float water, float stone) => ProbabiliSpawnElement[_setLevelNumber] = new Vector3(fire, water, stone);

    public bool IsEnd(int level)
    {
        if (level >= MaxLevelNumber) return true;
        else return false;
    }

    int IData.GetHeight() => Height[LevelNumber];
    int IData.GetWidth() => Width[LevelNumber];
    int IData.GetElementsPerStroke() => ElementsPerStroke[LevelNumber];
    ConditionExitLvl IData.GetCondition() => Condition[LevelNumber];
    Vector3 IData.GetProbabilityEl() => ProbabiliSpawnElement[LevelNumber];
    public Vector2 GetPlayerPos() => PlayerPos[LevelNumber];
    public Vector2 GetPortalPos() => PortalPos[LevelNumber];
    public Vector2 GetKeyPos() => KeyPos[LevelNumber];
    public LevelEnemy GetEnemy() => Enemy[LevelNumber];
    public LevelStalker GetStalker()
    {
        if (Stalker[LevelNumber] != null)
        {
            return Stalker[LevelNumber];
        }
        else return  null;
    }
    public LevelFirstElement GetFirstElements() => FirstElement[LevelNumber];
    public LevelTwoElement GetTwoElement()
    {
        if (TwoElement[LevelNumber] != null)
        {
            return TwoElement[LevelNumber];
        }
        else return null;
    }
}

public class LevelEnemy
{
    public int CountEnemy{ get;private set; }
    public Vector2[] Pos { get; private set; }

    public LevelEnemy()
    {
        Pos = new Vector2[100];
        CountEnemy = 0;
    }

    public void AddEnemy(Vector2 pos)
    {
        Pos[CountEnemy] = pos;
        CountEnemy++;
    }

}

public class LevelStalker
{
    public int CountStalker { get; private set; }
    public Vector2[] StalkerPos { get; private set; }

    public LevelStalker()
    {
        StalkerPos = new Vector2[100];
    }

    public void AddStalker(Vector2 pos)
    {
        CountStalker++;
        StalkerPos[CountStalker] = pos;
    }
}

public class LevelFirstElement
{
    public int ElementCount { get; private set; }
    public ElementFirstType[] Type { get; private set; }
    public Vector2[] ElementPos { get; private set; }

    public LevelFirstElement()
    {
        Type = new ElementFirstType[100];
        ElementPos = new Vector2[100];
        ElementCount = 0;
    }

    public void AddElement(ElementFirstType type, Vector2 pos)
    {
        Type[ElementCount] = type;
        ElementPos[ElementCount] = pos;
        ElementCount++;
    }
}

public class LevelTwoElement
{
    public ElementTwoType[] Type { get; private set; }
    public Vector2[] ElementPos { get; private set; }
    public int ElementCount { get; private set; }

    public LevelTwoElement()
    {
        Type = new ElementTwoType[100];
        ElementPos = new Vector2[100];
        ElementCount = 0;
    }

    public void AddElement(ElementTwoType type, Vector2 pos)
    {
        Type[ElementCount] = type;
        ElementPos[ElementCount] = pos;
        ElementCount++;
    }
}

