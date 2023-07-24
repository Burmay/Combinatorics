using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected override void Die()
    {
        Debug.Log("The enemy is dead");
        base.Die();
    }

    public override Block Init(GameManager manager)
    {
        base.Init(manager);
        return this;
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
