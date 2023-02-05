using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _render;
    [SerializeField] private TextMeshPro _text;
    public int value;
    public Node node;

    public Vector2 Pos => transform.position;

    public void Init(BlockType type)
    {
        value = type.value;
        _render.color = type.color;
        _text.text = type.value.ToString();
    }

    public void SetBlock(Node newNode)
    {
        if(node != null) { node.occupiedBlock = null; }
        node = newNode;
        node.occupiedBlock = this;
    }
}
