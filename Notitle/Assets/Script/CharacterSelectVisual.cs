using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectVisual : MonoBehaviour
{
   [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        CharacterActionSystem.Instance.OnSelectedCharacterChanged += CharacterActionSystem_OnSelectedCharacterChanged;
        UpdateVisual();
    }

    private void CharacterActionSystem_OnSelectedCharacterChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()//loads the select icon so the player knows who they are controlling.
    {

        if (CharacterActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        CharacterActionSystem.Instance.OnSelectedCharacterChanged -= CharacterActionSystem_OnSelectedCharacterChanged;
    }
}
