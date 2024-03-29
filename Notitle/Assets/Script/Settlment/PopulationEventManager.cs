using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationEventManager : MonoBehaviour
{
    public static PopulationEventManager Instance;

    private PopulationCounter populationCounter;

    private void Awake()
    {
        Instance = this;
        populationCounter = GetComponent<PopulationCounter>();
    }

    public void UnitDied()
    {
        if (populationCounter != null)
        {
            populationCounter.DecreasePopulation();
            populationCounter.UpdatePopulationText();
        }
    }
}
