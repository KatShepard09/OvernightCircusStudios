using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public void CraftItem(string sampItem)
    {
        // Check if the player has the required materials in their inventory
        // If so, deduct the materials and create the crafted item

        if (inventoryManager != null)
        {
            // Check if the required materials are present in the inventory
            if (inventoryManager.inventoryItems.Contains("MaterialA"))
            {
                // Craft the item (modify inventory as needed)
                inventoryManager.AddItem(sampItem);
                Debug.Log("Crafted: " + sampItem);
            }
            else
            {
                Debug.Log("Not enough materials to craft: " + sampItem);
            }
        }
    }
}
