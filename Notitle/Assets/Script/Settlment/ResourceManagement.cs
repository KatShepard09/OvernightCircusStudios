using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    public static ResourceManagement Instance;

    [SerializeField] private PlayerResources playerResources;
    [SerializeField] private PlacementSystem placementSystem;

    private void Awake()
    {
        Instance = this;
    }

    // Function to add resources
    public void AddResources(string resourceName, int id, int amount)
    {
        playerResources.AddResource(resourceName, id, amount);
    }

    public void DeductResources(string resourceName, int amount)
    {
        playerResources.DeductResource(resourceName, amount);
    }
}
