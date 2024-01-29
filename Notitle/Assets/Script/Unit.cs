using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private GridPostion gridPostion;
    private MoveAction moveAction;
    private SpinAction spinAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
    }



    private void Start()
    {

        gridPostion = LevelGrid.Instance.GetGridPostion(transform.position);//tells the grid if a unit is standing on it.
        LevelGrid.Instance.AddUnitGridPostion(gridPostion, this);
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
}
