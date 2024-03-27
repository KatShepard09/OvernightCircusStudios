using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulationCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text populationText;
    private int playerPopulation = 3; // Initial population count

    private void Start()
    {
        UpdatePopulationText();
    }

    public void UpdatePopulationText()
    {
        if (populationText != null)
        {
            populationText.text = $"Population: {playerPopulation}";
        }
    }

    public void DecreasePopulation()
    {
        playerPopulation--;
        if (playerPopulation < 0)
        {
            playerPopulation = 0;
        }
        UpdatePopulationText();
    }
}
