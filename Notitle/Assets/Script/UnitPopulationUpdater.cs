using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPopulationUpdater : MonoBehaviour
{
    public static UnitPopulationUpdater Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void NotifyUnitDied()
    {
        Debug.Log("UnitPopulationUpdater: NotifyUnitDied called");
        if (GameManager.Instance != null && GameManager.Instance.populationCounter != null)
        {
            GameManager.Instance.populationCounter.DecreasePopulation();
            GameManager.Instance.populationCounter.UpdatePopulationText();
        }
        else
        {
            Debug.LogWarning("UnitPopulationUpdater: GameManager instance or PopulationCounter is null");
        }
    }
}
