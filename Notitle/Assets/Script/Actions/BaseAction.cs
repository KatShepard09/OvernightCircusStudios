using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour//this script tells the game which player is active and if the action they are doing is complete or not.
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;


    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPostion gridPostion, Action onActionComplete);

    public bool IsValidActionGridPostion(GridPostion gridPostion)
    {
        List<GridPostion> validGridPostionList = GetValidActionGridPostionList();
        return validGridPostionList.Contains(gridPostion);
    }

    public abstract List<GridPostion> GetValidActionGridPostionList();
   
    public virtual int GetActionCost()
    {
        return 1;//move action cost 1 action point.
    }
}
