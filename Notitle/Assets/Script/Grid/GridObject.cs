using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private GridSystem gridSystem;
    private GridPostion gridPostion;
    private List <Unit> unitList;
   

    public GridObject(GridSystem gridSystem,GridPostion gridPostion)
    {
        this.gridPostion = gridPostion;
        this.gridSystem = gridSystem;
        unitList = new List <Unit>();
    }

    public override string ToString()
    {
        string unitString = " ";
        foreach(Unit unit in unitList)
        {
            unitString += unit + "\n";
        }
        return gridPostion.ToString() + "\n" + unitString;

    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit) 
    {
        unitList.Remove(unit);
    }

    public List <Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit() 
    { 
        return unitList.Count > 0;
    }
}
