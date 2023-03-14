using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Element _fireOnePrefab, _waterOnePrefab, _stoneOnePrefab, _fireTwoPrefab, _waterTwoPrefab, _stomeTwoPrefab, _lavaPrefab, _steamPrefab, _plantPrefab;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Teleport _teleportPrefab;

    [SerializeField] private float _coefficientCells;
    [SerializeField] private float _trevelBlockTime;
    [SerializeField] private SceneBuilder _sceneBuilder;
    [SerializeField] private Collision _collision;
    [SerializeField] private Vector2 _playerStartPosition;

    [SerializeField] private int _elementsPerStroke;
    [SerializeField] private float _strokeLag;
    [SerializeField] private int _startElementCount;
    [SerializeField] private int _maxOrederElenet;

    private List<Block> _blocksList;
    private List<Node> _nodesList;
    private List<LoadData> _loadDataList;
    public GameState _state;
    private int _round = 0;
    private Player _player;


    private void Start()
    {
        ChangeState(GameState.GenerateLvl);
    }

    private void Update()
    {
        //Debug.Log(_state);

        if (Input.GetKey(KeyCode.D)) { PrevShift(Vector2.right); }
        else if (Input.GetKey(KeyCode.A)) { PrevShift(Vector2.left); }
        else if (Input.GetKey(KeyCode.W)) { PrevShift(Vector2.up); }
        else if (Input.GetKey(KeyCode.S)) { PrevShift(Vector2.down); }
        else if(_state == GameState.PrevShift) { Debug.Log("��������� �������"); ChangeState(GameState.WatingInput); } // ������� �� �������

        if (Input.GetKeyDown(KeyCode.RightArrow)) { Shift(Vector2.right); }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { Shift(Vector2.left); }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { Shift(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { Shift(Vector2.down); }

        if (Input.GetKeyDown(KeyCode.Backspace)) { ReverseMove(); }

    }

    public void ChangeState(GameState newState)
    {
        _state = newState;
        switch(newState)
        {
            case GameState.GenerateLvl:
                GenerateLvl();
                this.Wait(3f, () =>
                {
                    SpawnRandomElement(_startElementCount);
                });
                break;
            case GameState.WatingInput:
                break;
            case GameState.Moving:
                //SaveMove();
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    private bool CheckOpportunityShift(Vector2 direction)
    {
        var orderedBlocks = SortingBlocks(direction);
        foreach (var block in orderedBlocks)
        {
            if (block.mobile != 0)
            {
                Node possibleNode = GetNodeAtPosition(block.node.Pos + new Vector2(direction.x, direction.y * _coefficientCells));
                if (possibleNode != null)
                {
                    if (possibleNode.occupiedBlock == null) { return true; }
                    if (possibleNode.occupiedBlock != null && _collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet)) { return true; }
                }
            }
        }
        return false;
    }

    private void PrevShift(Vector2 direction)
    {
        if (_state == GameState.PrevShift) { return; }
        else { ChangeState(GameState.PrevShift); }

        var orderedBlocks = SortingBlocks(direction);
        foreach (var block in orderedBlocks)
        {
            if (block.mobile != 0)
            {
                Node possibleNode = GetNodeAtPosition(block.node.Pos + new Vector2(direction.x, direction.y * _coefficientCells)); 
                if (possibleNode != null) 
                {
                    if (possibleNode.occupiedBlock == null) { block.ChangeNode(possibleNode); } 
                }

                if ((Vector2)block.transform.position != block.node.Pos) 
                {
                    block.transform.DOMove(block.node.Pos - direction / 2, _trevelBlockTime);
                }
            }
        }
    }

    private void Shift(Vector2 direction)
    {
        if (CheckOpportunityShift(direction) == false || _state != GameState.WatingInput) { return; }
        ChangeState(GameState.Moving);
        Invoke("SpawnRandomElement", _trevelBlockTime);
        bool collision;

        _player.ChangeMotionStatus(true);
        var seqence = DOTween.Sequence();

        var orderedBlocks = SortingBlocks(direction);
        foreach (var block in orderedBlocks)
        {
            collision = false;
            int distance = block.mobile + 1;
            if (distance > 0)
            {
                Node next = block.node; // �������� ���� �� ���� � ��������� �������
                do // ������� � ������� ���, ������� �� �������
                {
                    distance--;
                    block.ChangeNode(next); // ���� � ������� ����� ������� ����������, ��������� ������ ������


                    Node possibleNode = GetNodeAtPosition(next.Pos + new Vector2(direction.x, direction.y * _coefficientCells)); // �������, ���������� �� ������ ���� �� +1 � ����������� ��������
                    if (possibleNode != null) // ���? ��������, �������� ���� � �����
                    {
                        if (possibleNode.occupiedBlock != null && _collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet) && possibleNode.occupiedBlock.mergingBlock == null) // �������� ����?
                        {
                            // !!!! ������ ��� ��������� ������������
                            block.mergingBlock = possibleNode.occupiedBlock;
                            next = possibleNode;
                            collision = true;
                            block.ChangeNode(next);
                        }

                        else if (possibleNode.occupiedBlock == null) { next = possibleNode; } // ����� ��� ������? ��� - ������ NEXT � ��� ������
                    }
                } while (next != block.node && distance > 0 && collision == false); // ���������, ���� �� ����� ������ �����������


                if ((Vector2)block.transform.position != block.node.Pos) // ���� �������� ����, ������� � ��������
                {
                    block.transform.DOMove(block.node.Pos, _trevelBlockTime);
                }

                Vector2 movePoint = block.mergingBlock != null ? block.mergingBlock.node.Pos : block.node.Pos; // ���� ���� ��������, ������� � � ����������� ��������
                seqence.Insert(0, block.transform.DOMove(movePoint, _trevelBlockTime)); 
            }
        }

        seqence.OnComplete(() => // �� ���������� �������� ��� ��������, ����������� ������������
        {
            foreach (var block in orderedBlocks.Where(b => b.mergingBlock != null))
            {
                _collision.MergeBlocks(block.mergingBlock, block);
            }

            SetLevelsBlocks();
            if (_state == GameState.Win) { Win(); }
            else if (_state == GameState.Lose) { Lose(); }
        });

        Invoke("EndMovingState", _strokeLag);
        Invoke("EndPlayerMoving", _trevelBlockTime);
    }

    public void ShiftOne(Character block, Vector2 direction)
    {
        //bool collision = false;
        //int distance = block.mobile + 1;
        //if (distance > 0)
        //{
        //    Node next = block.node; // �������� ���� �� ���� � ��������� �������
        //    do // ������� � ������� ���, ������� �� �������
        //    {
        //        distance--;
        //        block.ChangeNode(next); // ���� � ������� ����� ������� ����������, ��������� ������ ������
        //
        //
        //        Node possibleNode = GetNodeAtPosition(next.Pos + new Vector2(direction.x, direction.y * _coefficientCells)); // �������, ���������� �� ������ ���� �� +1 � ����������� ��������
        //        if (possibleNode != null) // ���? ��������, �������� ���� � �����
        //        {
        //            if (possibleNode.occupiedBlock != null && _collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet) && possibleNode.occupiedBlock.mergingBlock == null) // �������� ����?
        //            {
        //                // !!!! ������ ��� ��������� ������������
        //                block.mergingBlock = possibleNode.occupiedBlock;
        //                next = possibleNode;
        //                collision = true;
        //                block.ChangeNode(next);
        //            }
        //
        //            else if (possibleNode.occupiedBlock == null) { next = possibleNode; } // ����� ��� ������? ��� - ������ NEXT � ��� ������
        //        }
        //    } while (next != block.node && distance > 0 && collision == false); // ���������, ���� �� ����� ������ �����������
        //
        //
        //    if ((Vector2)block.transform.position != block.node.Pos) // ���� �������� ����, ������� � ��������
        //    {
        //        block.transform.DOMove(block.node.Pos, _trevelBlockTime);
        //    }
        //}
    }

    private void EndMovingState()
    {
        ChangeState(GameState.WatingInput);
    }

    private void EndPlayerMoving()
    {
        _player.ChangeMotionStatus(false);
    }

    private void GenerateLvl()
    {
        if(_blocksList == null)
        {
            _blocksList = new List<Block>();
            _loadDataList = new List<LoadData>();
        }
        _nodesList = _sceneBuilder.GenerateLvl();
        _collision.SetElementsPrefab(_fireOnePrefab, _waterOnePrefab, _stoneOnePrefab, _fireTwoPrefab, _waterTwoPrefab, _stomeTwoPrefab, _lavaPrefab, _steamPrefab, _plantPrefab);
        Invoke("CreatePlayer", 2f);
        Invoke("CreateEnemy", 2f);
        Invoke("CreateTeleport", 2f);
        _round = 0;
        ChangeState(GameState.WatingInput);
    }
    private void CreatePlayer()
    {
        var player = Instantiate(_playerPrefab, _nodesList[0].Pos, Quaternion.identity);
        player.Init(this);
        player.ChangeNode(_nodesList[0]);
        _blocksList.Add(player);
        _player = player.Initialize(this);
    }

    private void CreateEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, _nodesList[_nodesList.Count - 1].Pos, Quaternion.identity);
        enemy.Init(this);
        enemy.ChangeNode(_nodesList[_nodesList.Count - 1]);
        _blocksList.Add(enemy);
    }

    private void CreateTeleport()
    {
        var teleport = Instantiate(_teleportPrefab, _nodesList[_nodesList.Count - 6].Pos, Quaternion.identity);
        teleport.Init(this);
        teleport.ChangeNode(_nodesList[_nodesList.Count - 6]);
        _blocksList.Add(teleport);
    }

    private List<Block> SortingBlocks(Vector2 direction)
    {
        var orderedBlocks = _blocksList.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if (direction == Vector2.right || direction == Vector2.up) { orderedBlocks.Reverse(); }
        return orderedBlocks;
    }

    private void SetLevelsBlocks()
    {
        foreach(var block in _blocksList)
        {
            block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, block.node.transform.position.z + 10);
        }
    }

    private void SpawnRandomElement()
    {
        SpawnRandomElement(_elementsPerStroke);
    }

    private void SpawnRandomElement(int amount)
    {
        var freeNodes = _nodesList.Where(n => n.occupiedBlock == null).OrderBy(b => UnityEngine.Random.value);

        foreach(var node in freeNodes.Take(amount))
        {
            float rand = UnityEngine.Random.value;
            if(rand > 0.66f) { SpawnElement(node, _fireOnePrefab); }
            else if(rand < 0.33f) { SpawnElement(node, _stoneOnePrefab); }
            else { SpawnElement(node, _waterOnePrefab); }
        }


        if(freeNodes.Count() == 1)
        {
            // End game
            _sceneBuilder.DestroyLvl();
            return;
        }
    }

    public void SpawnElement(Node node, Element elementPrefab)
    {
        var element = Instantiate(elementPrefab, node.Pos, Quaternion.identity);
        element.ChangeNode(node);
        _blocksList.Add(element);
    }

    public void RemoveBlock(Block block)
    {
        _blocksList.Remove(block);
        Destroy(block.gameObject);
    }

    public Node GetNodeAtPosition(Vector2 pos)
    {
        return _nodesList.FirstOrDefault(n => n.Pos == pos);
    }

    private void SaveMove()
    {
        _loadDataList.Add(new LoadData(_blocksList));
    }

    private void ReverseMove()
    {
        if (_loadDataList.Count < 1) { return; }
        _loadDataList.RemoveAt(_loadDataList.Count - 1);
        _blocksList.Clear();

    }

    private void RestoreBlock()
    {
        LoadData loadData = _loadDataList[_loadDataList.Count - 1];
    }

    public void Win()
    {
        Debug.Log("Win");
        DestroyScene();
        GenerateLvl();
    }

    private void Lose()
    {
        Debug.Log("Lose");
        DestroyScene();
    }

    private void DestroyScene()
    {
        foreach(Block block in _blocksList)
        {
            Destroy(block.gameObject);
        }
        _blocksList.Clear();
        _loadDataList.Clear();
        _sceneBuilder.DestroyLvl();
    }

}

public class LoadData
{
    private List<Node> nodeList;
    private List<BlockType> typeList;
    private List<int> orderList;

    public LoadData(List<Block> blocks)
    {
        nodeList = new List<Node>(); typeList = new List<BlockType>(); orderList = new List<int>();
        foreach(Block block in blocks)
        {
            nodeList.Add(block.node);
            typeList.Add(block.type);
            if(block is Element)
            {
                Element element = block as Element;
                orderList.Add(element.orderElement);
            }
            else { orderList.Add(int.MinValue); }
        }
    }
}

public enum BlockType
{
    Null,
    Fire,
    Water,
    Stone,
    Lava,
    Steam,
    Plant,
    Player,
    Enemy,
    Teleport
}

public enum GameState
{
    GenerateLvl,
    WatingInput,
    Moving,
    Win,
    Lose,
    PrevShift
}