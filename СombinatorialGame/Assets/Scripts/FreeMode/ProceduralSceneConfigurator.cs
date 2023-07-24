using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ProceduralSceneConfigurator : LevelSettings, IData
{
    private System.Random random;
    [SerializeField] public GameManager manager;
    private int _width, _height;
    [SerializeField] private int _anchor, _downWard, _upWard, _numberElement, _numberEnemy;
    [SerializeField] private ConditionExitLvl _condition;
    [SerializeField] private bool _squareField, _fixSquareSize, _stalkerMode;
    [SerializeField] private float _probabilityFireEl, _probabilityStoneEl, _probabilityWaterEl;
    [SerializeField] private int ElementsPerStroke;
    [SerializeField] private Vector3 _probabilitySpawnElement;

    private GameObject _levelCountTag;
    private TMP_Text _levelCount;

    public void Awake()
    {
        base.LevelNumber = 1;
        ElementsPerStroke = 1;
        random = new System.Random();
        _stalkerMode = false;
        _probabilitySpawnElement = new Vector3(1, 1, 1);

        CreateFieldSize();
    }

    public void Start()
    {
        manager.SetProbabilityEl(_probabilityFireEl, _probabilityStoneEl, _probabilityWaterEl);
        _levelCountTag = GameObject.FindGameObjectWithTag("LevelCount");
        _levelCount = _levelCountTag.GetComponent<TMP_Text>();
        Debug.Log(_levelCountTag);
        Debug.Log(_levelCount);
    }

    private void CreateFieldSize()
    {
        if (_fixSquareSize == false)
        {
            if (_squareField == false)
            {
                _width = _anchor + random.Next(_downWard, _upWard);
                _height = _anchor + random.Next(_downWard, _upWard);
            }
            else
            {
                int fieldSize = _anchor + random.Next(_downWard, _upWard);
                _width = fieldSize;
                _height = fieldSize;
            }
        }
    }

    public void UpLvl()
    {
        LevelNumber++;
        _levelCount.text = LevelNumber.ToString();
        CreateFieldSize();
        if(LevelNumber % 3 == 0) _upWard++; _numberEnemy++; ElementsPerStroke++;
        if (LevelNumber % 5 == 0) _anchor++; _numberElement++;
        if(LevelNumber == 5) _downWard--;
        if (LevelNumber % 10 == 0) _downWard--;
        if(_stalkerMode == true && LevelNumber == 2) { _stalkerMode = true; }
    }

    int IData.GetWidth() => _width;
    int IData.GetHeight() => _height;
    ConditionExitLvl IData.GetCondition() => _condition;
    int IData.GetElementsPerStroke() => ElementsPerStroke;
    Vector3 IData.GetProbabilityEl() => _probabilitySpawnElement;
    public bool StalkerMode => _stalkerMode;

    public int GetNumberElement => _numberElement;
    public int GetNumberEnemy => _numberEnemy;
}

public enum ConditionExitLvl
{
    No,
    KillAllEnemy,
    GetKey
}

