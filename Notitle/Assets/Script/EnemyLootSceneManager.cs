using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyLootSceneManager : MonoBehaviour
{
    private static EnemyLootSceneManager instance;

    public static EnemyLootSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyLootSceneManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(EnemyLootSceneManager).Name;
                    instance = obj.AddComponent<EnemyLootSceneManager>();
                }
            }
            return instance;
        }
    }

    private string sceneToLoad = "SettlementPhase"; // Name of the Settlement Phase scene

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSettlementPhaseWithLoot()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

