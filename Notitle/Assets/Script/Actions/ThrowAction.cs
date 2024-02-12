using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowAction : BaseAction
{

    private enum State
    {
        Aiming,
        Throwing,
        Cooloff,
    }

   private State state;
    private int maxThrowDistance = 4;//how far the unit can throw.
    private float stateTimer;
    private Unit targetUnit;
    private bool canThrowObject;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;

        switch(state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPostion() - unit.GetWorldPostion()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Throwing:
               if (canThrowObject)
                {
                    Throw();
                    canThrowObject = false;
                }
                break;
            case State.Cooloff:
               
                break;

        }

        if(stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch (state)//slows down combat into three phases of an attack.
        {
            case State.Aiming:
                if (stateTimer <= 0)
                {
                    state = State.Throwing;
                    float throwingStateTime = 0.1f;
                    stateTimer = throwingStateTime;
                }
                break;
            case State.Throwing:
                if (stateTimer <= 0)
                {
                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                }
                break;
            case State.Cooloff:
                if (stateTimer <= 0)
                {
                    isActive = false;
                    ActionComplete();
                }
                break;

        }
    }

    private void Throw()
    {
        targetUnit.Damage();
    }
    public override string GetActionName()//creates Throw button.
    {
        return "Throw";
    }

    public override List<GridPostion> GetValidActionGridPostionList()
    {
        List<GridPostion> validGridPostionList = new List<GridPostion>();

        GridPostion unitGridPostion = unit.GetGridPostion();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPostion offsetGridPostion = new GridPostion(x, z);
                GridPostion testGridPostion = unitGridPostion + offsetGridPostion;

                if (!LevelGrid.Instance.IsValidGridPostion(testGridPostion))//check to make sure valid spaces stay in the grid.
                {
                    continue;
                }

               

                if (!LevelGrid.Instance.HasUnitOnGridPostion(testGridPostion))
                {
                    //Grid space does not have anyone on grid space.
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPostion(testGridPostion);

                if(targetUnit.IsEnemy() == unit.IsEnemy())//this checks to see if the characters on the same team if so they are not selected.
                {
                    continue;
                }

                validGridPostionList.Add(testGridPostion);
            }
        }

        return validGridPostionList;

      
    }

    public override void TakeAction(GridPostion gridPostion, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;


        targetUnit = LevelGrid.Instance.GetUnitAtGridPostion(gridPostion);

        canThrowObject = true;
    }

   
}
