using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Block
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected int hp;
    [SerializeField] protected GameManager manager;
    protected bool shield;

    public override void Init(GameManager manager)
    {
        this.manager = manager;
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

    private void ShieldOff()
    {
        shield = false;
    }

    protected virtual void Die()
    {
        manager.RemoveBlock(this);
    }
}
