using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction {

    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    private enum State {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,

    }

    private int maxSwordDistance = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    void Update() {
        if (!isActive) {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state) {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0f) {
            NextState();
        }
    }

    private void NextState() {
        switch (state) {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = .5f;
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                stateTimer = afterHitStateTime;
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName() {
        return "Slice";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++) {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    // Tile already occupied
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy()) {
                    // both units are on the same team
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance() {
        return maxSwordDistance;
    }
}
