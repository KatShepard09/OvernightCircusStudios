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

    protected void ActionStart(Action onActionComplete)//this tells each action if it is active if so do the action.
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
    }

    protected void ActionComplete()//this tells each action that once it completes what it is suppose to do to stop doing it.
    {
        isActive = false;
        onActionComplete();
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionsList = new List<EnemyAIAction>();

        List<GridPostion> validActionGridPostionList = GetValidActionGridPostionList();

        foreach(GridPostion gridPostion in validActionGridPostionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPostion);
            enemyAIActionsList.Add(enemyAIAction);
        }

        if(enemyAIActionsList.Count > 0)
        {
            enemyAIActionsList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionsList[0];
        }
        else
        {
            return null;//no AI actions to take.
        }

        
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPostion gridPostion);
}
