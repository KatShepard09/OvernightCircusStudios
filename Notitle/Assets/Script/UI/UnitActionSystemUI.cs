using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefabe;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUi> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUi>();
    }

    void Start()
    {
        CharacterActionSystem.Instance.OnSelectedCharacterChanged += CharacterActionSystem_OnSelectedCharacterChange;//listener for character action
        CharacterActionSystem.Instance.OnSelectedActionChanged += CharacterActionSystem_OnSelectedActionChange;
        CharacterActionSystem.Instance.OnActionStarted += CharacterActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChange;
        UpdateActionPoints();
        CreateUnitActionButton();
        UpdateSelectedVisual();
    }

   
   private void CreateUnitActionButton()//creates buttons for UI
   {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);//Destroys buttons that are nor longer relvent to a given character.
        }
        actionButtonUIList.Clear();
       Unit selectedUnit = CharacterActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
          Transform actionButtonTransform =  Instantiate(actionButtonPrefabe, actionButtonContainerTransform);
           ActionButtonUi actionButtonUi = actionButtonTransform.GetComponent<ActionButtonUi>();
            actionButtonUi.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUi);
        }
   }

    private void CharacterActionSystem_OnSelectedCharacterChange(object sender, EventArgs e)//creates buttons based on what skills a character has when selected.
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void CharacterActionSystem_OnSelectedActionChange(object sender, EventArgs e)//updates button highlight.
    {
        
        UpdateSelectedVisual();
    }

    private void CharacterActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUi actionButtonUi in actionButtonUIList)
        {
            actionButtonUi.UpdatedSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
       Unit selectedUnit = CharacterActionSystem.Instance.GetSelectedUnit();

        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
}
