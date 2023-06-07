using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefabs", menuName = "Prefabs Data")]
public class Prefabs : ScriptableObject
{
    [Header("Elements")]
    public Element fireOne;
    public Element fireTwo;
    public Element waterOne;
    public Element waterTwo;
    public Element stoneOne;
    public Element stoneTwo;

    public Element steam;
    public Element lava;
    public Element plant;

    [Header("Objects")]
    public Player player;
    public Enemy enemy;
    public Stalker stalker;
    public Teleport teleport;
    public Key key;

    [Header("Envitroment")]
    public Node floorOne;
    public Node floorTwo;
    public Node floorThree;

    public void GetPrefabs(Object _class)
    {
        if(_class is GameManager)
        {
            var gameManager = _class as GameManager;
            gameManager.SetPrefabs(fireOne, fireTwo, waterOne, waterTwo, stoneOne, stoneTwo, steam, lava, plant, player, enemy, key, teleport, stalker);
        }
        else if(_class is SceneBuilder)
        {
            var sceneBuilder = _class as SceneBuilder;
            sceneBuilder.SetPrefabs(floorOne, floorTwo, floorThree);
        }
        else if( _class is CollisionHandler)
        {
            var collisionHandler = _class as CollisionHandler;
            collisionHandler.SetElementsPrefab(fireOne, waterOne, stoneOne, fireTwo, waterTwo, stoneTwo, lava, steam, plant);
        }
    }

}
