using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{

    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 targetPosition;
    private Unit unit;
    private bool isActive;

    private void Awake()
    {
        unit = GetComponent<Unit>();
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


            unitAnimator.SetBool("IsWalking", true);//when charcter moves it activates the move animation.
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);// when character is not moving it activates the idle animation.
            isActive = false;
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);//rotates charcter based on direction that they are moveing
    }

    public void Move(GridPostion gridPostion)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPostion(gridPostion);
        isActive = true;
    }

    public bool IsValidActionGridPostion(GridPostion gridPostion)
    {
        List<GridPostion> validGridPostionList = GetValidActionGridPostionList();
        return validGridPostionList.Contains(gridPostion);
    }

    public List<GridPostion> GetValidActionGridPostionList()
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
}
