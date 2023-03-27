using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Block occupiedBlock;
    public bool generationAvalible;

    private void Awake()
    {
        generationAvalible = true;
    }

    public Vector2 Pos => transform.position;
}
