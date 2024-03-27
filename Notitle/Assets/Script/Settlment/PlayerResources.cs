using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerResource")]
public class PlayerResources : ScriptableObject
{
    public static PlayerResources Instance;

    public event Action OnResourceChange;

    [Serializable]
    public class Resource
    {
        public string Name;
        public int ID; // Added ID field
        public int Amount;
    }

    public List<Resource> Resources = new List<Resource>();

    private void OnEnable()
    {
        Instance = this; // Assign the instance when the scriptable object is enabled
    }

    public bool HasEnoughResource(string resourceName, int amount)
    {
        Resource resource = GetResource(resourceName);
        if (resource != null)
        {
            return resource.Amount >= amount;
        }
        return false;
    }

    public void AddResource(string resourceName, int id, int amount)
    {
        Resource resource = GetResource(resourceName);
        if (resource != null)
        {
            resource.Amount += amount;
        }
        else
        {
            Resources.Add(new Resource { Name = resourceName, ID = id, Amount = amount });
        }

        OnResourceChange?.Invoke();
    }

    public void DeductResource(string resourceName, int amount)
    {
        Resource resource = GetResource(resourceName);
        if (resource != null)
        {
            resource.Amount -= amount;
            if (resource.Amount < 0)
            {
                Debug.LogWarning($"Player has negative {resourceName}.");
                resource.Amount = 0;
            }
        }
        else
        {
            Debug.LogWarning($"Player does not have {resourceName}.");
        }

        OnResourceChange?.Invoke();
    }

    public Resource GetResource(string resourceName)
    {
        return Resources.Find(r => r.Name == resourceName);
    }

}
