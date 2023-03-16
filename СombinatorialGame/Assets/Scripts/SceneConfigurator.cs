using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfigurator : MonoBehaviour
{
    private int _lvlNumber;
    private LvlData _lvlData;

    public void Start()
    {
        _lvlNumber = 1;
        _lvlData = new LvlData();
        _lvlData.numberEnemy = 1;
    }

    public void UpLvl()
    {
        _lvlNumber++;
        _lvlData.numberEnemy = _lvlNumber;
    }

    public int GetNumberEnemy => _lvlData.numberEnemy;
    public int GetWidth => _lvlData._width;
    public int GetHeight => _lvlData._height;
}

public class LvlData
{
    [SerializeField] public int _width = 6, _height = 6;
    [SerializeField] public int numberEnemy;
}
