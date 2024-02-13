using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent<ThrowAction>(out ThrowAction throwAction))
        {
            throwAction.OnThrow += ThrowAction_OnThrow;
        }

    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e) 
    {
        animator.SetBool("IsWalking", false);
    }

    private void ThrowAction_OnThrow(object sender, EventArgs e)
    {
        animator.SetTrigger("Throw");
    }
}
