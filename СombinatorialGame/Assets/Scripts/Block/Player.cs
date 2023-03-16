using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start()
    {
        animator.GetComponent<Animator>();
        base.type = BlockType.Player;
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
        Debug.Log(manager);
        base.Die();
        manager._state = GameState.Lose;
    }
}
