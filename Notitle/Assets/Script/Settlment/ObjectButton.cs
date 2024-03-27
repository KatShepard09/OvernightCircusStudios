using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int ObjectID;
    private Button button;
    private ToolTip tooltip;
    [SerializeField] private ToolTip tooltipPrefab;
    private ObjectDataBase dataBase;
    private ObjectDataBase resources;
    private ObjectDataBase fight;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // Find the necessary databases in the scene
        dataBase = FindObjectOfType<PlacementSystem>().dataBase;
        resources = FindObjectOfType<PlacementSystem>().resources;
        fight = FindObjectOfType<PlacementSystem>().fight;
    }

    public void UpdateButtonInteractability()
    {
        ObjectData objectData = null;

        // Try to get the object data from the first database (dataBase)
        objectData = dataBase.GetObjectByID(ObjectID);

        // If not found in the first database, try the second (resources)
        if (objectData == null)
        {
            objectData = resources.GetObjectByID(ObjectID);
        }

        // If still not found, try the third (fight)
        if (objectData == null)
        {
            objectData = fight.GetObjectByID(ObjectID);
        }

        if (objectData != null)
        {
            button.interactable = objectData.IsUnlocked;
        }
        else
        {
            Debug.LogWarning($"ObjectData with ID {ObjectID} is null.");
            button.interactable = false;
        }
    }

    private void OnClick()
    {
        if (tooltip != null)
        {
            Destroy(tooltip.gameObject);
        }

        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        if (placementSystem != null)
        {
            placementSystem.StartPlacement(ObjectID);
        }
        else
        {
            Debug.LogWarning($"PlacementSystem not found for ObjectButton ID {ObjectID}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipPrefab != null)
        {
            ObjectData objectData = null;

            // Try to get the object data from the first database (dataBase)
            objectData = dataBase.GetObjectByID(ObjectID);

            // If not found in the first database, try the second (resources)
            if (objectData == null)
            {
                objectData = resources.GetObjectByID(ObjectID);
            }

            // If still not found, try the third (fight)
            if (objectData == null)
            {
                objectData = fight.GetObjectByID(ObjectID);
            }

            if (objectData != null)
            {
                string description = objectData.Description;
                string craftingCost = GetCraftingCostText(objectData);

                // Instantiate the tooltip prefab near the button
                tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
                tooltip.ShowTooltip(objectData.Name, description, craftingCost);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            Destroy(tooltip.gameObject);
        }
    }

    private string GetCraftingCostText(ObjectData objData)
    {
        if (objData == null || objData.CraftingCost == null)
        {
            return "Crafting Cost: ";
        }

        string costText = "Crafting Cost:\n";

        foreach (var materialCost in objData.CraftingCost)
        {
            costText += $"{materialCost.MaterialName}: {materialCost.Amount}\n";
        }

        return costText;
    }
}


