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
            isActive = false;
            onActionComplete();
        }
    }

    public override void TakeAction(GridPostion gridPostion, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
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
  
}
