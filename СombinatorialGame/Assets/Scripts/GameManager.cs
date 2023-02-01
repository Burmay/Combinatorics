using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 6, _height = 6;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private List<BlockType> _typesList;

    private List<Node> _nodesList;
    private List<Block> _blocksList;
    private GameState _state;
    private int _round = 0;

    private BlockType GetBlockTypeValue(int value) => _typesList.First(t => t.value == value);


    private void Start()
    {
        ChangeState(GameState.GenerateLvl);
        SpawnBlocks(4);
    }

    private void Update()
    {
        if (_state != GameState.WatingInput) { return; }

        if (Input.GetKeyDown(KeyCode.RightArrow)) { Shift(Vector2.left); }
        if (Input.GetKeyDown(KeyCode.LeftArrow))  {Shift(Vector2.right); }

    }

    private void ChangeState(GameState newState)
    {
        _state = newState;
        switch(newState)
        {
            case GameState.GenerateLvl:
                GenerateLvl();
                break;
            case GameState.SpawningBlocks:
                SpawnBlocks(_round++ == 0 ? 2 : 1);
                break;
            case GameState.WatingInput:
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }
    
    private void GenerateLvl()
    {
        _round = 0;
        _nodesList = new List<Node>();
        _blocksList = new List<Block>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _width; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                _nodesList.Add(node);
            }
        }

        Vector2 centerCoor = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);
        var board = Instantiate(_boardPrefab, centerCoor, Quaternion.identity);
        board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(centerCoor.x, centerCoor.y, -10);

        ChangeState(GameState.SpawningBlocks);
    }

    private void SpawnBlocks(int amount)
    {
        var freeNodes = _nodesList.Where(n => n.occupiedBlock == null).OrderBy(b => UnityEngine.Random.value);

        foreach(var node in freeNodes.Take(amount))
        {
            var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
            block.Init(GetBlockTypeValue(UnityEngine.Random.value > 0.8f ? 4 : 2)); // !!!
            _blocksList.Add(block);
        }


        if(freeNodes.Count() == 1)
        {
            // End game
            return;
        }
    }

    private void Shift(Vector2 direction)
    {
        var orderdBlocks = _blocksList.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y);
    }
}

[Serializable]
public struct BlockType
{
    public int value;
    public Color color;
}

public enum GameState
{
    GenerateLvl,
    SpawningBlocks,
    WatingInput,
    Moving,
    Win,
    Lose
}