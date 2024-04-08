using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowAction : BaseAction
{
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioSource miss;

    private enum State
    {
        Aiming,
        Throwing,
        Cooloff,
    }

    public event EventHandler OnThrow;
  
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
        OnThrow?.Invoke(this, EventArgs.Empty);
        targetUnit.Damage(40);
        hit.Play();
    }
    public override string GetActionName()//creates Throw button.
    {
        return "Throw";
    }
    public override List<GridPostion> GetValidActionGridPostionList()
    {
        GridPostion unitGridPostion = unit.GetGridPostion();
        return GetValidActionGridPostionList(unitGridPostion);
    }

    public  List<GridPostion> GetValidActionGridPostionList(GridPostion unitGridPostion)
    {
        List<GridPostion> validGridPostionList = new List<GridPostion>();

       

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

    public override EnemyAIAction GetEnemyAIAction(GridPostion gridPostion)
    {
       

        return new EnemyAIAction
        {
            gridPostion = gridPostion,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtTargetPostion(GridPostion gridPostion)
    {
        return GetValidActionGridPostionList(gridPostion).Count;
    }
}
