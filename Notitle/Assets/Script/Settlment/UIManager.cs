using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject errorMessagePanel;
    public TextMeshProUGUI errorMessageText;

    public void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessagePanel.SetActive(true);
    }

    public void HideErrorMessage()
    {
        errorMessagePanel.SetActive(false);
    }
}
