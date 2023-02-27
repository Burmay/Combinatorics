using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Block
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected int hp;
    [SerializeField] protected GameManager manager;

    public override void Init(GameManager manager)
    {
        this.manager = manager;
        // характеристики заложены, или выставляются руками?
    }

    public virtual void SubtractHP(int hp)
    {
        if (this.hp - hp > 0)
        {
            this.hp -= hp;
        }
        else
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        manager.RemoveBlock(this);
    }
}
