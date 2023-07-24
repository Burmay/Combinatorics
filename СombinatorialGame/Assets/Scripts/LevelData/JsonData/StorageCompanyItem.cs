using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCompanyItem
{

    private int _maxLevel;

    public int MaxLevel
    {
        get { return _maxLevel; }
        set
        {
            if (_maxLevel < value)
            {
                _maxLevel = value;
            }
        }
    }
}
