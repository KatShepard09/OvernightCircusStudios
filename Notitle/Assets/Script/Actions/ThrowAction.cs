using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAction : BaseAction
{

    private float totalSpinAmount;
    private int maxThrowDistance = 4;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360)
        {
            isActive = false;
            onActionComplete();
        }
    }
    public override string GetActionName()
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
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }

   
}
