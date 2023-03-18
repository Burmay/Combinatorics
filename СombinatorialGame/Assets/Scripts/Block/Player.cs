using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start()
    {
        animator.GetComponent<Animator>();
        manager.SetPlayerLink = this;
    }

    public override Block Init(GameManager manager)
    {
        base.Init(manager);
        return this;
    }

    public override void Move()
    {
        if (animator != null)
        {
            animator.SetTrigger("Move");
        }
    }

    protected override void Die()
    {
        Debug.Log("Перс сдох");
        base.Die();
        manager._state = GameState.Lose;
    }
}
