using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneBuilder : MonoBehaviour
{
    [SerializeField] private int _width = 6, _height = 6;
    [SerializeField] private Node _nodePrefab1, _nodePrefab2, _nodePrefab3;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private float _coefficientCells;
    [SerializeField] private GameManager _manager;
    [SerializeField] private float speedUnfoldingNodes;
    private List<Node> _nodesList;
    private int _currentNodeIndex;

    public List<Node> GenerateLvl()
    {
        SetEnviroment();
        float[,] fieldData = GenereteDataForField();
        _nodesList = new List<Node>();
        StartCoroutine(CreateField(fieldData));
        return _nodesList;
    }

    IEnumerator CreateField(float[,] fieldData)
    {
        while(_currentNodeIndex < _width * _height)
        {
            yield return new WaitForSeconds(speedUnfoldingNodes); ; // скорость развёртки
            SetNode(fieldData);
        }
        if(_currentNodeIndex == _width * _height)
        {

            StopCoroutine("CreateField");
        }
    }

    private void CreatePlayer()
    {

    }

    private void SetEnviroment()
    {
        Vector2 centerCoor = new Vector2((float)_width / 2 - 0.5f, (float)_height * _coefficientCells / 2 - 0.5f);
        var board = Instantiate(_boardPrefab, centerCoor, Quaternion.identity);
        board.size = new Vector2(_width, _height * _coefficientCells);
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
            //_nodesList.Remove(node);
            //Destroy(node.gameObject);
        }
    }
}