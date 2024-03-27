using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private PopulationCounter populationCounter;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        populationCounter = FindObjectOfType<PopulationCounter>();
    }

    public void UnitDestroyed(bool isEnemy)
    {
        if (!isEnemy && populationCounter != null)
        {
            populationCounter.DecreasePopulation();
        }
    }
}
