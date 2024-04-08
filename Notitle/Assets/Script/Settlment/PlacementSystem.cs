using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] public ObjectDataBase dataBase;
    [SerializeField] public ObjectDataBase fight;
    [SerializeField] public ObjectDataBase resources;
    [SerializeField] private PlayerResources playerResource;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private UIManager uiManager;

    private int selectedObjectIndex = -1;
    private bool isObjectPlaced = false;
    private HashSet<int> placedObjectIndices = new HashSet<int>();

    private void Start()
    {
        StopPlacement();
        UpdateButtonInteractability();
    }

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPostion();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (selectedObjectIndex >= 0 || inputManager.IsPointerOverUI())
        {
            return;
        }

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();

        // Search for the object in the "dataBase" database
        selectedObjectIndex = GetObjectIndexByID(ID, dataBase, "dataBase");
        ObjectData selectedObject = null;

        if (selectedObjectIndex >= 0)
        {
            selectedObject = dataBase.objectData[selectedObjectIndex];
        }

        // If not found in "dataBase", search in the "fight" database
        if (selectedObject == null)
        {
            selectedObjectIndex = GetObjectIndexByID(ID, fight, "fight");
            if (selectedObjectIndex >= 0)
            {
                selectedObject = fight.objectData[selectedObjectIndex];
            }
        }

        // If not found in "fight", search in the "resources" database
        if (selectedObject == null)
        {
            selectedObjectIndex = GetObjectIndexByID(ID, resources, "resources");
            if (selectedObjectIndex >= 0)
            {
                selectedObject = resources.objectData[selectedObjectIndex];
            }
        }

        if (selectedObject == null)
        {
            Debug.LogError($"No ID Found {ID}");
            return;
        }

        if (placedObjectIndices.Contains(selectedObjectIndex))
        {
            Debug.LogWarning($"Object {selectedObject.Name} already placed.");
            return;
        }

        if (!CanCraftObject(selectedObject))
        {
            Debug.Log($"Insufficient {selectedObject.CraftingCost[0].MaterialName} to craft {selectedObject.Name}!");
            uiManager.ShowErrorMessage($"Insufficient {selectedObject.CraftingCost[0].MaterialName} to craft {selectedObject.Name}!");
            StartCoroutine(HideErrorMessage(5f)); // Show error for 5 seconds
            return;
        }

        // Enable grid visualization
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

        CraftObject(selectedObject);
        UnlockNextObject(selectedObject);  // Unlock the next object in the tech tree

        UpdateButtonInteractability();
    }

    private int GetObjectIndexByID(int ID, ObjectDataBase objectDataBase, string dbName)
    {
        if (objectDataBase == null || objectDataBase.objectData == null)
        {
            Debug.LogWarning($"ObjectDataBase {dbName} is null or objectData is null.");
            return -1;
        }

        int index = objectDataBase.objectData.FindIndex(data => data.ID == ID);

        if (index == -1)
        {
            Debug.LogWarning($"ObjectData with ID {ID} not found in {dbName}.");
        }

        return index;
    }

    public bool CanCraftObject(ObjectData objData)
    {
        if (objData == null)
        {
            Debug.LogError("ObjectData is null.");
            return false;
        }

        if (objData.CraftingCost == null)
        {
            Debug.LogError($"CraftingCost for {objData.Name} is null.");
            return false;
        }

        foreach (var materialCost in objData.CraftingCost)
        {
            if (!playerResource.HasEnoughResource(materialCost.MaterialName, materialCost.Amount))
            {
                return false;
            }
        }

        return true;
    }

    private void CraftObject(ObjectData objData)
    {
        if (!CanCraftObject(objData))
        {
            Debug.LogError($"Cannot craft object {objData.Name}. Insufficient resources.");
            return;
        }

        foreach (var materialCost in objData.CraftingCost)
        {
            playerResource.DeductResource(materialCost.MaterialName, materialCost.Amount);
        }

        SetObjectButtonInteractability(objData.ID, false);

        GameObject newObject = Instantiate(objData.Prefab);
        // Additional setup for the new object, like positioning, etc.

        // Set the flag to indicate that an object is placed
        isObjectPlaced = true;
        placedObjectIndices.Add(selectedObjectIndex);
    }

    private void SetObjectButtonInteractability(int objectID, bool interactable)
    {
        foreach (Transform buttonTransform in buildPanel.transform)
        {
            ObjectButton objectButton = buttonTransform.GetComponent<ObjectButton>();
            if (objectButton != null && objectButton.ObjectID == objectID)
            {
                buttonTransform.GetComponent<Button>().interactable = interactable;
                objectButton.gameObject.SetActive(interactable); // Turn off the button GameObject
                break;
            }
        }
    }

    private void UnlockNextObject(ObjectData objData)
    {
        int nextID = objData.ID + 1;
        ObjectData nextObject = null;

        // Try to get the next object from the first database (dataBase)
        nextObject = dataBase.GetObjectByID(nextID);

        // If not found in the first database, try the second (resources)
        if (nextObject == null)
        {
            nextObject = resources.GetObjectByID(nextID);
        }

        // If still not found, try the third (fight)
        if (nextObject == null)
        {
            nextObject = fight.GetObjectByID(nextID);
        }

        if (nextObject != null)
        {
            nextObject.IsUnlocked = true;
            SetObjectUnlocked(nextObject.ID, true);
            SetObjectButtonInteractability(nextObject.ID, true);
        }
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPostion();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(dataBase.objectData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        // Set the flag to indicate that an object is placed
        isObjectPlaced = true;
        placedObjectIndices.Add(selectedObjectIndex);

        // Disable grid after placing the object
        gridVisualization.SetActive(false);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        // Reset the flag when placement stops
        isObjectPlaced = false;
    }

    private void UpdateButtonInteractability()
    {
        foreach (Transform buttonTransform in buildPanel.transform)
        {
            ObjectButton objectButton = buttonTransform.GetComponent<ObjectButton>();
            if (objectButton != null)
            {
                ObjectData objData = dataBase.GetObjectByID(objectButton.ObjectID);

                if (objData == null)
                {
                    objData = resources.GetObjectByID(objectButton.ObjectID);
                }

                if (objData == null)
                {
                    objData = fight.GetObjectByID(objectButton.ObjectID);
                }

                if (objData != null)
                {
                    bool canCraft = CanCraftObject(objData);
                    bool isUnlocked = IsObjectUnlocked(objectButton.ObjectID);
                    buttonTransform.GetComponent<Button>().interactable = canCraft && isUnlocked;
                }
                else
                {
                    // ObjectData is null
                    Debug.LogError($"ObjectData with ID {objectButton.ObjectID} is null.");
                }
            }
        }
    }

    public bool IsObjectUnlocked(int objectID)
    {
        ObjectData objectData = null;

        // Check the first ObjectDataBase (dataBase)
        objectData = dataBase.GetObjectByID(objectID);

        // If not found in the first database, check the second (resources)
        if (objectData == null)
        {
            objectData = resources.GetObjectByID(objectID);
        }

        // If not found in the resources database, check the third (fight)
        if (objectData == null)
        {
            objectData = fight.GetObjectByID(objectID);
        }

        return objectData != null && objectData.IsUnlocked;
    }

    public void SetObjectUnlocked(int objectID, bool isUnlocked)
    {
        ObjectData objectData = null;

        // Check the first ObjectDataBase (dataBase)
        objectData = dataBase.GetObjectByID(objectID);

        // If not found in the first database, check the second (resources)
        if (objectData == null)
        {
            objectData = resources.GetObjectByID(objectID);
        }

        // If not found in the resources database, check the third (fight)
        if (objectData == null)
        {
            objectData = fight.GetObjectByID(objectID);
        }

        if (objectData != null)
        {
            objectData.IsUnlocked = isUnlocked;
            Debug.Log($"Object with ID {objectID} is now {(isUnlocked ? "unlocked" : "locked")}");
        }
        else
        {
            Debug.LogWarning($"Object with ID {objectID} not found");
        }

        UpdateButtonInteractability();
    }

    private IEnumerator HideErrorMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.HideErrorMessage();
    }
}
