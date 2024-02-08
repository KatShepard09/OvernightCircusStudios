using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener(() =>
        {
            CharacterActionSystem.Instance.SetSelectedAction(baseAction);//allows buttons to be clicked
        });
    }

    public void UpdatedSelectedVisual()
    {
        BaseAction selectedBaseAction = CharacterActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);//tells the game which button to highlight when clicked.
    }
}
