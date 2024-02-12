using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;

    private GridPostion gridPostion;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;//starting action points.
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }



    private void Start()
    {

        gridPostion = LevelGrid.Instance.GetGridPostion(transform.position);//tells the grid if a unit is standing on it.
        LevelGrid.Instance.AddUnitGridPostion(gridPostion, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void Update()
    {


        GridPostion newGridPostion = LevelGrid.Instance.GetGridPostion(transform.position);// keeps grid updated when a unit moves.
        if (newGridPostion != gridPostion)
        {
            //unit changed postion.
            LevelGrid.Instance.UnitMovedGridPostion(this, gridPostion, newGridPostion);
            gridPostion = newGridPostion;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPostion GetGridPostion()
    {
        return gridPostion;
    }

    public Vector3 GetWorldPostion()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    { 
        return baseActionArray; 
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)//checks to see if the player has any action points to spend if so spend an action point.
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionCost());
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)//spends action points if the player is allowed too.
    {
        if(actionPoints >= baseAction.GetActionCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)//keeps track of what action points the player has left.
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    { 
        return actionPoints; 
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))//checks to see whos turn it is and reset the action points for that turn.
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
       
    }

    public bool IsEnemy()//checks to see if it is an enemy.
    {
        return isEnemy;
    }

    public void Damage()
    {
        Debug.Log(transform + " damaged!");
    }
}
