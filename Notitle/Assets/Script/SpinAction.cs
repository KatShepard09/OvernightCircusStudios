using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
 
    private float totalSpinAmount;
   

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
            ActionComplete();
        }
    }

    public override void TakeAction(GridPostion gridPostion, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "spin";
    }

    public override List<GridPostion> GetValidActionGridPostionList()
    {
      
        GridPostion unitGridPostion = unit.GetGridPostion();

        return new List<GridPostion>
        {
            unitGridPostion
        };
    }

    public override int GetActionCost()
    {
        return 2;//spin action cost two points.
    }

    public override EnemyAIAction GetEnemyAIAction(GridPostion gridPostion)
    {
        return new EnemyAIAction
        {
            gridPostion = gridPostion,
            actionValue = 0,
        };
    }

}
