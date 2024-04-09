using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EnemyLootDrop;

public class ResourceDrop : MonoBehaviour
{
    [Serializable]
    public class ResourceItem
    {
        public string resourceName;
        public int minAmount;
        public int maxAmount;
    }

    [SerializeField] private List<LootItem> lootTable = new List<LootItem>();
    public GameObject dropPanel;
    public TMP_Text dropText;
    public GameObject loadSceneButton;

    public static ResourceDrop Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DropResourcesAndShowPanel()
    {
        string lootInfo = "";

        foreach (LootItem lootItem in lootTable)
        {
            int amount = UnityEngine.Random.Range(lootItem.minAmount, lootItem.maxAmount + 1);
            int id = 0; // Set a default ID for now, replace with appropriate ID if needed
            ResourceManagement.Instance.AddResources(lootItem.itemName, id, amount);
            lootInfo += $"Dropped {amount} {lootItem.itemName}\n";
        }

        if (dropPanel != null && dropText != null)
        {
            dropPanel.SetActive(true);
            dropText.text = lootInfo;
        }

        if (loadSceneButton != null)
        {
            loadSceneButton.SetActive(true);
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("SettlementPhase");
    }
}
