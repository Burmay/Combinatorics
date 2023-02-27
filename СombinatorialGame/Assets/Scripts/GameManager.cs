using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Element _firePrefab;
    [SerializeField] private Element _waterPrefab;
    [SerializeField] private Element _stonePrefab;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Enemy _enemyPrefab;

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
    public GameState _state;
    private int _round = 0;
    private Player _player;


    private void Start()
    {
        ChangeState(GameState.GenerateLvl);
        this.Wait(3f, () =>
        {
            SpawnRandomElement(_startElementCount);
        });
    }

    private void Update()
    {
        //Debug.Log(_state);

        if (Input.GetKey(KeyCode.D)) { PrevShift(Vector2.right); }
        else if (Input.GetKey(KeyCode.A)) { PrevShift(Vector2.left); }
        else if (Input.GetKey(KeyCode.W)) { PrevShift(Vector2.up); }
        else if (Input.GetKey(KeyCode.S)) { PrevShift(Vector2.down); }
        else if(_state == GameState.PrevShift) { Debug.Log("Вернуться надобно"); ChangeState(GameState.WatingInput); } // вернуть на позицию

        if (Input.GetKeyDown(KeyCode.RightArrow)) { Shift(Vector2.right); }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { Shift(Vector2.left); }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { Shift(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { Shift(Vector2.down); }

    }

    public void ChangeState(GameState newState)
    {
        _state = newState;
        switch(newState)
        {
            case GameState.GenerateLvl:
                GenerateLvl();
                break;
            case GameState.SpawningBlocks:
                SpawnRandomElement(_elementsPerStroke);
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

        _player.ChangeMotionStatus(true);

        var orderedBlocks = SortingBlocks(direction);
        foreach (var block in orderedBlocks)
        {
            int distance = block.mobile;
            if (distance > 0)
            {
                distance--;
                Node next = block.node; // получаем линк на ноду в отдельный кластер
                do // начиная с крайних нод, пробуем их смещать
                {

                    block.ChangeNode(next); // если в прошлом цикле позиция изменилась, обновляем данные ячейки


                    Node possibleNode = GetNodeAtPosition(next.Pos + new Vector2(direction.x, direction.y * _coefficientCells)); // смотрим, существует ли вообще нода на +1 в направлении смещения
                    if (possibleNode != null) // нет? Проехали, оставили блок в покое
                    {
                        if (possibleNode.occupiedBlock != null && _collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet) && possibleNode.occupiedBlock.mergingBlock == null) // коллизия есть?
                        {
                            // !!!! запись для обработки столкновения
                            block.mergingBlock = possibleNode.occupiedBlock;
                            next = possibleNode;
                        }

                        else if (possibleNode.occupiedBlock == null) { next = possibleNode; } // место уже занято? Нет - меняем NEXT и идём дальше
                    }
                } while (next != block.node || distance < 0); // повторяем, пока не будет любого препядствия

                if ((Vector2)block.transform.position != block.node.Pos) // если смещение есть, двигаем в конечную
                {
                    block.transform.DOMove(block.node.Pos, _trevelBlockTime);
                }
            }
        }

        var seqence = DOTween.Sequence();

        foreach (Block block in orderedBlocks)
        {
            var movePoint = block.mergingBlock != null ? block.mergingBlock.node.Pos : block.node.Pos;

            seqence.Insert(0, block.transform.DOMove(movePoint, _trevelBlockTime));
        }

        seqence.OnComplete(() =>
        {
            foreach (var block in orderedBlocks.Where(b => b.mergingBlock != null))
            {
                _collision.MergeBlocks(block.mergingBlock, block);
            }

            SetLevelsBlocks();
        });
        Invoke("EndMovingState", _strokeLag);
        Invoke("EndPlayerMoving", _trevelBlockTime);
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
        _blocksList = new List<Block>();
        _nodesList = _sceneBuilder.GenerateLvl();
        _collision.SetElementsPrefab(_firePrefab, _waterPrefab, _stonePrefab);
        Invoke("CreatePlayer", 2f);
        Invoke("CreateEnemy", 2f);
        _round = 0;
        ChangeState(GameState.SpawningBlocks);
        ChangeState(GameState.WatingInput);
    }
    private void CreatePlayer()
    {
        var player = Instantiate(_playerPrefab, _nodesList[0].Pos, Quaternion.identity);
        player.Init(this);
        player.ChangeNode(_nodesList[0]);
        _blocksList.Add(player);
        _player = player.Initialize();
    }

    private void CreateEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, _nodesList[_nodesList.Count - 1].Pos, Quaternion.identity);
        enemy.Init(this);
        enemy.ChangeNode(_nodesList[_nodesList.Count - 1]);
        _blocksList.Add(enemy);
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
            if(rand > 0.66f) { SpawnElement(node, _firePrefab); }
            else if(rand < 0.33f) { SpawnElement(node, _stonePrefab); }
            else { SpawnElement(node, _waterPrefab); }
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

}

public enum GameState
{
    GenerateLvl,
    SpawningBlocks,
    WatingInput,
    Moving,
    Win,
    Lose,
    PrevShift
}