using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettlementScene : MonoBehaviour
{
    public static SettlementScene Instance;

    [SerializeField] private PopulationCounter populationCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdatePopulationText();
    }

    public void UpdatePopulationText()
    {
        Debug.Log("SettlementScene: UpdatePopulationText called");
        if (populationCounter != null)
        {
            populationCounter.UpdatePopulationText();
        }
    }


    public void GoToSettlement()
    {
        SceneManager.LoadScene("SettlementPhase");
    }
}
