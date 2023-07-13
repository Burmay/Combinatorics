using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _coefficientCells, _trevelBlockTime;
    [SerializeField] private SceneBuilder _sceneBuilder;
    [SerializeField] private CollisionHandler _collision;
    [SerializeField] private ProceduralSceneConfigurator _configurator;
    [SerializeField] private LevelDataA1 _levelDataA1;
    [SerializeField] private Vector2 _playerStartPosition;

    [SerializeField] private int _elementsPerStroke;
    [SerializeField] private float _strokeLag;
    [SerializeField] private int _maxOrederElenet;

    [SerializeField] private bool _isFree;
    [SerializeField] private int _lvlNumber = 1;

    [SerializeField] private Prefabs prefabs;

    private Element _fireOnePrefab, _waterOnePrefab, _stoneOnePrefab, _fireTwoPrefab, _waterTwoPrefab, _stoneTwoPrefab, _lavaPrefab, _steamPrefab, _plantPrefab;
    private Player _playerPrefab;
    private Enemy _enemyPrefab;
    private Teleport _teleportPrefab;
    private Key _keyPrefab;
    private Stalker _stalkerPrefab;

    GameObject loaderTag;
    SceneLoader loader;

    private JsonToFileStorageService storageService;
    private const string key = "FreeModeKey";

    private List<Block> _blocksList;
    private List<Node> _nodesList;
    private List<LoadData> _loadDataList;
    public GameState _state;
    private int _round;
    private Player _player;
    public Vector2 _lastMove;
    private ConditionExitLvl _conditionExit;
    private Teleport _teleport;
    private Stalker _stalker;
    private float _probabilityFireEl, _probabilityStoneEl, _probabilityWaterEl;
    private System.Random random;


    private void Start()
    {
        prefabs.GetPrefabs(this);

        random = new System.Random();
        storageService = new JsonToFileStorageService();
        loaderTag = GameObject.FindWithTag("SceneLoader");
        loader = loaderTag.GetComponent<SceneLoader>();
    }

    public Player SetPlayerLink { set { _player = value; } }
    public Stalker SetStalkerLink { set { _stalker = value; } }
    public void SetProbabilityEl(float fire, float stone, float water)
    {
        _probabilityFireEl = fire; _probabilityStoneEl = stone; _probabilityWaterEl = water;
    }

    public void SetPrefabs(Element fireOne, Element fireTwo, Element waterOne, Element waterTwo, Element stoneOne, Element stoneTwo, Element steam, Element lava, Element plant, Player player, Enemy enemy, Key key, Teleport teleport, Stalker stalker)
    {
        _fireOnePrefab = fireOne;
        _fireTwoPrefab = fireTwo;
        _waterOnePrefab = waterOne;
        _waterTwoPrefab = waterTwo;
        _stoneOnePrefab = stoneOne;
        _stoneTwoPrefab = stoneTwo;
        _steamPrefab = steam;
        _lavaPrefab = lava;
        _plantPrefab = plant;

        _playerPrefab = player;
        _enemyPrefab = enemy;
        _teleportPrefab = teleport;
        _keyPrefab = key;
        _stalkerPrefab = stalker;
    }

    private void Update()
    {

        //if (Input.GetKey(KeyCode.D)) { PrevShift(Vector2.right); }
        //else if (Input.GetKey(KeyCode.A)) { PrevShift(Vector2.left); }
        //else if (Input.GetKey(KeyCode.W)) { PrevShift(Vector2.up); }
        //else if (Input.GetKey(KeyCode.S)) { PrevShift(Vector2.down); }
        //else if(_state == GameState.PrevShift) {  ChangeState(GameState.WatingInput); } 

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
                break;
            case GameState.WatingInput:
                break;
            case GameState.Moving:
                //SaveMove();
                _round++;
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
                    DoMove(block, block.node.Pos - direction / 2);
                }
            }
        }
    }

    private void Shift(Vector2 direction)
    {
        if (CheckOpportunityShift(direction) == false || _state != GameState.WatingInput) { return; }
        ChangeState(GameState.Moving);
        _lastMove = direction;
        Invoke("SpawnRandomElement", _trevelBlockTime);
        bool collision;
        var seqence = DOTween.Sequence();

        var orderedBlocks = SortingBlocks(direction);
        foreach (var block in orderedBlocks)
        {
            collision = false;
            int distance = block.mobile + 1;
            if (distance > 0 && CheckPossibilityOfMove(block) && !(block is Stalker))
            {
                Node next = block.node; 
                do
                {
                    distance--;
                    block.ChangeNode(next); 


                    Node possibleNode = GetNodeAtPosition(next.Pos + new Vector2(direction.x, direction.y * _coefficientCells)); 
                    if (possibleNode != null) 
                    {
                        if (possibleNode.occupiedBlock != null && _collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet) && possibleNode.occupiedBlock.mergingBlock == null && distance > 0) // коллизия есть?
                        {
                            block.mergingBlock = possibleNode.occupiedBlock;
                            next = possibleNode;
                            collision = true;
                            block.ChangeNode(next);
                        }

                        else if (possibleNode.occupiedBlock == null) { next = possibleNode; }
                    }
                } while (next != block.node && distance > 0 && collision == false); 

                if ((Vector2)block.transform.position != block.node.Pos) 
                {
                    if(block is Character)
                    {
                        Character character = block as Character;
                        character.Move();
                    }
                    DoMove(block, block.node.Pos);
                }

                Vector2 movePoint = block.mergingBlock != null ? block.mergingBlock.node.Pos : block.node.Pos;
                seqence.Insert(0, block.transform.DOMove(movePoint, _trevelBlockTime)); 
            }
        }

        seqence.OnComplete(() => 
        {
            foreach (var block in orderedBlocks.Where(b => b.mergingBlock != null))
            {
                _collision.MergeBlocks(block.mergingBlock, block);
                if(block != null) { block.mergingBlock = null; }
            }

            SetLevelsBlocks();
            CheckWinLose();
            if (_stalker != null) StalkerMove();
        }); 

        Invoke("EndMovingState", _strokeLag);
    }

    private bool CheckKillAllEnemy()
    {
        foreach(Block block in _blocksList)
        {
            if(block is Enemy || block is Stalker) { return false; }
        }
        return true;
    }

    private void CheckWinLose()
    {
        if (_conditionExit == ConditionExitLvl.KillAllEnemy)
        {
            if (CheckKillAllEnemy() == true) { _teleport.Condition = true; }
        }
        if(CheckOpportunityShift(new Vector2(0, 1)) == false && CheckOpportunityShift(new Vector2(1, 0)) == false && CheckOpportunityShift(new Vector2(-1, 0)) == false && CheckOpportunityShift(new Vector2(0, -1)) == false)
        {
            ChangeState(GameState.Lose);
        }
        if (_state == GameState.Win) { Win(); }
        else if (_state == GameState.Lose) { Lose(); }
    }

    public void ShiftForOne(Block block, Vector2 dir)
    {

        bool collision = false;
        var seqence = DOTween.Sequence();
        int distance = block.mobile + 1;
        if (distance > 0)
        {
            Node next = block.node; 
            do
            {
                distance--;
                block.ChangeNode(next); 


                Node possibleNode = GetNodeAtPosition(next.Pos + new Vector2(dir.x, dir.y * _coefficientCells)); 
                if (possibleNode != null) 
                {
                    //if(possibleNode.occupiedBlock != null) { Debug.Log(_collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet)); }
                    if (possibleNode.occupiedBlock != null && _collision.CollisionResult(block, possibleNode.occupiedBlock, _maxOrederElenet) && distance > 0) 
                    {
                        block.mergingBlock = possibleNode.occupiedBlock;
                        next = possibleNode;
                        collision = true;
                        block.ChangeNode(next);
                    }

                    else if (possibleNode.occupiedBlock == null) { next = possibleNode; } 
                }
            } while (next != block.node && distance > 0 && collision == false); 


            if ((Vector2)block.transform.position != block.node.Pos) 
            {
                if (block is Character)
                {
                    Character character = block as Character;
                    character.Move();
                }
                DoMove(block, block.node.Pos);
            }

            seqence.OnComplete(() =>    
            {
                if(block.mergingBlock != null && block != null) { _collision.MergeBlocks(block.mergingBlock, block); }
                if (block != null) { block.mergingBlock = null; }

                SetLevelsBlocks();
                CheckWinLose();
            });
        }
    }

    private void DoMove(Block block, Vector3 dir)
    {
        block.transform.DOMove(dir, _trevelBlockTime).SetEase(Ease.InOutCubic).SetLink(gameObject);
    }


    private bool CheckPossibilityOfMove(Block block)
    {
        if (block is Character) { Character character = block as Character; if (character.stun) { character.StunOff(); return false; } else return true; }
        else return true;
    }

    private void StalkerMove()
    {
        if (_player == null) return;
        Vector2 difference = _player.Pos - _stalker.Pos;
        Vector2 FirstStep = new Vector2(difference.x, 0).normalized;
        Vector2 TwoStep = new Vector2(0, difference.y).normalized;
        if(Math.Abs(difference.x) > Math.Abs(difference.y))
        {
            if(StalkerTryStep(FirstStep) == true)
            {
                ShiftForOne(_stalker, FirstStep);
            }
            else
            {
                if (StalkerTryStep(TwoStep) == true)
                {
                    ShiftForOne(_stalker, TwoStep);
                }
            }
        }
        else if (Math.Abs(difference.x) < Math.Abs(difference.y))
        {
            if (StalkerTryStep(TwoStep) == true)
            {
                ShiftForOne(_stalker, TwoStep);
            }
            else
            {
                if (StalkerTryStep(FirstStep) == true)
                {
                    ShiftForOne(_stalker, FirstStep);
                } 
            }
        }
        Debug.Log(FirstStep + "   " + TwoStep);

    }
    private bool StalkerTryStep(Vector2 dir)
    {
        Node possibleNode = GetNodeAtPosition(_stalker.Pos + new Vector2(dir.x, dir.y * _coefficientCells));
        if(possibleNode != null)
        {
            if(possibleNode.occupiedBlock == null) { return true; }
            else if(possibleNode.occupiedBlock is Player) { return true; }
            else { return false; }
        }
        else { return false; }
    }

    private void EndMovingState()
    {
        ChangeState(GameState.WatingInput);
    }

    private void GenerateLvl()
    {
        _round = 0;

        if (_blocksList == null)
        {
            _blocksList = new List<Block>();
            _loadDataList = new List<LoadData>();
        }
        if(_isFree == true)
        {
            _conditionExit = _configurator.GetConditionExit;
        }
        else
        {
            _conditionExit = _levelDataA1.GetCondition(_lvlNumber);
        }
        _nodesList = _sceneBuilder.GenerateLvl();
        ChangeState(GameState.WatingInput);
    }

    public ConditionExitLvl GetConditionExit(Teleport teleport)
    {
        _teleport = teleport;
        _collision.SetTeleportLink(_teleport);
        return _conditionExit;
    }

    public void EnablingEnvironment()
    {
        SpawnBlock(_playerPrefab, 1);
        SpawnBlock(_teleportPrefab, 1);
        if(_conditionExit == ConditionExitLvl.GetKey) { SpawnBlock(_keyPrefab, 1); }
        if (_configurator.StalkerMode == true) { SpawnBlock(_stalkerPrefab, 1); }
        else { SpawnBlock(_enemyPrefab, _configurator.GetNumberEnemy); }
        SpawnRandomElement(_configurator.GetNumberElement);
    }


    private void SpawnBlock(Block blockPrefab, int amount)
    {
        var freeNodes = _nodesList.Where(n => n.occupiedBlock == null).Where(n => n.generationAvalible == true).OrderBy(b => UnityEngine.Random.value);
        foreach (var node in freeNodes.Take(amount))
        {
            var block = Instantiate(blockPrefab, node.Pos, Quaternion.identity);
            block.Init(this);
            block.ChangeNode(node);
            _blocksList.Add(block);

            if(block is Player) { CloseRow(node); }
        }
    }

    private void CloseRow(Node nodeToClose)
    {
        foreach(Node node in _nodesList)
        {
            if(node.Pos.x == nodeToClose.Pos.x || node.Pos.y == nodeToClose.Pos.y) { node.generationAvalible = false; }
        } 
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
            int rand = random.Next(0, (int)((_probabilityFireEl + _probabilityStoneEl + _probabilityWaterEl) * 1000));

            if(rand < _probabilityFireEl * 1000) { SpawnElement(node, _fireOnePrefab); }
            else if(rand < (_probabilityFireEl + _probabilityStoneEl) * 1000) { SpawnElement(node, _stoneOnePrefab); }
            else { SpawnElement(node, _waterOnePrefab); }
        }


        if(freeNodes.Count() == 1)
        {
            
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
        block.Destroy();
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
        _configurator.UpLvl();
        Debug.Log("Win");
        DestroyScene();
        ChangeState(GameState.GenerateLvl);
        _lvlNumber++;
    }

    private void Lose()
    {
        if (_isFree)
        {
            StorageItemFreeMode e = new StorageItemFreeMode();
            e.Round = _configurator._lvlNumber;
            storageService.Save(key, e);


            /// test

            /// storageService.Load<StorageItemFreeMode>(key, data => { Debug.Log($"Loaded. int : {e.Round}"); });
        }
        Debug.Log("Lose");
        Invoke("GoToMenu", 1f);
    }

    private void GoToMenu()
    {
        loader.LoadScene("MainMenu");
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
    Stalker,
    Key,
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