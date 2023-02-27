using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    void Start()
    {
        animator.GetComponent<Animator>();
    }

    public Player Initialize()
    {
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
    }
}
