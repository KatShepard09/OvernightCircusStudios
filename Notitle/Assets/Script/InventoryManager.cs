using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<string> inventoryItems = new List<string>();

    //Adding sample item to inventory
    public void AddItem(string sampItem)
    {
        inventoryItems.Add(sampItem);
     
    }

}
