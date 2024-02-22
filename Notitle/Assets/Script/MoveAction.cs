using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 targetPosition;
   

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive)//checks to see if the move action is active if it no other action can be done until complete.
        {
            return;
        }
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float stoppingDistance = .1f;//Tells unit to stop moving when it is .1 away from location.
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            float moveSpeed = 4f;//movement speed of character
            transform.position += moveDirection * moveSpeed * Time.deltaTime;//keeps track of player movment and the amount of frames at which it moves.


            
        }
        else
        {
            

            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);//rotates charcter based on direction that they are moveing
    }

    public override void TakeAction(GridPostion gridPostion, Action onActionComplete)
    {
        ActionStart(onActionComplete);
       
        this.targetPosition = LevelGrid.Instance.GetWorldPostion(gridPostion);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
       
    }

   

    public override List<GridPostion> GetValidActionGridPostionList()
    {
        List<GridPostion> validGridPostionList = new List<GridPostion>();

        GridPostion unitGridPostion = unit.GetGridPostion();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPostion offsetGridPostion = new GridPostion(x, z);
                GridPostion testGridPostion = unitGridPostion + offsetGridPostion;

                if (!LevelGrid.Instance.IsValidGridPostion(testGridPostion))//check to make sure valid spaces stay in the grid.
                {
                    continue;
                }

                if (unitGridPostion == testGridPostion)
                {
                    //Same Grid Postion where unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasUnitOnGridPostion(testGridPostion))
                {
                    //Grid space has a unit there already
                    continue;
                }

                validGridPostionList.Add(testGridPostion);
            }
        }

        return validGridPostionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPostion gridPostion)
    {
        int targetCountAtGridPosition = unit.GetThrowAction().GetTargetCountAtTargetPostion(gridPostion);
        return new EnemyAIAction
        {
            gridPostion = gridPostion,
            actionValue = targetCountAtGridPosition * 10,// this tells the AI to move to where there is a bigger group to attack.
        };
    }

   
}
