using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterActionSystem : MonoBehaviour
{
    public static CharacterActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedCharacterChanged;//event that handles character select
    public event EventHandler OnSelectedActionChanged;// event that handles button highlight
    public event EventHandler OnActionStarted;// event that handles action points each turn.


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more then one character selected" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if(isBusy)
        {
            return;
        }

        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

       
        
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

            if (HandleCharacterSelection()) 
            {
            return;
            }

            HandleSelectedAction();

        

       
    }

    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPostion mouseGridPostion = LevelGrid.Instance.GetGridPostion(MouseWorld.GetPosition());

            if(selectedAction.IsValidActionGridPostion(mouseGridPostion))
            {
                if(selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPostion, ClearBusy);

                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                }
                
            }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    private void ClearBusy()
    { 
        isBusy = false; 
    }

    private bool HandleCharacterSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Raycast checking to see if the mouse is touching an object if true tells location of object.

            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))//checks to see if the mouse is clicking a unit if true then that character selected.
                {
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
                    if(unit.IsEnemy())//this is so you can not click and move eneimes.
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
      
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedCharacterChanged?.Invoke(this, EventArgs.Empty);//Checks to see if a character is selected.
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
