using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDestroyed;
    public static event EventHandler OnUnitDestroyed;

    [SerializeField] private bool isEnemy;
    private GridPostion gridPostion;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ThrowAction throwAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;
    private PopulationCounter populationCounter;



    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        throwAction = GetComponent<ThrowAction>();
        baseActionArray = GetComponents<BaseAction>();
        populationCounter = FindObjectOfType<PopulationCounter>();
    }

    private void Start()
    {
        gridPostion = LevelGrid.Instance.GetGridPostion(transform.position);
        LevelGrid.Instance.AddUnitGridPostion(gridPostion, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        healthSystem.OnDeath += HealthSystem_OnDeath;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        // Only increase population when a player unit is spawned, not on scene start
        if (!IsEnemy())
        {
            IncreasePopulation();
        }
    }

    private void Update()
    {
        GridPostion newGridPostion = LevelGrid.Instance.GetGridPostion(transform.position);
        if (newGridPostion != gridPostion)
        {
            LevelGrid.Instance.UnitMovedGridPostion(this, gridPostion, newGridPostion);
            gridPostion = newGridPostion;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public ThrowAction GetThrowAction()
    {
        return throwAction;
    }

    public GridPostion GetGridPostion()
    {
        return gridPostion;
    }

    public Vector3 GetWorldPostion()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitGridPostion(gridPostion, this);
        EnemyLootDrop lootDrop = GetComponent<EnemyLootDrop>();
        if (lootDrop != null)
        {
            lootDrop.DropLootAndShowPanel();
        }

        Destroy(gameObject);

        if (!IsEnemy())
        {
            DecreasePopulation();
        }

        OnUnitDestroyed?.Invoke(this, EventArgs.Empty);
    }
   

    private void IncreasePopulation()
    {
        if (populationCounter != null)
        {
            populationCounter.UpdatePopulationText();
        }
    }

    private void DecreasePopulation()
    {
        if (populationCounter != null)
        {
            populationCounter.DecreasePopulation();
        }
        OnAnyUnitDestroyed?.Invoke(this, EventArgs.Empty);
    }


}
