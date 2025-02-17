using System;
using UnityEngine;

public class Unit : MonoBehaviour {
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawn;
    public static event EventHandler OnAnyUnitDeath;

    private GridPosition currentGridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    [SerializeField] private int actionPointsMax = 2;
    [SerializeField] private bool isEnemy;
    private int actionPoints;
    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
        actionPoints = actionPointsMax;
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDeath += HealthSystem_OnDeath;

        OnAnyUnitSpawn?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != currentGridPosition) {
            GridPosition previousGridPosition = currentGridPosition;
            currentGridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, previousGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction {
        foreach (BaseAction baseAction in baseActionArray) {
            if (baseAction is T) {
                return (T)baseAction;
            }
        }

        return null;
    }

    public GridPosition GetGridPosition() {
        return currentGridPosition;
    }

    public Vector3 GetWorldPosition() {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray() {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) {
        if (CanSpendActionPointsToTakeAction(baseAction)) {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        if (actionPoints >= baseAction.GetActionPointsCost()) {
            return true;
        }
        else {
            return false;
        }
    }

    private void SpendActionPoints(int amount) {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints() {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn())) {
            actionPoints = actionPointsMax;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy() {
        return isEnemy;
    }

    public void Damage(float damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(currentGridPosition, this);

        OnAnyUnitDeath?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    public float GetHealthNormalized() {
        return healthSystem.GetHealthNormalized();
    }
}
