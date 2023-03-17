using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfigurator : MonoBehaviour
{
    private int _lvlNumber;
    [SerializeField] private LvlData _lvlData;

    public void UpLvl()
    {
        _lvlNumber++;
        _lvlData.numberEnemy = _lvlNumber;
    }

    public int GetNumberEnemy => _lvlData.numberEnemy;
    public int GetWidth => _lvlData._width;
    public int GetHeight => _lvlData._height;
}

