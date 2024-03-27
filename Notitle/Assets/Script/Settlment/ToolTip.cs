using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI craftingCostText;
    [SerializeField] private RectTransform backgroundRectTransform;

    public void ShowTooltip(string objectName, string description, string craftingCost)
    {
        objectNameText.text = objectName;
        descriptionText.text = description;
        craftingCostText.text = craftingCost;

        gameObject.SetActive(true);

        // Adjust the background size based on text content
        Vector2 backgroundSize = new Vector2(
            Mathf.Max(objectNameText.preferredWidth, descriptionText.preferredWidth, craftingCostText.preferredWidth) + 20f,
            objectNameText.preferredHeight + descriptionText.preferredHeight + craftingCostText.preferredHeight + 20f
        );
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
