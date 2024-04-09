using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Image unitImage; // Reference to the UI Image component
    [SerializeField] private Text healthText; // Reference to the UI Text component for health

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

    private void UpdateVisual()
    {
        if (CharacterActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;

            // Load the unit's sprite onto the UI Image component
            if (unitImage != null && unit != null)
            {
                Sprite unitSprite = unit.GetUnitSprite();
                if (unitSprite != null)
                {
                    unitImage.sprite = unitSprite;
                }
                else
                {
                    Debug.LogWarning("Unit sprite not set for " + unit.name);
                }
            }
            else
            {
                Debug.LogWarning("Unit image component or unit reference not set.");
            }

            // Update the health text
            if (healthText != null && unit != null)
            {
                healthText.text = "Health: " + unit.GetHealth().ToString();
            }
            else
            {
                Debug.LogWarning("Health text component or unit reference not set.");
            }
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
