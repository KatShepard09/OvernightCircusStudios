using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDataBase dataBase;

    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisulization;

    private void Start()
    {
        StopPlacment();
    }

    public void StartPlacment(int ID)
    {
        StopPlacment();
        selectedObjectIndex = dataBase.objectData.FindIndex(data => data.ID == ID);//looks for the items ID from the list and allows it to be placed on the grid.
        if(selectedObjectIndex < 0) 
        {
            Debug.LogError($"No ID Found {ID}");
            return;
        }
        gridVisulization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacment;
    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePostion = inputManager.GetSelectedMapPostion();
        Vector3Int gridPostion = grid.WorldToCell(mousePostion);
        GameObject newObject = Instantiate(dataBase.objectData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPostion);
    }

    private void StopPlacment()
    {
        selectedObjectIndex = -1;
        gridVisulization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacment;
    }
    private void Update()
    {
        if(selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePostion = inputManager.GetSelectedMapPostion();
        Vector3Int gridPostion = grid.WorldToCell(mousePostion);
        mouseIndicator.transform.position = mousePostion;
        cellIndicator.transform.position = grid.CellToWorld(gridPostion);
    }
}
