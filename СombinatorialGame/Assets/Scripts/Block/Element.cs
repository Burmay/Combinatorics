using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : Block
{
    public int orderElement;
    public Elements type;
    public int damage;

}

public enum Elements
{
    Null,
    Fire,
    Water,
    Stone,
    Lava,
    Steam,
    Plant
}
