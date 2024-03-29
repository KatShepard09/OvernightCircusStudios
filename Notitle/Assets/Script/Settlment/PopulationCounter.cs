using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulationCounter : MonoBehaviour
{
    [SerializeField] private int populationCount = 0;
    [SerializeField] private TMP_Text populationText;

    private void Start()
    {
        UpdatePopulationText();
    }

    public void IncreasePopulation()
    {
        populationCount++;
        UpdatePopulationText();
    }

    public void DecreasePopulation()
    {
        populationCount--;
        UpdatePopulationText();
    }

    public void UpdatePopulationText()
    {
        if (populationText != null)
        {
            populationText.text = $"Population: {populationCount}";
            Debug.Log($"PopulationCounter: Updated population text to {populationCount}");
        }
        else
        {
            Debug.LogWarning("PopulationCounter: Population Text reference is null. Please assign a TMP_Text component.");
        }
    }
}
