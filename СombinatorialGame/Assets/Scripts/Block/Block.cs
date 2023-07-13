using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Node node;
    public int mobile;
    public Block mergingBlock;
    public BlockType type;
    protected GameManager manager;

    public Vector2 Pos => transform.position;

    public virtual Block Init(GameManager manager)
    {
        this.manager = manager;
        return this;
    }

    public void ChangeNode(Node newNode)
    {
        if(node != null) { node.occupiedBlock = null; }
        node = newNode;
        node.occupiedBlock = this;
    }

    public void MergeBlock(Block blockMergeWith)
    {
        mergingBlock = blockMergeWith;
        node.occupiedBlock = null;
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
}
