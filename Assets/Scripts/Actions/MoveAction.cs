using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction {

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private const string IS_WALKING = "IsWalking";
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 targetPosition;

    protected override void Awake() {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update() {

        if (!isActive) return;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float stoppingDistance = .1f;

        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance) {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        OnStartMoving(this, EventArgs.Empty);

        ActionStart(onActionComplete);

    }
    public override List<GridPosition> GetValidActionGridPositionList() {

        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (unitGridPosition == testGridPosition) {
                    // Don't move to the same tile
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    // Tile already occupied
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName() {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
