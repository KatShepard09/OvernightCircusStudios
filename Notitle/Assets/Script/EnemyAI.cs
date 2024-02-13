using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChanged;//event for the enemy to end its turn.
    }

    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())//if it is the players turn the AI will do nothing.
        {
            return;
        }

        switch(state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = State.Busy;
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    { 
                        TurnSystem.Instance.NextTurn();//if Ai has no more actions to take they will end their turn.
                    }

                  
                    
                }
                break;
            case State.Busy:
                break;
        }

    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)//listener for enemy to end its turn.
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
     
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)//will check to see if their are any enemy units left to take their turn.
    {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            { 
                return true; 
            }
           
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)//checks for vaild grid postions for an action then sees if they have action points for said action.
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPostion actionGridPostion = enemyUnit.GetGridPostion();

        if (!spinAction.IsValidActionGridPostion(actionGridPostion))
        {
            return false;
           

        }
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            return false;


        }
       
        spinAction.TakeAction(actionGridPostion, onEnemyAIActionComplete);
        return true;
    }
}
