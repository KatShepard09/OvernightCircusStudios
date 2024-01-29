using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionSystem : MonoBehaviour
{
    public static CharacterActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedCharacterChanged;


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

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

    private void Update()
    {


        if (Input.GetMouseButtonDown(0))//charcter moves to location of where the mouse was clicked
        {
            if (HandleCharacterSelection()) return;

            GridPostion mouseGridPostion = LevelGrid.Instance.GetGridPostion(MouseWorld.GetPosition());
            if (selectedUnit.GetMoveAction().IsValidActionGridPostion(mouseGridPostion))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPostion);
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedUnit.GetSpinAction().Spin();
        }
    }

    private bool HandleCharacterSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Raycast checking to see if the mouse is touching an object if true tells location of object.

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))//checks to see if the mouse is clicking a unit if true then that character selected.
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedCharacterChanged?.Invoke(this, EventArgs.Empty);//Checks to see if a character is selected.
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
