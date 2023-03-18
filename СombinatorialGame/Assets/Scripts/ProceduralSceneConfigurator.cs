using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProceduralSceneConfigurator : MonoBehaviour
{
    private int _lvlNumber;
    private System.Random random;
    [SerializeField] private GenerationData lvlData;
    [SerializeField] private int _width, _height, _anchor, _downWard, _upWard, _numberElement, _numberEnemy;
    [SerializeField] private ConditionExitLvl _condition;
    [SerializeField] private bool _squareField, _fixSquareSize, _stalkerMode;

    public void Awake()
    {
        _lvlNumber = 1;
        random = new System.Random();
        lvlData = new GenerationData();

        lvlData.numberElement = _numberElement;
        lvlData.numberEnemy = _numberEnemy;
        lvlData.squareField = _squareField;
        lvlData.fixSquareSize = _fixSquareSize;
        lvlData.ConditionExit = _condition;
        lvlData.stalkerMode = _stalkerMode;
        _stalkerMode = false;

        CreateFieldSize();
    }

    private void CreateFieldSize()
    {
        if (lvlData.fixSquareSize == false)
        {
            if (lvlData.squareField == false)
            {
                lvlData.width = _anchor + random.Next(_downWard, _upWard);
                lvlData.height = _anchor + random.Next(_downWard, _upWard);
            }
            else
            {
                int fieldSize = _anchor + random.Next(_downWard, _upWard);
                lvlData.width = fieldSize;
                lvlData.height = fieldSize;
            }
        }
        else
        {
            lvlData.width = _width;
            lvlData.height = _height;
        }
    }

    public void UpLvl()
    {
        _lvlNumber++;
        CreateFieldSize();
        if(_lvlNumber % 3 == 0) _upWard++;
        if(_lvlNumber % 5 == 0) _anchor++; lvlData.numberElement++;
        if(_lvlNumber % 3 == 0) lvlData.numberEnemy++;
        if(_lvlNumber == 5) _downWard--;
        if (_lvlNumber % 10 == 0) _downWard--;
        if(lvlData.stalkerMode == true && _lvlNumber == 2) { _stalkerMode = true; }
    }

    public int GetNumberEnemy => lvlData.numberEnemy;
    public int GetWidth => lvlData.width;
    public int GetHeight => lvlData.height;
    public int GetNumberElement => lvlData.numberElement;
    public ConditionExitLvl GetConditionExit => lvlData.ConditionExit;
    public bool StalkerMode => _stalkerMode;
}

public class GenerationData
{
    public int width, height;
    public int numberElement;
    public int numberEnemy;
    public bool squareField;
    public bool fixSquareSize;
    public List<BlockType> blockTypeList;
    public List<Vector2> blockPosList;
    public ConditionExitLvl ConditionExit;
    public bool stalkerMode;
}

public enum ConditionExitLvl
{
    No,
    KillAllEnemy,
    GetKey
}

