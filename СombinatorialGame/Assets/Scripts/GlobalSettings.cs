using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalSettings
{
    private static GlobalSettings instance;

    public static GlobalSettings getInstance()
    {
        if (instance == null)
            instance = new GlobalSettings();
        return instance;
    }

    public float CoefficientCells { get; private set; }
    public float TrevelBlockTime { get; private set; }

    //public enum BlockType
    //{
    //    Null,
    //    Fire,
    //    Water,
    //    Stone,
    //    Lava,
    //    Steam,
    //    Plant,
    //    Player,
    //    Enemy,
    //    Stalker,
    //    Key,
    //    Teleport
    //}
}
