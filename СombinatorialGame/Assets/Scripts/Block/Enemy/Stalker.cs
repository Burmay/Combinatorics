using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    private void Start()
    {
        manager.SetStalkerLink = this;
    }
}
