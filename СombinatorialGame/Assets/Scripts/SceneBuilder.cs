using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneBuilder : MonoBehaviour
{
    private Action stageIsSet;

    [SerializeField] private bool _isFree;
    private int _width, _height;
    private Node _nodePrefab1, _nodePrefab2, _nodePrefab3;
    [SerializeField] private float _coefficientCells;
    [SerializeField] private GameManager _manager;
    private IData data;
    [SerializeField] private float speedUnfoldingNodes;
    [SerializeField] private float floorAnimatoinTime;
    private List<Node> _nodesList;
    private int _currentNodeIndex;
    private int _numberLevel = 0;

    [SerializeField] private ProceduralSceneConfigurator _configurator;
    [SerializeField] private LevelDataA1 _levelDataA1;


    [SerializeField] private Prefabs prefabs;

    public List<Node> GenerateLvl(bool isFree)
    {
        this._isFree = isFree;
        if (_isFree) data = _configurator;
        else data = _levelDataA1;
        stageIsSet += EnvironmentSet;

        _width = data.GetWidth();
        _height = data.GetHeight();

        prefabs.GetPrefabs(this);

        SetEnviroment();
        float[,] fieldData = GenereteDataForField();
        if(_nodesList == null)
        {
            _nodesList = new List<Node>();
        }
        StartCoroutine(CreateField(fieldData));
        _numberLevel++;
        return _nodesList;

    }

    public void SetPrefabs(Node node1, Node node2, Node node3)
    {
        _nodePrefab1 = node1;
        _nodePrefab2 = node2;
        _nodePrefab3 = node3;
    }

    IEnumerator CreateField(float[,] fieldData)
    {
        while(_currentNodeIndex < _width * _height)
        {
            yield return new WaitForSeconds(speedUnfoldingNodes);
            SetNode(fieldData);
        }
        if(_currentNodeIndex == _width * _height)
        {
            yield return new WaitForSeconds(floorAnimatoinTime);
        }

        StopCoroutine("CreateField");
        stageIsSet?.Invoke();
    }

    void EnvironmentSet()
    {
        _manager.EnablingEnvironment();
    }

    private void SetEnviroment()
    {
        Vector2 centerCoor = new Vector2((float)_width / 2 - 0.5f, (float)_height * _coefficientCells / 2 - 0.5f);
        Camera.main.orthographicSize = _width > _height ? _width : _height;
        Camera.main.transform.position = new Vector3(centerCoor.x, centerCoor.y, -10);
        Camera.main.backgroundColor = new Color(0.109803922f, 0.066666666f, 0.235294118f, 1f);
    }

    private float[,] GenereteDataForField()
    {
        float[,] fieldData = new float[_height * _width, 5];
        int index = 0;
        for (int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                fieldData[index, 0] = j;
                fieldData[index, 1] = i * _coefficientCells;
                fieldData[index, 2] = i * 0.01f;
                fieldData[index, 3] = UnityEngine.Random.value;
                fieldData[index, 4] = i + j;
                index++;
            }
        }

        return SortDataArray(fieldData);
    }

    private float[,] SortDataArray(float[,] fieldData)
    {
        for (int i = 0; i < _height * _width; i++)
        {
            for (int j = 0; j < _height * _width - 1 - i; j++)
            {
                if (fieldData[j,4] > fieldData[j + 1,4])
                {
                    Swap(ref fieldData[j, 0], ref fieldData[j + 1, 0]);
                    Swap(ref fieldData[j, 1], ref fieldData[j + 1, 1]);
                    Swap(ref fieldData[j, 2], ref fieldData[j + 1, 2]);
                    Swap(ref fieldData[j, 3], ref fieldData[j + 1, 3]);
                    Swap(ref fieldData[j, 4], ref fieldData[j + 1, 4]);
                }
            }
        }
        return fieldData;
    }

    public void Swap(ref float aFirstArg, ref float aSecondArg)
    {
        float tmpParam = aFirstArg;
        aFirstArg = aSecondArg;
        aSecondArg = tmpParam;
    }


    private void SetNode(float[,] fieldData)
    {
        Node node;
        if(fieldData[_currentNodeIndex, 3] < 0.33f) {
            node = Instantiate(_nodePrefab1, new Vector3(fieldData[_currentNodeIndex, 0], fieldData[_currentNodeIndex, 1], fieldData[_currentNodeIndex, 2]), Quaternion.identity);
        }
        else if (fieldData[_currentNodeIndex, 3] > 0.66f) {
            node = Instantiate(_nodePrefab2, new Vector3(fieldData[_currentNodeIndex, 0], fieldData[_currentNodeIndex, 1], fieldData[_currentNodeIndex, 2]), Quaternion.identity);
        }
        else {
            node = Instantiate(_nodePrefab3, new Vector3(fieldData[_currentNodeIndex, 0], fieldData[_currentNodeIndex, 1], fieldData[_currentNodeIndex, 2]), Quaternion.identity);
        }
        _currentNodeIndex++;
        _nodesList.Add(node);
    }

    public int GetFieldMaxSize()
    {
        return _width > _height ? _width : _height;
    }

    public void DestroyLvl()
    {
        foreach(Node node in _nodesList)
        {
            Destroy(node.gameObject);
        }
        _nodesList.Clear();
        DestroyEnviroment();
        _currentNodeIndex = 0;
    }

    private void DestroyEnviroment()
    {
        stageIsSet -= EnvironmentSet;
    }
}