using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPostion;

    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem gridSystem;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more then one character selected" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(8, 12, 2f);//Creates grid hight and width and then how far apart each grid piece is.
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

    }

    public void AddUnitGridPostion(GridPostion gridPostion, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostion);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitGridPostion(GridPostion gridPostion)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostion);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitGridPostion(GridPostion gridPostion, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostion);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPostion(Unit unit, GridPostion fromGridPostion, GridPostion toGridPostion)
    {
        RemoveUnitGridPostion(fromGridPostion, unit);

        AddUnitGridPostion(toGridPostion, unit);

        OnAnyUnitMovedGridPostion?.Invoke(this, EventArgs.Empty);
    }

    public GridPostion GetGridPostion(Vector3 worldPostion)
    {
        return gridSystem.GetGridPostion(worldPostion);

    }

    public Vector3 GetWorldPostion(GridPostion gridPostion) => gridSystem.GetWorldPostion(gridPostion);

    public bool IsValidGridPostion(GridPostion gridPostion) => gridSystem.IsValidGridPostion(gridPostion);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool HasUnitOnGridPostion(GridPostion gridPostion)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostion);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPostion(GridPostion gridPostion)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPostion);
        return gridObject.GetUnit();
    }



    private void Update()
    {
       
    }

}
