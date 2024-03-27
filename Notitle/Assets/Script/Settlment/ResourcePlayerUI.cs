using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerResources playerResources;
    [SerializeField] private TMP_Text resourceListTextPrefab;
    [SerializeField] private Transform resourceListParent;
    [SerializeField] private float verticalSpacing = 30f;

    private Dictionary<string, TMP_Text> resourceTexts = new Dictionary<string, TMP_Text>();
    private float nextYPosition = 0f;

    private void Start()
    {
        CreateResourceTexts();
        UpdateResourceTexts();
    }

    private void CreateResourceTexts()
    {
        nextYPosition = 0f;

        foreach (PlayerResources.Resource resource in playerResources.Resources)
        {
            CreateResourceText(resource);
        }
    }

    private void CreateResourceText(PlayerResources.Resource resource)
    {
        TMP_Text newText = Instantiate(resourceListTextPrefab, resourceListParent);
        newText.rectTransform.anchoredPosition = new Vector2(0f, -nextYPosition);
        newText.text = $"{resource.Name}: {resource.Amount}";
        resourceTexts.Add(resource.Name, newText);

        // Increase the nextYPosition for the next text
        nextYPosition += verticalSpacing;
    }

    public void UpdateResourceTexts()
    {
        foreach (var pair in resourceTexts)
        {
            string resourceName = pair.Key;
            TMP_Text text = pair.Value;
            PlayerResources.Resource resource = playerResources.GetResource(resourceName);
            if (resource != null)
            {
                text.text = $"{resource.Name}: {resource.Amount}";
            }
        }
    }

    private void OnEnable()
    {
        playerResources.OnResourceChange += UpdateResourceTexts;
    }

    private void OnDisable()
    {
        playerResources.OnResourceChange -= UpdateResourceTexts;
    }
}
