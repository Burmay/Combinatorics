using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Block
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected int hp;
    [SerializeField] protected bool shield;
    public bool stun;

    public override Block Init(GameManager manager)
    {
        base.Init(manager);
        stun = false;
        return this;
        // характеристики заложены, или выставляются руками?
    }

    public int HP => hp;
    public bool Shield => shield;

    public virtual void SubtractHP(int hp)
    {
        if(shield == true) { ShieldOff(); }
        else if (this.hp - hp > 0)
        {
            this.hp -= hp;
        }
        else
        {
            Die();
        }
    }

    public void ShieldOn()
    {
        shield = true;
    }

    public void ShieldOff()
    {
        shield = false;
    }

    public void StunOn()
    {
        stun = true;
    }
    public void StunOff()
    {
        stun = false;
    }

    public virtual void Move()
    {

    }

    protected virtual void Die()
    {
        manager.RemoveBlock(this);
    }
}
