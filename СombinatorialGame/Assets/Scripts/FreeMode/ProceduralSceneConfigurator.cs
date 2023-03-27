using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProceduralSceneConfigurator : MonoBehaviour
{
    private int _lvlNumber;
    private System.Random random;
    [SerializeField] public GameManager manager;
    [SerializeField] private int _width, _height, _anchor, _downWard, _upWard, _numberElement, _numberEnemy;
    [SerializeField] private ConditionExitLvl _condition;
    [SerializeField] private bool _squareField, _fixSquareSize, _stalkerMode;
    [SerializeField] private float _probabilityFireEl, _probabilityStoneEl, _probabilityWaterEl;

    public void Awake()
    {
        _lvlNumber = 1;
        random = new System.Random();
        _stalkerMode = false;

        CreateFieldSize();
    }

    public void Start()
    {
        manager.SetProbabilityEl(_probabilityFireEl, _probabilityStoneEl, _probabilityWaterEl);
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
        _lvlNumber++;
        CreateFieldSize();
        if(_lvlNumber % 3 == 0) _upWard++;
        if(_lvlNumber % 5 == 0) _anchor++; _numberElement++;
        if(_lvlNumber % 3 == 0) _numberEnemy++;
        if(_lvlNumber == 5) _downWard--;
        if (_lvlNumber % 10 == 0) _downWard--;
        if(_stalkerMode == true && _lvlNumber == 2) { _stalkerMode = true; }
    }

    public int GetNumberEnemy => _numberEnemy;
    public int GetWidth => _width;
    public int GetHeight => _height;
    public int GetNumberElement => _numberElement;
    public ConditionExitLvl GetConditionExit => _condition;
    public bool StalkerMode => _stalkerMode;
}

public enum ConditionExitLvl
{
    No,
    KillAllEnemy,
    GetKey
}

