using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private GameManager _manager;

    void Start()
    {
        animator.GetComponent<Animator>();
        base.type = BlockType.Player;
    }

    public Player Initialize(GameManager manager)
    {
        _manager = manager;
        return this;
    }

    public void ChangeMotionStatus(bool motion)
    {
        if(animator != null)
        {
            animator.SetBool("Move", motion);
        }
    }

    protected override void Die()
    {
        Debug.Log("Перс сдох");
        base.Die();
        _manager.ChangeState(GameState.Lose);
    }
}
