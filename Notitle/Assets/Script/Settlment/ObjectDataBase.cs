using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectDataBase : ScriptableObject
{
    public List<ObjectData> objectData;

    public ObjectData GetObjectByID(int id)
    {
        return objectData.Find(obj => obj.ID == id);
    }
}

[Serializable]
public class ObjectData
{
    [SerializeField] public string Name;
    [SerializeField] public int ID;
    [SerializeField] public string Description;
    [SerializeField] public Vector2Int Size;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public List<MaterialCost> CraftingCost;
    [SerializeField] public bool IsUnlocked;
}

[Serializable]
public class MaterialCost
{
    [SerializeField] public string MaterialName;
    [SerializeField] public int Amount;
}
