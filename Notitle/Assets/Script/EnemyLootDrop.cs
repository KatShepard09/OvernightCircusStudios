using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLootDrop : MonoBehaviour
{
    [Serializable]
    public class LootItem
    {
        public string itemName;
        public int minAmount;
        public int maxAmount;
    }

    [SerializeField] private List<LootItem> lootTable = new List<LootItem>();
    public GameObject lootPanel;
    public TMP_Text lootText;
    public GameObject loadSceneButton;

    public void DropLootAndShowPanel()
    {
        string lootInfo = "";
        foreach (LootItem lootItem in lootTable)
        {
            int amount = UnityEngine.Random.Range(lootItem.minAmount, lootItem.maxAmount + 1);
            int id = 0; // Set a default ID for now, replace with appropriate ID if needed
            ResourceManagement.Instance.AddResources(lootItem.itemName, id, amount);
            lootInfo += $"Dropped {amount} {lootItem.itemName}\n";
        }

        if (lootPanel != null && lootText != null)
        {
            lootPanel.SetActive(true);
            lootText.text = lootInfo;
        }

        if (loadSceneButton != null)
        {
            loadSceneButton.SetActive(true);
        }
    }

    public void LoadSettlementPhase()
    {
        SceneManager.LoadScene("SettlementPhase");
    }

}
